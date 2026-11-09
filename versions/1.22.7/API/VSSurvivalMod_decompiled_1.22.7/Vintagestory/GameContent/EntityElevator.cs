using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.Common.Entities;
using Vintagestory.API.MathTools;
using Vintagestory.API.Server;

namespace Vintagestory.GameContent;

public class EntityElevator : Entity, ISeatInstSupplier, IMountableListener, ICustomInteractionHelpPositioning
{
	private double swimmingOffsetY;

	public float SpeedMultiplier = 1.5f;

	public Dictionary<string, string> MountAnimations = new Dictionary<string, string>();

	private ICoreClientAPI capi;

	private ICoreServerAPI sapi;

	public ILoadedSound travelSound;

	public ILoadedSound latchSound;

	private float accum;

	private bool isMovingUp;

	private bool isMovingDown;

	private int colliderBlockId;

	private int lastStopIndex;

	private int currentHeight = -1;

	public ElevatorSystem? ElevatorSys;

	private int CurrentStopIndex;

	private const string UpAp = "UpAP";

	private const string DownAp = "DownAP";

	public override double FrustumSphereRadius => base.FrustumSphereRadius * 2.0;

	public override bool IsCreature => true;

	public override bool ApplyGravity => false;

	public override bool IsInteractable => true;

	public override float MaterialDensity => 100f;

	public override double SwimmingOffsetY => swimmingOffsetY;

	public bool TransparentCenter => true;

	public string? NetworkCode
	{
		get
		{
			return Attributes.GetString("networkCode");
		}
		set
		{
			Attributes.SetString("networkCode", value);
		}
	}

	public bool IsMoving
	{
		get
		{
			if (!isMovingUp)
			{
				return isMovingDown;
			}
			return true;
		}
	}

	public override void Initialize(EntityProperties properties, ICoreAPI api, long InChunkIndex3d)
	{
		swimmingOffsetY = properties.Attributes["swimmingOffsetY"].AsDouble();
		MountAnimations = properties.Attributes?["mountAnimations"].AsObject<Dictionary<string, string>>() ?? new Dictionary<string, string>();
		base.Initialize(properties, api, InChunkIndex3d);
		if (api is ICoreClientAPI clientApi)
		{
			capi = clientApi;
			travelSound = capi.World.LoadSound(new SoundParams
			{
				Location = new AssetLocation("sounds/effect/gearbox_turn.ogg"),
				ShouldLoop = true,
				RelativePosition = false,
				DisposeOnFinish = false,
				Volume = 0.15f
			});
			latchSound = capi.World.LoadSound(new SoundParams
			{
				Location = new AssetLocation("sounds/effect/latch.ogg"),
				ShouldLoop = false,
				RelativePosition = false,
				DisposeOnFinish = false
			});
		}
		if (!(api is ICoreServerAPI serverApi))
		{
			return;
		}
		sapi = serverApi;
		ModsystemElevator elevatorModSystem = sapi.ModLoader.GetModSystem<ModsystemElevator>();
		if (NetworkCode != null)
		{
			ElevatorSys = elevatorModSystem.RegisterElevator(NetworkCode, this);
			if (Attributes.GetBool("isActivated") && !Attributes.HasAttribute("maxHeight"))
			{
				ElevatorSystem? elevatorSys = ElevatorSys;
				if (elevatorSys != null && elevatorSys.ControlPositions.Count > 0)
				{
					Attributes.SetInt("maxHeight", ElevatorSys.ControlPositions.Last());
				}
			}
		}
		colliderBlockId = sapi.World.BlockAccessor.GetBlock("meta-collider").Id;
		currentHeight = (int)base.Pos.Y;
	}

	public override void OnGameTick(float dt)
	{
		if (World.Side == EnumAppSide.Server)
		{
			updatePosition(dt);
		}
		if (World.Side == EnumAppSide.Client)
		{
			bool movingUp = WatchedAttributes.GetBool("isMovingUp");
			bool movingDown = WatchedAttributes.GetBool("isMovingDown");
			if (!movingUp && !movingDown && IsMoving)
			{
				StopAnimation("gearturndown");
				StopAnimation("gearturnup");
			}
			isMovingUp = movingUp;
			isMovingDown = movingDown;
			NowInMotion(dt);
		}
		base.OnGameTick(dt);
	}

