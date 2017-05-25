using System.Collections.Generic;
namespace ELI.Service.LanguageModeler
{
    interface ICorpusLoader
    {
       IList<string> GetCorpus();
    }
}
