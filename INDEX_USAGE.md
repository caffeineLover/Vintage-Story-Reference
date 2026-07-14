# Vintage Story Reference Instructions for AI Projects

This repository is a local reference library for AI-assisted Vintage Story modding work. Other projects should use it as a searchable reference source, not as a project to build or copy wholesale.

The most important rule: query the SQLite indexes first. Do not recursively scan or read the full `Assets`, `API`, or `mods` trees unless an index cannot answer the question.

## Instruction Block for Other AI Projects

Use this text when pointing another AI-enabled project at this repository:

```text
Use the Vintage Story Reference repository at:
E:\Gaming\Vintage Story\mods\Vintage Story Reference

Before scanning raw Vintage Story assets, mod assets, or decompiled C# source, query the reference indexes:
- Asset index: E:\Gaming\Vintage Story\mods\Vintage Story Reference\versions\1.22.3\Index\assets.sqlite
- Code index: E:\Gaming\Vintage Story\mods\Vintage Story Reference\versions\1.22.3\Index\code.sqlite
- Web documentation index: E:\Gaming\Vintage Story\mods\Vintage Story Reference\versions\1.22.3\Index\web.sqlite

Use the helper query scripts:
- E:\Gaming\Vintage Story\mods\Vintage Story Reference\tools\asset-index\query_asset_index.py
- E:\Gaming\Vintage Story\mods\Vintage Story Reference\tools\code-index\query_code_index.py
- E:\Gaming\Vintage Story\mods\Vintage Story Reference\tools\web-index\query_web_index.py

Keep result limits small. Open raw files only after an index returns candidate paths and line numbers. Treat the indexes as locators and summaries, not as the final source of truth for exact implementation details. Web documentation entries are dated index content; check the upstream URL when recency matters. Do not read NOTES.md as metadata; it is personal notes only. Do not load binary assets into AI context.

If sandbox permissions prevent reading this folder, ask the user to add the Vintage Story Reference repository as a readable workspace root or otherwise grant read access. Do not ask the user to paste or attach the full asset/source trees.
```

## What This Repository Contains

- `versions/<game-version>/API`
  - Decompiled C# reference code for Vintage Story assemblies.
  - For `1.22.3`, this includes `VintagestoryAPI`, `VintagestoryLib`, and `VSSurvivalMod`.
  - Use this when answering API, vanilla behavior, class, method, interface, registration, and implementation questions.

- `versions/<game-version>/Assets`
  - Extracted Vintage Story asset trees such as `creative`, `game`, and `survival`.
  - Includes blocktypes, itemtypes, recipes, lang files, shapes, textures, sounds, worldgen data, shaders, config, and related asset metadata.
  - Use this when answering questions about JSON assets, recipes, blocks, items, textures, shapes, localization, worldgen, and vanilla content definitions.

- `versions/<game-version>/mods`
  - Selected downloaded mods kept as reference material.
  - Mod `assets` folders are included in the asset index when present.
  - Some mods include decompiled source folders. The code index includes decompiled mod code when present.
  - Old mod versions may remain side by side. Do not assume the newest-looking folder is the only relevant one; check manifests and folder names.

- `versions/<game-version>/Web`
  - Curated external documentation source manifests.
  - These source lists feed the generated web documentation index.
  - The web index is for searchable documentation knowledge, not local page snapshots.

- `versions/<game-version>/Index`
  - Stable generated index artifacts.
  - `assets.sqlite` and `assets.index.json` describe assets.
  - `code.sqlite` and `code.index.json` describe decompiled C# code.
  - `web.sqlite` and `web.index.json` describe curated external documentation.

- `tools/asset-index`
  - Scripts for building and querying the asset index.

- `tools/code-index`
  - Scripts for building and querying the code index.

- `tools/web-index`
  - Scripts for building and querying the curated external documentation index.

- `NOTES.md`
  - Personal user notes. Do not treat this file as authoritative metadata.

## Current Version: 1.22.3

