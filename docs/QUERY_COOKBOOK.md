# Vintage Story Reference Query Cookbook

This cookbook gives agents repeatable, task-shaped ways to research the catalog-default Vintage Story `1.22.7`
reference without scanning the raw trees. Use `INDEX_USAGE.md` for the complete index schema and command reference;
use this document when the starting point is a modding question rather than an index name.

## Setup

From PowerShell:

```powershell
$vsref = 'E:\Gaming\Vintage Story\mods\Vintage Story Reference'
$v = '1.22.7'
```

Use the unified wrapper for routine work:

```powershell
python -B "$vsref\tools\reference-query.py" --version $v --list-indexes
python -B "$vsref\tools\reference-query.py" --version $v --list-cross-references
python -B "$vsref\tools\reference-query.py" --version $v --list-manifests
```

The five query targets have different jobs:

| Target | Use it for |
| --- | --- |
| `asset` | JSON assets, blocks, items, recipes, patches, shapes, textures, language files, and compact JSON facts |
| `code` | C# types, members, inheritance, source locations, and conservative reference hints |
| `class` / `xref` | Asset `class` and `entityClass` values joined to registration calls and C# declarations |
| `mod` | Selected-mod identity, compatibility, provenance, retained content, native binaries, and license-file status |
| `web` | Curated external documentation sections and their upstream URLs |

The core workflow is always the same: query a narrow result set, inspect returned paths or line numbers, and only then
open the exact raw file needed to prove behavior.

## Find A Block Or Item Definition

Search by content kind and a distinctive code fragment:

```powershell
python -B "$vsref\tools\reference-query.py" --version $v asset `
  --domain survival --category blocktypes --search displaycase --limit 10

python -B "$vsref\tools\reference-query.py" --version $v asset `
  --domain survival --category itemtypes --search prospecting --limit 10
```

The result's `rel_path` is the candidate source file. Open only that file:

```powershell
Get-Content -LiteralPath "$vsref\versions\1.22.7\Assets\survival\blocktypes\wood\displaycase.json"
```

If the text search is noisy, query a known compact JSON fact such as `code`, `class`, or `entityClass`:

```powershell
python -B "$vsref\tools\reference-query.py" --version $v asset `
  --fact-key class --fact-value BlockDisplayCase --limit 10
```

Do not infer exact JSON behavior from an index row. The index identifies the file and JSON path; the raw JSON remains
the source of truth.

## Trace An Asset Class To Its C# Implementation

Use the class cross-reference when the asset-side class name is known:

```powershell
python -B "$vsref\tools\reference-query.py" --version $v class `
  --asset-class BlockDisplayCase --limit 10
```

One result now supplies the asset path, literal registration call and line, registered type, and resolved type
declaration. JSON output is available for another project or tool to consume:

```powershell
python -B "$vsref\tools\reference-query.py" --version $v xref `
  --asset-class BlockDisplayCase --format json --limit 10
```

For a block entity, query its `entityClass` registration key the same way:

```powershell
python -B "$vsref\tools\reference-query.py" --version $v class `
  --asset-class DisplayCase --kind block_entity --limit 10
```

If the cross-reference is empty, fall back to separate asset and code queries. Dynamic keys, helper-wrapped calls,
factories, reflection, JSON inheritance, and runtime patches are outside the narrow relationship index. Open the
returned JSON and C# paths before making an exact behavioral claim.

## Find Class Registration Patterns

First locate the API contract:

```powershell
python -B "$vsref\tools\reference-query.py" --version $v code `
  --member RegisterBlockClass --limit 10
```

Then query actual literal registration calls rather than searching every declaration or source file:

```powershell
python -B "$vsref\tools\reference-query.py" --version $v code `
  --registration-method RegisterBlockClass --root VSSurvivalMod --limit 10
```

Filter by registration key or registered type when one is already known:

```powershell
python -B "$vsref\tools\reference-query.py" --version $v code `
  --registration DisplayCase --format json --limit 10
```

The registration index requires a supported literal key and `typeof(...)` argument. Search only the returned C# file
when investigating a dynamic or helper-wrapped registration that this conservative extraction cannot represent.

The code index also exposes conservative reference hints:

```powershell
python -B "$vsref\tools\reference-query.py" --version $v code `
  --reference ICoreAPI --root VSEssentials --limit 10
```

