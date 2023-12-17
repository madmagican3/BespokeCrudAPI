using System.Security.Cryptography.X509Certificates;
using BasicCrudAPI.Models.AdminModels;
using BCrypt.Net;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.Azure.Cosmos;
using PasswordGenerator;
using User = BasicCrudAPI.Models.AdminModels.User;

namespace BasicCrudAPI.Classes
{
    public class UserHelper
    {
        public DbHelper _dbHelper { get; set; }
        private Password passwordGeneratorActualInstance { get; set; }

        public Password _passwordGenerator
        {
            get
            {
                passwordGeneratorActualInstance ??=  new Password()
                {
                    Settings = new PasswordSettings(true, true, true, false, 256, 3, false)
                };
                return passwordGeneratorActualInstance;
            }
        }

        public UserHelper(IConfiguration config, DbHelper helper)
        {
            _dbHelper = helper;
        }
        public bool LoginUser(string username, string password, out Models.AdminModels.User user  )
        {
            user = null;
            var potentialUser = _dbHelper.GetRecords<Models.AdminModels.User>(x => x.UserName == username);
            if (potentialUser == null|| !potentialUser.Any())
            {
                return false;
            }

            if (!BCrypt.Net.BCrypt.EnhancedVerify(password, potentialUser.FirstOrDefault().PasswordHash, HashType.SHA512))
            {
                return false;
            }

            user = potentialUser.FirstOrDefault();
            return true;
        }

        public Token GetUserToken(Models.AdminModels.User user)
        {
            return GenerateToken(user.id);
        }

        public Token RefreshUserToken(string refreshToken)
        {
            var cachedToken = _dbHelper.GetRecords<Token>(x => x.RefreshToken == refreshToken);
            if (cachedToken == null ||!cachedToken.Any())
            {
                return null;
            }

            string userId = null;
            foreach (var t in cachedToken)
            {
                userId ??= t.UserId;
                t.Active = false;
                _dbHelper.UpsertRecord(t);
            }
            return GenerateToken(userId);
        }

        private Token GenerateToken(string userId)
        {
            var token = new Token()
            {
                Active = true,
                AuthorizationToken = _passwordGenerator.Next(),
                RefreshToken = _passwordGenerator.Next(),
                UserId = userId
            };
            var existingTokens = _dbHelper.GetRecords<Token>(x => x.UserId == userId);
            foreach (var t in existingTokens)
            {
                t.Active = false;
                _dbHelper.UpsertRecord(t);
            }
            return _dbHelper.UpsertRecord(token);
        }

        public void RegisterUser(string username, string email, string password)
        {
            var user = new Models.AdminModels.User()
            {
                Active = true,
                IsAdmin = false,
                IsApproved = false,
                UserEmail = email,
                PasswordHash = BCrypt.Net.BCrypt.EnhancedHashPassword(password, HashType.SHA512),
                UserName = username
            };
            _dbHelper.UpsertRecord(user);
        }

        public bool UsernameExists(string username)=>_dbHelper.GetRecords<Models.AdminModels.User>(x => x.UserName == username).Any();
        public bool EmailExists(string email) => _dbHelper.GetRecords<Models.AdminModels.User>(x => x.UserEmail == email).Any();

    }
}
