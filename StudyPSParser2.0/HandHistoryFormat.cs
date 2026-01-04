using System.Reflection;
using System.Collections.Immutable;

namespace StudyPSParser2._0;


public class 
HandHistory {
    public long HandId { get; }
    public IEnumerable<HandHistoryPlayer> Players { get; }
    
    public HandHistory(long handId, IEnumerable<HandHistoryPlayer> players) {
        HandId = handId;
        Players = players;
    }
}

public class 
HandHistoryPlayer {
    public int SeatNumber { get; } 
    public string NickName { get; } 
    public double StackSize { get; }
    public ImmutableList<Card> DealtCards { get; }
    
    public HandHistoryPlayer(int seatNumber, string nickName, double stackSize, ImmutableList<Card>? dealtCards = null) {
        SeatNumber = seatNumber;
        NickName = nickName;
        StackSize = stackSize;
        DealtCards = dealtCards ?? ImmutableList<Card>.Empty; //fixed by ai
    }
}

public class
Card {
    public CardRank Rank { get; }
    public Suit Suit { get; }
    public Card(CardRank rank, Suit suit) {
        Rank = rank;
        Suit = suit;
    }
}
public enum
CardRank
    {[Symbol("2")] Two = 2,
    [Symbol("3")] Three,
    [Symbol("4")] Four,
    [Symbol("5")] Five,
    [Symbol("6")] Six,
    [Symbol("7")] Seven,
    [Symbol("8")] Eight,
    [Symbol("9")] Nine,
    [Symbol("T")] Ten,
    [Symbol("J")] Jack,
    [Symbol("Q")] Queen,
    [Symbol("K")] King,
    [Symbol("A")] Ace
}
public enum 
Suit {
    [Symbol("♣")] Clubs,
    [Symbol("♦")] Diamonds,
    [Symbol("♥")] Hearts,
    [Symbol("♠")] Spades
}

// This function created by AI to fix Symbol error in the enums
[AttributeUsage(AttributeTargets.Field, Inherited = false, AllowMultiple = false)]
public sealed class SymbolAttribute : Attribute
{
    public string Symbol { get; }
    public SymbolAttribute(string symbol)=>Symbol = symbol;
}


