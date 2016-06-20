using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using PlayGen.SGA.Contracts;
using PlayGen.SGA.Contracts.Controllers;
using PlayGen.SGA.DataController;
using PlayGen.SGA.DataModel;
using PlayGen.SGA.ServerAuthentication;
using PlayGen.SGA.WebAPI.Exceptions;
using PlayGen.SGA.WebAPI.ExtensionMethods;

namespace PlayGen.SGA.WebAPI.Controllers
{
    /// <summary>
    /// Web Controller that facilitates account specific operations.
    /// </summary>
    [Route("api/[controller]")]
    public class AccountController : Controller, IAccountController
    {
        private readonly AccountDbController _accountDbController;
        private readonly UserDbController _userDbController;
        private readonly PasswordEncryption _passwordEncryption;
        private readonly JsonWebTokenUtility _jsonWebTokenUtility;

        public AccountController(AccountDbController accountDbController,
            UserDbController userDbController,
            PasswordEncryption passwordEncryption,
            JsonWebTokenUtility jsonWebTokenUtility)
        {
            _accountDbController = accountDbController;
            _passwordEncryption = passwordEncryption;
            _userDbController = userDbController;
            _jsonWebTokenUtility = jsonWebTokenUtility;
        }

        /// <summary>
        /// Register a new account and creates an associated user.
        /// Requires the name to be unique.
        /// 
        /// Example Usage: POST api/account/register
        /// <param name="accountRequest"></param>
        /// <returns></returns>
        [HttpPost]
        public AccountResponse Register([FromBody] AccountRequest accountRequest)
        {
            if (string.IsNullOrWhiteSpace(accountRequest.Name) || string.IsNullOrWhiteSpace(accountRequest.Password))
            {
                throw new InvalidAccountDetailsException("Name and Password cannot be empty.");
            }

            User user = new User
            {
                Name = accountRequest.Name,
            };
            user = _userDbController.Create(user);

            var account = CreateAccount(accountRequest, user);

            var response = account.ToContract();
            response.Token = CreateToken(account);
            return response;
        }

        /// <summary>
        /// Register a new account for an existing user.
        /// Requires the name to be unique.
        /// 
        /// Example Usage: POST api/account/register
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="accountRequest"></param>
        /// <returns></returns>
        [HttpPost("userId")]
        public AccountResponse Register(int userId, [FromBody] AccountRequest accountRequest)
        {
            if (string.IsNullOrWhiteSpace(accountRequest.Name) || string.IsNullOrWhiteSpace(accountRequest.Password))
            {
                throw new InvalidAccountDetailsException("Name and Password cannot be empty.");
            }

            var users = _userDbController.Get(new[] {userId});

            if (!users.Any())
            {
                throw new InvalidAccountDetailsException("Name and Password cannot be empty.");
            }

            var user = users.ElementAt(0);

            var account = CreateAccount(accountRequest, user);

            var response = account.ToContract();
            response.Token = CreateToken(account);
            return response;
        }

        /// <summary>
        /// Logs in an account based on the name and password combination.
        /// Returns a JWT used for authorization in any further calls to the API.
        /// 
        /// Example Usage: POST api/account
        /// </summary>
        /// <param name="accountRequest"></param>
        /// <returns></returns>
        [HttpGet]
        public AccountResponse Login(AccountRequest accountRequest)
        {
            var accounts = _accountDbController.Get(new string[] {accountRequest.Name});

            if (!accounts.Any())
            {
                throw new InvalidAccountDetailsException("Invalid Login Details.");
            }

            var account = accounts.ElementAt(0);

            if (account.PasswordHash != _passwordEncryption.Encrypt(accountRequest.Password, account.Salt))
            {
                throw new InvalidAccountDetailsException("Invalid Login Details.");
            }

            string token = _jsonWebTokenUtility.CreateToken(new Dictionary<string, object>
            {
                {"user", account.UserId},
            });

            var response = account.ToContract();
            response.Token = CreateToken(account);
            return response;
        }

        /// <summary>
        /// Delete accounts based on their IDs.
        /// 
        /// Example Usage: DELETE api/account?id=1&id=2
        /// </summary>
        /// <param name="id"></param>
        [HttpDelete]
        public void Delete(int[] id)
        {
            _accountDbController.Delete(id);
        }

        #region Helpers

        private Account CreateAccount(AccountRequest accountRequest, User user)
        {
            var newAccount = accountRequest.ToModel();
            newAccount.Salt = _passwordEncryption.CreateSalt();
            newAccount.PasswordHash = _passwordEncryption.Encrypt(accountRequest.Password, newAccount.Salt);
            newAccount.UserId = user.Id;
            newAccount.User = user;

            return _accountDbController.Create(newAccount);
        }

        public string CreateToken(Account account)
        {
            return _jsonWebTokenUtility.CreateToken(new Dictionary<string, object>
            {
                { "userid", account.UserId}
            });
        }
        #endregion
    }
}
