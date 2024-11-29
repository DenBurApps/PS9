using UnityEditor;
using UnityEditor.Callbacks;
using UnityEditor.iOS.Xcode;
using System.IO;

public class IosBuildPostprocessor
{

    [PostProcessBuild(1)]
    public static void EditPlist(BuildTarget target, string path)
    {
        if (target != BuildTarget.iOS)
            return;


        string plistPath = path + "/Info.plist";
        PlistDocument plist = new PlistDocument();
        plist.ReadFromFile(plistPath);

        PlistElementDict rootDict = plist.root;

        // Add ITSAppUsesNonExemptEncryption to Info.plist
        rootDict.SetString("ITSAppUsesNonExemptEncryption", "false");
        string projectPath = path + "/Unity-iPhone.xcodeproj/project.pbxproj";
        PBXProject pbxProject = new PBXProject();
        pbxProject.ReadFromFile(projectPath);

        // Getting the UnityFramework Target and changing build settings
        string tar = pbxProject.GetUnityFrameworkTargetGuid();
        pbxProject.SetBuildProperty(tar, "ALWAYS_EMBED_SWIFT_STANDARD_LIBRARIES", "NO");

        // After we're done editing the build settings we save it 
        pbxProject.WriteToFile(projectPath);


        File.WriteAllText(plistPath, plist.WriteToString());
    }
}