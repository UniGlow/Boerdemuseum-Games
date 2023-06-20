using System.IO;
using UnityEngine;

public static class PathExtension
{

    /// <summary>
    /// Returns the absolut directory to a given path if the path starts with Assets or is already absolut
    /// </summary>
    /// <param name="path">path to get directory</param>
    /// <param name="useBackslash">wether to use back or forwardslash</param>
    /// <param name="slashAtEnd">if true the path will end with a slash</param>
    /// <returns></returns>
    public static string GetAbsolutDirectory(string path, bool slashAtEnd = false, bool useBackslash = false)
    {
        if (string.IsNullOrEmpty(path))
        {
            return null;
        }

        if (Path.HasExtension(path))
        {
            path = Path.GetDirectoryName(path);
        }
        path = SlashAtEnd(path, slashAtEnd, useBackslash);
        path = GetAbsolutPath(path, useBackslash);

        return path;
    }

    public static string GetAbsolutPath(string path, bool useBackslash = false)
    {
        path = ChangeSlashInPath(path, useBackslash);

        string slash = useBackslash ? "\\" : "/";

        if (path.StartsWith(slash) || !Path.IsPathRooted(path))
        {
            if (path.StartsWith("Assets" + slash) || path.Equals("Assets"))
            {
                path = ChangeSlashInPath(Application.dataPath, useBackslash) + path.Substring("Assets".Length);
            }
            else
            {
                return null;
            }
        }
        return path;
    }

    public static string GetAssetPath(string path)
    {
        if (string.IsNullOrEmpty(path))
        {
            return null;
        }

        path = ChangeSlashInPath(path, false);

        if (Path.IsPathRooted(path) && !path.StartsWith("/"))
        {
            if (path.StartsWith(Application.dataPath))
            {
                path = path.Replace(Application.dataPath, "Assets");

            }
            else
            {
                return null;
            }
        }
        else
        {
            if (!path.StartsWith("Assets/") && !path.Equals("Assets"))
            {
                path = (path.StartsWith("/") ? "Assets" : "Assets/") + path;
            }
        }
        return path;
    }

    public static string GetAssetDirectory(string path, bool slashAtEnd = false)
    {
        if (string.IsNullOrEmpty(path))
        {
            return null;
        }

        if (Path.HasExtension(path))
        {
            path = Path.GetDirectoryName(path);
        }

        path = SlashAtEnd(path, slashAtEnd);

        path = GetAssetPath(path);

        return path;
    }

    public static string ChangeSlashInPath(string path, bool useBackslash)
    {
        string oldValue = useBackslash ? "/" : "\\";
        string newValue = useBackslash ? "\\" : "/";

        path = path.Replace(oldValue, newValue);
        path = path.Replace(newValue + newValue, newValue);

        return path;
    }

    public static string SlashAtEnd(string path, bool slashAtEnd, bool useBackslash = false)
    {
        string newValue = useBackslash ? "\\" : "/";

        if (slashAtEnd && !path.EndsWith(newValue))
        {
            path += newValue;
        }
        else if (!slashAtEnd && path.EndsWith(newValue))
        {
            path = path.Remove(path.Length - 1);
        }

        return path;
    }
}
