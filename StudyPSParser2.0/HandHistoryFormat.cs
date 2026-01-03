using System.Collections.Immutable;

namespace StudyPSParser2._0;

public class 
HandHistory {
    public long HandId { get; }
    public ImmutableList<HandHistoryPlayer> Players { get; }
    
    public HandHistory(long handId, ImmutableList<HandHistoryPlayer> players) 
    {
        HandId = handId;
        Players = players;
    }
}

public class 
HandHistoryPlayer {
    public int SeatNumber { get; } 
    public string NickName { get; } 
    public double StackSize { get; }
    public string DealtCards { get; }
    
    public HandHistoryPlayer(int seatNumber, string nickName, double stackSize, string dealtCards) 
    {
        SeatNumber = seatNumber;
        NickName = nickName;
        StackSize = stackSize;
        DealtCards = dealtCards;
    }
}
