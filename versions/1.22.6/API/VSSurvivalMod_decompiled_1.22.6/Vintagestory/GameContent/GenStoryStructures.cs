using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using Vintagestory.API.Common;
using Vintagestory.API.Config;
using Vintagestory.API.Datastructures;
using Vintagestory.API.MathTools;
using Vintagestory.API.Server;
using Vintagestory.API.Util;
using Vintagestory.ServerMods;
using Vintagestory.ServerMods.NoObf;

namespace Vintagestory.GameContent;

public class GenStoryStructures : ModStdWorldGen, IBlockPatchModifier, IStoryStructureSystem
{
	public WorldGenStoryStructuresConfig scfg;

	protected LCGRandom strucRand;

	protected LCGRandom grassRand;

	protected bool FailedToGenerateLocation;

	protected IServerNetworkChannel serverChannel;

	protected IWorldGenBlockAccessor worldgenBlockAccessor;

	protected ICoreServerAPI api;

	protected bool genStoryStructures;

	protected Cuboidi tmpCuboid = new Cuboidi();

	public BlockLayerConfig blockLayerConfig;

	public StoryStructures Structures;

	public bool StoryStructureInstancesDirty;

	private List<StoryStructureLocation> nearStructures = new List<StoryStructureLocation>();

	private Dictionary<string, BlockPatchConfig> storyStructurePatches;

	private int storyPatchhashcode = BitConverter.ToInt32(SHA256.HashData("storyPatches"u8.ToArray()));

	private GenBlockLayers genBlockLayers;

	WorldGenStoryStructuresConfig IStoryStructureSystem.scfg => scfg;

	public override double ExecuteOrder()
	{
		return 0.2;
	}

	public override void StartServerSide(ICoreServerAPI api)
	{
		this.api = api;
		base.StartServerSide(api);
		api.Event.SaveGameLoaded += Event_SaveGameLoaded;
		api.Event.GameWorldSave += Event_GameWorldSave;
		api.Event.PlayerJoin += OnPlayerJoined;
		Structures = new StoryStructures();
		ModSystemWorldGenStructures modSystem = api.ModLoader.GetModSystem<ModSystemWorldGenStructures>();
		modSystem.RegisterProvider(Structures);
		modSystem.OnRegenFinalized += FinalizeRegeneration;
		if (TerraGenConfig.DoDecorationPass)
		{
			api.Event.InitWorldGenerator(InitWorldGen, "standard");
			api.Event.ChunkColumnGeneration(OnChunkColumnGen, EnumWorldGenPass.Vegetation, "standard");
			api.Event.GetWorldgenBlockAccessor(OnWorldGenBlockAccessor);
			api.Event.WorldgenHook(GenerateHookStructure, "standard", "genHookStructure");
			api.ModLoader.GetModSystem<GenVegetationAndPatches>().RegisterPatchModifier(this);
			genBlockLayers = api.ModLoader.GetModSystem<GenBlockLayers>();
		}
		api.Event.ServerRunPhase(EnumServerRunPhase.RunGame, delegate
		{
			if (genStoryStructures)
			{
				api.ChatCommands.GetOrCreate("wgen").BeginSubCommand("story").BeginSubCommand("tp")
					.WithRootAlias("tpstoryloc")
					.WithDescription("Teleport to a story structure instance")
					.RequiresPrivilege(Privilege.controlserver)
					.RequiresPlayer()
					.WithArgs(api.ChatCommands.Parsers.WordRange("code", scfg.Structures.Select((WorldGenStoryStructure s) => s.Code).ToArray()))
					.HandleWith(OnTpStoryLoc)
					.EndSubCommand()
					.EndSubCommand();
			}
		});
		api.ChatCommands.GetOrCreate("wgen").BeginSubCommand("story").BeginSubCommand("setpos")
			.WithRootAlias("setstorystrucpos")
			.WithDescription("Set the location of a story structure")
			.RequiresPrivilege(Privilege.controlserver)
			.WithArgs(api.ChatCommands.Parsers.Word("code"), api.ChatCommands.Parsers.WorldPosition("position"), api.ChatCommands.Parsers.OptionalBool("confirm"))
			.HandleWith(OnSetStoryStructurePos)
			.EndSubCommand()
			.BeginSubCommand("removeschematiccount")
			.WithAlias("rmsc")
			.WithDescription("Remove the story structures schematic count, which allows on regen to spawn them again")
			.RequiresPrivilege(Privilege.controlserver)
			.WithArgs(api.ChatCommands.Parsers.Word("code"))
			.HandleWith(OnSetStructureNotSpawned)
			.EndSubCommand()
			.BeginSubCommand("listmissing")
			.WithDescription("List story locations that failed to generate.")
			.RequiresPrivilege(Privilege.controlserver)
			.HandleWith(OnListMissingStructures)
			.EndSubCommand()
			.EndSubCommand();
		serverChannel = api.Network.RegisterChannel("StoryGenFailed");
		serverChannel.RegisterMessageType<StoryGenFailed>();
	}

	private TextCommandResult OnListMissingStructures(TextCommandCallingArgs args)
	{
		List<string> missingStructures = GetMissingStructures();
		if (missingStructures.Count > 0)
		{
			string missing = string.Join(",", missingStructures);
			if (args.Caller.Player is IServerPlayer player)
			{
				StoryGenFailed message = new StoryGenFailed
				{
					MissingStructures = missingStructures
				};
				serverChannel.SendPacket(message, player);
			}
			return TextCommandResult.Success("Missing story locations: " + missing);
		}
		return TextCommandResult.Success("No story locations are missing.");
	}

	private void OnPlayerJoined(IServerPlayer byplayer)
	{
		if (FailedToGenerateLocation && byplayer.HasPrivilege(Privilege.controlserver))
		{
			StoryGenFailed message = new StoryGenFailed
			{
				MissingStructures = GetMissingStructures()
			};
			serverChannel.SendPacket(message, byplayer);
		}
	}

	public List<string> GetMissingStructures()
	{
		List<string> attemptedToGen = api.WorldManager.SaveGame.GetData<List<string>>("attemptedToGenerateStoryLocation");
		return Structures.GetMissingStructures(attemptedToGen);
	}

