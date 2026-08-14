using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json.Linq;
using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.Datastructures;
using Vintagestory.API.MathTools;
using Vintagestory.API.Util;

namespace Vintagestory.GameContent;

public class CollectibleBehaviorSqueezable : CollectibleBehavior
{
	private WorldInteraction[]? interactions;

	public float SqueezeTime { get; set; }

	public float SqueezedLitres { get; set; }

	public string AnimationCode { get; set; } = "squeezehoneycomb";

	public JsonItemStack[]? ReturnStacks { get; set; }

	public AssetLocation? SqueezingSound { get; set; }

	protected AssetLocation? liquidItemCode { get; set; }

	public Item? SqueezedLiquid { get; set; }

	public virtual bool CanSqueezeInto(IWorldAccessor world, Block block, BlockSelection? blockSel)
	{
		BlockPos pos = blockSel?.Position;
		if (block is BlockLiquidContainerTopOpened blcto)
		{
			if (!(pos == null))
			{
				return !blcto.IsFull(pos);
			}
			return true;
		}
		if (pos != null)
		{
			if (block is BlockBarrel barrel && world.BlockAccessor.GetBlockEntity(pos) is BlockEntityBarrel beb)
			{
				if (!beb.Sealed)
				{
					return !barrel.IsFull(pos);
				}
				return false;
			}
			if (world.BlockAccessor.GetBlockEntity(pos) is BlockEntityGroundStorage beg)
			{
				ItemSlot squeezeIntoSlot = beg.GetSlotAt(blockSel);
				if (squeezeIntoSlot?.Itemstack?.Block is BlockLiquidContainerTopOpened bowl)
				{
					return !bowl.IsFull(squeezeIntoSlot.Itemstack);
				}
			}
		}
		return false;
	}

	public CollectibleBehaviorSqueezable(CollectibleObject collObj)
		: base(collObj)
	{
		base.collObj = collObj;
	}

	public override void Initialize(JsonObject properties)
	{
		base.Initialize(properties);
		SqueezeTime = properties["squeezeTime"].AsFloat();
		SqueezedLitres = properties["squeezedLitres"].AsFloat();
		AnimationCode = properties["AnimationCode"].AsString("squeezehoneycomb");
		ReturnStacks = properties["returnStacks"].AsObject<JsonItemStack[]>();
		string code = properties["squeezingSound"].AsString("game:sounds/player/squeezehoneycomb");
		if (code != null)
		{
			SqueezingSound = AssetLocation.Create(code, collObj.Code.Domain);
		}
		code = properties["liquidItemCode"].AsString();
		if (code != null)
		{
			liquidItemCode = AssetLocation.Create(code, collObj.Code.Domain);
		}
	}

	public override void OnLoaded(ICoreAPI api)
	{
		base.OnLoaded(api);
		ReturnStacks?.Foreach(delegate(JsonItemStack returnStack)
		{
			returnStack?.Resolve(api.World, "returnStack for squeezing item ", collObj.Code);
		});
		SqueezedLiquid = api.World.GetItem(liquidItemCode);
		if (SqueezedLiquid == null)
		{
			api.World.Logger.Warning("Unable to resolve liquid item code '{0}' for item {1}. Will ignore.", liquidItemCode, collObj.Code);
		}
		ICoreClientAPI capi = api as ICoreClientAPI;
		if (capi == null)
		{
			return;
		}
		interactions = ObjectCacheUtil.GetOrCreate(capi, "squeezableInteractions", delegate
		{
			List<ItemStack> list = new List<ItemStack>();
			foreach (Block current in capi.World.Blocks)
			{
				if (!(current.Code == null))
				{
					if (current is BlockBarrel)
					{
						list.Add(new ItemStack(current));
					}
					if (CanSqueezeInto(capi.World, current, null))
					{
						list.Add(new ItemStack(current));
					}
				}
			}
			return new WorldInteraction[1]
			{
				new WorldInteraction
				{
					ActionLangCode = "heldhelp-squeeze",
					HotKeyCode = "shift",
					MouseButton = EnumMouseButton.Right,
					Itemstacks = list.ToArray()
				}
			};
		});
		AddSqueezableHandbookInfo(capi);
	}

	public override void OnHeldInteractStart(ItemSlot slot, EntityAgent byEntity, BlockSelection blockSel, EntitySelection entitySel, bool firstEvent, ref EnumHandHandling handHandling, ref EnumHandling handling)
	{
		if (blockSel?.Block != null && CanSqueezeInto(byEntity.World, blockSel.Block, blockSel) && byEntity.Controls.ShiftKey)
		{
			handling = EnumHandling.PreventDefault;
			handHandling = EnumHandHandling.PreventDefault;
			if (byEntity.World.Side == EnumAppSide.Client)
			{
				byEntity.World.PlaySoundAt(SqueezingSound, byEntity, null, randomizePitch: true, 16f, 0.5f);
			}
		}
		else
		{
			base.OnHeldInteractStart(slot, byEntity, blockSel, entitySel, firstEvent, ref handHandling, ref handling);
		}
	}

