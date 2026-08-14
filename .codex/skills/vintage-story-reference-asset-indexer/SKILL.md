---
name: vintage-story-reference-asset-indexer
description: Project-local workflow for building, refreshing, and using the Vintage Story Reference asset index. Use when answering questions about assets, recipes, patches, blocks, items, entities, textures, shapes, or language files in this repo, or when the user asks to build or update the asset index.
---

# Vintage Story Reference Asset Indexer

## Scope

Use this skill only inside the Vintage Story Reference repository. The repository root is expected to contain `versions/`, and extracted game assets live under `versions/<game-version>/Assets`.

The goal is to save memory, time, and AI credits by querying a compact local SQLite index before opening raw asset files.

## Hard Rules

- Query the asset index before recursively scanning or reading `versions/<game-version>/Assets`.
- Read raw asset JSON only after the index narrows the candidate set.
- Do not read binary asset contents into context. Index binary files by metadata only.
- Do not store indexes under `.codex/tmp`; use stable versioned index artifacts under `versions/<game-version>/Index`.
- Treat `NOTES.md` as personal user notes, not authoritative metadata.
- Do not treat the index as the source of truth when exact file content matters; use it to find candidate files, then open the specific source files.
- Keep query result sets small. Prefer counts, paths, IDs, and short field values over dumping full JSON.

## Version Selection

Determine the target Vintage Story version in this order:

1. Use the version explicitly named by the user.
2. Use the version implied by a file path or folder currently being discussed, such as `versions/1.22.3`.
3. If there is only one `versions/<version>` folder, use it.
4. If multiple version folders exist and the user did not specify one, ask which version to use.

## Index Artifacts

Use these stable paths for each version:

- SQLite index: `versions/<game-version>/Index/assets.sqlite`
- Manifest: `versions/<game-version>/Index/assets.index.json`
- Cross-project usage note: `INDEX_USAGE.md` at the repository root
- Build/query tools: `tools/asset-index/`

The skill is project-local, but these artifacts are intended to be consumed by other projects by absolute path.

## Query Workflow

1. Resolve the target game version.
2. Check for `versions/<game-version>/Index/assets.sqlite` and `assets.index.json`.
3. If the index exists, inspect the manifest for schema version, source asset root, indexed file counts, and stale status.
4. Query SQLite first, limiting results to the smallest useful set.
5. Open only the source files identified by the query when exact JSON content is needed.
6. If the index is missing or stale, ask whether to build or refresh it unless the user already requested indexing.

When a top-level `class` or `entityClass` value must be traced into C#, prefer the unified wrapper's `class` or `xref`
target. It performs a category-aware join against literal code-registration evidence and returns both asset and code
paths. An empty result is not proof that the runtime relationship does not exist.

## Build Workflow

When the user asks to build or refresh the index:

1. Reuse `tools/asset-index/build_asset_index.py` and `tools/asset-index/query_asset_index.py` if they exist.
2. Place new or revised implementation scripts under `tools/asset-index/`, not inside the skill folder.
3. Stream files one at a time; do not load the full asset tree into memory.
4. Write to a temporary SQLite file beside the final index, then replace the final index only after validation succeeds.
5. Create or update `versions/<game-version>/Index/assets.index.json`.
6. Create or update root `INDEX_USAGE.md` so other projects know to query the index instead of scanning raw assets.
7. Validate with representative queries before reporting completion.

## Minimum Schema

Use SQLite. Include these concepts even if table names evolve:

- `assets`: one row per asset file with path, version, domain, category, extension, size, modified time, and content hash when cheap.
- `json_assets`: parsed JSON facts such as `code`, `class`, `entityClass`, variant groups, shape references, texture references, recipe outputs, recipe ingredients, and patch targets.
- `asset_text`: SQLite FTS table for compact searchable fields, not full file dumps.
- `index_meta`: schema version, generated time, source asset root, source counts, and tool version.

Index domains from the first asset directory level, usually `game`, `survival`, or `creative`.

Index categories from common Vintage Story asset folders such as `blocktypes`, `itemtypes`, `entities`, `recipes`, `patches`, `lang`, `shapes`, `textures`, `sounds`, `config`, and `worldproperties`.

## Binary Assets

For PNG, OGG, TTF, SVG, shader, OBJ, and GLTF files:

- Store path, extension, size, modified time, and category.
- Store image dimensions for PNG when cheap and dependency-free.
- Store references to binaries found inside JSON files.
- Do not OCR, transcribe, embed, summarize, or base64-encode binary contents.

## Implementation Notes

- Vintage Story asset JSON often uses JSON5-like syntax, especially unquoted object keys such as `code: "..."`, trailing commas, and comments. Do not use strict `json.loads` directly on raw files; normalize or parse leniently.
- On Windows, explicitly close every SQLite connection before replacing `assets.sqlite.tmp` with `assets.sqlite`. Python's sqlite context manager commits but does not close the handle.
- Run Python scripts with `python -B` when possible to avoid creating `__pycache__` in `tools/asset-index`.
- If `assets.sqlite` already exists and the manifest is current, query it instead of rebuilding.
- The current build script stores parse-error rows in `assets` instead of dropping them, so path/category lookup still works even when a JSON file cannot be parsed into facts.

## Staleness

Consider the index stale if:

- `assets.index.json` is missing.
- The manifest schema version differs from the current tool schema.
- The asset root path differs.
- Source file counts or cheap directory signatures differ.
- Any indexed source file path no longer exists.

Use cheap checks first. Avoid hashing the entire asset tree unless the user requests a stronger verification.

## Cross-Project Usage Note

Keep root `INDEX_USAGE.md` concise and stable. It should tell other projects:

- where the current version index lives;
- where the manifest lives;
- to query SQLite before scanning `versions/*/Assets`;
- to read raw files only after the index narrows candidates;
- that `NOTES.md` is personal and not metadata.

## Validation

Before reporting that an index is ready, verify:

- The SQLite file opens.
- `index_meta` exists and reports the expected version.
- Counts for total files and JSON files approximately match the asset tree.
- A path lookup query returns known files.
- A text search query returns plausible candidates.
- A recipe/block/item/patch query works if those categories exist.
- A representative class cross-reference works when the matching code index has current registration data.
