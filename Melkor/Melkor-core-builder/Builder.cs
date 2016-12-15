using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.Build.Evaluation;
using Microsoft.Build.Exceptions;
using Microsoft.Build.Execution;
using Microsoft.Build.Framework;
using Microsoft.Build.Logging;

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
            bool result = false;
            try
            {
                if (path == null) throw new NullReferenceException();
                if (!path.EndsWith(".csproj")) throw new InvalidProjectFileException();

                string projectFilePath = path;

                ProjectCollection pc = new ProjectCollection();
                
                Dictionary<string, string> globalProperty = new Dictionary<string, string>();
               // globalProperty.Add("OutputPath", Directory.GetCurrentDirectory() + "\\build\\bin\\Release");

                BuildParameters bp = new BuildParameters(pc);
                BuildRequestData buildRequest = new BuildRequestData(projectFilePath, globalProperty, "4.0", new string[] { "Build" }, null);
                BuildResult buildResult = BuildManager.DefaultBuildManager.Build(bp, buildRequest);
                if (buildResult.OverallResult == BuildResultCode.Success)
                {
                    result = true;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Build Failed: " + e.ToString());
            }
            return result;
        }
        

        public string[] FindProjectFile(string path)
        {
            string[] files = Directory.GetFiles(path, "*.csproj", SearchOption.AllDirectories);
            return files;
        }

        /// <summary>
        /// <p>Metadata :</p>
        /// <p> - Filename -> "Zad6" </p>
        /// <p> - Extension -> ".dll" </p>
        /// </summary>
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