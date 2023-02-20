using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace DSoft.Portable.WebClient.Encryption.Helpers
{
	/// <summary>
	/// Class ClientVersionHelper.
	/// </summary>
	public class ClientVersionHelper
    {
		/// <summary>
		/// The invalid version number
		/// </summary>
		public const string InvalidVersionNumber = "The version number is invalid";
		/// <summary>
		/// The invalid client version number
		/// </summary>
		public const string InvalidClientVersionNumber = "The client version number is invalid";

		/// <summary>
		/// Versions the check.
		/// </summary>
		/// <param name="versionNo">The version no.</param>
		/// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
		/// <exception cref="System.Exception"></exception>
		public static bool VersionCheck(string versionNo)
        {
            if (string.IsNullOrWhiteSpace(versionNo))
                throw new Exception(InvalidVersionNumber);

            Version aVersion;

            if (!Version.TryParse(versionNo, out aVersion))
                throw new Exception(InvalidVersionNumber);

            return VersionCheck(aVersion);

        }

		/// <summary>
		/// Versions the check.
		/// </summary>
		/// <param name="version">The version.</param>
		/// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
		public static bool VersionCheck(Version version)
        {
            var asm = Assembly.GetAssembly(typeof(ClientVersionHelper));

            var curVersion = asm.GetName().Version;

            if (curVersion.Major == version.Major && curVersion.Minor == version.Minor)
                return true;

            return false;
        }
    }
}
