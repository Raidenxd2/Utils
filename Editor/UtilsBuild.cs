using System.IO;
using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;

namespace raiden.utils.editor
{
    public class UtilsBuild : IPostprocessBuildWithReport
    {
        public int callbackOrder { get {return 0;} }

        public void OnPostprocessBuild(BuildReport report)
        {
            string dataName = string.Empty;
            if (report.summary.platform is BuildTarget.StandaloneWindows64 or BuildTarget.StandaloneWindows)
            {
                dataName = Path.GetFileNameWithoutExtension(report.summary.outputPath) + "_Data";
            }
            
            if (report.summary.platform == BuildTarget.StandaloneWindows64)
            {
                if (UnityEditor.WindowsStandalone.UserBuildSettings.architecture == OSArchitecture.ARM64)
                {
                    string outputPath = Path.GetDirectoryName(report.summary.outputPath);
                    Directory.CreateDirectory(outputPath + "/" + dataName + "/ExecutableSourcesAndSymbols");
                    CopyFilesRecursively("Packages/com.raiden.utils/BuildOutput/ARM64/ExecutableSourcesAndSymbols", outputPath + "/" + dataName + "/ExecutableSourcesAndSymbols");
                }
                else
                {
                    string outputPath = Path.GetDirectoryName(report.summary.outputPath);
                    Directory.CreateDirectory(outputPath + "/" + dataName + "/ExecutableSourcesAndSymbols");
                    CopyFilesRecursively("Packages/com.raiden.utils/BuildOutput/x64/ExecutableSourcesAndSymbols", outputPath + "/" + dataName + "/ExecutableSourcesAndSymbols");
                }
            }
            else if (report.summary.platform == BuildTarget.StandaloneWindows)
            {
                string outputPath = Path.GetDirectoryName(report.summary.outputPath);
                Directory.CreateDirectory(outputPath +  "/" + dataName + "/ExecutableSourcesAndSymbols");
                CopyFilesRecursively("Packages/com.raiden.utils/BuildOutput/x86/ExecutableSourcesAndSymbols", outputPath + "/" + dataName + "/ExecutableSourcesAndSymbols");
            }
        }

        private static void CopyFilesRecursively(string sourcePath, string targetPath)
        {
            //Now Create all of the directories
            foreach (string dirPath in Directory.GetDirectories(sourcePath, "*", SearchOption.AllDirectories))
            {
                Directory.CreateDirectory(dirPath.Replace(sourcePath, targetPath));
            }

            //Copy all the files & Replaces any files with the same name
            foreach (string newPath in Directory.GetFiles(sourcePath, "*.*", SearchOption.AllDirectories))
            {
                if (newPath.Contains(".meta"))
                {
                    continue;
                }
                File.Copy(newPath, newPath.Replace(sourcePath, targetPath), true);
            }
        }
    }
}