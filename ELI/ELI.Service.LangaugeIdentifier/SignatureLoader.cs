using System.Collections.Generic;
using System.IO;
using System.Text;
using ELI.Service.Shared;
using Newtonsoft.Json;

namespace ELI.Service.LangaugeIdentifier
{
    public class SignatureLoader
    {
        public IList<SignatureModel> Load(string path)
        {
            if (!Directory.Exists(path))
                throw new DirectoryNotFoundException(path);

            IList<SignatureModel> signatures = new List<SignatureModel>();

            var signatureFiles = Directory.EnumerateFiles(path);

            foreach (var file in signatureFiles)
            {
                var text = File.ReadAllText(file, Encoding.UTF8);

                var signature = JsonConvert.DeserializeObject<SignatureModel>(text);

                signatures.Add(signature);
            }

            return signatures;
        }
    }
}