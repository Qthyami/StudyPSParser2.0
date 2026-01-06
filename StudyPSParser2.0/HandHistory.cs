namespace StudyPSParser2._0;
public class
HandHistory {
    public long HandId { get; }
    public ImmutableList<HandHistoryPlayer> Players { get; }
    public HandHistory(long handId, ImmutableList<HandHistoryPlayer> players) {
        HandId = handId;
        Players = players;
    }
    public bool TryGetHeroPlayer(out HandHistoryPlayer heroPlayer) {
        heroPlayer = Players.First(player => player.DealtCards.Count > 0);    
        return true;
        }
  }

public class
HandHistoryPlayer {
    public int SeatNumber { get; }
    public string Nickname { get; }
    public double StackSize { get; }
    public ImmutableList<Card> DealtCards { get; }
    public HandHistoryPlayer(int seatNumber, string nickName, double stackSize, ImmutableList<Card>dealtCards) {
        SeatNumber = seatNumber;
        Nickname = nickName;
        StackSize = stackSize;
        DealtCards = dealtCards;
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