	private TextCommandResult OnSetStructureNotSpawned(TextCommandCallingArgs args)
	{
		string code = (string)args[0];
		StoryStructureLocation structure = Structures.Get(code);
		if (structure != null)
		{
			structure.SchematicsSpawned = null;
			StoryStructureInstancesDirty = true;
			return TextCommandResult.Success("Ok, story structure locations " + code + " set as not spawned.");
		}
		return TextCommandResult.Error("No such story structure exist in assets");
	}

	private TextCommandResult OnSetStoryStructurePos(TextCommandCallingArgs args)
	{
		WorldGenStoryStructure storyStruc = scfg.Structures.FirstOrDefault((WorldGenStoryStructure st) => st.Code == (string)args[0]);
		if (storyStruc == null)
		{
			return TextCommandResult.Error("No such story structure exist in assets");
		}
		if (!(bool)args[2])
		{
			double chunkRange = Math.Ceiling((float)storyStruc.LandformRadius / 32f) + 3.0;
			return TextCommandResult.Success($"Ok, will move the story structure location to this position. Make sure that at least {storyStruc.LandformRadius + 32} blocks around you are unoccupied.\n Add 'true' to the command to confirm.\n After this is done, you will have to regenerate chunks in this area, \ne.g. via <a href=\"chattype:///wgen delr {chunkRange}\">/wgen delr {chunkRange}</a> to delete {chunkRange * 32.0} blocks in all directions. They will then generate again as you move around.");
		}
		BlockPos pos = ((Vec3d)args[1]).AsBlockPos;
		pos.Y = 1;
		GenMaps genmaps = api.ModLoader.GetModSystem<GenMaps>();
		BlockSchematicPartial schem = storyStruc.schematicData;
		int minX = pos.X - schem.SizeX / 2;
		int minZ = pos.Z - schem.SizeZ / 2;
		Cuboidi cub = new Cuboidi(minX, pos.Y, minZ, minX + schem.SizeX - 1, pos.Y + schem.SizeY - 1, minZ + schem.SizeZ - 1);
		StoryStructureLocation strucloc = new StoryStructureLocation
		{
			Code = storyStruc.Code,
			CenterPos = pos,
			Location = cub,
			LandformRadius = storyStruc.LandformRadius,
			GenerationRadius = storyStruc.GenerationRadius,
			SkipGenerationFlags = storyStruc.SkipGenerationFlags
		};
		Structures.Set(storyStruc.Code, strucloc);
		if (storyStruc.RequireLandform != null)
		{
			genmaps.ForceLandformAt(new ForceLandform
			{
				CenterPos = pos,
				Radius = storyStruc.LandformRadius,
				LandformCode = storyStruc.RequireLandform
			});
		}
		if (storyStruc.ForceTemperature.HasValue || storyStruc.ForceRain.HasValue)
		{
			genmaps.ForceClimateAt(new ForceClimate
			{
				Radius = strucloc.LandformRadius,
				CenterPos = strucloc.CenterPos,
				Climate = (Climate.DescaleTemperature(((float?)storyStruc.ForceTemperature) ?? 0f) << 16) + (storyStruc.ForceRain.GetValueOrDefault() << 8)
			});
		}
		StoryStructureInstancesDirty = true;
		return TextCommandResult.Success("Ok, story structure location moved to this position. Regenerating chunks at the location should make it appear now.");
	}

	public void InitWorldGen()
	{
		genStoryStructures = api.World.Config.GetAsString("loreContent", "true").ToBool(defaultValue: true);
		if (!genStoryStructures)
		{
			return;
		}
		strucRand = new LCGRandom(api.WorldManager.Seed + 1095);
		Dictionary<AssetLocation, WorldGenStoryStructuresConfig> many = api.Assets.GetMany<WorldGenStoryStructuresConfig>(api.Logger, "worldgen/storystructures.json");
		scfg = new WorldGenStoryStructuresConfig();
		scfg.SchematicYOffsets = new Dictionary<string, int>();
		scfg.RocktypeRemapGroups = new Dictionary<string, Dictionary<AssetLocation, AssetLocation>>();
		List<WorldGenStoryStructure> structures = new List<WorldGenStoryStructure>();
		foreach (KeyValuePair<AssetLocation, WorldGenStoryStructuresConfig> item in many)
		{
			item.Deconstruct(out var key, out var value);
			WorldGenStoryStructuresConfig conf = value;
			foreach (KeyValuePair<string, Dictionary<AssetLocation, AssetLocation>> remap in conf.RocktypeRemapGroups)
			{
				if (scfg.RocktypeRemapGroups.TryGetValue(remap.Key, out var remapGroup))
				{
					foreach (KeyValuePair<AssetLocation, AssetLocation> item2 in remap.Value)
					{
						item2.Deconstruct(out key, out var value2);
						AssetLocation source = key;
						AssetLocation target = value2;
						remapGroup.TryAdd(source, target);
					}
				}
				else
				{
					scfg.RocktypeRemapGroups.TryAdd(remap.Key, remap.Value);
				}
			}
			foreach (KeyValuePair<string, int> remap2 in conf.SchematicYOffsets)
			{
				scfg.SchematicYOffsets.TryAdd(remap2.Key, remap2.Value);
			}
			structures.AddRange(conf.Structures);
		}
		scfg.Structures = structures.ToArray();
		grassRand = new LCGRandom(api.WorldManager.Seed);
		blockLayerConfig = BlockLayerConfig.GetInstance(api);
		scfg.Init(api, blockLayerConfig.RockStrata, blockLayerConfig);
		double distScale = api.World.Config.GetDecimal("storyStructuresDistScaling", 1.0);
		WorldGenStoryStructure[] structures2 = scfg.Structures;
		foreach (WorldGenStoryStructure obj in structures2)
		{
			obj.MinSpawnDistX = (int)((double)obj.MinSpawnDistX * distScale);
			obj.MinSpawnDistZ = (int)((double)obj.MinSpawnDistZ * distScale);
			obj.MaxSpawnDistX = (int)((double)obj.MaxSpawnDistX * distScale);
			obj.MaxSpawnDistZ = (int)((double)obj.MaxSpawnDistZ * distScale);
		}
		DetermineStoryStructures();
		strucRand.SetWorldSeed(api.WorldManager.Seed + 1095);
	}

