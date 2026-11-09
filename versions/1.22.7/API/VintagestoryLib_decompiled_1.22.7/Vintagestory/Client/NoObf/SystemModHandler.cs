using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using ProperVersion;
using Vintagestory.API.Common;
using Vintagestory.API.Config;
using Vintagestory.API.Util;
using Vintagestory.ClientNative;
using Vintagestory.Common;
using Vintagestory.ModDb;

namespace Vintagestory.Client.NoObf;

public class SystemModHandler : ClientSystem
{
	private ModLoader loader;

	public override string Name => "modhandler";

	public SystemModHandler(ClientMain game)
		: base(game)
	{
	}

	public override void OnServerIdentificationReceived()
	{
		if (game.IsSingleplayer)
		{
			return;
		}
		List<string> modSearchPaths = new List<string>(ClientSettings.ModPaths);
		if (ScreenManager.ParsedArgs.AddModPath != null)
		{
			modSearchPaths.AddRange(ScreenManager.ParsedArgs.AddModPath);
		}
		if (game.Connectdata.Host != null)
		{
			string path = Path.Combine(GamePaths.DataPathServerMods, GamePaths.ReplaceInvalidChars(game.Connectdata.Host + "-" + game.Connectdata.Port));
			if (Directory.Exists(path))
			{
				modSearchPaths.Add(path);
			}
		}
		game.Logger.Notification("Loading and pre-starting client side mods...");
		loader = new ModLoader(game.api, modSearchPaths, ScreenManager.ParsedArgs.TraceLog);
		game.api.modLoader = loader;
		List<ModContainer> allMods = game.api.modLoader.LoadModInfos();
		List<string> disableMods = new List<string>();
		Dictionary<string, ModId> serverModsById = game.ServerMods.ToDictionary((ModId t) => t.Id, (ModId t) => t);
		foreach (ModContainer cMod in allMods)
		{
			if (cMod.Info != null && serverModsById.TryGetValue(cMod.Info.ModID, out var mod) && mod.Version != cMod.Info.Version && mod.Id != "game" && mod.Id != "creative" && mod.Id != "survival")
			{
				disableMods.Add(mod.Id + "@" + cMod.Info.Version);
			}
		}
		List<ModContainer> mods = game.api.modLoader.DisableAndVerify(allMods, disableMods);
		disableMods.AddRange(ClientSettings.DisabledMods);
		List<string> availableModsOnClient = (from modContainer in mods
			where modContainer.Info.Side == EnumAppSide.Universal && !modContainer.Error.HasValue
			select modContainer.Info.ModID + "@" + modContainer.Info.NetworkVersion).ToList();
		List<string> availableUniversalMods = (from modContainer in mods
			where modContainer.Info.Side == EnumAppSide.Universal && modContainer.Info.RequiredOnServer && !modContainer.Error.HasValue
			select modContainer.Info.ModID + "@" + modContainer.Info.NetworkVersion).ToList();
		List<string> missingModsOnClient = (from modidver in (from modId in game.ServerMods
				where modId.RequiredOnClient
				select modId.Id + "@" + modId.NetworkVersion).ToList().Except(availableModsOnClient)
			where !modidver.StartsWithOrdinal("game@") && !modidver.StartsWithOrdinal("creative@") && !modidver.StartsWithOrdinal("survival@")
			select game.ServerMods.FirstOrDefault((ModId modId) => modId.Id + "@" + modId.NetworkVersion == modidver) into modId
			select modId.Id + "@" + modId.Version).ToList();
		List<string> requiredCoreModVersions = (from text in (from modidver in missingModsOnClient
				select mods.FirstOrDefault((ModContainer modContainer) => modContainer.Info.ModID + "@" + modContainer.Info.Version == modidver && modContainer.Error == ModError.Dependency) into modContainer
				where modContainer?.MissingDependencies != null
				select modContainer).SelectMany((ModContainer modContainer) => modContainer.MissingDependencies)
			where text.StartsWithOrdinal("game@") || text.StartsWithOrdinal("creative@") || text.StartsWithOrdinal("survival@")
			select text.Split("@")[1]).ToList();
		if (requiredCoreModVersions.Count > 0)
		{
			requiredCoreModVersions.Sort((string x, string y) => SemVer.Compare(SemVer.Parse(x), SemVer.Parse(y)));
			game.disconnectReason = Lang.Get("disconnect-modrequiresnewerclient", requiredCoreModVersions[0]);
			game.exitReason = "client<=>server game version mismatch";
			game.DestroyGameSession(gotDisconnected: true, EnumExitMode.SoftExit);
			return;
		}
		if (missingModsOnClient.Count > 0)
		{
			List<string> erroringMods = new List<string>();
			foreach (string modid in missingModsOnClient)
			{
				ModContainer erroringMod = mods.FirstOrDefault((ModContainer modContainer) => modid == modContainer.Info.ModID + "@" + modContainer.Info.NetworkVersion && modContainer.Error.HasValue);
				if (erroringMod != null)
				{
					erroringMods.Add(erroringMod.Info.ModID + "@" + erroringMod.Info.Version);
				}
			}
			foreach (string val in erroringMods)
			{
				missingModsOnClient.Remove(val);
			}
			game.Logger.Notification("Disconnected, modded server with lacking mods on the client side. Mods in question: {0}, our available mods: {1}", string.Join(", ", missingModsOnClient), string.Join(", ", availableModsOnClient));
			if (erroringMods.Count > 0)
			{
				game.disconnectReason = Lang.Get("joinerror-modsmissing-modserroring", string.Join(", ", missingModsOnClient).Replace("@", " v"), string.Join(", ", erroringMods).Replace("@", " v"));
			}
			else
			{
				game.disconnectReason = Lang.Get("joinerror-modsmissing", string.Join(", ", missingModsOnClient).Replace("@", " v"));
			}
			game.disconnectAction = "trydownloadmods";
			game.disconnectMissingMods = missingModsOnClient;
			game.DestroyGameSession(gotDisconnected: true, EnumExitMode.SoftExit);
			return;
		}
		foreach (ModId mod2 in game.ServerMods)
		{
			disableMods.Remove(mod2.Id + "@" + mod2.Version);
		}
		List<string> serverMods = game.ServerMods.Select((ModId modId) => modId.Id + "@" + modId.NetworkVersion).ToList();
		List<string> missingModsOnServer = availableUniversalMods.Except(serverMods).ToList();
		disableMods.AddRange(missingModsOnServer);
		disableMods.AddRange(game.ServerModIdBlacklist);
		if (game.ServerModIdWhitelist.Count > 0)
		{
			List<string> modWhitelist = game.ServerModIdWhitelist.ToList();
			IEnumerable<string> notAllowedMods = from modContainer in mods
				where modContainer.Info.Side == EnumAppSide.Client || modContainer.Info.Side == EnumAppSide.Universal
				where modWhitelist.All((string serverModId) => !(modContainer.Info.ModID + "@" + modContainer.Info.Version).Contains(serverModId))
				select modContainer.Info.ModID + "@" + modContainer.Info.Version;
			disableMods.AddRange(notAllowedMods);
		}
		loader.LoadMods(mods, disableMods);
		CrashReporter.LoadedMods = mods.Where((ModContainer modContainer) => modContainer.Enabled).ToList();
		game.textureSize = loader.TextureSize;
		PreStartMods();
		StartMods();
		ReloadExternalAssets();
	}

