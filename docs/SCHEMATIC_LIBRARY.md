# Schematic Library

The repository keeps a curated Vintage Story schematic collection under `schematics/`. The authoritative schematic
files are ordinary JSON files on disk; generated indexes are locators and must not replace those source files.

## Layout

```text
schematics/
├── inbox/          Local, untracked files awaiting review
├── library/        Flat, tracked collection of approved schematic JSON files
├── Index/          Local, rebuildable search indexes
└── manifest.json   Tracked identity, provenance, compatibility, and licensing metadata
```

Keep `library/` flat until the collection is large enough to justify another layout. Store categories, authorship,
game versions, dependencies, dimensions, tags, and provenance in `manifest.json`, not in subdirectories.

## Intake

Place incoming `.json` or archive files in `schematics/inbox/`. The inbox is intentionally ignored by Git. Before a
schematic enters the tracked library:

1. Preserve and record its original filename and source URL.
2. Identify its author and redistribution permission or license.
3. Validate that it is a Vintage Story schematic and record its declared game version.
4. Derive dimensions, block count, referenced asset domains, and other searchable metadata when practical.
5. Calculate a SHA-256 hash of the retained JSON file.
6. Assign a stable, lowercase, kebab-case filename and add its manifest entry.

Prefer filenames shaped like `<author-or-source>-<schematic-name>-<variant>.json`. Add a short content-hash suffix if
two distinct schematics would otherwise receive the same filename.

Do not commit third-party schematics without clear redistribution permission. A public download is not automatically
permission to republish. When permission is unclear, retain only catalog metadata and the upstream source URL.

## Consumption

Consumers should search the tracked manifest or a future generated index, select one schematic, and copy that JSON
file to the intended destination. A game or mod must not depend at runtime on an absolute path into this repository.

For interactive WorldEdit use, copy a selected schematic to the game or server data `WorldEdit` directory and load it
by filename. For mod use, copy the selected source into the consuming mod's appropriate asset tree and document the
schematic's provenance and license there.

The `schematics/Index/` directory is reserved for future generated query artifacts. Its contents remain ignored except
for its `.gitignore` placeholder.
