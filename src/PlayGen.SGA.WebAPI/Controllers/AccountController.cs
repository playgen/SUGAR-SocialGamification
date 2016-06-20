using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using PlayGen.SGA.Contracts;
using PlayGen.SGA.Contracts.Controllers;
using PlayGen.SGA.DataController;
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
        private readonly PasswordEncryption _passwordEncryption;
        private readonly JsonWebTokenUtility _jsonWebTokenUtility;

        public AccountController(AccountDbController accountDbController, PasswordEncryption passwordEncryption, JsonWebTokenUtility jsonWebTokenUtility)
        {
            _accountDbController = accountDbController;
            _passwordEncryption = passwordEncryption;
            _jsonWebTokenUtility = jsonWebTokenUtility;
        }

        /// <summary>
        /// Register a new account.
        /// Requires the name to be unique.
        /// 
        /// Example Usage: POST api/account/register
        /// <param name="accountRequest"></param>
        /// <returns>AccountResponse</returns>
        [HttpPost]
        public AccountResponse Register([FromBody]AccountRequest accountRequest)
        {
            if(string.IsNullOrWhiteSpace(accountRequest.Name) || string.IsNullOrWhiteSpace(accountRequest.Password))
            {
                throw new InvalidAccountDetailsException("Name and Password cannot be empty.");
            }

            var newAccount = accountRequest.ToModel();
            newAccount.Salt = _passwordEncryption.CreateSalt();
            newAccount.PasswordHash = _passwordEncryption.Encrypt(accountRequest.Password, newAccount.Salt);
            
            var account = _accountDbController.Create(newAccount);
            return account.ToContract();
        }

        /// <summary>
        /// Logs in an account based on the name and password combination.
        /// Returns a JWT used for authorization in any further calls to the API.
        /// 
        /// Example Usage: POST api/account
        /// </summary>
        /// <param name="accountRequest"></param>
        /// <returns>AccountResponse</returns>
        [HttpGet]
        public AccountResponse Login(AccountRequest accountRequest)
        {
            var accounts = _accountDbController.Get(new string[] { accountRequest.Name});

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
                {"user", account.UserId },
            });

            var response = new AccountResponse();
            response.User = account.User.ToContract();
            response.Token = token;

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
    }
}
