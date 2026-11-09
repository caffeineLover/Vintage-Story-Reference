# Simple VS Manager 2.3.0 Reference Source

This folder contains the Vintage Story Mod DB release archive for Simple VS Manager 2.3.0.

- Mod DB page: https://mods.vintagestory.at/simplevsmanager
- Source repository linked by Mod DB: https://github.com/Interzoneism/Simple-Mod-Manager
- Mod DB type: `externaltool`
- Mod DB author: `Interzoneism`
- Selected release timestamp: `2026-08-07 21:30:45`
- Mod DB page last-modified timestamp: `2026-08-09 21:11:02`
- Downloaded archive: `.codex/tmp/mod-updates/1.22.7/archives/simplevsmanager_2.3.0.zip`
- Archive SHA-256: `71F7F9F91A6DCC234ADE5BF095FA34FE2D043AE0DBA92FAEFCF3C2D9EC2AF7F8`

The released ZIP contains a single native .NET single-file executable, `Simple VS Manager.exe`. The embedded .NET
bundle was extracted without running the executable. Because this release archive and executable are byte-for-byte
identical to the already validated 1.22.6 reference copy, its preserved bundle payload was used as the extraction
source. The managed assemblies were decompiled again for this 1.22.7 reference folder.

The extracted bundle contents are kept under `bundle contents`, and decompiled managed assemblies are kept under
`decompiled assemblies`. Native bundled DLLs were preserved but not decompiled.

Simple VS Manager is not a normal in-game Vintage Story mod package and does not include `modinfo.json`. It remains in
the reference set as an external tool because its manager and launcher behavior is useful for mod-management research.

No explicit license file was present in the downloaded release archive. Treat the decompile and bundled dependencies
as non-reusable third-party reference material unless upstream licensing is verified separately.
