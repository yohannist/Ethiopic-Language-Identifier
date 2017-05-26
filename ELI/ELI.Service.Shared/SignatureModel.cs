using System.Data;


namespace ELI.Service.LangaugeIdentifier
{
    public class SignatureModel
    {
        public string Language { get; set; }
        public DataTable Matrix { get; set; }
    }
}
