## Persistent project memory
Before performing substantial work, read and follow the project-local prompt catalog instructions at:

`prompts\Project Memory\PROJECT_MEMORY.md`

These instructions are mandatory.  Maintain this repository's project-specific memory at `docs/PROJECT_MEMORY.md`.
Before completing a work session, update that project memory as required by the project-local instructions.


## Shared Coding Standards
Before writing, modifying, reviewing, or refactoring code, read and follow:

`prompts\Coding Standards\CODING_STANDARDS.md`

These project-local coding standards are mandatory for all code changes in this project.  Always read the authoritative
copy from the path above before changing code.


## Shared Project Standards
Before making changes, read and follow:

`prompts\Project Standards\PROJECT_STANDARDS.md`

These standards define the required project structure, Git hygiene, documentation rules, release process, Vintage Story
Reference usage, and handling of generated files and user-maintained documents.  Apply them throughout the project
unless this `AGENTS.md` contains a more specific instruction that explicitly overrides them.


## Vintage Story Research

When this project needs Vintage Story game, API, asset, recipe, patch, decompiled code, external documentation, or
indexed mod-reference knowledge, use the Vintage Story Reference repository:

```text
.
```

Before researching, read and follow:

```text
prompts\Vintage Story Research\VINTAGE_STORY_RESEARCH.md
```

That prompt defines when and how to use the archive, including version selection, index-first research, raw-file access,
and safety rules.


## Prompt and Skill Catalog
This repository intentionally keeps reusable agent prompts under the root-level `prompts/` folder and project-local
Codex skills under `.codex/skills/`.  These folders are part of this reference project even though the shared project
standards normally place documentation under `docs/`.

The `prompts\Vintage Story Research\VINTAGE_STORY_RESEARCH.md` prompt teaches other Codex-assisted projects how to
consult this repository as a searchable Vintage Story knowledge archive.


## Research Documents

When the user asks for research to be documented, keep the source and reader deliverables under `docs/research/`:

- The Markdown file is the canonical, detailed record for agents and future maintenance.
- Generate and commit a same-basename LaTeX file from the Markdown with Pandoc, using the repository defaults
  documented in `docs/research/README.md`.
- Compile a same-basename PDF from the LaTeX source as the polished reader copy for the user.
- Commit the Markdown and LaTeX source files. Keep the generated PDF local and uncommitted.
- Do not edit generated LaTeX or PDF output directly. Update the Markdown, then regenerate both files.
- If Pandoc or a LaTeX engine is unavailable, preserve the Markdown and report the missing dependency instead of
  silently substituting a different rendering system.
