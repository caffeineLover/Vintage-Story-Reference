# Project Memory

Last updated: 2026-09-07

## Project overview

Vintage Story Reference is a local reference library for AI-assisted Vintage Story modding. It stores versioned decompiled assemblies, extracted assets, selected mod references, and generated SQLite indexes so other coding agents can query narrow facts without scanning the full reference trees.

## Current state

The repository currently retains indexed Vintage Story `1.22.6` and `1.22.7` reference versions. The root catalog
uses `1.22.7` as its default. The newest retained Vintage Story release is always the catalog default; future
new-version work must promote a newer retained release and update default-version documentation and examples in the
same change. Asset, code, and web index tooling lives under `tools/asset-index`, `tools/code-index`, and
`tools/web-index`, with generated index artifacts under each version's `Index` folder.

The `versions/1.22.7` payload contains the five flattened official API decompiles, full client assets, 18 selected
reference mods/tools, four curated web sources, and locally generated asset/code/web indexes. The 1.22.7 asset index
contains `22,837` files; the code index contains `8,607` files, `9,732` types, `95,780` members, `689,732` bounded
reference hints, and `836` literal registrations; the web index contains `4` sources and `99` sections.

The Config Lib JSON API GitHub wiki page, VS Village Mod DB page, RPVoiceChat Server Administration wiki page, and
RPVoiceChat GitHub README are indexed as curated external documentation for 1.22.6. The source manifest is
`versions/1.22.6/Web/sources.json`; the generated SQLite index is `versions/1.22.6/Index/web.sqlite`; the generated
manifest is `versions/1.22.6/Index/web.index.json`.

The same four curated source slugs are copied into `versions/1.22.7/Web/sources.json` and indexed in
`versions/1.22.7/Index/web.sqlite`. The generated 1.22.7 web manifest records `99` sections from the 2026-09-07 fetch.

Selected reference-mod metadata is tracked in `versions/1.22.6/mods.manifest.json`. It contains one entry for every
retained mod/tool folder and records identity, version, exact/fallback compatibility, source provenance, retained
asset/code availability, native-binary presence, indexed code roots, and explicit license-file presence. Query it
through `tools/mod-manifest/query_mod_manifest.py` or the `mod` target in `tools/reference-query.py`.

The matching `versions/1.22.7/mods.manifest.json` has `18` unique entries: `11` exact 1.22.7 releases, `6` same-line
fallbacks, and `1` external tool. All 18 package archives and extraction roots are preserved under
`.codex/tmp/mod-updates/1.22.7`.

`docs/QUERY_COOKBOOK.md` provides tested, task-shaped workflows for block/item lookup, asset-to-code tracing, class
registration research, recipe input/output lookup, vanilla-versus-mod comparison, mod provenance, and curated web
documentation.

Documented research lives under `docs/research/`. Each completed item has canonical Markdown plus same-basename,
Pandoc-generated LaTeX committed for reproducibility. XeLaTeX compiles the user-facing PDF locally; research PDFs are
ignored and not committed. `docs/research/README.md` defines the workflow and
`tools/research-docs/pandoc-pdf.yaml` provides the shared rendering defaults.

The Config Lib integration guide under `docs/research/mods/Config Lib` documents optional Config Lib `1.12.0`
support for Vintage Story `1.22.6`. It covers asset-only YAML/patch definitions, JSON-backed code-mod configuration,
runtime APIs and events, server/client ownership, migration, localization, verification, and the Betterer Prospecting
consumer pattern. Its Markdown and generated LaTeX are tracked research sources; its compiled PDF is local and
ignored.

The schematic library scaffold lives under `schematics/`. `library/` is a flat tracked collection for approved JSON
schematics, `inbox/` is an ignored local intake area, `manifest.json` is the tracked metadata source, and `Index/` is
reserved for ignored rebuildable query artifacts. The manifest is currently empty.

The code-index schema is now version `2`. Its `registrations` table stores supported literal Vintage Story
registration calls, their call-site evidence, containing type, and an unambiguous registered-type declaration link
when available. `tools/cross-reference/query_class_cross_reference.py` joins top-level asset `class` and `entityClass`
facts to that evidence at query time; the unified wrapper exposes the join as `class`, `classes`, or `xref`.

The repository also contains a root-level `prompts/` catalog for reusable Codex agent operating standards and reference-research workflows. Current prompt bundles cover persistent project memory, Vintage Story mod coding standards, project/Git hygiene, and Vintage Story research. `AGENTS.md` intentionally points at these project-local prompt files instead of the older sibling `Shared Agent Skills` folder.

The repository now has machine-readable discovery catalogs. `reference.catalog.json` is the root catalog for docs, prompts, skills, tools, and available versions. Each version should have `versions/<version>/version.catalog.json` for version-specific reference roots, indexes, manifests, and query/build scripts.

The `versions/1.22.6/API` and `versions/1.22.7/API` trees are populated with decompiled `VintagestoryAPI`,
`VintagestoryLib`, `VSCreativeMod`, `VSEssentials`, and `VSSurvivalMod` sources. Decompiled API trees intentionally use
flattened assembly wrapper folders and do not keep `.sln` files. Both version folders have copied full client assets,
selected reference mods/tools, copied curated web sources, plus code, asset, and web indexes. The root solution contains
browse-only wrapper projects for both retained versions.

Generated files inside `versions/<version>/Index` are local rebuildable output and should not be staged or pushed by
default. Each `Index` folder should exist in Git via a tracked `versions/<version>/Index/.gitignore` file that ignores
every generated file below it. `versions/1.22.6/Index/assets.sqlite` is over GitHub's normal 100 MB file limit. Commit
curated source manifests such as `versions/<version>/Web/sources.json`, not the generated SQLite indexes, unless the
user explicitly chooses a large-file strategy.

The `versions/1.22.3` and `versions/1.22.4` payloads were removed on 2026-08-14 at the user's request. Historical
memory entries still mention them as provenance, but current consumers should not query or open those version paths.

The working tree may contain untracked temporary staging files under `.codex/tmp`. Treat those as existing
user/reference material unless the user explicitly asks to clean or commit them.

## Active work

No active work is currently in progress. The 1.22.7 reference payload and indexes are complete and 1.22.7 is the
catalog default. The schematic library structure remains ready for intake, but no schematics or schematic index
tooling have been added yet.

## Durable technical knowledge

Agents should query the SQLite indexes and tracked mod manifest before reading raw reference trees. Asset questions
should start with `assets.sqlite`; C# and symbol questions should start with `code.sqlite`; selected-mod provenance and
compatibility questions should start with `mods.manifest.json`; curated external documentation questions should start
with `web.sqlite`.

PetAI 5.1.1 does not register a Vintage Story slash/chat command for setting pet obedience. Its `EntityBehaviorReceiveCommand.SetCommand` handles PetAI pet commands (`SIMPLE`, `COMPLEX`, and `AGGRESSIONLEVEL`) and gates command execution against `EntityBehaviorTameable.Obedience` using each command's `minObedience`, but does not mutate obedience. Obedience is stored in `domesticationstatus.obedience` by `EntityBehaviorTameable`; it is set to `1f` by the `magicbone` path, increased by accepted taming items and petting, and decreased by the disobedience tick.

Vintage Story 1.22.4 `Vintagestory.Common.ModCompilationContext` includes `Mods/VSCreativeMod.dll`, `Mods/VSEssentials.dll`, and `Mods/VSSurvivalMod.dll` in the built-in Roslyn reference set used when compiling source-code mods. The decompiled `VSSurvivalMod.csproj` also references both `VSEssentials` and `VSCreativeMod`.

For routine consumer queries, prefer `tools/reference-query.py`. It reads the catalogs, resolves the selected version,
and delegates to the existing source-specific query helpers. It supports asset, code, mod/mods, and web targets;
`--list-manifests` discovers curated manifests. The source-specific scripts remain the specialized interfaces and
should not be duplicated inside the wrapper.

The newest retained Vintage Story release is always `reference.catalog.json`'s `default_game_version`. When a newer
release is added, promote it in the root catalog and update default-version documentation and examples in the same
change. Backfilling an older release does not change the default.

The 1.22.6 mod manifest has `18` entries: `4` exact 1.22.6 Mod DB releases, `13` same-line fallbacks, and `1` external
tool. All 18 manifest folders exist, every indexed code-root name in the manifest matches `code.index.json`, and no
explicit `LICENSE*` or `COPYING*` file is present in the retained mod folders. License-file absence is only a package
observation, not a legal conclusion about upstream licensing.

`tools/code-index/query_code_index.py --reference` exposes the existing conservative `references_hint` table. These
results are bounded identifier hints, not a complete call graph; an empty result does not prove a symbol is unused.

The 1.22.6 schema-2 code index contains `820` supported literal registrations: `258` block, `129` item, `140` block
entity, `65` block behavior, `40` collectible behavior, `52` block-entity behavior, `78` entity behavior, `42` entity,
`5` entity-renderer, and `11` inventory registrations. All 820 currently resolve to one best indexed type declaration.
The category-aware class join produces `710` asset rows across `395` distinct class keys.

Other Codex-assisted projects should read `INDEX_USAGE.md` when they need to use this repository as a Vintage Story knowledge source. They can paste `prompts/Vintage Story Research/Add to AGENTS.md` into their own `AGENTS.md` to learn the index-first research workflow. They should read or adopt the other files under `prompts/` when they need the user's agent operating standards.

The Config Lib JSON API upstream source is the GitHub wiki page at `https://github.com/maltiez2/vsmod_configlib/wiki/JSON-API`. Its raw Markdown endpoint is `https://raw.githubusercontent.com/wiki/maltiez2/vsmod_configlib/JSON-API.md`.

