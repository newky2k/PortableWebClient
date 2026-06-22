using System;
using System.Reflection;

namespace DSoft.Portable.WebClient.Encryption.Helpers;

/// <summary>
/// Helper for checking that a caller-supplied client version is compatible with this assembly,
/// comparing on major and minor version only.
/// </summary>
public class ClientVersionHelper
{
    /// <summary>
    /// Error message used when a version string is missing or cannot be parsed.
    /// </summary>
    public const string InvalidVersionNumber = "The version number is invalid";
    /// <summary>
    /// Error message used when a client version number is rejected.
    /// </summary>
    public const string InvalidClientVersionNumber = "The client version number is invalid";

    /// <summary>
    /// Parses a version string and checks it against this assembly's version.
    /// </summary>
    /// <param name="versionNo">The version string, e.g. "1.2.3.4".</param>
    /// <returns><c>true</c> when the major and minor components match this assembly; otherwise <c>false</c>.</returns>
    /// <exception cref="System.Exception">Thrown when <paramref name="versionNo"/> is null, whitespace, or not a valid version.</exception>
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
    /// Checks a version against this assembly's version, comparing major and minor components only.
    /// </summary>
    /// <param name="version">The version to compare.</param>
    /// <returns><c>true</c> when the major and minor components match this assembly; otherwise <c>false</c>.</returns>
    public static bool VersionCheck(Version version)
    {
        var asm = Assembly.GetAssembly(typeof(ClientVersionHelper));

        var curVersion = asm.GetName().Version;

        if (curVersion.Major == version.Major && curVersion.Minor == version.Minor)
            return true;

        return false;
    }
}
