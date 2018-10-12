namespace PlayGen.SUGAR.Client
{
	public interface ISavedPrefsHandler
	{
		string Prefix { get; }

		T Get<T>(string key);
		void Save<T>(string key, T value);
		void Delete(string key);
	}
}
