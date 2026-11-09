using System;
using System.Text;
using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.Config;
using Vintagestory.API.Datastructures;
using Vintagestory.API.MathTools;

namespace Vintagestory.GameContent;

public class BlockEntityShelf : BlockEntityDisplay
{
	protected static int slotCount = 8;

	protected InventoryGeneric inv;

	public override InventoryBase Inventory => inv;

	public override string InventoryClassName => "shelf";

	public override string AttributeTransformCode => "onshelfTransform";

	protected string GetSlotType(int slotid)
	{
		return "shelf";
	}

	public BlockEntityShelf()
	{
		inv = new InventoryGeneric(slotCount, "shelf-0", null, (int id, InventoryGeneric inv) => new ItemSlotDisplay(inv, GetSlotType(id)));
	}

	public override void Initialize(ICoreAPI api)
	{
		base.Initialize(api);
		inv.OnAcquireTransitionSpeed += Inv_OnAcquireTransitionSpeed;
	}

	protected float Inv_OnAcquireTransitionSpeed(EnumTransitionType transType, ItemStack stack, float baseMul)
	{
		if ((transType == EnumTransitionType.Dry || transType == EnumTransitionType.Melt) ? true : false)
		{
			Room room = container.Room;
			return ((room != null && room.ExitCount == 0) ? 2f : 0.5f) * 4f;
		}
		if (Api == null)
		{
			return 0f;
		}
		if (transType != EnumTransitionType.Ripen)
		{
			return 1f;
		}
		return GameMath.Clamp((1f - container.GetPerishRate() - 0.5f) * 3f, 0f, 1f);
	}

	public bool OnInteract(IPlayer byPlayer, BlockSelection blockSel)
	{
		ItemSlot slot = byPlayer.InventoryManager.ActiveHotbarSlot;
		if (TryUse(byPlayer, blockSel))
		{
			return true;
		}
		if (slot.Empty)
		{
			return TryTake(byPlayer, blockSel);
		}
		if (GetShelvableLayout(slot.Itemstack).HasValue)
		{
			return TryPut(byPlayer, blockSel);
		}
		return false;
	}

	public static EnumShelvableLayout? GetShelvableLayout(ItemStack? stack)
	{
		if (stack == null)
		{
			return null;
		}
		JsonObject attr = stack.Collectible?.Attributes;
		EnumShelvableLayout? layout = stack.Collectible?.GetCollectibleInterface<IShelvable>()?.GetShelvableType(stack);
		EnumShelvableLayout? enumShelvableLayout = layout;
		if (!enumShelvableLayout.HasValue)
		{
			layout = attr?["shelvable"].AsString() switch
			{
				"Quadrants" => EnumShelvableLayout.Quadrants, 
				"Halves" => EnumShelvableLayout.Halves, 
				"SingleCenter" => EnumShelvableLayout.SingleCenter, 
				_ => null, 
			};
		}
		enumShelvableLayout = layout;
		if (!enumShelvableLayout.HasValue)
		{
			layout = ((attr != null && attr["shelvable"].AsBool()) ? new EnumShelvableLayout?(EnumShelvableLayout.Quadrants) : ((EnumShelvableLayout?)null));
		}
		return layout;
	}

	public bool CanUse(ItemStack? stack, BlockSelection blockSel)
	{
		if (stack == null)
		{
			return false;
		}
		CollectibleObject obj = stack.Collectible;
		bool up = blockSel.SelectionBoxIndex > 1;
		bool left = blockSel.SelectionBoxIndex % 2 == 0;
		EnumShelvableLayout? shelvableLayout = GetShelvableLayout(inv[up ? 4 : 0].Itemstack);
		if ((!shelvableLayout.HasValue || shelvableLayout != EnumShelvableLayout.SingleCenter) && !left)
		{
			shelvableLayout = GetShelvableLayout(inv[up ? 6 : 2].Itemstack);
		}
		int start = (up ? 4 : 0) + ((!shelvableLayout.HasValue || shelvableLayout != EnumShelvableLayout.SingleCenter) ? ((!left) ? 2 : 0) : 0);
		int num = start;
		bool flag;
		if (shelvableLayout.HasValue)
		{
			EnumShelvableLayout valueOrDefault = shelvableLayout.GetValueOrDefault();
			if ((uint)(valueOrDefault - 1) <= 1u)
			{
				flag = true;
				goto IL_00be;
			}
		}
		flag = false;
		goto IL_00be;
		IL_00be:
		for (int i = num + (flag ? 1 : 2) - 1; i >= start; i--)
		{
			if (!inv[i].Empty)
			{
				CollectibleObject invColl = inv[i].Itemstack.Collectible;
				flag = obj != null && obj.Attributes?["mealContainer"]?.AsBool() == true;
				if (!flag)
				{
					bool flag2 = ((obj is IContainedInteractable || obj is IBlockMealContainer) ? true : false);
					flag = flag2;
				}
				if (flag)
				{
					return invColl is BlockCookedContainerBase;
				}
				if (obj != null && obj.Attributes?["canSealCrock"]?.AsBool() == true)
				{
					return invColl is BlockCrock;
				}
			}
		}
		return false;
	}