Config Lib `1.12.0` discovers assets named exactly `configlib-patches.json` in the Vintage Story `config` asset
category. Without a `file` property it uses a domain YAML file; with `file` it manages the named JSON file under
`ModConfig`. Its public runtime contracts include `IConfigProvider`, `IConfig`, and `ISetting`; the provider exposes
config-loaded and setting-changed events, while config/setting assignment helpers copy current values onto matching
object members. Optional code integrations should retain ordinary config loading, guard Config Lib resolution with
`IsModEnabled("configlib")`, assign initial values after `ConfigsLoaded`, and keep C#/asset/Config Lib defaults aligned.

The VS Village upstream source is the Vintage Story Mod DB page at `https://mods.vintagestory.at/vsvillage`. The builder extracts the page's description tab via the `moddb_description` HTML scope so the searchable index focuses on gameplay/design-reference content and current Mod DB metadata instead of full site navigation or file tables.

The retired 1.22.3 and 1.22.4 payloads are no longer available on disk. Use current 1.22.7 indexes for new work, and
use Git history only if old-version provenance is required.

As of the 2026-08-16 Firearms Fork and Overhaul Lib Legacy Compat addition, the 1.22.6 asset index contains `22,792`
files, including `11,618` JSON files with `11,269` parsed successfully and `349` parse-error rows retained for
path/category lookup. The 1.22.6 code index was rebuilt with parser tool `1.1.4` on 2026-09-07 and contains `8,428`
C# files, `9,543` types, `94,449` members, and `679,367` bounded reference hints across `20` source roots. The 1.22.6
web index contains `4` copied source entries split into `84` sections. Generated manifests under
`versions/1.22.6/Index` remain the exact source of truth for timestamps, roots, hashes, and detailed counts.

The 1.22.7 asset index contains `22,837` files, including `11,660` JSON files with `11,311` parsed successfully and
`349` parse-error rows retained for path/category lookup. The 1.22.7 code index contains `8,607` C# files, `9,732`
types, `95,780` members, `689,732` bounded reference hints, and `836` registrations across `20` source roots. All 836
registrations resolve to an indexed type declaration. The category-aware class join produces `722` asset rows across
`407` case-insensitive class keys. The 1.22.7 web index contains `4` copied sources split into `99` sections. Generated
manifests under `versions/1.22.7/Index` remain the exact source of truth.

RPVoiceChat 2.5.8 is indexed from `versions/1.22.6/mods/rpvoicechat_2.5.8`. Its asset root is
`versions/1.22.6/mods/rpvoicechat_2.5.8/assets`, and its C# source root is
`versions/1.22.6/mods/rpvoicechat_2.5.8/decompiled assemblies/RPVoiceChat`.

For Opus voice-chat implementation research, start with the code index root
`rpvoicechat_2.5.8/decompiled assemblies`. Useful indexed symbols and paths include `RPVoiceChat.Audio.OpusCodec` at
`versions/1.22.6/mods/rpvoicechat_2.5.8/decompiled assemblies/RPVoiceChat/RPVoiceChat/Audio/OpusCodec.cs`,
`RPVoiceChat.Audio.MicrophoneManager`, `RPVoiceChat.Audio.PlayerAudioSource`, and
`RPVoiceChat.Networking.AudioPacket.Codec`. Verified source behavior: `MicrophoneManager` defaults capture to `Opus`;
the world config toggle `encode-audio` chooses `Opus` versus `Dummy`; `PlayerAudioSource` constructs `OpusCodec` when
packet codec names `Opus`; `AudioPacket` carries a `Codec` property; `OpusCodec` uses Concentus `OpusCodecFactory`,
`OPUS_APPLICATION_VOIP`, voice signal mode, SILK-only mode, VBR, and separate normal/broadcast bitrate settings.

RPVoiceChat upstream documentation sources are now indexed as `rpvoicechat-server-administration` and `rpvoicechat-github-readme`. The Server Administration page covers server config path `ModConfig/RPVoiceChat/rpvoicechat-server.json`, network settings, codec bitrate settings, communication systems, sound-emitting object ranges, and `/rpvc` admin commands. The README records project overview, usage/configuration entry points, developer documentation links, and Opus/RNNoise licensing notes.

Vintage Story 1.22.4 API sources were produced from the public Windows dedicated server ZIP `vs_server_win-x64_1.22.4.zip` from the Vintage Story CDN. The downloaded ZIP was verified against MD5 `162941f6a5d6dbc576cce700f50d16bf`. Decompilation used `ilspycmd` `10.1.1.8388` with `--nested-directories`, `--use-varnames-from-pdb`, and reference paths to the extracted server root, `Lib`, and `Mods`. The transient temp-derived `<HintPath>` entries in the generated SDK-style `.csproj` files were removed before keeping the output. The 1.22.4 API wrappers are flattened, so files live directly under `VintagestoryAPI_decompiled_1.22.4`, `VintagestoryLib_decompiled_1.22.4`, `VSCreativeMod_decompiled_1.22.4`, `VSEssentials_decompiled_1.22.4`, and `VSSurvivalMod_decompiled_1.22.4`.

Vintage Story 1.22.4 full assets were copied from the public Linux client archive `vs_client_linux-x64_1.22.4.tar.gz` from the Vintage Story CDN. The downloaded archive was verified against MD5 `c7d943c25de23f5055e485d86b01aaf8`. The Windows dedicated server package contains a reduced `assets` folder; do not use server-only assets as the final full `versions/<version>/Assets` payload.

Vintage Story 1.22.6 API sources were produced from the public Windows dedicated server ZIP
`vs_server_win-x64_1.22.6.zip` from the Vintage Story CDN. The downloaded ZIP was verified against MD5
`59e0aeaa213359025d51e67f26f7f255`. Decompilation used `ilspycmd` `10.1.1.8388` with `--nested-directories`,
`--use-varnames-from-pdb`, and reference paths to the extracted server root, `Lib`, and `Mods`. Transient temp-derived
`<HintPath>` entries were removed before keeping the output. The 1.22.6 API wrappers are flattened under
`VintagestoryAPI_decompiled_1.22.6`, `VintagestoryLib_decompiled_1.22.6`,
`VSCreativeMod_decompiled_1.22.6`, `VSEssentials_decompiled_1.22.6`, and
`VSSurvivalMod_decompiled_1.22.6`.

Vintage Story 1.22.6 full assets were copied from the public Linux client archive
`vs_client_linux-x64_1.22.6.tar.gz` from the Vintage Story CDN. The downloaded archive was verified against MD5
`c00c436cab612b436747b4bb5f39049b`. The Windows dedicated server package contains a reduced `assets` folder; do not
use server-only assets as the final full `versions/<version>/Assets` payload.

Vintage Story 1.22.7 API sources were produced from the public Windows dedicated server ZIP
`vs_server_win-x64_1.22.7.zip` from the Vintage Story CDN. The downloaded ZIP was verified against the official MD5
`c5bcd15a558041533ed3b88f707bb19e`; its locally computed SHA-256 is
`E72F27DE387C53342A6AEDAFB181FDFB154816E263FEC41C31823C9AFD69C65C`. Decompilation used `ilspycmd`
`10.1.1.8388` with nested directories, PDB variable names, and reference paths to the extracted server root, `Lib`,
and `Mods`. The five flattened wrappers contain `1,122`, `1,009`, `51`, `524`, and `1,296` C# files respectively for
`VintagestoryAPI`, `VintagestoryLib`, `VSCreativeMod`, `VSEssentials`, and `VSSurvivalMod`. Temporary `<HintPath>`
entries were removed, and no `.sln` or `.slnx` files remain below `versions/1.22.7/API`.

Vintage Story 1.22.7 full assets were copied from the public Linux client archive
`vs_client_linux-x64_1.22.7.tar.gz`. The download was verified against the official MD5
`ecfa56411b0a2912ebba29839fa98a3f`; its locally computed SHA-256 is
`483CC6827A50C08C5C41CCD0262C6F1223C104E54CA7135C46605278D89F5DE1`. The retained base asset tree matches the
extracted archive at `19,983` files and `837,864,218` bytes, with `creative`, `game`, and `survival` directly below
`versions/1.22.7/Assets`.

Rider/JetBrains `no index` labels on version folders refer to IDE solution/project indexing, not the repository's SQLite reference indexes. On 2026-07-21, `versions/1.22.4/VintageStory.Reference.1.22.4.csproj` was added and nested under a `1.22.4` solution folder beside the existing `1.22.3` wrapper. Version wrapper projects disable default compile/resource items and include `version.catalog.json`, `API`, `Assets`, `mods`, and `Web` as `None` items so Rider can browse the reference payload without compiling decompiled game sources. If Rider still displays stale `no index` labels after this change, reload the solution or refresh IDE indexes.

Future new Vintage Story releases should use the project-local `do-new-version` Codex skill. It records the preferred workflow: create a browse-only `versions/<version>/VintageStory.Reference.<version>.csproj` and solution entry, create `versions/<version>/API` and `versions/<version>/Assets`, decompile `VintagestoryAPI.dll`, `VintagestoryLib.dll`, `Mods/VSCreativeMod.dll`, `Mods/VSEssentials.dll`, and `Mods/VSSurvivalMod.dll` from the official release package into flattened wrapper folders, copy full client assets, use `new-version-mods` to download/decompile current compatible selected reference mods, use `do-new-web` to copy/index curated web sources from the nearest previous version, avoid keeping decompiler-generated `.sln` or `.slnx` files, build the code, asset, and web indexes, and update catalogs so other agents can discover the indexed API, assets, mod references, and web references.

The project-local skill for reference mod population is `.codex/skills/new-version-mods`. It replaced the longer `vintage-story-reference-mod-updater` name on 2026-07-21. Use it independently for `versions/<version>/mods` update work, and invoke it from `do-new-version` before final code/asset index builds so mod assets and decompiled mod code are included in the new version's indexes.

`new-version-mods` now creates `versions/<game-version>/mods` if it is missing. For patch-version targets, it prefers exact Mod DB tags and may use the newest same major/minor release as a documented reference fallback when no exact patch tag exists. For new target versions, it should normalize completed mod folder names as `<modid>_<modversion>` from `modinfo.json` or Mod DB metadata and keep exactly one latest compatible folder per mod id. Existing duplicate mod folders should be reported and removed only when cleanup was explicitly requested.

