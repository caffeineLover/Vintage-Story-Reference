using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using ICSharpCode.SharpZipLib.Zip;
using Mono.Cecil;
using Newtonsoft.Json;
using ProperVersion;
using Vintagestory.API.Common;
using Vintagestory.API.Config;
using Vintagestory.API.Util;

namespace Vintagestory.Common;

public class ModContainer : Mod
{
	public List<string> MissingDependencies;

	private string selectedAssemblyFile;

	private static HashAlgorithm fileHasher = SHA1.Create();

	public bool Enabled => Status == ModStatus.Enabled;

	public ModStatus Status { get; set; } = ModStatus.Enabled;

	public ModError? Error { get; set; }

	public string FolderPath { get; private set; }

	public List<string> SourceFiles { get; } = new List<string>();

	public List<string> AssemblyFiles { get; } = new List<string>();

	public bool RequiresCompilation => SourceFiles.Count > 0;

	public Assembly Assembly { get; private set; }

	public ModContainer(FileSystemInfo fsInfo, ILogger parentLogger, bool logDebug)
	{
		base.SourceType = GetSourceType(fsInfo).Value;
		base.FileName = fsInfo.Name;
		base.SourcePath = fsInfo.FullName;
		base.Logger = new ModLogger(parentLogger, this);
		base.Logger.TraceLog = logDebug;
		switch (base.SourceType)
		{
		case EnumModSourceType.CS:
			SourceFiles.Add(base.SourcePath);
			break;
		case EnumModSourceType.DLL:
			AssemblyFiles.Add(base.SourcePath);
			break;
		case EnumModSourceType.Folder:
			FolderPath = base.SourcePath;
			break;
		case EnumModSourceType.ZIP:
			break;
		}
	}

	public static EnumModSourceType? GetSourceType(FileSystemInfo fsInfo)
	{
		if (fsInfo is DirectoryInfo)
		{
			return EnumModSourceType.Folder;
		}
		return GetSourceTypeFromExtension(fsInfo.Name);
	}

	private static EnumModSourceType? GetSourceTypeFromExtension(string fileName)
	{
		string ext = Path.GetExtension(fileName);
		if (string.IsNullOrEmpty(ext))
		{
			return null;
		}
		ext = ext.Substring(1).ToUpperInvariant();
		if (!Enum.TryParse<EnumModSourceType>(ext, out var type))
		{
			return null;
		}
		return type;
	}

	public void SetError(ModError error)
	{
		Status = ModStatus.Errored;
		Error = error;
	}

	public static IEnumerable<string> EnumerateModFiles(string path)
	{
		IEnumerable<string> modfiles = Directory.EnumerateFiles(path);
		foreach (string folder in Directory.EnumerateDirectories(path))
		{
			if (!(Path.GetFileName(folder) == ".git"))
			{
				modfiles = modfiles.Concat(EnumerateModFiles(folder));
			}
		}
		return modfiles;
	}

