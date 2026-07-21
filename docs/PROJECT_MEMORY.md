# Project Memory

Last updated: 2026-07-21

## Project overview

Vintage Story Reference is a local reference library for AI-assisted Vintage Story modding. It stores versioned decompiled assemblies, extracted assets, selected mod references, and generated SQLite indexes so other coding agents can query narrow facts without scanning the full reference trees.

## Current state

The repository currently documents Vintage Story `1.22.3` as the default fully indexed version. Existing asset and code index tooling lives under `tools/asset-index` and `tools/code-index`, with generated index artifacts under `versions/1.22.3/Index`.

The Config Lib JSON API GitHub wiki page, VS Village Mod DB page, RPVoiceChat Server Administration wiki page, and RPVoiceChat GitHub README are indexed as curated external documentation. The source manifest is `versions/1.22.3/Web/sources.json`; the generated SQLite index is `versions/1.22.3/Index/web.sqlite`; the generated manifest is `versions/1.22.3/Index/web.index.json`; query tooling is under `tools/web-index`.

The repository also contains a root-level `prompts/` catalog for reusable Codex agent operating standards and reference-research workflows. Current prompt bundles cover persistent project memory, Vintage Story mod coding standards, project/Git hygiene, and Vintage Story research. `AGENTS.md` intentionally points at these project-local prompt files instead of the older sibling `Shared Agent Skills` folder.

The repository now has machine-readable discovery catalogs. `reference.catalog.json` is the root catalog for docs, prompts, skills, tools, and available versions. Each version should have `versions/<version>/version.catalog.json` for version-specific reference roots, indexes, manifests, and query/build scripts.

The `versions/1.22.4/API` tree is populated with decompiled `VintagestoryAPI`, `VintagestoryLib`, and `VSSurvivalMod` sources. Decompiled API trees for `1.22.3` and `1.22.4` intentionally use flattened assembly wrapper folders and do not keep `.sln` files. The `1.22.4` version folder has copied full client assets, selected reference mods, copied curated web sources, plus code, asset, and web indexes. The root solution now contains a `VintageStory.Reference.1.22.4` wrapper project for browsing the `1.22.4` payload in Rider.

Generated files inside `versions/<version>/Index` are local rebuildable output and should not be staged or pushed by default. Each `Index` folder should exist in Git via a tracked `versions/<version>/Index/.gitignore` file that ignores every generated file below it. `versions/1.22.4/Index/assets.sqlite` is over GitHub's normal 100 MB file limit. Commit curated source manifests such as `versions/<version>/Web/sources.json`, not the generated SQLite indexes, unless the user explicitly chooses a large-file strategy.

The working tree contains many untracked reference and index files. Treat those as existing user/reference material unless the user explicitly asks to clean or commit them.

## Active work

No active work is currently in progress. RPVoiceChat 2.5.8 assets, project-style C# source, and curated upstream docs have been added to the 1.22.3 indexes and verified.

## Durable technical knowledge

Agents should query the SQLite indexes before reading raw reference trees. Asset questions should start with `assets.sqlite`; C# and symbol questions should start with `code.sqlite`; curated external documentation questions should start with `web.sqlite` once present.

For routine consumer queries, prefer `tools/reference-query.py`. It reads the catalogs, resolves the selected version, and delegates to the existing source-specific query helpers. The asset, code, and web query scripts remain the source-specific interfaces and should not be duplicated inside the wrapper.

Other Codex-assisted projects should read `INDEX_USAGE.md` when they need to use this repository as a Vintage Story knowledge source. They can paste `prompts/Vintage Story Research/Add to AGENTS.md` into their own `AGENTS.md` to learn the index-first research workflow. They should read or adopt the other files under `prompts/` when they need the user's agent operating standards.

The Config Lib JSON API upstream source is the GitHub wiki page at `https://github.com/maltiez2/vsmod_configlib/wiki/JSON-API`. Its raw Markdown endpoint is `https://raw.githubusercontent.com/wiki/maltiez2/vsmod_configlib/JSON-API.md`.

