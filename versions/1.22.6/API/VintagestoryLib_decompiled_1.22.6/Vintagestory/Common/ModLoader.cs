using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using ProperVersion;
using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.Config;
using Vintagestory.API.Datastructures;
using Vintagestory.API.Server;
using Vintagestory.Client;
using Vintagestory.ModDb;

namespace Vintagestory.Common;

public class ModLoader : IModLoader
{
	private readonly ICoreAPI api;

	private readonly EnumAppSide side;

	private readonly ILogger logger;

	private bool traceLog;

	private readonly ModCompilationContext compilationContext = new ModCompilationContext();

	private Dictionary<string, ModContainer> loadedMods = new Dictionary<string, ModContainer>();

	private List<ModSystem> enabledSystems = new List<ModSystem>();

	public List<string> MissingDependencies = new List<string>();

	internal Vintagestory.API.Datastructures.OrderedDictionary<string, IAssetOrigin> contentAssetOrigins;

	internal Vintagestory.API.Datastructures.OrderedDictionary<string, IAssetOrigin> themeAssetOrigins;

	public int TextureSize { get; set; } = 32;

	public IReadOnlyCollection<string> ModSearchPaths { get; }

	public string UnpackPath { get; } = Path.Combine(GamePaths.Cache, "unpack");

	public IEnumerable<Mod> Mods => loadedMods.Values.Where((ModContainer mod) => mod.Enabled);

	public IEnumerable<ModSystem> Systems => enabledSystems.Select((ModSystem x) => x);

	public Mod GetMod(string modID)
	{
		if (!loadedMods.TryGetValue(modID, out var mod))
		{
			return null;
		}
		if (!mod.Enabled)
		{
			return null;
		}
		return mod;
	}

	public bool IsModEnabled(string modID)
	{
		return (GetMod(modID) as ModContainer)?.Enabled ?? false;
	}

	public ModSystem GetModSystem(string fullName)
	{
		return Systems.FirstOrDefault((ModSystem mod) => string.Equals(mod.GetType().FullName, fullName, StringComparison.InvariantCultureIgnoreCase));
	}

	public T GetModSystem<T>(bool withInheritance = true) where T : ModSystem
	{
		if (withInheritance)
		{
			return Systems.OfType<T>().FirstOrDefault();
		}
		return Systems.FirstOrDefault((ModSystem mod) => mod.GetType() == typeof(T)) as T;
	}

	public bool IsModSystemEnabled(string fullName)
	{
		return GetModSystem(fullName) != null;
	}

	public ModLoader(ILogger logger, EnumAppSide side, IEnumerable<string> modSearchPaths, bool traceLog)
		: this(null, side, logger, modSearchPaths, traceLog)
	{
	}

	public ModLoader(ICoreAPI api, IEnumerable<string> modSearchPaths, bool traceLog)
		: this(api, api.Side, api.World.Logger, modSearchPaths, traceLog)
	{
	}

	private ModLoader(ICoreAPI api, EnumAppSide side, ILogger logger, IEnumerable<string> modSearchPaths, bool traceLog)
	{
		this.api = api;
		this.side = side;
		this.logger = logger;
		this.traceLog = traceLog;
		ModSearchPaths = modSearchPaths.Select((string path) => Path.IsPathRooted(path) ? path : Path.Combine(GamePaths.Binaries, path)).ToList().AsReadOnly();
	}

	public Vintagestory.API.Datastructures.OrderedDictionary<string, IAssetOrigin> GetContentArchives()
	{
		return contentAssetOrigins;
	}

	public Vintagestory.API.Datastructures.OrderedDictionary<string, IAssetOrigin> GetThemeArchives()
	{
		return themeAssetOrigins;
	}

	public List<ModContainer> LoadModInfos()
	{
		List<ModContainer> mods = CollectMods();
		using ModAssemblyLoader loader = new ModAssemblyLoader(ModSearchPaths, mods);
		foreach (ModContainer item in mods)
		{
			item.LoadModInfo(compilationContext, loader);
		}
		return mods;
	}

	public List<ModContainer> LoadModInfosAndVerify(IEnumerable<string> disabledModsByIdAndVersion = null)
	{
		List<ModContainer> mods = LoadModInfos();
		return DisableAndVerify(mods, disabledModsByIdAndVersion);
	}

