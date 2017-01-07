using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace melkor_core_testrun
{
    /// <summary>
    ///     <p>Autor : Kucis</p>
    /// </summary>
    public class TesterH2T1
    {
        private readonly Type _repoType;
        private readonly Type _itemType;
        private Dictionary<string,bool> results = new Dictionary<string, bool>();

        public TesterH2T1(string DLLPath)
        {
            DLLPath = DllHelper.FindDll(DLLPath,"TodoRepository");
            using (Stream stream = File.OpenRead(DLLPath))
            {
                byte[] rawAssmebly = new byte[stream.Length];
                stream.Read(rawAssmebly, 0, (int) stream.Length);
                var asm = Assembly.Load(rawAssmebly);
                var repoTypeName = asm.GetTypes().Where(x => x.ToString().ToLower().Contains(".todorepository"))
                    .Select(x => x.ToString()).FirstOrDefault();
                var itemTypeName = asm.GetTypes().Where(x => x.ToString().ToLower().Contains(".todoitem"))
                    .Select(x => x.ToString()).FirstOrDefault();
                _repoType = asm.GetType(repoTypeName);
                _itemType = asm.GetType(itemTypeName);
            }
        }
        
        public Dictionary<string,bool> RunTest()
        {
            results.Add("AddingNullToDatabaseThrowsException",AddingNullToDatabaseThrowsException());
            results.Add("AddingItemWillAddToDatabase",AddingItemWillAddToDatabase());
            return results;
        }
        
        public bool AddingNullToDatabaseThrowsException()
        {
            object[] classConstructorArgs = {null};
           
            dynamic repoInstance = Activator.CreateInstance(_repoType, classConstructorArgs);
            try
            {
                repoInstance.Add(null);
            }
            catch (ArgumentNullException)
            {
                return true;
            }
            return false;
        }

        public bool AddingItemWillAddToDatabase()
        {
            object[] classConstructorArgs = { null };
            object[] constArgs = { "Test" };
            
            dynamic itemInstance = Activator.CreateInstance(_itemType, constArgs);
            dynamic repoInstance = Activator.CreateInstance(_repoType, classConstructorArgs);
            repoInstance.Add(itemInstance);
            if (repoInstance.GetAll().Count == 1) return true;
            return false;
        }
    }
}
