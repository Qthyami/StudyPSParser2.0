using System.Collections.Immutable;

namespace StudyPSParser2._0;

public class HandHistoryFormat {
    public class
    HandHistory {
        public long HandId { get; }
        public ImmutableList<PlayerHistory> Players { get; }
        public HandHistory(long handId, ImmutableList<PlayerHistory> players) {
            HandId = handId;
            Players = players;
        }
    }
    
 public class PlayerHistory {
    public int SeatNumber { get; } 
    public string NickName { get; } 
    public double StackSize { get; }
    public string DealtCards { get; }
    public PlayerHistory(int seatNumber, string nickName, double stackSize, string dealtCards) {
        SeatNumber = seatNumber;
        NickName = nickName;
        StackSize = stackSize;
        DealtCards = dealtCards;
    }
    }
}
