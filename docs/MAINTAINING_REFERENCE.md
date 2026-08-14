# Maintaining The Vintage Story Reference Archive

This document records the maintainer workflows for adding or refreshing reference material. Other projects should normally consume this archive through indexes and should not modify it directly.

## Before Changing The Archive

1. Read `AGENTS.md`.
2. Read `docs/PROJECT_MEMORY.md`.
3. Check `git status --short` and preserve unrelated user changes.
4. Identify the target Vintage Story version.
5. Prefer project-local skills under `.codex/skills/` for asset indexing, code indexing, new-version mod population, web source indexing, and mod updates.

## Adding A New Vintage Story Version

Use `.codex/skills/do-new-version/SKILL.md` for the standard new-version workflow. It creates the browse-only version wrapper project, populates API and assets, invokes `.codex/skills/new-version-mods/SKILL.md` for selected reference mods, invokes `.codex/skills/do-new-web/SKILL.md` for curated web sources, and then builds the final indexes.

1. Create `versions/<version>/`.
2. Add `versions/<version>/VintageStory.Reference.<version>.csproj` to `Vintage Story Reference.sln` under a matching version solution folder.
3. Add the raw reference payload under the expected version layout:
   - `API/`
   - `Assets/`
   - `mods/`
   - `Web/`
   - `Index/`
4. For the built-in API payload, decompile `VintagestoryAPI.dll`, `VintagestoryLib.dll`, `Mods/VSCreativeMod.dll`, `Mods/VSEssentials.dll`, and `Mods/VSSurvivalMod.dll` into flattened `API/<Assembly>_decompiled_<version>` folders. Do not keep `.sln` or `.slnx` files.
5. Copy the full client `assets` contents into `Assets/`; do not use a reduced server-only asset folder as the final full asset payload.
6. Download the newest compatible selected reference mods for the target version, normalize mod folders as
   `<modid>_<modversion>`, keep one latest compatible folder per mod id, decompile managed .NET DLLs when present, and
   create `versions/<version>/mods.manifest.json` with tracked compatibility and provenance metadata.
