using System.Collections.Generic;
using System.Linq;
using Vintagestory.API.Common;

namespace Vintagestory.GameContent;

public class MicroBlockSounds : BlockSounds
{
	public BlockEntityMicroBlock be;

	public Block defaultBlock;

	public override SoundAttributes Break
	{
		get
		{
			return block.Sounds.Break;
		}
		set
		{
		}
	}

	public override SoundAttributes Hit
	{
		get
		{
			return block.Sounds.Hit;
		}
		set
		{
		}
	}

	public override SoundAttributes Inside
	{
		get
		{
			return block.Sounds.Inside;
		}
		set
		{
		}
	}

	public override SoundAttributes Place
	{
		get
		{
			return block.Sounds.Place;
		}
		set
		{
		}
	}

	public override SoundAttributes Walk
	{
		get
		{
			return block.Sounds.Walk;
		}
		set
		{
		}
	}

	public override Dictionary<EnumTool, BlockSounds>? ByTool
	{
		get
		{
			return block.Sounds.ByTool;
		}
		set
		{
		}
	}

	private Block block
	{
		get
		{
			IList<Block> blocks = be.Api?.World.Blocks;
			if (blocks == null)
			{
				return defaultBlock;
			}
			if (!(defaultBlock is BlockChisel) && ((BlockMicroBlock)defaultBlock).IsSoilNonSoilMix(be))
			{
				return blocks[be.BlockIds.First((int blockid) => blocks[blockid].BlockMaterial == EnumBlockMaterial.Soil || blocks[blockid].BlockMaterial == EnumBlockMaterial.Gravel || blocks[blockid].BlockMaterial == EnumBlockMaterial.Sand)];
			}
			if (be?.BlockIds != null && be.BlockIds.Length != 0)
			{
				Block block = blocks[be.GetMajorityMaterialId()];
				if (block.Sounds != null)
				{
					return block;
				}
				return defaultBlock;
			}
			return defaultBlock;
		}
	}

	public void Init(BlockEntityMicroBlock be, Block defaultBlock)
	{
		this.be = be;
		this.defaultBlock = defaultBlock;
		Ambient = block.Sounds.Ambient;
	}
}
