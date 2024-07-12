namespace Finansik.Experimental.Console.TextParser;

public interface ITextParser
{
    IEnumerable<string> ParseBySentences(string text);

    IEnumerable<string> ParseByWords(string sentence);
}