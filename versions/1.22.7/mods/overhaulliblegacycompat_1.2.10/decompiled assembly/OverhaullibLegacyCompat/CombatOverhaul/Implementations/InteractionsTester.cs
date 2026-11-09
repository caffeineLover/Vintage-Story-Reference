using System;
using System.Linq;
using System.Runtime.CompilerServices;
using CombatOverhaul.Inputs;
using CombatOverhaul.Integration;
using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.GameContent;

namespace CombatOverhaul.Implementations
{
	// Token: 0x0200009A RID: 154
	[NullableContext(1)]
	[Nullable(0)]
	public static class InteractionsTester
	{
		// Token: 0x0600084C RID: 2124 RVA: 0x000302BC File Offset: 0x0002E4BC
		public static bool PlayerTriesToInteract(EntityPlayer player, bool mainHand, ActionEventData eventData)
		{
			ItemSlot itemSlot;
			if (!mainHand)
			{
				itemSlot = player.LeftHandItemSlot;
			}
			else
			{
				ItemSlot activeHandItemSlot = player.ActiveHandItemSlot;
				itemSlot = ((((activeHandItemSlot != null) ? activeHandItemSlot.Itemstack : null) != null) ? player.ActiveHandItemSlot : player.RightHandItemSlot);
			}
			ItemSlot currentSlot = itemSlot;
			if (GrindingWheelCompat.PlayerTriesToUseGrindingWheel(player, currentSlot, eventData, mainHand))
			{
				return true;
			}
			if (!eventData.AltPressed)
			{
				return false;
			}
			if (!InteractionsTester.PlayerTriesToPutItemOnGround(player, mainHand, eventData) && !InteractionsTester.PlayerTriesToPutItemIntoRack(player, mainHand, eventData) && !InteractionsTester.PlayerTriesToPutItemIntoGroundStorage(player, mainHand, eventData))
			{
				BlockSelection blockSelection = player.BlockSelection;
				return ((blockSelection != null) ? blockSelection.Block : null) != null;
			}
			return true;
		}

		// Token: 0x0600084D RID: 2125 RVA: 0x00030344 File Offset: 0x0002E544
		public static bool PlayerTriesToPutItemIntoRack(EntityPlayer player, bool mainHand, ActionEventData eventData)
		{
			BlockSelection blockSelection = player.BlockSelection;
			Block selectedBlock = (blockSelection != null) ? blockSelection.Block : null;
			return selectedBlock != null && Enumerable.Any<WorldInteraction>(selectedBlock.GetPlacedBlockInteractionHelp(player.Api.World, player.BlockSelection, player.Player), (WorldInteraction interaction) => InteractionsTester.IsWorldInteractionBlocking(interaction, player, mainHand, eventData));
		}

		// Token: 0x0600084E RID: 2126 RVA: 0x000303C8 File Offset: 0x0002E5C8
		public static bool PlayerTriesToPutItemOnGround(EntityPlayer player, bool mainHand, ActionEventData eventData)
		{
			if (!mainHand)
			{
				return false;
			}
			BlockSelection blockSelection = player.BlockSelection;
			bool flag = ((blockSelection != null) ? blockSelection.Block : null) != null;
			bool modifiersPressed = Enumerable.Contains<EnumEntityAction>(eventData.Modifiers, 14) && Enumerable.Contains<EnumEntityAction>(eventData.Modifiers, 13);
			return flag && modifiersPressed;
		}

		// Token: 0x0600084F RID: 2127 RVA: 0x00030414 File Offset: 0x0002E614
		public static bool IsWorldInteractionBlocking(WorldInteraction interaction, EntityPlayer player, bool mainHand, ActionEventData eventData)
		{
			bool sameMouseButton = (eventData.Action.Action == 9 && interaction.MouseButton == null) || (eventData.Action.Action == 10 && interaction.MouseButton == 2);
			ItemSlot itemSlot;
			if (!mainHand)
			{
				itemSlot = player.LeftHandItemSlot;
			}
			else
			{
				ItemSlot activeHandItemSlot = player.ActiveHandItemSlot;
				itemSlot = ((((activeHandItemSlot != null) ? activeHandItemSlot.Itemstack : null) != null) ? player.ActiveHandItemSlot : player.RightHandItemSlot);
			}
			ItemSlot currentSlot = itemSlot;
			ItemStack currentStack = (currentSlot != null) ? currentSlot.Itemstack : null;
			return (currentStack == null || interaction.Itemstacks == null || interaction.Itemstacks.Length == 0 || Enumerable.Any<ItemStack>(interaction.Itemstacks, delegate(ItemStack stack)
			{
				int? num;
				if (stack == null)
				{
					num = default(int?);
				}
				else
				{
					Item item = stack.Item;
					num = ((item != null) ? new int?(item.Id) : default(int?));
				}
				int? num2 = num;
				Item item2 = currentStack.Item;
				int? num3 = (item2 != null) ? new int?(item2.Id) : default(int?);
				return num2.GetValueOrDefault() == num3.GetValueOrDefault() & num2 != null == (num3 != null);
			})) && (sameMouseButton && mainHand) && !interaction.RequireFreeHand;
		}

		// Token: 0x06000850 RID: 2128 RVA: 0x000304E1 File Offset: 0x0002E6E1
		public static bool PlayerTriesToPutItemIntoGroundStorage(EntityPlayer player, bool mainHand, ActionEventData eventData)
		{
			BlockSelection blockSelection = player.BlockSelection;
			return ((blockSelection != null) ? blockSelection.Block : null) is BlockGroundStorage && Enumerable.Contains<EnumEntityAction>(eventData.Modifiers, 13);
		}
	}
}
