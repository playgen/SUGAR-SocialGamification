using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using PlayGen.SGA.DataAccess;
using PlayGen.SGA.DataController.Exceptions;
using PlayGen.SGA.DataController.Interfaces;
using PlayGen.SGA.DataModel;

namespace PlayGen.SGA.DataController
{
    public class GroupSaveDataDbController : DbController, ISaveDataDbController
    {
        public GroupSaveDataDbController(string nameOrConnectionString) : base(nameOrConnectionString)
        {
        }

        public IEnumerable<GroupData> Get(int gameId, int groupId, string[] keys)
        {
            using (var context = new SGAContext(NameOrConnectionString))
            {
                SetLog(context);

                var data = context.GroupDatas.Where(d => d.GroupId == groupId && d.GameId == gameId && keys.Contains(d.Key)).ToList();

                return data;
            }
        }

        // TODO expose via WebAPI
        public float SumFloats(int gameId, int groupId, string key)
        {
            using (var context = new SGAContext(NameOrConnectionString))
            {
                SetLog(context);

                var datas = context.GroupDatas
                            .Where(s => s.GameId == gameId
                                        && s.GroupId == groupId
                                        && s.Key == key
                                        && s.DataType == DataType.Float)
                            .ToList();

                float sum = datas.Sum(s => float.Parse(s.Value));
                return sum;
            }
        }

        // TODO expose via WebAPI
        public long SumLongs(int gameId, int groupId, string key)
        {
            using (var context = new SGAContext(NameOrConnectionString))
            {
                SetLog(context);

                var datas = context.GroupDatas
                    .Where(s => s.GameId == gameId
                            && s.GroupId == groupId
                            && s.Key == key
                            && s.DataType == DataType.Long).ToList();

                long sum = datas.Sum(s => long.Parse(s.Value));
                return sum;
            }
        }

        // TODO expose via WebAPI
        public bool TryGetLatestBool(int gameId, int groupId, string key, out bool latestBool)
        {
            using (var context = new SGAContext(NameOrConnectionString))
            {
                SetLog(context);

                var data = context.GroupDatas
                    .Where(s => s.GameId == gameId
                            && s.GroupId == groupId
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
        public bool TryGetLatestString(int gameId, int groupId, string key, out string latestString)
        {
            using (var context = new SGAContext(NameOrConnectionString))
            {
                SetLog(context);

                var data = context.GroupDatas
                    .Where(s => s.GameId == gameId
                            && s.GroupId == groupId
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

        public GroupData Create(GroupData data)
        {
            using (var context = new SGAContext(NameOrConnectionString))
            {
                SetLog(context);

                var actorExists = context.Groups.Any(u => u.Id == data.GroupId);
                var gameExists = context.Games.Any(g => g.Id == data.GameId);

                if (!actorExists)
                {
                    throw new MissingRecordException(string.Format("The provided group does not exist."));
                }

                if (!gameExists)
                {
                    throw new MissingRecordException(string.Format("The provided game does not exist."));
                }

                context.GroupDatas.Add(data);
                SaveChanges(context);
                return data;
            }
        }
    }
}