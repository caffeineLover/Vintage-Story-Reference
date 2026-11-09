using System.Collections.Generic;
using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.Datastructures;
using Vintagestory.API.Server;

namespace Vintagestory.ServerMods;

public class ModBasicBlocksLoader : ModSystem
{
	private ICoreServerAPI api;

	public override bool ShouldLoad(EnumAppSide side)
	{
		return side == EnumAppSide.Server;
	}

	public override double ExecuteOrder()
	{
		return 0.1;
	}

	public override void Start(ICoreAPI manager)
	{
		if (manager is ICoreServerAPI sapi)
		{
			api = sapi;
			Block block = new Block();
			block.Code = new AssetLocation("mantle");
			block.Textures = new FastSmallDictionary<string, CompositeTexture>("all", new CompositeTexture(new AssetLocation("block/mantle")));
			block.DrawType = EnumDrawType.Cube;
			block.MatterState = EnumMatterState.Solid;
			block.BlockMaterial = EnumBlockMaterial.Mantle;
			block.Replaceable = 0;
			block.Resistance = 31337f;
			block.RequiredMiningTier = 196;
			block.Sounds = new BlockSounds
			{
				Walk = new SoundAttributes(new AssetLocation("sounds/walk/stone"), withRandomPitch: true),
				ByTool = new Dictionary<EnumTool, BlockSounds> { 
				{
					EnumTool.Pickaxe,
					new BlockSounds
					{
						Hit = new SoundAttributes(new AssetLocation("sounds/block/rock-hit-pickaxe"), withRandomPitch: true),
						Break = new SoundAttributes(new AssetLocation("sounds/block/rock-hit-pickaxe"), withRandomPitch: true)
					}
				} }
			};
			block.CreativeInventoryTabs = new string[1] { "general" };
			Block block2 = block;
			api.RegisterBlock(block2);
		}
	}
}
