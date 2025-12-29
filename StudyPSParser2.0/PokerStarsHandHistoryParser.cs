using System.Collections.Immutable;
using System.Globalization;
using System.IO.Pipelines;
using static StudyPSParser2._0.HandHistoryFormat;

namespace StudyPSParser2._0;

public class PokerStarsHandHistoryParser
{
    public static HandHistoryFormat.HandHistory ParseSingleHandHistory(string handHistoryText) {
        var parser = new FluentParser(handHistoryText);
        var HandId = ParseHandId(parser);
        var players = ParsePlayers(parser);
        return new HandHistoryFormat.HandHistory(HandId, players);
    }

    public static long ParseHandId(FluentParser parser)
    {
        if (!parser.TrySkipUntil("PokerStars Hand #"))
            throw new FormatException("Not a valid PokerStars hand history.");
        else
        {
            parser.Skip("PokerStars Hand #".Length);
        }
        var startPosition = parser.Position;
        while (parser.HasCurrent && char.IsDigit(parser.NextChar))
        {
            parser = parser.SkipOne();
        }
        var handIdString = parser.String.Substring(startPosition, parser.Position - startPosition);
        return StringToNumberParser.TryParseCustom.ParseNumber<long>(handIdString);
    }

    // FIX: Use ImmutableList<T>.Builder for mutable list construction
    public static ImmutableList<HandHistoryFormat.PlayerHistory> ParsePlayers(FluentParser parser)
    {
        var playersBuilder = ImmutableList.CreateBuilder<HandHistoryFormat.PlayerHistory>();
       parser.SkipAfter('\n');
       while (parser.Next("Seat ")) {
            var player = ParseSeatLine(parser);
            playersBuilder.Add(player);
            if (!parser.HasNext)
                break;
            parser.SkipAfter('\n');            

        }
       return playersBuilder.ToImmutable();
      
    }

    

    static PlayerHistory ParseSeatLine(FluentParser parser) {
        parser.VerifyNext("Seat "). Skip("Seat ".Length);  
        var seat=parser.ReadInt();
        parser.VerifyNext(": ")
            .Skip(2)
            .SkipSpaces();
        var nickName = parser.TryReadWord(out var nick)
        ? nick
        : throw new FormatException("failed to read player's nickname");
        
        parser.SkipSpaces()
            .VerifyNext("($")
            .Skip(2);
        var stackSize=parser.ReadDouble(CultureInfo.InvariantCulture);
        parser.SkipSpaces()
            .VerifyNext("in chips")
            .Skip("in chips".Length);

        
        return new PlayerHistory(seat,nickName,stackSize,dealtCards:"");

    }
}

