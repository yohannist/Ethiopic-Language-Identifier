using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.Linq;

namespace ELI.Service.LangaugeIdentifier
{
    public class LanguageIdentifier : ILanguageIdentifier
    {
        private readonly IEnumerable<SignatureModel> _signatures;

        public LanguageIdentifier(IEnumerable<SignatureModel> signatures)
        {
            _signatures = signatures;
        }

        public string IdentifyLanguage(string sampleText)
        {
            IDictionary<string, double> probabilityScore = new Dictionary<string, double>();

            foreach (var signature in _signatures)
            {
                probabilityScore.Add(signature.Language, 0);

                for (int i = 0; i < sampleText.Length - 1; i++)
                {
                    var currentChar = sampleText[i];
                    var nextChar = sampleText[i + 1];

                    var row = signature.Matrix.AsEnumerable().SingleOrDefault(t => t.Field<string>("Char") == currentChar.ToString());
                    if (row == null) continue;

                    double value = 0;

                    if (!signature.Matrix.Columns.Contains(nextChar.ToString()))
                        continue;

                    if (double.TryParse(row[nextChar.ToString()].ToString(), out value))
                        if(Math.Abs(value) > 0.0001)
                        probabilityScore[signature.Language] += value;
                }

            }

            return probabilityScore.OrderByDescending(t => t.Value).First().Key;
        }
    }
}
