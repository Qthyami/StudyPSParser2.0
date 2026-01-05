
namespace StudyPSParser2._0;

public static class
PokerStarsHandHistoryParser {
    public static HandHistory
    ParseSingleHandHistory(this string handHistoryText) {
        var parser = new FluentParser(handHistoryText);
        var handId = parser.ParseHandId();
        var players = parser.ParsePlayers().ToImmutableList();
        var (heroNick, dealtCards) = parser.ParseHeroAndCards();

        return new HandHistory(
            handId:handId,
            players: [..players.Select(player =>
                player.NickName == heroNick
                ? new HandHistoryPlayer(
                    seatNumber: player.SeatNumber,
                    nickName:player.NickName ,
                    stackSize:player.StackSize ,
                    dealtCards:dealtCards.ParseDealtCards()
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
    ParsePlayers(this FluentParser parser) {
        if (!parser.TrySkipUntil("\nSeat "))
            yield break;
        parser.Skip("#".Length);
        while (TryParseNextSeat(parser, out var player)) {
            yield return player;
        }
    }

    public static HandHistoryPlayer
    ParseSeatLine(this FluentParser parser) {
        var seatNumber = parser.ReadSeatNumber();
        var nickName = parser.ReadPlayerNick();
        var stackSize = parser.ReadPlayerStack();
        return new HandHistoryPlayer(
            seatNumber: seatNumber,
            nickName: nickName,
            stackSize: stackSize,
            dealtCards: ImmutableList<Card>.Empty // TODO to be filled later
            );
          }

    public static (string heroNickName, string cards)
    ParseHeroAndCards(this FluentParser parser) {
        parser.SkipToHoleCards();
        var heroNickName = parser.ReadHeroNick();
        var dealtCards = parser.ReadHeroCards();
        return (heroNickName: heroNickName, cards: dealtCards);
    }

    //Extensions method for ParsePlayers()
    private static bool
    TryParseNextSeat(this FluentParser parser, out HandHistoryPlayer result) {
        result = default;
        if (!parser.Next("Seat "))
            return false;
        var lookahead = parser.Clone();
        if (!lookahead.TryReadUntil('\n', int.MaxValue, out var line) || !line.Contains("in chips"))
            return false;

        result = ParseSeatLine(parser);
        if (parser.HasNext)
            parser.SkipUntilNextLine();
        return true;
    }

    //extensions methods for ParseHeroAndCards()
    private static void
    SkipToHoleCards(this FluentParser parser) {
        if (!parser.TrySkipUntil("*** HOLE CARDS ***"))
            throw new FormatException("HOLE CARDS section not found");
        parser.Skip("*** HOLE CARDS ***".Length).SkipUntilNextLine();
    }

    private static string
    ReadHeroNick(this FluentParser parser) {
        parser.VerifyNext("Dealt to ").Skip("Dealt to ".Length);
        if (!parser.TryReadWord(out var heroNick))
            throw new FormatException("Failed to read hero nickname");
        return heroNick;
    }

    private static string
    ReadHeroCards(this FluentParser parser) {
        parser.SkipSpaces().VerifyNext("[").Skip(1);
        return parser.ReadUntil(']');
    }
    //Extensions methods for ParseSeatLine()
    private static int
    ReadSeatNumber(this FluentParser parser) {
        parser.VerifyNext("Seat ").Skip("Seat ".Length);
        return parser.ReadInt();
    }

    private static string
    ReadPlayerNick(this FluentParser parser) {
        parser.VerifyNext(": ").Skip(2).SkipSpaces();
        if (!parser.TryReadWord(out var nickName))
            throw new FormatException("Failed to read player's nickname");
        return nickName;
    }

    private static double
    ReadPlayerStack(this FluentParser parser) {
        parser.SkipSpaces().VerifyNext("($").Skip(2);
        return parser.ReadDouble();
    }

}
