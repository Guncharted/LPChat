using LPChat.Domain;
using LPChat.Infrastructure.Interfaces;
using LPChat.Domain.Results;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using LPChat.Common.Exceptions;
using LPChat.Data.MongoDb.Entities;
using LPChat.Common.DbContracts;
using LPChat.Common.Models;
using LPChat.Infrastructure.Mapping;

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

        public async Task<OperationResult> RegisterAsync(UserSecurityModel userForRegister)
        {
            Guard.NotNull(userForRegister, nameof(userForRegister));

            if (await PersonExists(userForRegister.Username.ToLower()))
            {
                throw new DuplicateException("User already exists!");
            }


            CreatePasswordHash(userForRegister.Password, out byte[] passwordHash, out byte[] passwordSalt);
            var user = DataMapper.Map<UserSecurityModel, User>(userForRegister);

            user.PasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt;

            var repository = _repoManager.GetRepository<User>();
            await repository.CreateAsync(user);

            return new OperationResult(true, "Registration successful", payload: user.ID);
        }

        public async Task<OperationResult> LoginAsync(UserSecurityModel userForLogin)
        {
            Guard.NotNull(userForLogin, nameof(userForLogin));

            var repository = _repoManager.GetRepository<User>();
            var persons = await repository.GetAsync(u => string.Equals(u.Username, userForLogin.Username, StringComparison.OrdinalIgnoreCase));
            var person = persons.FirstOrDefault();

            if (person == null)
                throw new PersonNotFoundException("User not found.");

            if (!VerifyPasswordHash(userForLogin.Password, person.PasswordHash, person.PasswordSalt))
                throw new PasswordMismatchException("Wrong password!");

            var token = GenerateToken(person);
            var result = new OperationResult(true, "Logged in", token);

            return result;
        }

        // TODO. remove overloads after policies will be introduced
        public async Task<OperationResult> ChangePasswordAsync(UserSecurityModel userToChange) => await ChangePasswordAsync(userToChange, null);

        public async Task<OperationResult> ChangePasswordAsync(UserSecurityModel userToChange, Guid? requestorId)
        {
            var validateRequestor = requestorId != null;
            return await ChangePasswordAsync(userToChange, validateRequestor, requestorId);
        }

        private async Task<OperationResult> ChangePasswordAsync(UserSecurityModel userToChange, bool validateRequestor, Guid? requestorId)
        {
            if (userToChange.Password != userToChange.ConfirmPassword)
                throw new PasswordMismatchException("New password is not matching confirmation value");

            var repository = _repoManager.GetRepository<User>();
            var user = (await repository.GetAsync(u => u.ID == userToChange.ID)).FirstOrDefault();

            Guard.NotNull(user, nameof(user));

            if (validateRequestor && requestorId != user.ID)
                throw new ChatAppException("Unauthorized attempt to change password");

            if (!VerifyPasswordHash(userToChange.OldPassword, user.PasswordHash, user.PasswordSalt))
                throw new PasswordMismatchException("Wrong old password!");

            CreatePasswordHash(userToChange.Password, out byte[] passwordHash, out byte[] passwordSalt);
            user.PasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt;

            await repository.UpdateAsync(user);

            return new OperationResult(true, "Password has been changed.");
        }

        private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
            }
        }

        private string GenerateToken(User person)
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
            var repository = _repoManager.GetRepository<User>();
            var persons = (await repository.GetAsync(u => string.Compare(u.Username, username, StringComparison.CurrentCultureIgnoreCase) == 0)).ToList();

            return persons.Count > 0;
        }

        private ClaimsIdentity GetClaimsIdentity(User person)
        {
            //array of claims (to PAYLOAD:DATA)
            var claims = new[]
            {
                    new Claim(ClaimTypes.NameIdentifier, person.ID.ToString()),

                    //below to be removed
                    new Claim(ClaimTypes.Name, person.Username),
                    new Claim(ClaimTypes.GivenName, person.FirstName ?? string.Empty),
                    new Claim(ClaimTypes.Surname, person.LastName ?? string.Empty)
            };

            var claimsIdentity = new ClaimsIdentity(claims);

            return claimsIdentity;
        }

    }
}
