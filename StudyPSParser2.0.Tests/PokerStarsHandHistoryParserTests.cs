namespace StudyPSParser2._0.Tests;

[TestFixture]
public class PokerStarsHandHistoryParserTests
{
    [Test] public void ParseHandId_ValidHand_ReturnsCorrectHandId() {
      
        var parser = new FluentParser(text);

        var handId = PokerStarsHandHistoryParser.ParseHandId(parser);

        TestContext.Out.WriteLine($"HandId = {handId}");

        Assert.That(handId, Is.EqualTo(93405882771L));
    }

    [Test]    public void ParseSeatLine_ValidSeatLine_ParsesPlayerCorrectly() {
        var line = "Seat 3: angrypaca ($26.87 in chips)\n";
        var parser = new FluentParser(line);
        var player = PokerStarsHandHistoryParser.ParseSeatLine(parser);
        TestContext.Out.WriteLine(
            $"Seat={player.SeatNumber}, Nick={player.NickName}, Stack={player.StackSize}"
        );

        Assert.Multiple(() =>
        {
            Assert.That(player.SeatNumber, Is.EqualTo(3));
            Assert.That(player.NickName, Is.EqualTo("angrypaca"));
            Assert.That(player.StackSize, Is.EqualTo(26.87));
        });
    }

    [Test]
public void ParsePlayers_MultipleSeats_ParsesAllPlayers()
{
    var parser = new FluentParser(text);

    parser.ParseHandId();
    var players = parser.ParsePlayers().ToList();

    TestContext.Out.WriteLine($"Players count = {players.Count}");
    foreach ( var p in players)
         TestContext.Out.WriteLine(
            $"{p.SeatNumber} | {p.NickName} | {p.StackSize} | {p.DealtCards}");

    Assert.That(players.Count, Is.EqualTo(6));

    Assert.Multiple(() =>
    {
        Assert.That(players[0].NickName, Is.EqualTo("VakaLuks"));
        Assert.That(players[0].SeatNumber, Is.EqualTo(1));
        Assert.That(players[0].StackSize, Is.EqualTo(26.87));

        Assert.That(players[1].NickName, Is.EqualTo("BigBlindBets"));
        Assert.That(players[2].NickName, Is.EqualTo("Jamol121"));
        Assert.That(players[3].NickName, Is.EqualTo("ubbikk"));
        Assert.That(players[4].NickName, Is.EqualTo("RicsiTheKid"));

        Assert.That(players[5].NickName, Is.EqualTo("angrypaca"));
        Assert.That(players[5].SeatNumber, Is.EqualTo(6));
        Assert.That(players[5].StackSize, Is.EqualTo(26.89));
    });
}


    [Test]
public void ParseSingleHandHistory_Debug() 
{
    
    var history = PokerStarsHandHistoryParser.ParseSingleHandHistory(text);

    TestContext.Out.WriteLine($"HandId: {history.HandId}");
    foreach (var p  in history.Players)
        TestContext.Out.WriteLine($"{p.SeatNumber} | {p.NickName} | {p.StackSize} | {p.DealtCards}");
 }

    [Test]
    public void ParseDealtCards_ValidHoleCards_ParsesHeroAndCards()
    {
      
        var parser = new FluentParser(text);

        var (heroNick, cards) = PokerStarsHandHistoryParser.ParseDealtCards(parser);

        TestContext.Out.WriteLine($"Hero:{heroNick}, Cards:{cards}");

        Assert.Multiple(() =>
        {
            Assert.That(heroNick, Is.EqualTo("angrypaca"));
            Assert.That(cards, Is.EqualTo("6d As"));
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



