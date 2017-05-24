using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;

namespace ELI.Service.LanguageModeler
{
    public class FidelProbabilityMatrixGenerator
    {
        private const string CharColumnName = "Char";
    
        private readonly string _corpus;

        private const uint StartCharCode = 0x1200;
        private const uint EndCharCode = 0x1357;

        private readonly IList<string> _words;

        private static readonly uint[] Additions = { 0x00A0, 0x005C, 0x002E };


        public FidelProbabilityMatrixGenerator(IEnumerable<string> words)
        {
            _words = words.ToList();
            _corpus = string.Join(" ", _words);
        }

        public DataTable GenerateProbabilityMatrix()
        {
            var summed = GenerateProceedingCharacterSumProbabilitMatrix();

            NormalizeTable(summed);

            return summed;
        }



        private void NormalizeTable(DataTable dataTable)
        {
            var totalAppearance = GetTotalCharacterAppearance(dataTable);
            
            foreach (DataRow row in dataTable.Rows)
            {
                foreach (var a in totalAppearance)
                {
                    row[a.Key] = row.Field<double>(a.Key)/a.Value;
                }
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private DataTable GenerateProceedingCharacterSumProbabilitMatrix()
        {
            var matrix = CreateCharacterMatrix();
            foreach (DataRow row in matrix.Rows)
            {
                for (var i = 0; i < _corpus.Length - 1; i++)
                {
                    var currentCharacter = new string(_corpus[i], 1);
                    var nextCharacter = new string(_corpus[i + 1], 1);

                    if (row.Field<string>(CharColumnName) != currentCharacter)
                        continue;

                    if (matrix.Columns.Contains(nextCharacter))
                        row[nextCharacter] = Convert.ToInt32(row[nextCharacter]) + 1;
                }
            }

            return matrix;
        }

        private IDictionary<char, int> GetTotalCharacterAppearanceInCorpus(string corpus)
        {
            var chars = new List<char>();

            for (var i = StartCharCode; i <= EndCharCode; i++)
                chars.Add((char)i);

            var summed = _words.SelectMany(t => t.Substring(0, t.Length - 1)).GroupBy(c => c).Select(t => new
            {
                t.Key,
                Count = t.Count()
            });

            var totalAppearance =  chars.Join(summed, a => a, b => b.Key, (a, b) => new
            {
                Key = a,
                b.Count
            }).ToDictionary(t => t.Key, t => t.Count);

            return totalAppearance;
        }

        private IDictionary<string, int> GetTotalCharacterAppearance(DataTable dataTable)
        {
            return dataTable
                .AsEnumerable()
                .Select(t => new
                {
                    Key = t.Field<string>(CharColumnName)
                    , Sum = t.ItemArray.Skip(1).Cast<int>().Sum()
                }).ToDictionary(t => t.Key, t => t.Sum);
        }

        /// <summary>
        /// Creates the barebones 300 by 300 matrix
        /// </summary>
        /// <returns></returns>
        private static DataTable CreateCharacterMatrix()
        {
            var dataTable = new DataTable();

            #region create the columns
            dataTable.Columns.Add(CharColumnName, typeof(string));
            for (var i = StartCharCode; i <= EndCharCode; i++)
            {
                dataTable.Columns.Add(new DataColumn(((char)i).ToString(CultureInfo.InvariantCulture), typeof(double)) { DefaultValue = 0 });
            }

            foreach (var addition in Additions)
            {
                dataTable.Columns.Add(new DataColumn(((char)addition).ToString(CultureInfo.InvariantCulture), typeof(double)) { DefaultValue = 0 });
            }
            #endregion

            #region add the rows

            for (var i = StartCharCode; i <= EndCharCode; i++)
            {
                var row = dataTable.NewRow();
                row[CharColumnName] = (char)i;
                dataTable.Rows.Add(row);
            }
            #endregion

            return dataTable;
        }
    }
}