	public void Unpack(string unpackPath)
	{
		if (!Enabled || SourceFiles.Count > 0 || AssemblyFiles.Count > 0)
		{
			return;
		}
		if (base.SourceType == EnumModSourceType.ZIP)
		{
			using (FileStream stream = File.OpenRead(base.SourcePath))
			{
				byte[] source = fileHasher.ComputeHash(stream);
				StringBuilder sb = new StringBuilder(12);
				foreach (byte item in source.Take(6))
				{
					sb.Append(item.ToString("x2"));
				}
				FolderPath = Path.Combine(unpackPath, base.FileName + "_" + sb.ToString());
			}
			string normalizedFolderPath = Path.GetFullPath(FolderPath);
			if (!Directory.Exists(FolderPath))
			{
				try
				{
					Directory.CreateDirectory(FolderPath);
					using ZipFile zipFile = new ZipFile(base.SourcePath);
					foreach (ZipEntry entry in zipFile)
					{
						string outputPath = Path.Combine(FolderPath, entry.Name);
						outputPath = Path.GetFullPath(outputPath);
						if (!outputPath.StartsWith(normalizedFolderPath))
						{
							throw new Exception("This mod tried to escape its extraction directory by extracting to '" + outputPath + "'!");
						}
						if (entry.IsDirectory)
						{
							Directory.CreateDirectory(outputPath);
							continue;
						}
						string outputDirectory = Path.GetDirectoryName(outputPath);
						if (!Directory.Exists(outputDirectory))
						{
							Directory.CreateDirectory(outputDirectory);
						}
						using Stream inputStream = zipFile.GetInputStream(entry);
						using FileStream outputStream = new FileStream(outputPath, FileMode.Create);
						inputStream.CopyTo(outputStream);
					}
				}
				catch (Exception e)
				{
					base.Logger.Error("An exception was thrown when trying to extract the mod archive to '{0}':", FolderPath);
					base.Logger.Error(e);
					SetError(ModError.Loading);
					try
					{
						Directory.Delete(FolderPath, recursive: true);
						return;
					}
					catch (Exception)
					{
						base.Logger.Error("Additionally, there was an exception when deleting cached mod folder path '{0}':", FolderPath);
						base.Logger.Error(e);
						return;
					}
				}
			}
		}
		string ignoreFilename = Path.Combine(FolderPath, ".ignore");
		IgnoreFile ignoreFile = (File.Exists(ignoreFilename) ? new IgnoreFile(ignoreFilename, FolderPath) : null);
		foreach (string path in EnumerateModFiles(FolderPath))
		{
			if (ignoreFile != null && !ignoreFile.Available(path))
			{
				continue;
			}
			EnumModSourceType? type = GetSourceTypeFromExtension(path);
			string relativePath = path.Substring(FolderPath.Length + 1);
			int slashIndex = relativePath.IndexOfAny(new char[2] { '/', '\\' });
			string topFolderName = ((slashIndex >= 0) ? relativePath.Substring(0, slashIndex) : null);
			switch (type)
			{
			case EnumModSourceType.CS:
				if (topFolderName != "src")
				{
					base.Logger.Error("File '{0}' is not in the 'src/' subfolder.", Path.GetFileName(path));
					if (base.SourceType != EnumModSourceType.Folder)
					{
						SetError(ModError.Loading);
						return;
					}
				}
				else
				{
					SourceFiles.Add(path);
				}
				break;
			case EnumModSourceType.DLL:
				if (topFolderName == "native")
				{
					break;
				}
				if (topFolderName != null)
				{
					base.Logger.Error("File '{0}' is not in the mod's root folder. Won't load this mod. If you need to ship unmanaged dlls, put them in the native/ folder.", Path.GetFileName(path));
					if (base.SourceType != EnumModSourceType.Folder)
					{
						SetError(ModError.Loading);
						return;
					}
				}
				else
				{
					AssemblyFiles.Add(path);
				}
				break;
			}
		}
	}

