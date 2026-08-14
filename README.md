# Vintage Story Reference

Versioned reference repository for Vintage Story decompiled assemblies, extracted assets, selected mods, curated external documentation indexes, and reusable Codex agent prompts/skills.

For AI-enabled projects that want to use this repository without scanning the full reference trees, see `INDEX_USAGE.md`.
For task-shaped examples that combine the available query sources, see `docs/QUERY_COOKBOOK.md`.
For the architecture and maintenance model, see `docs/ARCHITECTURE.md` and `docs/MAINTAINING_REFERENCE.md`.
For committed Markdown/LaTeX research and locally generated reader PDFs, see `docs/research/`.

## Versions

- `versions/1.22.6` - Vintage Story 1.22.6 five-assembly API, assets, selected mods/tools, tracked mod manifest,
  web sources, code/asset/web indexes, and asset-class-to-registration cross-references.

## Layout

- `API` - Decompiled game assemblies and their original decompiler-generated project files.
- `Assets` - Extracted base asset trees such as `creative`, `game`, and `survival`.
- `mods` - Selected mod assets, binaries, and supporting files.
- `mods.manifest.json` - Queryable selected-mod identity, compatibility, provenance, content, and license-file metadata.
- `Web` - Curated external documentation source manifests for generated searchable web indexes.
- `prompts` - Reusable agent prompt standards for project memory, coding style, project/Git hygiene, and Vintage Story research.
- `.codex/skills` - Project-local Codex skills for maintaining and querying this reference repository.
- `tools` - Index builders, query helpers, class cross-references, and the `reference-query.py` wrapper.
- `reference.catalog.json` - Machine-readable discovery catalog for versions, prompts, skills, tools, and docs.
- `docs/research` - Topic-organized research with committed Markdown/LaTeX sources and local reader PDFs.

The version project is an index project for browsing and search. It is not intended to rebuild Vintage Story.
