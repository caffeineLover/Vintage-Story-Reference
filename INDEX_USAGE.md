# Vintage Story Reference Instructions for AI Projects

This repository is a local reference library for AI-assisted Vintage Story modding work. Other projects should use it
as a searchable reference source, not as a project to build or copy wholesale.

Always query the SQLite indexes and tracked mod manifest first. Do not recursively scan or read the full `Assets`,
`API`, or `mods` trees unless those query sources cannot answer the question.

`INDEX_USAGE.md` is the handoff document for using this repository as a Vintage Story knowledge source. For a reusable
AGENTS.md prompt that teaches other Codex-assisted projects this research workflow, use
`prompts\Vintage Story Research\Add to AGENTS.md`. For reusable agent operating standards such as persistent project
memory, coding standards, and project/Git hygiene, use the project-local prompt catalog under `prompts/`.
For tested, task-shaped query sequences that cross multiple sources, use `docs/QUERY_COOKBOOK.md`.

For machine-readable discovery, use `reference.catalog.json` and `versions/<game-version>/version.catalog.json`.
For a single query entry point, prefer `tools/reference-query.py`; the source-specific query scripts remain available
when you need their exact interfaces.

The retained indexed versions are Vintage Story `1.22.6` and `1.22.7`. Both have decompiled API sources, copied full
client assets, selected reference mods/tools, copied curated web sources, and generated code/asset/web indexes.
`1.22.7` is the catalog default. The newest retained Vintage Story release is always the default.

## Instruction Block For Other AI Projects

Use this text when pointing another AI-enabled project at this repository:

