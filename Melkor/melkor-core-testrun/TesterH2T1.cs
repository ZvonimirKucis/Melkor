using System;
using System.Collections.Generic;
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

        public TesterH2T1(string DLLPath)
        {
            var asm = Assembly.LoadFrom(DLLPath);
            var repoTypeName = asm.GetTypes().Where(x => x.ToString().ToLower().Contains(".todorepository"))
                .Select(x => x.ToString()).FirstOrDefault();
            var itemTypeName = asm.GetTypes().Where(x => x.ToString().ToLower().Contains(".todoitem"))
                .Select(x => x.ToString()).FirstOrDefault();
            _repoType = asm.GetType(repoTypeName);
            _itemType = asm.GetType(itemTypeName);
            asm = null;
            GC.Collect();
            GC.WaitForPendingFinalizers();
        }

        public bool RunTest()
        {
            return AddingNullToDatabaseThrowsException() && AddingItemWillAddToDatabase();
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
