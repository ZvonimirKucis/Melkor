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

        public List<BuildItem> Build(string saveLocation)
        {
            var items = new List<BuildItem>();
            var allProjectPaths = FindProjectFile(_targetPath);
            foreach (var dir in allProjectPaths)
            {
                try
                {
                    Console.WriteLine("Found : " + dir);
                    var item = BuildProject(dir, saveLocation, false);
                    items.Add(item);

                    Console.WriteLine("Building " + item.Status.ToString() + " -> " + item.Name);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.StackTrace);
                }
            }
            return items;
        }
        
        private static BuildItem BuildProject(string path, string savePath, bool debug)
        {
            var logger = new ConsoleLogger(LoggerVerbosity.Minimal);

            try
            {
                if (path == null) throw new NullReferenceException();
                if (!path.EndsWith(".csproj")) throw new InvalidProjectFileException();

                var projectFilePath = path;

                var pc = new ProjectCollection();

                var globalProperty = new Dictionary<string, string>();
                
                globalProperty.Add("OutputPath", savePath);
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

                var item = new BuildItem(name: buildResult.ResultsByTarget["Build"].Items[0].GetMetadata("Filename"), 
                                            dir: buildResult.ResultsByTarget["Build"].Items[0].GetMetadata("FullPath"), 
                                            status: buildResult.OverallResult == BuildResultCode.Success);
                
                return item;
            }
            catch (Exception e)
            {
                Console.WriteLine("Build Failed: " + e);
            }
            return new BuildItem("BUILD FAILED ON ITEM", path, false);
        }

        public string[] FindProjectFile(string path)
        {
            var files = Directory.GetFiles(path, "*.csproj", SearchOption.AllDirectories);
            return files;
        }

         public static void getTargetName(BuildResult results)
         {
             if (results.ResultsByTarget != null)
             {
                 var targetData = results?.ResultsByTarget;
                 foreach (var target in targetData)
                 {
                     Console.WriteLine(target.Key + " : ");
                     foreach (var item in target.Value.Items)
                     {
                         foreach (var item2 in item.MetadataNames)
                         {
                             Console.WriteLine(item2.ToString() + " -> " + item.GetMetadata(item2.ToString()));
                         }
                     }
                 }
             }
         }
    }

}