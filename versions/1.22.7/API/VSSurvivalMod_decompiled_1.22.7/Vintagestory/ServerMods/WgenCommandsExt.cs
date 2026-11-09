using System;
using System.Linq;
using System.Text;
using Vintagestory.API.Common;
using Vintagestory.API.Config;
using Vintagestory.API.Datastructures;
using Vintagestory.API.MathTools;
using Vintagestory.API.Server;
using Vintagestory.ServerMods.NoObf;

namespace Vintagestory.ServerMods;

public class WgenCommandsExt : ModSystem
{
	private ICoreServerAPI api;

	private int _regionSize;

	private int _chunksize;

	private int _regionChunkSize;

	private WorldGenStructuresConfig _scfg;

	public override bool ShouldLoad(EnumAppSide side)
	{
		return side == EnumAppSide.Server;
	}

	public override double ExecuteOrder()
	{
		return 0.33;
	}

	public override void StartServerSide(ICoreServerAPI api)
	{
		this.api = api;
		api.Event.SaveGameLoaded += OnGameWorldLoaded;
		if (this.api.Server.CurrentRunPhase == EnumServerRunPhase.RunGame)
		{
			OnGameWorldLoaded();
		}
		if (TerraGenConfig.DoDecorationPass)
		{
			api.Event.InitWorldGenerator(InitWorldGen, "standard");
		}
		CreateCommands();
	}

	private void OnGameWorldLoaded()
	{
		_regionSize = api.WorldManager.RegionSize;
	}

	private void CreateCommands()
	{
		_ = api.ChatCommands.Parsers;
		api.ChatCommands.GetOrCreate("wgen").BeginSubCommand("testvillage").WithDescription("Testvillage command")
			.RequiresPlayer()
			.HandleWith(OnCmdTestVillage)
			.EndSubCommand();
	}

	private void InitWorldGen()
	{
		_chunksize = 32;
		_regionChunkSize = api.WorldManager.RegionSize / _chunksize;
		_scfg = api.ModLoader.GetModSystem<GenStructures>().scfg;
		CommandArgumentParsers parsers = api.ChatCommands.Parsers;
		api.ChatCommands.GetOrCreate("wgen").BeginSubCommand("structures").BeginSubCommand("spawn")
			.RequiresPlayer()
			.WithDescription("Spawn a structure from structure.json like during worldgen. Target position will be the selected block or your position. See /dev list [num] command to get the correct index.")
			.WithArgs(parsers.Int("structure_index"), parsers.OptionalInt("schematic_index"), parsers.OptionalIntRange("rotation_index", 0, 3))
			.HandleWith(OnStructuresSpawn)
			.EndSubCommand()
			.BeginSubCommand("list")
			.WithDescription("List structures with their indices for the /dev structure spawn command")
			.WithArgs(parsers.OptionalInt("structure_num"))
			.HandleWith(OnStructuresList)
			.EndSubCommand()
			.EndSubCommand();
	}

	private TextCommandResult OnCmdTestVillage(TextCommandCallingArgs args)
	{
		if (api.Server.Config.HostedMode)
		{
			return TextCommandResult.Success(Lang.Get("Can't access this feature, server is in hosted mode"));
		}
		api.Assets.Reload(AssetCategory.worldgen);
		GenStructures ms = api.ModLoader.GetModSystem<GenStructures>();
		ms.initWorldGen();
		Vec3d pos = args.Caller.Pos;
		int chunkx = (int)pos.X / 32;
		int chunkz = (int)pos.Z / 32;
		IMapRegion mr = api.World.BlockAccessor.GetMapRegion((int)pos.X / _regionSize, (int)pos.Z / _regionSize);
		for (int i = 0; i < 50; i++)
		{
			int len = ms.vcfg.VillageTypes.Length;
			WorldGenVillage struc = ms.vcfg.VillageTypes[api.World.Rand.Next(len)];
			if (ms.GenVillage(api.World.BlockAccessor, mr, struc, chunkx, chunkz))
			{
				return TextCommandResult.Success($"Generated after {i + 1} tries");
			}
		}
		return TextCommandResult.Error("Unable to generate, likely not flat enough here.");
	}

