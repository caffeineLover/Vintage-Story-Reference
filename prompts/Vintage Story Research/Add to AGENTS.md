## Vintage Story Research

When this project needs Vintage Story game, API, asset, recipe, patch, Config Lib, or indexed
mod-reference knowledge, use the Vintage Story Reference repository:

```text
..\Vintage Story Reference
```

Before researching, read and follow:

```text
..\Vintage Story Reference\prompts\Vintage Story Research\VINTAGE_STORY_RESEARCH.md
```

Then read the current reference usage guide:

```text
..\Vintage Story Reference\INDEX_USAGE.md
```

Always query the SQLite indexes before opening raw files:

- `versions\1.22.3\Index\assets.sqlite`
  - for assets, JSON, recipes, blocks, items, textures, shapes, lang files, worldgen, and patches.
- `versions\1.22.3\Index\code.sqlite`
  - for API symbols, decompiled C# types, methods, inheritance, member signatures, and source locations.
- `versions\1.22.3\Index\web.sqlite`
  - for curated external documentation such as Config Lib JSON API behavior and indexed mod overview pages.

Use the helper scripts under

- `tools\asset-index`
- `tools\code-index`
- `tools\web-index`

with small result limits.  Open raw reference files only after an index returns specific candidate paths or line numbers.
Do not recursively scan the full reference trees, do not load binary assets into text context, and do not treat
`NOTES.md` as authoritative metadata.
