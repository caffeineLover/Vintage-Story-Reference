---
name: new-version-mods
description: Project-local workflow for populating or updating reference mod folders in the Vintage Story Reference repository. Use when do-new-version needs current compatible mods for a new Vintage Story version, or when the user explicitly asks Codex to check, download, stage, extract, or decompile mods under versions/VERSION/mods in this repo.
---

# New Version Mods

## Scope

Use this skill only inside the Vintage Story Reference repository. The repository root is expected to contain `versions/`, and mod reference folders live under `versions/<game-version>/mods`.

Use this workflow as part of `do-new-version` when creating a new Vintage Story version, after the version folders exist and before the final asset/code indexes are built. Also use it independently when the user explicitly asks to update, check, download, stage, extract, or decompile reference mods.

## Hard Rules

- Do not delete anything unless the user explicitly asks for deletion in the same request.
- Do not overwrite or replace existing mod folders.
- Do not remove old mod versions, downloaded archives, extracted staging folders, generated files, or failed attempts.
- Create `versions/<game-version>/mods` if it does not already exist.
- For new target versions, keep exactly one completed folder per mod id under `versions/<game-version>/mods`.
- Normalize completed mod folder names across versions as much as possible, using `<modid>_<modversion>` from `modinfo.json` or Mod DB metadata. Use lowercase mod ids and filesystem-safe version strings.
- For seeded mods, normalize from the previous version's folder name into the canonical `<modid>_<modversion>` style instead of preserving display-name folder styles.
- If the target version already contains duplicate folders for the same mod id, report the duplicates. Remove or replace duplicate existing folders only when the user explicitly asks for cleanup in the same request.
- If a canonical destination folder already exists for the selected latest compatible release, reuse it as the target for validation instead of creating a duplicate.
- Always decompile managed .NET DLLs found in updated code mods before calling the update complete.
- For a new Vintage Story version, use the previous version's selected mod folders as the seed list unless the user provides a different list.
- Prefer releases explicitly tagged for the exact target game version.
- For Vintage Story patch-version targets, if a seeded mod has no exact target tag but has a current release tagged for the same major/minor line, such as `1.22.x`, keep that release as a same-line fallback and record that Mod DB did not claim exact patch compatibility.
- Do not silently omit a seeded mod. If no exact or same-line compatible current release is found for the target game version, report it as skipped with the evidence checked.
- Treat `NOTES.md` as personal user notes, not authoritative metadata.
- Prefer `README.md`, project files, `modinfo.json`, and folder names for reference metadata.

## Version Selection

Determine the target Vintage Story version in this order:

1. Use the version explicitly named by the user.
2. Use the version implied by a file path or folder currently being discussed, such as `versions/1.22.3`.
3. If there is only one `versions/<version>` folder, use it.
4. If multiple version folders exist and the user did not specify one, ask which version to update.

## Workflow

1. Create `versions/<game-version>/mods` if it does not already exist.
2. Inspect the target `versions/<game-version>/mods` folder and identify any local mods from folder names and `modinfo.json` when present. Group findings by mod id so duplicates are visible.
3. If invoked from `do-new-version` and the target mod folder is empty, inspect the nearest previous `versions/<previous-version>/mods` folder and use those selected reference mods as the seed list.
4. Check the Vintage Story Mod DB for the newest releases compatible with the target game version. Start at `https://mods.vintagestory.at/list/mod`, then use individual mod pages or search results as needed.
5. Compare local or seeded mod versions with available releases compatible with the target game version. For each mod id, select only the newest compatible release. Prefer exact target-version releases; for patch-version targets, allow the newest same major/minor release as a documented fallback when no exact tag exists.
6. Report available updates before downloading unless the user already asked to download updates or this workflow is running as part of `do-new-version`.
7. Download each selected update to a staging path under `.codex/tmp/mod-updates/<game-version>/`.
8. Extract the archive into a staging subfolder. Preserve the archive and extracted staging files.
9. Inspect the extracted mod for `modinfo.json`, assets, DLLs, and nested archive structure. If the archive contains a single top-level mod folder, preserve that folder layout.
10. If DLLs are present, decompile managed .NET DLLs before placing the update in `versions/<game-version>/mods`. Prefer decompiling mod-owned DLLs first, but do not skip bundled managed dependency DLLs unless the user explicitly limits scope. Detect and report native/non-.NET DLLs instead of treating them as C# decompilation failures.
11. Place decompiled output in a clearly named folder inside the updated mod folder, such as `decompiled assembly`, `decompiled assemblies`, or `<dll-name> - decompiled`. Do not overwrite an existing decompiled output folder; choose a unique suffix if needed.
12. Place the completed update in `versions/<game-version>/mods/<modid>_<modversion>`, using the canonical mod id and selected release version. Do not create a second folder for the same mod id in the target version.
13. Summarize seeded version if any, old version, new version, exact or same-line compatibility status, source URL, destination folder, staged archive path, extraction path, DLLs decompiled, decompiled output paths, and any seeded mods skipped because no compatible target-version or same-line release was found.

## Decompilation

Always decompile managed .NET DLLs found in updated code mods. Prefer the 64-bit dotNetSpy/dnSpy path if available:

- `E:\home\Bin\dotNetSpy 64bit`
- `E:\home\Bin\dotNetSpy 32bit`

Do not spend time retrying `dnSpy.Console.exe` headlessly if it fails with `System.IO.IOException: The handle is invalid`; this dotNetSpy console build is known to crash in the Codex/Rider non-interactive shell while setting console encoding.

If `.codex/tmp/decompiler/VsReferenceDecompiler.csproj` exists, prefer it for automated decompilation. It uses the dotNetSpy-bundled `ICSharpCode.Decompiler`/`dnlib` libraries and writes one folder per managed assembly. It should skip native/non-.NET DLLs with a clear message.

If no CLI-capable path works, stop after extraction and report that the update is not complete because decompilation needs manual handling or another CLI-capable decompiler.

## Validation

Before saying an update is ready, verify:

- The target game version is correct.
- The target `versions/<game-version>/mods` folder exists.
- The source page or release claims exact compatibility with that game version, or the release is explicitly recorded as a same major/minor fallback for a patch-version target.
- The extracted folder has a recognizable Vintage Story mod structure, preferably including `modinfo.json`.
- Managed .NET DLLs in updated code mods were decompiled, and native/non-.NET DLLs were reported as skipped, or the update is reported incomplete because decompilation could not be automated.
- The target version has only one completed folder per mod id.
- The destination folder uses normalized `<modid>_<modversion>` naming unless a user explicitly requested a different name for that version.
- Existing mod folders remain untouched.

If any check fails, leave all files in staging, do not copy into `versions/<game-version>/mods`, and report the issue.
