# Vintage Story Reference Architecture

This repository is a searchable reference archive for AI-assisted Vintage Story modding. It is not a normal mod project and is not intended to rebuild Vintage Story. Its main job is to make large reference trees usable through small, targeted indexes.

## Main Audiences

- Other Codex-assisted Vintage Story projects that need game, asset, API, or mod-reference knowledge.
- Agents working in this repository to add versions, mods, websites, indexes, prompts, or skills.
- Human maintainers who need to understand how the archive is organized.

## Documentation Roles

- `README.md` gives a short human overview and points to the right next documents.
- `INDEX_USAGE.md` is the concrete consumer guide for the current archive layout, index paths, query commands, tables, and limitations.
- `docs/QUERY_COOKBOOK.md` provides task-shaped, cross-index answer recipes for common modding research.
- `docs/ARCHITECTURE.md` explains how the repository's moving parts relate to each other.
- `docs/MAINTAINING_REFERENCE.md` explains how to add and refresh reference material.
- `docs/PROJECT_MEMORY.md` records durable project history, decisions, risks, and verified commands.
- `docs/research/` stores topic-organized research: canonical Markdown for agents, generated and committed LaTeX
  source, and a matching local PDF for the user.
- `schematics/` stores a flat curated schematic library and tracked manifest. Its ignored `inbox/` is the local intake
  boundary, while its ignored `Index/` is reserved for rebuildable query artifacts.

## Prompt And Skill Roles

- `prompts/` is the reusable prompt catalog. These files can be referenced or copied into other projects.
- `prompts/Vintage Story Research/Add to AGENTS.md` is the short drop-in snippet for another project's `AGENTS.md`.
- `prompts/Vintage Story Research/VINTAGE_STORY_RESEARCH.md` is the stable behavior prompt for using this archive correctly.
- `.codex/skills/` contains project-local Codex skills for maintaining this archive.

## Catalog Roles

- `reference.catalog.json` is the root machine-readable discovery catalog.
- `versions/<version>/version.catalog.json` is the version-specific discovery catalog.
- Generated index manifests under `versions/<version>/Index/*.index.json` remain the source of truth for exact generated timestamps, detailed counts, roots, and limitations.
- `versions/<version>/mods.manifest.json` is tracked, curated metadata for selected mod identity, compatibility,
  provenance, retained content, and license-file presence.
- Source manifests such as `versions/<version>/Web/sources.json` record curated external inputs.

Catalogs should point to existing manifests, scripts, prompts, and skills. They should not duplicate large generated details that already live in generated index manifests.

The root catalog's `default_game_version` is the version that ordinary consumers should query when they do not have a
stronger target-version requirement. The newest retained Vintage Story release is always the catalog default. The
repository currently retains indexed `1.22.6` and `1.22.7` payloads, so `1.22.7` is the default. References to versions
older than `1.22.6` may remain in project history and memory for provenance, but current research workflows should not
assume those older payloads are present on disk.

## Tool Roles

- `tools/reference-query.py` is the preferred consumer entry point when an agent wants one stable command shape.
- `tools/asset-index/` builds and queries the asset SQLite index.
- `tools/code-index/` builds and queries the decompiled C# code SQLite index.
- `tools/cross-reference/` joins compatible facts from independently generated asset and code indexes at query time.
- `tools/mod-manifest/` queries the tracked selected-mod manifest.
- `tools/web-index/` builds and queries the curated external documentation SQLite index.

The wrapper delegates to the specialized query helpers. It should stay thin so the source-specific scripts remain the place where index-specific query behavior lives.

## Version Layout

Each version folder owns the reference payload and generated indexes for one Vintage Story game version:

```text
versions/<version>/
  API/
  Assets/
  mods/
  Web/
  Index/
  mods.manifest.json
  version.catalog.json
```

The `API`, `Assets`, and `mods` folders are raw reference material. Agents should not scan them broadly. The `Index`
folder is the normal entry point for asset, code, and web research; `mods.manifest.json` is the normal entry point for
selecting and scoping third-party reference material.

Version folders may also have a browse-only `VintageStory.Reference.<version>.csproj` so IDEs can navigate the
reference payload. Those wrapper projects are not buildable game or mod projects; they exist to make the raw reference
trees visible to editors without compiling decompiled Vintage Story source.

## Source And Generated Boundaries

The repository keeps curated source material and rebuildable generated artifacts separate:

- Retained source/reference material includes version catalogs, API decompiles, copied client assets, selected
  reference mods/tools, curated mod and web source manifests, prompt files, skills, and index/query tools.
- Rebuildable generated material includes SQLite databases and generated index manifests under
  `versions/<version>/Index`.
- Research LaTeX files are an explicit, user-requested exception to the ordinary generated-file rule. They are
  reproducible text sources committed beside their canonical Markdown under `docs/research/`. Compiled research PDFs
  remain local and ignored by Git.
- Curated schematic JSON files and `schematics/manifest.json` are retained source/reference material. Unreviewed inbox
  files and generated schematic indexes are local artifacts and remain ignored.
- Each `Index` folder should be represented in Git by its local `.gitignore`; generated `.sqlite` and `.index.json`
  files should remain local unless the maintainer explicitly chooses a large-file publishing strategy.

This boundary matters because the asset indexes can exceed normal GitHub file limits, while the underlying source
manifests and build scripts are small enough to review and reproduce from a clean checkout plus the documented source
archives.

## Index And Manifest Types

- Asset index: searchable metadata and extracted facts for assets, JSON, recipes, shapes, textures, lang files, worldgen, and patches.
- Code index: conservative symbol metadata plus evidence-backed literal registration calls for decompiled C# source.
- Class cross-reference: a query-time, category-aware join from asset `class` and `entityClass` facts to literal code
  registrations and uniquely resolved type declarations. It is not a separate generated database.
- Mod manifest: curated identity, release compatibility, source provenance, retained asset/code availability, native
  binary presence, and license-file presence for selected reference mods and tools.
- Web index: curated external documentation converted into searchable sections.

Indexes are locators and summaries. Exact behavior still requires opening the specific raw source, asset, or upstream page identified by an index result.

## Design Rules

- Query indexes before opening raw files.
- Keep consumer prompts stable and version-agnostic.
- Keep `INDEX_USAGE.md` concrete and current.
- Keep machine-readable catalogs small and pointer-oriented.
- Treat web documentation entries as dated records.
- Treat decompiled source as behavioral evidence, not source to paste into mods.
- Do not treat `NOTES.md` as project metadata.