	protected virtual void updatePosition(float dt)
	{
		dt = Math.Min(0.5f, dt);
		if (ElevatorSys == null || ElevatorSys.ControlPositions.Count == 0)
		{
			return;
		}
		int elevatorStopHeight = ElevatorSys.ControlPositions[CurrentStopIndex];
		double diff = Math.Abs(base.Pos.Y - (double)elevatorStopHeight);
		if (diff >= 0.019999999552965164)
		{
			if (isMovingDown && currentHeight != (int)base.Pos.Y)
			{
				SetGround(base.Pos.AsBlockPos, (int)base.Pos.Y);
				UnSetGround(base.Pos.AsBlockPos, currentHeight);
				currentHeight = (int)base.Pos.Y;
			}
			else if (!IsMoving)
			{
				UnSetGround(base.Pos.AsBlockPos, ElevatorSys.ControlPositions[lastStopIndex]);
			}
			double mul = Math.Max(0.5, Math.Clamp(diff, 0.0, 1.0));
			if (base.Pos.Y < (double)elevatorStopHeight)
			{
				base.Pos.Y += (double)(dt * SpeedMultiplier) * mul;
				isMovingUp = true;
			}
			else
			{
				base.Pos.Y -= (double)(dt * SpeedMultiplier) * mul;
				isMovingDown = true;
			}
		}
		else
		{
			if (IsMoving)
			{
				UnSetGround(base.Pos.AsBlockPos, currentHeight);
				currentHeight = elevatorStopHeight;
				SetGround(base.Pos.AsBlockPos, currentHeight);
			}
			isMovingUp = (isMovingDown = false);
		}
		WatchedAttributes.SetBool("isMovingUp", isMovingUp);
		WatchedAttributes.SetBool("isMovingDown", isMovingDown);
	}

	private void SetGround(BlockPos pos, int elevatorStopHeight)
	{
		BlockPos tmpPos = pos.Copy();
		tmpPos.Y = elevatorStopHeight;
		for (int x = -1; x < 2; x++)
		{
			for (int z = -1; z < 2; z++)
			{
				tmpPos.Set(pos.X + x, elevatorStopHeight, pos.Z + z);
				MakeGroundSolid(tmpPos);
			}
		}
	}

	private void MakeGroundSolid(BlockPos tmpPos)
	{
		Block block = sapi.World.BlockAccessor.GetBlock(tmpPos);
		if (block.Id == 0)
		{
			sapi.World.BlockAccessor.SetBlock(colliderBlockId, tmpPos);
		}
		else if (block is BlockToggleCollisionBox)
		{
			BlockEntityGeneric blockEntity = sapi.World.BlockAccessor.GetBlockEntity<BlockEntityGeneric>(tmpPos);
			blockEntity.GetBehavior<BEBehaviorToggleCollisionBox>().Solid = true;
			blockEntity.MarkDirty();
		}
	}

	private void MakeGroundAir(BlockPos tmpPos)
	{
		Block block = sapi.World.BlockAccessor.GetBlock(tmpPos);
		if (block.Id == colliderBlockId)
		{
			sapi.World.BlockAccessor.SetBlock(0, tmpPos);
		}
		else if (block is BlockToggleCollisionBox)
		{
			BlockEntityGeneric blockEntity = sapi.World.BlockAccessor.GetBlockEntity<BlockEntityGeneric>(tmpPos);
			blockEntity.GetBehavior<BEBehaviorToggleCollisionBox>().Solid = false;
			blockEntity.MarkDirty();
		}
	}

	private void UnSetGround(BlockPos pos, int elevatorStopHeight)
	{
		BlockPos tmpPos = pos.Copy();
		tmpPos.Y = elevatorStopHeight;
		for (int x = -1; x < 2; x++)
		{
			for (int z = -1; z < 2; z++)
			{
				tmpPos.Set(pos.X + x, elevatorStopHeight, pos.Z + z);
				MakeGroundAir(tmpPos);
			}
		}
	}

	public void NowInMotion(float dt)
	{
		accum += dt;
		if ((double)accum < 0.2)
		{
			return;
		}
		accum = 0f;
		if (isMovingDown || isMovingUp)
		{
			if (isMovingUp && !AnimManager.IsAnimationActive("gearturnup"))
			{
				StopAnimation("gearturndown");
				StartAnimation("gearturnup");
			}
			if (isMovingDown && !AnimManager.IsAnimationActive("gearturndown"))
			{
				StopAnimation("gearturnup");
				StartAnimation("gearturndown");
			}
			if (!travelSound.IsPlaying)
			{
				travelSound.Start();
				travelSound.FadeTo(0.15000000596046448, 0.5f, null);
			}
			travelSound.SetPosition((float)base.Pos.X, (float)base.Pos.InternalY, (float)base.Pos.Z);
		}
		else if (travelSound.IsPlaying)
		{
			travelSound.FadeTo(0.0, 0.5f, delegate
			{
				travelSound.Stop();
			});
		}
	}

