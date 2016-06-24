using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using PlayGen.SUGAR.Contracts;
using PlayGen.SUGAR.Contracts.Controllers;
using PlayGen.SUGAR.Data.Model;
using PlayGen.SUGAR.Data.EntityFramework;
using PlayGen.SUGAR.ServerAuthentication;
using PlayGen.SUGAR.WebAPI.Exceptions;
using PlayGen.SUGAR.WebAPI.ExtensionMethods;

namespace PlayGen.SUGAR.WebAPI.Controllers
{
	/// <summary>
	/// Web Controller that facilitates account specific operations.
	/// </summary>
	[Route("api/[controller]")]
	public class AccountController : Controller, IAccountController
	{
		private readonly Data.EntityFramework.Controllers.AccountController _accountDbController;
		private readonly Data.EntityFramework.Controllers.UserController _userDbController;
		private readonly PasswordEncryption _passwordEncryption;
		private readonly JsonWebTokenUtility _jsonWebTokenUtility;

		public AccountController(Data.EntityFramework.Controllers.AccountController accountDbController,
			Data.EntityFramework.Controllers.UserController userDbController,
			PasswordEncryption passwordEncryption,
			JsonWebTokenUtility jsonWebTokenUtility)
		{
			_accountDbController = accountDbController;
			_passwordEncryption = passwordEncryption;
			_userDbController = userDbController;
			_jsonWebTokenUtility = jsonWebTokenUtility;
		}

		/// <summary>
		/// Logs in an account based on the name and password combination.
		/// Returns a JsonWebToken used for authorization in any further calls to the API.
		/// 
		/// Example Usage: PUT api/account
		/// </summary>
		/// <param name="accountRequest"><see cref="AccountRequest"/> object that contains the account details provided.</param>
		/// <returns>A <see cref="AccountResponse"/> containing the Account details.</returns>
		[HttpPut]
		public AccountResponse Login([FromBody]AccountRequest accountRequest)
		{
			var accounts = _accountDbController.Get(new string[] { accountRequest.Name });

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
		/// Register a new account and creates an associated user.
		/// Requires the name to be unique.
		/// Returns a JsonWebToken used for authorization in any further calls to the API.
		/// 
		/// Example Usage: POST api/account
		/// </summary>
		/// <param name="accountRequest"><see cref="AccountRequest"/> object that contains the details of the new Account.</param>
		/// <returns>A <see cref="AccountResponse"/> containing the new Account details.</returns>
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
			_userDbController.Create(user);

			var account = CreateAccount(accountRequest, user);

			var response = account.ToContract();
			if (accountRequest.AutoLogin)
			{
				response.Token = CreateToken(account);
			}
			return response;
		}

		/// <summary>
		/// Register a new account for an existing user.
		/// Requires the name to be unique.
		/// Returns a JsonWebToken used for authorization in any further calls to the API.
		/// 
		/// Example Usage: POST api/account/userId?userId=1
		/// </summary>
		// <param name="userId">ID of the existing User.</param>
		/// <param name="newAccount"><see cref="AccountRequest"/> object that contains the details of the new Account.</param>
		/// <returns>A <see cref="AccountResponse"/> containing the new Account details.</returns>
		[HttpPost("userId")]
		public AccountResponse Register(int userId, [FromBody] AccountRequest accountRequest)
		{
			var users = _userDbController.Get(new[] { userId });

			if (string.IsNullOrWhiteSpace(accountRequest.Name) 
				|| string.IsNullOrWhiteSpace(accountRequest.Password)
				|| !users.Any())
			{
				throw new InvalidAccountDetailsException("Name and Password cannot be empty.");
			}
			
			var user = users.ElementAt(0);

			var account = CreateAccount(accountRequest, user);

			var response = account.ToContract();
			if (accountRequest.AutoLogin)
			{
				response.Token = CreateToken(account);
			}
			return response;
		}

		/// <summary>
		/// Delete Accounts with the IDs provided.
		/// 
		/// Example Usage: DELETE api/account?id=1&amp;id=2
		/// </summary>
		/// <param name="id">Array of Account IDs.</param>
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
