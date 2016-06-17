using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PlayGen.SGA.Contracts;
using PlayGen.SGA.Contracts.Controllers;
using PlayGen.SGA.DataController;
using PlayGen.SGA.ServerAuthentication.Providers;
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

        public AccountController(AccountDbController accountDbController)
        {
            _accountDbController = accountDbController;
        }

        /// <summary>
        /// Register a new account.
        /// Requires the name to be unique.
        /// 
        /// Example Usage: POST api/account/register
        /// </summary>
        /// <param name="newAccount"></param>
        /// <returns></returns>
        [HttpPost]
        public AccountResponse Register([FromBody]AccountRequest newAccount)
        {
            var account = _accountDbController.Create(newAccount.ToModel());
            return account.ToContract();
        }

        /// <summary>
        /// Logs in an account based on the name and password combination.
        /// Returns a JWT used for authorization in any further calls to the API.
        /// 
        /// Example Usage: POST api/account
        /// </summary>
        /// <param name="name"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        [HttpGet]
        public AccountResponse Login(AccountRequest account)
        {
            var accounts = _accountDbController.Get(new string[] {accountDetails.Name});

            if (!accounts.Any()) 
            {
                throw new InvalidLoginDetailsException("Invalid Login Details.");
            }

            var account = accounts.ElementAt(0);
            PasswordValidation validation = new PasswordValidation(accountDetails.Password, account.Password);

            if (!validation.IsValid) throw new InvalidLoginDetailsException("Invalid Login Details.");


            // TODO
            // get authentication to compare passwords
            // if authenticated:
            //  create jwt with claims - id specifically
            //   return the jwt

            throw new NotImplementedException();
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
