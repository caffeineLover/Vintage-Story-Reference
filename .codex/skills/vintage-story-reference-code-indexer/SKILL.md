---
name: vintage-story-reference-code-indexer
description: Project-local workflow for building, refreshing, and using the Vintage Story Reference decompiled C# code index. Use when answering questions about VintagestoryAPI, VintagestoryLib, VSCreativeMod, VSEssentials, VSSurvivalMod, decompiled mods, symbols, types, methods, inheritance, references, or when the user asks to build or update the code index.
---

# Vintage Story Reference Code Indexer

## Scope

Use this skill only inside the Vintage Story Reference repository. The repository root is expected to contain `versions/`, and decompiled C# reference code lives under `versions/<game-version>/API` and selected mod folders under `versions/<game-version>/mods`.

The goal is to save memory, time, and AI credits by querying a compact local SQLite symbol index before opening decompiled `.cs` files.

## Hard Rules

- Query the code index before recursively scanning or reading decompiled C# trees.
- Read source files only after the index narrows the candidate set.
- Do not store full source files in the index. Store symbols, signatures, paths, line numbers, and compact searchable text.
- Do not store indexes under `.codex/tmp`; use stable versioned index artifacts under `versions/<game-version>/Index`.
- Treat `NOTES.md` as personal user notes, not authoritative metadata.
- Treat literal registration rows as strong navigation evidence, not an exhaustive runtime registry. Dynamic keys,
  helper wrappers, factories, reflection, and patches may not be represented.
- Do not treat the index as the source of truth when exact implementation matters; use it to find candidate files and line numbers, then open the specific source files.
- Keep query result sets small. Prefer symbol names, paths, line numbers, and short signatures over dumping source.

## Version Selection

Determine the target Vintage Story version in this order:

1. Use the version explicitly named by the user.
2. Use the version implied by a file path or folder currently being discussed, such as `versions/1.22.3`.
3. Use `reference.catalog.json` `default_game_version` when no stronger target-version signal exists.
4. If the catalog has no valid default and there is only one `versions/<version>` folder, use it; otherwise ask which
   version to use.

## Index Artifacts

Use these stable paths for each version:

- SQLite index: `versions/<game-version>/Index/code.sqlite`
- Manifest: `versions/<game-version>/Index/code.index.json`
- Cross-project usage note: root `INDEX_USAGE.md`, updated to mention the code index when built
- Build/query tools: `tools/code-index/`

The skill is project-local, but these artifacts are intended to be consumed by other projects by absolute path.

## Query Workflow

1. Resolve the target game version.
2. Check for `versions/<game-version>/Index/code.sqlite` and `code.index.json`.
3. If the index exists, inspect the manifest for schema version, source roots, indexed file counts, and stale status.
4. Query SQLite first for symbols, signatures, inheritance, literal registrations, or references.
5. Open only the source files and line ranges identified by the query when exact implementation details are needed.
6. If the index is missing or stale, ask whether to build or refresh it unless the user already requested indexing.

## Build Workflow

When the user asks to build or refresh the code index:

1. Place implementation scripts under `tools/code-index/`, not inside the skill folder.
2. Stream and parse files one at a time; do not load the whole code tree into memory.
3. Prefer a real C# parser or Roslyn when available. If using text extraction initially, keep it conservative and record parser limitations in the manifest.
4. Write to a temporary SQLite file beside the final index, then replace the final index only after validation succeeds.
5. Create or update `versions/<game-version>/Index/code.index.json`.
6. Update root `INDEX_USAGE.md` so other projects know to query the code index before scanning decompiled code.
7. Validate with representative symbol, member, inheritance, and text-search queries before reporting completion.

## Implementation Notes

- If `tools/code-index/build_code_index.py` and `query_code_index.py` already exist, reuse them instead of rewriting the indexer.
- Run Python index/query commands with `python -B` so validation does not create `__pycache__` folders. If a cache folder appears under `tools/code-index`, remove it after verifying the path is inside the repository.
- For conservative text parsing, start member extraction at the actual type opening brace. Starting before the brace can produce an index with valid type counts but zero members.
- Discover mod source roots recursively. Decompiled mod code may live under names like `decompiled assembly`, `decompiled assemblies`, or package-specific `*-decompiled` folders.
- Use the generated manifest's source-root and file counts for sanity checks; do not carry historical version counts
  forward as thresholds for newer payloads.

