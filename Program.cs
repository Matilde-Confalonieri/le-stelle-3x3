using System.Data;
using Microsoft.EntityFrameworkCore;
using ThreeByThreeManager.Data;
using ThreeByThreeManager.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents()
    .AddHubOptions(options =>
    {
        options.MaximumReceiveMessageSize = 10 * 1024 * 1024;
    });

var connectionString = Environment.GetEnvironmentVariable("DATABASE_URL") ?? 
    builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContextFactory<ApplicationDbContext>(options =>
    options.UseNpgsql(connectionString));

builder.Services.AddScoped<IMatchService, MatchService>();
builder.Services.AddScoped<IPlayerService, PlayerService>();
builder.Services.AddScoped<IStatsService, StatsService>();
builder.Services.AddScoped<IPlaybookService, PlaybookService>();
builder.Services.AddScoped<ExportService>();

var app = builder.Build();

var forceReseed = string.Equals(
    Environment.GetEnvironmentVariable("RESEED"),
    "true",
    StringComparison.OrdinalIgnoreCase);

using (var scope = app.Services.CreateScope())
{
    var factory = scope.ServiceProvider.GetRequiredService<IDbContextFactory<ApplicationDbContext>>();
    await using var context = await factory.CreateDbContextAsync();

    await context.Database.EnsureCreatedAsync();

    // Rileva drift dello schema (es. nuova tabella "Shots"/"SeedInfos" aggiunta dopo un deploy precedente)
    // e, se serve, ricrea lo schema completo. I dati sono demo e vengono rigenerati dal seed.
    if (forceReseed || !await TableExistsAsync(context, "Shots") || !await TableExistsAsync(context, "SeedInfos"))
    {
        await RecreateSchemaAsync(context);
    }

    await DbSeeder.SeedAsync(context, forceReseed);
}

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<ThreeByThreeManager.App>()
    .AddInteractiveServerRenderMode();

app.Run();

static async Task<bool> TableExistsAsync(ApplicationDbContext context, string table)
{
    var conn = context.Database.GetDbConnection();
    var wasOpen = conn.State == ConnectionState.Open;
    if (!wasOpen) await conn.OpenAsync();
    try
    {
        await using var cmd = conn.CreateCommand();
        cmd.CommandText = "SELECT COUNT(*) FROM information_schema.tables WHERE table_schema = 'public' AND lower(table_name) = lower(@p)";
        var p = cmd.CreateParameter();
        p.ParameterName = "p";
        p.Value = table;
        cmd.Parameters.Add(p);
        var result = await cmd.ExecuteScalarAsync();
        return Convert.ToInt64(result) > 0;
    }
    finally
    {
        if (!wasOpen) await conn.CloseAsync();
    }
}

static async Task RecreateSchemaAsync(ApplicationDbContext context)
{
    const string dropSql = """
        DROP TABLE IF EXISTS "SeedInfos" CASCADE;
        DROP TABLE IF EXISTS "Shots" CASCADE;
        DROP TABLE IF EXISTS "PlayerMatchStats" CASCADE;
        DROP TABLE IF EXISTS "Matches" CASCADE;
        DROP TABLE IF EXISTS "PlaybookSchemes" CASCADE;
        DROP TABLE IF EXISTS "Players" CASCADE;
        """;
    await context.Database.ExecuteSqlRawAsync(dropSql);
    var createScript = context.Database.GenerateCreateScript();
    await context.Database.ExecuteSqlRawAsync(createScript);
}
