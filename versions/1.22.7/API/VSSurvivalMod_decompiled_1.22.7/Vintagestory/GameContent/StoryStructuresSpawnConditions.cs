using System;
using System.Collections.Generic;
using System.Linq;
using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.Common.Entities;
using Vintagestory.API.MathTools;
using Vintagestory.API.Server;
using Vintagestory.ServerMods;

namespace Vintagestory.GameContent;

public class StoryStructuresSpawnConditions : ModSystem
{
	private ICoreServerAPI sapi;

	private ICoreAPI api;

	private Cuboidi[] structureLocations;

	private List<GeneratedStructure> storyStructuresClient = new List<GeneratedStructure>();

	private Vec3d tmpPos = new Vec3d();

	public override bool ShouldLoad(EnumAppSide forSide)
	{
		return true;
	}

	public override void Start(ICoreAPI api)
	{
		api.ModLoader.GetModSystem<SystemTemporalStability>().OnGetTemporalStability += ResoArchivesSpawnConditions_OnGetTemporalStability;
		this.api = api;
	}

	public override void StartClientSide(ICoreClientAPI api)
	{
		base.StartClientSide(api);
		structureLocations = Array.Empty<Cuboidi>();
		api.Event.MapRegionLoaded += Event_MapRegionLoaded;
		api.Event.MapRegionUnloaded += Event_MapRegionUnloaded;
	}

	private void Event_MapRegionUnloaded(Vec2i mapCoord, IMapRegion region)
	{
		foreach (GeneratedStructure val in region.GeneratedStructures)
		{
			if (val.Group == "storystructure")
			{
				storyStructuresClient.Remove(val);
			}
		}
		structureLocations = storyStructuresClient.Select((GeneratedStructure generatedStructure) => generatedStructure.Location).ToArray();
	}

	private void Event_MapRegionLoaded(Vec2i mapCoord, IMapRegion region)
	{
		foreach (GeneratedStructure val in region.GeneratedStructures)
		{
			if (val.Group == "storystructure")
			{
				storyStructuresClient.Add(val);
			}
		}
		structureLocations = storyStructuresClient.Select((GeneratedStructure generatedStructure) => generatedStructure.Location).ToArray();
	}

	public override void StartServerSide(ICoreServerAPI api)
	{
		base.StartServerSide(api);
		sapi = api;
		sapi.Event.OnTrySpawnEntity += Event_OnTrySpawnEntity;
	}

	private float ResoArchivesSpawnConditions_OnGetTemporalStability(float stability, double x, double y, double z)
	{
		if (isInStoryStructure(tmpPos.Set(x, y, z)))
		{
			return 1f;
		}
		return stability;
	}

	private bool Event_OnTrySpawnEntity(IBlockAccessor blockAccessor, ref EntityProperties properties, Vec3d spawnPosition, long herdId)
	{
		if (properties.Server.SpawnConditions?.Runtime == null)
		{
			return true;
		}
		if (properties.Server.SpawnConditions.Runtime.Group == "hostile" && isInStoryStructure(spawnPosition))
		{
			return false;
		}
		return true;
	}

	private void loadLocations()
	{
		if (sapi == null)
		{
			return;
		}
		GenStoryStructures structureGen = sapi.ModLoader.GetModSystem<GenStoryStructures>();
		if (structureGen == null)
		{
			return;
		}
		List<Cuboidi> locations = new List<Cuboidi>();
		foreach (KeyValuePair<string, StoryStructureLocation> structure in structureGen.Structures)
		{
			locations.Add(structure.Value.Location);
		}
		structureLocations = locations.ToArray();
	}

	private bool isInStoryStructure(Vec3d position)
	{
		if (structureLocations == null)
		{
			loadLocations();
		}
		if (structureLocations == null)
		{
			return false;
		}
		for (int i = 0; i < structureLocations.Length; i++)
		{
			if (structureLocations[i].Contains(position))
			{
				return true;
			}
		}
		return false;
	}

	public GeneratedStructure GetStoryStructureAt(BlockPos pos)
	{
		int regionSize = api.World.BlockAccessor.RegionSize;
		IMapRegion mapregion = api.World.BlockAccessor.GetMapRegion(pos.X / regionSize, pos.Z / regionSize);
		if (mapregion?.GeneratedStructures == null)
		{
			return null;
		}
		foreach (GeneratedStructure struc in mapregion.GeneratedStructures)
		{
			if (struc?.Location != null && struc.Group == "storystructure" && struc.Location.Contains(pos))
			{
				return struc;
			}
		}
		return null;
	}
}
