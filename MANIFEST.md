# Pensieve - Product Manifest

> Save the parts of videos that matter. Come back to them. Learn from them.

**Status:** idea stage, nothing built yet. **Written:** 2026-10-27.
**Audience:** me first, a few friends later. **Nature:** hobby project, built to grow slowly.

---

## 1. Problem

I learn a lot from YouTube: workouts, philosophy, meditation, nutrition, long podcasts. Three things keep wasting my time.

1. **Long videos hold a small amount of useful content.** In a workout video the real demonstration is a few minutes, surrounded by talking and ads. LLMs (Claude, ChatGPT, Gemini) write very good plans, but they link whole videos.
2. **Different videos explain different parts best.** To get one lift right I watch two or three videos, each for a minute or two. Playlists do not help, because I still hunt for the part in every video.
3. **I cannot finish 2-3 hour podcasts in one sitting, and YouTube history is off.** YouTube therefore does not remember where I stopped, and I do not want it to.

## 2. What Pensieve is

A personal learning companion built on top of YouTube. It remembers where I stopped, lets me mark the useful moments of a video, and plays those moments back-to-back as a short reel. Later it turns them into plans, notes and a map of linked ideas.

Pensieve stores **references to videos, never the videos themselves.**

## 3. The core loop

```
Add a video  ->  Watch (position is remembered)  ->  Mark a moment + one-line note
      ^                                                        |
      |                                                        v
 Recall / review  <-  Replay moments as a reel  <-  Group moments into a plan
```

The name fits the loop: a pensieve is where memories are kept and later stepped back into.

## 4. Principles

- **References, not copies.** Pensieve never downloads, re-hosts or alters YouTube content. Playback uses the official embedded player (IFrame API), which supports start and end times.
- **Private by default.** Everything belongs to the signed-in user. No ads, no tracking, no recommendation feed. Use the privacy-enhanced embed domain (`youtube-nocookie.com`) where possible.
- **Phone first.** The gym is a main place of use. Large controls, fast load, installable as a PWA.
- **Small steps.** Each milestone is useful alone. If a milestone is not used for two weeks, rethink before building the next.
- **Boring technology.** Choose tools I know. Prefer one deployable thing and one database file.
- **Keep the brand out of the code.** The code name is Pensieve. The display name lives in one config value, so renaming is cheap.

## 5. Vocabulary

One word per idea, used identically in the UI, API and database.

| Term | Meaning |
|---|---|
| **Item** | A video or other media in my library. Source: YouTube now, local file or other URL later. |
| **Progress** | The last position I reached in an item, per user. |
| **Bookmark** | A single saved position in an item, with an optional label. |
| **Moment** | A marked range of an item (start, end) with a one-line note. This is the "clip". |
| **Reel** | An ordered list of moments played back-to-back. |
| **Plan** | A set of sections (for example exercises or topics), each holding moments from several items. |
| **Portkey** | A link that opens an item at an exact time. |
| **Note** | Free text attached to an item or a moment. |
| **Link** | A relation between two moments (later). |

Avoid `Trace`, `Segment`, `Range`, `Span`, `Index`, `Timer` and `Stream` as type or namespace names in .NET. They collide with framework types or with the words above.

## 6. Roadmap

### Milestone 1 - Library and resume
Build the foundation and prove I use it.

- Register, sign in, sign out.
- Add a YouTube video by pasting a URL (watch, youtu.be, shorts). Fetch title and thumbnail. Honour a `t=` in the URL.
- Embedded player that saves the position every few seconds, on pause, on tab close and on switching videos.
- **Continue watching** row, newest first, with "1h 12m of 2h 40m" and a progress bar.
- Several **bookmarks** per item (for example "come back at 45:10").
- Remember playback speed per item.
- Items past about 95% count as finished and restart from 0.
- Newest save wins when two devices are used, and a stale tab never overwrites a newer position.

**Done when:** I use it for a real multi-hour podcast on my phone and laptop for two weeks without opening YouTube's own resume.