```text
Use the Vintage Story Reference repository at:
E:\Gaming\Vintage Story\mods\Vintage Story Reference

Before scanning raw Vintage Story assets, mod assets, or decompiled C# source, query the reference indexes:
- Asset index: E:\Gaming\Vintage Story\mods\Vintage Story Reference\versions\1.22.7\Index\assets.sqlite
- Code index: E:\Gaming\Vintage Story\mods\Vintage Story Reference\versions\1.22.7\Index\code.sqlite
- Web documentation index: E:\Gaming\Vintage Story\mods\Vintage Story Reference\versions\1.22.7\Index\web.sqlite
- Mod manifest: E:\Gaming\Vintage Story\mods\Vintage Story Reference\versions\1.22.7\mods.manifest.json

These paths use the catalog default. Replace `1.22.7` with `1.22.6` when the consuming project targets Vintage Story
1.22.6.

Use the helper query scripts:
- E:\Gaming\Vintage Story\mods\Vintage Story Reference\tools\reference-query.py
- E:\Gaming\Vintage Story\mods\Vintage Story Reference\tools\asset-index\query_asset_index.py
- E:\Gaming\Vintage Story\mods\Vintage Story Reference\tools\code-index\query_code_index.py
- E:\Gaming\Vintage Story\mods\Vintage Story Reference\tools\cross-reference\query_class_cross_reference.py
- E:\Gaming\Vintage Story\mods\Vintage Story Reference\tools\mod-manifest\query_mod_manifest.py
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

- `versions/<game-version>/API`
  - Decompiled C# reference code for `VintagestoryAPI`, `VintagestoryLib`, `VSCreativeMod`, `VSEssentials`, and
    `VSSurvivalMod`.
  - Use this when answering API, vanilla behavior, class, method, interface, registration, and implementation
    questions.

- `versions/<game-version>/Assets`
  - Extracted Vintage Story asset trees such as `creative`, `game`, and `survival`.
  - Includes blocktypes, itemtypes, recipes, lang files, shapes, textures, sounds, worldgen data, shaders, config, and
    related asset metadata.

- `versions/<game-version>/mods`
  - Selected downloaded mods and tools kept as reference material.
  - Mod `assets` folders are included in the asset index when present.
  - Decompiled mod/tool source folders are included in the code index when present.
  - Simple VS Manager is an external-tool reference, not a normal in-game mod package.

- `versions/<game-version>/mods.manifest.json`
  - Tracked, queryable identity, compatibility, source-provenance, retained-content, and license-file metadata for the
    18 selected mod/tool entries.

- `versions/<game-version>/Web`
  - Curated external documentation source manifests.
  - These source lists feed the generated web documentation index.
  - The web index is for searchable documentation knowledge, not local page snapshots.

- `versions/<game-version>/Index`
  - Stable generated index artifacts.
  - `assets.sqlite` and `assets.index.json` describe assets.
  - `code.sqlite` and `code.index.json` describe decompiled C# code.
  - `web.sqlite` and `web.index.json` describe curated external documentation.
  - Generated manifests remain the source of truth for exact index generation details.

- `versions/<game-version>/version.catalog.json`
  - Version-specific discovery catalog for reference roots, indexes, source manifests, and query/build scripts.

- `reference.catalog.json`
  - Root discovery catalog for available versions, docs, prompts, skills, and the query wrapper.

- `tools/asset-index`, `tools/code-index`, `tools/cross-reference`, `tools/mod-manifest`, and `tools/web-index`
  - Scripts for building/querying indexes and querying the tracked mod manifest.

- `tools/reference-query.py`
  - Unified wrapper that resolves the selected version and delegates to asset, code, class-cross-reference,
    mod-manifest, or web query helpers.

- `prompts`
  - Reusable agent prompt standards maintained with this repository.

- `.codex/skills`
  - Project-local Codex skills for maintaining this reference repository.

- `NOTES.md`
  - Personal user notes. Do not treat this file as authoritative metadata.

## Retained Version: 1.22.6

Repository root:

```text
E:\Gaming\Vintage Story\mods\Vintage Story Reference
```

Version root:

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

Selected-mod manifest:

```text
E:\Gaming\Vintage Story\mods\Vintage Story Reference\versions\1.22.6\mods.manifest.json
```

The asset index currently covers `22,792` files, including `11,618` JSON files with `11,269` parsed successfully and
`349` parse-error rows retained for path/category lookup. The code index currently covers `8,428` C# files, `9,543`
types, `94,449` members, and `820` literal registrations across `20` source roots. The class cross-reference currently
joins `710` asset rows across `395` distinct class keys. The web index covers `4` copied sources and `84` sections.

## Default Version: 1.22.7

Version root:

```text
E:\Gaming\Vintage Story\mods\Vintage Story Reference\versions\1.22.7
```

Generated indexes:

```text
E:\Gaming\Vintage Story\mods\Vintage Story Reference\versions\1.22.7\Index\assets.sqlite
E:\Gaming\Vintage Story\mods\Vintage Story Reference\versions\1.22.7\Index\code.sqlite
E:\Gaming\Vintage Story\mods\Vintage Story Reference\versions\1.22.7\Index\web.sqlite
```

Tracked selected-mod manifest:

```text
E:\Gaming\Vintage Story\mods\Vintage Story Reference\versions\1.22.7\mods.manifest.json
```

The 1.22.7 asset index covers `22,837` files, including `11,660` JSON files with `11,311` parsed successfully and
`349` parse-error rows retained for path/category lookup. The code index covers `8,607` C# files, `9,732` types,
`95,780` members, and `836` literal registrations across `20` source roots. The class cross-reference joins `722`
asset rows across `407` case-insensitive class keys. The web index covers `4` copied sources and `99` sections. The mod
manifest contains `18` entries: `11` exact 1.22.7 releases, `6` same-line fallbacks, and `1` external tool.

## Recommended Workflow

1. Identify the Vintage Story version being targeted.
2. Use the consuming project's exact target version when it is `1.22.6` or `1.22.7`; otherwise use the catalog
   default only as an explicit fallback assumption.
3. Read the matching generated manifests in `versions/<game-version>/Index`.
4. Query `mods.manifest.json` first when selected third-party reference material may be relevant.
5. Query `assets.sqlite` for asset questions or `code.sqlite` for C# questions.
6. Query the `class` cross-reference when an asset `class` or `entityClass` should lead to its registration and C#
   declaration.
7. Query `web.sqlite` for curated external documentation such as Config Lib JSON API behavior, VS Village gameplay
   behavior, or RPVoiceChat administration and project docs.
8. Use small limits, usually `10` to `50`.
9. Open only the raw files returned by the query source.
10. For exact behavior, read the specific source file and line range returned by the code index.
11. For exact JSON details, read the specific asset file returned by the asset index.
12. For external documentation, inspect the source URL when current upstream behavior matters.
13. If an index or manifest is missing, stale, or for the wrong game version, return to this repository and refresh
    the reference metadata.

## Access From Another Workspace

From any PowerShell session, define the reference root once:

```powershell
$vsref = 'E:\Gaming\Vintage Story\mods\Vintage Story Reference'
```

Then call query scripts by absolute path:

```powershell
python -B "$vsref\tools\reference-query.py" --list-versions
python -B "$vsref\tools\reference-query.py" --version 1.22.6 --list-indexes
python -B "$vsref\tools\reference-query.py" --version 1.22.7 --list-indexes
python -B "$vsref\tools\reference-query.py" --version 1.22.7 --list-cross-references
python -B "$vsref\tools\reference-query.py" --version 1.22.7 --list-manifests
python -B "$vsref\tools\reference-query.py" --version 1.22.7 asset --search copper --limit 20
python -B "$vsref\tools\reference-query.py" --version 1.22.7 code --type ICoreAPI --limit 20
python -B "$vsref\tools\reference-query.py" --version 1.22.7 class --asset-class BlockDisplayCase --limit 20
python -B "$vsref\tools\reference-query.py" --version 1.22.7 mod --compatibility exact --limit 20
python -B "$vsref\tools\reference-query.py" --version 1.22.7 web --search "conditional patches" --limit 20
```

The source-specific helpers are also available:

```powershell
python -B "$vsref\tools\asset-index\query_asset_index.py" --db "$vsref\versions\1.22.7\Index\assets.sqlite" --search copper --limit 20
python -B "$vsref\tools\code-index\query_code_index.py" --db "$vsref\versions\1.22.7\Index\code.sqlite" --type ICoreAPI --limit 20
python -B "$vsref\tools\cross-reference\query_class_cross_reference.py" `
  --asset-db "$vsref\versions\1.22.7\Index\assets.sqlite" `
  --code-db "$vsref\versions\1.22.7\Index\code.sqlite" `
  --asset-class BlockDisplayCase --limit 20
python -B "$vsref\tools\mod-manifest\query_mod_manifest.py" --manifest "$vsref\versions\1.22.7\mods.manifest.json" --compatibility exact --limit 20
python -B "$vsref\tools\web-index\query_web_index.py" --db "$vsref\versions\1.22.7\Index\web.sqlite" --search "conditional patches" --limit 20
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

python -B "$vsref\tools\asset-index\query_asset_index.py" --db "$vsref\versions\1.22.7\Index\assets.sqlite" --search copper --limit 20
python -B "$vsref\tools\asset-index\query_asset_index.py" --db "$vsref\versions\1.22.7\Index\assets.sqlite" --category recipes --limit 20
python -B "$vsref\tools\asset-index\query_asset_index.py" --db "$vsref\versions\1.22.7\Index\assets.sqlite" --domain survival --category blocktypes --limit 20
python -B "$vsref\tools\asset-index\query_asset_index.py" --db "$vsref\versions\1.22.7\Index\assets.sqlite" --fact-key output --fact-value ingot-brass --limit 20
```

