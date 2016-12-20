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

        public bool Build3(string path)
        {
            var result = false;
            try
            {
                if (path == null) throw new NullReferenceException();
                if (!path.EndsWith(".csproj")) throw new InvalidProjectFileException();

                var projectFilePath = path;

                var pc = new ProjectCollection();

                var globalProperty = new Dictionary<string, string>();
                // globalProperty.Add("OutputPath", Directory.GetCurrentDirectory() + "\\build\\bin\\Release");

                var bp = new BuildParameters(pc);
                var buildRequest = new BuildRequestData(projectFilePath, globalProperty, null,
                    new[] {"Build"}, null);
                var buildResult = BuildManager.DefaultBuildManager.Build(bp, buildRequest);
                if (buildResult.OverallResult == BuildResultCode.Success)
                    result = true;
            }
            catch (Exception e)
            {
                Console.WriteLine("Build Failed: " + e);
            }
            return result;
        }

        public bool Build4(string path, bool debug)
        {
            var result = false;
            try
            {
                var logger = new ConsoleLogger(LoggerVerbosity.Minimal);

                if (path == null) throw new NullReferenceException();
                if (!path.EndsWith(".csproj")) throw new InvalidProjectFileException();

                var projectFilePath = path;

                var pc = new ProjectCollection();

                var globalProperty = new Dictionary<string, string>();
                // globalProperty.Add("OutputPath", Directory.GetCurrentDirectory() + "\\build\\bin\\Release");
                BuildParameters bp = null;
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

                if (buildResult.OverallResult == BuildResultCode.Success)
                    result = true;
            }
            catch (Exception e)
            {
                Console.WriteLine("Build Failed: " + e);
            }
            return result;
        }

        public string[] FindProjectFile (string path)
        {
            var files = Directory.GetFiles(path, "*.csproj", SearchOption.AllDirectories);
            return files;
        }

        /// </summary>
        /// <p> - Extension -> ".dll" </p>
        /// <p> - Filename -> "Zad6" </p>
        /// <p>Metadata :</p>

        /// <summary>
        /*  public void GetTargetData()
          {
              if (_results.ResultsByTarget != null)
              {
                  var targetData = _results?.ResultsByTarget;
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
          }*/
    }
}