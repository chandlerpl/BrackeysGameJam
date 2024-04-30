using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEditor.Build.Reporting;
using UnityEngine;

public class BuildManager
{
    [MenuItem("Build/Build WebGL")]
    public static void PerformWebGLBuild()
    {
        Build("Build/WebGL", BuildTarget.WebGL);
    }

    [MenuItem("Build/Build Windows")]
    public static void PerformWindowsBuild()
    {
        Build("Build/Windows/" + GetName() + ".exe", BuildTarget.StandaloneWindows64);
    }

    [MenuItem("Build/Build Linux")]
    public static void PerformLinuxBuild()
    {
        Build("Build/Linux/" + GetName() + ".x86_64", BuildTarget.StandaloneLinux64);
    }

    public static void PerformAllBuilds()
    {
        string name = GetName();

        Build("Build/Windows/" + name + ".exe", BuildTarget.StandaloneWindows64);
        Build("Build/Linux/" + name + ".x86_64", BuildTarget.StandaloneLinux64);
    }

#if STEAMWORKS_NET
    [MenuItem("Build/Build Windows (Steam)")]
    public static void PerformWindowsSteamBuild()
    {
        Build("Build/Windows/" + GetName() + ".exe", BuildTarget.StandaloneWindows64);
    }

    [MenuItem("Build/Build Linux (Steam)")]
    public static void PerformLinuxSteamBuild()
    {
        Build("Build/Linux/" + GetName() + ".x86_64", BuildTarget.StandaloneLinux64);
    }

    public static void PerformAllSteamBuilds()
    {
        string name = GetName();

        Build("Build/Windows/" + name + ".exe", BuildTarget.StandaloneWindows64);
        Build("Build/Linux/" + name + ".x86_64", BuildTarget.StandaloneLinux64);
    }

    private static void WriteSteamBlock()
    {
		string currentDefines = PlayerSettings.GetScriptingDefineSymbolsForGroup(EditorUserBuildSettings.selectedBuildTargetGroup);
		HashSet<string> defines = new HashSet<string>(currentDefines.Split(';')) {
			"DISABLESTEAMWORKS"
		};

		string newDefines = string.Join(";", defines);
		if (newDefines != currentDefines) {
			PlayerSettings.SetScriptingDefineSymbolsForGroup(EditorUserBuildSettings.selectedBuildTargetGroup, newDefines);
		}
    }
#endif

    /*    [MenuItem("Build/Build Android")]
        public static void PerformAndroidBuild()
        {
            DeleteOldBuild("Android");

            Build("Build/Android/" + GetName() + ".apk", BuildTarget.Android);
        }

        [MenuItem("Build/Build IOS")]
        public static void PerformIOSBuild()
        {
            DeleteOldBuild("IOS");

            Build("Build/IOS/" + GetName(), BuildTarget.iOS);
        }*/

    private static string GetName()
    {
        string buildID = GetBuildID();
        int buildNumber = BuildInfo.BuildNumber + 1;

        EditorPrefs.SetBool("JdkUseEmbedded", true);
        EditorPrefs.SetBool("NdkUseEmbedded", true);
        EditorPrefs.SetBool("SdkUseEmbedded", true);
        EditorPrefs.SetBool("GradleUseEmbedded", true);
        EditorPrefs.SetBool("AndroidGradleStopDaemonsOnExit", true);
        WriteBuildID(buildID, buildNumber);

        AssetDatabase.Refresh(ImportAssetOptions.ForceUpdate);
        return PlayerSettings.productName + "-" + buildID + "-" + buildNumber;
    }

    private static string GetBuildInfo()
    {
        string buildID = GetBuildID();
        int buildNumber = BuildInfo.BuildNumber + 1;

        return buildID + "-" + buildNumber;
    }

    private static void DeleteOldBuild(string folder)
    {
        string oldBuildID = BuildInfo.BuildID;
        int buildNumber = BuildInfo.BuildNumber;

        string projectFolder = Path.Combine(Application.dataPath, "../Build/" + folder);

        if (System.IO.Directory.Exists(projectFolder))
            System.IO.Directory.Delete(projectFolder, true);
    }

    private static void WriteBuildID(string buildID, int buildNumber)
    {
        string projectFolder = Path.Combine(Application.dataPath, "BuildInfo/BuildInfo.cs");

        List<string> lines = new List<string> {
            "public class BuildInfo {",
            "    public static int BuildNumber = " + buildNumber + ";",
            "    public static string BuildID = \"" + buildID + "\";",
            "}"
        };
        System.IO.File.WriteAllLines(projectFolder, lines);

        PlayerSettings.bundleVersion = "0.1." + buildNumber;
    }

    private static string GetBuildID()
    {
        string projectFolder = Path.Combine(Application.dataPath, "../");
        string git = Path.Combine(projectFolder, ".git");

        string buildID = System.IO.File.ReadAllText(Path.Combine(git, "FETCH_HEAD")).Substring(0, 7);

        return buildID;
    }

    private static void Build(string path, BuildTarget target)
    {
        BuildPlayerOptions buildPlayerOptions = new BuildPlayerOptions();
        List<string> scenes = new List<string>();
        foreach (EditorBuildSettingsScene scene in EditorBuildSettings.scenes)
        {
            if (!scene.enabled) continue;
            scenes.Add(scene.path);
        }
        buildPlayerOptions.scenes = scenes.ToArray();
        buildPlayerOptions.options = BuildOptions.None;
        buildPlayerOptions.locationPathName = path;
        buildPlayerOptions.target = target;


        BuildReport report = BuildPipeline.BuildPlayer(buildPlayerOptions);
        BuildSummary summary = report.summary;

        switch (summary.result)
        {
            case BuildResult.Succeeded:
                Debug.Log("Build Succeeded: " + summary.totalSize + " bytes - time: " + summary.totalTime.TotalSeconds + " seconds.");
                break;
            case BuildResult.Failed:
                Debug.Log("Build Failed - Errors:" + summary.totalErrors);
                break;
            default:
                Debug.Log("Unknown problem.");
                break;
        }
    }
}