The asset manifest records all indexed asset roots, including the selected version's `Assets` tree and mod roots with
`assets` folders. For binary assets such as `.png`, `.ogg`, `.ttf`, `.gltf`, and shader files, use index metadata
first. Do not read binary contents into AI context.

Some Vintage Story JSON uses JSON-like syntax that may not parse as strict JSON. The asset index still keeps rows for
parse-error files in `assets`; open the raw file only when the specific file matters.

## Mod Manifest

Use `mods.manifest.json` to select and scope third-party reference material before querying mod assets or decompiled
code. Unlike the generated SQLite manifests under `Index`, this is a tracked, curated source file.

Each entry records:

- `mod_id`, display name, version, package type, and retained folder;
- exact, same-line-fallback, or external-tool compatibility status plus Mod DB release tags;
- Mod DB/source URLs, local metadata path, release id/timestamp when available, and archive SHA-256;
- whether the retained folder has assets, decompiled code, indexed code-root names, or native binaries;
- whether an explicit license file was found and the scope of that observation.

Mod-manifest query examples:

```powershell
$vsref = 'E:\Gaming\Vintage Story\mods\Vintage Story Reference'

python -B "$vsref\tools\mod-manifest\query_mod_manifest.py" --manifest "$vsref\versions\1.22.7\mods.manifest.json" --list
python -B "$vsref\tools\mod-manifest\query_mod_manifest.py" --manifest "$vsref\versions\1.22.7\mods.manifest.json" --mod-id tentbagreworked --format json
python -B "$vsref\tools\mod-manifest\query_mod_manifest.py" --manifest "$vsref\versions\1.22.7\mods.manifest.json" --compatibility exact
python -B "$vsref\tools\mod-manifest\query_mod_manifest.py" --manifest "$vsref\versions\1.22.7\mods.manifest.json" --has-assets yes --has-code yes
python -B "$vsref\tools\mod-manifest\query_mod_manifest.py" --manifest "$vsref\versions\1.22.7\mods.manifest.json" --has-native-binaries yes
```

