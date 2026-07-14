# Maintaining The Vintage Story Reference Archive

This document records the maintainer workflows for adding or refreshing reference material. Other projects should normally consume this archive through indexes and should not modify it directly.

## Before Changing The Archive

1. Read `AGENTS.md`.
2. Read `docs/PROJECT_MEMORY.md`.
3. Check `git status --short` and preserve unrelated user changes.
4. Identify the target Vintage Story version.
5. Prefer project-local skills under `.codex/skills/` for asset indexing, code indexing, and mod updates.

## Adding A New Vintage Story Version

1. Create `versions/<version>/`.
2. Add the raw reference payload under the expected version layout:
   - `API/`
   - `Assets/`
   - `mods/`
   - `Web/`
   - `Index/`
3. Add `versions/<version>/version.catalog.json`.
4. Add the version to `reference.catalog.json`.
5. Build asset and code indexes for the new version.
6. Build the web index if `versions/<version>/Web/sources.json` exists.
7. Update `INDEX_USAGE.md` with the current version, paths, counts, and examples.
8. Update `docs/PROJECT_MEMORY.md`.

## Refreshing Asset Indexes

Use the project-local asset index workflow or run:

```powershell
python -B tools\asset-index\build_asset_index.py --version <version>
```

Verify with targeted queries, for example:

```powershell
python -B tools\reference-query.py --version <version> asset --search copper --limit 20
python -B tools\reference-query.py --version <version> asset --category recipes --limit 20
```

After refreshing, inspect `versions/<version>/Index/assets.index.json` and update catalogs or docs if counts, roots, or assumptions changed.

## Refreshing Code Indexes

Use the project-local code index workflow or run:

```powershell
python -B tools\code-index\build_code_index.py --version <version>
```

Verify with targeted queries, for example:

```powershell
python -B tools\reference-query.py --version <version> code --type ICoreAPI --limit 20
python -B tools\reference-query.py --version <version> code --member RegisterBlockClass --limit 20
```

The code index is conservative symbol metadata, not a compiler or full semantic model. For exact behavior, open the returned source file and inspect the relevant line range.

## Adding Or Updating Mods

Use `.codex/skills/vintage-story-reference-mod-updater/SKILL.md` when downloading, staging, extracting, or decompiling mods under `versions/<version>/mods`.

After adding or updating a mod:

1. Rebuild the asset index if the mod contains assets.
2. Rebuild the code index if the mod contains decompiled C# source or a project-style source folder with a `.csproj`.
3. Update `versions/<version>/version.catalog.json` if the change affects documented roots or summary counts.
4. Update `INDEX_USAGE.md` if the list of indexed roots or consumer guidance changed.
5. Update `docs/PROJECT_MEMORY.md` with durable notes, failed attempts, or risks.

## Adding A Website

1. Add a source entry to `versions/<version>/Web/sources.json`.
2. Prefer `source_type` values already supported by `tools/web-index/build_web_index.py`.
3. For Mod DB pages, use an appropriate `html_scope` so navigation, file tables, and unrelated site chrome are not indexed as documentation.
4. Rebuild the web index:

```powershell
python -B tools\web-index\build_web_index.py --version <version>
```

5. Verify with `tools/reference-query.py`, for example:

```powershell
python -B tools\reference-query.py --version <version> web --list-sources
python -B tools\reference-query.py --version <version> web --search "search terms" --limit 20
```

6. Update `INDEX_USAGE.md`, `versions/<version>/version.catalog.json`, and `docs/PROJECT_MEMORY.md` when sources, counts, or assumptions change.

## Updating Catalogs

Update `reference.catalog.json` when:

- a new version is added;
- the default version changes;
- a top-level prompt, skill, wrapper, or documentation path changes.

Update `versions/<version>/version.catalog.json` when:

- an index path changes;
- a build or query script path changes;
- source manifests change;
- summary counts change enough to make the catalog misleading.

Catalogs are discovery aids. Do not copy large generated manifest details into them.

## Updating Consumer Documentation

Update `INDEX_USAGE.md` when:

- the current/default indexed version changes;
- index paths or helper commands change;
- available indexes change;
- important indexed roots or web documentation sources change;
- query examples should be revised.

Update `prompts/Vintage Story Research/VINTAGE_STORY_RESEARCH.md` only when the stable research behavior changes. Keep it version-agnostic.

Update `prompts/Vintage Story Research/Add to AGENTS.md` only when the short trigger or pointer text changes. Do not put version-specific index paths there.

## Verification Checklist

Run the relevant checks before finishing:

```powershell
python -B tools\reference-query.py --list-versions
python -B tools\reference-query.py --version <version> --list-indexes
python -B tools\reference-query.py --version <version> asset --search copper --limit 5
python -B tools\reference-query.py --version <version> code --type ICoreAPI --limit 5
python -B tools\reference-query.py --version <version> web --list-sources
git diff --check
```

Also check `git status --short` and leave unrelated user changes untouched.
