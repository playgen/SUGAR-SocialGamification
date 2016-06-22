using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using PlayGen.SGA.DataController;
using PlayGen.SGA.Contracts.Controllers;
using PlayGen.SGA.WebAPI.ExtensionMethods;
using PlayGen.SGA.Contracts;
using PlayGen.SGA.WebAPI.Exceptions;

namespace PlayGen.SGA.WebAPI.Controllers
{
    /// <summary>
    /// Web Controller that facilitates GroupData specific operations.
    /// </summary>
    [Route("api/[controller]")]
    public class GroupSaveDataController : Controller
    {
        private readonly GroupSaveDataDbController _groupSaveDataDbController;

        public GroupSaveDataController(GroupSaveDataDbController groupSaveDataDbController)
        {
            _groupSaveDataDbController = groupSaveDataDbController;
        }

        /// <summary>
        /// GetByGame a list of all GroupData that match the <param name="actorId"/>, <param name="gameId"/> and <param name="key"/> provided.
        /// 
        /// Example Usage: GET api/groupsavedata?actorId=1amp;gameId=1amp;key=key1amp;key=key2
        /// </summary>
        /// <param name="actorId">ID of a Group.</param>
        /// <param name="gameId">ID of a Game.</param>
        /// <param name="key">Array of Key names.</param>
        /// <returns>A list of <see cref="SaveDataResponse"/> which match the search criteria.</returns>
        [HttpGet]
        public IEnumerable<SaveDataResponse> Get(int actorId, int gameId, string[] key)
        {
            var data = _groupSaveDataDbController.Get(actorId, gameId, key);
            var dataContract = data.ToContract();
            return dataContract;
        }

        /// <summary>
        /// Create a new GroupData record.
        /// 
        /// Example Usage: POST api/groupsavedata
        /// </summary>
        /// <param name="newData"><see cref="SaveDataRequest"/> object that holds the details of the new GroupData.</param>
        /// <returns>A <see cref="SaveDataResponse"/> containing the new GroupData details.</returns>
        [HttpPost]
        public SaveDataResponse Add([FromBody]SaveDataRequest newData)
        {
            if (newData == null)
            {
                throw new NullObjectException("Invalid object passed");
            }
            var data = newData.ToGroupModel();
            _groupSaveDataDbController.Create(data);
            var dataContract = data.ToContract();
            return dataContract;
        }
    }
}
