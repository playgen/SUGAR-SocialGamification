using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.SqlServer;
using System.Linq;
using System.Threading.Tasks;
using PlayGen.SGA.DataAccess;
using PlayGen.SGA.DataController.Exceptions;
using PlayGen.SGA.DataController.Interfaces;
using PlayGen.SGA.DataModel;

namespace PlayGen.SGA.DataController
{
    public class UserSaveDataDbController : DbController, ISaveDataDbController
    {
        public UserSaveDataDbController(string nameOrConnectionString) : base(nameOrConnectionString)
        {
        }

        public IEnumerable<UserData> Get(int gameId, int userId, string[] keys)
        {
            using (var context = new SGAContext(NameOrConnectionString))
            {
                SetLog(context);

                var data = context.UserDatas.Where(d => d.UserId == userId && d.GameId == gameId && keys.Contains(d.Key)).ToList();

                return data;
            }
        }

        // TODO expose via WebAPI
        public float SumFloats(int gameId, int userId, string key)
        {
            using (var context = new SGAContext(NameOrConnectionString))
            {
                SetLog(context);

                var datas = context.UserDatas
                            .Where(s => s.GameId == gameId
                                        && s.UserId == userId
                                        && s.Key == key
                                        && s.DataType == DataType.Float)
                            .ToList();

                float sum = datas.Sum(s => float.Parse(s.Value));
                return sum;
            }
        }

        // TODO expose via WebAPI
        public long SumLongs(int gameId, int userId, string key)
        {
            using (var context = new SGAContext(NameOrConnectionString))
            {
                SetLog(context);

                var datas = context.UserDatas
                    .Where(s => s.GameId == gameId
                            && s.UserId == userId
                            && s.Key == key
                            && s.DataType == DataType.Long).ToList();

                long sum = datas.Sum(s => long.Parse(s.Value));
                return sum;
            }
        }

        // TODO expose via WebAPI
        public bool TryGetLatestBool(int gameId, int userId, string key, out bool latestBool)
        {
            using (var context = new SGAContext(NameOrConnectionString))
            {
                SetLog(context);

                var data = context.UserDatas
                    .Where(s => s.GameId == gameId
                            && s.UserId == userId
                            && s.Key == key
                            && s.DataType == DataType.Boolean)
                    .OrderByDescending(s => s.DateModified)
                    .FirstOrDefault();

                if (data == null)
                {
                    latestBool = default(bool);
                    return false;
                }

                latestBool = bool.Parse(data.Value);
                return true;
            }
        }

        // TODO expose via WebAPI
        public bool TryGetLatestString(int gameId, int userId, string key, out string latestString)
        {
            using (var context = new SGAContext(NameOrConnectionString))
            {
                SetLog(context);

                var data = context.UserDatas
                    .Where(s => s.GameId == gameId
                            && s.UserId == userId
                            && s.Key == key
                            && s.DataType == DataType.String)
                    .OrderByDescending(s => s.DateModified)
                    .FirstOrDefault();

                if (data == null)
                {
                    latestString = default(string);
                    return false;
                }

                latestString = data.Value;
                return true;
            }
        }

        public UserData Create(UserData data)
        {
            using (var context = new SGAContext(NameOrConnectionString))
            {
                SetLog(context);

                var actor = context.Users.SingleOrDefault(u => u.Id == data.UserId);
                var game = context.Games.SingleOrDefault(g => g.Id == data.GameId);

                if (actor == null)
                {
                    throw new MissingRecordException(string.Format("The provided user does not exist."));
                }

                if (game == null)
                {
                    throw new MissingRecordException(string.Format("The provided game does not exist."));
                }

                if (data.User == null)
                {
                    data.User = actor;
                }
                else if (context.Entry(data.User).State == EntityState.Detached)
                {
                    context.Users.Attach(data.User);
                }

                if (data.Game == null)
                {
                    data.Game = game;
                }               
                else if (context.Entry(data.Game).State == EntityState.Detached)
                {
                    context.Games.Attach(data.Game);
                }

                context.UserDatas.Add(data);
                SaveChanges(context);
                return data;
            }
        }
    }
}