The project-local skill for carrying curated web documentation forward is `.codex/skills/do-new-web`. Use it independently for `versions/<version>/Web` source copying, merging, refreshing, or indexing work, and invoke it from `do-new-version` before final validation so the new version has `web.sqlite` and `web.index.json` alongside its asset and code indexes. `versions/<version>/Web/sources.json` is curated version metadata and should be added to Git after it is created or merged; do not leave it only as an untracked local file.

Vintage Story 1.22.4 selected reference mods were seeded from the 1.22.3 mod set on 2026-07-21 and extended with TentBag-Reworked on 2026-07-25 and Betterer Prospecting on 2026-08-10. The target contains 15 unique mod ids under `versions/1.22.4/mods`: `A Panserbjorn Story Rebjorn (1.0.0)` for mod id `panserbjornrebjorn`, `allclasses_2.3.0`, `bettererprospecting_3.4.5`, `betterruins_0.6.3`, `bunnypet_1.0.4`, `cats_5.0.1`, `configlib_1.12.0`, `critcherscalculator_1.2.3`, `jaunt_3.1.0`, `petai_5.1.1`, `primitivesurvival_5.0.6`, `rpvoicechat_2.5.8`, `tentbagreworked_3.2.4`, `vsimgui_1.2.7`, and `wolftaming_5.0.1`. `betterruins_0.6.3` was the only selected release with an exact Mod DB `1.22.4` tag; Betterer Prospecting 3.4.5 declares a `game` dependency of `1.22.0`, and TentBag-Reworked 3.2.4 and the other non-exact selected mods are same-line `1.22.x` fallbacks because Mod DB did not publish an exact `1.22.4` tag for those mod ids at selection time. The earlier seeded selection/download report is `.codex/tmp/mod-updates/1.22.4/selected-releases.json`; Betterer Prospecting was added later from a local ZIP and is documented in the 2026-08-10 session entry.

Vintage Story 1.22.6 selected reference mods were seeded from the 1.22.4 mod set on 2026-08-10. The target contains 18
unique reference entries under `versions/1.22.6/mods`: `allclasses_2.3.1`, `bettererprospecting_3.4.5`,
`betterruins_0.6.3`, `bunnypet_1.0.4`, `cats_5.0.1`, `configlib_1.12.0`, `critcherscalculator_1.2.3`,
`firearmsfork_1.10.1`, `jaunt_3.1.0`, `overhaulliblegacycompat_1.2.5`, `panserbjornrebjorn_1.0.0`, `petai_5.1.1`,
`primitivesurvival_5.1.0`, `rpvoicechat_2.5.8`, `simplevsmanager_2.3.0`, `tentbagreworked_3.2.4`, `vsimgui_1.2.7`, and
`wolftaming_5.0.1`. Betterer Prospecting
3.4.5, Primitive Survival 5.1.0, RPVoiceChat 2.5.8, and TentBag-Reworked 3.2.4 had exact Mod DB `1.22.6` releases.
The other selected mods are same-line `1.22.x` fallbacks. Simple VS Manager 2.3.0 is a Mod DB `externaltool`, not a
normal game mod package; it has no `modinfo.json` or game-version tag and was added for mod-manager workflow research.
The original selected-mod selection/download/decompilation report is `.codex/tmp/mod-updates/1.22.6/selected-releases.json`;
Simple VS Manager provenance is recorded in `versions/1.22.6/mods/simplevsmanager_2.3.0/REFERENCE_SOURCE.md`.

Vintage Story 1.22.7 selected reference mods were seeded from the 1.22.6 manifest on 2026-09-07. The target contains
18 unique folders under `versions/1.22.7/mods`. Eleven packages were updated: Allclasses 2.3.2, BetterEr Prospecting
3.4.8, Cats 5.0.2, Config Lib 1.13.2, Critcher's Calculator 1.3.0, Firearms 1.10.2, Overhaul Lib Legacy Compat 1.2.10,
A Panserbjørn Story: Rebjørn 1.0.1, Primitive Survival 5.1.3, RP Voice Chat 2.6.5, and TentBag Reworked 3.2.5. The
remaining selected versions are BetterRuins 0.6.3, Pet Bunny 1.0.4, Jaunt 3.1.0, Pet AI 5.1.1, Simple VS Manager
2.3.0, Dear ImGui 1.2.7, and Wolf Taming 5.0.1. Eleven releases explicitly list 1.22.7; Pet Bunny, Cats, Jaunt,
Pet AI, Dear ImGui, and Wolf Taming are documented same-line fallbacks; Simple VS Manager remains an external tool.
Across the set, `38` managed DLLs were decompiled, `10` native DLLs were retained without decompilation, and native
binaries occur in RP Voice Chat, Simple VS Manager, and Dear ImGui. The detailed report is
`.codex/tmp/mod-updates/1.22.7/selected-releases.json`.

TentBag-Reworked 3.2.4 default player behavior: an Empty Tent Bag is crafted from one cloth, four rope, and four sticks; cloth color controls bag color. An empty bag packs a default 7x7x7 area above the top face of the clicked ground block and becomes a Packed Tent Bag. A packed bag unpacks above the clicked block and returns to an empty bag. Survival-mode pack and unpack actions consume satiety at the default `BuildEffort` value of `2` per saved non-air block. Defaults include packed-tent hologram preview, empty-bag grab-area preview, 90-degree rotation, waypoint creation on unpack, waypoint removal on pack, and banned blocks such as liquids, crops, ores, rocks, anvils, and forges. The player-facing guide is `docs/research/mods/Tentbag Reworked/TENTBAG_REWORKED_PLAYER_GUIDE.md`.

TentBag-Reworked also ships `assets/tentbagreworked/config/tent.json`, a `7 x 3 x 6` WorldEdit/BlockSchematic sample tent. `TentBagReworkedModSystem.EnsureTentSchematicExists` copies it to `<GamePaths.DataPath>/WorldEdit/tent.json` on server startup if the target file is absent. It is not a pre-packed tent bag or player-granted item; players/admins with WorldEdit can use it as a template and then pack it into a tent bag if desired.

Vintage Story 1.22.6 WorldEdit requires creative mode and the `worldedit` privilege. Server-side schematic import loads filenames from the game/server data `WorldEdit` folder. For the TentBag-Reworked starter tent, a direct placement workflow is to mark a start/end point, then run `/we import tent.json BottomCenter`; `BottomCenter` centers the `7 x 3 x 6` schematic horizontally on the marker. The interactive import-tool workflow is `/we tool import`, `/we ims tent.json`, optional `/we imr 90` or `/we imflip`, then right-click or `/we apply`/`/we place`/`/we commit`.

Vintage Story 1.22.4 web sources were copied from `versions/1.22.3/Web/sources.json` on 2026-07-21 and re-indexed into `versions/1.22.4/Index/web.sqlite`. The 1.22.4 web index contains the same 4 curated source slugs as 1.22.3, split into 87 sections: `configlib-json-api`, `vsvillage-moddb`, `rpvoicechat-server-administration`, and `rpvoicechat-github-readme`.

Vintage Story 1.22.6 web sources were copied from `versions/1.22.4/Web/sources.json` on 2026-08-10 and re-indexed into
`versions/1.22.6/Index/web.sqlite`. The 1.22.6 web index contains the same 4 curated source slugs, split into 84
sections: `configlib-json-api`, `vsvillage-moddb`, `rpvoicechat-server-administration`, and
`rpvoicechat-github-readme`.

Vintage Story 1.22.4 display cases (`displaycase` and `talldisplaycase`) were verified to use `BlockDisplayCase`,
`DisplayCase`, and the `UnstableFalling` behavior. Their JSON set `sidesolid: { all: false }`, so the default
`Block.CanAttachBlockAt` returned false for the top face. Re-query 1.22.7 before applying this to current work; the
old 1.22.4 payload has been retired.

Betterer Prospecting 3.4.5 settings verified on 2026-08-10: `BoreholeScansStone` defaults false and controls whether Borehole mode adds rock/stone results after ore checks; `BoreholeDmg` is the durability cost returned by a valid Borehole sample, although `ModConfig` initializes it to `2` while the Config Lib manifest advertises default `4`; `ReprospectParellelism` is intentionally misspelled in the mod assets and controls `Parallel.ForEachAsync` concurrency for `/btrpr reprospect`, using the explicit value when greater than zero or `Environment.ProcessorCount / 4` when set to zero.

For Betterer Prospecting 3.4.5, when `ProximitySearchRadius = 5`, the balanced companion value is
`BoreholeRadius = 5`: Proximity scans a radius-5 sphere, while Borehole scans a vertical cylinder with a horizontal
radius. Radius 5 checks 81 horizontal cells per Y layer; the bundled default radius 8 checks 197 cells per layer, about
2.43 times as many before accounting for Borehole mode's full downward depth.

## Architecture and design decisions

The web documentation index follows the existing project pattern of generated SQLite indexes plus small query helpers. It stores external documentation as dated searchable index content instead of adding separate local HTML or Markdown snapshots.

The code indexer now recognizes project-style mod source roots when no `decompiled` folder is present. It scans each mod folder for `.csproj` directories that contain `.cs` files, skips generated/editor folders and `assets`, and indexes those directories as mod source roots. This was required for RPVoiceChat because its C# files live under `rpvoicechat_2.5.8/RPVoiceChat` rather than a folder named `decompiled`.

The Markdown splitter ignores headings inside fenced code blocks. This matters for the Config Lib page because YAML examples contain decorative `###` lines that should remain example text rather than becoming documentation headings.

The web index builder supports both Markdown-like sources and scoped HTML sources. For HTML pages, it converts selected HTML into Markdown-like text before sectioning so headings, lists, and paragraphs remain searchable through the same `sections` and `web_text` tables.

