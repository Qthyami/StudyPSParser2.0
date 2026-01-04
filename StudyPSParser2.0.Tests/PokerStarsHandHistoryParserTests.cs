namespace StudyPSParser2._0.Tests;

[TestFixture]
public class PokerStarsHandHistoryParserTests
{
    [Test] public void 
    ParseHandId_ValidHand_ReturnsCorrectHandId() {
      
        var parser = new FluentParser(text);
        var handId = parser.ParseHandId();
        handId.Assert(93405882771);
    }

    [Test] public void 
    ParseSeatLine_ValidSeatLine_ParsesPlayerCorrectly() {
        var line = "Seat 3: angrypaca ($26.87 in chips)\n";
        var parser = new FluentParser(line);
        var player = parser.ParseSeatLine();
        player.SeatNumber
        .Assert(3);

    player.NickName
        .Assert("angrypaca");

    player.StackSize
        .Assert(26.87);
    }
    [Test]
    public void ParsePlayers_MultipleSeats_ParsesAllPlayers()
{
    var parser = new FluentParser(text);

    parser.ParseHandId();
    var players = parser.ParsePlayers().ToList();

    players.Count
        .Assert(6);
    // Player 0
    players[0].SeatNumber
        .Assert(1);
    players[0].NickName
        .Assert("VakaLuks");
    players[0].StackSize
        .Assert(26.87);
    // Player 1
    players[1].SeatNumber
        .Assert(2);
    players[1].NickName
        .Assert("BigBlindBets");
    players[1].StackSize
        .Assert(29.73);
    // Player 2
    players[2].SeatNumber
        .Assert(3);
    players[2].NickName
        .Assert("Jamol121");
    players[2].StackSize
        .Assert(17.66);
    // Player 3
    players[3].SeatNumber
        .Assert(4);
    players[3].NickName
        .Assert("ubbikk");
    players[3].StackSize
        .Assert(26.06);
    // Player 4
    players[4].SeatNumber
        .Assert(5);
    players[4].NickName
        .Assert("RicsiTheKid");
    players[4].StackSize
        .Assert(25.0);
    // Player 5
    players[5].SeatNumber
        .Assert(6);
    players[5].NickName
        .Assert("angrypaca");
    players[5].StackSize
        .Assert(26.89);
}
[Test]
public void ParseSingleHandHistory_Debug()
{
    var history = PokerStarsHandHistoryParser.ParseSingleHandHistory(text);

    history.HandId.Assert(93405882771L);
    history.Players.Count().Assert(6); // <-- используем Count() для IEnumerable

    var hero = history.Players.First(p => p.DealtCards.Count > 0);

    hero.SeatNumber.Assert(6);
    hero.NickName.Assert("angrypaca");
    hero.StackSize.Assert(26.89);

    hero.DealtCards.Count.Assert(2); // <-- ImmutableList имеет свойство Count

    hero.DealtCards[0].Rank.Assert(CardRank.Six);
    hero.DealtCards[0].Suit.Assert(Suit.Diamonds);

    hero.DealtCards[1].Rank.Assert(CardRank.Ace);
    hero.DealtCards[1].Suit.Assert(Suit.Spades);
}

    [Test] public void 
    ParseDealtCards_ValidHoleCards_ParsesHeroAndCards()
    {
        var parser = new FluentParser(text);
        var (heroNick, cards) = parser.ParseDealtCards();
        heroNick.Assert("angrypaca");
        cards.Assert("6d As");
    }

    [Test]
    public void ParseDealtCards_TwoCards_BothFormatsParsedCorrectly()
{       var cardsString = "As 4h";   
        var cards = cardsString.ParseDealtCards();

        Assert.Multiple(() =>
        {
            cards.Count.Assert(2);
            cards[0].Rank.Assert(CardRank.Ace);
            cards[0].Suit.Assert(Suit.Spades);
            cards[1].Rank.Assert(CardRank.Four);
            cards[1].Suit.Assert(Suit.Hearts);
        });
    
}

    public string text = """
PokerStars Hand #93405882771:  Hold'em No Limit ($0.10/$0.25 USD) - 2013/02/03 1:16:19 EET [2013/02/02 18:16:19 ET]
Table 'Stobbe III' 6-max Seat #4 is the button
Seat 1: VakaLuks ($26.87 in chips) 
Seat 2: BigBlindBets ($29.73 in chips) 
Seat 3: Jamol121 ($17.66 in chips) 
Seat 4: ubbikk ($26.06 in chips) 
Seat 5: RicsiTheKid ($25 in chips) 
Seat 6: angrypaca ($26.89 in chips) 
RicsiTheKid: posts small blind $0.10
angrypaca: posts big blind $0.25
*** HOLE CARDS ***
Dealt to angrypaca [6d As]
VakaLuks: folds 
BigBlindBets: folds 
Jamol121: calls $0.25
ubbikk: folds 
RicsiTheKid: folds 
angrypaca: checks 
*** FLOP *** [5s Qs 3c]
angrypaca: checks 
Jamol121: checks 
*** TURN *** [5s Qs 3c] [8d]
angrypaca: checks 
Jamol121: bets $0.25
angrypaca: folds 
Uncalled bet ($0.25) returned to Jamol121
Jamol121 collected $0.57 from pot
*** SUMMARY ***
Total pot $0.60 | Rake $0.03 
Board [5s Qs 3c 8d]
Seat 1: VakaLuks folded before Flop (didn't bet)
Seat 2: BigBlindBets folded before Flop (didn't bet)
Seat 3: Jamol121 collected ($0.57)
Seat 4: ubbikk (button) folded before Flop (didn't bet)
Seat 5: RicsiTheKid (small blind) folded before Flop
Seat 6: angrypaca (big blind) folded on the Turn
""";
}




