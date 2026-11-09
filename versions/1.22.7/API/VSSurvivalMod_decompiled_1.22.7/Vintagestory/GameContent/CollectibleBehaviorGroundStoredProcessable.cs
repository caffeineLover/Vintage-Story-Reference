using System;
using System.Linq;
using Vintagestory.API;
using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.Config;
using Vintagestory.API.Datastructures;
using Vintagestory.API.MathTools;
using Vintagestory.API.Util;

namespace Vintagestory.GameContent;

public class CollectibleBehaviorGroundStoredProcessable(CollectibleObject collObj) : CollectibleBehavior(collObj), IContainedInteractable
{
	[DocumentAsJson("Recommended", "0", false)]
	public float ProcessTime;

	[DocumentAsJson("Required", "", false)]
	public BlockDropItemStack[]? ProcessedStacks = Array.Empty<BlockDropItemStack>();

	[DocumentAsJson("Optional", "None", false)]
	public AssetLocation? ProcessingSound;

	[DocumentAsJson("Optional", "None", false)]
	public string? ProcessingAnimationCode;

	[DocumentAsJson("Optional", "None", false)]
	public JsonItemStack? RemainingItem;

	[DocumentAsJson("Optional", "blockhelp-processable-process", false)]
	private string? interactionHelpCode;

	[DocumentAsJson("Optional", "groundstoredprocessesdesc-title", false)]
	public string? HandbookProcessIntoTitle;

	[DocumentAsJson("Optional", "handbook-createdby-groundstoredprocessing", false)]
	public string? HandbookCreatedByTitle;

	[DocumentAsJson("Optional", "None", false)]
	public EnumTool? Tool;

	[DocumentAsJson("Optional", "1", false)]
	private int toolDamage = 1;

	[DocumentAsJson("Optional", "false", false)]
	private bool transferFreshness;

	[DocumentAsJson("Optional", "None", false)]
	public EnumBlockMaterial[]? RequiredSurfaceMaterials;

	[DocumentAsJson("Optional", "itemore-needssolid-error", false)]
	private string? surfaceErrorLangCode;

	[DocumentAsJson("Optional", "None", false)]
	public AssetLocation? CompletionSound;

	public override void Initialize(JsonObject properties)
	{
		base.Initialize(properties);
		interactionHelpCode = properties["interactionHelpCode"].AsString("blockhelp-processable-process");
		HandbookProcessIntoTitle = properties["handbookProcessIntoTitle"].AsString("groundstoredprocessesdesc-title");
		HandbookCreatedByTitle = properties["handbookCreatedByTitle"].AsString("handbook-createdby-groundstoredprocessing");
		ProcessTime = properties["processTime"].AsFloat();
		ProcessedStacks = properties["processedStacks"].AsObject<BlockDropItemStack[]>();
		ProcessingAnimationCode = properties["processingAnimationCode"].AsString();
		string code = properties["processingSound"].AsString();
		if (code != null)
		{
			ProcessingSound = AssetLocation.Create(code, collObj.Code.Domain);
		}
		code = properties["completionSound"].AsString();
		if (code != null)
		{
			CompletionSound = AssetLocation.Create(code, collObj.Code.Domain);
		}
		RemainingItem = properties["remainingItem"].AsObject<JsonItemStack>();
		Tool = properties["tool"].AsObject<EnumTool?>();
		toolDamage = properties["toolDurabilityCost"].AsInt(1);
		transferFreshness = properties["transferFreshness"].AsBool();
		RequiredSurfaceMaterials = properties["requiredSurfaceMaterials"].AsObject<EnumBlockMaterial[]>();
		surfaceErrorLangCode = properties["surfaceErrorLangCode"].AsString("itemore-needssolid-error");
	}

	public override void OnLoaded(ICoreAPI api)
	{
		base.OnLoaded(api);
		if (ProcessedStacks == null && RemainingItem == null)
		{
			api.Logger.Warning($"{collObj.Code} has no processedStacks or remainingItem specified for GroundStoredProcessable behavior");
		}
		ProcessedStacks?.Foreach(delegate(BlockDropItemStack processedStack)
		{
			processedStack?.Resolve(api.World, "processedStack of item ", collObj.Code);
		});
		RemainingItem?.Resolve(api.World, "remainingItem of item ", collObj.Code);
	}

