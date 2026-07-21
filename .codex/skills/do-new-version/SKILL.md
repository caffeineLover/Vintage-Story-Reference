---
name: do-new-version
description: Project-local workflow for adding a newly released Vintage Story game version to this reference repository by creating a version project, populating versions/VERSION/API and versions/VERSION/Assets, decompiling VintagestoryAPI, VintagestoryLib, and VSSurvivalMod, downloading/decompiling current compatible reference mods, copying/indexing previous-version web documentation sources, and building the code, asset, and web indexes. Use when the user says "do new version", asks to add or populate a new Vintage Story version, or asks to update indexed API, asset, mod-reference, and web-reference sources for a new release.
---

# Do New Version

## Scope

Use this skill only inside the Vintage Story Reference repository. It creates the initial reference payload for a newly released Vintage Story version by creating a browse-only version project, decompiling the official release DLLs, copying the full release assets, updating selected reference mods, copying curated web documentation sources, and building the version's code, asset, and web indexes.

This skill includes selected reference mod updates by invoking `new-version-mods` and curated web documentation updates by invoking `do-new-web`. It should update the code, asset, and web indexes plus the catalogs needed to make those indexes discoverable.

## Hard Rules

- Determine the exact Vintage Story version from the user request before creating files.
- Verify the current official release/download details before downloading a package.
- Do not overwrite an existing `versions/<version>/API` or `versions/<version>/Assets` payload without explicit user approval.
- Stage downloaded archives, extracted game files, and temporary tools outside the kept reference tree, such as under a temp directory or `.codex/tmp/new-version/<version>/`.
- Create `versions/<version>/VintageStory.Reference.<version>.csproj` and add it to `Vintage Story Reference.sln` under a matching `<version>` solution folder.
- Keep the version wrapper project browse-only: disable default compile/resource items and include `version.catalog.json`, `API`, `Assets`, `mods`, and `Web` as `None` items.
- Prefer the Windows dedicated server ZIP for decompilation inputs because it is smaller and contains `VintagestoryAPI.dll`, `VintagestoryLib.dll`, and `Mods/VSSurvivalMod.dll`.
- Prefer the full Linux client archive for copying `Assets` because it extracts cleanly and includes the full client asset payload. Do not treat the server package's reduced `assets` folder as the final full asset copy.
- Do not keep decompiler-generated `.sln` or `.slnx` files. Prefer avoiding solution generation; delete solution files if the decompiler creates them.
- Keep generated `.csproj` files only as lightweight reference or IDE metadata, and scrub temporary or machine-specific `<HintPath>` values before keeping them.
- Flatten decompiled API output so the assembly wrapper contains source directly. Do not keep a single same-name child folder such as `VintagestoryAPI_decompiled_<version>/VintagestoryAPI`.
- Use `.codex/skills/new-version-mods/SKILL.md` to download current compatible selected reference mods, normalize mod folder names, keep one latest compatible folder per mod id, and decompile managed .NET DLLs before the final index builds.
- Use `.codex/skills/do-new-web/SKILL.md` to copy previous-version curated web sources, add `versions/<version>/Web/sources.json` to Git, and build the target web index.
- Build or refresh `versions/<version>/Index/code.sqlite` and `versions/<version>/Index/code.index.json` after API decompilation and mod decompilation.
- Build or refresh `versions/<version>/Index/assets.sqlite` and `versions/<version>/Index/assets.index.json` after copying assets and adding mod assets.
- Build or refresh `versions/<version>/Index/web.sqlite` and `versions/<version>/Index/web.index.json` after copying `versions/<version>/Web/sources.json`.
- Track `versions/<version>/Index/.gitignore` so the `Index` folder exists in Git, but do not stage or commit index SQLite databases or generated index manifests unless the user explicitly asks and a large-file strategy such as Git LFS has been confirmed.
- Do not report a new version as ready until representative code-index, asset-index, and web-index queries succeed.
- Update `docs/PROJECT_MEMORY.md` before finishing the work session.

## Expected Layout

For version `<version>`, populate:

- `versions/<version>/API/VintagestoryAPI_decompiled_<version>`
- `versions/<version>/API/VintagestoryLib_decompiled_<version>`
- `versions/<version>/API/VSSurvivalMod_decompiled_<version>`
- `versions/<version>/Assets`
- `versions/<version>/mods`
- `versions/<version>/Web`
- `versions/<version>/VintageStory.Reference.<version>.csproj`

Each API wrapper folder should contain the matching `.csproj`, namespace folders, and root `.cs` files directly. It should not contain `.sln`, `.slnx`, or a single same-name child project folder.

The `Assets` folder should contain the release asset domains directly, normally including `creative`, `game`, and `survival`.

The version wrapper `.csproj` should include the reference payload as browseable `None` items, including `Web`, and should not compile decompiled game sources.

Build locally under the tracked `Index` folder and keep out of Git by default:

- `versions/<version>/Index/code.sqlite`
- `versions/<version>/Index/code.index.json`
- `versions/<version>/Index/assets.sqlite`
- `versions/<version>/Index/assets.index.json`
- `versions/<version>/Index/web.sqlite`
- `versions/<version>/Index/web.index.json`

## Workflow

1. Read the project-local instructions required by `AGENTS.md`, including project memory and project standards. Read coding standards if editing code or tooling.
2. Inspect `git status` and the target `versions/<version>` tree so existing user/reference material is not overwritten.
3. Create `versions/<version>`, `versions/<version>/API`, `versions/<version>/Assets`, `versions/<version>/mods`, `versions/<version>/Web`, and `versions/<version>/Index` if they do not already exist.
4. Create a browse-only `versions/<version>/VintageStory.Reference.<version>.csproj` and add it to `Vintage Story Reference.sln` under a `<version>` solution folder.
5. Locate the official release packages. For stable releases, first try these CDN patterns, then verify the current release page or account/download metadata:
   - DLL package: `https://cdn.vintagestory.at/gamefiles/stable/vs_server_win-x64_<version>.zip`
   - Full assets package: `https://cdn.vintagestory.at/gamefiles/stable/vs_client_linux-x64_<version>.tar.gz`
6. Verify download checksums when checksums are available.
7. Extract the DLL package into the staging area and locate:
   - `VintagestoryAPI.dll`
   - `VintagestoryLib.dll`
   - `Mods/VSSurvivalMod.dll`
8. Extract the full client package into the staging area and locate its `assets` folder. Copy that folder's contents into `versions/<version>/Assets`.
9. Install or locate `ilspycmd` for decompilation. A temporary local tool install is acceptable.
10. Decompile each DLL into the flattened API wrapper. A known-good command shape is:

```powershell
ilspycmd --disable-updatecheck --nested-directories --use-varnames-from-pdb -p `
  -o <api-wrapper> `
  -r <extracted-server-root> `
  -r <extracted-server-root>\Lib `
  -r <extracted-server-root>\Mods `
  <path-to-dll>
```

11. If `ilspycmd` creates a same-name child folder inside a wrapper, merge that child folder's contents up into the wrapper and remove the child folder. Check for name collisions before moving files.
12. Remove any `.sln` or `.slnx` files produced by the decompiler before keeping the output.
13. Remove `<HintPath>` entries that point to temp directories, local package extraction folders, or other machine-specific paths.
14. Validate the copied and decompiled source result:
    - Each of the three expected flattened API wrapper folders exists.
    - Each folder contains substantial `.cs` source output.
    - No API wrapper contains a same-name child folder.
    - No `.sln` or `.slnx` files exist under `versions/<version>/API`.
    - No kept `.csproj` file contains temp extraction paths.
    - `versions/<version>/Assets` contains the full asset domains directly.
15. Read and follow `.codex/skills/new-version-mods/SKILL.md` to download the newest compatible selected reference mods for the target game version, place exactly one normalized `<modid>_<modversion>` folder per mod id under `versions/<version>/mods`, and decompile managed .NET DLLs when present.
16. Read and follow `.codex/skills/do-new-web/SKILL.md` to copy the previous version's curated web sources into `versions/<version>/Web/sources.json`, add that source manifest to Git, and build the web index.
17. Build the code and asset indexes:

```powershell
python -B tools\code-index\build_code_index.py --version <version>
python -B tools\asset-index\build_asset_index.py --version <version>
```

18. Create or update `versions/<version>/version.catalog.json` so the `code`, `asset`, and `web` indexes point at the new SQLite databases, manifests, source manifests, build scripts, and query scripts. Add the new version to `reference.catalog.json`. Do not mark the new version as the default unless the user asks.
19. Validate the solution and indexes with representative queries. Prefer the unified wrapper after catalogs are updated:

```powershell
dotnet sln "Vintage Story Reference.sln" list
python -B tools\reference-query.py --version <version> --list-indexes
python -B tools\reference-query.py --version <version> asset --search copper --limit 5
python -B tools\reference-query.py --version <version> asset --category recipes --limit 5
python -B tools\reference-query.py --version <version> code --type ICoreAPI --limit 5
python -B tools\reference-query.py --version <version> code --member StartServer --limit 5
python -B tools\reference-query.py --version <version> code --search BlockEntity --limit 5
python -B tools\reference-query.py --version <version> web --list-sources
python -B tools\reference-query.py --version <version> web --search "configlib-patches json" --limit 5
```

20. Update `docs/PROJECT_MEMORY.md` with the version, source packages, checksums if verified, decompiler/tool version, kept output layout, wrapper project/solution entry, downloaded/decompiled mod updates, copied web sources, code-index counts, asset-index counts, web-index counts, catalog updates, and anything deliberately not updated.

## Reporting

When finishing, report the populated API, Assets, mods, and Web paths, the three assemblies decompiled, whether checksum verification was available, the selected mods downloaded or skipped, the copied web source count, the code-index, asset-index, and web-index count summaries, the wrapper project added to the solution, and which catalogs were updated.
