# Project Memory

Last updated: 2026-07-14

## Project overview

Vintage Story Reference is a local reference library for AI-assisted Vintage Story modding. It stores versioned decompiled assemblies, extracted assets, selected mod references, and generated SQLite indexes so other coding agents can query narrow facts without scanning the full reference trees.

## Current state

The repository currently documents Vintage Story `1.22.3` as the indexed version. Existing asset and code index tooling lives under `tools/asset-index` and `tools/code-index`, with generated index artifacts under `versions/1.22.3/Index`.

The Config Lib JSON API GitHub wiki page and VS Village Mod DB page are indexed as curated external documentation. The source manifest is `versions/1.22.3/Web/sources.json`; the generated SQLite index is `versions/1.22.3/Index/web.sqlite`; the generated manifest is `versions/1.22.3/Index/web.index.json`; query tooling is under `tools/web-index`.

The repository also contains a root-level `prompts/` catalog for reusable Codex agent operating standards and reference-research workflows. Current prompt bundles cover persistent project memory, Vintage Story mod coding standards, project/Git hygiene, and Vintage Story research. `AGENTS.md` intentionally points at these project-local prompt files instead of the older sibling `Shared Agent Skills` folder.

The working tree contains many untracked reference and index files. Treat those as existing user/reference material unless the user explicitly asks to clean or commit them.

## Active work

No active work is currently in progress. The Config Lib web documentation index has been added and verified.

## Durable technical knowledge

Agents should query the SQLite indexes before reading raw reference trees. Asset questions should start with `assets.sqlite`; C# and symbol questions should start with `code.sqlite`; curated external documentation questions should start with `web.sqlite` once present.

Other Codex-assisted projects should read `INDEX_USAGE.md` when they need to use this repository as a Vintage Story knowledge source. They can paste `prompts/Vintage Story Research/Add to AGENTS.md` into their own `AGENTS.md` to learn the index-first research workflow. They should read or adopt the other files under `prompts/` when they need the user's agent operating standards.

The Config Lib JSON API upstream source is the GitHub wiki page at `https://github.com/maltiez2/vsmod_configlib/wiki/JSON-API`. Its raw Markdown endpoint is `https://raw.githubusercontent.com/wiki/maltiez2/vsmod_configlib/JSON-API.md`.

The VS Village upstream source is the Vintage Story Mod DB page at `https://mods.vintagestory.at/vsvillage`. The builder extracts the page's description tab via the `moddb_description` HTML scope so the searchable index focuses on gameplay/design-reference content and current Mod DB metadata instead of full site navigation or file tables.

As of the 2026-07-14 build, the web index contains `2` sources and `35` heading-based sections. Source content hashes are recorded in `versions/1.22.3/Index/web.index.json`.

## Architecture and design decisions

The web documentation index follows the existing project pattern of generated SQLite indexes plus small query helpers. It stores external documentation as dated searchable index content instead of adding separate local HTML or Markdown snapshots.

The Markdown splitter ignores headings inside fenced code blocks. This matters for the Config Lib page because YAML examples contain decorative `###` lines that should remain example text rather than becoming documentation headings.

The web index builder supports both Markdown-like sources and scoped HTML sources. For HTML pages, it converts selected HTML into Markdown-like text before sectioning so headings, lists, and paragraphs remain searchable through the same `sections` and `web_text` tables.

Project-specific structure decision: `prompts/` is intentionally kept at the repository root as a reusable prompt catalog, and `.codex/skills/` is intentionally kept for project-local Codex skills. This overrides the generic project standard that ordinary documentation belongs under `docs/`.

The Vintage Story Research prompt is deliberately separate from `INDEX_USAGE.md`: `INDEX_USAGE.md` documents this repository's current indexes and commands, while `prompts/Vintage Story Research/VINTAGE_STORY_RESEARCH.md` is the reusable instruction set that can be referenced or copied into other projects.

The drop-in `prompts/Vintage Story Research/Add to AGENTS.md` snippet should remain version-agnostic. It should refer to `versions/<game-version>/...` and instruct consuming projects to determine their target Vintage Story version, then consult `INDEX_USAGE.md` for currently indexed versions. Do not hardcode `1.22.3` in that reusable snippet; version-specific paths belong in `INDEX_USAGE.md`, manifests, or local examples.

## External interfaces and integrations

The web index source manifest is `versions/1.22.3/Web/sources.json`. Each source entry includes a public page URL, raw fetch URL, title, source type, applicability notes, and last-known edit date when available.

## Known failed approaches

No failed approaches have been recorded yet.

## Bugs, risks, and limitations

The web index is a dated snapshot of fetched upstream content. If current documentation matters, rebuild the index or check the upstream URL.

The working tree currently has many untracked files that appear to be reference payloads or generated indexes. Do not assume a clean Git state.

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
```

On 2026-07-14, those commands succeeded. `git diff --check` reported only Git line-ending warnings for `INDEX_USAGE.md` and `README.md`.

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
- `prompts` - Reusable Codex agent prompt standards for project memory, coding standards, project/Git hygiene, and Vintage Story research.
- `.codex/skills` - Project-local Codex skills for indexing assets, indexing decompiled C# code, and updating reference mod folders.
- `versions/1.22.3/Index` - Generated SQLite indexes and manifests.
- `versions/1.22.3/Web/sources.json` - Curated external web documentation source list.

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
