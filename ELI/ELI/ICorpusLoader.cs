using System.Collections.Generic;
using System.Threading.Tasks;

namespace ELI.Service.LanguageModeler
{
    interface ICorpusLoader
    {
        Task<IList<string>> GetCorpusAsync();
    }
}
