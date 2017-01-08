using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Melkor_core_dbhandler;

namespace melkor_core_testrun
{
    /// <summary>
    ///     <p>Autor : Kucis</p>
    /// </summary>
    public class TesterH2T1
    {
        private readonly Type _repoType;
        private readonly Type _itemType;
        private readonly Guid _userGuid;

        public TesterH2T1(string DLLPath, Guid userGuid)
        {
            this._userGuid = userGuid;
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
        
        public List<TestContext> RunTest()
        {
            var res = new List<TestContext>
            {
                new TestContext("AddingNullToDatabaseThrowsException", AddingNullToDatabaseThrowsException(), _userGuid),
                new TestContext("AddingItemWillAddToDatabase", AddingItemWillAddToDatabase(), _userGuid)
            };

            return res;
        }
        
        private bool AddingNullToDatabaseThrowsException()
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

        private bool AddingItemWillAddToDatabase()
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