Reference hints are deliberately incomplete: extraction is bounded and is not a call graph. An empty reference result
does not prove that a symbol is unused. Use it only to narrow candidates, then inspect the returned raw source.

## Find A Recipe By Output Or Ingredient

Find recipes that produce a known output:

```powershell
python -B "$vsref\tools\reference-query.py" --version $v asset `
  --fact-key output --fact-value ingot-brass --limit 10
```

Find recipes that consume a known ingredient:

```powershell
python -B "$vsref\tools\reference-query.py" --version $v asset `
  --fact-key ingredients --fact-value ingot-copper --limit 20
```

The index may return both vanilla and selected-mod recipes. Use the `domain` and `rel_path` fields to distinguish them.
Open the exact recipe JSON afterward to verify quantities, variants, wildcard substitutions, returned stacks, and any
recipe-type-specific rules that compact facts do not capture.

## Compare Vanilla Behavior With A Reference Mod

Begin with the mod manifest so compatibility and available evidence are explicit:

```powershell
python -B "$vsref\tools\reference-query.py" --version $v mod `
  --has-assets yes --has-code yes --compatibility exact --limit 20

python -B "$vsref\tools\reference-query.py" --version $v mod `
  --mod-id tentbagreworked --format json
```

The manifest provides the asset folder and indexed code-root name. Use those values to constrain the next queries:

```powershell
python -B "$vsref\tools\reference-query.py" --version $v asset `
  --domain tentbagreworked --category itemtypes --search tentbagreworked --limit 10

python -B "$vsref\tools\reference-query.py" --version $v asset `
  --fact-key class --fact-value ItemTentBag --limit 10

python -B "$vsref\tools\reference-query.py" --version $v code `
  --root tentbagreworked_3.2.5 --type ItemTentBag --limit 10
```

For the vanilla side, query the analogous asset class, base type, or member in the built-in roots. Compare the exact
raw JSON and C# files returned by both searches. Treat decompiled code as behavioral evidence; do not copy it into a
mod as though it were licensed, build-ready source.

## Check A Reference Mod's Compatibility And Provenance

List exact 1.22.7 entries:

```powershell
python -B "$vsref\tools\reference-query.py" --version $v mod `
  --compatibility exact --limit 25
```

List same-line fallbacks:

```powershell
python -B "$vsref\tools\reference-query.py" --version $v mod `
  --compatibility same-line-fallback --limit 25
```

Find entries with native binaries or without decompiled code:

```powershell
python -B "$vsref\tools\reference-query.py" --version $v mod `
  --has-native-binaries yes --limit 25

python -B "$vsref\tools\reference-query.py" --version $v mod `
  --has-code no --limit 25
```

Use `--format json` when release tags, hashes, local metadata paths, or source repositories are needed. A
`license_file=no` result means no explicit license file was found in the retained reference folder; it does not prove
that the upstream project has no license. Verify upstream licensing before reusing third-party material.

## Consult Curated External Documentation

List the available sources before searching when the source scope is unclear:

```powershell
python -B "$vsref\tools\reference-query.py" --version $v web --list-sources
```

Then search a specific source:

```powershell
python -B "$vsref\tools\reference-query.py" --version $v web `
  --source configlib-json-api --search "conditional patches" --limit 10
```

The result includes the heading, indexed line range, retrieval metadata, and upstream URL. Treat the indexed content as
a dated reference. Visit the upstream URL when current wording or current compatibility matters.

## Cross-Index Investigation Checklist

For a question that crosses assets, code, mods, and documentation:

1. Record the target Vintage Story version.
2. Query `mod` first when selected third-party material may be relevant; record compatibility and the code-root name.
3. Query `asset` for candidate JSON and compact `class`, `entityClass`, `output`, or `ingredients` facts.
4. Query `class` when a class fact should lead directly to registration and type-declaration evidence.
5. Query `code` for any remaining type, member, base class, root, registration, or conservative reference hint.
6. Open only the exact raw JSON and C# paths returned by the indexes.
7. Query `web` when an external configuration or administration contract is part of the answer.
8. Report the version, query terms, source paths, line numbers, and any exact-versus-fallback compatibility caveat.

This sequence keeps retrieval cheap while preserving a clear evidence trail from question to source.
