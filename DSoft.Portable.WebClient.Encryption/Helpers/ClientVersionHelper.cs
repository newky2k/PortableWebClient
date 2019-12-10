using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace DSoft.Portable.WebClient.Encryption.Helpers
{
    public class ClientVersionHelper
    {
        public const string InvalidVersionNumber = "The version number is invalid";
        public const string InvalidClientVersionNumber = "The client version number is invalid";

        public static bool VersionCheck(string versionNo)
        {
            if (string.IsNullOrWhiteSpace(versionNo))
                throw new Exception(InvalidVersionNumber);

            Version aVersion;

            if (!Version.TryParse(versionNo, out aVersion))
                throw new Exception(InvalidVersionNumber);

            return VersionCheck(aVersion);

        }

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