	public override bool OnHeldInteractStep(float secondsUsed, ItemSlot slot, EntityAgent byEntity, BlockSelection blockSel, EntitySelection entitySel, ref EnumHandling handling)
	{
		if (blockSel?.Block != null && CanSqueezeInto(byEntity.World, blockSel.Block, blockSel))
		{
			handling = EnumHandling.PreventDefault;
			if (!byEntity.Controls.ShiftKey)
			{
				return false;
			}
			if (byEntity.World is IClientWorldAccessor)
			{
				byEntity.StartAnimation(AnimationCode);
			}
			return secondsUsed < SqueezeTime;
		}
		return base.OnHeldInteractStep(secondsUsed, slot, byEntity, blockSel, entitySel, ref handling);
	}

	public override void OnHeldInteractStop(float secondsUsed, ItemSlot slot, EntityAgent byEntity, BlockSelection blockSel, EntitySelection entitySel, ref EnumHandling handling)
	{
		byEntity.StopAnimation(AnimationCode);
		if (blockSel != null)
		{
			Block block = byEntity.World.BlockAccessor.GetBlock(blockSel.Position);
			if (CanSqueezeInto(byEntity.World, block, blockSel))
			{
				handling = EnumHandling.PreventDefault;
				if (secondsUsed < SqueezeTime - 0.05f || SqueezedLiquid == null || byEntity.World.Side == EnumAppSide.Client)
				{
					return;
				}
				IWorldAccessor world = byEntity.World;
				if (!CanSqueezeInto(world, block, blockSel))
				{
					return;
				}
				ItemStack squeezedStack = new ItemStack(SqueezedLiquid, 99999);
				if (block is BlockLiquidContainerTopOpened blockCnt)
				{
					if (blockCnt.TryPutLiquid(blockSel.Position, squeezedStack, SqueezedLitres) == 0)
					{
						return;
					}
				}
				else if (block is BlockBarrel blockBarrel && world.BlockAccessor.GetBlockEntity(blockSel.Position) is BlockEntityBarrel beb)
				{
					if (beb.Sealed || blockBarrel.TryPutLiquid(blockSel.Position, squeezedStack, SqueezedLitres) == 0)
					{
						return;
					}
				}
				else if (world.BlockAccessor.GetBlockEntity(blockSel.Position) is BlockEntityGroundStorage beg)
				{
					ItemSlot squeezeIntoSlot = beg.GetSlotAt(blockSel);
					if (squeezeIntoSlot != null && squeezeIntoSlot.Itemstack?.Block is BlockLiquidContainerTopOpened begCnt && CanSqueezeInto(world, begCnt, null))
					{
						if (begCnt.TryPutLiquid(squeezeIntoSlot.Itemstack, squeezedStack, SqueezedLitres) == 0)
						{
							return;
						}
						beg.MarkDirty(redrawOnClient: true);
					}
				}
				slot.TakeOut(1);
				slot.MarkDirty();
				IPlayer byPlayer = world.PlayerByUid((byEntity as EntityPlayer)?.PlayerUID);
				ReturnStacks?.Foreach(delegate(JsonItemStack returnStack)
				{
					ItemStack itemStack = returnStack.ResolvedItemstack?.Clone();
					if (itemStack != null)
					{
						IPlayer player = byPlayer;
						if (player == null || !player.InventoryManager.TryGiveItemstack(itemStack))
						{
							world.SpawnItemEntity(itemStack, blockSel.Position);
						}
					}
				});
				return;
			}
		}
		base.OnHeldInteractStop(secondsUsed, slot, byEntity, blockSel, entitySel, ref handling);
	}

	public override bool OnHeldInteractCancel(float secondsUsed, ItemSlot slot, EntityAgent byEntity, BlockSelection blockSel, EntitySelection entitySel, EnumItemUseCancelReason cancelReason, ref EnumHandling handling)
	{
		byEntity.StopAnimation(AnimationCode);
		return base.OnHeldInteractCancel(secondsUsed, slot, byEntity, blockSel, entitySel, cancelReason, ref handling);
	}

	public override WorldInteraction[] GetHeldInteractionHelp(ItemSlot inSlot, ref EnumHandling handling)
	{
		return interactions.Append<WorldInteraction>(base.GetHeldInteractionHelp(inSlot, ref handling));
	}

	protected virtual void AddSqueezableHandbookInfo(ICoreClientAPI capi)
	{
		ExtraHandbookSection[] extraHandbookSections = collObj.Attributes?["handbook"]?["extraSections"]?.AsObject<ExtraHandbookSection[]>();
		if (extraHandbookSections?.FirstOrDefault((ExtraHandbookSection s) => s?.Title == "handbook-squeezinghelp-title") != null)
		{
			return;
		}
		JsonObject attributes = collObj.Attributes;
		if (attributes == null || !attributes["handbook"].Exists)
		{
			if (collObj.Attributes == null)
			{
				collObj.Attributes = new JsonObject(JToken.Parse("{ handbook: {} }"));
			}
			else
			{
				collObj.Attributes.Token["handbook"] = JToken.Parse("{ }");
			}
		}
		ExtraHandbookSection section = new ExtraHandbookSection
		{
			Title = "handbook-squeezinghelp-title",
			Text = "handbook-squeezinghelp-text"
		};
		if (extraHandbookSections != null)
		{
			extraHandbookSections.Append(section);
		}
		else
		{
			extraHandbookSections = new ExtraHandbookSection[1] { section };
		}
		collObj.Attributes["handbook"].Token["extraSections"] = JToken.FromObject(extraHandbookSections);
	}
}