	public bool CanPlace(ItemStack? stack, BlockSelection blockSel, out bool canTake)
	{
		bool up = blockSel.SelectionBoxIndex > 1;
		bool left = blockSel.SelectionBoxIndex % 2 == 0;
		EnumShelvableLayout? shelvableLayout = GetShelvableLayout(inv[up ? 4 : 0].Itemstack);
		if (shelvableLayout.HasValue)
		{
			EnumShelvableLayout shelvableLayoutFullSlot = shelvableLayout.GetValueOrDefault();
			if (shelvableLayoutFullSlot == EnumShelvableLayout.SingleCenter || (shelvableLayoutFullSlot == EnumShelvableLayout.Halves && left))
			{
				goto IL_0085;
			}
		}
		shelvableLayout = GetShelvableLayout(inv[up ? 6 : 2].Itemstack);
		if (shelvableLayout.HasValue && shelvableLayout == EnumShelvableLayout.Halves && !left)
		{
			goto IL_0085;
		}
		EnumShelvableLayout? shelvableLayout2 = GetShelvableLayout(stack);
		int start = (up ? 4 : 0) + ((!shelvableLayout2.HasValue || shelvableLayout2 != EnumShelvableLayout.SingleCenter) ? ((!left) ? 2 : 0) : 0);
		int num = start;
		bool flag;
		if (shelvableLayout2.HasValue)
		{
			EnumShelvableLayout valueOrDefault = shelvableLayout2.GetValueOrDefault();
			if ((uint)(valueOrDefault - 1) <= 1u)
			{
				flag = true;
				goto IL_00dd;
			}
		}
		flag = false;
		goto IL_00dd;
		IL_0085:
		canTake = true;
		return false;
		IL_00dd:
		int num2 = num + (flag ? 1 : 2);
		canTake = false;
		bool canPlace = false;
		for (int i = num2 - 1; i >= start; i--)
		{
			if (inv[i].Empty)
			{
				canPlace = true;
			}
			else
			{
				canTake = true;
			}
		}
		return canPlace;
	}

	private bool TryUse(IPlayer player, BlockSelection blockSel)
	{
		bool up = blockSel.SelectionBoxIndex > 1;
		bool left = blockSel.SelectionBoxIndex % 2 == 0;
		EnumShelvableLayout? shelvableLayout = GetShelvableLayout(inv[up ? 4 : 0].Itemstack);
		if ((!shelvableLayout.HasValue || shelvableLayout != EnumShelvableLayout.SingleCenter) && !left)
		{
			shelvableLayout = GetShelvableLayout(inv[up ? 6 : 2].Itemstack);
		}
		int start = (up ? 4 : 0) + ((!shelvableLayout.HasValue || shelvableLayout != EnumShelvableLayout.SingleCenter) ? ((!left) ? 2 : 0) : 0);
		int num = start;
		bool flag;
		if (shelvableLayout.HasValue)
		{
			EnumShelvableLayout valueOrDefault = shelvableLayout.GetValueOrDefault();
			if ((uint)(valueOrDefault - 1) <= 1u)
			{
				flag = true;
				goto IL_00b0;
			}
		}
		flag = false;
		goto IL_00b0;
		IL_00b0:
		int end = num + (flag ? 1 : 2);
		if (player.Entity.Controls.ShiftKey)
		{
			return false;
		}
		for (int i = end - 1; i >= start; i--)
		{
			IContainedInteractable collIci = inv[i].Itemstack?.Collectible.GetCollectibleInterface<IContainedInteractable>();
			if (collIci != null && collIci.OnContainedInteractStart(this, inv[i], player, blockSel))
			{
				MarkDirty();
				return true;
			}
		}
		return false;
	}

