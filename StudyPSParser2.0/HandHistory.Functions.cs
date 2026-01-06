using StudyPSParser2._0.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace StudyPSParser2._0;

public static class HandHistoryFunctions {
    public static ImmutableList<Card>
    ParseDealtCards(this string cardsString) =>
        cardsString
            .Split(' ', StringSplitOptions.RemoveEmptyEntries)
            .Select(card => {
                if (card.Length != 2)
                    throw new ArgumentException($"Invalid card string: {card}");

                var rank = char.ToUpper(card[0]).ParseCardRank();
                var suit = char.ToLower(card[1]).ParseSuit();
                return new Card(rank, suit);
            })
            .ToImmutableList();

    public static CardRank
    ParseCardRank(this char symbol)=>
        symbol.ParseEnumBySymbol<CardRank>();
    //    symbol = char.ToUpper(symbol);
    //return Enum.GetValues<CardRank>().First(rank=>rank.GetSymbol()==symbol);
    

    public static Suit
    ParseSuit(this char symbol) {
        symbol = char.ToLower(symbol);
        return Enum.GetValues<Suit>().First(suit=>suit.GetSymbol()==symbol);
    
    }
}

