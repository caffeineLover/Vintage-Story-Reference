# Project Memory

Last updated: 2026-07-14

## Project overview

Vintage Story Reference is a local reference library for AI-assisted Vintage Story modding. It stores versioned decompiled assemblies, extracted assets, selected mod references, and generated SQLite indexes so other coding agents can query narrow facts without scanning the full reference trees.

## Current state

The repository currently documents Vintage Story `1.22.3` as the indexed version. Existing asset and code index tooling lives under `tools/asset-index` and `tools/code-index`, with generated index artifacts under `versions/1.22.3/Index`.

The Config Lib JSON API GitHub wiki page, VS Village Mod DB page, RPVoiceChat Server Administration wiki page, and RPVoiceChat GitHub README are indexed as curated external documentation. The source manifest is `versions/1.22.3/Web/sources.json`; the generated SQLite index is `versions/1.22.3/Index/web.sqlite`; the generated manifest is `versions/1.22.3/Index/web.index.json`; query tooling is under `tools/web-index`.

The repository also contains a root-level `prompts/` catalog for reusable Codex agent operating standards and reference-research workflows. Current prompt bundles cover persistent project memory, Vintage Story mod coding standards, project/Git hygiene, and Vintage Story research. `AGENTS.md` intentionally points at these project-local prompt files instead of the older sibling `Shared Agent Skills` folder.

The repository now has machine-readable discovery catalogs. `reference.catalog.json` is the root catalog for docs, prompts, skills, tools, and available versions. Each version should have `versions/<version>/version.catalog.json` for version-specific reference roots, indexes, manifests, and query/build scripts.

The working tree contains many untracked reference and index files. Treat those as existing user/reference material unless the user explicitly asks to clean or commit them.

## Active work

No active work is currently in progress. RPVoiceChat 2.5.8 assets, project-style C# source, and curated upstream docs have been added to the 1.22.3 indexes and verified.

## Durable technical knowledge

Agents should query the SQLite indexes before reading raw reference trees. Asset questions should start with `assets.sqlite`; C# and symbol questions should start with `code.sqlite`; curated external documentation questions should start with `web.sqlite` once present.

For routine consumer queries, prefer `tools/reference-query.py`. It reads the catalogs, resolves the selected version, and delegates to the existing source-specific query helpers. The asset, code, and web query scripts remain the source-specific interfaces and should not be duplicated inside the wrapper.

Other Codex-assisted projects should read `INDEX_USAGE.md` when they need to use this repository as a Vintage Story knowledge source. They can paste `prompts/Vintage Story Research/Add to AGENTS.md` into their own `AGENTS.md` to learn the index-first research workflow. They should read or adopt the other files under `prompts/` when they need the user's agent operating standards.

The Config Lib JSON API upstream source is the GitHub wiki page at `https://github.com/maltiez2/vsmod_configlib/wiki/JSON-API`. Its raw Markdown endpoint is `https://raw.githubusercontent.com/wiki/maltiez2/vsmod_configlib/JSON-API.md`.

The VS Village upstream source is the Vintage Story Mod DB page at `https://mods.vintagestory.at/vsvillage`. The builder extracts the page's description tab via the `moddb_description` HTML scope so the searchable index focuses on gameplay/design-reference content and current Mod DB metadata instead of full site navigation or file tables.

As of the 2026-07-14 RPVoiceChat rebuild, the 1.22.3 asset index contains `22,052` files; the code index contains `4,320` C# files, `4,461` types, and `21,231` members; the web index contains `4` sources and `87` heading-based sections. Generated manifests under `versions/1.22.3/Index` remain the exact source of truth for timestamps, roots, hashes, and detailed counts.

RPVoiceChat 2.5.8 is indexed from `versions/1.22.3/mods/rpvoicechat_2.5.8`. Its asset root is `versions/1.22.3/mods/rpvoicechat_2.5.8/assets` and its C# source root is `versions/1.22.3/mods/rpvoicechat_2.5.8/RPVoiceChat`.

For Opus voice-chat implementation research, start with the code index root `rpvoicechat_2.5.8/RPVoiceChat`. Useful indexed symbols and paths include `RPVoiceChat.Audio.OpusCodec` at `versions/1.22.3/mods/rpvoicechat_2.5.8/RPVoiceChat/Audio/OpusCodec.cs`, `RPVoiceChat.Audio.MicrophoneManager`, `RPVoiceChat.Audio.PlayerAudioSource`, and `RPVoiceChat.Networking.AudioPacket.Codec`. Verified source behavior: `MicrophoneManager` defaults capture to `Opus`; the world config toggle `encode-audio` chooses `Opus` versus `Dummy`; `PlayerAudioSource` constructs `OpusCodec` when packet codec names `Opus`; `AudioPacket` carries a `Codec` property; `OpusCodec` uses Concentus `OpusCodecFactory`, `OPUS_APPLICATION_VOIP`, voice signal mode, SILK-only mode, VBR, and separate normal/broadcast bitrate settings.