Project-specific structure decision: `prompts/` is intentionally kept at the repository root as a reusable prompt catalog, and `.codex/skills/` is intentionally kept for project-local Codex skills. This overrides the generic project standard that ordinary documentation belongs under `docs/`.

The Vintage Story Research prompt is deliberately separate from `INDEX_USAGE.md`: `INDEX_USAGE.md` documents this repository's current indexes and commands, while `prompts/Vintage Story Research/VINTAGE_STORY_RESEARCH.md` is the reusable instruction set that can be referenced or copied into other projects.

The drop-in `prompts/Vintage Story Research/Add to AGENTS.md` snippet should stay short, version-agnostic, and broadly scoped. It should tell consuming projects when to use the Vintage Story Reference archive and where to read the full research prompt. It should not name one indexed mod as though it is special; detailed examples, index paths, version-selection rules, and research mechanics belong in `prompts/Vintage Story Research/VINTAGE_STORY_RESEARCH.md`, `INDEX_USAGE.md`, manifests, or local examples.

Architecture decision: keep consumer docs, maintainer docs, catalogs, scripts, and prompts separate. `README.md` remains a short overview; `INDEX_USAGE.md` is the current concrete consumer map; `docs/ARCHITECTURE.md` explains the system shape; `docs/MAINTAINING_REFERENCE.md` explains maintenance workflows; catalogs are small machine-readable pointers; generated manifests remain the detailed generated-index source of truth.

Architecture decision: selected-mod provenance belongs in a tracked per-version JSON manifest rather than another
generated SQLite database. Compatibility and source selection require curation and must survive a clean clone;
asset/code facts remain in rebuildable SQLite indexes. `version.catalog.json` exposes the manifest separately from
`indexes`, while the unified wrapper hides that storage distinction behind the `mod` query target.

Architecture decision: literal registration evidence belongs in `code.sqlite`, while asset-to-registration links are
computed at query time instead of stored in a fourth generated database. This keeps asset and code rebuilds
independent, avoids duplicated relationship state, and lets version catalogs describe the two required inputs. The
join is category-aware so identical strings are compared only against the correct block, item, entity, or block-entity
registry.

Architecture decision: keep the initial curated schematic library flat under `schematics/library/`. Classification,
authorship, compatibility, dependency, dimension, tag, provenance, and licensing fields belong in the tracked
`schematics/manifest.json`, not directory names. The ignored inbox separates unreviewed drops from approved content;
the ignored index directory separates rebuildable query data from authoritative schematic JSON files.

## External interfaces and integrations

The web index source manifest is `versions/<version>/Web/sources.json`. Each source entry includes a public page URL, raw fetch URL, title, source type, applicability notes, and last-known edit date when available. For new versions, `do-new-web` copies or merges every source entry from the nearest previous version's source manifest before rebuilding the target `web.sqlite`.

The RPVoiceChat Server Administration upstream source is `https://github.com/Ridderrasmus/RPVoiceChat/wiki/Server-Administration`; its raw Markdown endpoint is `https://raw.githubusercontent.com/wiki/Ridderrasmus/RPVoiceChat/Server-Administration.md`. The RPVoiceChat GitHub README source is `https://github.com/Ridderrasmus/RPVoiceChat`; the indexed raw Markdown endpoint is `https://raw.githubusercontent.com/Ridderrasmus/RPVoiceChat/development/README.md`.

Pandoc `3.10.2` is installed under `%ProgramFiles%\Pandoc`, and MiKTeX `25.12` provides MiKTeX-XeTeX `4.16` under
`%LOCALAPPDATA%\Programs\MiKTeX`. Neither installation was on the active shell `PATH` on 2026-08-14, so the documented
workflow prefers `Get-Command` and then probes these environment-relative installation locations. MiKTeX automatic
missing-package installation is enabled with `[MPM]AutoInstall=1`.

## Known failed approaches

`dotnet clean "Vintage Story Reference.sln" --no-restore` fails under the installed .NET 10 SDK with
`MSB1001: Unknown switch`. `dotnet clean "Vintage Story Reference.sln"` succeeds and removes the generated build
outputs.

The first Pandoc PDF attempt on 2026-08-14 failed because no LaTeX engine was installed. MiKTeX was installed later
that day. The first XeLaTeX compile then waited indefinitely at first-use package installation until
`initexmf --set-config-value='[MPM]AutoInstall=1'` enabled non-interactive missing-package installation. Direct
XeLaTeX compilation now succeeds. Do not replace the requested LaTeX rendering with an HTML/browser renderer.

## Bugs, risks, and limitations

The web index is a dated snapshot of fetched upstream content. If current documentation matters, rebuild the index or check the upstream URL.

Research `.tex` files are intentionally committed generated sources at the user's explicit direction. Research PDFs
under `docs/research/` are ignored local reader copies and must not be staged. Update Markdown first, regenerate the
LaTeX source, then compile and visually review the PDF; never hand-edit generated LaTeX or PDF output.

The schematic library has no ingestion validator or search index yet. Third-party downloads must not enter the tracked
library without recorded provenance and clear redistribution permission; a public download alone is insufficient.

The asset index rebuild for 1.22.3 can take longer than two minutes. On 2026-07-14 it timed out at a 120-second command limit but succeeded with a 300-second limit in about 220 seconds.

The mod manifest is curated metadata and can become stale when mod folders change. Every mod update and new-version
workflow must update it and verify one entry per retained mod/tool folder.

Registration extraction is intentionally incomplete. It requires a supported registration method with a literal
string key and `typeof(...)` value. Dynamic keys, helper-wrapped calls, factories, reflection, JSON inheritance, and
runtime patches are not inferred. Empty registration or class-cross-reference results do not prove that no runtime
relationship exists.

The code index remains a conservative regex parser rather than a full C# lexer. Complex interpolated or raw strings
with nested literal braces can still defeat structural masking; no such imbalance affects the retained 1.22.6 or
1.22.7 corpora as currently indexed. It also deliberately omits declarations whose terminating semicolon appears
alone on the following line, avoiding false methods from multiline expression continuations; the retained corpora do
not use that declaration layout. Treat query results as navigation hints and open the returned source for exact
behavior.

## Open questions

No open technical questions are currently recorded.

## Testing and verification

Current 1.22.7 verification performed on 2026-09-07:

- All three SQLite databases passed `PRAGMA integrity_check`; their table counts match the generated manifests.
- The tracked mod manifest matches all 18 target folders, has unique ids and folders, and matches all 17 available
  `modinfo.json` id/version pairs. Exact releases include a 1.22.7 tag; same-line fallbacks do not claim one.
- API validation found the expected root `.csproj` and substantial C# output in all five wrappers, with no API
  `.sln`/`.slnx`, `<HintPath>`, or staging-path residue. The base asset copy matches its extracted source by file count
  and total bytes.
- `tools/reference-query.py` lists both versions with 1.22.7 as the default, all 1.22.7 indexes, the class
  cross-reference, and the mod manifest. Representative mod, copper, recipe, `ICoreAPI`, `StartServerSide`,
  `BlockDisplayCase`, `BlockEntity`, Config Lib, and RPVoiceChat queries returned expected results.
- All 13 code-index parser regression tests pass through root `unittest` discovery, both retained code databases use
  tool `1.1.4`, and exact `StartServerSide` member queries return the expected `ModSystem` declaration in both versions.
- `dotnet build "Vintage Story Reference.sln"` succeeded with zero warnings and zero errors.
- Maintained metadata and documentation pass scoped `git diff --cached --check`; the complete imported API, asset, and
  mod payload reports decompiler/upstream whitespace that was intentionally preserved.

Current 1.22.6 verification performed on 2026-09-07:

- The code index passed SQLite integrity checks with `8,428` files, `9,543` types, `94,449` members, `679,367`
  reference hints, `820` registrations, and `20` source roots. Queries returned `Firearms.FirearmsModSystem`,
  Firearms item registrations, and Overhaul Lib Legacy Compat types from the new roots.
- The asset index passed SQLite integrity checks with `22,792` files and returned Firearms assets from the
  `maltiezfirearms` domain plus Overhaul Lib Legacy Compat configuration and patch assets.
- The tracked mod manifest has `18` unique entries for `18` retained folders; all local metadata paths and indexed
  code-root names exist. Firearms Fork 1.10.1 and Overhaul Lib Legacy Compat 1.2.5 are current Mod DB releases recorded
  as same-line fallbacks for target 1.22.6.
- `dotnet build "Vintage Story Reference.sln" --no-restore` succeeded with zero warnings and errors. The maintained
  metadata/documentation changes pass `git diff --check`; the complete staged third-party payload reports whitespace
  already present in upstream translation, shape, shader, and metadata files, which was preserved without rewriting.

The Config Lib integration research was rendered on 2026-08-15 with Pandoc and three XeLaTeX passes. The final PDF
has `15` physical pages including the title page, and the final LaTeX log has zero overfull boxes. Visual spot checks
covered the title, contents, JSON example, runtime API section, query examples, and implementation checklist.

Current 1.22.6 verification performed on 2026-08-14:

```powershell
dotnet build "Vintage Story Reference.sln" --no-restore
python -B tools\reference-query.py --list-versions
python -B tools\reference-query.py --version 1.22.6 --list-indexes
python -B tools\reference-query.py --version 1.22.6 --list-manifests
python -B tools\reference-query.py --version 1.22.6 mod --compatibility exact --limit 10
python -B tools\reference-query.py --version 1.22.6 mod --has-native-binaries yes --limit 10
python -B tools\reference-query.py --version 1.22.6 asset --fact-key output --fact-value ingot-brass --limit 2
python -B tools\reference-query.py --version 1.22.6 code --type BlockDisplayCase --limit 2
python -B tools\reference-query.py --version 1.22.6 code --registration BlockDisplayCase --limit 2
python -B tools\reference-query.py --version 1.22.6 class --asset-class BlockDisplayCase --limit 2
python -B tools\reference-query.py --version 1.22.6 code --reference ICoreAPI --root VSEssentials --limit 2
python -B tools\reference-query.py --version 1.22.6 web --source configlib-json-api --search "conditional patches" --limit 2
git diff --check
```

