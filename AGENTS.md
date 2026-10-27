# Pensieve - agent guide

Pensieve is a hobby learning companion built on YouTube: remember where I stopped, mark useful moments of videos, replay them as a reel, group them into plans. Read [MANIFEST.md](MANIFEST.md) first. It holds the problem, principles, vocabulary and roadmap. Do not duplicate it here.

## Layout

- `src/Pensieve.Api` - ASP.NET Core minimal API (.NET 10). Serves the built frontend from `wwwroot`.
- `src/Pensieve.Web` - Svelte 5 + Vite + TypeScript. `npm run build` writes into `src/Pensieve.Api/wwwroot` (git-ignored).
- `tests/Pensieve.Api.Tests` - xUnit, with `WebApplicationFactory<Program>`.
- `openspec/` - specs (`openspec/specs`) and change proposals (`openspec/changes`).

## Commands

```bash
dotnet run --project src/Pensieve.Api        # API on http://localhost:5141
dotnet test                                   # all .NET tests
cd src/Pensieve.Web && npm run dev            # frontend on :5173, proxies /api to :5141
cd src/Pensieve.Web && npm run check          # svelte-check + tsc
cd src/Pensieve.Web && npm run build          # build into the API's wwwroot
```

Run `dotnet test` and `npm run check` before calling work done.

## How we work

- **Spec first, one slice at a time.** Use OpenSpec (`/opsx:propose`, `/opsx:apply`, `/opsx:archive`) for each roadmap slice. Keep a change to about half a page. Only spec the slice about to be built, not later milestones.
- **Small, runnable steps.** Each change should leave the app runnable and verifiable.
- **Do not commit, push or open pull requests unless asked.** Work on the branch that is checked out unless told otherwise.
- **Do not build features outside the current milestone**, even if the manifest lists them for later.

## Conventions

- **Vocabulary is fixed**: Item, Progress, Bookmark, Moment, Reel, Plan, Portkey, Note, Link (see the manifest). Use the same word in UI text, API routes, code and database.
- **Do not name types or namespaces** `Trace`, `Segment`, `Range`, `Span`, `Index`, `Timer` or `Stream`. They collide with .NET types or with the vocabulary.
- **References, not copies.** Never download, re-host or alter YouTube content. Play it only through the official IFrame Player API.
- **IDs are ULIDs or UUIDs**, never auto-increment integers. Every synced record has an `updatedAt`; newest save wins.
- **Everything is owned by a user.** Include the owner on every record from the start, even before accounts exist.
- **The product display name lives in one config value**, not scattered through the code. The code name is Pensieve.
- **Phone first.** Design and test the UI at phone width before desktop.
- **C#**: nullable enabled, minimal APIs, small focused files. **TypeScript**: strict, Svelte 5 runes.
- Keep dependencies few. Prefer the standard library and framework features.
