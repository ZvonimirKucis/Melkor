#region

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.Build.Evaluation;
using Microsoft.Build.Exceptions;
using Microsoft.Build.Execution;
using Microsoft.Build.Framework;
using Microsoft.Build.Logging;

#endregion

namespace Melkor_core_builder
{
    /// <summary>
    ///     <p>Autor : Alen</p>
    /// </summary>
    public class Builder
    {
        private readonly string _targetPath;

        public Builder(string targetPath)
        {
            _targetPath = targetPath;
        }

        public IEnumerable<BuildItem> Build()
        {
            var items = new List<BuildItem>();
            var allProjectPaths = FindProjectFile(_targetPath);
            foreach (var dir in allProjectPaths)
            {
                var item = BuildProject(dir, false);
                items.Add(item);

                Console.WriteLine("Building " + item.Status.ToString() + " -> " + item.Dir);
            }
            return items;
        }
        
        private static BuildItem BuildProject(string path, bool debug)
        {
            var logger = new ConsoleLogger(LoggerVerbosity.Minimal);

            try
            {
                if (path == null) throw new NullReferenceException();
                if (!path.EndsWith(".csproj")) throw new InvalidProjectFileException();

                var projectFilePath = path;

                var pc = new ProjectCollection();

                var globalProperty = new Dictionary<string, string>();
                // globalProperty.Add("OutputPath", Directory.GetCurrentDirectory() + "\\build\\bin\\Release");
                BuildParameters bp;
                if (debug)
                    bp = new BuildParameters(pc)
                    {
                        OnlyLogCriticalEvents = true,
                        DetailedSummary = false,
                        Loggers = new List<ILogger> {logger}.AsEnumerable(),
                        MaxNodeCount = 16
                    };
                else
                    bp = new BuildParameters(pc)
                    {
                        MaxNodeCount = 8
                    };

                var buildRequest = new BuildRequestData(projectFilePath, globalProperty, null, new[] {"Build"}, null);

                var buildResult = BuildManager.DefaultBuildManager.Build(bp, buildRequest);

                return new BuildItem(dir: path, status: buildResult.OverallResult == BuildResultCode.Success);
            }
            catch (Exception e)
            {
                Console.WriteLine("Build Failed: " + e);
            }
            return null;
        }

        public string[] FindProjectFile(string path)
        {
            var files = Directory.GetFiles(path, "*.csproj", SearchOption.AllDirectories);
            return files;
        }

    }

}