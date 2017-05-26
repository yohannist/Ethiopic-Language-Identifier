namespace ELI.Service.LangaugeIdentifier
{
    internal interface ILanguageIdentifier
    {
        string IdentifyLanguage(string sampleText);
    }
}