	private TextCommandResult OnTpStoryLoc(TextCommandCallingArgs args)
	{
		if (!(args[0] is string code))
		{
			return TextCommandResult.Success();
		}
		StoryStructureLocation storyloc = Structures.Get(code);
		if (storyloc != null)
		{
			BlockPos pos = storyloc.CenterPos.Copy();
			pos.Y = (storyloc.Location.Y1 + storyloc.Location.Y2) / 2;
			args.Caller.Entity.TeleportTo(pos);
			return TextCommandResult.Success("Teleporting to " + code);
		}
		return TextCommandResult.Success("No such story structure, " + code);
	}

	protected void DetermineStoryStructures()
	{
		List<string> data = api.WorldManager.SaveGame.GetData<List<string>>("attemptedToGenerateStoryLocation");
		if (data == null)
		{
			data = new List<string>();
		}
		int i = 0;
		WorldGenStoryStructure[] structures = scfg.Structures;
		foreach (WorldGenStoryStructure wgenStoryStructure in structures)
		{
			StoryStructureLocation strucloc = Structures.Get(wgenStoryStructure.Code);
			if (strucloc != null)
			{
				strucloc.LandformRadius = wgenStoryStructure.LandformRadius;
				strucloc.GenerationRadius = wgenStoryStructure.GenerationRadius;
				strucloc.SkipGenerationFlags = wgenStoryStructure.SkipGenerationFlags;
				int max = strucloc.SkipGenerationFlags.Max((KeyValuePair<int, int> flag) => flag.Value);
				strucloc.MaxSkipGenerationRadiusSq = max * max;
				BlockSchematicPartial schem = wgenStoryStructure.schematicData;
				int minX = strucloc.CenterPos.X - schem.SizeX / 2;
				int minZ = strucloc.CenterPos.Z - schem.SizeZ / 2;
				Cuboidi cuboidi = new Cuboidi(minX, strucloc.CenterPos.Y, minZ, minX + schem.SizeX, strucloc.CenterPos.Y + schem.SizeY, minZ + schem.SizeZ);
				strucloc.Location = cuboidi;
				strucloc.WorldgenHeight = -1;
			}
			else if (!data.Contains(wgenStoryStructure.Code))
			{
				strucRand.SetWorldSeed(api.WorldManager.Seed + 1095 + i);
				TryAddStoryLocation(wgenStoryStructure);
				data.Add(wgenStoryStructure.Code);
			}
			i++;
		}
		StoryStructureInstancesDirty = true;
		api.WorldManager.SaveGame.StoreData("attemptedToGenerateStoryLocation", data);
		SetupForceLandform();
	}

	private void TryAddStoryLocation(WorldGenStoryStructure storyStructure)
	{
		BlockPos basePos = null;
		StoryStructureLocation dependentLocation = null;
		if (!string.IsNullOrEmpty(storyStructure.DependsOnStructure))
		{
			if (storyStructure.DependsOnStructure == "spawn")
			{
				basePos = new BlockPos(api.World.BlockAccessor.MapSizeX / 2, 0, api.World.BlockAccessor.MapSizeZ / 2, 0);
			}
			else
			{
				dependentLocation = Structures.Get(storyStructure.DependsOnStructure);
				if (dependentLocation != null)
				{
					basePos = dependentLocation.CenterPos.Copy();
				}
			}
		}
		if (basePos == null)
		{
			FailedToGenerateLocation = true;
			api.Logger.Error("Could not find dependent structure " + storyStructure.DependsOnStructure + " to generate structure: " + storyStructure.Code + ". Make sure that the dependent structure is before this one in the list.");
			api.Logger.Error($"You will need to add them manually by running /wgen story setpos {storyStructure.DependsOnStructure} and /wgen story setpos {storyStructure.Code} at two different locations about at least 1000 blocks apart.");
			return;
		}
		int dirX = dependentLocation?.DirX ?? ((!((double)strucRand.NextFloat() > 0.5)) ? 1 : (-1));
		int dirZ = ((!((double)strucRand.NextFloat() > 0.5)) ? 1 : (-1));
		int distanceX = storyStructure.MinSpawnDistX + strucRand.NextInt(storyStructure.MaxSpawnDistX + 1 - storyStructure.MinSpawnDistX);
		int distanceZ = storyStructure.MinSpawnDistZ + strucRand.NextInt(storyStructure.MaxSpawnDistZ + 1 - storyStructure.MinSpawnDistZ);
		BlockSchematicPartial schem = storyStructure.schematicData;
		EnumStructurePlacement placement = storyStructure.Placement;
		bool flag = (uint)placement <= 1u;
		int locationHeight = ((!flag) ? 1 : (api.World.SeaLevel + schem.OffsetY));
		BlockPos pos = new BlockPos(basePos.X + distanceX * dirX, locationHeight, basePos.Z + distanceZ * dirZ, 0);
		int radius = Math.Max(storyStructure.LandformRadius, storyStructure.GenerationRadius);
		int posMinX = pos.X - radius;
		int posMaxX = pos.X + radius;
		int posMinZ = pos.Z - radius;
		int posMaxZ = pos.Z + radius;
		int mapRegionPosMinX = posMinX / api.WorldManager.RegionSize;
		int mapRegionPosMaxX = posMaxX / api.WorldManager.RegionSize;
		int mapRegionPosMinZ = posMinZ / api.WorldManager.RegionSize;
		int mapRegionPosMaxZ = posMaxZ / api.WorldManager.RegionSize;
		bool hasMapRegion = false;
		for (int x = mapRegionPosMinX; x <= mapRegionPosMaxX; x++)
		{
			for (int z = mapRegionPosMinZ; z <= mapRegionPosMaxZ; z++)
			{
				if (api.WorldManager.BlockingTestMapRegionExists(x, z))
				{
					hasMapRegion = true;
				}
			}
		}
		int mapChunkPosMinX = posMinX / 32;
		int mapChunkPosMaxX = posMaxX / 32;
		int mapChunkPosMinZ = posMinZ / 32;
		int mapChunkPosMaxZ = posMaxZ / 32;
		if (hasMapRegion)
		{
			for (int i = mapChunkPosMinX; i <= mapChunkPosMaxX; i++)
			{
				for (int j = mapChunkPosMinZ; j <= mapChunkPosMaxZ; j++)
				{
					if (!api.WorldManager.BlockingTestMapChunkExists(i, j))
					{
						continue;
					}
					IServerChunk[] blockingLoadChunk = api.WorldManager.BlockingLoadChunkColumn(i, j);
					if (blockingLoadChunk == null)
					{
						continue;
					}
					IServerChunk[] array = blockingLoadChunk;
					foreach (IServerChunk chunk in array)
					{
						if (chunk.BlocksPlaced > 0 || chunk.BlocksRemoved > 0)
						{
							FailedToGenerateLocation = true;
							api.Logger.Error($"Map chunk in area of story location {storyStructure.Code} contains player edits, can not automatically add it your world. You can add it manually running /wgen story setpos {storyStructure.Code} at a location that seems suitable for you.");
							return;
						}
						chunk.Dispose();
					}
				}
			}
		}
		if (hasMapRegion)
		{
			for (int l = mapRegionPosMinX; l <= mapRegionPosMaxX; l++)
			{
				for (int m = mapRegionPosMinZ; m <= mapRegionPosMaxZ; m++)
				{
					api.WorldManager.DeleteMapRegion(l, m);
				}
			}
			for (int n = mapChunkPosMinX; n <= mapChunkPosMaxX; n++)
			{
				for (int num = mapChunkPosMinZ; num <= mapChunkPosMaxZ; num++)
				{
					api.WorldManager.DeleteChunkColumn(n, num);
				}
			}
		}
		int minX = pos.X - schem.SizeX / 2;
		int minZ = pos.Z - schem.SizeZ / 2;
		Cuboidi cuboidi = new Cuboidi(minX, pos.Y, minZ, minX + schem.SizeX, pos.Y + schem.SizeY, minZ + schem.SizeZ);
		Structures.Set(storyStructure.Code, new StoryStructureLocation
		{
			Code = storyStructure.Code,
			CenterPos = pos,
			Location = cuboidi,
			LandformRadius = storyStructure.LandformRadius,
			GenerationRadius = storyStructure.GenerationRadius,
			DirX = dirX,
			SkipGenerationFlags = storyStructure.SkipGenerationFlags
		});
	}