	internal void SinglePlayerStart()
	{
		List<string> modSearchPaths = new List<string>(ClientSettings.ModPaths);
		if (ScreenManager.ParsedArgs.AddModPath != null)
		{
			modSearchPaths.AddRange(ScreenManager.ParsedArgs.AddModPath);
		}
		game.Logger.Notification("Loading and pre-starting client side mods...");
		loader = new ModLoader(game.api, modSearchPaths, ScreenManager.ParsedArgs.TraceLog);
		game.api.modLoader = loader;
		List<ModContainer> allMods = loader.LoadModInfos();
		List<string> disableMods = new List<string>();
		disableMods.AddRange(ClientSettings.DisabledMods);
		List<ModContainer> mods = loader.DisableAndVerify(allMods, disableMods);
		if (loader.MissingDependencies.Count > 0)
		{
			List<string> requiredCoreModVersions = (from modid in loader.MissingDependencies
				where modid.StartsWithOrdinal("game@") || modid.StartsWithOrdinal("creative@") || modid.StartsWithOrdinal("survival@")
				select modid.Split("@")[1]).ToList();
			if (requiredCoreModVersions.Count > 0)
			{
				requiredCoreModVersions.Sort((string x, string y) => SemVer.Compare(SemVer.Parse(x), SemVer.Parse(y)));
				StringBuilder sb = new StringBuilder();
				sb.AppendLine(Lang.Get("disconnect-modrequiresnewerclient", requiredCoreModVersions[0]));
				sb.AppendLine();
				sb.AppendLine(Lang.Get("disconnect-modrequiresnewerclient-sp"));
				game.disconnectReason = sb.ToString();
				game.disconnectAction = "disconnectSP";
				game.exitReason = "mod requiers newer game version";
				game.DestroyGameSession(gotDisconnected: true, EnumExitMode.SoftExit);
			}
			else
			{
				game.disconnectReason = Lang.Get("joinerror-modsmissing", string.Join(", ", loader.MissingDependencies).Replace("@", " v"));
				game.disconnectAction = "trydownloadmods";
				game.disconnectMissingMods = loader.MissingDependencies;
				game.DestroyGameSession(gotDisconnected: true, EnumExitMode.SoftExit);
			}
		}
		else
		{
			if (!ClientSettings.DisableModSafetyCheck)
			{
				ModDbUtil.ModBlockListMRE.WaitOne();
			}
			loader.LoadMods(mods, disableMods);
			CrashReporter.LoadedMods = mods.Where((ModContainer mod) => mod.Enabled).ToList();
			game.textureSize = loader.TextureSize;
		}
	}