Repository root:

```text
E:\Gaming\Vintage Story\mods\Vintage Story Reference
```

Current indexed version:

```text
E:\Gaming\Vintage Story\mods\Vintage Story Reference\versions\1.22.3
```

Asset index:

```text
E:\Gaming\Vintage Story\mods\Vintage Story Reference\versions\1.22.3\Index\assets.sqlite
E:\Gaming\Vintage Story\mods\Vintage Story Reference\versions\1.22.3\Index\assets.index.json
```

Code index:

```text
E:\Gaming\Vintage Story\mods\Vintage Story Reference\versions\1.22.3\Index\code.sqlite
E:\Gaming\Vintage Story\mods\Vintage Story Reference\versions\1.22.3\Index\code.index.json
```

Web documentation index:

```text
E:\Gaming\Vintage Story\mods\Vintage Story Reference\versions\1.22.3\Index\web.sqlite
E:\Gaming\Vintage Story\mods\Vintage Story Reference\versions\1.22.3\Index\web.index.json
```

At the time these instructions were written, the `1.22.3` asset index covered about `21,921` vanilla and mod asset files, the code index covered about `4,100` C# files, `4,233` types, and `20,372` members, and the web index covered `2` external documentation sources split into `35` sections. Treat those as sanity-check values, not permanent truth.

## Recommended Workflow

1. Identify the Vintage Story version being targeted.
2. Read the matching manifests in `versions/<version>/Index`.
3. Query `assets.sqlite` for asset questions or `code.sqlite` for C# questions.
4. Query `web.sqlite` for curated external documentation such as Config Lib JSON API behavior or VS Village gameplay behavior.
5. Use small limits, usually `10` to `50`.
6. Open only the raw files returned by the index.
7. For exact behavior, read the specific source file and line range returned by the code index.
8. For exact JSON details, read the specific asset file returned by the asset index.
9. For external documentation, inspect the source URL when current upstream behavior matters.
10. If the index is missing, stale, or for the wrong game version, ask the user to refresh the index from this reference project.

## Access From Another Workspace

From any PowerShell session, define the reference root once:

```powershell
$vsref = 'E:\Gaming\Vintage Story\mods\Vintage Story Reference'
```

Then call query scripts by absolute path:

```powershell
python -B "$vsref\tools\asset-index\query_asset_index.py" --db "$vsref\versions\1.22.3\Index\assets.sqlite" --search copper --limit 20
python -B "$vsref\tools\code-index\query_code_index.py" --db "$vsref\versions\1.22.3\Index\code.sqlite" --type ICoreAPI --limit 20
python -B "$vsref\tools\web-index\query_web_index.py" --db "$vsref\versions\1.22.3\Index\web.sqlite" --search "conditional patches" --limit 20
```

Use `python -B` so Python does not create `__pycache__` folders.

If a tool sandbox cannot read `E:\Gaming\Vintage Story\mods\Vintage Story Reference`, ask for read access to that folder. Do not load broad directory listings or file contents into the prompt as a workaround.

## Asset Index

Use `assets.sqlite` for vanilla and indexed mod asset, content, JSON, texture, shape, recipe, localization, and worldgen questions.

Important tables:

- `assets`
  - One row per indexed asset file.
  - Useful columns include `rel_path`, `domain`, `category`, `subcategory`, `extension`, `size`, `is_json`, `parse_status`, `png_width`, and `png_height`.

- `json_assets`
  - Compact facts extracted from parseable JSON files.
  - Useful columns include `fact_key`, `fact_value`, `value_type`, and `json_path`.

- `asset_text`
  - Full-text search table over compact asset metadata and extracted JSON text.

Asset query examples:

