using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ELI.Service.LanguageModeler
{
    class FileSystemCorpusLoader : ICorpusLoader
    {
        public readonly char[] WordDelimiters = {';', ' ', ':', ',', '\n'};
        private readonly string _path;

        public FileSystemCorpusLoader(string path)
        {
            _path = path;
        }

        public  IList<string> GetCorpus()
        {
            if (File.Exists(_path))
            {
               return File.ReadAllText(_path).Split(WordDelimiters, StringSplitOptions.RemoveEmptyEntries).ToList();
            }

            if (Directory.Exists(_path))
            {
                StringBuilder  builder = new StringBuilder();
                var files = Directory.EnumerateFiles(_path, "*.*", SearchOption.AllDirectories);

                foreach (var file in files)
                    builder.AppendLine(File.ReadAllText(file));

                return builder.ToString().Split(WordDelimiters, StringSplitOptions.RemoveEmptyEntries).ToList();
            }

            throw new FileNotFoundException($"No file or directory was found at {_path}");
        }
    }
}