	private bool canProcessOnSurfaceMaterial(BlockEntityContainer be, IPlayer byPlayer)
	{
		if (RequiredSurfaceMaterials == null)
		{
			return true;
		}
		EnumBlockMaterial belowMaterial = be.Api.World.BlockAccessor.GetBlock(be.Pos.DownCopy()).BlockMaterial;
		return RequiredSurfaceMaterials.Contains(belowMaterial);
	}

	public bool OnContainedInteractStart(BlockEntityContainer be, ItemSlot slot, IPlayer byPlayer, BlockSelection blockSel)
	{
		if (Tool.HasValue && byPlayer.InventoryManager.ActiveTool != Tool)
		{
			return false;
		}
		if (!byPlayer.Entity.Controls.ShiftKey)
		{
			return false;
		}
		if (!be.Api.World.Claims.TryAccess(byPlayer, blockSel.Position, EnumBlockAccessFlags.Use))
		{
			return false;
		}
		if (!canProcessOnSurfaceMaterial(be, byPlayer))
		{
			(be.Api as ICoreClientAPI)?.TriggerIngameError(this, "needssolidsurface", Lang.Get(surfaceErrorLangCode));
			return false;
		}
		if (ProcessedStacks != null || RemainingItem != null)
		{
			be.Api.World.PlaySoundAt(ProcessingSound, blockSel.Position, 0.0, byPlayer);
			return true;
		}
		return false;
	}

	public bool OnContainedInteractStep(float secondsUsed, BlockEntityContainer be, ItemSlot slot, IPlayer byPlayer, BlockSelection blockSel)
	{
		if (Tool.HasValue && byPlayer.InventoryManager.ActiveTool != Tool)
		{
			return false;
		}
		if (!byPlayer.Entity.Controls.ShiftKey)
		{
			return false;
		}
		if (blockSel == null)
		{
			return false;
		}
		if (!canProcessOnSurfaceMaterial(be, byPlayer))
		{
			return false;
		}
		if (ProcessingAnimationCode != null && byPlayer.Entity.World is IClientWorldAccessor)
		{
			byPlayer.Entity.StartAnimation(ProcessingAnimationCode);
		}
		if (be.Api.World.Rand.NextDouble() < 0.05)
		{
			be.Api.World.PlaySoundAt(ProcessingSound, blockSel.Position, 0.0, byPlayer);
		}
		if (be.Api.World.Side == EnumAppSide.Client && be.Api.World.Rand.NextDouble() < 0.25)
		{
			BlockDropItemStack[]? processedStacks = ProcessedStacks;
			if ((((processedStacks == null) ? null : processedStacks[0]?.ResolvedItemstack) ?? RemainingItem?.ResolvedItemstack) != null)
			{
				IWorldAccessor world = be.Api.World;
				Vec3d pos = blockSel.Position.ToVec3d().Add(blockSel.HitPosition);
				BlockDropItemStack[]? processedStacks2 = ProcessedStacks;
				world.SpawnCubeParticles(pos, ((processedStacks2 == null) ? null : processedStacks2[0]?.ResolvedItemstack) ?? RemainingItem.ResolvedItemstack, 0.25f, 1, 0.5f, byPlayer, new Vec3f(0f, 1f, 0f));
			}
		}
		return secondsUsed < ProcessTime;
	}