The solution build succeeded with zero warnings and errors. Manifest validation confirmed `16` entries for `16`
folders, unique mod ids/folders, matching `modinfo.json` ids and versions, matching asset flags, and valid code roots.
The representative asset, code, mod, and web queries returned expected candidates. `git diff --check` returned only
the repository's expected LF-to-CRLF working-copy warnings.

Schema-2 registration validation confirmed SQLite integrity, `766` unique call-site rows, containing-type and
registered-type links for all 766 rows, no duplicate evidence tuples, valid source paths/lines, and a class join of
`674` rows across `386` distinct keys. Synthetic extraction confirmed commented and dynamic-key calls are excluded
while multiline literal calls retain the correct starting line. Wrapper aliases, text/JSON output, backward-compatible
asset/code/mod/web queries, and a zero-warning, zero-error solution build all succeeded. `dotnet clean` then removed
the generated build outputs.

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
python -B tools\reference-query.py --version 1.22.7 --list-indexes
python -B tools\reference-query.py --version 1.22.7 mod --compatibility exact --limit 20
python -B tools\reference-query.py --version 1.22.7 asset --search copper --limit 20
python -B tools\reference-query.py --version 1.22.7 code --type ICoreAPI --limit 20
python -B tools\reference-query.py --version 1.22.7 web --search "configlib-patches json" --limit 20
python -B tools\reference-query.py --version 1.22.6 mod --mod-id tentbagreworked --format json
python -B tools\reference-query.py --version 1.22.6 asset --search copper --limit 20
python -B tools\reference-query.py --version 1.22.6 code --type ICoreAPI --limit 20
python -B tools\reference-query.py --version 1.22.6 web --search "configlib-patches json" --limit 20
```

## Important files and code paths

- `INDEX_USAGE.md` - Instructions for other AI-enabled projects that consume this reference repository.
- `docs/QUERY_COOKBOOK.md` - Tested, task-shaped and cross-index query recipes for common Vintage Story research.
- `docs/research/README.md` - Required Markdown/LaTeX/local-PDF research organization and rendering workflow.
- `tools/research-docs/pandoc-pdf.yaml` - Shared Pandoc defaults for readable, letter-sized XeLaTeX PDFs.
- `docs/research/mods/Config Lib/CONFIG_LIB_INTEGRATION_GUIDE.md` - Canonical Config Lib `1.12.0` optional
  integration guide for content mods and code mods targeting the retained Vintage Story `1.22.6` reference.
- `docs/research/mods/Config Lib/CONFIG_LIB_INTEGRATION_GUIDE.tex` - Committed Pandoc-generated LaTeX source for
  the local Config Lib integration reader PDF.
- `docs/SCHEMATIC_LIBRARY.md` - Flat schematic-library layout, intake, naming, licensing, and consumption policy.
- `schematics/inbox` - Ignored local drop area for unreviewed schematic files.
- `schematics/library` - Flat tracked directory for approved source schematic JSON files.
- `schematics/manifest.json` - Tracked curated schematic metadata; initially contains an empty `schematics` array.
- `schematics/Index` - Ignored location reserved for future generated schematic query artifacts.
- `tools/asset-index` - Asset index builder and query helper.
- `tools/code-index` - Decompiled C# code index builder and query helper.
- `tests/test_build_code_index.py` - Root-discoverable regression coverage for conservative type/member parsing.
- `tools/cross-reference/query_class_cross_reference.py` - Category-aware query-time join from asset class facts to
  literal C# registrations and resolved type declarations.
- `tools/mod-manifest/query_mod_manifest.py` - Query helper for tracked selected-mod compatibility and provenance.
- `tools/web-index` - Curated external documentation index builder and query helper.
- `tools/reference-query.py` - Unified query wrapper that resolves catalog paths and delegates to source-specific query helpers.
- `prompts` - Reusable Codex agent prompt standards for project memory, coding standards, project/Git hygiene, and Vintage Story research.
- `.codex/skills` - Project-local Codex skills for indexing assets, indexing decompiled C# code, and updating reference mod folders.
- `reference.catalog.json` - Root machine-readable discovery catalog. It lists `1.22.6` and `1.22.7`, with `1.22.7`
  as the default because the newest retained release is always the default.
- `versions/1.22.6/version.catalog.json` - Version-specific discovery catalog for the retained 1.22.6 reference payload.
- `versions/1.22.7/version.catalog.json` - Version-specific discovery catalog for the retained 1.22.7 reference payload.
- `versions/1.22.6/mods.manifest.json` - Tracked selected-mod/tool identity, compatibility, provenance, content, code-root,
  native-binary, and license-file metadata.
- `versions/1.22.7/mods.manifest.json` - Tracked 1.22.7 selected-mod/tool manifest with 18 unique entries.
- `versions/1.22.6/VintageStory.Reference.1.22.6.csproj` - Non-compiling Rider/MSBuild wrapper project that includes
  the version and mod catalogs, API, assets, mods, and Web source manifest as browseable `None` items.
- `versions/1.22.7/VintageStory.Reference.1.22.7.csproj` - Matching browse-only wrapper for the 1.22.7 payload.
- `docs/ARCHITECTURE.md` - Explanation of repository roles and design boundaries.
- `docs/MAINTAINING_REFERENCE.md` - Maintainer workflows for adding versions, mods, websites, and refreshing indexes.
- `docs/research/mods/Tentbag Reworked/TENTBAG_REWORKED_PLAYER_GUIDE.md` - Canonical TentBag-Reworked 3.2.4 guide.
- `docs/research/mods/Tentbag Reworked/TENTBAG_REWORKED_PLAYER_GUIDE.tex` - Committed Pandoc-generated LaTeX source
  for the local TentBag reader PDF.
- `docs/research/mods/Betterer Prospecting/BETTERER_PROSPECTING_SETTINGS.md` - Betterer Prospecting settings research;
  the Markdown remains its editable source.
- `docs/research/mods/Betterer Prospecting/BETTERER_PROSPECTING_SETTINGS.tex` - Committed Pandoc-generated LaTeX
  source for the local Betterer Prospecting reader PDF.
- `versions/1.22.6/API` - Decompiled 1.22.6 `VintagestoryAPI`, `VintagestoryLib`, `VSCreativeMod`,
  `VSEssentials`, and `VSSurvivalMod` sources.
- `versions/1.22.6/Assets` - Full copied 1.22.6 client assets from the Linux client archive.
- `versions/1.22.6/mods` - Selected 1.22.6 reference mod/tool set, with exact 1.22.6 Mod DB releases preferred and
  same-line 1.22.x fallbacks recorded when exact patch tags were unavailable.
- `versions/1.22.6/mods/firearmsfork_1.10.1` - Firearms Fork 1.10.1 assets, managed assembly, and decompiled
  `decompiled assembly/FirearmsFork` source root.
- `versions/1.22.6/mods/overhaulliblegacycompat_1.2.5` - Overhaul Lib Legacy Compat 1.2.5 assets, managed assembly,
  and decompiled `decompiled assembly/OverhaullibLegacyCompat` source root required by Firearms Fork.
- `versions/1.22.6/mods/bettererprospecting_3.4.5` - Betterer Prospecting 3.4.5 reference mod folder containing
  assets, `BetterErProspecting.dll`, and a decompiled `decompiled assembly/BetterErProspecting` source root.
- `versions/1.22.6/mods/bettererprospecting_3.4.5/assets/bettererprospecting/config/configlib-patches.json` -
  Config Lib setting manifest for Betterer Prospecting.
- `versions/1.22.6/mods/bettererprospecting_3.4.5/decompiled assembly/BetterErProspecting/BetterErProspecting/BetterErProspect.cs` -
  Main Betterer Prospecting `ModSystem` that registers the replacement prospecting pick item class and Harmony patch categories.
- `versions/1.22.6/mods/tentbagreworked_3.2.4` - TentBag-Reworked 3.2.4 reference mod folder containing assets,
  `TentBagReworked.dll`, and a decompiled `decompiled assembly/TentBagReworked` source root.
- `versions/1.22.6/mods/tentbagreworked_3.2.4/assets/tentbagreworked/config/tent.json` - Bundled
  WorldEdit/BlockSchematic sample tent copied to game data `WorldEdit/tent.json` on server startup if absent.
- `versions/1.22.6/mods/tentbagreworked_3.2.4/decompiled assembly/TentBagReworked/TentBagReworked/TentBagReworkedModSystem.cs` -
  Contains `EnsureTentSchematicExists`, the server-side copy path for the bundled sample tent.
- `versions/1.22.6/mods/simplevsmanager_2.3.0/REFERENCE_SOURCE.md` - Provenance and licensing-status note for the
  Simple VS Manager external-tool reference, including archive hash and single-file bundle extraction details.
- `versions/1.22.6/mods/simplevsmanager_2.3.0/decompiled assemblies/Simple VS Manager` - Decompiled application
  assembly for Simple VS Manager 2.3.0; start here for Ultimate Mod Manager workflow comparisons.
- `versions/1.22.6/mods/rpvoicechat_2.5.8/decompiled assemblies/RPVoiceChat/RPVoiceChat/Audio/OpusCodec.cs` -
  RPVoiceChat's Concentus/Opus codec implementation.
- `versions/1.22.6/mods/rpvoicechat_2.5.8/decompiled assemblies/RPVoiceChat/RPVoiceChat/Audio/MicrophoneManager.cs` -
  RPVoiceChat client capture path that selects `Opus` or `Dummy` based on `encode-audio`.
- `versions/1.22.6/mods/rpvoicechat_2.5.8/decompiled assemblies/RPVoiceChat/RPVoiceChat/Audio/PlayerAudioSource.cs` -
  RPVoiceChat playback path that constructs the matching codec for incoming packets.
- `versions/1.22.6/mods/rpvoicechat_2.5.8/decompiled assemblies/RPVoiceChat/RPVoiceChat/Networking/AudioPacket.cs` -
  RPVoiceChat network packet carrying the codec name with voice payload metadata.
- `versions/1.22.6/Web/sources.json` - Curated external web documentation source manifest for the 1.22.6 web index.
- `versions/1.22.6/Index/code.sqlite` - Generated 1.22.6 code index for the decompiled API and selected mod/tool sources.
- `versions/1.22.6/Index/assets.sqlite` - Generated 1.22.6 asset index for the copied full client assets and selected
  mod assets.
- `versions/1.22.6/Index/web.sqlite` - Generated 1.22.6 web documentation index.
- `versions/1.22.7/API` - Five flattened official 1.22.7 assembly decompiles.
- `versions/1.22.7/Assets` - Full 1.22.7 Linux-client asset payload.
- `versions/1.22.7/mods` - Selected 1.22.7 reference mod/tool set with decompiled managed assemblies.
- `versions/1.22.7/Web/sources.json` - Copied four-source curated web manifest for 1.22.7.
- `versions/1.22.7/Index` - Local generated 1.22.7 code, asset, and web index artifacts; only `.gitignore` is tracked.
- `.codex/skills/do-new-version` - Project-local Codex skill for creating a new version wrapper project, API tree,
  Assets tree, selected mods tree, copied web source manifest, and corresponding code/asset/web indexes from official
  Vintage Story release packages and carried-forward curated sources.
- `.codex/skills/do-new-web` - Project-local Codex skill for copying/merging previous-version curated web sources and building `web.sqlite`/`web.index.json` under `versions/<version>/Index`.
- `.codex/skills/new-version-mods` - Project-local Codex skill for downloading, staging, extracting, and decompiling selected compatible reference mods under `versions/<version>/mods`.

## Session history

### 2026-09-07

- Added Vintage Story `1.22.7` alongside retained `1.22.6` and promoted `1.22.7` to `default_game_version`. Recorded the
  standing rule that the newest retained Vintage Story release is always the catalog default.
- Updated the new-version and web-copy skills to preserve that default invariant, and updated the asset/code indexer
  skills to use the catalog default when no stronger version signal exists. Skill validation and fresh behavior
  pressure tests passed after the changes.
- Verified the official stable release through the Vintage Story 1.22.7 news post and
  `https://api.vintagestory.at/stable.json`. The release was published on 2026-08-16.