The VS Village upstream source is the Vintage Story Mod DB page at `https://mods.vintagestory.at/vsvillage`. The builder extracts the page's description tab via the `moddb_description` HTML scope so the searchable index focuses on gameplay/design-reference content and current Mod DB metadata instead of full site navigation or file tables.

As of the 2026-07-14 RPVoiceChat rebuild, the 1.22.3 asset index contains `22,052` files and the web index contains `4` sources and `87` heading-based sections. As of the 2026-07-21 API flatten rebuild, the 1.22.3 code index contains `4,324` C# files, `4,465` types, and `21,242` members. Generated manifests under `versions/1.22.3/Index` remain the exact source of truth for timestamps, roots, hashes, and detailed counts.

RPVoiceChat 2.5.8 is indexed from `versions/1.22.3/mods/rpvoicechat_2.5.8`. Its asset root is `versions/1.22.3/mods/rpvoicechat_2.5.8/assets` and its C# source root is `versions/1.22.3/mods/rpvoicechat_2.5.8/RPVoiceChat`.

For Opus voice-chat implementation research, start with the code index root `rpvoicechat_2.5.8/RPVoiceChat`. Useful indexed symbols and paths include `RPVoiceChat.Audio.OpusCodec` at `versions/1.22.3/mods/rpvoicechat_2.5.8/RPVoiceChat/Audio/OpusCodec.cs`, `RPVoiceChat.Audio.MicrophoneManager`, `RPVoiceChat.Audio.PlayerAudioSource`, and `RPVoiceChat.Networking.AudioPacket.Codec`. Verified source behavior: `MicrophoneManager` defaults capture to `Opus`; the world config toggle `encode-audio` chooses `Opus` versus `Dummy`; `PlayerAudioSource` constructs `OpusCodec` when packet codec names `Opus`; `AudioPacket` carries a `Codec` property; `OpusCodec` uses Concentus `OpusCodecFactory`, `OPUS_APPLICATION_VOIP`, voice signal mode, SILK-only mode, VBR, and separate normal/broadcast bitrate settings.

RPVoiceChat upstream documentation sources are now indexed as `rpvoicechat-server-administration` and `rpvoicechat-github-readme`. The Server Administration page covers server config path `ModConfig/RPVoiceChat/rpvoicechat-server.json`, network settings, codec bitrate settings, communication systems, sound-emitting object ranges, and `/rpvc` admin commands. The README records project overview, usage/configuration entry points, developer documentation links, and Opus/RNNoise licensing notes.

Vintage Story 1.22.4 API sources were produced from the public Windows dedicated server ZIP `vs_server_win-x64_1.22.4.zip` from the Vintage Story CDN. The downloaded ZIP was verified against MD5 `162941f6a5d6dbc576cce700f50d16bf`. Decompilation used `ilspycmd` `10.1.1.8388` with `--nested-directories`, `--use-varnames-from-pdb`, and reference paths to the extracted server root, `Lib`, and `Mods`. The transient temp-derived `<HintPath>` entries in the generated SDK-style `.csproj` files were removed before keeping the output. The API wrappers are flattened, so files live directly under `VintagestoryAPI_decompiled_1.22.4`, `VintagestoryLib_decompiled_1.22.4`, and `VSSurvivalMod_decompiled_1.22.4` rather than under same-name child folders.

Vintage Story 1.22.4 full assets were copied from the public Linux client archive `vs_client_linux-x64_1.22.4.tar.gz` from the Vintage Story CDN. The downloaded archive was verified against MD5 `c7d943c25de23f5055e485d86b01aaf8`. The Windows dedicated server package contains a reduced `assets` folder; do not use server-only assets as the final full `versions/<version>/Assets` payload.