## Source Roots

Index the built-in decompiled API trees:

- `versions/<game-version>/API/VintagestoryAPI_decompiled_<game-version>`
- `versions/<game-version>/API/VintagestoryLib_decompiled_<game-version>`
- `versions/<game-version>/API/VSCreativeMod_decompiled_<game-version>`
- `versions/<game-version>/API/VSEssentials_decompiled_<game-version>`
- `versions/<game-version>/API/VSSurvivalMod_decompiled_<game-version>`

These API wrapper folders should be flattened: source files, namespace folders, and the `.csproj` should live directly under each wrapper rather than under a single same-name child folder.

Also index decompiled mod folders under `versions/<game-version>/mods` when they contain `.cs` files or folders named like `decompiled assembly`, `decompiled assemblies`, or `*-decompiled`.

## Minimum Schema

Use SQLite. Include these concepts even if table names evolve:

- `source_files`: file path, source root, assembly/tree name, namespace hint, size, modified time, and cheap content hash when useful.
- `types`: full name, namespace, name, kind, accessibility, source file, start line, end line, base type, implemented interfaces, attributes.
- `members`: containing type, member kind, name, signature, return type, accessibility, static/abstract/virtual/override flags, source file, start line, end line, attributes.
- `references`: conservative symbol references from member bodies when feasible, with source file and line.
- `registrations`: supported calls with a literal registration key and `typeof(...)` value, their method/kind, call
  site, evidence text, and an optional unambiguous link to the registered type declaration.
- `code_text`: SQLite FTS table over compact fields such as type names, member names, full names, signatures, attributes, base types, interface names, and selected comments.
- `index_meta`: schema version, generated time, source roots, file counts, symbol counts, parser mode, and tool version.

Index declaration metadata first. Add reference/call graph data only if it can be produced reliably without excessive cost.

## Query Patterns

Use the index for:

- finding a type by name or namespace;
- finding methods or properties on an interface/class;
- finding implementations or subclasses;
- finding source files for a symbol;
- finding classes with a base type, interface, attribute, or method name;
- finding likely code locations before opening source;
- comparing API, library, survival mod, and decompiled mod symbols.
- querying conservative `references_hint` tokens with `--reference` when a bounded identifier hint can narrow a
  candidate set.
- querying registration keys, kinds, methods, registered types, call sites, and resolved declarations with
  `--registration`, `--registration-kind`, or `--registration-method`.
- joining top-level asset `class` and `entityClass` facts through the wrapper's `class`/`xref` target.

Avoid broad recursive `rg` over all `.cs` files unless the index is missing, stale, or cannot answer the question.

## Staleness

Consider the index stale if:

- `code.index.json` is missing.
- The manifest schema version differs from the current tool schema.
- Source root paths differ.
- Source file counts or cheap directory signatures differ.
- Any indexed source file path no longer exists.

Use cheap checks first. Avoid hashing every source file unless the user requests a stronger verification.

## Cross-Project Usage Note

Keep root `INDEX_USAGE.md` concise and stable. It should tell other projects:

- where the current asset and code indexes live;
- where the manifests live;
- to query SQLite before scanning `versions/*/Assets` or decompiled `.cs` trees;
- to read raw files only after an index narrows candidates;
- that `NOTES.md` is personal and not metadata.

## Validation

Before reporting that a code index is ready, verify:

- `python -B -m unittest discover -v` finds and passes the code-index parser regressions.
- The SQLite file opens.
- `index_meta` exists and reports the expected version.
- Counts for source files and type declarations approximately match the source tree.
- A type-name query returns a known API type.
- An exact `--member StartServerSide --root VintagestoryAPI` query returns the ordinary Allman-style `ModSystem`
  method declaration.
- A full-text query returns plausible candidates.
- A literal registration query returns its call site and resolved type declaration.
- A representative `class` cross-reference returns both asset and C# evidence.
- Opening a reported file and line number reaches the expected declaration.