- Downloaded `vs_server_win-x64_1.22.7.zip` and `vs_client_linux-x64_1.22.7.tar.gz` into ignored staging under
  `.codex/tmp/new-version/1.22.7/downloads`. Their local byte counts and MD5 hashes exactly match the official
  manifest: server `c5bcd15a558041533ed3b88f707bb19e`; client `ecfa56411b0a2912ebba29839fa98a3f`.
- Copied the complete Linux-client assets into `versions/1.22.7/Assets`. The base tree matches its extracted source at
  `19,983` files and `837,864,218` bytes and exposes `creative`, `game`, and `survival` directly.
- Decompiled `VintagestoryAPI.dll`, `VintagestoryLib.dll`, `Mods/VSCreativeMod.dll`, `Mods/VSEssentials.dll`, and
  `Mods/VSSurvivalMod.dll` with `ilspycmd` `10.1.1.8388`. The flattened wrappers contain `1,122`, `1,009`, `51`,
  `524`, and `1,296` C# files respectively. Removed temporary `<HintPath>` entries and verified no generated `.sln`
  or `.slnx` files remain under the API tree.
- Populated `versions/1.22.7/mods` from all 18 entries in the 1.22.6 seed manifest. Used 11 exact 1.22.7 releases,
  six documented same-line fallbacks, and the unchanged Simple VS Manager external-tool reference. Eleven selected
  package versions changed from the 1.22.6 set.
- Preserved all 18 mod archives and extraction roots under `.codex/tmp/mod-updates/1.22.7`. Verified every archive
  SHA-256 against both `.codex/tmp/mod-updates/1.22.7/selected-releases.json` and the tracked
  `versions/1.22.7/mods.manifest.json`.
- Inspected 48 DLLs across the selected set, decompiled all 38 managed DLLs, and retained 10 native DLLs without
  treating them as decompilation failures. RP Voice Chat also retains `RNNoise.so`; native binaries are present in
  RP Voice Chat, Simple VS Manager, and Dear ImGui.
- Copied all four curated web source slugs from `versions/1.22.6/Web/sources.json`, staged the 1.22.7 source manifest,
  and rebuilt the web index with `4` sources and `99` sections.
- Fixed the conservative code-index parser so ordinary Allman-style declarations and constructors with `base`/`this`
  initializers are indexed, while declaration-shaped LINQ/async-lambda continuations, block comments, and braces in
  character literals do not corrupt member extraction. Delegate declarations now use their declared name and full
  semicolon-terminated span, one-line bodies and constructor arguments with object initializers have accurate bounds,
  and legal contextual-keyword identifiers remain indexable. Added 13 root-discoverable regression tests, bumped the
  tool to `1.1.4`, and rebuilt both retained code indexes. The 1.22.7 schema-2 index now has `8,607` source files,
  `9,732` types, `95,780` members, `689,732` reference hints, and `836` fully resolved literal registrations across
  `20` roots; the rebuilt 1.22.6 index has `8,428` files, `9,543` types, `94,449` members, `679,367` reference hints,
  and `820` registrations.
- Built the 1.22.7 asset index with `22,837` files, including `11,660` JSON files with `11,311` parsed successfully and
  `349` retained parse-error rows. The class cross-reference returns `722` rows across `407` case-insensitive class
  keys.
- Added `versions/1.22.7/VintageStory.Reference.1.22.7.csproj` to `Vintage Story Reference.sln` under a matching
  solution folder. The full solution builds with zero warnings and zero errors.
- Added `versions/1.22.7/version.catalog.json`, added 1.22.7 to `reference.catalog.json`, and updated `README.md`,
  `INDEX_USAGE.md`, and `docs/ARCHITECTURE.md` so both retained versions and the 1.22.7 default are discoverable.
- Verified SQLite integrity for all three generated indexes; validated the 18-entry mod manifest against folders,
  metadata, compatibility tags, hashes, and code roots; and ran representative mod, asset, recipe, API type/member,
  registration, class cross-reference, code text, web source, Config Lib, and RPVoiceChat queries.

### 2026-08-16

- Added the user-provided `firearmsfork_1.10.1.zip` and `overhaulliblegacycompat_1.2.5.zip` archives to the retained
  1.22.6 reference mod set. Preserved staged archive and extraction copies under
  `.codex/tmp/mod-updates/1.22.6/`.
- Verified both archives against current Vintage Story Mod DB release metadata. Firearms Fork 1.10.1 release `48165`
  lists game versions 1.22.0 through 1.22.3; Overhaul Lib Legacy Compat 1.2.5 release `50266` lists 1.22.0 through
  1.22.4. Both are recorded as same-line fallbacks for target 1.22.6.
- Decompiled `FirearmsFork.dll` into `firearmsfork_1.10.1/decompiled assembly/FirearmsFork`, producing `50` C# files,
  and `OverhaullibLegacyCompat.dll` into
  `overhaulliblegacycompat_1.2.5/decompiled assembly/OverhaullibLegacyCompat`, producing `421` C# files.
- Updated `versions/1.22.6/mods.manifest.json` with archive hashes, Mod DB provenance, compatibility tags, source
  repositories, content/code-root facts, and license-file absence for both mods.
- Rebuilt the 1.22.6 code and asset indexes. The code index now contains `8,428` files, `9,544` types, `40,839`
  members, `50,697` reference hints, and `820` registrations across `20` roots. The asset index now contains `22,792`
  files, including `11,618` JSON files with `11,269` parsed successfully and `349` retained parse-error rows.
- Updated the version catalog and index usage guide with current counts; representative mod, code, registration,
  asset, and class-cross-reference queries succeeded. The browse-only solution built with zero warnings and errors.

### 2026-08-15

- Added `docs/research/mods/Config Lib/CONFIG_LIB_INTEGRATION_GUIDE.md`, documenting optional Config Lib `1.12.0`
  integration for Vintage Story `1.22.6`. The guide covers the exact asset path, YAML and JSON-backed definitions,
  normal fallback config loading, guarded live callbacks, setting/formatting fields, content-mod asset patching,
  public runtime interfaces, managed registration, synchronization, defaults/migration, localization, limitations,
  evidence, queries, and a release checklist.
- Used the 1.22.6 mod manifest plus web, asset, and code indexes before inspecting Config Lib and Betterer Prospecting
  source candidates. Verified the Config Lib asset loader, public `IConfigProvider`/`IConfig`/`ISetting` contracts,
  `RegisterCustomManagedConfig`, event names, JSON `file` behavior, and Betterer Prospecting's optional guard and
  default mismatch.
- Generated `CONFIG_LIB_INTEGRATION_GUIDE.tex` with the shared Pandoc defaults and compiled the ignored local PDF
  with three XeLaTeX passes. The final document has 15 physical pages, zero overfull boxes, and visually checked
  title, contents, code-heavy sections, runtime API material, source queries, and checklist.

### 2026-08-14

- Created the initial flat schematic-library scaffold under `schematics/`: ignored `inbox/`, tracked `library/`,
  ignored `Index/`, and an empty tracked `manifest.json`. Added `docs/SCHEMATIC_LIBRARY.md` with intake, naming,
  provenance/licensing, and consumption rules, then exposed the library through the root README, architecture doc,
  `AGENTS.md`, and machine-readable catalog. No schematic files or index tooling were added.

- Finalized the research artifact policy at the user's direction: commit canonical `.md` and generated `.tex`, but
  keep compiled `.pdf` reader copies local and ignored. Updated `.gitignore`, `AGENTS.md`, architecture/maintenance
  docs, and the research workflow accordingly; preserved the existing TentBag PDF locally while removing it from Git.
