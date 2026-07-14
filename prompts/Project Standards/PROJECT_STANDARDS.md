# Project Standards

These standards apply to every project unless the user gives explicit project-specific instructions that override them.

## Project Structure

Every project must contain:

- A root-level `AGENTS.md` file.
- A root-level `docs/` folder for documentation.
- A root-level `src/` folder for source code.

All documentation must be stored under `docs/`.

All source code files must be stored under `src/`. Strongly related source files should be grouped in a clearly named subfolder under `src/`.

Example:

```text
project-root/
├── AGENTS.md
├── docs/
│   └── RELEASES.md
└── src/
    ├── Commands/
    ├── Configuration/
    └── Systems/
```

## AGENTS.md

Every project must contain a root-level `AGENTS.md` file.

Project-specific instructions belong in that file. Shared agent skills should be referenced rather than duplicated.

## Git

Every project must be managed with Git.

If a project is not already a Git repository, initialize Git before making project changes.

Every project must contain an appropriate root-level `.gitignore`.

The repository should also contain:

- A root-level `.gitattributes` file to standardize line endings and identify binary files.
- A root-level `.editorconfig` file to standardize basic formatting across editors and coding agents.

Do not rewrite published Git history. Do not move or reuse published tags, and do not force-push shared branches unless the user explicitly instructs you to do so.

## Repository Reproducibility

A project must be reproducible from a clean clone.

Do not rely on:

- Untracked files.
- IDE state.
- Undocumented machine-specific paths.
- Undocumented files outside the repository.

Explicitly documented shared resources, such as the Vintage Story Reference project, are permitted.

Dependency manifests and lock files should normally be committed. Downloaded dependency caches should not be committed.

## Secrets and Local Configuration

Never commit secrets or private machine configuration.

This includes:

- API keys.
- Credentials.
- Access tokens.
- Signing keys.
- Personal paths.
- Private environment files.
- Machine-specific configuration.

When a configurable file is needed, provide a sanitized example such as `config.example.json`.

## Generated Files and Release Artifacts

Generated files must not be committed to Git.

The root-level `.gitignore` should exclude applicable generated content, including:

```text
bin/
obj/
dist/
release/
releases/
*.zip
```

Release ZIP files, compiled assemblies, packaged mods, logs, temporary files, and other generated artifacts must not be versioned with Git.

A binary or ZIP file may be committed only when it is an intentional source asset rather than generated output. Such an exception must be explicit and documented.

## Microsoft Word Documents

Projects may contain Microsoft Word documents (`.docx`) under `docs/`.

These files are maintained by the user. A coding agent must not modify, regenerate, reformat, rename, move, or delete them unless the user gives explicit instructions to do so.

A coding agent may read a `.docx` file when necessary for a task. Permission to read it does not imply permission to modify it.

## Third-Party Material

Do not casually copy third-party source code, libraries, binaries, assets, or decompiled files into a project repository.

Only add third-party material when it is genuinely required. Document its origin and licensing.

## Config Lib

Every Vintage Story mod should support Config Lib when Config Lib is available.

Config Lib integration must be optional. A mod must load and function correctly when Config Lib is not installed.

A missing Config Lib dependency must not:

- Prevent the mod from starting.
- Disable unrelated features.
- Produce avoidable errors.

Configuration must have a non-Config Lib fallback, such as direct configuration-file loading or sensible built-in defaults.

## Vintage Story Reference Project

Vintage Story mod projects may freely consult the shared **Vintage Story Reference** solution.

The Reference contains:

- Decompiled Vintage Story assemblies.
- Vintage Story assets.
- A selection of mods.
- Decompiled source for code mods.

The Reference is organized by Vintage Story game version, with one project for each supported game version. A mod must consult the Reference project matching the mod's target Vintage Story version.

Ordinary mod projects must not broadly ingest or analyze the entire Reference. The Reference project is responsible for being the comprehensive Vintage Story expert. Other projects should retrieve only the information needed for the current task.

Whenever practical, mod projects must use the Reference project's index to locate relevant files, symbols, assets, and examples before opening the underlying material.

Detailed instructions for locating and using the Reference index are still to be added.

## Releases

When the user writes **“do release,”** perform the complete release process.

Before beginning, determine:

- How the project version number should be increased.
- The Git tag name to create.

If the user has not supplied either value, ask for the missing value before making the release. Never infer the version increment or tag name.

A Git tag defines a release. The tag must identify the exact commit from which the release was built.

The version in project metadata must agree with the release tag.

Published release tags must never be moved, reused, or silently replaced. A correction to a published release requires a new version and a new tag.

### Release Verification

Before creating a release, verify that:

- The working tree is clean.
- The project builds successfully.
- Tests pass, when tests exist.
- The project version, Git tag, and `docs/RELEASES.md` entry agree.
- The release ZIP is built from the tagged commit.

### Release Procedure

The release process must:

1. Update the project version.
2. Add a new entry to `docs/RELEASES.md`.
3. Commit the release changes.
4. Create the specified Git tag.
5. Build the release ZIP from the tagged commit.
6. Create or prepare the corresponding release distribution.
7. Provide ready-to-paste changelog text for the Vintage Story Mod Database.
8. Open File Explorer to the folder containing the completed release ZIP.

Release artifacts may be attached to a GitHub Release or uploaded to the appropriate distribution platform, but they must not be committed to Git.

### Release Artifact Names

Use a consistent release filename containing the project name and exact version.

Example:

```text
ModName-1.2.0.zip
```

Do not use ambiguous names such as:

```text
latest.zip
final.zip
new.zip
```

## Release History

Every release must add a new entry to `docs/RELEASES.md`.

Release entries must:

- Be listed newest first.
- Use the release date in `YYYY-MM-DD` format.
- Identify both the version number and Git tag.
- Describe changes from the perspective of players or mod users.
- Avoid copying raw Git commit messages.
- Omit change categories that have no entries.

Use this format:

```markdown
## Version 1.2.0

**Tag:** `v1.2.0`  
**Released:** 2026-07-13

Brief description of the purpose or significance of the release.

### Added

- Added a new feature.

### Changed

- Changed an existing mechanic.

### Fixed

- Fixed an existing problem.

### Removed

- Removed obsolete content.
```

The standard change categories are:

- `Added`
- `Changed`
- `Fixed`
- `Removed`

For a small release, include only the relevant categories:

```markdown
## Version 1.2.1

**Tag:** `v1.2.1`  
**Released:** 2026-07-13

### Fixed

- Fixed rabbits failing to recognize food placed in certain troughs.
```

The changelog text supplied for the Vintage Story Mod Database should be derived from the release entry, normally without the version, tag, and release-date metadata.

## AGENTS.md Integration Snippet

Add the following to each project's root-level `AGENTS.md`:

```markdown
## Shared Project Standards

Before making changes, read and follow:

`E:\Gaming\Vintage Story\mods\Vintage Story Reference\prompts\Project and Git Hygiene\PROJECT_STANDARDS.md`

These shared standards apply unless this project's `AGENTS.md` contains a more specific instruction that explicitly overrides them.
```

This path points to the prompt catalog maintained in the Vintage Story Reference repository. If the prompt catalog is copied into another repository, update the path in that repository's `AGENTS.md` to the local copy.
