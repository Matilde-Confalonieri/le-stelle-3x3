# Pro Circuit 3x3 — Basketball Manager

Applicazione web Blazor Server (.NET 9) per la gestione di una squadra di basket 3x3 e del torneo.

## Prerequisiti

- [.NET 9 SDK](https://dotnet.microsoft.com/download/dotnet/9.0)
- Visual Studio 2022 o 2025 (opzionale, qualsiasi editor va bene)
- EF Core Tools: `dotnet tool install --global dotnet-ef` (se non già installato)

## Avvio Rapido

```bash
# 1. Entra nella cartella del progetto
cd ThreeByThreeManager

# 2. Ripristina i pacchetti NuGet
dotnet restore

# 3. Avvia l'applicazione
dotnet run
```

Il database SQLite (`tournament.db`) viene creato automaticamente al primo avvio con dati di esempio.

L'applicazione sarà disponibile su `https://localhost:5001` e `http://localhost:5000`.

## Struttura del Progetto

```
ThreeByThreeManager/
├── Models/                    # Entità del dominio
│   ├── Enums.cs               # Enum (MatchType, MatchStatus, SchemeCategory)
│   ├── Player.cs              # Giocatore
│   ├── Match.cs               # Partita
│   ├── PlayerMatchStats.cs    # Statistiche giocatore per partita
│   └── PlaybookScheme.cs      # Schema di gioco
├── Data/
│   ├── ApplicationDbContext.cs # DbContext EF Core (SQLite)
│   └── DbSeeder.cs            # Seed iniziale dati demo
├── Services/                  # Business logic
│   ├── IMatchService.cs / MatchService.cs
│   ├── IPlayerService.cs / PlayerService.cs
│   ├── IStatsService.cs / StatsService.cs
│   ├── IPlaybookService.cs / PlaybookService.cs
│   └── ExportService.cs       # Esportazione CSV
├── Components/
│   ├── Shared/                # Layout condivisi
│   │   ├── MainLayout.razor   # Layout principale con sidebar
│   │   ├── NavMenu.razor      # Menu di navigazione
│   │   ├── GlobalSearch.razor # Barra di ricerca globale
│   │   └── ExportDropdown.razor # Dropdown esportazione CSV
│   └── Pages/                 # Pagine dell'applicazione
│       ├── Home.razor         # Dashboard
│       ├── Matches.razor      # Gestione partite
│       ├── UpdateStats.razor  # Inserimento statistiche
│       ├── Roster.razor       # Gestione giocatori
│       ├── Statistics.razor   # Statistiche e classifiche
│       ├── Playbook.razor     # Schemi di gioco
│       ├── Rules.razor        # Regole FIBA 3x3
│       ├── Tournament.razor   # Classifica torneo
│       ├── Scorers.razor      # Classifica marcatori
│       ├── Calendar.razor     # Calendario mensile
│       └── TeamRecords.razor  # Record di squadra
├── wwwroot/
│   ├── css/site.css           # CSS design system Pro-Circuit 3x3
│   ├── js/site.js             # Chart.js e utility JS
│   └── uploads/               # Upload immagini schemi
├── Program.cs                 # Configurazione app e DI
├── App.razor                  # Root component
├── Routes.razor               # Router Blazor
└── _Imports.razor             # Namespace globali
```

## Funzionalità

| Pagina | Descrizione |
|--------|-------------|
| **Dashboard** | Card riepilogo, prossima partita, risultati recenti, grafico andamento, top scorer |
| **Partite** | CRUD partite, aggiornamento automatico stato con punteggi |
| **Aggiorna Stats** | Inserimento statistiche giocatore per partita (punti, rimbalzi, assist, etc.) |
| **Roster** | CRUD giocatori, card con statistiche riepilogative |
| **Statistiche** | Filtri per giocatore/partita, classifiche totali, grafici |
| **Schemi** | Archivio schemi con upload immagini, ricerca e filtri per categoria |
| **Regole** | Regolamento FIBA 3x3 completo pre-compilato |
| **Classifica** | Classifica torneo calcolata automaticamente, grafici vittorie/sconfitte |
| **Marcatori** | Classifica marcatori con medie e valutazioni |
| **Calendario** | Vista mensile interattiva con partite del giorno |
| **Record** | Miglior vittoria, peggior sconfitta, ultime 5 partite, top performers |

### Extra

- **MVP Automatico**: calcolato con valutazione = PTS + AST + RIM + REC + STP - FAL
- **Grafici Chart.js**: andamento squadra, distribuzione statistiche, vittorie/sconfitte
- **Ricerca Globale**: cerca giocatori, partite, schemi
- **Esportazione CSV**: roster, calendario, statistiche, classifica marcatori
- **Responsive**: sidebar desktop, bottom nav mobile

## Design System — Pro-Circuit 3x3

Basato sul design system urban-athletic:
- **Primary**: Safety Orange `#ab3600`
- **Secondary**: Dark Charcoal `#585e6f`
- **Tertiary**: Neon Yellow `#506600` (MVP/accenti)
- **Headlines**: Archivo Narrow (700-800 weight)
- **Body**: Inter (400-600)
- **Labels/Mono**: Space Mono (700)

## Database

Il database SQLite viene creato automaticamente in `tournament.db` nella root del progetto. Per ricrearlo da zero, elimina il file e riavvia l'applicazione.

## Pubblicazione

```bash
dotnet publish -c Release -o publish
```

Poi copia la cartella `publish` sul server e avvia con:

```bash
dotnet ThreeByThreeManager.dll
```

## Estensione Futura

Il progetto è strutturato per accogliere l'autenticazione:
- Aggiungere `Microsoft.AspNetCore.Identity` al progetto
- Aggiungere `IdentityDbContext` o estendere l'`ApplicationDbContext`
- Proteggere le pagine con `[Authorize]`
- I servizi sono già iniettati via DI e facilmente testabili
