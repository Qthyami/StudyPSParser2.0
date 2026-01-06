using StudyPSParser2._0.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace StudyPSParser2._0;

public static class HandHistoryFunctions {
    public static IEnumerable<Card>
    ParseDealtCards(this string cardsString) =>
        cardsString
            .SplitWords().Select(card => { ParseCards(card);
                return card.ParseCards();
            });
            
            public static Card ParseCards (this string CardsSting) {
                if (CardsSting.Length != 2)
                    throw new ArgumentException($"Invalid card string: {CardsSting}");

                var rank = CardsSting[0].ParseCardRank();
                var suit = CardsSting[1].ParseSuit();
                return new Card(rank, suit); 
            }
           
    public static CardRank
    ParseCardRank(this char symbol)=>
        symbol.ParseEnumBySymbol<CardRank>();
    
    public static Suit
    ParseSuit(this char symbol) {
        symbol = char.ToLower(symbol);
        return Enum.GetValues<Suit>().First(suit=>suit.GetSymbol()==symbol);
    }
}

