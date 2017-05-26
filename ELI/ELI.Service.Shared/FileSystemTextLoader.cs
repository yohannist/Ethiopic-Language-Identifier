using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace ELI.Service.LanguageModeler
{
    public class FileSystemTextLoader : ITextLoader
    {
        public readonly char[] WordDelimiters = {';', ' ', ':', ',', '\n'};
        private readonly string _path;

        public FileSystemTextLoader(string path)
        {
            _path = path;
        }

        public  string GetText()
        {
            if (File.Exists(_path))
            {
                return File.ReadAllText(_path);
            }

            if (Directory.Exists(_path))
            {
                StringBuilder  builder = new StringBuilder();
                var files = Directory.EnumerateFiles(_path, "*.*", SearchOption.AllDirectories);

                foreach (var file in files)
                    builder.AppendLine(File.ReadAllText(file));

                return builder.ToString();
            }

            throw new FileNotFoundException($"No file or directory was found at {_path}");
        }
    }
}
