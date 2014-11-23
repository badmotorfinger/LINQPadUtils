namespace LINQPad
{
    using System;
    using System.IO;
    using System.Windows.Forms;

    using LINQPadUtils;

    enum SolutionType
    {
        VisualStudio2010,
        VisualStudio2012,
        VisualStudio2013,
        XamarinStudio5
    }

    public static partial class LinqPadExtensions
    {
        public static void CreateSolution<T>(this T obj, string solutionName)
        {
            var saveDialog = new FolderBrowserDialog()
            {
                Description = string.Format("Select a path where {0} can be saved.", solutionName)
            };

            DialogResult result = saveDialog.ShowDialog();

            if (result == DialogResult.OK)
            {
                saveDialog.SelectedPath.EnsureDirectoryExists();
                
                string solutionNameClean = solutionName.EndsWith(".sln")
                    ? solutionName.Substring(0, solutionName.Length - 4)
                    : solutionName;

                var solutionPath = CreateSolution(solutionName, saveDialog, solutionNameClean);

                var projectPath = CreateProject(solutionPath, solutionNameClean);

                string propertiesPath = Path.Combine(projectPath, "Properties");

                propertiesPath.EnsureDirectoryExists();

                string assemblyInfoFilePath = Path.Combine(propertiesPath, "AssemblyInfo.cs");

                File.WriteAllText(assemblyInfoFilePath, LinqPadUtilResources.linqpad_assemblyinfo_2012);
            }
        }

        static string CreateProject(string solutionPath, string solutionNameClean)
        {
            string projectPath = Path.Combine(solutionPath, solutionNameClean);

            projectPath.EnsureDirectoryExists();

            string projectFile = Path.Combine(projectPath, solutionNameClean + ".csproj");

            File.WriteAllText(projectFile, LinqPadUtilResources.linqpad_proj_template_2012.Replace("{projname}", solutionNameClean));

            return projectPath;
        }

        static string CreateSolution(string solutionName, FolderBrowserDialog saveDialog, string solutionNameClean)
        {
            string solutionPath = Path.Combine(saveDialog.SelectedPath, solutionNameClean);

            solutionPath.EnsureDirectoryExists();

            string solutionFile = Path.Combine(solutionPath, solutionNameClean + ".sln");

            File.WriteAllText(solutionFile, LinqPadUtilResources.linqpad_sln_template_2012.Replace("{slnname}", solutionName).Replace("{projguid}", Guid.NewGuid().ToString()));
            return solutionPath;
        }

        static void EnsureDirectoryExists(this string path)
        {
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
        }
    }
}