	internal void PreStartMods()
	{
		loader.RunModPhase(ModRunPhase.Pre);
		game.Logger.Notification("Done loading and pre-starting client side mods.");
	}

	internal void ReloadExternalAssets()
	{
		game.Logger.VerboseDebug("Searching file system (including mods) for asset files");
		game.Platform.AssetManager.AddExternalAssets(game.Logger, loader);
		game.Logger.VerboseDebug("Finished the search for asset files");
		foreach (KeyValuePair<string, ITranslationService> availableLanguage in Lang.AvailableLanguages)
		{
			availableLanguage.Value.Invalidate();
		}
		Lang.Load(game.Logger, game.AssetManager, ClientSettings.Language);
		game.Logger.Notification("Reloaded lang file now with mod assets");
		game.Logger.VerboseDebug("Loaded lang file: " + ClientSettings.Language);
	}

	internal void OnAssetsLoaded()
	{
		loader.RunModPhase(ModRunPhase.AssetsLoaded);
	}

	internal override void OnLevelFinalize()
	{
		loader.RunModPhase(ModRunPhase.AssetsFinalize);
	}

	internal void StartMods()
	{
		loader.RunModPhase(ModRunPhase.Start);
	}

	internal void StartModsFully()
	{
		loader.RunModPhase(ModRunPhase.Normal);
	}

	public override void OnBlockTexturesLoaded()
	{
		game.api.Logger.VerboseDebug("Trigger mod event OnBlockTexturesLoaded");
		game.api.eventapi.TriggerBlockTexturesLoaded();
	}

	public override void Dispose(ClientMain game)
	{
		base.Dispose(game);
		loader?.Dispose();
		CrashReporter.LoadedMods.Clear();
	}

	public override EnumClientSystemType GetSystemType()
	{
		return EnumClientSystemType.Misc;
	}
}
