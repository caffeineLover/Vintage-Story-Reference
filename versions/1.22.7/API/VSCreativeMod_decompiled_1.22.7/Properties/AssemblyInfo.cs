using System.Diagnostics;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.Versioning;
using System.Security;
using System.Security.Permissions;
using Vintagestory.API.Common;

[assembly: AssemblyTitle("Vintagestory World Edit Mod")]
[assembly: AssemblyDescription("www.vintagestory.at")]
[assembly: AssemblyCompany("Tyron Madlener (Anego Studios)")]
[assembly: AssemblyProduct("Vintage Story")]
[assembly: AssemblyCopyright("Copyright © 2016-2026 Anego Studios")]
[assembly: AssemblyTrademark("")]
[assembly: ComVisible(false)]
[assembly: Guid("203dfbf1-3599-43fd-8487-e1c79c2b788f")]
[assembly: AssemblyFileVersion("1.22.7")]
[assembly: ModInfo("Creative Mode", "creative", Version = "1.22.7", NetworkVersion = "1.22.6", CoreMod = true, Description = "World editing, worldedit GUI and super flat world generation", IconPath = "game/textures/gui/modicon.png", Authors = new string[] { "Tyron" }, WorldConfig = "{\r\n    \"playstyles\": [\r\n        {\r\n            \"code\": \"creativebuilding\",\r\n            \"playListCode\": \"creative\",\r\n            \"langCode\": \"creativebuilding\",\r\n            \"listOrder\": 10,\r\n            \"mods\": [\"game\", \"creative\"],\r\n            \"worldType\": \"superflat\",\r\n            \"worldConfig\": {\r\n                \"worldClimate\": \"superflat\",\r\n                \"gameMode\": \"creative\",\r\n                \"hoursPerDay\": \"2400\",\r\n                \"cloudypos\": \"0.5\",\r\n                \"temporalStability\": \"false\",\r\n                \"temporalStorms\": \"off\",\r\n                \"snowAccum\": \"false\",\r\n                \"colorAccurateWorldmap\": \"true\",\r\n                \"temporalRifts\": \"off\",\r\n                \"loreContent\": \"false\"\r\n            }\r\n        }\r\n    ],\r\n    \"worldConfigAttributes\": [\r\n\r\n    ]\r\n}")]
[assembly: ModDependency("game", "")]
[assembly: AssemblyVersion("1.22.7.0")]