	private void SetupForceLandform()
	{
		GenMaps genmaps = api.ModLoader.GetModSystem<GenMaps>();
		foreach (KeyValuePair<string, StoryStructureLocation> structure in Structures)
		{
			structure.Deconstruct(out var key, out var value);
			string code = key;
			StoryStructureLocation location = value;
			WorldGenStoryStructure structureConfig = scfg.Structures.FirstOrDefault((WorldGenStoryStructure s) => s.Code == code);
			if (structureConfig == null)
			{
				api.Logger.Warning("Could not find config for story structure: " + code + ". Terrain will not be generated as it should at " + code);
				continue;
			}
			if (structureConfig.ForceTemperature.HasValue || structureConfig.ForceRain.HasValue)
			{
				genmaps.ForceClimateAt(new ForceClimate
				{
					Radius = location.LandformRadius,
					CenterPos = location.CenterPos,
					Climate = (Climate.DescaleTemperature(((float?)structureConfig.ForceTemperature) ?? 0f) << 16) + (structureConfig.ForceRain.GetValueOrDefault() << 8)
				});
			}
			if (structureConfig.RequireLandform != null)
			{
				genmaps.ForceLandformAt(new ForceLandform
				{
					Radius = location.LandformRadius,
					CenterPos = location.CenterPos,
					LandformCode = structureConfig.RequireLandform
				});
			}
		}
	}

	private void Event_GameWorldSave()
	{
		if (StoryStructureInstancesDirty)
		{
			api.WorldManager.SaveGame.StoreData("storystructurelocations", Structures.ToBytes());
			StoryStructureInstancesDirty = false;
		}
	}

	private void Event_SaveGameLoaded()
	{
		Vintagestory.API.Datastructures.OrderedDictionary<string, StoryStructureLocation> strucs = api.WorldManager.SaveGame.GetData<Vintagestory.API.Datastructures.OrderedDictionary<string, StoryStructureLocation>>("storystructurelocations");
		if (strucs != null)
		{
			Structures.Load(strucs);
		}
		if (GameVersion.IsLowerVersionThan(api.WorldManager.SaveGame.CreatedGameVersion, "1.21.0-pre.3") && !api.WorldManager.SaveGame.GetData("storyLocUpgrade-1.21.0-pre.3", defaultValue: false))
		{
			UpdateOldStoryClaims();
		}
	}

	private void UpdateOldStoryClaims()
	{
		foreach (LandClaim landClaim in api.World.Claims.All)
		{
			bool flag = landClaim.ProtectionLevel == 10 && landClaim.AllowUseEveryone;
			if (flag)
			{
				bool flag2;
				switch (landClaim.LastKnownOwnerName)
				{
				case "custommessage-nadiya":
				case "custommessage-tobias":
				case "custommessage-treasurehunter":
					flag2 = true;
					break;
				default:
					flag2 = false;
					break;
				}
				flag = flag2;
			}
			if (flag)
			{
				landClaim.AllowUseEveryone = false;
				landClaim.AllowTraverseEveryone = true;
			}
		}
		api.WorldManager.SaveGame.StoreData("storyLocUpgrade-1.21.0-pre.3", data: true);
	}

	private void OnWorldGenBlockAccessor(IChunkProviderThread chunkProvider)
	{
		worldgenBlockAccessor = chunkProvider.GetBlockAccessor(updateHeightmap: false);
	}