	public List<ModContainer> DisableAndVerify(List<ModContainer> mods, IEnumerable<string> disabledModsByIdAndVersion = null)
	{
		if (disabledModsByIdAndVersion != null && disabledModsByIdAndVersion.Count() > 0)
		{
			DisableMods(mods, disabledModsByIdAndVersion);
		}
		return verifyMods(mods);
	}

	public void LoadMods(IEnumerable<string> disabledModsByIdAndVersion = null)
	{
		List<ModContainer> mods = LoadModInfos();
		LoadMods(mods, disabledModsByIdAndVersion);
	}

	public void LoadMods(List<ModContainer> mods, IEnumerable<string> disabledModsByIdAndVersion = null)
	{
		Dictionary<string, string>? modBlockList = ModDbUtil.ModBlockList;
		if (modBlockList != null && modBlockList.Count > 0)
		{
			List<string> nowBlockedMods = new List<string>();
			foreach (ModContainer mod in mods)
			{
				if (!mod.Error.HasValue && ModDbUtil.IsModBlocked(mod.Info.ModID, mod.Info.Version, out string reason))
				{
					nowBlockedMods.Add($"{mod.Info.ModID}@{mod.Info.Version}: {reason}");
				}
			}
			if (nowBlockedMods.Count > 0)
			{
				logger.Warning("The following mods where blocked from loading:");
				foreach (string bm in nowBlockedMods)
				{
					logger.Warning("  " + bm);
				}
			}
			disabledModsByIdAndVersion = ((disabledModsByIdAndVersion == null) ? new List<string>() : new List<string>(disabledModsByIdAndVersion));
			((List<string>)disabledModsByIdAndVersion).AddRange(ModDbUtil.ModBlockList.Keys);
		}
		if (disabledModsByIdAndVersion != null && disabledModsByIdAndVersion.Count() > 0)
		{
			using (ModAssemblyLoader loader = new ModAssemblyLoader(ModSearchPaths, mods))
			{
				foreach (ModContainer mod3 in mods)
				{
					mod3.LoadModInfo(compilationContext, loader);
				}
			}
			int disabledModCount = DisableMods(mods, disabledModsByIdAndVersion);
			logger.Notification("Found {0} mods ({1} disabled)", mods.Count, disabledModCount);
		}
		else
		{
			logger.Notification("Found {0} mods (0 disabled)", mods.Count);
		}
		mods = verifyMods(mods);
		logger.Notification("Mods, sorted by dependency: {0}", string.Join(", ", mods.Select((ModContainer m) => m.Info.ModID)));
		foreach (ModContainer mod2 in mods)
		{
			if (mod2.Enabled)
			{
				mod2.Unpack(UnpackPath);
			}
		}
		ClearCacheFolder(mods);
		enabledSystems = instantiateMods(mods);
	}

	private List<ModContainer> verifyMods(List<ModContainer> mods)
	{
		CheckDuplicateModIDMods(mods);
		return CheckAndSortDependencies(mods);
	}