Rider/JetBrains `no index` labels on version folders refer to IDE solution/project indexing, not the repository's SQLite reference indexes. On 2026-07-21, `versions/1.22.4/VintageStory.Reference.1.22.4.csproj` was added and nested under a `1.22.4` solution folder beside the existing `1.22.3` wrapper. Version wrapper projects disable default compile/resource items and include `version.catalog.json`, `API`, `Assets`, `mods`, and `Web` as `None` items so Rider can browse the reference payload without compiling decompiled game sources. If Rider still displays stale `no index` labels after this change, reload the solution or refresh IDE indexes.

Future new Vintage Story releases should use the project-local `do-new-version` Codex skill. It records the preferred workflow: create a browse-only `versions/<version>/VintageStory.Reference.<version>.csproj` and solution entry, create `versions/<version>/API` and `versions/<version>/Assets`, decompile `VintagestoryAPI.dll`, `VintagestoryLib.dll`, and `Mods/VSSurvivalMod.dll` from the official release package into flattened wrapper folders, copy full client assets, use `new-version-mods` to download/decompile current compatible selected reference mods, use `do-new-web` to copy/index curated web sources from the nearest previous version, avoid keeping decompiler-generated `.sln` or `.slnx` files, build the code, asset, and web indexes, and update catalogs so other agents can discover the indexed API, assets, mod references, and web references.

The project-local skill for reference mod population is `.codex/skills/new-version-mods`. It replaced the longer `vintage-story-reference-mod-updater` name on 2026-07-21. Use it independently for `versions/<version>/mods` update work, and invoke it from `do-new-version` before final code/asset index builds so mod assets and decompiled mod code are included in the new version's indexes.

`new-version-mods` now creates `versions/<game-version>/mods` if it is missing. For patch-version targets, it prefers exact Mod DB tags and may use the newest same major/minor release as a documented reference fallback when no exact patch tag exists. For new target versions, it should normalize completed mod folder names as `<modid>_<modversion>` from `modinfo.json` or Mod DB metadata and keep exactly one latest compatible folder per mod id. Existing duplicate mod folders should be reported and removed only when cleanup was explicitly requested.

The project-local skill for carrying curated web documentation forward is `.codex/skills/do-new-web`. Use it independently for `versions/<version>/Web` source copying, merging, refreshing, or indexing work, and invoke it from `do-new-version` before final validation so the new version has `web.sqlite` and `web.index.json` alongside its asset and code indexes. `versions/<version>/Web/sources.json` is curated version metadata and should be added to Git after it is created or merged; do not leave it only as an untracked local file.

Vintage Story 1.22.4 selected reference mods were seeded from the 1.22.3 mod set on 2026-07-21. The target contains 13 unique mod ids under `versions/1.22.4/mods`: `A Panserbjorn Story Rebjorn (1.0.0)` for mod id `panserbjornrebjorn`, `allclasses_2.3.0`, `betterruins_0.6.3`, `bunnypet_1.0.4`, `cats_5.0.1`, `configlib_1.12.0`, `critcherscalculator_1.2.3`, `jaunt_3.1.0`, `petai_5.1.1`, `primitivesurvival_5.0.6`, `rpvoicechat_2.5.8`, `vsimgui_1.2.7`, and `wolftaming_5.0.1`. `betterruins_0.6.3` was the only selected release with an exact Mod DB `1.22.4` tag; the rest are same-line `1.22.x` fallbacks because Mod DB did not publish an exact `1.22.4` tag for those mod ids at selection time. The selection/download report is `.codex/tmp/mod-updates/1.22.4/selected-releases.json`.

Vintage Story 1.22.4 web sources were copied from `versions/1.22.3/Web/sources.json` on 2026-07-21 and re-indexed into `versions/1.22.4/Index/web.sqlite`. The 1.22.4 web index contains the same 4 curated source slugs as 1.22.3, split into 87 sections: `configlib-json-api`, `vsvillage-moddb`, `rpvoicechat-server-administration`, and `rpvoicechat-github-readme`.

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