	public void LoadModInfo(ModCompilationContext compilationContext, ModAssemblyLoader loader)
	{
		if (!Enabled || base.Info != null)
		{
			return;
		}
		try
		{
			if (base.SourceType == EnumModSourceType.ZIP || base.SourceType == EnumModSourceType.Folder)
			{
				if (FolderPath != null)
				{
					string modInfoPath = Path.Combine(FolderPath, "modinfo.json");
					if (File.Exists(modInfoPath))
					{
						string content = File.ReadAllText(modInfoPath);
						base.Info = JsonConvert.DeserializeObject<ModInfo>(content);
						base.Info?.Init();
					}
					if (base.Info?.BackgroundPaths != null)
					{
						base.Info.Backgrounds = new BitmapSupplierDelegate[base.Info.BackgroundPaths.Length];
						for (int i = 0; i < base.Info.BackgroundPaths.Length; i++)
						{
							string path = base.Info.BackgroundPaths[i];
							if (RuntimeEnv.IsDevEnvironment)
							{
								global::_003C_003Ey__InlineArray6<string> buffer = default(global::_003C_003Ey__InlineArray6<string>);
								buffer[0] = FolderPath;
								buffer[1] = "..";
								buffer[2] = "..";
								buffer[3] = "..";
								buffer[4] = "..";
								buffer[5] = path;
								string p = Path.Combine(buffer);
								base.Info.Backgrounds[i] = () => new BitmapExternal(p);
							}
							else
							{
								base.Info.Backgrounds[i] = () => new BitmapExternal(Path.Combine(FolderPath, path));
							}
						}
					}
					string worldConfigPath = Path.Combine(FolderPath, "worldconfig.json");
					if (File.Exists(worldConfigPath))
					{
						string content2 = File.ReadAllText(worldConfigPath);
						base.WorldConfig = JsonConvert.DeserializeObject<ModWorldConfiguration>(content2);
					}
					string iconPath = null;
					if (!string.IsNullOrWhiteSpace(base.Info?.IconPath))
					{
						try
						{
							iconPath = Path.GetFullPath(Path.Combine(FolderPath, base.Info.IconPath));
						}
						catch (Exception ex)
						{
							base.Logger.Warning("Failed create path from the IconPath '{0}' specified in the ModInfo, did you use characters that are not valid in a path?: {1}.", base.Info.IconPath, ex);
							iconPath = null;
						}
						if (!iconPath.StartsWithOrdinal(FolderPath))
						{
							base.Logger.Warning("The IconPath '{0}' specified in the ModInfo tried to escape the mod root. This is not allowed.", base.Info.IconPath);
							iconPath = null;
						}
					}
					else
					{
						iconPath = Path.Combine(FolderPath, "modicon.png");
						if (!File.Exists(iconPath))
						{
							iconPath = GetFallbackIconPath();
						}
					}
					if (File.Exists(iconPath))
					{
						base.Icon = new BitmapExternal(iconPath, base.Logger);
					}
				}
				else
				{
					using (ZipFile zip = new ZipFile(base.SourcePath))
					{
						ZipEntry modInfoEntry = zip.GetEntry("modinfo.json");
						if (modInfoEntry != null)
						{
							using StreamReader reader = new StreamReader(zip.GetInputStream(modInfoEntry));
							string content3 = reader.ReadToEnd();
							base.Info = JsonConvert.DeserializeObject<ModInfo>(content3);
							base.Info?.Init();
						}
						if (base.Info.BackgroundPaths != null)
						{
							base.Info.Backgrounds = new BitmapSupplierDelegate[base.Info.BackgroundPaths.Length];
							for (int i2 = 0; i2 < base.Info.BackgroundPaths.Length; i2++)
							{
								string path2 = base.Info.BackgroundPaths[i2];
								base.Info.Backgrounds[i2] = () => loadBackground(path2);
							}
						}
						ZipEntry worldConfigEntry = zip.GetEntry("worldconfig.json");
						if (worldConfigEntry != null)
						{
							using StreamReader reader2 = new StreamReader(zip.GetInputStream(worldConfigEntry));
							string content4 = reader2.ReadToEnd();
							base.WorldConfig = JsonConvert.DeserializeObject<ModWorldConfiguration>(content4);
						}
						if (!string.IsNullOrWhiteSpace(base.Info?.IconPath))
						{
							ZipEntry iconEntry = zip.GetEntry(base.Info.IconPath);
							if (iconEntry != null)
							{
								using MemoryStream memoryStream = new MemoryStream(1048576);
								using Stream entryStream = zip.GetInputStream(iconEntry);
								entryStream.CopyTo(memoryStream);
								base.Icon = new BitmapExternal(memoryStream, base.Logger);
							}
							else
							{
								base.Logger.Warning("Failed find the IconPath '{0}' specified in the ModInfo within the mod archive.", base.Info.IconPath);
							}
						}
						else
						{
							ZipEntry iconEntry2 = zip.GetEntry("modicon.png");
							if (iconEntry2 != null)
							{
								using MemoryStream memoryStream2 = new MemoryStream(1048576);
								using Stream entryStream2 = zip.GetInputStream(iconEntry2);
								entryStream2.CopyTo(memoryStream2);
								base.Icon = new BitmapExternal(memoryStream2, base.Logger);
							}
							else
							{
								string iconPath2 = GetFallbackIconPath();
								if (File.Exists(iconPath2))
								{
									base.Icon = new BitmapExternal(iconPath2, base.Logger);
								}
							}
						}
					}
					if (base.WorldConfig != null)
					{
						Unpack(Path.Combine(GamePaths.Cache, "unpack"));
					}
				}
				if (base.Info == null)
				{
					base.Logger.Error("Missing modinfo.json");
					SetError(ModError.Loading);
				}
				if (base.WorldConfig != null)
				{
					Lang.PreLoadModWorldConfig(FolderPath, base.Info?.ModID, Lang.CurrentLocale);
				}
			}
			else
			{
				base.Info = LoadModInfoFromCode(compilationContext, loader, out var worldConfig);
				base.Info?.Init();
				base.WorldConfig = worldConfig;
				if (base.Info == null)
				{
					base.Logger.Error("Missing ModInfoAttribute");
					SetError(ModError.Loading);
				}
				string iconPath3 = null;
				if (!string.IsNullOrWhiteSpace(base.Info?.IconPath))
				{
					try
					{
						iconPath3 = Path.GetFullPath(Path.Combine(GamePaths.AssetsPath, base.Info.IconPath));
					}
					catch (Exception ex2)
					{
						base.Logger.Warning("Failed create path from the IconPath '{0}' specified in the ModInfo, did you use characters that are not valid in a path?: {1}.", base.Info.IconPath, ex2);
						iconPath3 = null;
					}
					if (!iconPath3.StartsWithOrdinal(GamePaths.AssetsPath))
					{
						base.Logger.Warning("The IconPath '{0}' specified in the ModInfo tried to escape the AssetPath. This is not allowed.", base.Info.IconPath);
						iconPath3 = null;
					}
				}
				else
				{
					iconPath3 = GetFallbackIconPath();
				}
				if (File.Exists(iconPath3))
				{
					base.Icon = new BitmapExternal(iconPath3, base.Logger);
				}
			}
			if (base.Info != null)
			{
				CheckProperVersions();
			}
		}
		catch (Exception e)
		{
			base.Logger.Error("An exception was thrown trying to to load the ModInfo:");
			base.Logger.Error(e);
			SetError(ModError.Loading);
		}
		static string GetFallbackIconPath()
		{
			return Path.Combine(GamePaths.AssetsPath, "game/textures/gui/3rdpartymodicon.png");
		}
	}

