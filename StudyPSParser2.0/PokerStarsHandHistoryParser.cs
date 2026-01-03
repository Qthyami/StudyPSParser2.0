
using System.Globalization;

namespace StudyPSParser2._0;

public static class PokerStarsHandHistoryParser
{
    public static HandHistory 
    ParseSingleHandHistory(string handHistoryText) {
        var parser = new FluentParser(handHistoryText);
        var handId = parser.ParseHandId();
        var players = parser.ParsePlayers().ToImmutableList();
        var (heroNick, dealtCards) = parser.ParseDealtCards();
      
        return new HandHistory(
            handId, 
            players:[..players.Select(player =>
            player.NickName == heroNick
            ? new HandHistoryPlayer(
                seatNumber: player.SeatNumber,
                nickName:player.NickName , 
                stackSize:player.StackSize , 
                dealtCards:dealtCards 
                )
            : player)]);
    }

    public static long 
    ParseHandId(this FluentParser parser) {
        if (!parser.TrySkipUntil("PokerStars Hand #"))
            throw new FormatException("Not a valid PokerStars hand history.");
        return parser.Skip("PokerStars Hand #".Length).ReadLong();
         
    }
            
    public static IEnumerable<HandHistoryPlayer>
    ParsePlayers(this FluentParser parser)  {
        if (!parser.TrySkipUntil("\nSeat "))
            yield break;
        parser.Skip("#".Length);
        while (TryParseNextSeat(parser, out var player)) {
            yield return player;
        }
    }

    public static HandHistoryPlayer 
    ParseSeatLine(this FluentParser parser) {
        parser.VerifyNext("Seat "). Skip("Seat ".Length);  
        var seat=parser.ReadInt();
        parser.VerifyNext(": ")
            .Skip(2)
            .SkipSpaces()
            .TryReadWord(out var nickName);
       
        parser.SkipSpaces()
            .VerifyNext("($")
            .Skip(2);
        var stackSize=parser.ReadDouble();
        
        return new HandHistoryPlayer(seat,nickName,stackSize,dealtCards:"");
    }

    public static (string heroNick, string cards)
    ParseDealtCards(this FluentParser parser) {
        if (!parser.TrySkipUntil("*** HOLE CARDS ***"))
            throw new FormatException("HOLE CARDS section not found");        
        parser.Skip("*** HOLE CARDS ***".Length)
            .SkipUntilNextLine()
            .VerifyNext("Dealt to ")
            .Skip("Dealt to ".Length);
            if (!parser.TryReadWord(out var heroNickName))
            throw new FormatException("Failed to read hero nickname");
        parser.SkipSpaces()
            .VerifyNext("[")
            .Skip(1);
        var dealtCards = parser.ReadUntil(']');
        parser.Skip(1); 
        return (heroNickName, dealtCards);  
    }

    static bool
    TryParseNextSeat(this FluentParser parser,out HandHistoryPlayer result){
        result = default;
        if (!parser.Next("Seat "))
            return false;
        var lookahead = parser.Clone();
        if (!lookahead.TryReadUntil('\n', int.MaxValue, out var line) ||!line.Contains("in chips"))
            return false;

   result = ParseSeatLine(parser);
   

    if (parser.HasNext)
        parser.SkipUntilNextLine();
        return true;
    }
}

