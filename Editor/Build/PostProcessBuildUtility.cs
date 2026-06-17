using System.IO;

namespace UnitySupport.Tools
{
    public class PostProcessBuildUtility
    {
        public static void UpdateBootConfig(string buildOutputPath, string targetKey, string targetValue)
        {
            var bootConfig = string.Empty;

#if UNITY_STANDALONE_WIN || UNITY_STANDALONE_LINUX
            var buildFolder = Path.GetDirectoryName(buildOutputPath);
            var dataFolder = Path.GetFileNameWithoutExtension(buildOutputPath) + "_Data";
            bootConfig = Path.Combine(buildFolder, dataFolder, "boot.config");
#elif UNITY_STANDALONE_OSX
        bootConfig = Path.Combine(buildOutputPath, "Contents", "Resources", "Data", "boot.config");
#elif UNITY_IOS || UNITY_TVOS
        bootConfig = Path.Combine(buildOutputPath, "Data", "boot.config");
#elif UNITY_ANDROID
        bootConfig = Path.Combine(buildOutputPath, "src", "main", "assets", "bin", "Data", "boot.config");
#endif

            if (!File.Exists(bootConfig))
                return;

            var lines = File.ReadAllLines(bootConfig);
            var searchSetting = $"{targetKey}=";

            int i;
            for (i = 0; i < lines.Length; ++i)
                if (lines[i].StartsWith(searchSetting))
                    break;

            targetValue = searchSetting + targetValue;

            if (i >= lines.Length)
            {
                File.AppendAllLines(bootConfig, new[] { targetValue });
            }
            else
            {
                if (lines[i] == targetValue)
                    return;

                lines[i] = targetValue;
                File.WriteAllLines(bootConfig, lines);
            }
        }
    }
}