	private BitmapExternal loadBackground(string path)
	{
		using ZipFile zip2 = new ZipFile(base.SourcePath);
		ZipEntry iconEntry = zip2.GetEntry(path);
		if (iconEntry != null)
		{
			using (MemoryStream memoryStream = new MemoryStream(1048576))
			{
				using Stream entryStream = zip2.GetInputStream(iconEntry);
				entryStream.CopyTo(memoryStream);
				return new BitmapExternal(memoryStream, base.Logger);
			}
		}
		return null;
	}

	private ModInfo LoadModInfoFromCode(ModCompilationContext compilationContext, ModAssemblyLoader loader, out ModWorldConfiguration modWorldConfig)
	{
		if (RequiresCompilation)
		{
			if (AssemblyFiles.Count > 0)
			{
				throw new Exception("Found both .cs and .dll files, this is not supported");
			}
			Assembly = compilationContext.CompileFromFiles(this);
			base.Logger.Notification("Successfully compiled {0} source files", SourceFiles.Count);
			base.Logger.VerboseDebug("Successfully compiled {0} source files", SourceFiles.Count);
			return LoadModInfoFromAssembly(Assembly, out modWorldConfig);
		}
		List<string> sanitizedPaths = new List<string>(AssemblyFiles.Count);
		foreach (string path in AssemblyFiles)
		{
			sanitizedPaths.Add(GamePaths.RemoveUsername(path));
		}
		base.Logger.VerboseDebug("Check for mod systems in mod {0}", string.Join(", ", sanitizedPaths));
		List<string> assemblyCandidates = AssemblyFiles.Where((string file) => isEligible(file)).ToList();
		if (assemblyCandidates.Count == 0)
		{
			throw new Exception(string.Format("{0} declared as code mod, but there are no .dll files that contain at least one ModSystem or has a ModInfo attribute", string.Join(", ", AssemblyFiles)));
		}
		if (assemblyCandidates.Count >= 2)
		{
			throw new Exception("Found multiple .dll files with ModSystems and/or ModInfo attributes");
		}
		selectedAssemblyFile = assemblyCandidates[0];
		base.Logger.VerboseDebug("Selected assembly {0}", GamePaths.RemoveUsername(selectedAssemblyFile));
		return LoadModInfoFromAssemblyDefinition(loader.LoadAssemblyDefinition(selectedAssemblyFile), out modWorldConfig);
		bool isEligible(string path2)
		{
			AssemblyDefinition assemblyDefinition = loader.LoadAssemblyDefinition(path2);
			if (assemblyDefinition.CustomAttributes.Any((CustomAttribute attribute) => attribute.AttributeType.Name == "ModInfoAttribute"))
			{
				return assemblyDefinition.Modules.SelectMany((ModuleDefinition module) => module.Types).Any((TypeDefinition type) => !type.IsAbstract && isModSystem(type));
			}
			return false;
		}
		static bool isModSystem(TypeDefinition typeDefinition)
		{
			for (TypeReference baseType = typeDefinition.BaseType; baseType != null; baseType = baseType.Resolve()?.BaseType)
			{
				if (baseType.FullName == typeof(ModSystem).FullName)
				{
					return true;
				}
			}
			return false;
		}
	}

