
namespace StudyPSParser2._0;

public static class CardParsingHelper {
    public static ImmutableList<Card>
    ParseDealtCards(this string cardsString) =>
        cardsString
            .Split(' ', StringSplitOptions.RemoveEmptyEntries)
            .Select(card => {
                if (card.Length != 2)
                    throw new ArgumentException($"Invalid card string: {card}");
                var rank = card[0].ToString().ToUpper().ParseCardRank();
                var suit = card[1].ToString().ToUpper().ParseSuit();
                return new Card(rank, suit);
            })
            .ToImmutableList();


    public static CardRank
    ParseCardRank(this string symbol) {
        switch (symbol) {
            case "2": return CardRank.Two;
            case "3": return CardRank.Three;
            case "4": return CardRank.Four;
            case "5": return CardRank.Five;
            case "6": return CardRank.Six;
            case "7": return CardRank.Seven;
            case "8": return CardRank.Eight;
            case "9": return CardRank.Nine;
            case "T": return CardRank.Ten;
            case "J": return CardRank.Jack;
            case "Q": return CardRank.Queen;
            case "K": return CardRank.King;
            case "A": return CardRank.Ace;
            default: throw new ArgumentException("Invalid card rank symbol");
        }
    }

    public static Suit ParseSuit(this string symbol) {
        switch (symbol)
        {
            case "C": return Suit.Clubs;
            case "D": return Suit.Diamonds;
            case "H": return Suit.Hearts;
            case "S": return Suit.Spades;
            default: throw new ArgumentException("Invalid suit symbol");
        }
    }
}

