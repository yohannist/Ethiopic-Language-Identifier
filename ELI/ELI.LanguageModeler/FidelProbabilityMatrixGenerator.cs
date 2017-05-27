using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using ELI.Service.Shared;

namespace ELI.Service.LanguageModeler
{
    public class FidelProbabilityMatrixGenerator
    {
        private static readonly uint[] Additions = {0x00A0, 0x005C, 0x002E};
        private readonly string _corpus;

        public FidelProbabilityMatrixGenerator(string corpus)
        {
            var array = CorpusCleaner.Clean(corpus).Split(new []{" ", "\n", ";", ":"}, StringSplitOptions.RemoveEmptyEntries).Distinct();
            _corpus = string.Join(" ", array);
        }

        public DataTable Generate()
        {
            var summed = GenerateProceedingCharacterSumProbabilitMatrix();

            NormalizeTable(summed);

            return summed;
        }


        private void NormalizeTable(DataTable dataTable)
        {
            var totalAppearance = GetTotalCharacterAppearance(dataTable);

            foreach (DataRow row in dataTable.Rows)
            foreach (var a in totalAppearance)
                row[a.Key] = row.Field<double>(a.Key) / a.Value;
        }


        /// <summary>
        /// </summary>
        /// <returns></returns>
        private DataTable GenerateProceedingCharacterSumProbabilitMatrix()
        {
            var matrix = CreateCharacterMatrix();

//            var s1 = DateTime.Now;
//            Parallel.ForEach(Enumerable.Range(0, matrix.Rows.Count), index =>
//            {
//                var row = matrix.Rows[index];
//                ComputeProbability(matrix, row);
//            });
//            var t1 = DateTime.Now - s1;

            var s2 = DateTime.Now;
            foreach (DataRow row in matrix.Rows)
                ComputeProbability(matrix, row);

            var t2 = DateTime.Now - s2;

            return matrix;
        }

        private void ComputeProbability(DataTable matrix, DataRow row)
        {
            for (var i = 0; i < _corpus.Length - 1; i++)
            {
                var currentCharacter = new string(_corpus[i], 1);
                var nextCharacter = new string(_corpus[i + 1], 1);

                if (row.Field<string>(Constants.CharColumnName) != currentCharacter)
                    continue;

                if (matrix.Columns.Contains(nextCharacter))
                    row[nextCharacter] = Convert.ToInt32(row[nextCharacter] ?? 0) + 1;
            }
        }

        private IDictionary<string, double> GetTotalCharacterAppearance(DataTable dataTable)
        {
            return dataTable
                .AsEnumerable()
                .Select(t => new
                {
                    Key = t.Field<string>(Constants.CharColumnName),
                    Sum = t.ItemArray.Skip(1).Cast<double>().Sum()
                }).ToDictionary(t => t.Key, t => t.Sum);
        }

        /// <summary>
        ///     Creates the barebones 300 by 300 matrix
        /// </summary>
        /// <returns></returns>
        private static DataTable CreateCharacterMatrix()
        {
            var dataTable = new DataTable();

            #region create the columns

            dataTable.Columns.Add(Constants.CharColumnName, typeof(string));

            var columns = Enumerable.Range((int) Constants.StartCharCode,
                    (int) (Constants.EndCharCode - Constants.StartCharCode) + 1)
                .Select(t => new DataColumn(((char) t).ToString(CultureInfo.InvariantCulture), typeof(double))
                {
                    DefaultValue = 0
                }).ToArray();

            dataTable.Columns.AddRange(columns);

            foreach (var addition in Additions)
                dataTable.Columns.Add(new DataColumn(((char) addition).ToString(CultureInfo.InvariantCulture),
                    typeof(double)) {DefaultValue = 0});

            #endregion

            #region add the rows

            for (var i = Constants.StartCharCode; i <= Constants.EndCharCode; i++)
            {
                var row = dataTable.NewRow();
                row[Constants.CharColumnName] = (char) i;
               
                dataTable.Rows.Add(row);
            }

            #endregion

            return dataTable;
        }
    }
}