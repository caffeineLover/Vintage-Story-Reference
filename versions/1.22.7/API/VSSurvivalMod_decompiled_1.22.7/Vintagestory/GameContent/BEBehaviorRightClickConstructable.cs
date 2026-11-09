using System;
using System.Text;
using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.Datastructures;

namespace Vintagestory.GameContent;

public class BEBehaviorRightClickConstructable : BlockEntityBehavior, IInteractableWithHelp, IInteractable
{
	protected RightClickConstruction rcc;

	public Vintagestory.API.Common.Func<IPlayer, BlockSelection, bool> OnAttemptConstruct;

	protected float brokenDropsRatio = 1f;

	public CompositeShape shape { get; protected set; }

	public bool IsComplete => rcc.CurrentCompletedStage == rcc.Stages.Length - 1;

	public int CurrentCompletedStage => rcc.CurrentCompletedStage;

	public int Stages => rcc.Stages.Length - 1;

	public event Action<CompositeShape> OnShapeChanged;

	public BEBehaviorRightClickConstructable(BlockEntity blockentity)
		: base(blockentity)
	{
		rcc = new RightClickConstruction();
		shape = blockentity.Block.Shape;
	}

	public override void Initialize(ICoreAPI api, JsonObject properties)
	{
		base.Initialize(api, properties);
		brokenDropsRatio = properties["brokenDropsRatio"].AsFloat(1f);
		ConstructionStage[] stages = properties["stages"].AsObject<ConstructionStage[]>();
		rcc.LateInit(stages, api, base.Pos.ToVec3d, "Block " + base.Block.Code);
		updateShape();
	}

	public bool OnBlockInteractStart(IWorldAccessor world, IPlayer byPlayer, BlockSelection blockSel, ref EnumHandling handling)
	{
		handling = EnumHandling.PreventDefault;
		if (OnAttemptConstruct != null && !OnAttemptConstruct(byPlayer, blockSel))
		{
			return false;
		}
		if (rcc.OnInteract(byPlayer.Entity, byPlayer.Entity.RightHandItemSlot))
		{
			updateShape();
			Blockentity.MarkDirty(redrawOnClient: true);
		}
		return true;
	}

	public override void FromTreeAttributes(ITreeAttribute tree, IWorldAccessor worldAccessForResolve)
	{
		base.FromTreeAttributes(tree, worldAccessForResolve);
		rcc.FromTreeAttributes(tree);
	}

	public override void ToTreeAttributes(ITreeAttribute tree)
	{
		rcc.ToTreeAttributes(tree);
		base.ToTreeAttributes(tree);
		updateShape();
	}

	public override bool OnTesselation(ITerrainMeshPool mesher, ITesselatorAPI tessThreadTesselator)
	{
		return true;
	}

	public override void GetBlockInfo(IPlayer forPlayer, StringBuilder dsc)
	{
		base.GetBlockInfo(forPlayer, dsc);
		if (Api.World.EntityDebugMode && forPlayer != null && forPlayer.WorldData?.CurrentGameMode == EnumGameMode.Creative)
		{
			dsc.AppendLine("<font color='#ccc'>construction stage= " + rcc.CurrentCompletedStage + " of " + rcc.Stages.Length + "</font>");
		}
	}

	public override void OnBlockBroken(IPlayer byPlayer = null)
	{
		if (byPlayer.WorldData.CurrentGameMode != EnumGameMode.Creative)
		{
			ItemStack[] drops = rcc.GetDrops(brokenDropsRatio, Api.World.Rand);
			foreach (ItemStack drop in drops)
			{
				Api.World.SpawnItemEntity(drop, base.Pos);
			}
		}
	}

	protected void updateShape()
	{
		shape = new CompositeShape
		{
			Base = base.Block.Shape.Base,
			rotateY = base.Block.Shape.rotateY,
			SelectiveElements = rcc.getShapeElements()
		};
		this.OnShapeChanged?.Invoke(shape);
	}

	public WorldInteraction[] GetPlacedBlockInteractionHelp(IWorldAccessor world, BlockSelection selection, IPlayer forPlayer, ref EnumHandling handling)
	{
		return rcc.GetInteractionHelp(world, forPlayer);
	}
}
