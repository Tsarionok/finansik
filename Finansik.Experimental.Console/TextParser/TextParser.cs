namespace Finansik.Experimental.Console.TextParser;

public class TextParser : ITextParser
{
    public IEnumerable<string> ParseBySentences(string text)
    {
        return text.Split('.');
    }

    public IEnumerable<string> ParseByWords(string sentence)
    {
        return sentence.Split(' ', ',');
    }
}