	public void LoadAssembly(ModCompilationContext compilationContext, ModAssemblyLoader loader)
	{
		EnumModType modType = base.Info?.Type ?? EnumModType.Code;
		if (!Enabled || Assembly != null)
		{
			return;
		}
		if (modType != EnumModType.Code)
		{
			if (SourceFiles.Count > 0 || AssemblyFiles.Count > 0)
			{
				base.Logger.Warning("Is a {0} mod, but .cs or .dll files were found. These will be ignored.", modType);
			}
			return;
		}
		string errorPath = "<unknown>";
		try
		{
			if (RequiresCompilation)
			{
				Assembly = compilationContext.CompileFromFiles(this);
				base.Logger.Notification("Successfully compiled {0} source files", SourceFiles.Count);
				base.Logger.VerboseDebug("Successfully compiled {0} source files", SourceFiles.Count);
				return;
			}
			if (selectedAssemblyFile != null)
			{
				errorPath = selectedAssemblyFile;
				Assembly = loader.LoadFrom(selectedAssemblyFile);
				return;
			}
			base.Logger.VerboseDebug("Check for mod systems in mod {0}", string.Join(", ", AssemblyFiles));
			List<Assembly> assemblyCandidates = (from ass in AssemblyFiles.Select(delegate(string path)
				{
					errorPath = path;
					return loader.LoadFrom(path);
				})
				where ass.GetCustomAttribute<ModInfoAttribute>() != null || GetModSystems(ass).Any()
				select ass).ToList();
			if (assemblyCandidates.Count == 0)
			{
				throw new Exception(string.Format("{0} declared as code mod, but there are no .dll files that contain at least one ModSystem or has a ModInfo attribute", string.Join(", ", AssemblyFiles)));
			}
			if (assemblyCandidates.Count >= 2)
			{
				throw new Exception("Found multiple .dll files with ModSystems and/or ModInfo attributes");
			}
			Assembly = assemblyCandidates[0];
			base.Logger.VerboseDebug("Loaded assembly {0}", Assembly.Location);
		}
		catch (Exception ex)
		{
			if (ex.Message == "Assembly with same name is already loaded")
			{
				string msg = "Please restart the game. The mod's .dll was already loaded and cannot be reloaded. Most likely cause is switching mod versions after already playing one world. Other rare causes include two mods with .dlls with the same name. Mod filename is " + base.FileName + ". Erroring path is: " + errorPath;
				base.Logger.Error(msg);
				SetError(ModError.ChangedVersion);
				throw new Exception(msg, ex);
			}
			base.Logger.Error("An exception was thrown when trying to load assembly:");
			base.Logger.Error(ex);
			SetError(ModError.Loading);
		}
	}