	private void OnChunkColumnGen(IChunkColumnGenerateRequest request)
	{
		if (!genStoryStructures)
		{
			return;
		}
		IServerChunk[] chunks = request.Chunks;
		int chunkX = request.ChunkX;
		int chunkZ = request.ChunkZ;
		tmpCuboid.Set(chunkX * 32, 0, chunkZ * 32, chunkX * 32 + 32, chunks.Length * 32, chunkZ * 32 + 32);
		worldgenBlockAccessor.BeginColumn();
		foreach (KeyValuePair<string, StoryStructureLocation> structure2 in Structures)
		{
			structure2.Deconstruct(out var key, out var value);
			string strucCode = key;
			StoryStructureLocation strucInst = value;
			Cuboidi strucloc = strucInst.Location;
			if (!strucloc.Intersects(tmpCuboid))
			{
				continue;
			}
			if (!strucInst.DidGenerate)
			{
				strucInst.DidGenerate = true;
				StoryStructureInstancesDirty = true;
			}
			BlockPos startPos = new BlockPos(strucloc.X1, strucloc.Y1, strucloc.Z1, 0);
			WorldGenStoryStructure structure = scfg.Structures.First((WorldGenStoryStructure s) => s.Code == strucCode);
			if (structure.UseWorldgenHeight)
			{
				int h;
				if (strucInst.WorldgenHeight >= 0)
				{
					h = strucInst.WorldgenHeight;
				}
				else
				{
					h = (strucInst.WorldgenHeight = chunks[0].MapChunk.WorldGenTerrainHeightMap[startPos.Z % 32 * 32 + startPos.X % 32]);
					strucloc.Y1 = h + structure.schematicData.OffsetY;
					strucloc.Y2 = strucloc.Y1 + structure.schematicData.SizeY;
					StoryStructureInstancesDirty = true;
				}
				startPos.Y = h + structure.schematicData.OffsetY;
			}
			else if (structure.Placement == EnumStructurePlacement.SurfaceRuin)
			{
				int h2;
				if (strucInst.WorldgenHeight >= 0)
				{
					h2 = strucInst.WorldgenHeight;
				}
				else
				{
					h2 = (strucInst.WorldgenHeight = chunks[0].MapChunk.WorldGenTerrainHeightMap[startPos.Z % 32 * 32 + startPos.X % 32]);
					strucloc.Y1 = h2 - structure.schematicData.SizeY + structure.schematicData.OffsetY;
					strucloc.Y2 = strucloc.Y1 + structure.schematicData.SizeY;
					StoryStructureInstancesDirty = true;
				}
				startPos.Y = h2 - structure.schematicData.SizeY + structure.schematicData.OffsetY;
			}
			else if (structure.Placement == EnumStructurePlacement.Surface)
			{
				strucloc.Y1 = api.World.SeaLevel + structure.schematicData.OffsetY;
				strucloc.Y2 = strucloc.Y1 + structure.schematicData.SizeY;
				StoryStructureInstancesDirty = true;
				startPos.Y = strucloc.Y1;
			}
			Block rockBlock = null;
			if (structure.resolvedRockTypeRemaps != null)
			{
				if (string.IsNullOrEmpty(strucInst.RockBlockCode))
				{
					strucRand.InitPositionSeed(chunkX, chunkZ);
					int lx = strucRand.NextInt(32);
					int lz = strucRand.NextInt(32);
					EnumStructurePlacement placement = structure.Placement;
					bool flag = (uint)placement <= 1u;
					int posY = ((!flag) ? (startPos.Y + structure.schematicData.SizeY / 2 + strucRand.NextInt(structure.schematicData.SizeY / 2)) : request.Chunks[0].MapChunk.WorldGenTerrainHeightMap[lz * 32 + lx]);
					int j = 0;
					while (rockBlock == null && j < 10)
					{
						Block block = worldgenBlockAccessor.GetBlockRaw(chunkX * 32 + lx, posY, chunkZ * 32 + lz, 1);
						if (block.BlockMaterial == EnumBlockMaterial.Stone)
						{
							rockBlock = block;
							strucInst.RockBlockCode = block.Code.ToString();
							StoryStructureInstancesDirty = true;
						}
						placement = structure.Placement;
						flag = (uint)placement <= 1u;
						posY = ((!flag) ? (startPos.Y + structure.schematicData.SizeY / 2 + strucRand.NextInt(structure.schematicData.SizeY / 2)) : (posY - 1));
						j++;
					}
					if (string.IsNullOrEmpty(strucInst.RockBlockCode))
					{
						api.Logger.Warning("Could not find rock block code for " + strucInst.Code);
					}
				}
				else
				{
					rockBlock = worldgenBlockAccessor.GetBlock(new AssetLocation(strucInst.RockBlockCode));
				}
			}
			int blocksPlaced = structure.schematicData.PlacePartial(chunks, worldgenBlockAccessor, api.World, chunkX, chunkZ, startPos, EnumReplaceMode.ReplaceAll, structure.Placement, GlobalConfig.ReplaceMetaBlocks, GlobalConfig.ReplaceMetaBlocks, structure.resolvedRockTypeRemaps, structure.replacewithblocklayersBlockids, rockBlock, structure.DisableSurfaceTerrainBlending);
			if (blocksPlaced > 0)
			{
				EnumStructurePlacement placement = structure.Placement;
				if ((uint)placement <= 1u)
				{
					ModStdWorldGen.UpdateHeightmap(request, worldgenBlockAccessor);
				}
				if (structure.GenerateGrass)
				{
					GenerateGrass(api, request, grassRand, genBlockLayers);
				}
			}
			string code = structure.Code + ":" + structure.Schematics[0];
			IMapRegion region = chunks[0].MapChunk.MapRegion;
			if (region.GeneratedStructures.FirstOrDefault((GeneratedStructure struc) => struc.Code.Equals(code)) == null)
			{
				region.AddGeneratedStructure(new GeneratedStructure
				{
					Code = code,
					Group = structure.Group,
					Location = strucloc.Clone()
				});
			}
			if (blocksPlaced <= 0 || !structure.BuildProtected)
			{
				continue;
			}
			if (!structure.ExcludeSchematicSizeProtect)
			{
				LandClaim[] claims = api.World.Claims.Get(strucloc.Center.AsBlockPos);
				if (claims == null || claims.Length == 0)
				{
					api.World.Claims.Add(new LandClaim
					{
						Areas = new List<Cuboidi> { strucloc },
						Description = structure.BuildProtectionDesc,
						ProtectionLevel = structure.ProtectionLevel,
						LastKnownOwnerName = structure.BuildProtectionName,
						AllowUseEveryone = structure.AllowUseEveryone,
						AllowTraverseEveryone = structure.AllowTraverseEveryone
					});
				}
			}
			if (structure.ExtraLandClaimX > 0 && structure.ExtraLandClaimZ > 0)
			{
				Cuboidi struclocDeva = new Cuboidi(strucloc.Center.X - structure.ExtraLandClaimX, 0, strucloc.Center.Z - structure.ExtraLandClaimZ, strucloc.Center.X + structure.ExtraLandClaimX, api.WorldManager.MapSizeY, strucloc.Center.Z + structure.ExtraLandClaimZ);
				LandClaim[] claims2 = api.World.Claims.Get(struclocDeva.Center.AsBlockPos);
				if (claims2 == null || claims2.Length == 0)
				{
					api.World.Claims.Add(new LandClaim
					{
						Areas = new List<Cuboidi> { struclocDeva },
						Description = structure.BuildProtectionDesc,
						ProtectionLevel = structure.ProtectionLevel,
						LastKnownOwnerName = structure.BuildProtectionName,
						AllowUseEveryone = structure.AllowUseEveryone,
						AllowTraverseEveryone = structure.AllowTraverseEveryone
					});
				}
			}
			if (structure.CustomLandClaims == null)
			{
				continue;
			}
			Cuboidi[] customLandClaims = structure.CustomLandClaims;
			for (int num = 0; num < customLandClaims.Length; num++)
			{
				Cuboidi cuboidi = customLandClaims[num].Clone();
				cuboidi.X1 += strucloc.X1;
				cuboidi.X2 += strucloc.X1;
				cuboidi.Y1 += strucloc.Y1;
				cuboidi.Y2 += strucloc.Y1;
				cuboidi.Z1 += strucloc.Z1;
				cuboidi.Z2 += strucloc.Z1;
				LandClaim[] claims3 = api.World.Claims.Get(cuboidi.Center.AsBlockPos);
				if (claims3 == null || claims3.Length == 0)
				{
					api.World.Claims.Add(new LandClaim
					{
						Areas = new List<Cuboidi> { cuboidi },
						Description = structure.BuildProtectionDesc,
						ProtectionLevel = structure.ProtectionLevel,
						LastKnownOwnerName = structure.BuildProtectionName,
						AllowUseEveryone = structure.AllowUseEveryone,
						AllowTraverseEveryone = structure.AllowTraverseEveryone
					});
				}
			}
		}
	}