### Milestone 2 - Moments
- Mark a start and end while watching, with keyboard and on-screen buttons.
- One-line note and optional label per moment.
- Edit and delete moments. Moments are listed on the item page.

### Milestone 3 - Reel player
- Play a list of moments continuously, seeking to each start and moving on at its end.
- Speed control, skip, previous, and a visible queue.
- Works with the screen locked as far as the browser allows. Large controls for use at the gym.

### Milestone 4 - Plans
- Create a plan with sections. Add moments from different items to each section.
- Play a section or the whole plan as a reel.
- First real use case: a workout plan.

### Milestone 5 - Import from an LLM plan
- Paste a plan written by an LLM that links videos. Attach moments to each exercise.
- Help finding moments (see risk 3): YouTube chapters first, pasted transcripts later.

### Milestone 6 - Sharing
- Read-only link to a plan or reel for friends.

### Later, not committed
Tags and playlists, notes with timestamps, spaced resurfacing of moments (review after 1, 3, 7 days), links between moments, a map view of linked moments, own video files and other sources, native offline apps.

## 7. Non-goals (for now)

- Not a video host. No uploads, no downloads, no re-encoding of YouTube content.
- Not a social network. No feeds, followers or comments.
- No recommendations or algorithmic suggestions.
- No monetisation, ads or analytics.
- No bulk import of YouTube history or subscriptions.

## 8. Technical direction

| Layer | Choice |
|---|---|
| Backend | ASP.NET Core minimal APIs, .NET (current LTS) |
| Database | SQLite via EF Core, with Postgres as a later option |
| Auth | ASP.NET Identity, cookie sessions, username and password to start |
| Frontend | Svelte + Vite + TypeScript, served by the same ASP.NET app |
| Player | YouTube IFrame Player API |
| Offline / native later | PWA first, then Tauri (desktop) or Capacitor (mobile) around the same frontend |
| Hosting | undecided; a single container on a small host or free tier |

Svelte was chosen over Blazor because later milestones lean on JavaScript libraries for the timeline editor and the map view. Revisit if I decide to stay fully in C#.

**Data model sketch** (names follow section 5):

- `User`
- `Item` (id, owner, source, external id, title, thumbnail, duration)
- `Progress` (item, user, position, updated at, speed, finished)
- `Bookmark` (item, position, label)
- `Moment` (item, start, end, note) - from milestone 2
- `Plan`, `PlanSection`, `PlanSectionMoment` - from milestone 4

IDs are UUIDs or ULIDs rather than auto-increment numbers, so an offline app can create records and sync them later. Every record carries an `updated at` for newest-wins syncing.

## 9. Risks and open questions

1. **Some videos block embedding.** The owner can disable it. Such videos cannot play in Pensieve. Show a link out and let the position be entered by hand.
2. **YouTube history with the embedded player.** With history off in the browser, embedded playback should not add to it, but this is controlled by YouTube. Test once milestone 1 exists.
3. **Finding moments automatically.** An LLM cannot see video and needs a transcript. Official access to transcripts is limited, and unofficial methods are fragile and a gray area in YouTube's terms. Plan: manual marking first, then chapters from the description, then pasted transcripts. Do not depend on scraping.
4. **Scope creep.** The wish list is six products. The rule is to finish one milestone and use it before starting the next.
5. **Authentication.** Username and password to start. Decide later whether to add Google sign-in.
6. **Hosting.** Undecided. Local-only is fine for milestone 1. A PWA used on a phone needs HTTPS and a reachable host.
7. **Names and trademarks.** "Vestigia" was the brand idea (a landmark app uses that name, `vestigia.in` looked free in a quick check). No domain bought. Check trademarks before any public launch.

## 10. How I will know it works

- I open Pensieve instead of YouTube for anything longer than an hour.
- I replay a reel at the gym instead of watching full videos.
- A friend asks to use it.

---

*This is a living document. Change it when a decision changes, and record the reason.*
