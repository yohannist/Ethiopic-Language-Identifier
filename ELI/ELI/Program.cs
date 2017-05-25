using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace ELI.Service.LanguageModeler
{
    class Program
    {
        public static int Main(string[] args)
        {
            if (args.Length != 2)
            {
                Console.ForegroundColor = ConsoleColor.DarkYellow;
                Console.WriteLine("Language modeler needs <Language Resource Path> <Signature output path");
                Console.ResetColor();

            }

            string inputPath = args[0];
            string outputpath = args[1];

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Attempting to load the corpus from {0} ...", inputPath);

            ICorpusLoader loader = new FileSystemCorpusLoader(inputPath);

            var corpus = loader.GetCorpus();

            Console.WriteLine("{0} words loaded \nComputing probability matrix...", corpus.Count());

            var generator = new FidelProbabilityMatrixGenerator(corpus);

            var matrix = generator.Generate();

            var serialized = JsonConvert.SerializeObject(matrix, Formatting.Indented);

            File.WriteAllText(outputpath, serialized, Encoding.UTF8);

            Console.WriteLine("Probability matrix output placed at {0}", outputpath);
            Console.Beep();

            Console.WriteLine("Press any key to exit");

            Console.ReadKey();
            
            Console.ResetColor();

            return 0;
        }
    }
}
