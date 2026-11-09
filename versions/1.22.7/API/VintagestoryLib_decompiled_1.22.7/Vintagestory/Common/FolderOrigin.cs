using System;
using System.Collections.Generic;
using System.IO;
using Vintagestory.API.Common;
using Vintagestory.API.Config;
using Vintagestory.API.Util;

namespace Vintagestory.Common;

public class FolderOrigin : IAssetOrigin
{
	protected readonly Dictionary<AssetLocation, string> _fileLookup = new Dictionary<AssetLocation, string>();

	public string OriginPath { get; protected set; }

	public FolderOrigin(string fullPath)
		: this(fullPath, null)
	{
	}

	public FolderOrigin(string fullPath, string? pathForReservedCharsCheck)
	{
		OriginPath = Path.Combine(fullPath, "assets");
		string ignoreFilename = Path.Combine(fullPath, ".ignore");
		IgnoreFile ignoreFile = (File.Exists(ignoreFilename) ? new IgnoreFile(ignoreFilename, fullPath) : null);
		DirectoryInfo dir = new DirectoryInfo(OriginPath);
		int dirPathLength = dir.FullName.Length;
		if (!Directory.Exists(OriginPath))
		{
			return;
		}
		foreach (FileInfo file in dir.EnumerateFiles("*", SearchOption.AllDirectories))
		{
			if (file.Name.Equals("thumbs.db", StringComparison.InvariantCultureIgnoreCase) || file.Extension == ".psd" || file.Name[0] == '.' || (ignoreFile != null && !ignoreFile.Available(file.FullName)))
			{
				continue;
			}
			string path = file.FullName.Substring(dirPathLength + 1);
			if (Path.DirectorySeparatorChar == '\\')
			{
				path = path.Replace('\\', '/');
			}
			int firstSlashIndex = path.IndexOf('/');
			if (firstSlashIndex >= 0)
			{
				string domain = path.Substring(0, firstSlashIndex);
				path = path.Substring(firstSlashIndex + 1);
				if (pathForReservedCharsCheck != null && path.StartsWith(pathForReservedCharsCheck))
				{
					CheckForReservedCharacters(domain, path);
				}
				AssetLocation location = new AssetLocation(domain, path.ToLowerInvariant());
				_fileLookup.Add(location, file.FullName);
			}
		}
	}

	public void LoadAsset(IAsset asset)
	{
		if (!_fileLookup.TryGetValue(asset.Location, out string filePath))
		{
			throw new Exception("Requested asset [" + asset?.ToString() + "] could not be found");
		}
		asset.Data = File.ReadAllBytes(filePath);
	}

	public bool TryLoadAsset(IAsset asset)
	{
		if (!_fileLookup.TryGetValue(asset.Location, out string filePath))
		{
			return false;
		}
		asset.Data = File.ReadAllBytes(filePath);
		return true;
	}

	public List<IAsset> GetAssets(AssetCategory Category, bool shouldLoad = true)
	{
		List<IAsset> assets = new List<IAsset>();
		if (!Directory.Exists(OriginPath))
		{
			return assets;
		}
		string[] directories = Directory.GetDirectories(OriginPath);
		foreach (string fullPath in directories)
		{
			ScanAssetFolderRecursive(fullPath.Substring(OriginPath.Length + 1).ToLowerInvariant(), fullPath + Path.DirectorySeparatorChar + Category.Code, assets, shouldLoad);
		}
		return assets;
	}

	public List<IAsset> GetAssets(AssetLocation baseLocation, bool shouldLoad = true)
	{
		List<IAsset> assets = new List<IAsset>();
		ScanAssetFolderRecursive(baseLocation.Domain, OriginPath + Path.DirectorySeparatorChar + baseLocation.Domain + Path.DirectorySeparatorChar + baseLocation.Path.Replace('/', Path.DirectorySeparatorChar), assets, shouldLoad);
		return assets;
	}

	private void ScanAssetFolderRecursive(string domain, string currentPath, List<IAsset> list, bool shouldLoad)
	{
		if (!Directory.Exists(currentPath))
		{
			return;
		}
		string[] directories = Directory.GetDirectories(currentPath);
		foreach (string fullPath in directories)
		{
			ScanAssetFolderRecursive(domain, fullPath, list, shouldLoad);
		}
		directories = Directory.GetFiles(currentPath);
		foreach (string fullPath2 in directories)
		{
			FileInfo f = new FileInfo(fullPath2);
			if (!f.Name.Equals("thumbs.db", StringComparison.InvariantCultureIgnoreCase) && !f.Name.EndsWithOrdinal(".psd") && !f.Name.StartsWith('.'))
			{
				string path = fullPath2.Substring(OriginPath.Length + domain.Length + 2).Replace(Path.DirectorySeparatorChar, '/');
				AssetLocation location = new AssetLocation(domain, path.ToLowerInvariant());
				Asset asset = new Asset(shouldLoad ? File.ReadAllBytes(fullPath2) : null, location, this);
				asset.FilePath = fullPath2;
				list.Add(asset);
			}
		}
	}

	public virtual bool IsAllowedToAffectGameplay()
	{
		return true;
	}

	public static void CheckForReservedCharacters(string domain, string filepath)
	{
		string[] reservedCharacterSequences = GlobalConstants.ReservedCharacterSequences;
		foreach (string reserved in reservedCharacterSequences)
		{
			if (filepath.Contains(reserved))
			{
				throw new FormatException("Reserved characters " + reserved + " not allowed in filename:- " + domain + ":" + filepath);
			}
		}
	}
}
