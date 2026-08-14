# Vintage Story Reference Instructions for AI Projects

This repository is a local reference library for AI-assisted Vintage Story modding work. Other projects should use it
as a searchable reference source, not as a project to build or copy wholesale.

Always query the SQLite indexes first. Do not recursively scan or read the full `Assets`, `API`, or `mods` trees unless
an index cannot answer the question.

`INDEX_USAGE.md` is the handoff document for using this repository as a Vintage Story knowledge source. For a reusable
AGENTS.md prompt that teaches other Codex-assisted projects this research workflow, use
`prompts\Vintage Story Research\Add to AGENTS.md`. For reusable agent operating standards such as persistent project
memory, coding standards, and project/Git hygiene, use the project-local prompt catalog under `prompts/`.

For machine-readable discovery, use `reference.catalog.json` and `versions/<game-version>/version.catalog.json`.
For a single query entry point, prefer `tools/reference-query.py`; the source-specific query scripts remain available
when you need their exact interfaces.

Vintage Story `1.22.6` is the only retained indexed version. It has decompiled API sources, copied full client assets,
selected reference mods/tools, copied curated web sources, and generated code/asset/web indexes.

## Instruction Block For Other AI Projects

Use this text when pointing another AI-enabled project at this repository:

```text
Use the Vintage Story Reference repository at:
E:\Gaming\Vintage Story\mods\Vintage Story Reference

Before scanning raw Vintage Story assets, mod assets, or decompiled C# source, query the reference indexes:
- Asset index: E:\Gaming\Vintage Story\mods\Vintage Story Reference\versions\1.22.6\Index\assets.sqlite
- Code index: E:\Gaming\Vintage Story\mods\Vintage Story Reference\versions\1.22.6\Index\code.sqlite
- Web documentation index: E:\Gaming\Vintage Story\mods\Vintage Story Reference\versions\1.22.6\Index\web.sqlite

Use the helper query scripts:
- E:\Gaming\Vintage Story\mods\Vintage Story Reference\tools\reference-query.py
- E:\Gaming\Vintage Story\mods\Vintage Story Reference\tools\asset-index\query_asset_index.py
- E:\Gaming\Vintage Story\mods\Vintage Story Reference\tools\code-index\query_code_index.py
- E:\Gaming\Vintage Story\mods\Vintage Story Reference\tools\web-index\query_web_index.py

Keep result limits small. Open raw files only after an index returns candidate paths and line numbers. Treat the
indexes as locators and summaries, not as the final source of truth for exact implementation details. Web documentation
entries are dated index content; check the upstream URL when recency matters. Do not read NOTES.md as metadata. Do not
load binary assets into AI context.

If sandbox permissions prevent reading this folder, ask the user to add the Vintage Story Reference repository as a
readable workspace root or otherwise grant read access. Do not ask the user to paste or attach the full asset/source
trees.
```

## What This Repository Contains

- `versions/1.22.6/API`
  - Decompiled C# reference code for `VintagestoryAPI`, `VintagestoryLib`, `VSCreativeMod`, `VSEssentials`, and
    `VSSurvivalMod`.
  - Use this when answering API, vanilla behavior, class, method, interface, registration, and implementation
    questions.

- `versions/1.22.6/Assets`
  - Extracted Vintage Story asset trees such as `creative`, `game`, and `survival`.
  - Includes blocktypes, itemtypes, recipes, lang files, shapes, textures, sounds, worldgen data, shaders, config, and
    related asset metadata.

- `versions/1.22.6/mods`
  - Selected downloaded mods and tools kept as reference material.
  - Mod `assets` folders are included in the asset index when present.
  - Decompiled mod/tool source folders are included in the code index when present.
  - Simple VS Manager is an external-tool reference, not a normal in-game mod package.

- `versions/1.22.6/Web`
  - Curated external documentation source manifests.
  - These source lists feed the generated web documentation index.
  - The web index is for searchable documentation knowledge, not local page snapshots.

- `versions/1.22.6/Index`
  - Stable generated index artifacts.
  - `assets.sqlite` and `assets.index.json` describe assets.
  - `code.sqlite` and `code.index.json` describe decompiled C# code.
  - `web.sqlite` and `web.index.json` describe curated external documentation.
  - Generated manifests remain the source of truth for exact index generation details.

- `versions/1.22.6/version.catalog.json`
  - Version-specific discovery catalog for reference roots, indexes, source manifests, and query/build scripts.

