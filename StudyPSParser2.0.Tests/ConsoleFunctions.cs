namespace StudyPSParser2._0.Tests;

public static class ConsoleFunctions {
    public static T
    WriteToConsole<T>(this T value, string text) {
        Console.WriteLine(text);
        return value;
    }

    public static T WriteToConsole<T>(this T value) {
        Console.WriteLine(value);
        return value;
    }

}
