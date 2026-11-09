using System;
using System.Text;
using Vintagestory.API.Common;
using Vintagestory.API.Config;
using Vintagestory.API.Datastructures;
using Vintagestory.API.MathTools;

namespace Vintagestory.GameContent;

public class BEBehaviorFruitingBushCutting : BlockEntityBehavior
{
	public double matureTotalDays;

	protected string? traits;

	public BEBehaviorFruitingBushCutting(BlockEntity blockentity)
		: base(blockentity)
	{
	}

	public override void Initialize(ICoreAPI api, JsonObject properties)
	{
		base.Initialize(api, properties);
		Blockentity.RegisterGameTickListener(OnMatureTick, 10000, api.World.Rand.Next(5000));
	}

	private void OnMatureTick(float dt)
	{
		if (Api.World.Calendar.TotalDays >= matureTotalDays)
		{
			Block block = Api.World.GetBlock(AssetLocation.Create(base.Block.Attributes["maturedBlockCode"].AsString(), base.Block.Code.Domain));
			if (block != null)
			{
				Api.World.BlockAccessor.SetBlock(block.BlockId, base.Pos);
				Api.World.BlockAccessor.GetBlockEntity(base.Pos).GetBehavior<BEBehaviorFruitingBush>()?.OnGrownFromCutting(traits);
			}
		}
	}

	public override void GetBlockInfo(IPlayer forPlayer, StringBuilder dsc)
	{
		dsc.AppendLine(Lang.Get("fruitingplant-cutting-growsindays", (int)Math.Ceiling(matureTotalDays - Api.World.Calendar.TotalDays)));
		if (traits != null)
		{
			BEBehaviorFruitingBush.addTraits(dsc, traits.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries));
		}
		base.GetBlockInfo(forPlayer, dsc);
	}

	public override void OnBlockPlaced(ItemStack? byItemStack = null)
	{
		double min = base.Block.Attributes["matureTotalMonthsMin"].AsDouble(2.0);
		double max = base.Block.Attributes["matureTotalMonthsMax"].AsDouble(4.0);
		matureTotalDays = Api.World.Calendar.TotalDays + (min + Api.World.Rand.NextDouble() * (max - min)) * (double)Api.World.Calendar.DaysPerMonth;
		traits = byItemStack?.Attributes.GetString("traits");
		base.OnBlockPlaced(byItemStack);
	}

	public override void FromTreeAttributes(ITreeAttribute tree, IWorldAccessor worldAccessForResolve)
	{
		base.FromTreeAttributes(tree, worldAccessForResolve);
		traits = tree.GetString("traits");
		matureTotalDays = tree.GetDouble("matureTotalDays");
	}

	public override void ToTreeAttributes(ITreeAttribute tree)
	{
		base.ToTreeAttributes(tree);
		tree.SetDouble("matureTotalDays", matureTotalDays);
		tree.SetString("traits", traits);
	}

	public ItemStack GetCuttingItemStack(BlockPos pos)
	{
		ItemStack cutting = new ItemStack(base.Block.GetSnowCoveredVariant(pos, 0f) ?? base.Block);
		if (traits != null)
		{
			cutting.Attributes.SetString("traits", traits);
		}
		return cutting;
	}
}
