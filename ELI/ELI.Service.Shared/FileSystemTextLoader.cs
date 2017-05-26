using System.IO;
using System.Text;

namespace ELI.Service.Shared
{
    public class FileSystemTextLoader : ITextLoader
    {
        private readonly string _path;
        public readonly char[] WordDelimiters = {';', ' ', ':', ',', '\n'};

        public FileSystemTextLoader(string path)
        {
            _path = path;
        }

        public string GetText()
        {
            if (File.Exists(_path))
                return File.ReadAllText(_path);

            if (Directory.Exists(_path))
            {
                var builder = new StringBuilder();
                var files = Directory.EnumerateFiles(_path, "*.*", SearchOption.AllDirectories);

                foreach (var file in files)
                    builder.AppendLine(File.ReadAllText(file));

                return builder.ToString();
            }

            throw new FileNotFoundException($"No file or directory was found at {_path}");
        }
    }
}