RPVoiceChat upstream documentation sources are now indexed as `rpvoicechat-server-administration` and `rpvoicechat-github-readme`. The Server Administration page covers server config path `ModConfig/RPVoiceChat/rpvoicechat-server.json`, network settings, codec bitrate settings, communication systems, sound-emitting object ranges, and `/rpvc` admin commands. The README records project overview, usage/configuration entry points, developer documentation links, and Opus/RNNoise licensing notes.

## Architecture and design decisions

The web documentation index follows the existing project pattern of generated SQLite indexes plus small query helpers. It stores external documentation as dated searchable index content instead of adding separate local HTML or Markdown snapshots.

The code indexer now recognizes project-style mod source roots when no `decompiled` folder is present. It scans each mod folder for `.csproj` directories that contain `.cs` files, skips generated/editor folders and `assets`, and indexes those directories as mod source roots. This was required for RPVoiceChat because its C# files live under `rpvoicechat_2.5.8/RPVoiceChat` rather than a folder named `decompiled`.

The Markdown splitter ignores headings inside fenced code blocks. This matters for the Config Lib page because YAML examples contain decorative `###` lines that should remain example text rather than becoming documentation headings.

The web index builder supports both Markdown-like sources and scoped HTML sources. For HTML pages, it converts selected HTML into Markdown-like text before sectioning so headings, lists, and paragraphs remain searchable through the same `sections` and `web_text` tables.

Project-specific structure decision: `prompts/` is intentionally kept at the repository root as a reusable prompt catalog, and `.codex/skills/` is intentionally kept for project-local Codex skills. This overrides the generic project standard that ordinary documentation belongs under `docs/`.

The Vintage Story Research prompt is deliberately separate from `INDEX_USAGE.md`: `INDEX_USAGE.md` documents this repository's current indexes and commands, while `prompts/Vintage Story Research/VINTAGE_STORY_RESEARCH.md` is the reusable instruction set that can be referenced or copied into other projects.

The drop-in `prompts/Vintage Story Research/Add to AGENTS.md` snippet should stay short, version-agnostic, and broadly scoped. It should tell consuming projects when to use the Vintage Story Reference archive and where to read the full research prompt. It should not name one indexed mod as though it is special; detailed examples, index paths, version-selection rules, and research mechanics belong in `prompts/Vintage Story Research/VINTAGE_STORY_RESEARCH.md`, `INDEX_USAGE.md`, manifests, or local examples.

Architecture decision: keep consumer docs, maintainer docs, catalogs, scripts, and prompts separate. `README.md` remains a short overview; `INDEX_USAGE.md` is the current concrete consumer map; `docs/ARCHITECTURE.md` explains the system shape; `docs/MAINTAINING_REFERENCE.md` explains maintenance workflows; catalogs are small machine-readable pointers; generated manifests remain the detailed generated-index source of truth.

## External interfaces and integrations

The web index source manifest is `versions/1.22.3/Web/sources.json`. Each source entry includes a public page URL, raw fetch URL, title, source type, applicability notes, and last-known edit date when available.

The RPVoiceChat Server Administration upstream source is `https://github.com/Ridderrasmus/RPVoiceChat/wiki/Server-Administration`; its raw Markdown endpoint is `https://raw.githubusercontent.com/wiki/Ridderrasmus/RPVoiceChat/Server-Administration.md`. The RPVoiceChat GitHub README source is `https://github.com/Ridderrasmus/RPVoiceChat`; the indexed raw Markdown endpoint is `https://raw.githubusercontent.com/Ridderrasmus/RPVoiceChat/development/README.md`.

## Known failed approaches

No failed approaches have been recorded yet.

## Bugs, risks, and limitations

The web index is a dated snapshot of fetched upstream content. If current documentation matters, rebuild the index or check the upstream URL.

The working tree currently has many untracked files that appear to be reference payloads or generated indexes. Do not assume a clean Git state.

The asset index rebuild for 1.22.3 can take longer than two minutes. On 2026-07-14 it timed out at a 120-second command limit but succeeded with a 300-second limit in about 220 seconds.

## Open questions

No open technical questions are currently recorded.

## Testing and verification

Useful verification for the web index:

