---
name: do-new-web
description: Project-local workflow for copying curated external web documentation sources from the previous Vintage Story Reference version into a new version, building versions/VERSION/Index/web.sqlite and web.index.json, and updating catalogs/docs so the web index is discoverable. Use when do-new-version needs web sources for a new Vintage Story version, or when the user asks to copy, add, refresh, rebuild, or index web documentation sources under versions/VERSION/Web in this repo.
---

# Do New Web

## Scope

Use this skill only inside the Vintage Story Reference repository. It carries curated external documentation sources forward from one Vintage Story version to another and builds the target version's web documentation index.

Use this workflow as part of `do-new-version` after the target version folder exists and before final catalog/query validation. Also use it independently when the user explicitly asks to copy, update, refresh, rebuild, or index web sources for a version.

## Hard Rules

- Do not delete existing `versions/<version>/Web` files unless the user explicitly asks for deletion in the same request.
- Create `versions/<version>/Web` and `versions/<version>/Index` if they do not already exist.
- For a new Vintage Story version, copy every source entry from the nearest previous version's `Web/sources.json` unless the user provides a different source version or source list.
- If the target `Web/sources.json` already exists, preserve target-only entries and merge missing previous-version entries by `slug`; do not silently remove or replace target-specific edits.
- Keep source slugs stable when carrying sources forward so existing query examples and docs continue to work.
- Treat `versions/<version>/Web/sources.json` as curated source metadata, not generated index output. It should be kept with the version.
- Add `versions/<version>/Web/sources.json` to Git after creating or merging it. Do not leave it only as an untracked local file.
- Build `versions/<version>/Index/web.sqlite` and `versions/<version>/Index/web.index.json` from the target source manifest.
- Track `versions/<version>/Index/.gitignore` so the `Index` folder exists in Git, but do not stage or commit `web.sqlite` or `web.index.json` unless the user explicitly asks and generated-index versioning has been confirmed.
- Update `versions/<version>/version.catalog.json` so it includes the `web` index entry after the index is built.
- Preserve the invariant that the newest retained Vintage Story release is always the root catalog default. If the
  target is the newest retained release, set it as `reference.catalog.json` `default_game_version`; when backfilling
  an older release, keep the newest retained release as the default.
- Update `docs/PROJECT_MEMORY.md` before finishing the work session.

## Version Selection

Determine the target Vintage Story version in this order:

1. Use the version explicitly named by the user.
2. Use the version implied by a file path or folder currently being discussed, such as `versions/1.22.4`.
3. If the workflow is invoked from `do-new-version`, use that target version.
4. If multiple version folders exist and the user did not specify a target, ask which version to update.

Determine the source version in this order:

1. Use the source version explicitly named by the user.
2. Use the nearest previous `versions/<version>` folder that contains `Web/sources.json`.
3. If no previous version has `Web/sources.json`, report that there is no web source manifest to copy.

## Workflow

1. Read the project-local instructions required by `AGENTS.md`, including project memory and project standards. Read coding standards if editing code or tooling.
2. Inspect `git status`, `versions/<target>`, and `versions/<source>/Web/sources.json` so existing user/reference material is not overwritten.
3. Create `versions/<target>/Web` and `versions/<target>/Index` if they do not exist.
4. Copy or merge source entries into `versions/<target>/Web/sources.json`:
   - If the target manifest does not exist, copy the source manifest exactly except for formatting normalizations produced by JSON tooling.
   - If the target manifest exists, merge by `slug`, keeping target entries when a slug already exists and adding only missing source entries.
5. Validate `sources.json`:
   - It parses as JSON.
   - It has `schema_version`.
   - It has a non-empty `sources` array.
   - Each entry has a stable `slug`, `title`, `source_type`, `url`, and `raw_url`.
6. Add the curated target source manifest to Git:

```powershell
git add -- versions/<target>/Web/sources.json
```

7. Build the web index:

```powershell
python -B tools\web-index\build_web_index.py --version <target>
```

8. Keep `versions/<target>/Index/.gitignore` tracked and leave the generated `versions/<target>/Index/web.sqlite` and `versions/<target>/Index/web.index.json` uncommitted by default.
9. Update `versions/<target>/version.catalog.json` so `indexes.web` points at:
   - `versions/<target>/Index/web.sqlite`
   - `versions/<target>/Index/web.index.json`
   - `versions/<target>/Web/sources.json`
   - `tools/web-index/build_web_index.py`
   - `tools/web-index/query_web_index.py`
10. Update `current_index_summary.web_sources` and `current_index_summary.web_sections` from the generated web manifest.
11. Ensure `reference.catalog.json` defaults to the newest retained release. If this changes the default, update root
    documentation and examples in the same change.
12. Update root docs or catalogs that describe the target version if they currently say the version has no web index.
13. Validate with representative queries:

```powershell
python -B tools\reference-query.py --version <target> --list-indexes
python -B tools\reference-query.py --version <target> web --list-sources
python -B tools\reference-query.py --version <target> web --search "configlib-patches json" --limit 5
python -B tools\reference-query.py --version <target> web --source rpvoicechat-server-administration --search "bitrate" --limit 5
```

14. Summarize the source version used, target manifest path, copied or merged source count, Git staging status for
    `sources.json`, generated web index counts, generated index Git status, catalog/doc updates, the retained release
    that is now the default, and validation commands.

## Validation

Before saying the web update is ready, verify:

- `versions/<target>/Web/sources.json` exists and includes all source slugs from the selected previous version.
- `git status --short -- versions/<target>/Web/sources.json` shows `A` or another tracked/staged state, not only `??`.
- `versions/<target>/Index/web.sqlite` and `versions/<target>/Index/web.index.json` exist.
- The generated manifest reports the expected source count and a non-zero section count.
- `tools/reference-query.py --version <target> --list-indexes` lists `web`.
- `tools/reference-query.py --version <target> web --list-sources` returns the copied source slugs.
- At least one representative content search succeeds.