- Added title metadata to both retained research documents, generated their LaTeX sources with Pandoc `3.10.2`, and
  compiled local PDFs with MiKTeX-XeTeX `4.16`. Enabled MiKTeX automatic missing-package installation after the first
  compile waited on its first-use prompt.
- Changed the Pandoc reader from GFM to Pandoc Markdown and shifted heading levels so LaTeX tables wrap to page width
  and numbered sections start at `1` instead of `0.1`. Shortened displayed source-path segments to eliminate clipping.
  Two/three-pass XeLaTeX builds succeeded; final visual inspection confirmed readable title pages, contents, tables,
  body text, and source lists. TentBag finished with no LaTeX layout warnings; Betterer retained only Pandoc's harmless
  unused table-caption setup warning.

- Established `docs/research/` as the durable home for paired research deliverables: canonical Markdown for agents and
  same-basename Pandoc/LaTeX PDFs for the user. Added the policy to `AGENTS.md`, maintainer/architecture docs, and the
  root catalog.
- Moved the complete tracked `docs/mods` tree to `docs/research/mods`, including Betterer Prospecting research and the
  TentBag Markdown/PDF pair, and updated current repository references to the new paths.
- Added `docs/research/README.md` and `tools/research-docs/pandoc-pdf.yaml` with a shared readable PDF style.
- Located Pandoc `3.10.2` at `C:\Program Files\Pandoc\pandoc.exe`. No `xelatex`, `lualatex`, or `pdflatex` executable
  was installed or discoverable, so the Betterer Prospecting PDF could not yet be generated.

- At the user's explicit request to commit every pending file, classified the previously untracked TentBag player-guide
  PDF as an intentional documentation artifact, added the PDF binary attribute, and included it in the repository.

- Upgraded `tools/code-index/build_code_index.py` to schema `2` and added the `registrations` table for supported
  literal `Register*` calls with string keys and `typeof(...)` values. Calls retain method/kind, containing type,
  source line, compact evidence, and an unambiguous registered-type declaration link when available.
- Added `tools/cross-reference/query_class_cross_reference.py` and exposed it through `tools/reference-query.py` as
  `class`, `classes`, and `xref`. The join maps top-level block/item/entity `class` and block `entityClass` facts to
  the appropriate registry and supports text or JSON output.
- Rebuilt the local generated 1.22.6 code index: `7,957` files, `9,016` types, `38,277` members, `47,032` bounded
  reference hints, and `766` literal registrations. All 766 registered types resolved; the class join returned `674`
  asset rows across `386` distinct keys.
- Updated catalogs, consumer/architecture/maintenance docs, query cookbook, Vintage Story Research prompt, and the
  code-indexer, asset-indexer, and new-version skills so other projects and future versions discover and validate the
  relationship query.

- Added tracked `versions/1.22.6/mods.manifest.json` with one curated entry for each of the 16 retained mod/tool
  folders. The manifest records exact/fallback compatibility, source URLs, release provenance and hashes, retained
  assets/code, code-root names, native binaries, and explicit license-file presence.
- Added `tools/mod-manifest/query_mod_manifest.py` and extended `tools/reference-query.py` with `mod`/`mods` query
  targets plus `--list-manifests`. Added `--reference` to the code query helper to expose existing conservative
  `references_hint` rows.
- Added `docs/QUERY_COOKBOOK.md` with tested query sequences for blocks/items, asset-to-code tracing, class
  registration, recipe input/output, vanilla-versus-mod comparison, mod provenance, and web documentation.
- Updated catalogs, consumer/architecture/maintenance docs, the Vintage Story Research prompt, and the
  `new-version-mods`, `do-new-version`, and code-indexer skills so future versions create, catalog, query, and validate
  the tracked mod manifest.
- Verified the manifest against all 16 retained folders, `modinfo.json` ids/versions, asset-folder presence, and the 18
  code-index roots. Verified representative cookbook queries and a zero-warning, zero-error solution build. Preserved
  the pre-existing untracked TentBag player-guide PDF.

- Removed `versions/1.22.3` and `versions/1.22.4` at the user's request after verifying both resolved paths were
  under the repository's `versions` directory.
- Removed the 1.22.3 and 1.22.4 wrapper projects from `Vintage Story Reference.sln`; `dotnet sln list` now reports
  only `versions\1.22.6\VintageStory.Reference.1.22.6.csproj`.
- Updated `reference.catalog.json` so `default_game_version` is `1.22.6` and the only advertised version is 1.22.6.
- Rewrote `INDEX_USAGE.md` as a 1.22.6-only consumer guide. Other projects should now query only
  `versions/1.22.6/Index/assets.sqlite`, `code.sqlite`, and `web.sqlite`.
- Updated `README.md`, `versions/1.22.6/version.catalog.json`, the Betterer Prospecting settings note, and the
  TentBag-Reworked player guide so current-facing docs no longer point at removed 1.22.3 or 1.22.4 payloads.
- Left historical project-memory entries about 1.22.3 and 1.22.4 intact as provenance; they are no longer live
  reference paths.
- Updated `docs/ARCHITECTURE.md` to make the root catalog's default-version role explicit, record that 1.22.6 is the
  only retained indexed version as of 2026-08-14, describe browse-only version wrapper projects, and document the
  boundary between retained source/reference material and rebuildable generated index artifacts.
- Verified `tools/reference-query.py --list-versions` reports `1.22.6 (default)` and
  `tools/reference-query.py --version 1.22.6 --list-indexes` resolves asset, code, and web indexes after the
  architecture update.
- Staged the 1.22.6-only cleanup for commit: removed tracked 1.22.3 and 1.22.4 payloads, added the 1.22.6 payload,
  kept only `versions/1.22.6/Index/.gitignore` from the generated index folder, added the asset/code/web index tool
  scripts referenced by the catalogs, and added `.codex/tmp/` to `.gitignore` so local decompiler/download caches stay
  untracked. `git config core.longpaths true` was set locally because a Config Lib decompiled regex-generator path
  exceeded the default Windows Git path limit during staging.
- Left `docs/mods/Tentbag Reworked/TENTBAG_REWORKED_PLAYER_GUIDE.pdf` and `tools/panserbjornrebjorn_1.0.0` untracked
  because they appear to be generated or misplaced local reference material, not required 1.22.6 commit content.
- Rewrote Git history with `git-filter-repo` to remove `versions/1.22.3` and `versions/1.22.4` from all reachable
  commits after creating the temporary mirror backup
  `E:\Gaming\Vintage Story\mods\Vintage Story Reference.backup.git`. `git-filter-repo` removed the `origin` remote as
  expected; it was restored to `https://github.com/caffeineLover/Vintage-Story-Reference.git`.
- Ran `git gc --aggressive --prune=now` after the rewrite. Packed Git storage fell from about `695 MiB` before the
  rewrite to about `647 MiB`; most storage remains because the current `versions/1.22.6` payload still contains a large
  asset/mod reference set that Git stores in the current history.
- Verified after the rewrite that `git rev-list --objects --all -- versions/1.22.3 versions/1.22.4` returns no
  objects, `git fsck --no-progress` succeeds, `dotnet build "Vintage Story Reference.sln" --no-restore` succeeds, and
  representative 1.22.6 asset/code queries still return results.

### 2026-08-10

- Created `versions/1.22.6` without creating a new Git branch, per the user's direction, and added
  `versions/1.22.6/VintageStory.Reference.1.22.6.csproj` to `Vintage Story Reference.sln` under a `1.22.6` solution
  folder.
- Verified official Vintage Story 1.22.6 stable release availability from the Vintage Story news post dated
  2026-07-31 and confirmed the CDN endpoints for `vs_server_win-x64_1.22.6.zip` and
  `vs_client_linux-x64_1.22.6.tar.gz`.
- Downloaded the 1.22.6 Windows server ZIP and Linux client archive to `.codex/tmp/new-version/1.22.6/downloads`;
  verified MD5 `59e0aeaa213359025d51e67f26f7f255` for the server ZIP and
  `c00c436cab612b436747b4bb5f39049b` for the client archive.
- Copied full 1.22.6 client assets into `versions/1.22.6/Assets`, with `creative`, `game`, and `survival` asset
  domains present directly under the target folder.
- Decompiled `VintagestoryAPI.dll`, `VintagestoryLib.dll`, `Mods/VSCreativeMod.dll`, `Mods/VSEssentials.dll`, and
  `Mods/VSSurvivalMod.dll` with `ilspycmd` `10.1.1.8388` into flattened 1.22.6 API wrapper folders. Removed generated
  temp `<HintPath>` entries and verified no `.sln` or `.slnx` files remain under `versions/1.22.6/API`.
- Populated `versions/1.22.6/mods` from the 1.22.4 seed set. The initial seeded set had 15 unique mod ids. Exact
  1.22.6 Mod DB releases were used for Betterer Prospecting 3.4.5, Primitive Survival 5.1.0, RPVoiceChat 2.5.8, and
  TentBag-Reworked 3.2.4; the other selected mods are same-line `1.22.x` fallbacks recorded in
  `.codex/tmp/mod-updates/1.22.6/selected-releases.json`.
- Decompiled managed DLLs for the selected 1.22.6 code mods using `.codex/tmp/decompiler/VsReferenceDecompiler.csproj`.
  Native/non-.NET DLLs, such as VSImGui native Windows DLLs, were left as binaries and reported in the generated mod
  selection report.
- Copied `versions/1.22.4/Web/sources.json` to `versions/1.22.6/Web/sources.json`, staged that curated source manifest
  with Git, built `versions/1.22.6/Index/web.sqlite`, and staged `versions/1.22.6/Index/.gitignore` while leaving
  generated index outputs ignored.
- Built `versions/1.22.6/Index/code.sqlite`; after the later Simple VS Manager addition it contains `7,957` C# files,
  `9,016` types, `38,277` members, and `18` source roots.
- Built `versions/1.22.6/Index/assets.sqlite`; it contains `22,103` files, including `11,066` JSON files with
  `10,717` parsed successfully and `349` parse-error rows retained for path/category lookup.
