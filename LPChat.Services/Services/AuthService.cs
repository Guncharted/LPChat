using LPChat.Domain;
using LPChat.Domain.Results;
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
using LPChat.Services.Mapping;
using LPChat.Services.Interfaces;
using Microsoft.Extensions.Configuration;
using LPChat.Common;

namespace LPChat.Services.Services
{
    public class AuthService : IAuthService
    {
        private readonly IConfiguration _config;
        private readonly IRepositoryManager _repoManager;
        private readonly IAuthorizationContext _authorizationContext;

        public AuthService(IRepositoryManager repoManager, IAuthorizationContext authorizationContext, IConfiguration configuration)
        {
            _config = configuration;
            _repoManager = repoManager;
            _authorizationContext = authorizationContext;
        }

        public async Task<OperationResult> RegisterAsync(UserSecurityModel userForRegister)
        {
            Guard.NotNull(userForRegister, nameof(userForRegister));

            if (await PersonExists(userForRegister.Email))
            {
                throw new DuplicateException("User already exists!");
            }

            CreatePasswordHash(userForRegister.Password, out byte[] passwordHash, out byte[] passwordSalt);
            var user = DataMapper.Map<UserSecurityModel, User>(userForRegister);

            user.Email = userForRegister.Email;
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
            var user = (await repository.GetAsync(u => string.Compare(u.Email, userForLogin.Email, true) == 0))?.FirstOrDefault();

            _ = user ?? throw new PersonNotFoundException("User not found.");

            if (!VerifyPasswordHash(userForLogin.Password, user.PasswordHash, user.PasswordSalt))
                throw new PasswordMismatchException("Wrong password!");

            var token = GenerateToken(user);
            return new OperationResult(true, "Logged in", token);
        }

        public async Task<OperationResult> ChangePasswordAsync(UserSecurityModel userToChange)
        {
            if (userToChange.Password != userToChange.ConfirmPassword)
                throw new PasswordMismatchException("New password is not matching confirmation value");

            var repository = _repoManager.GetRepository<User>();
            var user = (await repository.GetAsync(u => u.ID == userToChange.ID))?.FirstOrDefault();

            Guard.NotNull(user, nameof(user));

            if (_authorizationContext.GetContext<UserModel>().ID != user.ID
                && !_authorizationContext.GetPolicy(UserPolicies.UserAdministration))
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

        private string GenerateToken(User user)
        {
            var claims = GetClaimsIdentity(user);

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

        private async Task<bool> PersonExists(string email)
        {
            var repository = _repoManager.GetRepository<User>();
            var persons = (await repository.GetAsync(u => string.Compare(u.Email, email, true) == 0))?.ToList();

            return persons?.Count > 0;
        }

        private ClaimsIdentity GetClaimsIdentity(User person)
        {
            //array of claims (to PAYLOAD:DATA)
            var claims = new[] { new Claim(ClaimTypes.NameIdentifier, person.ID.ToString()) };

            return new ClaimsIdentity(claims);
        }

    }
}
