using System.Linq;
using ELI.Service.Shared;

namespace ELI.Service.LanguageModeler
{
    internal static class CorpusCleaner
    {
        internal static string Clean(string input)
        {
            var clean = input.Where(t => !(char.IsPunctuation(t) || char.IsDigit(t) || char.IsNumber(t) ||
                                           t < Constants.StartCharCode || t > Constants.EndCharCode)).ToArray();

            return new string(clean);
        }
    }
}