	public void OnContainedInteractStop(float secondsUsed, BlockEntityContainer be, ItemSlot slot, IPlayer byPlayer, BlockSelection blockSel)
	{
		byPlayer.Entity.StopAnimation(ProcessingAnimationCode);
		if ((Tool.HasValue && byPlayer.InventoryManager.ActiveTool != Tool) || !canProcessOnSurfaceMaterial(be, byPlayer) || !(secondsUsed > ProcessTime - 0.05f) || (ProcessedStacks == null && RemainingItem == null) || be.Api.World.Side != EnumAppSide.Server)
		{
			return;
		}
		ProcessedStacks?.Foreach(delegate(BlockDropItemStack processedStack)
		{
			ItemStack nextItemStack = processedStack.GetNextItemStack();
			if (nextItemStack != null)
			{
				ItemStack itemStack = nextItemStack.Clone();
				int stackSize = nextItemStack.StackSize;
				if (transferFreshness)
				{
					TransitionableProperties transitionableProperties = nextItemStack.Collectible.GetTransitionableProperties(be.Api.World, nextItemStack, null)?.FirstOrDefault((TransitionableProperties p) => p.Type == EnumTransitionType.Perish);
					if (transitionableProperties != null)
					{
						CollectibleObject.CarryOverFreshness(be.Api, slot, nextItemStack, transitionableProperties);
					}
				}
				if (!byPlayer.InventoryManager.TryGiveItemstack(nextItemStack))
				{
					be.Api.World.SpawnItemEntity(nextItemStack, blockSel.Position);
				}
				be.Api.World.Logger.Audit("{0} Took {1}x{2} from {3} at {4}.", byPlayer.PlayerName, stackSize, nextItemStack.Collectible.Code, collObj.Code, blockSel.Position);
				TreeAttribute data = new TreeAttribute
				{
					["itemstack"] = new ItemstackAttribute(itemStack.Clone()),
					["byentityid"] = new LongAttribute(byPlayer.Entity.EntityId)
				};
				be.Api.World.Api.Event.PushEvent("onitemcollected", data);
			}
		});
		ItemStack remainingStack = RemainingItem?.ResolvedItemstack?.Clone();
		if (transferFreshness)
		{
			TransitionableProperties perishProps = (remainingStack?.Collectible.GetTransitionableProperties(be.Api.World, remainingStack, null))?.FirstOrDefault((TransitionableProperties p) => p.Type == EnumTransitionType.Perish);
			if (perishProps != null)
			{
				CollectibleObject.CarryOverFreshness(be.Api, slot, remainingStack, perishProps);
			}
		}
		if (slot.Itemstack.StackSize > 1)
		{
			if (remainingStack != null)
			{
				ItemStack origStack = remainingStack.Clone();
				int quantity = remainingStack.StackSize;
				if (!byPlayer.InventoryManager.TryGiveItemstack(remainingStack))
				{
					be.Api.World.SpawnItemEntity(remainingStack, blockSel.Position);
				}
				be.Api.World.Logger.Audit("{0} Took {1}x{2} from {3} at {4}.", byPlayer.PlayerName, quantity, remainingStack.Collectible.Code, collObj.Code, blockSel.Position);
				TreeAttribute tree = new TreeAttribute();
				tree["itemstack"] = new ItemstackAttribute(origStack.Clone());
				tree["byentityid"] = new LongAttribute(byPlayer.Entity.EntityId);
				be.Api.World.Api.Event.PushEvent("onitemcollected", tree);
			}
			slot.TakeOut(1);
		}
		else
		{
			slot.Itemstack = remainingStack;
		}
		be.MarkDirty(redrawOnClient: true);
		if (be.Inventory.Empty)
		{
			be.Api.World.BlockAccessor.SetBlock(0, blockSel.Position);
		}
		if (Tool.HasValue)
		{
			ItemSlot toolSlot = byPlayer.InventoryManager.ActiveHotbarSlot;
			toolSlot.Itemstack?.Collectible.DamageItem(be.Api.World, byPlayer.Entity, toolSlot, toolDamage);
		}
		be.Api.World.PlaySoundAt(CompletionSound ?? ProcessingSound, blockSel.Position, 0.0, byPlayer);
	}

	public bool OnContainedInteractCancel(float secondsUsed, BlockEntityContainer be, ItemSlot slot, IPlayer byPlayer, BlockSelection blockSel, EnumItemUseCancelReason cancelReason)
	{
		byPlayer.Entity.StopAnimation(ProcessingAnimationCode);
		return true;
	}

	public WorldInteraction[] GetContainedInteractionHelp(BlockEntityContainer be, ItemSlot slot, IPlayer byPlayer, BlockSelection blockSel)
	{
		if (ProcessedStacks != null || RemainingItem != null)
		{
			if (!canProcessOnSurfaceMaterial(be, byPlayer))
			{
				return Array.Empty<WorldInteraction>();
			}
			bool notProtected = true;
			if (be.Api.World.Claims != null && be.Api.World is IClientWorldAccessor clientWorld)
			{
				IClientPlayer player = clientWorld.Player;
				if (player != null && player.WorldData.CurrentGameMode == EnumGameMode.Survival && clientWorld.Claims.TestAccess(clientWorld.Player, blockSel.Position, EnumBlockAccessFlags.Use) != EnumWorldAccessResponse.Granted)
				{
					notProtected = false;
				}
			}
			if (notProtected)
			{
				return new WorldInteraction[1]
				{
					new WorldInteraction
					{
						ActionLangCode = interactionHelpCode,
						MouseButton = EnumMouseButton.Right,
						HotKeyCode = "shift",
						Itemstacks = ((!Tool.HasValue) ? null : ObjectCacheUtil.GetToolStacks(be.Api, Tool.Value))
					}
				};
			}
		}
		return Array.Empty<WorldInteraction>();
	}
}