	public void InstantiateModSystems(EnumAppSide side)
	{
		if (!Enabled || Assembly == null || base.Systems.Count > 0)
		{
			return;
		}
		if (base.Info == null)
		{
			throw new InvalidOperationException("LoadModInfo was not called before InstantiateModSystems");
		}
		if (!base.Info.Side.Is(side))
		{
			Status = ModStatus.Unused;
			return;
		}
		List<ModSystem> systems = new List<ModSystem>();
		foreach (Type systemType in GetModSystems(Assembly))
		{
			try
			{
				ModSystem system = (ModSystem)Activator.CreateInstance(systemType);
				system.Mod = this;
				systems.Add(system);
			}
			catch (Exception e)
			{
				base.Logger.Error("Exception thrown when trying to create an instance of ModSystem {0}:", systemType);
				base.Logger.Error(e);
			}
		}
		base.Systems = systems.AsReadOnly();
		if (base.Systems.Count == 0 && FolderPath == null)
		{
			base.Logger.Warning("Is a Code mod, but no ModSystems found");
		}
	}

	private IEnumerable<Type> GetModSystems(Assembly assembly)
	{
		try
		{
			return from type in assembly.GetTypes()
				where typeof(ModSystem).IsAssignableFrom(type) && !type.IsAbstract
				select type;
		}
		catch (Exception ex)
		{
			if (ex is ReflectionTypeLoadException)
			{
				Exception[] es = (ex as ReflectionTypeLoadException).LoaderExceptions;
				base.Logger.Error("Exception thrown when attempting to retrieve all types of the assembly {0}. Will ignore asssembly. Loader exceptions:", assembly.FullName);
				base.Logger.Error(ex);
				if (ex.InnerException != null)
				{
					base.Logger.Error("InnerException:");
					base.Logger.Error(ex.InnerException);
				}
				for (int i = 0; i < es.Length; i++)
				{
					base.Logger.Error(es[i]);
				}
			}
			else
			{
				base.Logger.Error("Exception thrown when attempting to retrieve all types of the assembly {0}: {1}, InnerException: {2}. Will ignore asssembly.", assembly.FullName, ex, ex.InnerException);
			}
			return Enumerable.Empty<Type>();
		}
	}

	private ModInfo LoadModInfoFromAssembly(Assembly assembly, out ModWorldConfiguration modWorldConfig)
	{
		ModInfoAttribute modInfoAttr = assembly.GetCustomAttribute<ModInfoAttribute>();
		if (modInfoAttr == null)
		{
			modWorldConfig = null;
			return null;
		}
		List<ModDependency> dependencies = (from attr in assembly.GetCustomAttributes<ModDependencyAttribute>()
			select new ModDependency(attr.ModID, attr.Version)).ToList();
		return LoadModInfoFromModInfoAttribute(modInfoAttr, dependencies, out modWorldConfig);
	}

