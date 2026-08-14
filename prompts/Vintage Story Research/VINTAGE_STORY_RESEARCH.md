# Vintage Story Research Instructions

Use these instructions when a project needs to consult the Vintage Story Reference repository as an external knowledge source for Vintage Story modding, game behavior, APIs, assets, recipes, patches, Config Lib, indexed mod documentation, or examples from decompiled code.

## Reference Repository

The Vintage Story Reference repository is located at:

```text
E:\Gaming\Vintage Story\mods\Vintage Story Reference
```

Before using the reference repository, read and follow:

```text
E:\Gaming\Vintage Story\mods\Vintage Story Reference\INDEX_USAGE.md
```

That file is the authoritative usage guide for the current index layout, query scripts, indexed game versions, and limitations.
Use `docs/QUERY_COOKBOOK.md` when a common modding task benefits from a tested, cross-index query sequence.

## Core Rule

Always query the SQLite indexes and tracked mod manifest first. Do not recursively scan or bulk-read the raw `Assets`,
`API`, or `mods` trees unless the query sources cannot answer the question and a narrow candidate path has already
been identified.

Use the indexes as locators and summaries:

- Use the asset index for JSON assets, recipes, blocks, items, textures, shapes, lang files, worldgen, patches, and other asset metadata.
- Use the code index for API symbols, decompiled C# types, methods, inheritance, member signatures, and source locations.
- Use the class cross-reference for top-level asset `class` or `entityClass` values that must be traced through a
  literal registration call to a C# declaration.
- Use the mod manifest for selected-mod identity, compatibility, provenance, asset/code availability, native binaries,
  and license-file presence.
- Use the web documentation index for curated external documentation, such as Config Lib JSON API behavior and indexed mod overview pages.

## Version Discipline

Identify the Vintage Story version targeted by the project or task before researching. If the target version is unknown, check the project files and project memory first. If it still cannot be determined, use the current indexed version from `INDEX_USAGE.md` only as an explicit assumption.

Do not assume a newer or older Vintage Story version behaves the same. Prefer the index and source files under the matching `versions/<game-version>` folder.

## Standard Query Setup

From PowerShell, define the reference root:

```powershell
$vsref = 'E:\Gaming\Vintage Story\mods\Vintage Story Reference'
$v = '<game-version>'
```

Replace `<game-version>` with the target Vintage Story version. If the target version is unknown, inspect the consuming project first, then consult `INDEX_USAGE.md` for currently indexed versions and state any fallback assumption explicitly.

Use `python -B` for all helper scripts so Python does not create `__pycache__` folders.

Prefer the unified wrapper for routine queries:

```powershell
python -B "$vsref\tools\reference-query.py" --version $v asset --search copper --limit 20
python -B "$vsref\tools\reference-query.py" --version $v asset --category recipes --limit 20
python -B "$vsref\tools\reference-query.py" --version $v code --type ICoreAPI --limit 20
python -B "$vsref\tools\reference-query.py" --version $v code --member RegisterBlockClass --limit 20
python -B "$vsref\tools\reference-query.py" --version $v code --registration BlockDisplayCase --limit 20
python -B "$vsref\tools\reference-query.py" --version $v class --asset-class BlockDisplayCase --limit 20
python -B "$vsref\tools\reference-query.py" --version $v mod --compatibility exact --limit 20
python -B "$vsref\tools\reference-query.py" --version $v web --list-sources
python -B "$vsref\tools\reference-query.py" --version $v web --search "configlib-patches json" --limit 20
```

The source-specific helpers are still available when their exact interfaces are useful:

```powershell
python -B "$vsref\tools\asset-index\query_asset_index.py" --db "$vsref\versions\$v\Index\assets.sqlite" --search copper --limit 20
python -B "$vsref\tools\asset-index\query_asset_index.py" --db "$vsref\versions\$v\Index\assets.sqlite" --category recipes --limit 20
python -B "$vsref\tools\code-index\query_code_index.py" --db "$vsref\versions\$v\Index\code.sqlite" --type ICoreAPI --limit 20
python -B "$vsref\tools\code-index\query_code_index.py" --db "$vsref\versions\$v\Index\code.sqlite" --member RegisterBlockClass --limit 20
python -B "$vsref\tools\cross-reference\query_class_cross_reference.py" `
  --asset-db "$vsref\versions\$v\Index\assets.sqlite" `
  --code-db "$vsref\versions\$v\Index\code.sqlite" `
  --asset-class BlockDisplayCase --limit 20
python -B "$vsref\tools\mod-manifest\query_mod_manifest.py" --manifest "$vsref\versions\$v\mods.manifest.json" --compatibility exact --limit 20
python -B "$vsref\tools\web-index\query_web_index.py" --db "$vsref\versions\$v\Index\web.sqlite" --list-sources
python -B "$vsref\tools\web-index\query_web_index.py" --db "$vsref\versions\$v\Index\web.sqlite" --search "configlib-patches json" --limit 20
```

If a required index or manifest for the target version does not exist, do not silently use another version. Report
the missing query source and ask whether to use the nearest indexed version or update the reference archive.

## Research Workflow

1. Determine whether the question is about assets, C# code, selected-mod provenance, external documentation, or a
   combination.
2. Query the mod manifest first when third-party reference material may be relevant, then use its folder and code-root
   fields to constrain asset and code queries.
3. Read the relevant generated manifest in `versions/<version>/Index` when counts, indexed roots, or freshness matter.
4. Query the class cross-reference first when the question starts with an asset `class` or `entityClass` value.
5. Query the narrowest useful individual index with a small limit, usually `10` to `50`.
6. Inspect the returned paths, line numbers, headings, snippets, or source URLs.
7. Open raw files only after the query returns specific candidate files or line ranges.
8. For exact C# behavior, read the specific source file and line range returned by the code index.
9. For exact JSON behavior, read the specific asset file returned by the asset index.
10. For curated web documentation, treat index entries as dated records and check the upstream URL when current
    upstream wording matters.
11. Summarize findings with the source of truth used, including paths, line numbers, query terms, or source URLs.

## Safety And Scope

- Do not recursively read the full reference trees into context.
- Do not load binary assets into text context.
- Do not read or use `NOTES.md` as authoritative metadata; it is personal notes only.
- Do not treat decompiled source as build-ready project source.
- Do not treat `references_hint` as a complete call graph.
- Do not treat literal registration rows or class cross-references as a complete runtime registry. Empty results do
  not rule out dynamic keys, wrappers, factories, reflection, JSON inheritance, or runtime patches.
- Do not edit the Vintage Story Reference repository from another project unless the user explicitly asks to update the reference archive itself.
- If sandbox permissions prevent reading the reference repository, ask the user to add it as a readable workspace root or otherwise grant access. Do not ask the user to paste large source, asset, or index contents.

## Applying Findings

When using reference findings to change another project:

- Prefer project-local abstractions and patterns over copying reference code.
- Treat decompiled code as behavioral evidence, not as code to paste.
- Keep compatibility notes version-specific.
- Record durable discoveries in the consuming project's project memory if persistent memory is enabled.
- Include enough reference context in the final answer that the user can tell which index, file, or documentation source supported the decision.
