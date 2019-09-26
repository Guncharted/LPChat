using LPChat.Domain.DTO;
using LPChat.Domain.Entities;
using LPChat.Domain.Exceptions;
using LPChat.Domain.Interfaces;
using LPChat.Domain.Results;
using LPChat.Infrastructure.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace LPChat.Infrastructure.Services
{
    public class AuthService : IAuthService
    {
        private readonly IConfiguration _config;
		private readonly IRepositoryManager _repoManager;

        public AuthService(IRepositoryManager repoManager, IConfiguration configuration)
        {
            _config = configuration;
			_repoManager = repoManager;
        }

        public async Task<OperationResult> Register(UserForRegister userForRegister)
        {
            if (await PersonExists(userForRegister.Username.ToLower()))
            {
				throw new DuplicateException("User already exists!");
            }

            var person = new Person
            {
                Username = userForRegister.Username,
                FirstName = userForRegister.FirstName,
                LastName = userForRegister.LastName
            };

            CreatePasswordHash(userForRegister.Password, out byte[] passwordHash, out byte[] passwordSalt);

            person.PasswordHash = passwordHash;
            person.PasswordSalt = passwordSalt;
 
			var repository = _repoManager.GetRepository<Person>();
			await repository.CreateAsync(person);

            return new OperationResult(true, "Registration succesful", payload: person.ID);
        }

        public async Task<OperationResult> Login(UserForLogin userForLoginDto)
        {
			var repository = _repoManager.GetRepository<Person>();
            var persons = await repository.GetAsync(u => u.Username.ToUpper() == userForLoginDto.UserName.ToUpper());
            var person = persons.FirstOrDefault();

			if (person == null)
				throw new UserNotFoundException("User not found.");

			if (!VerifyPasswordHash(userForLoginDto.Password, person.PasswordHash, person.PasswordSalt))
				throw new PasswordMismatchException("Wrong password!");

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
			var repository = _repoManager.GetRepository<Person>();
			var persons = (await repository.GetAsync(u => u.Username.ToUpper() == username.ToUpper())).ToList();

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