	private ModInfo LoadModInfoFromAssemblyDefinition(AssemblyDefinition assemblyDefinition, out ModWorldConfiguration modWorldConfig)
	{
		CustomAttribute modInfoAttribute = assemblyDefinition.CustomAttributes.SingleOrDefault((CustomAttribute attribute) => attribute.AttributeType.Name == "ModInfoAttribute");
		if (modInfoAttribute == null)
		{
			modWorldConfig = null;
			return null;
		}
		string name = modInfoAttribute.ConstructorArguments[0].Value as string;
		string modID = modInfoAttribute.ConstructorArguments[1].Value as string;
		ModInfoAttribute modInfo = new ModInfoAttribute(name, modID);
		foreach (Mono.Cecil.CustomAttributeNamedArgument property in modInfoAttribute.Properties.Where((Mono.Cecil.CustomAttributeNamedArgument p) => p.Name != "Name" && p.Name != "ModID"))
		{
			PropertyInfo propertySetter = modInfo.GetType().GetProperty(property.Name);
			if (property.Argument.Value is CustomAttributeArgument[] array)
			{
				propertySetter.SetValue(modInfo, array.Select((CustomAttributeArgument item) => item.Value as string).ToArray());
			}
			else
			{
				propertySetter.SetValue(modInfo, property.Argument.Value);
			}
		}
		List<ModDependency> dependencies = (from attribute in assemblyDefinition.CustomAttributes
			where attribute.AttributeType.Name == "ModDependencyAttribute"
			select new ModDependency((string)attribute.ConstructorArguments[0].Value, attribute.ConstructorArguments[1].Value as string)).ToList();
		return LoadModInfoFromModInfoAttribute(modInfo, dependencies, out modWorldConfig);
	}

	private ModInfo LoadModInfoFromModInfoAttribute(ModInfoAttribute modInfoAttr, List<ModDependency> dependencies, out ModWorldConfiguration modWorldConfig)
	{
		if (!Enum.TryParse<EnumAppSide>(modInfoAttr.Side, ignoreCase: true, out var side))
		{
			base.Logger.Warning("Cannot parse '{0}', must be either 'Client', 'Server' or 'Universal'. Defaulting to 'Universal'.", modInfoAttr.Side);
			side = EnumAppSide.Universal;
		}
		if (modInfoAttr.WorldConfig != null)
		{
			modWorldConfig = JsonConvert.DeserializeObject<ModWorldConfiguration>(modInfoAttr.WorldConfig);
		}
		else
		{
			modWorldConfig = null;
		}
		return new ModInfo(EnumModType.Code, modInfoAttr.Name, modInfoAttr.ModID, modInfoAttr.Version, modInfoAttr.Description, modInfoAttr.Authors, modInfoAttr.Contributors, modInfoAttr.Website, side, modInfoAttr.RequiredOnClient, modInfoAttr.RequiredOnServer, dependencies)
		{
			NetworkVersion = modInfoAttr.NetworkVersion,
			CoreMod = modInfoAttr.CoreMod,
			IconPath = modInfoAttr.IconPath
		};
	}

	private void CheckProperVersions()
	{
		if (!string.IsNullOrEmpty(base.Info.Version) && !SemVer.TryParse(base.Info.Version, out var guess, out var error))
		{
			base.Logger.Warning("{0} (best guess: {1})", error, guess);
		}
		foreach (ModDependency dep in base.Info.Dependencies)
		{
			if (!(dep.Version == "*") && !string.IsNullOrEmpty(dep.Version) && !SemVer.TryParse(dep.Version, out guess, out error))
			{
				base.Logger.Warning("Dependency '{0}': {1} (best guess: {2})", dep.ModID, error, guess);
			}
		}
	}
}
