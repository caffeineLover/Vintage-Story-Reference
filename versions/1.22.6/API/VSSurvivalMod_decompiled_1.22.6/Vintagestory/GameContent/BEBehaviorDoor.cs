using System;
using System.Collections.Generic;
using System.Text;
using Vintagestory.API;
using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.Config;
using Vintagestory.API.Datastructures;
using Vintagestory.API.MathTools;

namespace Vintagestory.GameContent;

[DocumentAsJson]
[AddDocumentationProperty("breakOnTriggerChance", "Affects the chance of door breaking and dropping itself upon using it", "System.Single", "Optional", "0", true)]
[AddDocumentationProperty("easingSpeed", "Affects animation speed of door opening or closing", "System.Single", "Optional", "10", true)]
public class BEBehaviorDoor : BEBehaviorAnimatable, IInteractable, IRotatable
{
	public float RotateYRad;

	protected bool opened;

	protected bool invertHandles;

	protected MeshData mesh;

	protected Cuboidf[] boxesClosed;

	protected Cuboidf[] boxesOpened;

	protected Vec3i leftDoorOffset;

	protected Vec3i rightDoorOffset;

	public BlockBehaviorDoor doorBh;

	public string StoryLockedCode;

	public BlockFacing facingWhenClosed => BlockFacing.HorizontalFromYaw(RotateYRad);

	public BlockFacing facingWhenOpened
	{
		get
		{
			if (!invertHandles)
			{
				return facingWhenClosed.GetCW();
			}
			return facingWhenClosed.GetCCW();
		}
	}

	public BEBehaviorDoor LeftDoor
	{
		get
		{
			if (leftDoorOffset != null)
			{
				BEBehaviorDoor doorAt = BlockBehaviorDoor.getDoorAt(Api.World, base.Pos.AddCopy(leftDoorOffset));
				if (doorAt == null)
				{
					leftDoorOffset = null;
				}
				return doorAt;
			}
			return null;
		}
		protected set
		{
			leftDoorOffset = value?.Pos.SubCopy(base.Pos).ToVec3i();
		}
	}

	public BEBehaviorDoor RightDoor
	{
		get
		{
			if (rightDoorOffset != null)
			{
				BEBehaviorDoor doorAt = BlockBehaviorDoor.getDoorAt(Api.World, base.Pos.AddCopy(rightDoorOffset));
				if (doorAt == null)
				{
					rightDoorOffset = null;
				}
				return doorAt;
			}
			return null;
		}
		protected set
		{
			rightDoorOffset = value?.Pos.SubCopy(base.Pos).ToVec3i();
		}
	}

	public Cuboidf[] ColSelBoxes
	{
		get
		{
			if (!opened)
			{
				return boxesClosed;
			}
			return boxesOpened;
		}
	}

	public bool Opened => opened;

	public bool InvertHandles => invertHandles;

	public BEBehaviorDoor(BlockEntity blockentity)
		: base(blockentity)
	{
		boxesClosed = base.Block.CollisionBoxes;
		doorBh = base.Block.GetBehavior<BlockBehaviorDoor>();
	}

	public override void Initialize(ICoreAPI api, JsonObject properties)
	{
		base.Initialize(api, properties);
		SetupRotationsAndColSelBoxes(initalSetup: false);
		if (opened && animUtil != null && !animUtil.activeAnimationsByAnimCode.ContainsKey("opened"))
		{
			ToggleDoorWing(opened: true);
		}
	}

	public Vec3i getAdjacentOffset(int right, int back = 0, int up = 0)
	{
		return getAdjacentOffset(right, back, up, RotateYRad, invertHandles);
	}

	public static Vec3i getAdjacentOffset(int right, int back, int up, float rotateYRad, bool invertHandles)
	{
		if (invertHandles)
		{
			right = -right;
		}
		return new Vec3i(right * (int)Math.Round(Math.Sin(rotateYRad + (float)Math.PI / 2f)) - back * (int)Math.Round(Math.Sin(rotateYRad)), up, right * (int)Math.Round(Math.Cos(rotateYRad + (float)Math.PI / 2f)) - back * (int)Math.Round(Math.Cos(rotateYRad)));
	}

