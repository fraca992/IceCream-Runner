using UnityEditor;

public class BuildScript    
{
    [MenuItem("Build/Build Windows")]
    static void PerformBuild()
    {
        string[] defaultScene = { "Assets/Scenes/Level1.unity" };
        BuildPlayerOptions myBuildOptions = new BuildPlayerOptions();

        myBuildOptions.scenes = defaultScene;
        myBuildOptions.locationPathName = "./Builds/ICR_Windowsx86";
        myBuildOptions.target = BuildTarget.StandaloneWindows;
        myBuildOptions.options = BuildOptions.None;

        BuildPipeline.BuildPlayer(myBuildOptions);
    }
}