7. Copy or merge curated web source entries from the nearest previous version with `Web/sources.json`.
8. Add `versions/<version>/version.catalog.json`.
9. Add the version to `reference.catalog.json`.
10. Build asset and code indexes for the new version after API, assets, and selected mods are in place.
11. Build the web index for the new version after `versions/<version>/Web/sources.json` exists.
12. Keep `versions/<version>/Index/.gitignore` tracked so the `Index` folder exists in Git, but leave generated index files uncommitted by default; they are local rebuildable artifacts and may exceed normal GitHub file limits.
13. Update `INDEX_USAGE.md` with the current version, paths, counts, and examples.
14. Update `docs/PROJECT_MEMORY.md`.

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
python -B tools\reference-query.py --version <version> code --registration BlockDisplayCase --limit 20
python -B tools\reference-query.py --version <version> class --asset-class BlockDisplayCase --limit 20
```

The code builder also extracts supported calls with a literal registration key and `typeof(...)` value. The class
cross-reference joins those calls to top-level asset `class` and `entityClass` facts. It remains conservative rather
than inferring dynamic keys, helper-wrapped calls, factories, reflection, JSON inheritance, or runtime patches. For
exact behavior, open the returned source and asset files.

## Adding Or Updating Mods

Use `.codex/skills/new-version-mods/SKILL.md` when downloading, staging, extracting, or decompiling mods under `versions/<version>/mods`.

For new Vintage Story version folders, normalize mod folder names as `<modid>_<modversion>` using `modinfo.json` or Mod DB metadata. Each version should contain only one completed folder per mod id: the latest release compatible with that game version, preferring an exact game-version tag and using a documented same-line fallback only when no exact patch release exists. If an existing version already has duplicates, report them and remove duplicates only when cleanup was explicitly requested.

After adding or updating a mod:

1. Add or update its entry in `versions/<version>/mods.manifest.json`. Record the mod id, version, folder, package
   type, exact/fallback compatibility evidence, source URLs, retained asset/code/native-binary availability, and
   whether an explicit license file is present.
2. Verify the manifest has exactly one entry for every completed mod/tool folder and no entry pointing at a missing
   folder.
3. Rebuild the asset index if the mod contains assets.
4. Rebuild the code index if the mod contains decompiled C# source or a project-style source folder with a `.csproj`.
5. Update `versions/<version>/version.catalog.json` if the change affects documented roots, manifests, or summary
   counts.
6. Verify the manifest through `tools/reference-query.py`:

```powershell
python -B tools\reference-query.py --version <version> mod --mod-id <modid> --format json
```

7. Update `INDEX_USAGE.md` if the list of indexed roots or consumer guidance changed.
8. Update `docs/PROJECT_MEMORY.md` with durable notes, failed attempts, or risks.

## Adding A Website

Use `.codex/skills/do-new-web/SKILL.md` when copying, merging, refreshing, or indexing curated external web sources for a Vintage Story version.

1. Add a source entry to `versions/<version>/Web/sources.json`.
2. Prefer `source_type` values already supported by `tools/web-index/build_web_index.py`.
3. For Mod DB pages, use an appropriate `html_scope` so navigation, file tables, and unrelated site chrome are not indexed as documentation.
4. Rebuild the web index as a local generated artifact:

```powershell
python -B tools\web-index\build_web_index.py --version <version>
```

5. Verify with `tools/reference-query.py`, for example:

```powershell
python -B tools\reference-query.py --version <version> web --list-sources
python -B tools\reference-query.py --version <version> web --search "search terms" --limit 20
```

6. Update `INDEX_USAGE.md`, `versions/<version>/version.catalog.json`, and `docs/PROJECT_MEMORY.md` when sources, counts, or assumptions change.
7. Do not stage generated `versions/<version>/Index` files other than the folder's `.gitignore` unless the user explicitly requests generated-index versioning and the repository has a suitable large-file policy.

## Adding Research Documents

Store completed research under `docs/research/<category>/<topic>/`. Read `docs/research/README.md` before creating or
updating a research document.

Every completed research item should have three same-basename files. Markdown preserves the canonical findings and
evidence for agents. Pandoc generates committed LaTeX source using `tools/research-docs/pandoc-pdf.yaml`. XeLaTeX
compiles the polished user-facing PDF, which remains local and ignored by Git. The generated LaTeX source is an
explicit, user-requested exception to the ordinary rule against tracking generated files.

When the Markdown changes:

1. Verify factual claims and source paths.
2. Regenerate the matching LaTeX source from the Markdown; never edit generated LaTeX directly.
3. Compile the PDF from the generated LaTeX source.
4. Review the PDF for clipped tables, awkward page breaks, unreadable code, and missing characters.
5. Stage the Markdown and LaTeX source together. Do not stage the PDF.

If Pandoc or `xelatex` is unavailable, keep the Markdown work and report the missing dependency. Do not silently
substitute an HTML/browser PDF renderer when the requested deliverable is LaTeX-based.

## Updating Catalogs

Update `reference.catalog.json` when:

- a new version is added;
- the default version changes;
- a top-level prompt, skill, wrapper, or documentation path changes.
- the research workflow entry point changes.

Update `versions/<version>/version.catalog.json` when:

- an index path changes;
- a build or query script path changes;
- source or mod manifests change;
- summary counts change enough to make the catalog misleading.

Catalogs are discovery aids. Do not copy large generated manifest details into them.

## Updating Consumer Documentation

Update `INDEX_USAGE.md` when:

- the current/default indexed version changes;
- index paths or helper commands change;
- available indexes change;
- available tracked manifests change;
- important indexed roots or web documentation sources change;
- query examples should be revised.

Update `prompts/Vintage Story Research/VINTAGE_STORY_RESEARCH.md` only when the stable research behavior changes. Keep it version-agnostic.

Update `prompts/Vintage Story Research/Add to AGENTS.md` only when the short trigger or pointer text changes. Do not put version-specific index paths there.

## Verification Checklist

Run the relevant checks before finishing:

```powershell
python -B tools\reference-query.py --list-versions
python -B tools\reference-query.py --version <version> --list-indexes
python -B tools\reference-query.py --version <version> --list-cross-references
python -B tools\reference-query.py --version <version> --list-manifests
python -B tools\reference-query.py --version <version> mod --limit 5
python -B tools\reference-query.py --version <version> asset --search copper --limit 5
python -B tools\reference-query.py --version <version> code --type ICoreAPI --limit 5
python -B tools\reference-query.py --version <version> code --registration BlockDisplayCase --limit 5
python -B tools\reference-query.py --version <version> class --asset-class BlockDisplayCase --limit 5
python -B tools\reference-query.py --version <version> web --list-sources
git diff --check
```

Also check `git status --short` and leave unrelated user changes untouched.