The same filters work through `tools/reference-query.py` with the `mod` or `mods` target. A missing license file does
not prove that an upstream project lacks a license; verify upstream licensing before reusing third-party material.

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

- `registrations`
  - Literal supported registration calls with the registration key, `typeof(...)` type, call-site line, compact
    evidence, and an optional link to the uniquely resolved type declaration.

- `code_text`
  - Full-text search table over compact symbol metadata.

Code query examples:

```powershell
$vsref = 'E:\Gaming\Vintage Story\mods\Vintage Story Reference'

python -B "$vsref\tools\code-index\query_code_index.py" --db "$vsref\versions\1.22.7\Index\code.sqlite" --type ICoreAPI --limit 20
python -B "$vsref\tools\code-index\query_code_index.py" --db "$vsref\versions\1.22.7\Index\code.sqlite" --member StartServerSide --root VintagestoryAPI --limit 20
python -B "$vsref\tools\code-index\query_code_index.py" `
  --db "$vsref\versions\1.22.7\Index\code.sqlite" `
  --registration BlockDisplayCase --format json --limit 20
python -B "$vsref\tools\code-index\query_code_index.py" --db "$vsref\versions\1.22.7\Index\code.sqlite" --reference ICoreAPI --root VSEssentials --limit 20
python -B "$vsref\tools\code-index\query_code_index.py" --db "$vsref\versions\1.22.7\Index\code.sqlite" --base BlockEntity --limit 20
python -B "$vsref\tools\code-index\query_code_index.py" --db "$vsref\versions\1.22.7\Index\code.sqlite" --root VSSurvivalMod --search Firepit --limit 20
```

The default 1.22.7 code roots include:

- `VintagestoryAPI`
- `VintagestoryLib`
- `VSCreativeMod`
- `VSEssentials`
- `VSSurvivalMod`
- `allclasses_2.3.2/decompiled assembly`
- `bettererprospecting_3.4.8/decompiled assembly`
- `cats_5.0.2/decompiled assembly`
- `configlib_1.13.2/decompiled assembly`
- `critcherscalculator_1.3.0/decompiled assembly`
- `firearmsfork_1.10.2/decompiled assembly`
- `jaunt_3.1.0/decompiled assembly`
- `overhaulliblegacycompat_1.2.10/decompiled assembly`
- `petai_5.1.1/decompiled assembly`
- `primitivesurvival_5.1.3/decompiled assembly`
- `rpvoicechat_2.6.5/decompiled assemblies`
- `simplevsmanager_2.3.0/decompiled assemblies`
- `tentbagreworked_3.2.5/decompiled assembly`
- `vsimgui_1.2.7/decompiled assemblies`
- `wolftaming_5.0.1/decompiled assembly`

