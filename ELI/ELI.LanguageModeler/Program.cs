using System;
using System.IO;
using System.Linq;
using System.Text;
using ELI.Service.Shared;
using Newtonsoft.Json;

namespace ELI.Service.LanguageModeler
{
    internal class Program
    {
        public static int Main(string[] args)
        {
            if (args.Length != 3)
            {
                Console.ForegroundColor = ConsoleColor.DarkYellow;
                Console.WriteLine("Language modeler needs <Language Resource Path> <Signature output path> <language>");
                Console.ResetColor();
            }

            var inputPath = args[0];
            var outputpath = args[1];
            var language = args[2];

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Attempting to load the corpus from {0} ...", inputPath);

            ITextLoader loader = new FileSystemTextLoader(inputPath);

            var corpus = loader.GetText();

            Console.WriteLine("{0} words loaded \nComputing probability matrix...", corpus.Count());

            var generator = new FidelProbabilityMatrixGenerator(corpus);

            var matrix = generator.Generate();

            var serialized = JsonConvert.SerializeObject(new SignatureModel {Language = language, Matrix = matrix},
                Formatting.Indented);

            File.WriteAllText(outputpath, serialized, Encoding.UTF8);

            Console.WriteLine("Probability matrix output placed at {0}", outputpath);

            Console.ResetColor();

            return 0;
        }
    }
}