	private List<ModSystem> instantiateMods(List<ModContainer> mods)
	{
		List<ModSystem> enabledSystems = new List<ModSystem>();
		mods = mods.OrderBy((ModContainer modContainer) => modContainer.RequiresCompilation).ToList();
		using (ModAssemblyLoader loader = new ModAssemblyLoader(ModSearchPaths, mods))
		{
			foreach (ModContainer mod in mods)
			{
				if (mod.Enabled)
				{
					mod.LoadAssembly(compilationContext, loader);
					if (mod.Status == ModStatus.Errored && mod.Error == ModError.ChangedVersion && api is ICoreServerAPI sapi && !sapi.Server.IsDedicated)
					{
						throw new RestartGameException(Lang.Get("modwarning-assemblyloaded", mod.Info.ModID));
					}
				}
			}
		}
		logger.VerboseDebug("{0} assemblies loaded", mods.Count);
		if (mods.Any((ModContainer modContainer) => modContainer.Error.HasValue && modContainer.RequiresCompilation))
		{
			logger.Warning("One or more source code mods failed to compile. Info to modders: In case you cannot find the problem, be aware that the game engine currently can only compile C# code until version 5.0. Any language features from C#6.0 or above will result in compile errors.");
		}
		foreach (ModContainer mod2 in mods)
		{
			if (mod2.Enabled)
			{
				logger.VerboseDebug("Instantiate mod systems for {0}", mod2.Info.ModID);
				mod2.InstantiateModSystems(side);
			}
		}
		contentAssetOrigins = new Vintagestory.API.Datastructures.OrderedDictionary<string, IAssetOrigin>();
		themeAssetOrigins = new Vintagestory.API.Datastructures.OrderedDictionary<string, IAssetOrigin>();
		Vintagestory.API.Datastructures.OrderedDictionary<string, int> textureSizes = new Vintagestory.API.Datastructures.OrderedDictionary<string, int>();
		foreach (ModContainer mod3 in mods.Where((ModContainer modContainer) => modContainer.Enabled))
		{
			loadedMods.Add(mod3.Info.ModID, mod3);
			enabledSystems.AddRange(mod3.Systems);
			if (mod3.FolderPath != null && Directory.Exists(Path.Combine(mod3.FolderPath, "assets")))
			{
				bool num = mod3.Info.Type == EnumModType.Theme;
				Vintagestory.API.Datastructures.OrderedDictionary<string, IAssetOrigin> origins = (num ? themeAssetOrigins : contentAssetOrigins);
				FolderOrigin origin = (num ? new ThemeFolderOrigin(mod3.FolderPath, (api.Side == EnumAppSide.Client) ? "textures/" : null) : new FolderOrigin(mod3.FolderPath, (api.Side == EnumAppSide.Client) ? "textures/" : null));
				origins.Add(mod3.FileName, origin);
				textureSizes.Add(mod3.FileName, mod3.Info.TextureSize);
			}
		}
		if (textureSizes.Count > 0)
		{
			TextureSize = textureSizes.Values.Last();
		}
		enabledSystems = enabledSystems.OrderBy((ModSystem system) => system.ExecuteOrder()).ToList();
		logger.Notification("Instantiated {0} mod systems from {1} enabled mods", enabledSystems.Count, Mods.Count());
		return enabledSystems;
	}

	private void ClearCacheFolder(IEnumerable<ModContainer> mods)
	{
		if (!Directory.Exists(UnpackPath))
		{
			return;
		}
		foreach (string folder in Directory.GetDirectories(UnpackPath).Except<string>(from mod in mods
			where !mod.Error.HasValue
			select mod.FolderPath, StringComparer.InvariantCultureIgnoreCase))
		{
			try
			{
				string[] files = Directory.GetFiles(folder, "*.dll");
				for (int num = 0; num < files.Length; num++)
				{
					File.Delete(files[num]);
				}
			}
			catch
			{
				break;
			}
			try
			{
				Directory.Delete(folder, recursive: true);
			}
			catch (Exception e)
			{
				logger.Error("There was an exception deleting the cached mod folder '{0}':");
				logger.Error(e);
			}
		}
	}

	private List<ModContainer> CollectMods()
	{
		List<DirectoryInfo> dirInfos = (from path in ModSearchPaths
			select new DirectoryInfo(path) into directoryInfo
			group directoryInfo by directoryInfo.FullName.ToLowerInvariant() into @group
			select @group.First()).ToList();
		logger.Notification("Will search the following paths for mods:");
		foreach (DirectoryInfo dirInfo in dirInfos)
		{
			if (dirInfo.Exists)
			{
				logger.Notification("    {0}", GamePaths.RemoveUsername(dirInfo.FullName));
			}
			else
			{
				logger.Notification("    {0} (Not found?)", GamePaths.RemoveUsername(dirInfo.FullName));
			}
		}
		return (from fsInfo in dirInfos.Where((DirectoryInfo directoryInfo) => directoryInfo.Exists).SelectMany((DirectoryInfo directoryInfo) => directoryInfo.GetFileSystemInfos())
			where ModContainer.GetSourceType(fsInfo).HasValue
			select new ModContainer(fsInfo, logger, traceLog)).OrderBy<ModContainer, string>((ModContainer mod) => mod.FileName, StringComparer.OrdinalIgnoreCase).ToList();
	}

