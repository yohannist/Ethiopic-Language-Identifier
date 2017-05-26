 using System;
 using ELI.Service.LangaugeIdentifier;
 using ELI.Service.LanguageModeler;

namespace ELI.LanguageIdentifier
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length != 2)
            {
                Console.ForegroundColor = ConsoleColor.DarkYellow;
                Console.WriteLine("Language identifier needs <Path to signature files> <Path to test file>");
                Console.ResetColor();
            }

            var signaturePath = args[0];
            var inputFilePath = args[1];

            var signatures = new SignatureLoader().Load(signaturePath);
            var inputFile = new FileSystemTextLoader(inputFilePath).GetText();
            
        
            var langIdentifier = new Service.LangaugeIdentifier.LanguageIdentifier(signatures);

            var language = langIdentifier.IdentifyLanguage(inputFile);

            Console.WriteLine("Input file language is {0}", language);

            Console.ReadLine();
        }
    }
}