	internal void SetupRotationsAndColSelBoxes(bool initalSetup)
	{
		if (initalSetup)
		{
			if (BlockBehaviorDoor.HasCombinableLeftDoor(Api.World, RotateYRad, base.Pos, doorBh.width, out var otherDoor, out var offset) && otherDoor.LeftDoor == null && otherDoor.RightDoor == null && otherDoor.facingWhenClosed == facingWhenClosed)
			{
				if (otherDoor.invertHandles)
				{
					if (otherDoor.doorBh.width > 1)
					{
						Api.World.BlockAccessor.SetBlock(0, otherDoor.Pos);
						BlockPos leftDoorPos = base.Pos.AddCopy(facingWhenClosed.GetCW(), otherDoor.doorBh.width + doorBh.width - 1);
						Api.World.BlockAccessor.SetBlock(otherDoor.Block.Id, leftDoorPos);
						otherDoor = base.Block.GetBEBehavior<BEBehaviorDoor>(leftDoorPos);
						otherDoor.RotateYRad = RotateYRad;
						otherDoor.doorBh.placeMultiblockParts(Api.World, leftDoorPos);
						LeftDoor = otherDoor;
						LeftDoor.RightDoor = this;
						LeftDoor.SetupRotationsAndColSelBoxes(initalSetup: true);
					}
					else
					{
						otherDoor.invertHandles = false;
						LeftDoor = otherDoor;
						LeftDoor.RightDoor = this;
						LeftDoor.Blockentity.MarkDirty(redrawOnClient: true);
						LeftDoor.SetupRotationsAndColSelBoxes(initalSetup: false);
					}
				}
				else
				{
					LeftDoor = otherDoor;
					LeftDoor.RightDoor = this;
				}
				invertHandles = true;
				Blockentity.MarkDirty(redrawOnClient: true);
			}
			if (BlockBehaviorDoor.HasCombinableRightDoor(Api.World, RotateYRad, base.Pos, doorBh.width, out otherDoor, out offset) && otherDoor.LeftDoor == null && otherDoor.RightDoor == null && otherDoor.facingWhenClosed == facingWhenClosed && Api.Side == EnumAppSide.Server)
			{
				if (!otherDoor.invertHandles)
				{
					if (otherDoor.doorBh.width > 1)
					{
						Api.World.BlockAccessor.SetBlock(0, otherDoor.Pos);
						BlockPos rightDoorPos = base.Pos.AddCopy(facingWhenClosed.GetCCW(), otherDoor.doorBh.width + doorBh.width - 1);
						Api.World.BlockAccessor.SetBlock(otherDoor.Block.Id, rightDoorPos);
						otherDoor = base.Block.GetBEBehavior<BEBehaviorDoor>(rightDoorPos);
						otherDoor.RotateYRad = RotateYRad;
						otherDoor.invertHandles = true;
						otherDoor.doorBh.placeMultiblockParts(Api.World, rightDoorPos);
						RightDoor = otherDoor;
						RightDoor.LeftDoor = this;
						otherDoor.SetupRotationsAndColSelBoxes(initalSetup: true);
					}
					else
					{
						otherDoor.invertHandles = true;
						RightDoor = otherDoor;
						RightDoor.LeftDoor = this;
						RightDoor.Blockentity.MarkDirty(redrawOnClient: true);
						RightDoor.SetupRotationsAndColSelBoxes(initalSetup: false);
					}
				}
				else
				{
					RightDoor = otherDoor;
					RightDoor.LeftDoor = this;
				}
			}
		}
		if (Api.Side == EnumAppSide.Client)
		{
			if (doorBh.animatableOrigMesh == null)
			{
				string animkey = base.Block.Shape.ToString();
				doorBh.animatableOrigMesh = animUtil.CreateMesh(animkey, null, out var shape, null);
				doorBh.animatableShape = shape;
				doorBh.animatableDictKey = animkey;
			}
			if (doorBh.animatableOrigMesh != null)
			{
				animUtil.InitializeAnimator(doorBh.animatableDictKey, doorBh.animatableOrigMesh, doorBh.animatableShape, null);
			}
		}
		UpdateHitBoxes();
	}

	protected virtual void UpdateMeshAndAnimations()
	{
		mesh = doorBh.animatableOrigMesh.Clone();
		if (RotateYRad != 0f)
		{
			float rot = (invertHandles ? (0f - RotateYRad) : RotateYRad);
			mesh = mesh.Rotate(0f, rot, 0f);
			animUtil.renderer.rotationDeg.Y = rot * (180f / (float)Math.PI);
		}
		if (invertHandles)
		{
			Matrixf matf = new Matrixf();
			matf.Translate(0.5f, 0.5f, 0.5f).Scale(-1f, 1f, 1f).Translate(-0.5f, -0.5f, -0.5f);
			mesh.MatrixTransform(matf.Values);
			animUtil.renderer.backfaceCulling = false;
			animUtil.renderer.ScaleX = -1f;
		}
	}

