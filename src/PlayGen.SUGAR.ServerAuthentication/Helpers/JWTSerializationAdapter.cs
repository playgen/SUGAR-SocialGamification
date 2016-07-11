using JWT;
using Newtonsoft.Json;

namespace PlayGen.SUGAR.ServerAuthentication.Helpers
{
	class JWTSerializationAdapter : IJsonSerializer
	{
		public T Deserialize<T>(string json)
		{
			return JsonConvert.DeserializeObject<T>(json);
		}

		public string Serialize(object obj)
		{
			return JsonConvert.SerializeObject(obj);
		}
	}
}