	private int DisableMods(IEnumerable<ModContainer> mods, IEnumerable<string> disabledModsByIdAndVersion)
	{
		if (disabledModsByIdAndVersion == null)
		{
			return 0;
		}
		HashSet<string> disabledSet = new HashSet<string>(disabledModsByIdAndVersion);
		List<ModContainer> disabledMods = mods.Where((ModContainer mod) => mod?.Info == null || disabledSet.Contains(mod.Info.ModID + "@" + mod.Info.Version) || disabledSet.Contains(mod.Info.ModID ?? "")).ToList();
		foreach (ModContainer item in disabledMods)
		{
			item.Status = ModStatus.Disabled;
		}
		return disabledMods.Count();
	}

	private void CheckDuplicateModIDMods(IEnumerable<ModContainer> mods)
	{
		foreach (IGrouping<string, ModContainer> duplicateMods in from mod in mods
			where mod.Info?.ModID != null && mod.Enabled
			group mod by mod.Info.ModID into @group
			where @group.Skip(1).Any()
			select @group)
		{
			IOrderedEnumerable<ModContainer> sortedMods = duplicateMods.OrderBy((ModContainer mod) => mod.Info);
			logger.Warning("Multiple mods share the mod ID '{0}' ({1}). Will only load the highest version one - v{2}.", duplicateMods.Key, string.Join(", ", duplicateMods.Select((ModContainer m) => "'" + m.FileName + "'")), sortedMods.First().Info.Version);
			foreach (ModContainer item in sortedMods.Skip(1))
			{
				item.SetError(ModError.Loading);
			}
		}
	}

	private List<ModContainer> CheckAndSortDependencies(IEnumerable<ModContainer> mods)
	{
		mods = mods.Where((ModContainer modContainer) => !modContainer.Error.HasValue && modContainer.Enabled).ToList();
		List<ModContainer> sorted = new List<ModContainer>();
		HashSet<ModContainer> toCheck = new HashSet<ModContainer>(mods);
		List<ModContainer> toRemove = new List<ModContainer>();
		Dictionary<string, ModContainer> lookup = mods.Where((ModContainer modContainer) => modContainer.Info?.ModID != null).ToDictionary((ModContainer modContainer) => modContainer.Info.ModID);
		do
		{
			toRemove.Clear();
			foreach (ModContainer mod in toCheck)
			{
				bool dependenciesMet = true;
				if (mod.Info != null)
				{
					foreach (ModDependency dependency in mod.Info.Dependencies)
					{
						if (!lookup.TryGetValue(dependency.ModID, out var dependingMod) || !SatisfiesVersion(dependency.Version, dependingMod.Info.Version) || !dependingMod.Enabled)
						{
							mod.SetError(ModError.Dependency);
						}
						else if (toCheck.Contains(dependingMod))
						{
							dependenciesMet = false;
						}
					}
				}
				if (dependenciesMet)
				{
					toRemove.Add(mod);
				}
			}
			foreach (ModContainer mod2 in toRemove)
			{
				toCheck.Remove(mod2);
				sorted.Add(mod2);
			}
		}
		while (toRemove.Count > 0);
		foreach (ModContainer mod3 in mods)
		{
			if (mod3.Enabled || mod3.Status == ModStatus.Disabled)
			{
				continue;
			}
			mod3.Logger.Error("Could not resolve some dependencies:");
			foreach (ModDependency dependency2 in mod3.Info.Dependencies)
			{
				if (!lookup.TryGetValue(dependency2.ModID, out var dependingMod2))
				{
					mod3.Logger.Error("    {0} - Missing", dependency2);
					MissingDependencies.Add(dependency2.ModID + "@" + dependency2.Version);
					if (mod3.MissingDependencies == null)
					{
						mod3.MissingDependencies = new List<string>();
					}
					mod3.MissingDependencies.Add(dependency2.ModID + "@" + dependency2.Version);
				}
				else if (!SatisfiesVersion(dependency2.Version, dependingMod2.Info.Version))
				{
					mod3.Logger.Error("    {0} - Version mismatch (has {1})", dependency2, dependingMod2.Info.Version);
					MissingDependencies.Add(dependency2.ModID + "@" + dependency2.Version);
					if (mod3.MissingDependencies == null)
					{
						mod3.MissingDependencies = new List<string>();
					}
					mod3.MissingDependencies.Add(dependency2.ModID + "@" + dependency2.Version);
				}
				else if (dependingMod2.Error == ModError.Loading)
				{
					mod3.Logger.Error("    {0} - Dependency {1} failed loading", dependency2, dependingMod2);
				}
				else if (dependingMod2.Error == ModError.Dependency)
				{
					mod3.Logger.Error("    {0} - Dependency {1} has dependency errors itself", dependency2, dependingMod2);
				}
				else if (!dependingMod2.Enabled)
				{
					mod3.Logger.Error("    {0} - Dependency {1} is not enabled", dependency2, dependingMod2);
				}
			}
		}
		if (toCheck.Count > 0)
		{
			logger.Warning("Possible cyclic dependencies between mods: " + string.Join(", ", toCheck));
			sorted.AddRange(toCheck);
		}
		return sorted;
	}

