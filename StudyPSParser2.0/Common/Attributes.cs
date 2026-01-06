using System;
using System.Collections.Generic;
using System.Text;

namespace StudyPSParser2._0.Common;

public class
SymbolAttribute : Attribute {
    public char Value { get; }
    public SymbolAttribute(char symbol) => Value = symbol;
}
public static class Attributes {
    public static char
    GetSymbol(this CardRank rank) =>
    rank.GetAttribute<SymbolAttribute>()!.Value;
    
    public static char
    GetSymbol(this Suit suit) =>
    suit.GetAttribute<SymbolAttribute>()!.Value;

    public static TEnum 
    ParseEnumBySymbol<TEnum>(this char symbol) where TEnum : Enum {
   foreach (TEnum value in Enum.GetValues(typeof(TEnum))) {
        var attr = (value as Enum)?.GetAttribute<SymbolAttribute>();
        if (attr != null && attr.Value == symbol)
            return value;
    }
    throw new ArgumentException($"No {typeof(TEnum).Name} with symbol '{symbol}' found.");
}
}







