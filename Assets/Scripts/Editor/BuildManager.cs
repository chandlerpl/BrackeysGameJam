using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Build.Reporting;
using UnityEngine;

public class BuildManager
{
    [MenuItem("Build/Build WebGL")]
    public static void PerformWebGLBuild()
    {
        Build("build/WebGL", BuildTarget.WebGL);
    }
    
    [MenuItem("Build/Build Windows")]
    public static void PerformWindowsBuild()
    {
        Build("build/Windows/" + PlayerSettings.productName + ".exe", BuildTarget.StandaloneWindows64);
    }
    
    [MenuItem("Build/Build Linux")]
    public static void PerformLinuxBuild()
    {
        Build("build/Linux/" + PlayerSettings.productName + ".x86_64", BuildTarget.StandaloneLinux64);
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

