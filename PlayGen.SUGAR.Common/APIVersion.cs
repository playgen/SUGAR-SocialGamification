using System.Linq;

namespace PlayGen.SUGAR.Common
{
	/// <summary>
	/// This class is used to see which version of the API exists in consuming 
	/// projects to facillitate checking compatibility.
	/// 
	/// Major versions should increment when API Breaking changes are added.
	/// 
	/// Minor version should increment for Fixes and Additions that won't cause existing clients with the same Major
	/// version to break.
	/// 
	/// Build version should increment for every build.
	/// </summary>
    public static class APIVersion
	{
		public const string Key = "APIVersion";

		public const int Major = 1;

		public const int Minor = 3;

		public const int Build = 3;

		public static string Version => $"{Major}.{Minor}.{Build}";

		public static bool IsCompatible(int checkMajor)
		{
			return Major == checkMajor;
		}

		public static bool IsCompatible(string checkVersion)
		{
			return int.TryParse(checkVersion.Split('.').First(), out var checkVersionMajor) && IsCompatible(checkVersionMajor);
		}
	}
}
