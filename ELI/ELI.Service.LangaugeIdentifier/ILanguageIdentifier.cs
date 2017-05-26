using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ELI.Service.LangaugeIdentifier
{
    interface ILanguageIdentifier
    {
        string IdentifyLanguage(string sampleText);
    }
}