	public override void OnInteract(EntityAgent byEntity, ItemSlot itemslot, Vec3d hitPosition, EnumInteractMode mode)
	{
		EnumHandling handled = EnumHandling.PassThrough;
		foreach (EntityBehavior behavior in base.SidedProperties.Behaviors)
		{
			behavior.OnInteract(byEntity, itemslot, hitPosition, mode, ref handled);
			if (handled == EnumHandling.PreventSubsequent)
			{
				break;
			}
		}
		int seleBox = (byEntity as EntityPlayer)?.EntitySelection?.SelectionBoxIndex ?? (-1);
		EntityBehaviorSelectionBoxes bhs = GetBehavior<EntityBehaviorSelectionBoxes>();
		EntityBehaviorSeatable bhse = GetBehavior<EntityBehaviorSeatable>();
		if (bhs == null || seleBox <= 0 || bhse == null || !bhse.Seats.Any((IMountableSeat s) => s.Passenger?.EntityId == byEntity.EntityId))
		{
			return;
		}
		string apname = bhs.selectionBoxes[seleBox - 1].AttachPoint.Code;
		if (string.Equals(apname, "UpAP"))
		{
			if (Api is ICoreServerAPI)
			{
				if (ElevatorSys != null)
				{
					int i = Attributes.GetInt("maxHeight");
					int num = ElevatorSys.ControlPositions.IndexOf(i);
					int nextStopIndex = Math.Min(CurrentStopIndex + 1, ElevatorSys.ControlPositions.Count - 1);
					if (num >= nextStopIndex)
					{
						CurrentStopIndex = nextStopIndex;
					}
				}
			}
			else
			{
				StartAnimation("leverUP");
				latchSound.SetPosition((float)base.Pos.X, (float)base.Pos.InternalY, (float)base.Pos.Z);
				latchSound.Start();
			}
		}
		else
		{
			if (!string.Equals(apname, "DownAP"))
			{
				return;
			}
			if (Api is ICoreServerAPI)
			{
				if (ElevatorSys != null)
				{
					CurrentStopIndex = Math.Max(CurrentStopIndex - 1, 0);
				}
			}
			else
			{
				StartAnimation("leverDOWN");
				latchSound.SetPosition((float)base.Pos.X, (float)base.Pos.InternalY, (float)base.Pos.Z);
				latchSound.Start();
			}
		}
	}

	public void CallElevator(BlockPos position, int offset)
	{
		int height = position.Y + offset;
		if (Attributes.GetInt("maxHeight") < height)
		{
			Attributes.SetInt("maxHeight", height);
		}
		int indexOf = ElevatorSys?.ControlPositions.IndexOf(height) ?? (-1);
		if (indexOf != -1)
		{
			CurrentStopIndex = indexOf;
		}
	}

	public override bool CanCollect(Entity byEntity)
	{
		return false;
	}

	public override void OnEntityDespawn(EntityDespawnData despawn)
	{
		travelSound?.Dispose();
		latchSound?.Dispose();
		base.OnEntityDespawn(despawn);
	}

	public IMountableSeat CreateSeat(IMountable mountable, string seatId, SeatConfig config)
	{
		return new EntityElevatorSeat(mountable, seatId, config);
	}

	public void DidUnmount(EntityAgent entityAgent)
	{
		MarkShapeModified();
	}

	public void DidMount(EntityAgent entityAgent)
	{
		MarkShapeModified();
	}

	public override void FromBytes(BinaryReader reader, bool isSync)
	{
		base.FromBytes(reader, isSync);
		CurrentStopIndex = Attributes.GetInt("currentStopIndex");
		lastStopIndex = CurrentStopIndex;
	}

	public override void ToBytes(BinaryWriter writer, bool forClient)
	{
		Attributes.SetInt("currentStopIndex", CurrentStopIndex);
		base.ToBytes(writer, forClient);
	}

	public override WorldInteraction[] GetInteractionHelp(IClientWorldAccessor world, EntitySelection es, IClientPlayer player)
	{
		int seleBox = player.Entity.EntitySelection?.SelectionBoxIndex ?? (-1);
		EntityBehaviorSelectionBoxes bhs = GetBehavior<EntityBehaviorSelectionBoxes>();
		if (bhs != null && seleBox > 0)
		{
			string apname = bhs.selectionBoxes[seleBox - 1].AttachPoint.Code;
			if (string.Equals(apname, "UpAP"))
			{
				return new WorldInteraction[1]
				{
					new WorldInteraction
					{
						ActionLangCode = "elevator-leverup",
						MouseButton = EnumMouseButton.Right
					}
				};
			}
			if (string.Equals(apname, "DownAP"))
			{
				return new WorldInteraction[1]
				{
					new WorldInteraction
					{
						ActionLangCode = "elevator-leverdown",
						MouseButton = EnumMouseButton.Right
					}
				};
			}
		}
		return base.GetInteractionHelp(world, es, player);
	}

	public Vec3d? GetInteractionHelpPosition()
	{
		EntitySelection selection = capi.World.Player.CurrentEntitySelection;
		if (selection == null)
		{
			return null;
		}
		int selebox = selection.SelectionBoxIndex - 1;
		if (selebox < 0)
		{
			return null;
		}
		EntityBehaviorSelectionBoxes? behavior = selection.Entity.GetBehavior<EntityBehaviorSelectionBoxes>();
		AttachmentPoint point = ((behavior != null) ? behavior.selectionBoxes[selebox].AttachPoint : null);
		double offset = 0.5;
		if ("UpAP".Equals(point?.Code) || "DownAP".Equals(point?.Code))
		{
			offset = 0.1;
		}
		return selection.Entity.GetBehavior<EntityBehaviorSelectionBoxes>()?.GetCenterPosOfBox(selebox)?.Add(0.0, offset, 0.0);
	}
}