- `reference.catalog.json`
  - Root discovery catalog for available versions, docs, prompts, skills, and the query wrapper.

- `tools/asset-index`, `tools/code-index`, and `tools/web-index`
  - Scripts for building and querying the asset, code, and web indexes.

- `tools/reference-query.py`
  - Unified wrapper that resolves the selected version and delegates to the asset, code, or web query helper.

- `prompts`
  - Reusable agent prompt standards maintained with this repository.

- `.codex/skills`
  - Project-local Codex skills for maintaining this reference repository.

- `NOTES.md`
  - Personal user notes. Do not treat this file as authoritative metadata.

## Current Version: 1.22.6

Repository root:

```text
E:\Gaming\Vintage Story\mods\Vintage Story Reference
```

Current indexed version:

```text
E:\Gaming\Vintage Story\mods\Vintage Story Reference\versions\1.22.6
```

Asset index:

```text
E:\Gaming\Vintage Story\mods\Vintage Story Reference\versions\1.22.6\Index\assets.sqlite
E:\Gaming\Vintage Story\mods\Vintage Story Reference\versions\1.22.6\Index\assets.index.json
```

Code index:

```text
E:\Gaming\Vintage Story\mods\Vintage Story Reference\versions\1.22.6\Index\code.sqlite
E:\Gaming\Vintage Story\mods\Vintage Story Reference\versions\1.22.6\Index\code.index.json
```

Web documentation index:

```text
E:\Gaming\Vintage Story\mods\Vintage Story Reference\versions\1.22.6\Index\web.sqlite
E:\Gaming\Vintage Story\mods\Vintage Story Reference\versions\1.22.6\Index\web.index.json
```

The asset index currently covers `22,103` files, including `11,066` JSON files with `10,717` parsed successfully and
`349` parse-error rows retained for path/category lookup. The code index currently covers `7,957` C# files, `9,016`
types, and `38,277` members across `18` source roots. The web index currently covers `4` copied source entries split
into `84` sections.

## Recommended Workflow

1. Identify the Vintage Story version being targeted.
2. Use `1.22.6` unless the consuming project explicitly requires a version this archive no longer retains.
3. Read the matching manifests in `versions/1.22.6/Index`.
4. Query `assets.sqlite` for asset questions or `code.sqlite` for C# questions.
5. Query `web.sqlite` for curated external documentation such as Config Lib JSON API behavior, VS Village gameplay
   behavior, or RPVoiceChat administration and project docs.
6. Use small limits, usually `10` to `50`.
7. Open only the raw files returned by the index.
8. For exact behavior, read the specific source file and line range returned by the code index.
9. For exact JSON details, read the specific asset file returned by the asset index.
10. For external documentation, inspect the source URL when current upstream behavior matters.
11. If the index is missing, stale, or for the wrong game version, return to this repository and refresh the index.

## Access From Another Workspace

From any PowerShell session, define the reference root once:

```powershell
$vsref = 'E:\Gaming\Vintage Story\mods\Vintage Story Reference'
```

Then call query scripts by absolute path:

```powershell
python -B "$vsref\tools\reference-query.py" --list-versions
python -B "$vsref\tools\reference-query.py" --version 1.22.6 --list-indexes
python -B "$vsref\tools\reference-query.py" --version 1.22.6 asset --search copper --limit 20
python -B "$vsref\tools\reference-query.py" --version 1.22.6 code --type ICoreAPI --limit 20
python -B "$vsref\tools\reference-query.py" --version 1.22.6 web --search "conditional patches" --limit 20
```

The source-specific helpers are also available:

```powershell
python -B "$vsref\tools\asset-index\query_asset_index.py" --db "$vsref\versions\1.22.6\Index\assets.sqlite" --search copper --limit 20
python -B "$vsref\tools\code-index\query_code_index.py" --db "$vsref\versions\1.22.6\Index\code.sqlite" --type ICoreAPI --limit 20
python -B "$vsref\tools\web-index\query_web_index.py" --db "$vsref\versions\1.22.6\Index\web.sqlite" --search "conditional patches" --limit 20
```

Use `python -B` so Python does not create `__pycache__` folders.

## Asset Index

Use `assets.sqlite` for vanilla and indexed mod asset, content, JSON, texture, shape, recipe, localization, and
worldgen questions.

Important tables:

- `assets`
  - One row per indexed asset file.
  - Useful columns include `rel_path`, `domain`, `category`, `subcategory`, `extension`, `size`, `is_json`,
    `parse_status`, `png_width`, and `png_height`.

