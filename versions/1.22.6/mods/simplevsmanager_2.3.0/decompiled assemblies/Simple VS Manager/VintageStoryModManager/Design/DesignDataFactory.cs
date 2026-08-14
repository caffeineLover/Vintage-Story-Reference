using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using VintageStoryModManager.Models;
using VintageStoryModManager.ViewModels;

namespace VintageStoryModManager.Design
{
	// Token: 0x020000C8 RID: 200
	[NullableContext(1)]
	[Nullable(0)]
	internal static class DesignDataFactory
	{
		// Token: 0x06001020 RID: 4128 RVA: 0x00056194 File Offset: 0x00054394
		public static IReadOnlyList<ModListItemViewModel> CreateSampleMods()
		{
			List<ModListItemViewModel> list = new List<ModListItemViewModel>();
			list.Add(DesignDataFactory.CreateMod("betterhud", "Better HUD", "1.4.2", "1.21.0", "https://example.com/betterhud", "Mods/betterhud_v1.4.2.zip", "Mods/betterhud_v1.4.2.zip", ModSourceKind.ZipArchive, new string[]
			{
				"Aurora"
			}, new string[]
			{
				"Vega"
			}, new ModDependencyInfo[]
			{
				new ModDependencyInfo("game", "1.21.0")
			}, "Adds additional HUD widgets and customization options.", "Client", new bool?(true), new bool?(false), true, null, null, null, null));
			list.Add(DesignDataFactory.CreateMod("expandedstorage", "Expanded Storage", "2.0.0", "1.21.0", "https://mods.vintagestory.at/expandedstorage", "Mods/expandedstorage", "Mods/expandedstorage", ModSourceKind.Folder, new string[]
			{
				"Epsilon"
			}, Array.Empty<string>(), new ModDependencyInfo[]
			{
				new ModDependencyInfo("game", "1.21.0"),
				new ModDependencyInfo("survival", "1.21.0")
			}, "Improves and rebalances all storage-related blocks and recipes.", "Both", new bool?(true), new bool?(true), false, "Missing dependency: CarryCapacity ≥ 1.3.0", null, null, null));
			list.Add(DesignDataFactory.CreateMod("utilityscripts", "Utility Scripts", "0.9.1", "1.21.0", null, "Mods/utilityscripts.dll", "Mods/utilityscripts.dll", ModSourceKind.Assembly, new string[]
			{
				"Nova",
				"Rigel"
			}, Array.Empty<string>(), new ModDependencyInfo[]
			{
				new ModDependencyInfo("game", "1.21.0")
			}, "Assortment of server utilities and chat commands.", "Server", new bool?(false), new bool?(true), true, null, "Unable to load mod. Requires dependency CarryCapacity v1.3.0", "Failed to enable scripts due to permission error.", null));
			list.Add(DesignDataFactory.CreateMod("worldeditplus", "World Edit Plus", "3.5.0", "1.21.0", "https://example.com/worldeditplus", "Mods/worldeditplus_v3.5.0.zip", "Mods/worldeditplus_v3.5.0.zip", ModSourceKind.ZipArchive, new string[]
			{
				"Lyra"
			}, new string[]
			{
				"Orion"
			}, new ModDependencyInfo[]
			{
				new ModDependencyInfo("game", "1.21.0")
			}, "Advanced world editing tools with region presets and macros.", "Both", new bool?(true), new bool?(true), true, null, null, null, null));
			return list;
		}

		// Token: 0x06001021 RID: 4129 RVA: 0x000563B0 File Offset: 0x000545B0
		[NullableContext(2)]
		[return: Nullable(1)]
		private static ModListItemViewModel CreateMod([Nullable(1)] string modId, [Nullable(1)] string name, string version, string networkVersion, string website, [Nullable(1)] string sourcePath, [Nullable(1)] string location, ModSourceKind sourceKind, [Nullable(1)] IReadOnlyList<string> authors, [Nullable(1)] IReadOnlyList<string> contributors, [Nullable(1)] IReadOnlyList<ModDependencyInfo> dependencies, string description, string side, bool? requiredOnClient, bool? requiredOnServer, bool isActive, string error, string loadError, string activationError, ModDatabaseInfo databaseInfo = null)
		{
			ModEntry modEntry = new ModEntry();
			modEntry.ModId = modId;
			modEntry.Name = name;
			modEntry.ManifestName = name;
			modEntry.Version = version;
			modEntry.NetworkVersion = networkVersion;
			modEntry.Website = website;
			modEntry.SourcePath = sourcePath;
			modEntry.SourceKind = sourceKind;
			modEntry.Authors = authors;
			modEntry.Contributors = contributors;
			modEntry.Dependencies = dependencies;
			modEntry.Description = description;
			modEntry.Side = side;
			modEntry.RequiredOnClient = requiredOnClient;
			modEntry.RequiredOnServer = requiredOnServer;
			modEntry.IconBytes = DesignDataFactory.SampleIcon;
			modEntry.Error = error;
			modEntry.LoadError = loadError;
			modEntry.DatabaseInfo = databaseInfo;
			ModListItemViewModel modListItemViewModel = new ModListItemViewModel(modEntry, isActive, location, (ModListItemViewModel _, bool _) => Task.FromResult<ActivationResult>(new ActivationResult(true, null)), null, false, null, null, true, null, null);
			if (!string.IsNullOrWhiteSpace(activationError))
			{
				DesignDataFactory.SetActivationError(modListItemViewModel, activationError);
			}
			return modListItemViewModel;
		}

		// Token: 0x06001022 RID: 4130 RVA: 0x0005649A File Offset: 0x0005469A
		private static void SetActivationError(ModListItemViewModel viewModel, string error)
		{
			PropertyInfo property = typeof(ModListItemViewModel).GetProperty("ActivationError", 52);
			MethodInfo methodInfo = (property != null) ? property.GetSetMethod(true) : null;
			if (methodInfo == null)
			{
				return;
			}
			methodInfo.Invoke(viewModel, new object[]
			{
				error
			});
		}

		// Token: 0x0400070B RID: 1803
		private static readonly byte[] SampleIcon = Convert.FromBase64String("iVBORw0KGgoAAAANSUhEUgAAAAEAAAABCAQAAAC1HAwCAAAAC0lEQVR42mP8/x8AAusB9s8Z5n8AAAAASUVORK5CYII=");
	}
}