	private bool TryPut(IPlayer byPlayer, BlockSelection blockSel)
	{
		ItemSlot heldSlot = byPlayer.InventoryManager.ActiveHotbarSlot;
		bool up = blockSel.SelectionBoxIndex > 1;
		bool left = blockSel.SelectionBoxIndex % 2 == 0;
		int filledSlots = 0;
		EnumShelvableLayout? shelvableLayout = GetShelvableLayout(heldSlot.Itemstack);
		int start = (up ? 4 : 0) + ((!shelvableLayout.HasValue || shelvableLayout != EnumShelvableLayout.SingleCenter) ? ((!left) ? 2 : 0) : 0);
		int end = start + ((shelvableLayout.HasValue && shelvableLayout == EnumShelvableLayout.SingleCenter) ? 4 : 2);
		bool flag;
		if (shelvableLayout.HasValue)
		{
			EnumShelvableLayout valueOrDefault = shelvableLayout.GetValueOrDefault();
			if ((uint)(valueOrDefault - 1) <= 1u)
			{
				flag = true;
				goto IL_0095;
			}
		}
		flag = false;
		goto IL_0095;
		IL_0204:
		int num;
		end = num + (flag ? 1 : 2);
		for (int i = start; i < end; i++)
		{
			if (inv[i].Empty)
			{
				int num2 = heldSlot.TryPutInto(Api.World, inv[i]);
				MarkDirty();
				(Api as ICoreClientAPI)?.World.Player.TriggerFpAnimation(EnumHandInteract.HeldItemInteract);
				if (num2 > 0)
				{
					Api.World.PlaySoundAt(inv[i].Itemstack?.Block?.Sounds?.Place ?? GlobalConstants.DefaultBuildSound, byPlayer.Entity, byPlayer);
					Api.World.Logger.Audit("{0} Put 1x{1} into Shelf at {2}.", byPlayer.PlayerName, inv[i].Itemstack?.Collectible.Code, Pos);
					return true;
				}
				return false;
			}
		}
		(Api as ICoreClientAPI)?.TriggerIngameError(this, "shelffull", Lang.Get("shelfhelp-shelffull-error"));
		return false;
		IL_0095:
		if (flag)
		{
			for (int j = start; j < end; j++)
			{
				if (!inv[j].Empty)
				{
					EnumShelvableLayout? layout = GetShelvableLayout(inv[j].Itemstack);
					filledSlots += ((layout.HasValue && layout == EnumShelvableLayout.SingleCenter) ? 4 : ((!layout.HasValue || layout != EnumShelvableLayout.Halves) ? 1 : 2));
				}
			}
		}
		if (filledSlots > 0 && filledSlots < ((shelvableLayout.HasValue && shelvableLayout == EnumShelvableLayout.SingleCenter) ? 4 : 2))
		{
			(Api as ICoreClientAPI)?.TriggerIngameError(this, "needsmorespace", Lang.Get("shelfhelp-needsmorespace-error"));
			return false;
		}
		if (!shelvableLayout.HasValue || shelvableLayout != EnumShelvableLayout.SingleCenter)
		{
			shelvableLayout = GetShelvableLayout(inv[up ? 4 : 0].Itemstack);
		}
		if ((!shelvableLayout.HasValue || shelvableLayout != EnumShelvableLayout.SingleCenter) && !left)
		{
			shelvableLayout = GetShelvableLayout(inv[up ? 6 : 2].Itemstack);
		}
		start = (up ? 4 : 0) + ((!shelvableLayout.HasValue || shelvableLayout != EnumShelvableLayout.SingleCenter) ? ((!left) ? 2 : 0) : 0);
		num = start;
		if (shelvableLayout.HasValue)
		{
			EnumShelvableLayout valueOrDefault = shelvableLayout.GetValueOrDefault();
			if ((uint)(valueOrDefault - 1) <= 1u)
			{
				flag = true;
				goto IL_0204;
			}
		}
		flag = false;
		goto IL_0204;
	}

