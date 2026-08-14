# Simple VS Manager 2.3.0 Reference Source

This folder contains the Vintage Story Mod DB release archive for Simple VS Manager 2.3.0.

- Mod DB page: https://mods.vintagestory.at/simplevsmanager
- Source repository linked by Mod DB: https://github.com/Interzoneism/Simple-Mod-Manager
- Mod DB type: `externaltool`
- Mod DB author: `Interzoneism`
- Mod DB release timestamp: `2026-08-09 21:11:02`
- Downloaded archive: `.codex/tmp/mod-updates/1.22.6/archives/Simple VS Manager 2.3.0.zip`
- Archive SHA-256: `71F7F9F91A6DCC234ADE5BF095FA34FE2D043AE0DBA92FAEFCF3C2D9EC2AF7F8`

The released ZIP contains a single native .NET single-file executable, `Simple VS Manager.exe`. The embedded .NET
bundle was extracted without running the executable. The extracted bundle contents are kept under `bundle contents`,
and the decompiled managed assemblies are kept under `decompiled assemblies`.

Simple VS Manager is not a normal in-game Vintage Story mod package and does not include `modinfo.json`. It is included
in the 1.22.6 reference set because its manager/launcher behavior may be useful when researching Ultimate Mod Manager
workflows.

The bundle contains managed dependencies from CommunityToolkit.Mvvm, HtmlAgilityPack, ModernWpf, QuestPDF,
UglyToad.PdfPig, and YamlDotNet, plus native libraries such as qpdf, QuestPdfSkia, zlib, libgcc, libstdc++, and
libwinpthread. These dependencies are preserved as bundled release material; their decompiled output is reference-only
evidence and should not be copied into another project.

No explicit license file was present in the downloaded release archive, and the GitHub license API returned 404 on
2026-08-10. Treat the Simple VS Manager decompile and bundled dependencies as non-reusable third-party reference
material unless a license is verified separately.