```powershell
python -B tools\web-index\build_web_index.py --version 1.22.3
python -B tools\web-index\query_web_index.py --db versions\1.22.3\Index\web.sqlite --list-sources
python -B tools\web-index\query_web_index.py --db versions\1.22.3\Index\web.sqlite --search "conditional patches"
python -B tools\web-index\query_web_index.py --db versions\1.22.3\Index\web.sqlite --search "configlib-patches json"
python -B tools\web-index\query_web_index.py --db versions\1.22.3\Index\web.sqlite --search "mapping values range logarithmic"
python -B tools\web-index\query_web_index.py --db versions\1.22.3\Index\web.sqlite --source vsvillage --search "Summoning Horn Settlement Keeper"
python -B tools\web-index\query_web_index.py --db versions\1.22.3\Index\web.sqlite --source vsvillage --search "villager pathfinding command"
python -B tools\web-index\query_web_index.py --db versions\1.22.3\Index\web.sqlite --source vsvillage --heading "Village Generation"
python -B tools\reference-query.py --version 1.22.3 asset --search rpvoicechat --limit 5
python -B tools\reference-query.py --version 1.22.3 code --type OpusCodec --limit 10
python -B tools\reference-query.py --version 1.22.3 code --member Codec --limit 10
python -B tools\reference-query.py --version 1.22.3 web --source rpvoicechat-server-administration --search "bitrate" --limit 10
python -B tools\reference-query.py --version 1.22.3 web --source rpvoicechat-github-readme --search "Opus RNNoise" --limit 10
python -B tools\reference-query.py --list-versions
python -B tools\reference-query.py --version 1.22.3 --list-indexes
python -B tools\reference-query.py --version 1.22.3 asset --search copper --limit 5
python -B tools\reference-query.py --version 1.22.3 code --type ICoreAPI --limit 5
python -B tools\reference-query.py --version 1.22.3 web --list-sources
```

On 2026-07-14, those commands succeeded. After the RPVoiceChat rebuild, `git diff --check` reported only Git line-ending warnings for `INDEX_USAGE.md`, `docs/MAINTAINING_REFERENCE.md`, and `versions/1.22.3/version.catalog.json`.

## Useful commands

```powershell
python -B tools\asset-index\query_asset_index.py --db versions\1.22.3\Index\assets.sqlite --search copper --limit 20
python -B tools\code-index\query_code_index.py --db versions\1.22.3\Index\code.sqlite --type ICoreAPI --limit 20
python -B tools\web-index\query_web_index.py --db versions\1.22.3\Index\web.sqlite --search "configlib-patches json" --limit 20
```

## Important files and code paths

- `INDEX_USAGE.md` - Instructions for other AI-enabled projects that consume this reference repository.
- `tools/asset-index` - Asset index builder and query helper.
- `tools/code-index` - Decompiled C# code index builder and query helper.
- `tools/web-index` - Curated external documentation index builder and query helper.
- `tools/reference-query.py` - Unified query wrapper that resolves catalog paths and delegates to source-specific query helpers.
- `prompts` - Reusable Codex agent prompt standards for project memory, coding standards, project/Git hygiene, and Vintage Story research.
- `.codex/skills` - Project-local Codex skills for indexing assets, indexing decompiled C# code, and updating reference mod folders.
- `reference.catalog.json` - Root machine-readable discovery catalog.
- `versions/1.22.3/version.catalog.json` - Version-specific discovery catalog for the 1.22.3 reference payload.
- `docs/ARCHITECTURE.md` - Explanation of repository roles and design boundaries.
- `docs/MAINTAINING_REFERENCE.md` - Maintainer workflows for adding versions, mods, websites, and refreshing indexes.
- `versions/1.22.3/Index` - Generated SQLite indexes and manifests.
- `versions/1.22.3/Web/sources.json` - Curated external web documentation source list.
- `versions/1.22.3/mods/rpvoicechat_2.5.8/RPVoiceChat/Audio/OpusCodec.cs` - RPVoiceChat's Concentus/Opus codec implementation.
- `versions/1.22.3/mods/rpvoicechat_2.5.8/RPVoiceChat/Audio/MicrophoneManager.cs` - RPVoiceChat client capture path that selects `Opus` or `Dummy` based on `encode-audio`.
- `versions/1.22.3/mods/rpvoicechat_2.5.8/RPVoiceChat/Audio/PlayerAudioSource.cs` - RPVoiceChat playback path that constructs the matching codec for incoming packets.
- `versions/1.22.3/mods/rpvoicechat_2.5.8/RPVoiceChat/Networking/AudioPacket.cs` - RPVoiceChat network packet carrying the codec name with voice payload metadata.

## Session history

### 2026-07-14

