using Newtonsoft.Json;

namespace PlayGen.SUGAR.Server.Authentication.Helpers
{
	class JWTSerializationAdapter : JsonSerializer
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