	protected virtual void UpdateHitBoxes()
	{
		if (RotateYRad != 0f)
		{
			boxesClosed = base.Block.CollisionBoxes;
			Cuboidf[] boxes = new Cuboidf[boxesClosed.Length];
			for (int i = 0; i < boxesClosed.Length; i++)
			{
				boxes[i] = boxesClosed[i].RotatedCopy(0f, RotateYRad * (180f / (float)Math.PI), 0f, new Vec3d(0.5, 0.5, 0.5));
			}
			boxesClosed = boxes;
		}
		Cuboidf[] boxesopened = new Cuboidf[boxesClosed.Length];
		for (int j = 0; j < boxesClosed.Length; j++)
		{
			boxesopened[j] = boxesClosed[j].RotatedCopy(0f, invertHandles ? 90 : (-90), 0f, new Vec3d(0.5, 0.5, 0.5));
		}
		boxesOpened = boxesopened;
	}

	public virtual void OnBlockPlaced(ItemStack byItemStack, IPlayer byPlayer, BlockSelection blockSel)
	{
		if (byItemStack != null)
		{
			RotateYRad = getRotateYRad(byPlayer, blockSel);
			SetupRotationsAndColSelBoxes(initalSetup: true);
		}
	}

	public static float getRotateYRad(IPlayer byPlayer, BlockSelection blockSel)
	{
		BlockPos targetPos = (blockSel.DidOffset ? blockSel.Position.AddCopy(blockSel.Face.Opposite) : blockSel.Position);
		double y = byPlayer.Entity.Pos.X - ((double)targetPos.X + blockSel.HitPosition.X);
		double dz = (double)(float)byPlayer.Entity.Pos.Z - ((double)targetPos.Z + blockSel.HitPosition.Z);
		float num = (float)Math.Atan2(y, dz);
		float deg90 = (float)Math.PI / 2f;
		return (float)(int)Math.Round(num / deg90) * deg90;
	}

	public bool IsSideSolid(BlockFacing facing)
	{
		if (opened || facing != facingWhenClosed)
		{
			if (opened)
			{
				return facing == facingWhenOpened;
			}
			return false;
		}
		return true;
	}

	public bool OnBlockInteractStart(IWorldAccessor world, IPlayer byPlayer, BlockSelection blockSel, ref EnumHandling handling)
	{
		if (!doorBh.handopenable && byPlayer.WorldData.CurrentGameMode != EnumGameMode.Creative)
		{
			(Api as ICoreClientAPI).TriggerIngameError(this, "nothandopenable", Lang.Get("This door cannot be opened by hand."));
			return true;
		}
		ToggleDoorState(byPlayer, !opened);
		handling = EnumHandling.PreventDefault;
		return true;
	}

	public void ToggleDoorState(IPlayer byPlayer, bool opened)
	{
		float breakChance = base.Block.Attributes["breakOnTriggerChance"].AsFloat();
		if (Api.Side == EnumAppSide.Server && Api.World.Rand.NextDouble() < (double)breakChance && byPlayer.WorldData.CurrentGameMode != EnumGameMode.Creative)
		{
			Api.World.BlockAccessor.BreakBlock(base.Pos, byPlayer);
			Api.World.PlaySoundAt(new AssetLocation("sounds/effect/toolbreak"), base.Pos, 0.0);
			return;
		}
		this.opened = opened;
		ToggleDoorWing(opened);
		float pitch = (opened ? 1.1f : 0.9f);
		AssetLocation sound = ((!opened) ? doorBh?.CloseSound : doorBh?.OpenSound);
		Api.World.PlaySoundAt(sound, (float)base.Pos.X + 0.5f, (float)base.Pos.InternalY + 0.5f, (float)base.Pos.Z + 0.5f, byPlayer, EnumSoundType.Sound, pitch);
		if (LeftDoor != null && invertHandles)
		{
			LeftDoor.ToggleDoorWing(opened);
			LeftDoor.UpdateNeighbors();
		}
		else if (RightDoor != null)
		{
			RightDoor.ToggleDoorWing(opened);
			RightDoor.UpdateNeighbors();
		}
		Blockentity.MarkDirty(redrawOnClient: true);
		UpdateNeighbors();
	}

	private void UpdateNeighbors()
	{
		if (Api.Side != EnumAppSide.Server)
		{
			return;
		}
		BlockPos tempPos = new BlockPos(base.Pos.dimension);
		for (int y = 0; y < doorBh.height; y++)
		{
			tempPos.Set(base.Pos).Add(0, y, 0);
			BlockFacing sideMove = BlockFacing.ALLFACES[Opened ? facingWhenClosed.HorizontalAngleIndex : facingWhenOpened.HorizontalAngleIndex];
			for (int x = 0; x < doorBh.width; x++)
			{
				Api.World.BlockAccessor.TriggerNeighbourBlockUpdate(tempPos);
				tempPos.Add(sideMove);
			}
		}
	}

