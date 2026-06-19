using System.Globalization;
using System.Text;
using CsvHelper;
using ThreeByThreeManager.Models;

namespace ThreeByThreeManager.Services;

public class ExportService
{
    private readonly IMatchService _matchService;
    private readonly IPlayerService _playerService;
    private readonly IStatsService _statsService;

    public ExportService(IMatchService matchService, IPlayerService playerService, IStatsService statsService)
    {
        _matchService = matchService;
        _playerService = playerService;
        _statsService = statsService;
    }

    public async Task<byte[]> ExportRosterCsvAsync()
    {
        var players = await _playerService.GetAllAsync();
        using var ms = new MemoryStream();
        using var writer = new StreamWriter(ms, Encoding.UTF8);
        using var csv = new CsvWriter(writer, CultureInfo.InvariantCulture);

        csv.WriteField("Nome"); csv.WriteField("Cognome"); csv.WriteField("Numero Maglia");
        csv.WriteField("Partite Giocate"); csv.WriteField("Punti Totali"); csv.WriteField("Media Punti");
        await csv.NextRecordAsync();

        foreach (var p in players)
        {
            var games = p.MatchStats.Select(s => s.MatchId).Distinct().Count();
            var pts = p.MatchStats.Sum(s => s.Points);
            var avg = games > 0 ? pts / (double)games : 0;
            csv.WriteField(p.FirstName); csv.WriteField(p.LastName); csv.WriteField(p.JerseyNumber.ToString());
            csv.WriteField(games.ToString()); csv.WriteField(pts.ToString()); csv.WriteField(avg.ToString("F1"));
            await csv.NextRecordAsync();
        }
        await writer.FlushAsync(); return ms.ToArray();
    }

    public async Task<byte[]> ExportMatchesCsvAsync()
    {
        var matches = await _matchService.GetAllAsync();
        using var ms = new MemoryStream();
        using var writer = new StreamWriter(ms, Encoding.UTF8);
        using var csv = new CsvWriter(writer, CultureInfo.InvariantCulture);

        csv.WriteField("Data"); csv.WriteField("Orario"); csv.WriteField("Campo"); csv.WriteField("Avversario");
        csv.WriteField("Tipo"); csv.WriteField("Stato"); csv.WriteField("Punti Fatti"); csv.WriteField("Punti Subiti"); csv.WriteField("Note");
        await csv.NextRecordAsync();

        foreach (var m in matches)
        {
            csv.WriteField(m.DateTime.ToString("dd/MM/yyyy")); csv.WriteField(m.DateTime.ToString("HH:mm"));
            csv.WriteField(m.Location); csv.WriteField(m.Opponent); csv.WriteField(m.TypeDisplay);
            csv.WriteField(m.StatusDisplay); csv.WriteField(m.OurPoints.ToString());
            csv.WriteField(m.TheirPoints.ToString()); csv.WriteField(m.Notes);
            await csv.NextRecordAsync();
        }
        await writer.FlushAsync(); return ms.ToArray();
    }

    public async Task<byte[]> ExportStatsCsvAsync()
    {
        var stats = await _statsService.GetAllStatsAsync();
        using var ms = new MemoryStream();
        using var writer = new StreamWriter(ms, Encoding.UTF8);
        using var csv = new CsvWriter(writer, CultureInfo.InvariantCulture);

        csv.WriteField("Giocatore"); csv.WriteField("Partita"); csv.WriteField("Punti"); csv.WriteField("Rimbalzi");
        csv.WriteField("Assist"); csv.WriteField("Recuperi"); csv.WriteField("Stoppate"); csv.WriteField("Falli"); csv.WriteField("Valutazione");
        await csv.NextRecordAsync();

        foreach (var s in stats)
        {
            csv.WriteField(s.Player.FullName); csv.WriteField($"{s.Match.Opponent} ({s.Match.DateTime:dd/MM})");
            csv.WriteField(s.Points.ToString()); csv.WriteField(s.Rebounds.ToString());
            csv.WriteField(s.Assists.ToString()); csv.WriteField(s.Steals.ToString());
            csv.WriteField(s.Blocks.ToString()); csv.WriteField(s.Fouls.ToString());
            csv.WriteField(s.Evaluation.ToString("F1"));
            await csv.NextRecordAsync();
        }
        await writer.FlushAsync(); return ms.ToArray();
    }

    public async Task<byte[]> ExportScorersCsvAsync()
    {
        var rankings = await _statsService.GetScorerRankingsAsync();
        using var ms = new MemoryStream();
        using var writer = new StreamWriter(ms, Encoding.UTF8);
        using var csv = new CsvWriter(writer, CultureInfo.InvariantCulture);

        csv.WriteField("Pos"); csv.WriteField("Giocatore"); csv.WriteField("Numero"); csv.WriteField("Punti"); csv.WriteField("Partite"); csv.WriteField("Media");
        await csv.NextRecordAsync();

        for (int i = 0; i < rankings.Count; i++)
        {
            csv.WriteField((i + 1).ToString()); csv.WriteField(rankings[i].PlayerName);
            csv.WriteField(rankings[i].JerseyNumber.ToString()); csv.WriteField(rankings[i].TotalPoints.ToString());
            csv.WriteField(rankings[i].GamesPlayed.ToString()); csv.WriteField(rankings[i].AvgPoints.ToString("F1"));
            await csv.NextRecordAsync();
        }
        await writer.FlushAsync(); return ms.ToArray();
    }
}
