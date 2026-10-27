# Pensieve

Save the parts of videos that matter. Come back to them. Learn from them.

Pensieve is a personal learning companion built on top of YouTube: it remembers where you stopped, lets you mark useful moments, and plays them back as a short reel. It stores references to videos, never the videos themselves.

Read [MANIFEST.md](MANIFEST.md) for the problem, principles, vocabulary and roadmap.

## Layout

| Path | What it is |
|---|---|
| `src/Pensieve.Api` | ASP.NET Core minimal API. Also serves the built frontend from `wwwroot`. |
| `src/Pensieve.Web` | Svelte + Vite + TypeScript frontend. |
| `tests/Pensieve.Api.Tests` | xUnit tests for the API. |
| `openspec/` | Specs and change proposals (see [AGENTS.md](AGENTS.md)). |

## Requirements

- .NET SDK 10 (`global.json` pins the minimum)
- Node.js 22 or newer

## Run it

Development, with hot reload for the frontend (two terminals):

```bash
dotnet run --project src/Pensieve.Api
```

```bash
cd src/Pensieve.Web
npm install
npm run dev
```

Open http://localhost:5173. The Vite dev server forwards `/api` calls to the API on port 5141.

Single process, as it would run when deployed:

```bash
cd src/Pensieve.Web && npm run build && cd ../..
dotnet run --project src/Pensieve.Api
```

Open http://localhost:5141.

## Test and check

```bash
dotnet test
```

```bash
cd src/Pensieve.Web && npm run check
```
