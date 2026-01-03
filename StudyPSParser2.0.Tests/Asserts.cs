namespace StudyPSParser2._0.Tests;

public static class Asserts {
    public static T
    Assert<T> (this T value, T expected) {
        NUnit.Framework.Assert.That(value, Is.EqualTo(expected));
        return value;
    }
}