The code index is a conservative symbol index, not a compiler. It is good for finding likely files, declarations,
signatures, inheritance, and line numbers. For exact control flow or implementation details, open the returned source
file and inspect the relevant lines.

## Class Cross-Reference

Use the `class`, `classes`, or `xref` wrapper target to join top-level asset class facts to literal registration calls.
The result identifies the asset, registration key and method, registration call site, registered type, and resolved C#
declaration when available:

```powershell
$vsref = 'E:\Gaming\Vintage Story\mods\Vintage Story Reference'

python -B "$vsref\tools\reference-query.py" --version 1.22.7 class `
  --asset-class BlockDisplayCase --limit 20

python -B "$vsref\tools\reference-query.py" --version 1.22.7 xref `
  --asset-class tentbagreworked.itemtentbag --format json --limit 20
```

The join is category-aware: block `class`, item `class`, entity `class`, and block `entityClass` values are matched only
to their corresponding registries. It does not infer dynamic keys, helper-wrapped calls, factories, reflection, JSON
inheritance, or runtime patches. An empty result means no supported evidence pair was indexed, not that no relationship
exists. Open the returned JSON and C# paths before making an exact behavioral claim.

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

python -B "$vsref\tools\web-index\query_web_index.py" --db "$vsref\versions\1.22.7\Index\web.sqlite" --list-sources
python -B "$vsref\tools\web-index\query_web_index.py" --db "$vsref\versions\1.22.7\Index\web.sqlite" --search "configlib-patches json" --limit 20
python -B "$vsref\tools\web-index\query_web_index.py" --db "$vsref\versions\1.22.7\Index\web.sqlite" --search "mapping values range logarithmic" --limit 20
python -B "$vsref\tools\web-index\query_web_index.py" --db "$vsref\versions\1.22.7\Index\web.sqlite" --heading "Conditional patches" --limit 20
python -B "$vsref\tools\web-index\query_web_index.py" --db "$vsref\versions\1.22.7\Index\web.sqlite" --source vsvillage --search "Summoning Horn Settlement Keeper" --limit 20
python -B "$vsref\tools\web-index\query_web_index.py" --db "$vsref\versions\1.22.7\Index\web.sqlite" --source vsvillage --search "villager pathfinding command" --limit 20
python -B "$vsref\tools\web-index\query_web_index.py" --db "$vsref\versions\1.22.7\Index\web.sqlite" --source vsvillage --heading "Village Generation" --limit 20
python -B "$vsref\tools\web-index\query_web_index.py" --db "$vsref\versions\1.22.7\Index\web.sqlite" --source rpvoicechat-server-administration --search "bitrate" --limit 20
python -B "$vsref\tools\web-index\query_web_index.py" --db "$vsref\versions\1.22.7\Index\web.sqlite" --source rpvoicechat-github-readme --search "Opus RNNoise" --limit 20
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

- Do not recursively read `versions\<game-version>\Assets` into AI context.
- Do not recursively read mod `assets` folders into AI context.
- Do not recursively read `versions\<game-version>\API` or `versions\<game-version>\mods` into AI context.
- Do not load binary assets into text context.
- Do not treat `NOTES.md` as project metadata.
- Do not assume this archive can answer behavior for game versions that are no longer retained.
- Do not treat `references_hint` as a complete call graph.
- Do not treat literal `registrations` or class cross-references as an exhaustive runtime registry.
- Do not treat `license.file_present: false` as proof that an upstream project has no license.
- Do not treat decompiled source as build-ready project source.
- Do not treat web documentation index entries as live upstream content; they are dated index records.

## Refreshing Indexes

Other projects should generally consume these indexes as read-only. To refresh or add a new Vintage Story version,
return to this Vintage Story Reference repository and use the project-local indexer workflows:

- `index assets`
- `index reference code`
- `.codex/skills/new-version-mods/SKILL.md` for selected-mod updates and `mods.manifest.json` maintenance
- `python -B tools\web-index\build_web_index.py --version <game-version>`

After refreshing, other projects should reread this file and the relevant manifests before querying.