	private bool TryTake(IPlayer byPlayer, BlockSelection blockSel)
	{
		bool up = blockSel.SelectionBoxIndex > 1;
		bool left = blockSel.SelectionBoxIndex % 2 == 0;
		EnumShelvableLayout? shelvableLayout = GetShelvableLayout(inv[up ? 4 : 0].Itemstack);
		if ((!shelvableLayout.HasValue || shelvableLayout != EnumShelvableLayout.SingleCenter) && !left)
		{
			shelvableLayout = GetShelvableLayout(inv[up ? 6 : 2].Itemstack);
		}
		int start = (up ? 4 : 0) + ((!shelvableLayout.HasValue || shelvableLayout != EnumShelvableLayout.SingleCenter) ? ((!left) ? 2 : 0) : 0);
		for (int i = start + ((shelvableLayout.HasValue && shelvableLayout == EnumShelvableLayout.SingleCenter) ? 4 : 2) - 1; i >= start; i--)
		{
			if (!inv[i].Empty)
			{
				ItemStack stack = inv[i].TakeOut(1);
				if (byPlayer.InventoryManager.TryGiveItemstack(stack))
				{
					SoundAttributes? obj = stack?.Block?.Sounds?.Place;
					Api.World.PlaySoundAt(obj ?? GlobalConstants.DefaultBuildSound, byPlayer.Entity, byPlayer);
				}
				if (stack != null && stack.StackSize > 0)
				{
					Api.World.SpawnItemEntity(stack, Pos);
				}
				Api.World.Logger.Audit("{0} Took 1x{1} from Shelf at {2}.", byPlayer.PlayerName, stack?.Collectible.Code, Pos);
				(Api as ICoreClientAPI)?.World.Player.TriggerFpAnimation(EnumHandInteract.HeldItemInteract);
				MarkDirty();
				return true;
			}
		}
		return false;
	}

	protected override float[][] genTransformationMatrices()
	{
		float[][] tfMatrices = new float[slotCount][];
		for (int index = 0; index < slotCount; index++)
		{
			EnumShelvableLayout? shelvableType = GetShelvableLayout(inv[index].Itemstack);
			float x = ((index % 4 >= 2) ? 0.75f : 0.25f);
			float y = ((index >= 4) ? 0.625f : 0.125f);
			float z = ((index % 2 == 0) ? 0.25f : 0.625f);
			bool flag = ((index == 0 || index == 4) ? true : false);
			if (flag && shelvableType.HasValue && shelvableType == EnumShelvableLayout.SingleCenter)
			{
				x = 0.5f;
			}
			switch (index)
			{
			case 0:
			case 2:
			case 4:
			case 6:
				flag = true;
				break;
			default:
				flag = false;
				break;
			}
			bool flag2 = flag;
			bool flag3;
			if (flag2)
			{
				if (shelvableType.HasValue)
				{
					EnumShelvableLayout valueOrDefault = shelvableType.GetValueOrDefault();
					if ((uint)(valueOrDefault - 1) <= 1u)
					{
						flag3 = true;
						goto IL_00e2;
					}
				}
				flag3 = false;
				goto IL_00e2;
			}
			goto IL_00e6;
			IL_00e2:
			flag2 = flag3;
			goto IL_00e6;
			IL_00e6:
			if (flag2)
			{
				z = 0.4f;
			}
			tfMatrices[index] = new Matrixf().Translate(0.5f, 0f, 0.5f).RotateYDeg(base.Block.Shape.rotateY).Translate(x - 0.5f, y, z - 0.5f)
				.Translate(-0.5f, 0f, -0.5f)
				.Values;
		}
		return tfMatrices;
	}

	public override void FromTreeAttributes(ITreeAttribute tree, IWorldAccessor worldForResolving)
	{
		base.FromTreeAttributes(tree, worldForResolving);
		RedrawAfterReceivingTreeAttributes(worldForResolving);
	}