The web index source manifest is `versions/<version>/Web/sources.json`. Each source entry includes a public page URL, raw fetch URL, title, source type, applicability notes, and last-known edit date when available. For new versions, `do-new-web` copies or merges every source entry from the nearest previous version's source manifest before rebuilding the target `web.sqlite`.

The RPVoiceChat Server Administration upstream source is `https://github.com/Ridderrasmus/RPVoiceChat/wiki/Server-Administration`; its raw Markdown endpoint is `https://raw.githubusercontent.com/wiki/Ridderrasmus/RPVoiceChat/Server-Administration.md`. The RPVoiceChat GitHub README source is `https://github.com/Ridderrasmus/RPVoiceChat`; the indexed raw Markdown endpoint is `https://raw.githubusercontent.com/Ridderrasmus/RPVoiceChat/development/README.md`.

## Known failed approaches

No failed approaches have been recorded yet.

## Bugs, risks, and limitations

The web index is a dated snapshot of fetched upstream content. If current documentation matters, rebuild the index or check the upstream URL.

The working tree currently has many untracked files that appear to be reference payloads or generated indexes. Do not assume a clean Git state.

The asset index rebuild for 1.22.3 can take longer than two minutes. On 2026-07-14 it timed out at a 120-second command limit but succeeded with a 300-second limit in about 220 seconds.

The 1.22.4 API source, full client assets, selected mod references, copied web sources, code index, asset index, and web index exist and are cataloged. Consumers should explicitly pass `--version 1.22.4` for 1.22.4 API/code/asset/web queries; 1.22.3 remains the default version.

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
- `versions/1.22.4/version.catalog.json` - Version-specific discovery catalog for the 1.22.4 API/asset/code-index/asset-index payload.
- `versions/<version>/VintageStory.Reference.<version>.csproj` - Non-compiling Rider/MSBuild wrapper project that includes the version catalog, API, assets, mods, and Web source manifest as browseable `None` items.
- `docs/ARCHITECTURE.md` - Explanation of repository roles and design boundaries.
- `docs/MAINTAINING_REFERENCE.md` - Maintainer workflows for adding versions, mods, websites, and refreshing indexes.
- `versions/1.22.3/Index` - Tracked folder containing local generated SQLite indexes and manifests; only `Index/.gitignore` should be committed by default.
- `versions/1.22.3/Web/sources.json` - Curated external web documentation source list.
- `versions/1.22.4/API` - Decompiled 1.22.4 `VintagestoryAPI`, `VintagestoryLib`, and `VSSurvivalMod` sources.
- `versions/1.22.4/Assets` - Full copied 1.22.4 client assets from the Linux client archive.
- `versions/1.22.4/mods` - Selected 1.22.4 reference mod set seeded from 1.22.3, with exact 1.22.4 Mod DB releases preferred and same-line 1.22.x fallbacks recorded when exact patch tags were unavailable.
- `versions/1.22.4/Web/sources.json` - Copied curated external web documentation source manifest for the 1.22.4 web index.
- `versions/1.22.4/Index/code.sqlite` - Generated 1.22.4 code index for the decompiled API sources.
- `versions/1.22.4/Index/assets.sqlite` - Generated 1.22.4 asset index for the copied full client assets.
- `versions/1.22.4/Index/web.sqlite` - Generated 1.22.4 web documentation index copied from the 1.22.3 web source manifest and fetched on 2026-07-21.
- `.codex/skills/do-new-version` - Project-local Codex skill for creating a new version wrapper project, API tree, Assets tree, selected mods tree, copied web source manifest, and corresponding code/asset/web indexes from official Vintage Story release packages and carried-forward curated sources.
- `.codex/skills/do-new-web` - Project-local Codex skill for copying/merging previous-version curated web sources and building `web.sqlite`/`web.index.json` under `versions/<version>/Index`.
- `.codex/skills/new-version-mods` - Project-local Codex skill for downloading, staging, extracting, and decompiling selected compatible reference mods under `versions/<version>/mods`.
- `versions/1.22.3/mods/rpvoicechat_2.5.8/RPVoiceChat/Audio/OpusCodec.cs` - RPVoiceChat's Concentus/Opus codec implementation.
- `versions/1.22.3/mods/rpvoicechat_2.5.8/RPVoiceChat/Audio/MicrophoneManager.cs` - RPVoiceChat client capture path that selects `Opus` or `Dummy` based on `encode-audio`.
- `versions/1.22.3/mods/rpvoicechat_2.5.8/RPVoiceChat/Audio/PlayerAudioSource.cs` - RPVoiceChat playback path that constructs the matching codec for incoming packets.
- `versions/1.22.3/mods/rpvoicechat_2.5.8/RPVoiceChat/Networking/AudioPacket.cs` - RPVoiceChat network packet carrying the codec name with voice payload metadata.