	private bool SatisfiesVersion(string requested, string provided)
	{
		if (string.IsNullOrEmpty(requested) || string.IsNullOrEmpty(provided) || requested == "*")
		{
			return true;
		}
		SemVer.TryParse(requested, out var reqVersion);
		SemVer.TryParse(provided, out var provVersion);
		return provVersion >= reqVersion;
	}

	public void RunModPhase(ModRunPhase phase)
	{
		RunModPhase(ref enabledSystems, phase);
	}

	public void RunModPhase(ref List<ModSystem> enabledSystems, ModRunPhase phase)
	{
		if (phase != ModRunPhase.Normal)
		{
			foreach (ModSystem system in enabledSystems)
			{
				if (system != null && system.ShouldLoad(api) && !TryRunModPhase(system.Mod, system, api, phase))
				{
					logger.Error("Failed to run mod phase {0} for mod {1}", phase, system);
				}
			}
			return;
		}
		List<ModSystem> startedSystems = new List<ModSystem>();
		foreach (ModSystem system2 in enabledSystems)
		{
			if (system2.ShouldLoad(api))
			{
				logger.VerboseDebug("Starting system: " + system2.GetType().Name);
				if (TryRunModPhase(system2.Mod, system2, api, ModRunPhase.Normal))
				{
					startedSystems.Add(system2);
					continue;
				}
				logger.Error("Failed to start system {0}", system2);
			}
		}
		logger.Notification("Started {0} systems on {1}:", startedSystems.Count, api.Side);
		foreach (IGrouping<Mod, ModSystem> group in from modSystem in startedSystems
			group modSystem by modSystem.Mod)
		{
			logger.Notification("    Mod {0}:", group.Key);
			foreach (ModSystem system3 in group)
			{
				logger.Notification("        {0}", system3);
			}
		}
		enabledSystems = startedSystems;
	}

	private bool TryRunModPhase(Mod mod, ModSystem system, ICoreAPI api, ModRunPhase phase)
	{
		try
		{
			switch (phase)
			{
			case ModRunPhase.Pre:
				system.StartPre(api);
				break;
			case ModRunPhase.Start:
				system.Start(api);
				break;
			case ModRunPhase.AssetsLoaded:
				system.AssetsLoaded(api);
				break;
			case ModRunPhase.AssetsFinalize:
				system.AssetsFinalize(api);
				break;
			case ModRunPhase.Normal:
				if (api.Side == EnumAppSide.Client)
				{
					system.StartClientSide(api as ICoreClientAPI);
				}
				else
				{
					system.StartServerSide(api as ICoreServerAPI);
				}
				break;
			case ModRunPhase.Dispose:
				system.Dispose();
				break;
			}
			return true;
		}
		catch (FormatException ex)
		{
			throw ex;
		}
		catch (Exception e)
		{
			mod.Logger.Error("An exception was thrown when trying to start the mod:");
			mod.Logger.Error(e);
		}
		return false;
	}

	public void Dispose()
	{
		RunModPhase(ModRunPhase.Dispose);
	}
}