	public override void GetBlockInfo(IPlayer forPlayer, StringBuilder sb)
	{
		base.GetBlockInfo(forPlayer, sb);
		float ripenRate = GameMath.Clamp((1f - container.GetPerishRate() - 0.5f) * 3f, 0f, 1f);
		if (ripenRate > 0f)
		{
			sb.Append(Lang.Get("Suitable spot for food ripening."));
		}
		sb.AppendLine();
		bool up = forPlayer.CurrentBlockSelection != null && forPlayer.CurrentBlockSelection.SelectionBoxIndex > 1;
		for (int j = 3; j >= 0; j--)
		{
			int i = j + (up ? 4 : 0);
			i ^= 2;
			if (!inv[i].Empty)
			{
				ItemStack stack = inv[i].Itemstack;
				TransitionableProperties[] transitionableProps = stack?.Collectible?.GetTransitionableProperties(Api.World, stack, forPlayer.Entity);
				if (transitionableProps != null && transitionableProps.Length != 0)
				{
					sb.Append(PerishableInfoCompact(Api, inv[i], ripenRate));
				}
				else
				{
					sb.AppendLine(stack?.Collectible.GetCollectibleInterface<IContainedCustomName>()?.GetContainedInfo(inv[i]) ?? stack?.GetName() ?? Lang.Get("unknown"));
				}
			}
		}
	}

	public static string PerishableInfoCompact(ICoreAPI Api, ItemSlot contentSlot, float ripenRate, bool withStackName = true)
	{
		if (contentSlot.Empty)
		{
			return "";
		}
		StringBuilder dsc = new StringBuilder();
		if (withStackName)
		{
			dsc.Append(contentSlot.Itemstack.GetName());
		}
		TransitionState[] transitionStates = contentSlot.Itemstack.Collectible.UpdateAndGetTransitionStates(Api.World, contentSlot);
		if (transitionStates == null)
		{
			return dsc.ToString();
		}
		bool nowSpoiling = false;
		bool appendLine = false;
		foreach (TransitionState state in transitionStates)
		{
			TransitionableProperties prop = state.Props;
			float perishRate = contentSlot.Itemstack.Collectible.GetTransitionRateMul(Api.World, contentSlot, prop.Type);
			if (perishRate <= 0f)
			{
				continue;
			}
			float transitionLevel = state.TransitionLevel;
			float freshHoursLeft = state.FreshHoursLeft / perishRate;
			switch (prop.Type)
			{
			case EnumTransitionType.Perish:
			{
				appendLine = true;
				if (transitionLevel > 0f)
				{
					nowSpoiling = true;
					dsc.Append(", " + Lang.Get("{0}% spoiled", (int)Math.Round(transitionLevel * 100f)));
					break;
				}
				double hoursPerday2 = Api.World.Calendar.HoursPerDay;
				if ((double)freshHoursLeft / hoursPerday2 >= (double)Api.World.Calendar.DaysPerYear)
				{
					dsc.Append(", " + Lang.Get("fresh for {0} years", Math.Round((double)freshHoursLeft / hoursPerday2 / (double)Api.World.Calendar.DaysPerYear, 1)));
				}
				else if ((double)freshHoursLeft > hoursPerday2)
				{
					dsc.Append(", " + Lang.Get("fresh for {0} days", Math.Round((double)freshHoursLeft / hoursPerday2, 1)));
				}
				else
				{
					dsc.Append(", " + Lang.Get("fresh for {0} hours", Math.Round(freshHoursLeft, 1)));
				}
				break;
			}
			case EnumTransitionType.Ripen:
			{
				if (nowSpoiling)
				{
					break;
				}
				appendLine = true;
				if (transitionLevel > 0f)
				{
					dsc.Append(", " + Lang.Get("{1:0.#} days left to ripen ({0}%)", (int)Math.Round(transitionLevel * 100f), (state.TransitionHours - state.TransitionedHours) / Api.World.Calendar.HoursPerDay / ripenRate));
					break;
				}
				double hoursPerday = Api.World.Calendar.HoursPerDay;
				if ((double)freshHoursLeft / hoursPerday >= (double)Api.World.Calendar.DaysPerYear)
				{
					dsc.Append(", " + Lang.Get("will ripen in {0} years", Math.Round((double)freshHoursLeft / hoursPerday / (double)Api.World.Calendar.DaysPerYear, 1)));
				}
				else if ((double)freshHoursLeft > hoursPerday)
				{
					dsc.Append(", " + Lang.Get("will ripen in {0} days", Math.Round((double)freshHoursLeft / hoursPerday, 1)));
				}
				else
				{
					dsc.Append(", " + Lang.Get("will ripen in {0} hours", Math.Round(freshHoursLeft, 1)));
				}
				break;
			}
			}
		}
		if (appendLine)
		{
			dsc.AppendLine();
		}
		return dsc.ToString();
	}
}