## Session history

### 2026-07-21

- Populated `versions/1.22.4/API` from the Vintage Story 1.22.4 Windows dedicated server package.
- Verified the downloaded `vs_server_win-x64_1.22.4.zip` MD5 as `162941f6a5d6dbc576cce700f50d16bf`.
- Installed `ilspycmd` `10.1.1.8388` into a temporary tool folder and decompiled `VintagestoryAPI.dll`, `VintagestoryLib.dll`, and `Mods/VSSurvivalMod.dll`.
- Created matching wrapper folders: `VintagestoryAPI_decompiled_1.22.4`, `VintagestoryLib_decompiled_1.22.4`, and `VSSurvivalMod_decompiled_1.22.4`, each containing a decompiled project folder.
- Removed decompiler-generated `<HintPath>` values that pointed to the temporary server extraction folder.
- Verified decompiled source counts: `1,122` C# files for `VintagestoryAPI`, `1,008` for `VintagestoryLib`, and `1,295` for `VSSurvivalMod`.
- Did not build 1.22.4 catalogs or indexes during the initial API-population pass.
- Removed generated `.sln` files from the `1.22.3` and `1.22.4` API decompiled trees because they are not useful for indexing agents.
- Added the `.codex/skills/do-new-version` skill to standardize future release ingestion: create `versions/<version>/API`, decompile the three official DLLs directly, scrub temp paths, avoid keeping solution files, build the code index, and update catalogs.
- Built `versions/1.22.4/Index/code.sqlite` and `versions/1.22.4/Index/code.index.json`; the 1.22.4 code index contains `3,425` C# files, `3,554` types, and `18,819` members across the three API roots.
- Added `versions/1.22.4/version.catalog.json` and updated `reference.catalog.json`/`INDEX_USAGE.md` so other agents can discover the 1.22.4 code index.
- Verified `tools/reference-query.py` lists `1.22.4`, lists its `code` index, and returns representative 1.22.4 results for `ICoreAPI`, `StartServer`, and `BlockEntity`.
- Flattened the API wrapper layout for all three built-in assemblies in both `versions/1.22.3/API` and `versions/1.22.4/API`; files now live directly under each `*_decompiled_<version>` wrapper instead of under a single same-name child folder.
- Rebuilt the 1.22.3 code index after flattening; it now contains `4,324` C# files, `4,465` types, and `21,242` members.
- Rebuilt the 1.22.4 code index after flattening; it still contains `3,425` C# files, `3,554` types, and `18,819` members.
- Copied the full 1.22.4 client asset payload from `vs_client_linux-x64_1.22.4.tar.gz` into `versions/1.22.4/Assets`, after verifying MD5 `c7d943c25de23f5055e485d86b01aaf8`.
- Built `versions/1.22.4/Index/assets.sqlite` and `versions/1.22.4/Index/assets.index.json`; the 1.22.4 asset index contains `19,982` files, including `9,404` JSON files with `9,059` parsed successfully and `345` parse-error rows retained for path/category lookup.
- Updated `.codex/skills/do-new-version`, `.codex/skills/vintage-story-reference-code-indexer`, `reference.catalog.json`, `versions/1.22.3/version.catalog.json`, `versions/1.22.4/version.catalog.json`, `INDEX_USAGE.md`, `README.md`, and `docs/MAINTAINING_REFERENCE.md` for the flattened API layout, full-asset copy rule, and required code/asset indexing.
- Investigated Rider/JetBrains `no index` labels on `versions/1.22.4`: confirmed they are solution/project indexing status, not missing SQLite reference indexes. The root solution only includes the 1.22.3 wrapper project; 1.22.4 has no version-level wrapper project or solution entry yet, while its code and asset SQLite indexes are present and queryable. Also confirmed `git ls-files versions/1.22.4` returns `0`.
- Added `versions/1.22.4/VintageStory.Reference.1.22.4.csproj` and nested it under a new `1.22.4` solution folder in `Vintage Story Reference.sln`. The project mirrors the non-compiling wrapper style used for 1.22.3 so Rider can index and browse the 1.22.4 reference payload without compiling decompiled sources.
- Verified `dotnet sln 'Vintage Story Reference.sln' list` shows both `versions\1.22.3\VintageStory.Reference.1.22.3.csproj` and `versions\1.22.4\VintageStory.Reference.1.22.4.csproj`.
- Verified `dotnet build 'Vintage Story Reference.sln' --no-restore` succeeds with `0` warnings and `0` errors after adding the 1.22.4 wrapper project.
- Renamed the project-local mod update skill from `.codex/skills/vintage-story-reference-mod-updater` to `.codex/skills/new-version-mods`; updated its frontmatter, UI metadata, `reference.catalog.json`, `docs/MAINTAINING_REFERENCE.md`, and `docs/Peters_Notes.md`.
- Updated `.codex/skills/do-new-version/SKILL.md` so new-version ingestion includes creating the browse-only version project and invoking `new-version-mods` to download/decompile current compatible selected reference mods before the final asset/code index builds.
- Deleted generated `versions/1.22.3/bin`, `versions/1.22.3/obj`, `versions/1.22.4/bin`, and `versions/1.22.4/obj` after verifying the resolved paths were inside the intended version folders.
- Updated `.codex/skills/new-version-mods/SKILL.md` so it explicitly creates `versions/<game-version>/mods` when missing and documents same-line `1.22.x` fallback behavior for patch-version targets with no exact Mod DB tag.
- Ran `new-version-mods` for `1.22.4`, seeding from the 1.22.3 mod folders. Downloaded and extracted 13 unique mod ids to `versions/1.22.4/mods`: `A Panserbjorn Story Rebjorn (1.0.0)` for mod id `panserbjornrebjorn`, `allclasses_2.3.0`, `betterruins_0.6.3`, `bunnypet_1.0.4`, `cats_5.0.1`, `configlib_1.12.0`, `critcherscalculator_1.2.3`, `jaunt_3.1.0`, `petai_5.1.1`, `primitivesurvival_5.0.6`, `rpvoicechat_2.5.8`, `vsimgui_1.2.7`, and `wolftaming_5.0.1`.
- Verified Mod DB release compatibility for the 1.22.4 mod set using the official Mod DB API. `betterruins_0.6.3` had an exact `1.22.4` tag. The other selected mods were the newest available `1.22.x` releases and were recorded as same-line fallbacks in `.codex/tmp/mod-updates/1.22.4/selected-releases.json`.
- Decompiled managed .NET DLLs found in the 1.22.4 mod set into per-mod `decompiled assembly` or `decompiled assemblies` folders. `vsimgui_1.2.7` included four native/non-.NET DLLs under `native/win`; the decompiler reported them as skipped.
- Rebuilt `versions/1.22.4/Index/code.sqlite`; it now contains `4,726` C# files, `5,085` types, and `25,624` members across 13 source roots, including API roots and decompiled mod roots.
- Rebuilt `versions/1.22.4/Index/assets.sqlite`; it now contains `22,070` files, including `11,035` JSON files with `10,686` parsed successfully and `349` parse-error rows retained for path/category lookup.
- Updated `versions/1.22.4/version.catalog.json`, `INDEX_USAGE.md`, and `README.md` for the 1.22.4 selected mod set and rebuilt index counts.
- Restored the 1.22.4 Panserbjorn reference mod from `.codex/tmp/mod-updates/1.22.4/extracted/panserbjornrebjorn_1.0.0` into the visible folder `versions/1.22.4/mods/A Panserbjorn Story Rebjorn (1.0.0)` after the normalized `panserbjornrebjorn_1.0.0` folder was no longer present. Rebuilt the 1.22.4 asset index afterward; counts remained `22,070` files, `10,686` parsed JSON files, and `349` JSON parse-error rows.
- Updated `.codex/skills/new-version-mods/SKILL.md` so future seeded mod updates preserve recognizable previous-version folder names when practical instead of switching to opaque mod-id folder names.
- Added `.codex/skills/do-new-web` for copying or merging curated web sources from the nearest previous Vintage Story version, adding the target `Web/sources.json` to Git, and building the target version's web documentation index.
- Updated `.codex/skills/do-new-version/SKILL.md` so future new-version ingestion invokes `do-new-web`, adds `versions/<version>/Web/sources.json` to Git, and validates code, asset, and web indexes before reporting completion.
- Copied `versions/1.22.3/Web/sources.json` to `versions/1.22.4/Web/sources.json`, built `versions/1.22.4/Index/web.sqlite` and `versions/1.22.4/Index/web.index.json`, and verified the 1.22.4 web index contains 4 sources and 87 sections.
- Updated `versions/1.22.4/version.catalog.json`, `reference.catalog.json`, `README.md`, `INDEX_USAGE.md`, and `docs/MAINTAINING_REFERENCE.md` so the 1.22.4 web index and `do-new-web` skill are discoverable.
- Staged `versions/1.22.4/Web/sources.json` with `git add` after the user noted that copied web source manifests must be part of the workflow.
- Deleted the redundant legacy `versions/1.22.3/README.md` per the user's request. Updated `versions/1.22.3/VintageStory.Reference.1.22.3.csproj` to remove the deleted README include and to include `version.catalog.json` plus `Web\**\*`, matching the 1.22.4 browse-only wrapper pattern.
- Updated `.codex/skills/new-version-mods/SKILL.md`, `.codex/skills/do-new-version/SKILL.md`, and `docs/MAINTAINING_REFERENCE.md` so future `do-new-version` runs normalize mod folders as `<modid>_<modversion>` and keep one latest compatible folder per mod id in each new version.
- Added a `.gitignore` rule for `versions/*/Index/` and updated the new-version/web workflows to leave generated SQLite indexes and generated index manifests uncommitted by default. `versions/1.22.4/Index/assets.sqlite` is larger than 100 MB, so pushing index databases without Git LFS would be unsafe.
- Corrected the generated-index Git policy so the `Index` folder itself is represented in Git by `versions/<version>/Index/.gitignore`, while generated `.sqlite` databases and index manifests remain ignored. Added this placeholder policy for `versions/1.22.3/Index` and `versions/1.22.4/Index`.
- Verified the pending 1.22.3 API layout changes match the current 1.22.4 flattened API pattern: `VintagestoryAPI_decompiled_1.22.3`, `VintagestoryLib_decompiled_1.22.3`, and `VSSurvivalMod_decompiled_1.22.3` contain source directly, have no same-name child project folders, and no `.sln`/`.slnx` files remain under `versions/1.22.3/API`.

### 2026-07-16

- Changed GitHub repository `caffeineLover/Vintage-Story-Reference` visibility from public to private with `gh repo edit`; verified GitHub reports `PRIVATE`.
- Pushed local `main` commits `cd267a53..865634aa` to `origin/main`.
- Verified the branch was not behind `origin/main` before pushing.
- Left the existing untracked reference, tool, mod, and index folders untouched.

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