```powershell
$vsref = 'E:\Gaming\Vintage Story\mods\Vintage Story Reference'

python -B "$vsref\tools\asset-index\query_asset_index.py" --db "$vsref\versions\1.22.3\Index\assets.sqlite" --search copper --limit 20
python -B "$vsref\tools\asset-index\query_asset_index.py" --db "$vsref\versions\1.22.3\Index\assets.sqlite" --category recipes --limit 20
python -B "$vsref\tools\asset-index\query_asset_index.py" --db "$vsref\versions\1.22.3\Index\assets.sqlite" --domain survival --category blocktypes --limit 20
python -B "$vsref\tools\asset-index\query_asset_index.py" --db "$vsref\versions\1.22.3\Index\assets.sqlite" --fact-key output --fact-value ingot-copper --limit 20
```

Asset categories in `1.22.3` include `blocktypes`, `itemtypes`, `recipes`, `lang`, `shapes`, `textures`, `worldgen`, `entities`, `sounds`, `config`, `patches`, `shaders`, and related folders.

The asset manifest records all indexed asset roots. For `1.22.3`, this includes `versions\1.22.3\Assets` and mod roots such as `versions\1.22.3\mods\A Panserbjorn Story Rebjorn (1.0.0)\assets`, `versions\1.22.3\mods\jaunt_3.0.0-rc.3\assets`, and other mod `assets` folders.

For binary assets such as `.png`, `.ogg`, `.ttf`, `.gltf`, and shader files, use index metadata first. Do not read binary contents into AI context. If visual inspection is needed, open only the specific file identified by the index.

Some Vintage Story JSON uses JSON-like syntax that may not parse as strict JSON. The asset index still keeps rows for parse-error files in `assets`; open the raw file only when the specific file matters.

## Code Index

Use `code.sqlite` for API, decompiled vanilla code, selected decompiled mod code, symbols, types, methods, inheritance, and source-location questions.

Important tables:

- `source_roots`
  - Indexed roots such as `VintagestoryAPI`, `VintagestoryLib`, `VSSurvivalMod`, and decompiled mod roots.

- `source_files`
  - One row per indexed `.cs` file.
  - Paths are repository-relative in `rel_path`.

- `types`
  - Type declarations with namespace, name, full name, kind, accessibility, base type, interfaces, attributes, and line ranges.

- `members`
  - Methods, constructors, properties, fields, and events with signatures and line ranges.

- `references_hint`
  - Token-based reference hints. Useful for narrowing candidates, not a reliable call graph.

- `code_text`
  - Full-text search table over compact symbol metadata.

Code query examples:

```powershell
$vsref = 'E:\Gaming\Vintage Story\mods\Vintage Story Reference'

python -B "$vsref\tools\code-index\query_code_index.py" --db "$vsref\versions\1.22.3\Index\code.sqlite" --type ICoreAPI --limit 20
python -B "$vsref\tools\code-index\query_code_index.py" --db "$vsref\versions\1.22.3\Index\code.sqlite" --member RegisterBlockClass --limit 20
python -B "$vsref\tools\code-index\query_code_index.py" --db "$vsref\versions\1.22.3\Index\code.sqlite" --base BlockEntity --limit 20
python -B "$vsref\tools\code-index\query_code_index.py" --db "$vsref\versions\1.22.3\Index\code.sqlite" --root VSSurvivalMod --search Firepit --limit 20
```

The code index is a conservative symbol index, not a compiler. It is good for finding likely files, declarations, signatures, inheritance, and line numbers. For exact control flow or implementation details, open the returned source file and inspect the relevant lines.

## Web Documentation Index

Use `web.sqlite` for curated external documentation that is not part of the local game assets or decompiled code. This is currently useful for Config Lib JSON API questions and VS Village gameplay/design-reference questions.

Important tables:

- `sources`
  - One row per external documentation page.
  - Useful columns include `slug`, `title`, `url`, `raw_url`, `retrieved_at_utc`, `last_known_edited`, `applies_to`, and `content_hash`.

- `sections`
  - One row per heading-based documentation section.
  - Useful columns include `heading_path`, `start_line`, `end_line`, and `body`.

- `web_text`
  - Full-text search table over source titles, heading paths, and section bodies.

Web query examples:

