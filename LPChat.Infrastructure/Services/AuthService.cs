using LPChat.Core.DTO;
using LPChat.Core.Entities;
using LPChat.Core.Interfaces;
using LPChat.Core.Results;
using LPChat.Infrastructure.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace LPChat.Infrastructure.Services
{
    public class AuthService : IAuthService
    {
        private readonly MongoDbService<Person> _personContext;
        private readonly IConfiguration _config;

        public AuthService(MongoDbService<Person> context, IConfiguration configuration)
        {
            _personContext = context;
            _config = configuration;
        }

        public async Task<OperationResult> Register(UserForRegister userForRegister)
        {
            if (await PersonExists(userForRegister.Username.ToLower()))
            {
                return new OperationResult(false, "User already exists");
            }

            var person = new Person
            {
                Username = userForRegister.Username
            };

            CreatePasswordHash(userForRegister.Password, out byte[] passwordHash, out byte[] passwordSalt);

            person.PasswordHash = passwordHash;
            person.PasswordSalt = passwordSalt;

            await _personContext.Insert(person);

            return new OperationResult(true, "Registration succesful", payload: person.ID);
        }

        public async Task<OperationResult> Login(UserForLogin userForLoginDto)
        {
            var persons = await _personContext.GetAsync(u => u.Username.ToUpper() == userForLoginDto.UserName.ToUpper());
            var person = persons.FirstOrDefault();

            if (person == null)
                return new OperationResult(false, "Wrong user");

            if (!VerifyPasswordHash(userForLoginDto.Password, person.PasswordHash, person.PasswordSalt))
                return new OperationResult(false, "Wrong password");

            var token = GenerateToken(person);
            var result = new OperationResult(true, "Logged in", token);

            return result;
        }

        private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
            }
        }

        private string GenerateToken(Person person)
        {
            var claims = GetClaimsIdentity(person);

            //security key from appsettings.json
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config.GetSection("AppSettings:Token").Value));

            //signing creds
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = claims,
                Expires = DateTime.Now.AddDays(1),
                SigningCredentials = creds
            };

            var tokenHandler = new JwtSecurityTokenHandler();

            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }

        private bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512(passwordSalt))
            {
                var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));

                for (int i = 0; i < computedHash.Length; i++)
                {
                    if (computedHash[i] != passwordHash[i])
                        return false;
                }
            }
            return true;
        }

        private async Task<bool> PersonExists(string username)
        {
            var persons = await _personContext.GetAsync(u => u.Username.ToUpper() == username.ToUpper());

            if (persons.Count > 0)
                return true;

            return false;
        }

        private ClaimsIdentity GetClaimsIdentity(Person person)
        {
            //array of claims (to PAYLOAD:DATA)
            var claims = new[]
            {
                    new Claim(ClaimTypes.NameIdentifier, person.ID.ToString()),
                    new Claim(ClaimTypes.Name, person.Username),
                    new Claim(ClaimTypes.GivenName, person.FirstName ?? string.Empty),
                    new Claim(ClaimTypes.Surname, person.LastName ?? string.Empty)
            };

            var claimsIdentity = new ClaimsIdentity(claims);

            return claimsIdentity;
        }

    }
}