- Created project memory after `AGENTS.md` introduced the shared persistent memory requirement and no existing `docs/PROJECT_MEMORY.md` was present.
- Added `tools/web-index/build_web_index.py` and `tools/web-index/query_web_index.py`.
- Added `versions/1.22.3/Web/sources.json` for the Config Lib JSON API GitHub wiki page.
- Built `versions/1.22.3/Index/web.sqlite` and `versions/1.22.3/Index/web.index.json`.
- Updated `README.md` and `INDEX_USAGE.md` so future agents know to query the web documentation index.
- Verified representative Config Lib searches for conditional patches, `configlib-patches.json`, and setting range/value/mapping/logarithmic behavior.
- Added the VS Village Mod DB page as `vsvillage-moddb` in the web source manifest.
- Extended the web index builder to normalize scoped HTML pages into searchable sections.
- Rebuilt `web.sqlite`; the web index now contains `2` sources and `35` sections, including `21` VS Village sections.
- Verified VS Village searches for village founding, pathfinding commands, and village generation.
- Updated `AGENTS.md` to use the project-local prompt catalog under `prompts/` for persistent memory, coding standards, and project/Git hygiene.
- Documented `prompts/` and `.codex/skills/` in `README.md`, `INDEX_USAGE.md`, and project memory.
- Updated the prompt catalog's small `Add to AGENTS` snippets so they point at the Vintage Story Reference prompt catalog instead of the old sibling `Shared Agent Skills` folder.
- Added `prompts/Vintage Story Research/VINTAGE_STORY_RESEARCH.md` and `prompts/Vintage Story Research/Add to AGENTS.md` so other Codex-assisted projects can adopt the index-first Vintage Story research workflow.
- Updated README, INDEX_USAGE, AGENTS, and project memory to document the Vintage Story Research prompt bundle.
- Revised the Vintage Story Research AGENTS snippet and full prompt to avoid hardcoding `1.22.3`; reusable instructions now use `<game-version>`/`$v` and tell agents to consult `INDEX_USAGE.md` for current indexed versions.
- Shortened `prompts/Vintage Story Research/Add to AGENTS.md` so it only declares when to use the archive and where the full instructions live.
- Removed the specific Config Lib mention from the Vintage Story Research AGENTS snippet so the trigger text remains about broad reference categories rather than one indexed mod.
- Cleaned up this repository's root `AGENTS.md` so it uses project-local prompt paths instead of sibling-project paths through `..\Vintage Story Reference`.
- Added `reference.catalog.json` and `versions/1.22.3/version.catalog.json` as machine-readable discovery catalogs.
- Added `tools/reference-query.py` as a unified wrapper around the asset, code, and web query helpers.
- Added `docs/ARCHITECTURE.md` and `docs/MAINTAINING_REFERENCE.md`.
- Updated `README.md`, `INDEX_USAGE.md`, and the Vintage Story Research prompt to document the catalog-and-wrapper workflow.
- Added RPVoiceChat 2.5.8 reference material under `versions/1.22.3/mods/rpvoicechat_2.5.8` to the generated reference indexes.
- Updated `tools/code-index/build_code_index.py` so project-style mod source roots with `.csproj` files are indexed when no `decompiled` folder exists.
- Rebuilt `versions/1.22.3/Index/code.sqlite`; the code index now includes `rpvoicechat_2.5.8/RPVoiceChat` and `wolftaming_v5.0.1/WolfTaming` source roots, with `4,320` C# files, `4,461` types, and `21,231` members.
- Rebuilt `versions/1.22.3/Index/assets.sqlite`; the asset index now includes `versions/1.22.3/mods/rpvoicechat_2.5.8/assets`, with `22,052` total files and `225` RPVoiceChat asset files.
- Added `rpvoicechat-server-administration` and `rpvoicechat-github-readme` to `versions/1.22.3/Web/sources.json`.
- Rebuilt `versions/1.22.3/Index/web.sqlite`; the web index now contains `4` sources and `87` sections, including RPVoiceChat server settings, codec bitrate settings, `/rpvc` commands, README usage/configuration links, and Opus/RNNoise dependency notes.
- Updated `INDEX_USAGE.md`, `versions/1.22.3/version.catalog.json`, and `docs/MAINTAINING_REFERENCE.md` for the new RPVoiceChat roots, docs, counts, examples, and project-style source-root behavior.
- Verified RPVoiceChat queries through `tools/reference-query.py`: asset search for `rpvoicechat`, code type lookup for `OpusCodec`, code member lookup for `Codec`, web search for RPVoiceChat bitrate settings, and README search for `Opus RNNoise`.
