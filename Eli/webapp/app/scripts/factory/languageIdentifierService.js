(function () {

    'use strict';

    angular.module('eliApp')
        .factory('languageIdentifier', factory);

    function factory() {
 
        return {
            identify: identify
        };
  
        function identify(signatures, sampleText) {
            if (!sampleText) {
                return undefined;
            }

            /* foreach (var signature in _signatures)
                        {
                            probabilityScore.Add(signature.Language, 0);
            
                            for (var i = 0; i < sampleText.Length - 1; i++)
                            {
                                var currentChar = sampleText[i];
                                var nextChar = sampleText[i + 1];
            
                                var row = signature.Matrix.AsEnumerable()
                                    .SingleOrDefault(t => t.Field<string>(Constants.CharColumnName) == currentChar.ToString());
                                if (row == null) continue;
            
                                double value = 0;
            
                                if (!signature.Matrix.Columns.Contains(nextChar.ToString()))
                                    continue;
            
                                if (double.TryParse(row[nextChar.ToString()].ToString(), out value))
                                    if (Math.Abs(value) > 0.0001)
                                        probabilityScore[signature.Language] += value;
                            }
                        }
            
             */
            var probabilityScore = [];

            angular.forEach(signatures, function (signature) {

                var score = {
                    Language: signature.Language,
                    Score: 0
                };


                for (var i = 0; i < sampleText.length - 1; i++) {

                    var currentChar = sampleText[i];
                    var nextChar = sampleText[i + 1];

                    for (var j = 0; j < signature.Matrix.length; j++) {
                        var row = signature.Matrix[j];

                        if (row.Char === currentChar) {
                            var value = 0;

                            if (row[nextChar]) {
                                value = row[nextChar];
                            }
                            score.Score += value;

                            break;
                        }
                    }
                }

                probabilityScore.push(score);

            });

            return probabilityScore;
        }


    };

})();