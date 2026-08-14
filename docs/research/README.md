# Research Document Workflow

Research produced for this repository lives under `docs/research/`, organized by subject. Mod-specific research uses
`docs/research/mods/<mod name>/`; broader topics should use an equally clear category and topic folder.

Each completed research document has three files with the same basename:

- `.md` is the canonical, detailed source for agents, review, maintenance, and future updates.
- `.tex` is generated from the Markdown with Pandoc and committed as reproducible LaTeX source.
- `.pdf` is the polished reader copy for the user, compiled from the LaTeX source and kept out of Git.

Commit the Markdown and LaTeX files together. Research PDFs are local deliverables covered by the root `.gitignore`.
Never edit generated LaTeX or PDF files directly. Update the Markdown first, then regenerate the LaTeX and PDF.

## Content Standard

A research document should identify:

- the subject and practical question;
- the applicable Vintage Story and mod versions;
- concise findings or recommendations near the beginning;
- the evidence and reasoning supporting those findings;
- important limitations, mismatches, or unresolved uncertainty;
- exact source paths, indexed queries, or upstream URLs used as evidence.

The Markdown can preserve more technical detail than the PDF reader is expected to memorize. The PDF should still
contain the complete conclusion, evidence, caveats, and sources; it is a rendered counterpart, not a shortened summary.

## LaTeX And PDF Generation

Use the shared Pandoc defaults at `tools/research-docs/pandoc-pdf.yaml`. From the repository root:

```powershell
$pandoc = (Get-Command pandoc -ErrorAction SilentlyContinue).Source
if (-not $pandoc) {
  $pandoc = Join-Path $env:ProgramFiles 'Pandoc\pandoc.exe'
}
if (-not (Test-Path -LiteralPath $pandoc)) { throw 'Pandoc is not installed or discoverable.' }

& $pandoc `
  --defaults tools/research-docs/pandoc-pdf.yaml `
  --output 'docs/research/<category>/<topic>/<NAME>.tex' `
  'docs/research/<category>/<topic>/<NAME>.md'

$xelatex = (Get-Command xelatex -ErrorAction SilentlyContinue).Source
if (-not $xelatex) {
  $xelatex = Join-Path $env:LOCALAPPDATA 'Programs\MiKTeX\miktex\bin\x64\xelatex.exe'
}
if (-not (Test-Path -LiteralPath $xelatex)) { throw 'XeLaTeX is not installed or discoverable.' }

$documentDirectory = 'docs/research/<category>/<topic>'
$repositoryRoot = (Resolve-Path '.').Path
$buildDirectory = Join-Path $repositoryRoot '.codex/tmp/research-docs/<NAME>'
New-Item -ItemType Directory -Force -Path $buildDirectory | Out-Null

Push-Location $documentDirectory
try {
  1..3 | ForEach-Object {
    & $xelatex -interaction=nonstopmode -halt-on-error -output-directory $buildDirectory '<NAME>.tex'
    if ($LASTEXITCODE -ne 0) { throw 'XeLaTeX compilation failed.' }
  }
} finally {
  Pop-Location
}

Copy-Item -LiteralPath (Join-Path $buildDirectory '<NAME>.pdf') `
  -Destination (Join-Path $documentDirectory '<NAME>.pdf') -Force
```

The defaults target XeLaTeX because it handles Unicode and modern fonts more reliably than the older pdfLaTeX path.
The documented fallback paths match the current Windows installation but commands should prefer executables on
`PATH`. Do not silently switch to a browser or HTML PDF renderer when the requested output is LaTeX-based.

After generation, open or render-check the PDF when possible, confirm that tables and code blocks fit the page, and
verify that all three basenames match exactly. Commit only `.md` and `.tex`; leave `.pdf` ignored.
