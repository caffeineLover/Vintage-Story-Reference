using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using Vintagestory.API.Common;
using Vintagestory.API.MathTools;
using Vintagestory.API.Server;
using Vintagestory.ServerMods.NoObf;

namespace Vintagestory.ServerMods;

public abstract class WorldGenAreaProvider : ModStdWorldGen, IBlockPatchModifier
{
	protected ICoreServerAPI api;

	protected Dictionary<string, BlockPatchConfig> areaPatchConfigs;

	protected List<IWorldGenArea> nearAreas = new List<IWorldGenArea>();

	protected int areaPatchHashCode;

	protected LCGRandom areaRand;

	protected RockStrataConfig rockStrata;

	protected abstract string PatchCategoryName { get; }

	protected abstract bool IsEnabled { get; }

	public abstract IEnumerable<IWorldGenArea> GetAllAreas();

	public abstract string GetBlockPatchPath(IWorldGenArea area);

	public override void StartServerSide(ICoreServerAPI api)
	{
		this.api = api;
		base.StartServerSide(api);
		areaPatchHashCode = BitConverter.ToInt32(SHA256.HashData(Encoding.UTF8.GetBytes(PatchCategoryName)));
		areaPatchConfigs = new Dictionary<string, BlockPatchConfig>();
	}

	protected virtual void InitAreaProvider(RockStrataConfig rockStrata, LCGRandom rand)
	{
		this.rockStrata = rockStrata;
		areaRand = rand;
	}

	protected virtual IWorldGenArea GetBlockingAreaAt(int x, int z, int category, IEnumerable<IWorldGenArea> areas)
	{
		foreach (IWorldGenArea area in areas)
		{
			if (area.Location.Contains(x, z))
			{
				return area;
			}
			float distSq = area.CenterPos.HorDistanceSqTo(x, z);
			int checkRadius = 0;
			int radius;
			if (category < 0)
			{
				checkRadius = area.LandformRadius;
			}
			else if (area.SkipGenerationFlags != null && area.SkipGenerationFlags.TryGetValue(category, out radius))
			{
				checkRadius = radius;
			}
			if (checkRadius > 0 && distSq < (float)(checkRadius * checkRadius))
			{
				return area;
			}
		}
		return null;
	}

	public virtual bool PreventPlacementAt(int x, int z, int category)
	{
		if (!IsEnabled)
		{
			return false;
		}
		if (category == areaPatchHashCode)
		{
			return false;
		}
		return GetBlockingAreaAt(x, z, category, nearAreas) != null;
	}

	public virtual bool PreventPlacementBroadlyAt(int chunkX, int chunkZ)
	{
		if (!IsEnabled)
		{
			return false;
		}
		HorRectanglei rect = new HorRectanglei((chunkX - 1) * 32, (chunkZ - 1) * 32, (chunkX + 2) * 32, (chunkZ + 2) * 32);
		nearAreas.Clear();
		foreach (IWorldGenArea area in GetAllAreas())
		{
			if (area.Location.Intersects(rect))
			{
				nearAreas.Add(area);
			}
		}
		if (nearAreas.Count > 0)
		{
			EnsurePatchConfigsLoaded();
		}
		return nearAreas.Count > 0;
	}

	public virtual BlockPatchConfig GetPatchProviderAt(int chunkX, int chunkZ, ref EnumHandling handling)
	{
		if (!IsEnabled)
		{
			return null;
		}
		if (areaRand == null)
		{
			return null;
		}
		int dx = areaRand.NextInt(32);
		int dz = areaRand.NextInt(32);
		int x = chunkX * 32 + dx;
		int z = chunkZ * 32 + dz;
		IWorldGenArea area = GetBlockingAreaAt(x, z, -1, nearAreas);
		if (area != null && areaPatchConfigs.TryGetValue(area.Code, out var blockPatchConfig))
		{
			return blockPatchConfig;
		}
		return null;
	}

	protected virtual void EnsurePatchConfigsLoaded()
	{
		foreach (IWorldGenArea area in GetAllAreas())
		{
			if (areaPatchConfigs.ContainsKey(area.Code))
			{
				continue;
			}
			string path = GetBlockPatchPath(area);
			if (string.IsNullOrEmpty(path))
			{
				continue;
			}
			List<KeyValuePair<AssetLocation, BlockPatch[]>> patches = (from b in api.Assets.GetMany<BlockPatch[]>(api.World.Logger, path)
				orderby b.Key.ToString()
				select b).ToList();
			if (patches == null || patches.Count <= 0)
			{
				continue;
			}
			List<BlockPatch> allPatches = new List<BlockPatch>();
			foreach (KeyValuePair<AssetLocation, BlockPatch[]> val in patches)
			{
				BlockPatch[] value = val.Value;
				for (int num = 0; num < value.Length; num++)
				{
					value[num].CategoryHashCode = areaPatchHashCode;
				}
				allPatches.AddRange(val.Value);
			}
			BlockPatchConfig config = new BlockPatchConfig
			{
				Patches = allPatches.ToArray()
			};
			config.ResolveBlockIds(api, rockStrata, areaRand);
			areaPatchConfigs[area.Code] = config;
		}
	}
}
