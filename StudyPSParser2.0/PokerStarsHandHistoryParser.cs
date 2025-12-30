using System.Collections.Immutable;
using System.Globalization;
using static StudyPSParser2._0.HandHistoryFormat;

namespace StudyPSParser2._0;
public static class PokerStarsHandHistoryParser
{
    public static   //possible to refactor into chaining, but less readable
    HandHistoryFormat.HandHistory ParseSingleHandHistory(string handHistoryText) {
        var parser = new FluentParser(handHistoryText);
        var handId = ParseHandId(parser);
        var players = ParsePlayers(parser);
        var (heroNick, DealtCards) = ParseDealtCards(parser);
        players = players.Select(player =>
            player.NickName == heroNick
            ? new PlayerHistory(
                player.SeatNumber,
                player.NickName,
                player.StackSize,
                DealtCards)
            : player).ToImmutableList();
        return new HandHistoryFormat.HandHistory(handId, players);
    }

    public static long 
    ParseHandId(this FluentParser parser)
    {
        if (!parser.TrySkipUntil("PokerStars Hand #"))
            throw new FormatException("Not a valid PokerStars hand history.");
            parser.Skip("PokerStars Hand #".Length);
        return parser.ReadLong();
    }
            
    public static ImmutableList<HandHistoryFormat.PlayerHistory>
    ParsePlayers(this FluentParser parser)  {
            var playersBuilder = ImmutableList.CreateBuilder<HandHistoryFormat.PlayerHistory>();     
            if (!parser.TrySkipUntil("\nSeat "))
            return playersBuilder.ToImmutable();        
            parser.Skip(1);      
            while (TryParseNextSeat (parser, playersBuilder)) //in cycle rolling TryParseNextSeat - extension method
            { }              
            return playersBuilder.ToImmutable();      
    }

    public static PlayerHistory 
    ParseSeatLine(this FluentParser parser) {
        parser.VerifyNext("Seat "). Skip("Seat ".Length);  
        var seat=parser.ReadInt();
        parser.VerifyNext(": ")
            .Skip(2)
            .SkipSpaces()
            .TryReadWord(out var nickName)
        
;        
        parser.SkipSpaces()
            .VerifyNext("($")
            .Skip(2);
        var stackSize=parser.ReadDouble(CultureInfo.InvariantCulture);
        parser.SkipSpaces()            
            .Skip("in chips".Length);        
        return new PlayerHistory(seat,nickName,stackSize,dealtCards:"");
    }

    public static (string HeroNick, string Cards)
    ParseDealtCards(this FluentParser parser) {
        if (!parser.TrySkipUntil("*** HOLE CARDS ***")) //slow perfomance of TrySkipUntil, can be refactored, but more code
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

    static bool //extension method for ParsePlayers
    TryParseNextSeat(this FluentParser parser,ImmutableList<HandHistoryFormat.PlayerHistory>.Builder players){
        if (!parser.Next("Seat "))
        return false;
        var lookahead = parser.Clone();
        if (!lookahead.TryReadUntil('\n', int.MaxValue, out var line) ||!line.Contains("in chips"))
            return false;

    var player = ParseSeatLine(parser);
    players.Add(player);

    if (parser.HasNext)
        parser.SkipUntilNextLine();
        return true;
    }
}