- `json_assets`
  - Compact facts extracted from parseable JSON files.
  - Useful columns include `fact_key`, `fact_value`, `value_type`, and `json_path`.

- `asset_text`
  - Full-text search table over compact asset metadata and extracted JSON text.

Asset query examples:

```powershell
$vsref = 'E:\Gaming\Vintage Story\mods\Vintage Story Reference'

python -B "$vsref\tools\asset-index\query_asset_index.py" --db "$vsref\versions\1.22.6\Index\assets.sqlite" --search copper --limit 20
python -B "$vsref\tools\asset-index\query_asset_index.py" --db "$vsref\versions\1.22.6\Index\assets.sqlite" --category recipes --limit 20
python -B "$vsref\tools\asset-index\query_asset_index.py" --db "$vsref\versions\1.22.6\Index\assets.sqlite" --domain survival --category blocktypes --limit 20
python -B "$vsref\tools\asset-index\query_asset_index.py" --db "$vsref\versions\1.22.6\Index\assets.sqlite" --fact-key output --fact-value ingot-copper --limit 20
```

The asset manifest records all indexed asset roots, including `versions\1.22.6\Assets` and mod roots with `assets`
folders. For binary assets such as `.png`, `.ogg`, `.ttf`, `.gltf`, and shader files, use index metadata first. Do not
read binary contents into AI context.

Some Vintage Story JSON uses JSON-like syntax that may not parse as strict JSON. The asset index still keeps rows for
parse-error files in `assets`; open the raw file only when the specific file matters.

## Code Index

Use `code.sqlite` for API, decompiled vanilla code, selected decompiled mod/tool code, symbols, types, methods,
inheritance, and source-location questions.

Important tables:

- `source_roots`
  - Indexed roots such as `VintagestoryAPI`, `VintagestoryLib`, `VSCreativeMod`, `VSEssentials`, `VSSurvivalMod`, and
    decompiled mod/tool roots.

- `source_files`
  - One row per indexed `.cs` file.
  - Paths are repository-relative in `rel_path`.

- `types`
  - Type declarations with namespace, name, full name, kind, accessibility, base type, interfaces, attributes, and line
    ranges.

- `members`
  - Methods, constructors, properties, fields, and events with signatures and line ranges.

- `references_hint`
  - Token-based reference hints. Useful for narrowing candidates, not a reliable call graph.

- `code_text`
  - Full-text search table over compact symbol metadata.

Code query examples:

```powershell
$vsref = 'E:\Gaming\Vintage Story\mods\Vintage Story Reference'

python -B "$vsref\tools\code-index\query_code_index.py" --db "$vsref\versions\1.22.6\Index\code.sqlite" --type ICoreAPI --limit 20
python -B "$vsref\tools\code-index\query_code_index.py" --db "$vsref\versions\1.22.6\Index\code.sqlite" --member RegisterBlockClass --limit 20
python -B "$vsref\tools\code-index\query_code_index.py" --db "$vsref\versions\1.22.6\Index\code.sqlite" --base BlockEntity --limit 20
python -B "$vsref\tools\code-index\query_code_index.py" --db "$vsref\versions\1.22.6\Index\code.sqlite" --root VSSurvivalMod --search Firepit --limit 20
```

Current code roots include:

- `VintagestoryAPI`
- `VintagestoryLib`
- `VSCreativeMod`
- `VSEssentials`
- `VSSurvivalMod`
- `allclasses_2.3.1/decompiled assembly`
- `bettererprospecting_3.4.5/decompiled assembly`
- `cats_5.0.1/decompiled assembly`
- `configlib_1.12.0/decompiled assembly`
- `critcherscalculator_1.2.3/decompiled assembly`
- `jaunt_3.1.0/decompiled assembly`
- `petai_5.1.1/decompiled assembly`
- `primitivesurvival_5.1.0/decompiled assembly`
- `rpvoicechat_2.5.8/decompiled assemblies`
- `simplevsmanager_2.3.0/decompiled assemblies`
- `tentbagreworked_3.2.4/decompiled assembly`
- `vsimgui_1.2.7/decompiled assemblies`
- `wolftaming_5.0.1/decompiled assembly`

The code index is a conservative symbol index, not a compiler. It is good for finding likely files, declarations,
signatures, inheritance, and line numbers. For exact control flow or implementation details, open the returned source
file and inspect the relevant lines.

## Web Documentation Index