	public void GenerateHookStructure(IBlockAccessor blockAccessor, BlockPos pos, string param)
	{
		AssetLocation code = new AssetLocation(param);
		api.Logger.VerboseDebug("Worldgen hook generation event fired, with code " + code);
		IMapChunk mapchunk = blockAccessor.GetMapChunkAtBlockPos(pos);
		IAsset assetMain = api.Assets.TryGet(code.WithPathPrefixOnce("worldgen/hookgeneratedstructures/").WithPathAppendixOnce(".json"));
		if (assetMain == null || mapchunk == null)
		{
			api.Logger.Error("Worldgen hook event failed: " + ((mapchunk == null) ? "bad coordinates" : string.Concat(code, "* not found")));
			return;
		}
		HookGeneratedStructure hookStruct = assetMain.ToObject<HookGeneratedStructure>();
		int mainsizeX = hookStruct.mainsizeX;
		int mainsizeZ = hookStruct.mainsizeZ;
		int minX = pos.X - mainsizeX / 2 - 2;
		int maxX = pos.X + mainsizeX / 2 + 2;
		int minZ = pos.Z - mainsizeZ / 2 - 2;
		int maxZ = pos.Z + mainsizeZ / 2 + 2;
		List<int> heights = new List<int>((maxX - minX + 1) * (maxZ - minZ + 1));
		int maxheight = 0;
		int minheight = int.MaxValue;
		int z;
		int x;
		for (x = minX; x <= maxX; x++)
		{
			for (z = minZ; z <= maxZ; z++)
			{
				mapchunk = blockAccessor.GetMapChunk(x / 32, z / 32);
				int h = mapchunk.WorldGenTerrainHeightMap[z % 32 * 32 + x % 32];
				heights.Add(h);
				maxheight = Math.Max(maxheight, h);
				minheight = Math.Min(minheight, h);
			}
		}
		x = Math.Max(mainsizeX, mainsizeZ);
		minX = pos.X - x / 2;
		maxX = pos.X + x / 2;
		minZ = pos.Z - x / 2;
		maxZ = pos.Z + x / 2;
		int weightedHeightW = 1;
		int weightedHeightE = 1;
		int weightedHeightN = 1;
		int weightedHeightS = 1;
		x = minX - 2;
		for (z = minZ; z <= maxZ; z++)
		{
			mapchunk = blockAccessor.GetMapChunk(x / 32, z / 32);
			int h2 = mapchunk.WorldGenTerrainHeightMap[z % 32 * 32 + x % 32];
			weightedHeightW += h2;
		}
		x = maxX + 2;
		for (z = minZ; z <= maxZ; z++)
		{
			mapchunk = blockAccessor.GetMapChunk(x / 32, z / 32);
			int h3 = mapchunk.WorldGenTerrainHeightMap[z % 32 * 32 + x % 32];
			weightedHeightE += h3;
		}
		z = minZ - 2;
		for (x = minX; x <= maxX; x++)
		{
			mapchunk = blockAccessor.GetMapChunk(x / 32, z / 32);
			int h4 = mapchunk.WorldGenTerrainHeightMap[z % 32 * 32 + x % 32];
			weightedHeightN += h4;
		}
		z = maxZ + 2;
		for (x = minX; x <= maxX; x++)
		{
			mapchunk = blockAccessor.GetMapChunk(x / 32, z / 32);
			int h5 = mapchunk.WorldGenTerrainHeightMap[z % 32 * 32 + x % 32];
			weightedHeightS += h5;
		}
		if (hookStruct.mainElements.Length != 0)
		{
			pos = pos.AddCopy(hookStruct.offsetX, hookStruct.offsetY, hookStruct.offsetZ);
			Vec3i[] offsets = new Vec3i[hookStruct.mainElements.Length];
			BlockSchematicStructure[] structures = new BlockSchematicStructure[hookStruct.mainElements.Length];
			int[] counts = new int[hookStruct.mainElements.Length];
			int[] maxCounts = new int[hookStruct.mainElements.Length];
			int structuresLength = 0;
			PathAndOffset[] mainElements = hookStruct.mainElements;
			foreach (PathAndOffset el in mainElements)
			{
				IAsset asset = api.Assets.TryGet(new AssetLocation(code.Domain, "worldgen/" + el.path + ".json"));
				if (asset == null)
				{
					api.Logger.Notification("Worldgen hook event elements: path not found: " + el.path);
					continue;
				}
				BlockSchematicStructure structure = asset.ToObject<BlockSchematicStructure>();
				structure.Init(blockAccessor);
				structures[structuresLength] = structure;
				maxCounts[structuresLength] = ((el.maxCount == 0) ? 16384 : el.maxCount);
				offsets[structuresLength++] = new Vec3i(el.dx, el.dy, el.dz);
			}
			Random rand = api.World.Rand;
			List<int> indices = new List<int>();
			List<int> bestIndices = new List<int>();
			int bestDiff = int.MaxValue;
			heights.Sort();
			int n = Math.Min(5, heights.Count);
			int height = 0;
			for (int j = 0; j < n; j++)
			{
				height += heights[j];
			}
			height = height / n + hookStruct.endOffsetY;
			if (maxheight - minheight < 5 && height - minheight < 2)
			{
				height++;
			}
			if (height < api.World.SeaLevel)
			{
				height = api.World.SeaLevel;
			}
			height = Math.Min(height, api.World.BlockAccessor.MapSizeY - 11);
			for (int k = 0; k < 25; k++)
			{
				indices.Clear();
				for (int l = 0; l < counts.Length; l++)
				{
					counts[l] = 0;
				}
				int testHeight = pos.Y;
				while (testHeight < height)
				{
					int i2 = rand.Next(structuresLength);
					if (counts[i2] >= maxCounts[i2])
					{
						continue;
					}
					int h6 = structures[i2].SizeY;
					if (testHeight + h6 > height)
					{
						if (testHeight + h6 - height > height - testHeight)
						{
							h6 = (height - testHeight) * 2;
						}
						else
						{
							indices.Add(i2);
							counts[i2]++;
						}
						int newDiff = testHeight + h6 - height;
						if (newDiff >= bestDiff)
						{
							break;
						}
						bestDiff = newDiff;
						bestIndices.Clear();
						foreach (int ix in indices)
						{
							bestIndices.Add(ix);
						}
						if (bestDiff == 0)
						{
							k = 25;
						}
						break;
					}
					indices.Add(i2);
					counts[i2]++;
					testHeight += h6;
				}
			}
			int posY = pos.Y;
			int entranceMinX = int.MaxValue;
			int entranceMinZ = int.MaxValue;
			int entranceMaxX = 0;
			int entranceMaxZ = 0;
			foreach (int ix2 in bestIndices)
			{
				BlockSchematicStructure struc = structures[ix2];
				Vec3i offset = offsets[ix2];
				BlockPos posPlace = pos.AddCopy(offset.X, offset.Y, offset.Z);
				struc.PlaceRespectingBlockLayers(blockAccessor, api.World, posPlace, 0, 0, 0, 0, null, Array.Empty<int>(), GlobalConfig.ReplaceMetaBlocks, replaceBlockEntities: true, suppressSoilIfAirBelow: false, displaceWater: true);
				pos.Y += struc.SizeY;
				entranceMinX = Math.Min(entranceMinX, posPlace.X);
				entranceMinZ = Math.Min(entranceMinZ, posPlace.Z);
				entranceMaxX = Math.Max(entranceMaxX, posPlace.X + struc.SizeX);
				entranceMaxZ = Math.Max(entranceMaxZ, posPlace.Z + struc.SizeY);
			}
			if (bestIndices.Count > 0)
			{
				IMapRegion mapRegion = blockAccessor.GetMapRegion(pos.X / blockAccessor.RegionSize, pos.Z / blockAccessor.RegionSize);
				Cuboidi location = new Cuboidi(entranceMinX, posY, entranceMinZ, entranceMaxX, pos.Y, entranceMaxZ);
				mapRegion.AddGeneratedStructure(new GeneratedStructure
				{
					Code = param,
					Group = hookStruct.group,
					Location = location.Clone()
				});
				if (hookStruct.buildProtected)
				{
					api.World.Claims.Add(new LandClaim
					{
						Areas = new List<Cuboidi> { location },
						Description = hookStruct.buildProtectionDesc,
						ProtectionLevel = hookStruct.ProtectionLevel,
						LastKnownOwnerName = hookStruct.buildProtectionName,
						AllowUseEveryone = hookStruct.AllowUseEveryone,
						AllowTraverseEveryone = hookStruct.AllowTraverseEveryone
					});
				}
			}
		}
		string topside = ((weightedHeightW < weightedHeightE) ? ((weightedHeightW >= weightedHeightN || weightedHeightW >= weightedHeightS) ? ((weightedHeightS < weightedHeightN) ? "s" : "n") : "w") : ((weightedHeightE >= weightedHeightN || weightedHeightE >= weightedHeightS) ? ((weightedHeightS < weightedHeightN) ? "s" : "n") : "e"));
		if (!hookStruct.endElements.TryGetValue(topside, out var endElement))
		{
			api.Logger.Notification("Worldgen hook event incomplete: no end structure for " + topside);
			return;
		}
		BlockSchematicStructure structTop = api.Assets.Get(new AssetLocation(code.Domain, endElement.path))?.ToObject<BlockSchematicStructure>();
		if (structTop == null)
		{
			api.Logger.Notification("Worldgen hook event incomplete: " + endElement.path + " not found");
			return;
		}
		int[] replaceblockids;
		if (hookStruct.ReplaceWithBlocklayers != null)
		{
			replaceblockids = new int[hookStruct.ReplaceWithBlocklayers.Length];
			for (int m = 0; m < replaceblockids.Length; m++)
			{
				Block block = api.World.GetBlock(hookStruct.ReplaceWithBlocklayers[m]);
				if (block == null)
				{
					api.Logger.Error($"Hook structure with code {code} has replace block layer {hookStruct.ReplaceWithBlocklayers[m]} defined, but no such block found!");
					return;
				}
				replaceblockids[m] = block.Id;
			}
		}
		else
		{
			replaceblockids = Array.Empty<int>();
		}
		IMapRegion mapRegion2 = mapchunk.MapRegion;
		IntDataMap2D climateMap = mapRegion2.ClimateMap;
		int regionChunkSize = api.WorldManager.RegionSize / 32;
		int rlX = pos.X / 32 % regionChunkSize;
		int rlZ = pos.Z / 32 % regionChunkSize;
		float facC = (float)climateMap.InnerSize / (float)regionChunkSize;
		int climateUpLeft = climateMap.GetUnpaddedInt((int)((float)rlX * facC), (int)((float)rlZ * facC));
		int climateUpRight = climateMap.GetUnpaddedInt((int)((float)rlX * facC + facC), (int)((float)rlZ * facC));
		int climateBotLeft = climateMap.GetUnpaddedInt((int)((float)rlX * facC), (int)((float)rlZ * facC + facC));
		int climateBotRight = climateMap.GetUnpaddedInt((int)((float)rlX * facC + facC), (int)((float)rlZ * facC + facC));
		structTop.blockLayerConfig = blockLayerConfig;
		structTop.Init(blockAccessor);
		pos.Add(endElement.dx, endElement.dy, endElement.dz);
		structTop.PlaceRespectingBlockLayers(blockAccessor, api.World, pos, climateUpLeft, climateUpRight, climateBotLeft, climateBotRight, null, replaceblockids, GlobalConfig.ReplaceMetaBlocks, replaceBlockEntities: true, suppressSoilIfAirBelow: true);
		Cuboidi locationEnd = new Cuboidi(pos.X, pos.Y, pos.Z, pos.X + structTop.SizeX, pos.Y + structTop.SizeY, pos.Z + structTop.SizeZ);
		mapRegion2.AddGeneratedStructure(new GeneratedStructure
		{
			Code = hookStruct.group,
			Group = hookStruct.group,
			Location = locationEnd.Clone()
		});
	}

