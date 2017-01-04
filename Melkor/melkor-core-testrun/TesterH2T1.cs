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
        private readonly Assembly _asm;
        private readonly string _repoTypeName;
        private readonly string _itemTypeName;

        public TesterH2T1(string DLLPath)
        {
            _asm = Assembly.LoadFrom(DLLPath);
            _repoTypeName = _asm.GetTypes().Where(x => x.ToString().ToLower().Contains(".todorepository"))
                .Select(x => x.ToString()).FirstOrDefault();
            _itemTypeName = _asm.GetTypes().Where(x => x.ToString().ToLower().Contains(".todoitem"))
                .Select(x => x.ToString()).FirstOrDefault();
        }

        public bool RunTest()
        {
            return AddingNullToDatabaseThrowsException() && AddingItemWillAddToDatabase();
        }
        
        public bool AddingNullToDatabaseThrowsException()
        {
            object[] classConstructorArgs = {null};
           
            Type repo = _asm.GetType(_repoTypeName);
            dynamic repoInstance = Activator.CreateInstance(repo, classConstructorArgs);
            try
            {
                repoInstance.Add(null);
            }
            catch (ArgumentNullException exception)
            {
                return true;
            }
            return false;
        }

        public bool AddingItemWillAddToDatabase()
        {
            object[] classConstructorArgs = { null };
            object[] constArgs = { "Test" };

            Type item = _asm.GetType(_itemTypeName);
            Type repo = _asm.GetType(_repoTypeName);
            dynamic itemInstance = Activator.CreateInstance(item, constArgs);
            dynamic repoInstance = Activator.CreateInstance(repo, classConstructorArgs);
            repoInstance.Add(itemInstance);
            if (repoInstance.GetAll().Count == 1) return true;
            return false;
        }
    }
}