- Built `versions/1.22.6/Index/web.sqlite`; it contains `4` sources and `84` sections.
- Updated `versions/1.22.6/version.catalog.json`, `reference.catalog.json`, `README.md`, and `INDEX_USAGE.md` so
  `tools/reference-query.py` discovers 1.22.6 and lists its asset, code, and web indexes.
- Verified `dotnet sln "Vintage Story Reference.sln" list`, `tools/reference-query.py --list-versions`,
  `tools/reference-query.py --version 1.22.6 --list-indexes`, representative asset searches for copper and recipes,
  code queries for `ICoreAPI`, `RegisterBlockClass`, and `BlockEntity`, and web queries for source listing,
  Config Lib patches, and RPVoiceChat bitrate documentation.
- Added Simple VS Manager 2.3.0 to `versions/1.22.6/mods/simplevsmanager_2.3.0` as a Mod DB `externaltool`
  reference for Ultimate Mod Manager research. The official page identifies it as a manager/launcher with Mod DB
  search, updates, shared modlists, and game profiles rather than a normal in-game mod.
- Downloaded `Simple VS Manager 2.3.0.zip` to `.codex/tmp/mod-updates/1.22.6/archives`; archive SHA256 is
  `71F7F9F91A6DCC234ADE5BF095FA34FE2D043AE0DBA92FAEFCF3C2D9EC2AF7F8`.
- Extracted the single-file .NET bundle from `Simple VS Manager.exe` without running it. The embedded bundle contained
  12 managed assemblies, 6 native libraries, `Simple VS Manager.deps.json`, and `Simple VS Manager.runtimeconfig.json`.
- Decompiled the 12 managed assemblies into
  `versions/1.22.6/mods/simplevsmanager_2.3.0/decompiled assemblies`, producing `2,624` C# files. Native libraries
  such as qpdf, QuestPdfSkia, zlib, libgcc, libstdc++, and libwinpthread were preserved as binaries only.
- Added `versions/1.22.6/mods/simplevsmanager_2.3.0/REFERENCE_SOURCE.md` because the archive has no `modinfo.json`
  and no explicit license file; the GitHub license API returned 404 on 2026-08-10.
- Rebuilt and validated the 1.22.6 code index after adding Simple VS Manager. It now returns results for
  `SimpleVsManager.Cloud.FirebaseModlistStore` and `VintageStoryModManager.Views.MainWindow`.
- Rebuilt the 1.22.6 asset index after adding Simple VS Manager; counts remained `22,103` files, including `11,066`
  JSON files with `10,717` parsed successfully and `349` parse-error rows.

- Consumed local archive `versions/1.22.4/mods/bettererprospecting_3.4.5.zip` for target Vintage Story `1.22.4`; archive SHA256 is `BA645FE026918178CF10E74A9981F47619BA2C1007FE48C4A6C27A32F2C77719`.
- Extracted the archive to `.codex/tmp/mod-updates/1.22.4/extracted/bettererprospecting_3.4.5` and copied it to `versions/1.22.4/mods/bettererprospecting_3.4.5`. The `modinfo.json` declares mod id `bettererprospecting`, version `3.4.5`, type `code`, side `Universal`, and game dependency `1.22.0`.
- Preserved a staged archive copy at `.codex/tmp/mod-updates/1.22.4/archives/bettererprospecting_3.4.5.zip`. The original ZIP under `versions/1.22.4/mods` was left in place because deletion was not requested.
- Decompiled `BetterErProspecting.dll` into `versions/1.22.4/mods/bettererprospecting_3.4.5/decompiled assembly/BetterErProspecting`, producing `22` C# files.
- Rebuilt the 1.22.4 code index; it now contains `5,332` C# files, `5,714` types, `29,464` members, and `17` source roots, including `bettererprospecting_3.4.5/decompiled assembly`.
- Rebuilt the 1.22.4 asset index; it now contains `22,100` files, including `9` Betterer Prospecting asset files under the `bettererprospecting` domain.
- Verified `tools/reference-query.py --version 1.22.4` returns Betterer Prospecting code results for `BetterErProspecting` and asset results for the `bettererprospecting` domain.
- Investigated Betterer Prospecting settings `ReprospectParellelism`, `BoreholeScansStone`, and `BoreholeDmg` using the 1.22.4 code and asset indexes, then confirmed exact behavior in the decompiled mod code and Config Lib assets.
- Added `docs/mods/Betterer Prospecting/BETTERER_PROSPECTING_SETTINGS.md`, documenting the three settings, the Borehole mode scan path, the `BoreholeDmg` default mismatch between `ModConfig` and Config Lib, and the `ReprospectParellelism` spelling/concurrency behavior.
- Updated the Betterer Prospecting settings reference with the `BoreholeRadius = 5` recommendation when
  `ProximitySearchRadius = 5`, including the scan-shape reasoning and per-layer radius cost comparison.

### 2026-07-28

- Investigated why Vintage Story 1.22.4 display cases cannot be stacked directly. Queried the 1.22.4 asset and code indexes, then inspected `survival/blocktypes/wood/displaycase.json`, `displaycase-tall.json`, `BlockDisplayCase`, `BlockBehaviorUnstableFalling`, and base `Block.CanAttachBlockAt`.
- Confirmed the blocker is not display-case inventory code. Vanilla display case JSON marks all sides non-solid and adds `UnstableFalling`; the falling placement check requires the block below to allow attachment on its top face and fails with `requiresolidground` when placing a display case on another display case.

### 2026-07-26

- Investigated the bundled TentBag-Reworked `tent.json` after the player guide wording raised a question about a tent that comes with the mod.
- Verified through the asset index and decompiled code that `TentBagReworkedModSystem.EnsureTentSchematicExists` copies `assets/tentbagreworked/config/tent.json` to game data `WorldEdit/tent.json` on server startup when that target file is missing.
- Updated `docs/TENTBAG_REWORKED_PLAYER_GUIDE.md` to clarify that the bundled file is a WorldEdit sample schematic, not a pre-packed Tent Bag, player-granted item, or extra survival recipe.
- Researched Vintage Story 1.22.4 WorldEdit import behavior in `VSCreativeMod` and updated the guide with direct and preview workflows for using `tent.json`; the guide is now hard-wrapped at 120 columns.

### 2026-07-25

- Downloaded TentBag-Reworked 3.2.4 for target Vintage Story `1.22.4` from the official Mod DB API entry `https://mods.vintagestory.at/api/mod/tentbagreworked`.
- Recorded the release as a same-line fallback because its Mod DB tags run through `1.22.3` and no exact `1.22.4` tag was published for the original TentBag-Reworked mod at selection time.
- Saved the archive as `.codex/tmp/mod-updates/1.22.4/archives/TentBag Reworked v3.2.4.zip`; archive SHA256 is `072889AD0DDE0505F340A147835F7CC5DCF7449BE9FFDB583F33D74E0F16A217`.
- Extracted and copied the mod to `versions/1.22.4/mods/tentbagreworked_3.2.4`.
- Decompiled `TentBagReworked.dll` into `versions/1.22.4/mods/tentbagreworked_3.2.4/decompiled assembly/TentBagReworked`, producing `9` C# files.
- Rebuilt the 1.22.4 code index; it now contains `5,310` C# files, `5,692` types, `29,402` members, and `16` source roots, including `tentbagreworked_3.2.4/decompiled assembly`.
- Rebuilt the 1.22.4 asset index; it now contains `22,091` files, including `21` TentBag-Reworked asset files under the `tentbagreworked` domain.
- Verified `tools/reference-query.py --version 1.22.4` returns TentBag-Reworked code results for `TentBagReworked` and asset results for `tentbagreworked`.
- Added `docs/TENTBAG_REWORKED_PLAYER_GUIDE.md`, a player-facing guide covering TentBag-Reworked crafting, packing, unpacking, hotkeys, satiety cost, banned blocks, placement failures, and server-config-dependent behavior.
- Updated `README.md` to point at the TentBag-Reworked player guide.

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

### 2026-07-23

- Investigated whether PetAI exposes a command for directly setting pet obedience. Queried the 1.22.4 code and asset indexes, then inspected PetAI 5.1.1 source and related pet entity JSON. Confirmed there is no `RegisterCommand`/`ChatCommands` registration in PetAI, and no PetAI command type or command name that sets obedience. PetAI commands only set behavior state or aggression mode and require minimum obedience thresholds.
- Investigated whether `VSEssentials.dll` should be added to the API decompile/index list. Found that 1.22.4 `ModCompilationContext` references both `Mods/VSCreativeMod.dll` and `Mods/VSEssentials.dll` before `Mods/VSSurvivalMod.dll`, and `VSSurvivalMod.csproj` references both `VSEssentials` and `VSCreativeMod`. No workflow or index changes were made during this investigation.
- Backfilled `versions/1.22.4/API` with flattened decompiles of `Mods/VSCreativeMod.dll` and `Mods/VSEssentials.dll` from the official 1.22.4 Windows dedicated server ZIP. Verified the ZIP MD5 as `162941f6a5d6dbc576cce700f50d16bf` and used `ilspycmd` `10.1.1.8388`.
- Updated `tools/code-index/build_code_index.py`, `.codex/skills/do-new-version/SKILL.md`, `.codex/skills/vintage-story-reference-code-indexer/SKILL.md`, `docs/MAINTAINING_REFERENCE.md`, `versions/1.22.4/version.catalog.json`, `reference.catalog.json`, `README.md`, and `INDEX_USAGE.md` so future versions and 1.22.4 consumers use the five built-in API roots.
- Rebuilt `versions/1.22.4/Index/code.sqlite`; the 1.22.4 code index now contains `5,301` C# files, `5,678` types, and `29,328` members across `15` source roots. The new API root counts are `51` C# files for `VSCreativeMod` and `524` for `VSEssentials`.
- Intentionally did not backfill `versions/1.22.3/API` with `VSCreativeMod` or `VSEssentials`, per the user's direction that current work targets `1.22.4` and future `1.22.5`.

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