	public void FinalizeRegeneration(int chunkMidX, int chunkMidZ)
	{
		api.ModLoader.GetModSystem<Timeswitch>().AttemptGeneration(worldgenBlockAccessor);
	}

	public bool PreventPlacementAt(int x, int z, int category)
	{
		if (category == storyPatchhashcode)
		{
			return false;
		}
		return StoryStructures.GetBlockingStructureAt(x, z, category, nearStructures) != null;
	}

	public bool PreventPlacementBroadlyAt(int chunkX, int chunkZ)
	{
		if (!genStoryStructures)
		{
			return false;
		}
		HorRectanglei rect = new HorRectanglei((chunkX - 1) * 32, (chunkZ - 1) * 32, (chunkX + 2) * 32, (chunkZ + 2) * 32);
		nearStructures.Clear();
		foreach (KeyValuePair<string, StoryStructureLocation> val in Structures)
		{
			Cuboidi loc = val.Value.Location;
			if (loc.Intersects(rect))
			{
				nearStructures.Add(val.Value);
			}
			else if (loc.FastCenter.DistanceSq(chunkX * 32, 0.0, chunkZ * 32) < (double)val.Value.MaxSkipGenerationRadiusSq)
			{
				nearStructures.Add(val.Value);
			}
		}
		if (nearStructures.Count > 0)
		{
			WorldGenStoryStructuresConfig stcfg = api.ModLoader.GetModSystem<GenStoryStructures>().scfg;
			if (storyStructurePatches == null)
			{
				storyStructurePatches = new Dictionary<string, BlockPatchConfig>();
				WorldGenStoryStructure[] structures = stcfg.Structures;
				foreach (WorldGenStoryStructure storyStructure in structures)
				{
					string path = "worldgen/story/" + storyStructure.Code + "/blockpatches/";
					List<KeyValuePair<AssetLocation, BlockPatch[]>> storyBlockPatches = (from b in api.Assets.GetMany<BlockPatch[]>(api.World.Logger, path)
						orderby b.Key.ToString()
						select b).ToList();
					if (storyBlockPatches == null || storyBlockPatches.Count <= 0)
					{
						continue;
					}
					List<BlockPatch> allLocationPatches = new List<BlockPatch>();
					foreach (KeyValuePair<AssetLocation, BlockPatch[]> val2 in storyBlockPatches)
					{
						BlockPatch[] value = val2.Value;
						for (int num = 0; num < value.Length; num++)
						{
							value[num].CategoryHashCode = storyPatchhashcode;
						}
						allLocationPatches.AddRange(val2.Value);
					}
					storyStructurePatches[storyStructure.Code] = new BlockPatchConfig
					{
						Patches = allLocationPatches.ToArray()
					};
					storyStructurePatches[storyStructure.Code].ResolveBlockIds(api, blockLayerConfig.RockStrata, grassRand);
				}
			}
		}
		return nearStructures.Count > 0;
	}

	public BlockPatchConfig GetPatchProviderAt(int chunkX, int chunkZ, ref EnumHandling handling)
	{
		if (!genStoryStructures)
		{
			return null;
		}
		int dx = grassRand.NextInt(32);
		int dz = grassRand.NextInt(32);
		IStructureLocation structure = GetIntersectingStructure(chunkX * 32 + dx, chunkZ * 32 + dz);
		if (structure != null && storyStructurePatches != null && storyStructurePatches.TryGetValue(structure.Code, out var blockPatchConfig))
		{
			handling = EnumHandling.PreventDefault;
			return blockPatchConfig;
		}
		return null;
	}
}