	private TextCommandResult OnStructuresList(TextCommandCallingArgs args)
	{
		StringBuilder sb = new StringBuilder();
		if (args.Parsers[0].IsMissing)
		{
			for (int i = 0; i < _scfg.Structures.Length; i++)
			{
				WorldGenStructure structure = _scfg.Structures[i];
				string domain = structure.Schematics.FirstOrDefault()?.Domain;
				StringBuilder stringBuilder = sb;
				StringBuilder stringBuilder2 = stringBuilder;
				StringBuilder.AppendInterpolatedStringHandler handler = new StringBuilder.AppendInterpolatedStringHandler(28, 5, stringBuilder);
				handler.AppendFormatted(i);
				handler.AppendLiteral(": Name: ");
				handler.AppendFormatted(structure.Name);
				handler.AppendLiteral(" - Code: ");
				handler.AppendFormatted(domain);
				handler.AppendLiteral(":");
				handler.AppendFormatted(structure.Code);
				handler.AppendLiteral(" - Group: ");
				handler.AppendFormatted(structure.Group);
				stringBuilder2.AppendLine(ref handler);
				stringBuilder = sb;
				StringBuilder stringBuilder3 = stringBuilder;
				handler = new StringBuilder.AppendInterpolatedStringHandler(28, 2, stringBuilder);
				handler.AppendLiteral("     YOff: ");
				handler.AppendFormatted(structure.OffsetY);
				handler.AppendLiteral(" - MinGroupDist: ");
				handler.AppendFormatted(structure.MinGroupDistance);
				stringBuilder3.AppendLine(ref handler);
			}
			return TextCommandResult.Success(sb.ToString());
		}
		int structureNum = (int)args[0];
		if (structureNum < 0 || structureNum >= _scfg.Structures.Length)
		{
			return TextCommandResult.Success($"structureNum is out of range: 0-{_scfg.Structures.Length - 1}");
		}
		WorldGenStructure structures = _scfg.Structures[structureNum];
		for (int j = 0; j < structures.schematicDatas.Length; j++)
		{
			BlockSchematicStructure[] schematic = structures.schematicDatas[j];
			StringBuilder stringBuilder = sb;
			StringBuilder stringBuilder4 = stringBuilder;
			StringBuilder.AppendInterpolatedStringHandler handler = new StringBuilder.AppendInterpolatedStringHandler(8, 2, stringBuilder);
			handler.AppendFormatted(j);
			handler.AppendLiteral(": File: ");
			handler.AppendFormatted(schematic[0].FromFile);
			stringBuilder4.AppendLine(ref handler);
		}
		return TextCommandResult.Success(sb.ToString());
	}

	private TextCommandResult OnStructuresSpawn(TextCommandCallingArgs args)
	{
		int structureNum = (int)args[0];
		int schematicNum = (int)args[1];
		int schematicRot = (int)args[2];
		if (structureNum < 0 || structureNum >= _scfg.Structures.Length)
		{
			return TextCommandResult.Success($"structureNum is out of range: 0-{_scfg.Structures.Length - 1}");
		}
		WorldGenStructure struc = _scfg.Structures[structureNum];
		if (schematicNum < 0 || schematicNum >= struc.schematicDatas.Length)
		{
			return TextCommandResult.Success($"schematicNum is out of range: 0-{struc.schematicDatas.Length - 1}");
		}
		BlockPos pos = args.Caller.Player.CurrentBlockSelection?.Position.AddCopy(0, struc.OffsetY.GetValueOrDefault(), 0) ?? args.Caller.Pos.AsBlockPos.AddCopy(0, struc.OffsetY.GetValueOrDefault(), 0);
		BlockSchematicStructure schematic = struc.schematicDatas[schematicNum][schematicRot];
		schematic.Unpack(api, schematicRot);
		int chunkX = pos.X / _chunksize;
		int chunkZ = pos.Z / _chunksize;
		int chunkY = pos.Y / _chunksize;
		switch (struc.Placement)
		{
		case EnumStructurePlacement.SurfaceRuin:
		case EnumStructurePlacement.Surface:
		{
			IntDataMap2D climateMap = api.WorldManager.GetChunk(chunkX, chunkY, chunkZ).MapChunk.MapRegion.ClimateMap;
			int rlX = chunkX % _regionChunkSize;
			int rlZ = chunkZ % _regionChunkSize;
			float facC = (float)climateMap.InnerSize / (float)_regionChunkSize;
			int climateUpLeft = climateMap.GetUnpaddedInt((int)((float)rlX * facC), (int)((float)rlZ * facC));
			int climateUpRight = climateMap.GetUnpaddedInt((int)((float)rlX * facC + facC), (int)((float)rlZ * facC));
			int climateBotLeft = climateMap.GetUnpaddedInt((int)((float)rlX * facC), (int)((float)rlZ * facC + facC));
			int climateBotRight = climateMap.GetUnpaddedInt((int)((float)rlX * facC + facC), (int)((float)rlZ * facC + facC));
			schematic.PlaceRespectingBlockLayers(api.World.BlockAccessor, api.World, pos, climateUpLeft, climateUpRight, climateBotLeft, climateBotRight, struc.resolvedRockTypeRemaps, struc.replacewithblocklayersBlockids, GlobalConfig.ReplaceMetaBlocks);
			break;
		}
		case EnumStructurePlacement.Underground:
			if (struc.resolvedRockTypeRemaps != null)
			{
				schematic.PlaceReplacingBlocks(api.World.BlockAccessor, api.World, pos, schematic.ReplaceMode, struc.resolvedRockTypeRemaps, null, GlobalConfig.ReplaceMetaBlocks);
			}
			else
			{
				schematic.Place(api.World.BlockAccessor, api.World, pos, GlobalConfig.ReplaceMetaBlocks);
			}
			break;
		default:
			throw new ArgumentOutOfRangeException();
		case EnumStructurePlacement.Underwater:
			break;
		}
		return TextCommandResult.Success($"placing structure: {struc.Name} :: {schematic.FromFile} placement: {struc.Placement}");
	}
}