Use `web.sqlite` for curated external documentation that is not part of the local game assets or decompiled code. This
is currently useful for Config Lib JSON API questions, VS Village gameplay/design-reference questions, and RPVoiceChat
server/project-reference questions.

Important tables:

- `sources`
  - One row per external documentation page.
  - Useful columns include `slug`, `title`, `url`, `raw_url`, `retrieved_at_utc`, `last_known_edited`, `applies_to`,
    and `content_hash`.

- `sections`
  - One row per heading-based documentation section.
  - Useful columns include `heading_path`, `start_line`, `end_line`, and `body`.

- `web_text`
  - Full-text search table over source titles, heading paths, and section bodies.

Web query examples:

```powershell
$vsref = 'E:\Gaming\Vintage Story\mods\Vintage Story Reference'

python -B "$vsref\tools\web-index\query_web_index.py" --db "$vsref\versions\1.22.6\Index\web.sqlite" --list-sources
python -B "$vsref\tools\web-index\query_web_index.py" --db "$vsref\versions\1.22.6\Index\web.sqlite" --search "configlib-patches json" --limit 20
python -B "$vsref\tools\web-index\query_web_index.py" --db "$vsref\versions\1.22.6\Index\web.sqlite" --search "mapping values range logarithmic" --limit 20
python -B "$vsref\tools\web-index\query_web_index.py" --db "$vsref\versions\1.22.6\Index\web.sqlite" --heading "Conditional patches" --limit 20
python -B "$vsref\tools\web-index\query_web_index.py" --db "$vsref\versions\1.22.6\Index\web.sqlite" --source vsvillage --search "Summoning Horn Settlement Keeper" --limit 20
python -B "$vsref\tools\web-index\query_web_index.py" --db "$vsref\versions\1.22.6\Index\web.sqlite" --source vsvillage --search "villager pathfinding command" --limit 20
python -B "$vsref\tools\web-index\query_web_index.py" --db "$vsref\versions\1.22.6\Index\web.sqlite" --source vsvillage --heading "Village Generation" --limit 20
python -B "$vsref\tools\web-index\query_web_index.py" --db "$vsref\versions\1.22.6\Index\web.sqlite" --source rpvoicechat-server-administration --search "bitrate" --limit 20
python -B "$vsref\tools\web-index\query_web_index.py" --db "$vsref\versions\1.22.6\Index\web.sqlite" --source rpvoicechat-github-readme --search "Opus RNNoise" --limit 20
```

Current indexed web documentation sources:

- `configlib-json-api` - Config Lib JSON API GitHub wiki page.
- `vsvillage-moddb` - VS Village Mod DB page.
- `rpvoicechat-server-administration` - RPVoiceChat Server Administration GitHub wiki page.
- `rpvoicechat-github-readme` - RPVoiceChat GitHub README.

The web index stores searchable documentation content in SQLite and a manifest, but it does not keep separate local
HTML or Markdown snapshots. Treat web entries as dated reference material and use the `url` column when current
upstream documentation matters.

## When To Open Raw Files

Open raw files only after the index returns a specific candidate:

- Open a `.json` asset when you need exact variants, conditions, patch operations, comments, or syntax not captured as
  compact facts.
- Open a `.cs` source file when you need exact behavior, method bodies, comments, or surrounding class context.
- Open images or other binary assets only when visual inspection is necessary and the index has identified the
  specific asset.

All `rel_path` values returned by the indexes are relative to:

```text
E:\Gaming\Vintage Story\mods\Vintage Story Reference
```

## What Not To Do

- Do not recursively read `versions\1.22.6\Assets` into AI context.
- Do not recursively read mod `assets` folders into AI context.
- Do not recursively read `versions\1.22.6\API` or `versions\1.22.6\mods` into AI context.
- Do not load binary assets into text context.
- Do not treat `NOTES.md` as project metadata.
- Do not assume this archive can answer behavior for game versions that are no longer retained.
- Do not treat `references_hint` as a complete call graph.
- Do not treat decompiled source as build-ready project source.
- Do not treat web documentation index entries as live upstream content; they are dated index records.

## Refreshing Indexes

Other projects should generally consume these indexes as read-only. To refresh or add a new Vintage Story version,
return to this Vintage Story Reference repository and use the project-local indexer workflows:

- `index assets`
- `index reference code`
- `python -B tools\web-index\build_web_index.py --version 1.22.6`

After refreshing, other projects should reread this file and the relevant manifests before querying.