	private void ToggleDoorWing(bool opened)
	{
		this.opened = opened;
		if (!opened)
		{
			animUtil.StopAnimation("opened");
		}
		else
		{
			float easingSpeed = base.Block.Attributes?["easingSpeed"].AsFloat(10f) ?? 10f;
			animUtil.StartAnimation(new AnimationMetaData
			{
				Animation = "opened",
				Code = "opened",
				EaseInSpeed = easingSpeed,
				EaseOutSpeed = easingSpeed
			});
		}
		Blockentity.MarkDirty();
	}

	public override bool OnTesselation(ITerrainMeshPool mesher, ITesselatorAPI tessThreadTesselator)
	{
		if (mesh == null)
		{
			UpdateMeshAndAnimations();
		}
		if (!base.OnTesselation(mesher, tessThreadTesselator))
		{
			mesher.AddMeshData(mesh);
		}
		return true;
	}

	public override void FromTreeAttributes(ITreeAttribute tree, IWorldAccessor worldAccessForResolve)
	{
		base.FromTreeAttributes(tree, worldAccessForResolve);
		bool beforeOpened = opened;
		RotateYRad = tree.GetFloat("rotateYRad");
		opened = tree.GetBool("opened");
		invertHandles = tree.GetBool("invertHandles");
		leftDoorOffset = tree.GetVec3i("leftDoorPos");
		rightDoorOffset = tree.GetVec3i("rightDoorPos");
		StoryLockedCode = tree.GetString("storyLockedCode");
		if (opened != beforeOpened && animUtil != null)
		{
			ToggleDoorWing(opened);
		}
		if (Api != null && Api.Side == EnumAppSide.Client)
		{
			UpdateMeshAndAnimations();
			if (opened && !beforeOpened && animUtil != null && !animUtil.activeAnimationsByAnimCode.ContainsKey("opened"))
			{
				ToggleDoorWing(opened: true);
			}
			UpdateHitBoxes();
			Api.World.BlockAccessor.MarkBlockDirty(base.Pos);
		}
	}

	public override void ToTreeAttributes(ITreeAttribute tree)
	{
		base.ToTreeAttributes(tree);
		tree.SetFloat("rotateYRad", RotateYRad);
		tree.SetBool("opened", opened);
		tree.SetBool("invertHandles", invertHandles);
		if (StoryLockedCode != null)
		{
			tree.SetString("storyLockedCode", StoryLockedCode);
		}
		if (leftDoorOffset != null)
		{
			tree.SetVec3i("leftDoorPos", leftDoorOffset);
		}
		if (rightDoorOffset != null)
		{
			tree.SetVec3i("rightDoorPos", rightDoorOffset);
		}
	}

	public override void GetBlockInfo(IPlayer forPlayer, StringBuilder dsc)
	{
		if (Api is ICoreClientAPI capi && capi.Settings.Bool["extendedDebugInfo"])
		{
			dsc.AppendLine(facingWhenClosed?.ToString() + (invertHandles ? "-inv " : " ") + (opened ? "open" : "closed"));
			dsc.AppendLine(doorBh.height + "x" + doorBh.width + ((leftDoorOffset != null) ? (" leftdoor at:" + leftDoorOffset) : " ") + ((rightDoorOffset != null) ? (" rightdoor at:" + rightDoorOffset) : " "));
			EnumHandling h = EnumHandling.PassThrough;
			if (doorBh.GetLiquidBarrierHeightOnSide(BlockFacing.NORTH, base.Pos, ref h) > 0f)
			{
				dsc.AppendLine("Barrier to liquid on side: North");
			}
			if (doorBh.GetLiquidBarrierHeightOnSide(BlockFacing.EAST, base.Pos, ref h) > 0f)
			{
				dsc.AppendLine("Barrier to liquid on side: East");
			}
			if (doorBh.GetLiquidBarrierHeightOnSide(BlockFacing.SOUTH, base.Pos, ref h) > 0f)
			{
				dsc.AppendLine("Barrier to liquid on side: South");
			}
			if (doorBh.GetLiquidBarrierHeightOnSide(BlockFacing.WEST, base.Pos, ref h) > 0f)
			{
				dsc.AppendLine("Barrier to liquid on side: West");
			}
		}
	}

	public void OnTransformed(IWorldAccessor worldAccessor, ITreeAttribute tree, int degreeRotation, Dictionary<int, AssetLocation> oldBlockIdMapping, Dictionary<int, AssetLocation> oldItemIdMapping, EnumAxis? flipAxis)
	{
		RotateYRad = tree.GetFloat("rotateYRad");
		RotateYRad = (RotateYRad - (float)degreeRotation * ((float)Math.PI / 180f)) % ((float)Math.PI * 2f);
		tree.SetFloat("rotateYRad", RotateYRad);
	}
}
