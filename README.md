# Vintage Story Reference

Versioned reference repository for Vintage Story decompiled assemblies, extracted assets, selected mods, curated external documentation indexes, and reusable Codex agent prompts/skills.

For AI-enabled projects that want to use this repository without scanning the full reference trees, see `INDEX_USAGE.md`.
For the architecture and maintenance model, see `docs/ARCHITECTURE.md` and `docs/MAINTAINING_REFERENCE.md`.

## Versions

- `versions/1.22.3` - Vintage Story 1.22.3 reference payload.
- `versions/1.22.4` - Vintage Story 1.22.4 API, assets, selected mods, web sources, code index, asset index, and web index.

## Layout

- `API` - Decompiled game assemblies and their original decompiler-generated project files.
- `Assets` - Extracted base asset trees such as `creative`, `game`, and `survival`.
- `mods` - Selected mod assets, binaries, and supporting files.
- `Web` - Curated external documentation source manifests for generated searchable web indexes.
- `prompts` - Reusable agent prompt standards for project memory, coding style, project/Git hygiene, and Vintage Story research.
- `.codex/skills` - Project-local Codex skills for maintaining and querying this reference repository.
- `tools` - Index builders, query helpers, and the `reference-query.py` wrapper.
- `reference.catalog.json` - Machine-readable discovery catalog for versions, prompts, skills, tools, and docs.

The version project is an index project for browsing and search. It is not intended to rebuild Vintage Story.
