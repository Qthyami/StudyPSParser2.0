
namespace StudyPSParser2._0.Common;

public static class Strings {
public static IEnumerable<string>
SplitWords(this string text)=>
    text.Split(new[] { ' ', '\t', '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries);
}