```powershell
$vsref = 'E:\Gaming\Vintage Story\mods\Vintage Story Reference'

python -B "$vsref\tools\web-index\query_web_index.py" --db "$vsref\versions\1.22.3\Index\web.sqlite" --list-sources
python -B "$vsref\tools\web-index\query_web_index.py" --db "$vsref\versions\1.22.3\Index\web.sqlite" --search "configlib-patches json" --limit 20
python -B "$vsref\tools\web-index\query_web_index.py" --db "$vsref\versions\1.22.3\Index\web.sqlite" --search "mapping values range logarithmic" --limit 20
python -B "$vsref\tools\web-index\query_web_index.py" --db "$vsref\versions\1.22.3\Index\web.sqlite" --heading "Conditional patches" --limit 20
python -B "$vsref\tools\web-index\query_web_index.py" --db "$vsref\versions\1.22.3\Index\web.sqlite" --source vsvillage --search "Summoning Horn Settlement Keeper" --limit 20
python -B "$vsref\tools\web-index\query_web_index.py" --db "$vsref\versions\1.22.3\Index\web.sqlite" --source vsvillage --search "villager pathfinding command" --limit 20
python -B "$vsref\tools\web-index\query_web_index.py" --db "$vsref\versions\1.22.3\Index\web.sqlite" --source vsvillage --heading "Village Generation" --limit 20
```

Current indexed web documentation sources in `1.22.3`:

- `configlib-json-api` - Config Lib JSON API GitHub wiki page, last known edited `2024-05-14`, retrieved into the index on `2026-07-14`.
- `vsvillage-moddb` - VS Village Mod DB page, last known modified `2026-06-09`, retrieved into the index on `2026-07-14`.

The web index stores searchable documentation content in SQLite and a manifest, but it does not keep separate local HTML or Markdown snapshots. Treat web entries as dated reference material and use the `url` column when current upstream documentation matters.

## Current Code Roots in 1.22.3

The current `1.22.3` code index includes:

- `VintagestoryAPI`
- `VintagestoryLib`
- `VSSurvivalMod`
- `allclasses2.3.0/decompiled assembly`
- `cats_v5.0.1/decompiled assembly`
- `configlib_1.12.0/configlib_1.12.0 - decompiled`
- `jaunt_3.0.0-rc.3/decompiled-assembly`
- `petai_v5.1.1/decompiled assembly`
- `vsimgui_1.2.7/decompiled assemblies`

Other mod folders may exist but may not be present in the code index if they do not contain decompiled `.cs` files.

## When To Open Raw Files

Open raw files only after the index returns a specific candidate:

- Open a `.json` asset when you need exact variants, conditions, patch operations, comments, or syntax not captured as compact facts.
- Open a `.cs` source file when you need exact behavior, method bodies, comments, or surrounding class context.
- Open images or other binary assets only when visual inspection is necessary and the index has identified the specific asset.

All `rel_path` values returned by the indexes are relative to:

```text
E:\Gaming\Vintage Story\mods\Vintage Story Reference
```

## What Not To Do

- Do not recursively read `versions\1.22.3\Assets` into AI context.
- Do not recursively read mod `assets` folders into AI context.
- Do not recursively read `versions\1.22.3\API` or `versions\1.22.3\mods` into AI context.
- Do not load binary assets into text context.
- Do not treat `NOTES.md` as project metadata.
- Do not assume `1.22.3` is the right version for every future question.
- Do not treat `references_hint` as a complete call graph.
- Do not treat decompiled source as build-ready project source.
- Do not treat web documentation index entries as live upstream content; they are dated index records.

## Refreshing Indexes

Other projects should generally consume these indexes as read-only. To refresh or add a new Vintage Story version, return to this Vintage Story Reference repository and use the project-local indexer workflows:

- `index assets`
- `index reference code`
- `python -B tools\web-index\build_web_index.py --version 1.22.3`

After refreshing, other projects should reread this file and the relevant manifests before querying.
