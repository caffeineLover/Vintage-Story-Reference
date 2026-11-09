using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RPVoiceChat.Audio;
using RPVoiceChat.Config;
using RPVoiceChat.Gui;
using RPVoiceChat.Networking.Packets;
using RPVoiceChat.Systems;
using RPVoiceChat.Util;
using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.MathTools;
using Vintagestory.API.Server;

namespace RPVoiceChat.GameContent.Items
{
	// Token: 0x020000C7 RID: 199
	public class ItemRadio : Item
	{
		// Token: 0x060005F8 RID: 1528 RVA: 0x00020409 File Offset: 0x0001E609
		public override void OnLoaded(ICoreAPI api)
		{
			base.OnLoaded(api);
		}

		// Token: 0x060005F9 RID: 1529 RVA: 0x00020414 File Offset: 0x0001E614
		public override void GetHeldItemInfo(ItemSlot inSlot, StringBuilder dsc, IWorldAccessor world, bool withDebugInfo)
		{
			base.GetHeldItemInfo(inSlot, dsc, world, withDebugInfo);
			dsc.AppendLine(UIUtils.I18n("Radio.Talkie.Info.Frequency", new object[]
			{
				ItemRadio.GetTunedFrequency((inSlot != null) ? inSlot.Itemstack : null)
			}));
			if (ItemRadio.GetInventoryListen((inSlot != null) ? inSlot.Itemstack : null))
			{
				dsc.AppendLine(UIUtils.I18n("Radio.Talkie.Info.InventoryListen"));
			}
		}

		// Token: 0x060005FA RID: 1530 RVA: 0x0002047C File Offset: 0x0001E67C
		public override WorldInteraction[] GetHeldInteractionHelp(ItemSlot inSlot)
		{
			return new WorldInteraction[]
			{
				new WorldInteraction
				{
					ActionLangCode = RPVoiceChatMod.modID + ":Radio.Talkie.Interaction.Tune",
					HotKeyCode = "sneak",
					MouseButton = 2,
					ShouldApply = delegate(WorldInteraction wi, BlockSelection bs, EntitySelection es)
					{
						if (bs == null && es == null)
						{
							ICoreClientAPI capi = this.api as ICoreClientAPI;
							if (capi != null)
							{
								IClientWorldAccessor world = capi.World;
								if (world == null)
								{
									return false;
								}
								IClientPlayer player = world.Player;
								bool? flag;
								if (player == null)
								{
									flag = default(bool?);
								}
								else
								{
									EntityPlayer entity = player.Entity;
									if (entity == null)
									{
										flag = default(bool?);
									}
									else
									{
										EntityControls controls = entity.Controls;
										flag = ((controls != null) ? new bool?(controls.Sneak) : default(bool?));
									}
								}
								bool? flag2 = flag;
								return flag2.GetValueOrDefault();
							}
						}
						return false;
					}
				}
			};
		}

		// Token: 0x060005FB RID: 1531 RVA: 0x000204D0 File Offset: 0x0001E6D0
		public override void OnHeldInteractStart(ItemSlot slot, EntityAgent byEntity, BlockSelection blockSel, EntitySelection entitySel, bool firstEvent, ref EnumHandHandling handling)
		{
			if (!firstEvent)
			{
				handling = 4;
				return;
			}
			if (byEntity.Controls.Sneak)
			{
				ICoreClientAPI capi = byEntity.Api as ICoreClientAPI;
				if (capi != null && byEntity == capi.World.Player.Entity)
				{
					RadioTalkieDialog.OpenOrFocus(capi, slot);
				}
				handling = 4;
				return;
			}
			ICoreClientAPI capiClient = byEntity.Api as ICoreClientAPI;
			if (capiClient != null && byEntity == capiClient.World.Player.Entity)
			{
				RadioTalkieDialog.CloseIfOpen();
				MicrophoneManager microphoneManager = RPVoiceChatClient.MicrophoneManagerInstance;
				if (microphoneManager != null)
				{
					microphoneManager.SetTransmissionRange(ServerConfigManager.RadioTalkieRangeBlocks);
					microphoneManager.SetTalkieHeldTransmit(true);
				}
				this.isTransmitting = true;
				this.SendTalkieState(capiClient, slot, true);
			}
			handling = 4;
		}

		// Token: 0x060005FC RID: 1532 RVA: 0x00020578 File Offset: 0x0001E778
		public override void OnHeldInteractStop(float secondsUsed, ItemSlot slot, EntityAgent byEntity, BlockSelection blockSel, EntitySelection entitySel)
		{
			if (!this.isTransmitting)
			{
				return;
			}
			ICoreClientAPI capi = byEntity.Api as ICoreClientAPI;
			if (capi != null && byEntity == capi.World.Player.Entity)
			{
				MicrophoneManager microphoneManagerInstance = RPVoiceChatClient.MicrophoneManagerInstance;
				if (microphoneManagerInstance != null)
				{
					microphoneManagerInstance.SetTalkieHeldTransmit(false);
				}
				if (microphoneManagerInstance != null)
				{
					microphoneManagerInstance.ResetTransmissionRange();
				}
				this.SendTalkieState(capi, slot, false);
			}
			this.isTransmitting = false;
		}

		// Token: 0x060005FD RID: 1533 RVA: 0x000205DD File Offset: 0x0001E7DD
		public override bool OnHeldInteractStep(float secondsUsed, ItemSlot slot, EntityAgent byEntity, BlockSelection blockSel, EntitySelection entitySel)
		{
			return this.isTransmitting;
		}

		// Token: 0x060005FE RID: 1534 RVA: 0x000205E5 File Offset: 0x0001E7E5
		public override void OnHeldIdle(ItemSlot slot, EntityAgent byEntity)
		{
			if (this.isTransmitting)
			{
				this.OnHeldInteractStop(0f, slot, byEntity, null, null);
			}
		}

		// Token: 0x060005FF RID: 1535 RVA: 0x000205FE File Offset: 0x0001E7FE
		public static string GetTunedFrequency(ItemStack stack)
		{
			if (((stack != null) ? stack.Attributes : null) == null)
			{
				return "100.0";
			}
			return RadioFrequencyUtil.Normalize(stack.Attributes.GetString("rpvc:tunedFrequency", "100.0"));
		}

		// Token: 0x06000600 RID: 1536 RVA: 0x00020630 File Offset: 0x0001E830
		public static IEnumerable<string> GetActiveListenFrequencies(IPlayer player)
		{
			if (((player != null) ? player.Entity : null) == null)
			{
				return Enumerable.Empty<string>();
			}
			if (player is IServerPlayer)
			{
				EntityPlayer entity2 = player.Entity;
				ICoreServerAPI sapi = ((entity2 != null) ? entity2.Api : null) as ICoreServerAPI;
				if (sapi != null)
				{
					RadioTalkieTransmissionSystem talkieTransmission = sapi.ModLoader.GetModSystem<RadioTalkieTransmissionSystem>(true);
					if (talkieTransmission != null && talkieTransmission.IsTalkieTransmitting(player.PlayerUID))
					{
						return Enumerable.Empty<string>();
					}
				}
			}
			IEnumerable<string> result;
			try
			{
				EntityAgent entity = player.Entity;
				if (!entity.Alive)
				{
					result = Enumerable.Empty<string>();
				}
				else
				{
					HashSet<string> frequencies = new HashSet<string>();
					ItemRadio.CollectInventoryListenFrequencies(player, frequencies);
					ItemRadio.CollectListenFrequency(entity.RightHandItemSlot, true, frequencies);
					ItemRadio.CollectListenFrequency(entity.LeftHandItemSlot, true, frequencies);
					result = frequencies;
				}
			}
			catch
			{
				result = Enumerable.Empty<string>();
			}
			return result;
		}

		// Token: 0x06000601 RID: 1537 RVA: 0x000206FC File Offset: 0x0001E8FC
		public static float GetLocalTalkieListenVolumeGain(ICoreClientAPI capi)
		{
			IPlayer player2;
			if (capi == null)
			{
				player2 = null;
			}
			else
			{
				IClientWorldAccessor world = capi.World;
				player2 = ((world != null) ? world.Player : null);
			}
			IPlayer player = player2;
			if (((player != null) ? player.Entity : null) == null)
			{
				return 1f;
			}
			float maxGain = 0f;
			bool found = false;
			EntityPlayer entity = player.Entity;
			ItemRadio.ConsiderListenVolume(entity.RightHandItemSlot, true, ref maxGain, ref found);
			ItemRadio.ConsiderListenVolume(entity.LeftHandItemSlot, true, ref maxGain, ref found);
			if (player.InventoryManager != null)
			{
				ItemRadio.ConsiderInventoryListenVolume(player.InventoryManager.GetOwnInventory("hotbar"), ref maxGain, ref found);
				ItemRadio.ConsiderInventoryListenVolume(player.InventoryManager.GetOwnInventory("backpack"), ref maxGain, ref found);
			}
			if (!found)
			{
				return 1f;
			}
			return maxGain;
		}

		// Token: 0x06000602 RID: 1538 RVA: 0x000207A8 File Offset: 0x0001E9A8
		public static string GetTunedFrequency(IPlayer player)
		{
			using (IEnumerator<string> enumerator = ItemRadio.GetActiveListenFrequencies(player).GetEnumerator())
			{
				if (enumerator.MoveNext())
				{
					return enumerator.Current;
				}
			}
			return "";
		}

		// Token: 0x06000603 RID: 1539 RVA: 0x000207F8 File Offset: 0x0001E9F8
		public static bool IsTalkieActiveInHands(IPlayer player)
		{
			return ((player != null) ? player.Entity : null) != null && (ItemRadio.IsTalkieStack(player.Entity.RightHandItemSlot) || ItemRadio.IsTalkieStack(player.Entity.LeftHandItemSlot));
		}

		// Token: 0x06000604 RID: 1540 RVA: 0x0002082E File Offset: 0x0001EA2E
		public static void SetTunedFrequency(ItemStack stack, string frequency)
		{
			if (stack == null)
			{
				return;
			}
			stack.Attributes.SetString("rpvc:tunedFrequency", RadioFrequencyUtil.Normalize(frequency));
		}

		// Token: 0x06000605 RID: 1541 RVA: 0x0002084A File Offset: 0x0001EA4A
		public static bool GetInventoryListen(ItemStack stack)
		{
			return ((stack != null) ? stack.Attributes : null) != null && stack.Attributes.GetBool("rpvc:inventoryListen", false);
		}

		// Token: 0x06000606 RID: 1542 RVA: 0x0002086D File Offset: 0x0001EA6D
		public static void SetInventoryListen(ItemStack stack, bool enabled)
		{
			if (((stack != null) ? stack.Attributes : null) == null)
			{
				return;
			}
			stack.Attributes.SetBool("rpvc:inventoryListen", enabled);
		}

		// Token: 0x06000607 RID: 1543 RVA: 0x0002088F File Offset: 0x0001EA8F
		public static int GetListenVolumePercent(ItemStack stack)
		{
			if (((stack != null) ? stack.Attributes : null) == null)
			{
				return 100;
			}
			return GameMath.Clamp(stack.Attributes.GetInt("rpvc:listenVolume", 100), 0, 100);
		}

		// Token: 0x06000608 RID: 1544 RVA: 0x000208BC File Offset: 0x0001EABC
		public static void SetListenVolumePercent(ItemStack stack, int volumePercent)
		{
			if (((stack != null) ? stack.Attributes : null) == null)
			{
				return;
			}
			stack.Attributes.SetInt("rpvc:listenVolume", GameMath.Clamp(volumePercent, 0, 100));
		}

		// Token: 0x06000609 RID: 1545 RVA: 0x000208E8 File Offset: 0x0001EAE8
		public static bool TryApplyServerSettings(IServerPlayer player, int slotNumber, string frequency, bool inventoryListen, int listenVolumePercent)
		{
			if (((player != null) ? player.InventoryManager : null) == null)
			{
				return false;
			}
			IInventory hotbar = player.InventoryManager.GetOwnInventory("hotbar");
			if (hotbar == null)
			{
				return false;
			}
			ItemSlot slot = ItemRadio.FindTalkieHotbarSlot(hotbar, slotNumber);
			if (slot == null)
			{
				return false;
			}
			ItemRadio.SetTunedFrequency(slot.Itemstack, frequency);
			ItemRadio.SetInventoryListen(slot.Itemstack, inventoryListen);
			ItemRadio.SetListenVolumePercent(slot.Itemstack, listenVolumePercent);
			slot.MarkDirty();
			return true;
		}

		// Token: 0x0600060A RID: 1546 RVA: 0x00020954 File Offset: 0x0001EB54
		public static void SendTalkieSettings(ICoreClientAPI capi, ItemSlot slot)
		{
			if (capi == null || ((slot != null) ? slot.Itemstack : null) == null)
			{
				return;
			}
			ItemRadio.SendTalkieSettings(capi, slot, ItemRadio.GetTunedFrequency(slot.Itemstack), ItemRadio.GetInventoryListen(slot.Itemstack), ItemRadio.GetListenVolumePercent(slot.Itemstack));
		}

		// Token: 0x0600060B RID: 1547 RVA: 0x00020990 File Offset: 0x0001EB90
		public static void SendTalkieSettings(ICoreClientAPI capi, ItemSlot slot, string frequency, bool inventoryListen, int listenVolumePercent)
		{
			if (capi == null || slot == null)
			{
				return;
			}
			int hotbarSlotNumber = capi.World.Player.InventoryManager.ActiveHotbarSlotNumber;
			if (!ItemRadio.IsTalkieStack(capi.World.Player.InventoryManager.ActiveHotbarSlot))
			{
				hotbarSlotNumber = ItemRadio.ResolveHotbarSlotIndex(slot, capi.World.Player.InventoryManager.GetOwnInventory("hotbar"));
			}
			IClientNetworkChannel radioTalkieClientChannel = RPVoiceChatMod.RadioTalkieClientChannel;
			if (radioTalkieClientChannel == null)
			{
				return;
			}
			radioTalkieClientChannel.SendPacket<RadioTalkieSettingsPacket>(new RadioTalkieSettingsPacket
			{
				Frequency = RadioFrequencyUtil.Normalize(frequency),
				InventoryListen = inventoryListen,
				SlotNumber = hotbarSlotNumber,
				ListenVolumePercent = GameMath.Clamp(listenVolumePercent, 0, 100)
			});
		}

		// Token: 0x0600060C RID: 1548 RVA: 0x00020A38 File Offset: 0x0001EC38
		private static int ResolveHotbarSlotIndex(ItemSlot slot, IInventory hotbar)
		{
			if (slot == null || hotbar == null)
			{
				return -1;
			}
			if (slot.Inventory == hotbar)
			{
				for (int slotIndex = 0; slotIndex < hotbar.Count; slotIndex++)
				{
					if (hotbar[slotIndex] == slot)
					{
						return slotIndex;
					}
				}
			}
			for (int slotIndex2 = 0; slotIndex2 < hotbar.Count; slotIndex2++)
			{
				if (hotbar[slotIndex2] == slot)
				{
					return slotIndex2;
				}
			}
			return -1;
		}

		// Token: 0x0600060D RID: 1549 RVA: 0x00020A94 File Offset: 0x0001EC94
		private static ItemSlot FindTalkieHotbarSlot(IInventory hotbar, int preferredSlotNumber)
		{
			if (hotbar == null)
			{
				return null;
			}
			if (preferredSlotNumber >= 0 && preferredSlotNumber < hotbar.Count && ItemRadio.IsTalkieStack(hotbar[preferredSlotNumber]))
			{
				return hotbar[preferredSlotNumber];
			}
			for (int slotIndex = 0; slotIndex < hotbar.Count; slotIndex++)
			{
				if (ItemRadio.IsTalkieStack(hotbar[slotIndex]))
				{
					return hotbar[slotIndex];
				}
			}
			return null;
		}

		// Token: 0x0600060E RID: 1550 RVA: 0x00020AF4 File Offset: 0x0001ECF4
		private static void CollectListenFrequency(ItemSlot slot, bool fromHands, HashSet<string> frequencies)
		{
			if (!ItemRadio.IsTalkieStack(slot))
			{
				return;
			}
			if (!fromHands && !ItemRadio.GetInventoryListen(slot.Itemstack))
			{
				return;
			}
			string frequency = ItemRadio.GetTunedFrequency(slot.Itemstack);
			if (!string.IsNullOrEmpty(frequency))
			{
				frequencies.Add(frequency);
			}
		}

		// Token: 0x0600060F RID: 1551 RVA: 0x00020B37 File Offset: 0x0001ED37
		private static void CollectInventoryListenFrequencies(IPlayer player, HashSet<string> frequencies)
		{
			if (((player != null) ? player.InventoryManager : null) == null)
			{
				return;
			}
			ItemRadio.CollectInventorySlots(player.InventoryManager.GetOwnInventory("hotbar"), frequencies);
			ItemRadio.CollectInventorySlots(player.InventoryManager.GetOwnInventory("backpack"), frequencies);
		}

		// Token: 0x06000610 RID: 1552 RVA: 0x00020B74 File Offset: 0x0001ED74
		private static void CollectInventorySlots(IInventory inventory, HashSet<string> frequencies)
		{
			if (inventory == null)
			{
				return;
			}
			for (int slotIndex = 0; slotIndex < inventory.Count; slotIndex++)
			{
				ItemRadio.CollectListenFrequency(inventory[slotIndex], false, frequencies);
			}
		}

		// Token: 0x06000611 RID: 1553 RVA: 0x00020BA4 File Offset: 0x0001EDA4
		private static void ConsiderListenVolume(ItemSlot slot, bool fromHands, ref float maxGain, ref bool found)
		{
			if (!ItemRadio.IsTalkieStack(slot))
			{
				return;
			}
			if (!fromHands && !ItemRadio.GetInventoryListen(slot.Itemstack))
			{
				return;
			}
			found = true;
			maxGain = Math.Max(maxGain, (float)ItemRadio.GetListenVolumePercent(slot.Itemstack) / 100f);
		}

		// Token: 0x06000612 RID: 1554 RVA: 0x00020BE0 File Offset: 0x0001EDE0
		private static void ConsiderInventoryListenVolume(IInventory inventory, ref float maxGain, ref bool found)
		{
			if (inventory == null)
			{
				return;
			}
			for (int slotIndex = 0; slotIndex < inventory.Count; slotIndex++)
			{
				ItemRadio.ConsiderListenVolume(inventory[slotIndex], false, ref maxGain, ref found);
			}
		}

		// Token: 0x06000613 RID: 1555 RVA: 0x00020C14 File Offset: 0x0001EE14
		private static bool IsTalkieStack(ItemSlot slot)
		{
			ItemStack stack = (slot != null) ? slot.Itemstack : null;
			AssetLocation assetLocation;
			if (stack == null)
			{
				assetLocation = null;
			}
			else
			{
				CollectibleObject collectible = stack.Collectible;
				assetLocation = ((collectible != null) ? collectible.Code : null);
			}
			return assetLocation != null && stack.Collectible.Code.Domain == RPVoiceChatMod.modID && stack.Collectible.Code.Path == "radiotalkie";
		}

		// Token: 0x06000614 RID: 1556 RVA: 0x00020C86 File Offset: 0x0001EE86
		private void SendTalkieState(ICoreClientAPI capi, ItemSlot slot, bool transmitting)
		{
			IClientNetworkChannel radioTalkieClientChannel = RPVoiceChatMod.RadioTalkieClientChannel;
			if (radioTalkieClientChannel == null)
			{
				return;
			}
			radioTalkieClientChannel.SendPacket<RadioTalkieStatePacket>(new RadioTalkieStatePacket
			{
				Transmitting = transmitting,
				Frequency = ItemRadio.GetTunedFrequency((slot != null) ? slot.Itemstack : null)
			});
		}

		// Token: 0x0400030E RID: 782
		public const string TunedFrequencyAttribute = "rpvc:tunedFrequency";

		// Token: 0x0400030F RID: 783
		public const string InventoryListenAttribute = "rpvc:inventoryListen";

		// Token: 0x04000310 RID: 784
		public const string ListenVolumeAttribute = "rpvc:listenVolume";

		// Token: 0x04000311 RID: 785
		public const int MinListenVolumePercent = 0;

		// Token: 0x04000312 RID: 786
		public const int MaxListenVolumePercent = 100;

		// Token: 0x04000313 RID: 787
		public const int DefaultListenVolumePercent = 100;

		// Token: 0x04000314 RID: 788
		private const string DefaultFrequency = "100.0";

		// Token: 0x04000315 RID: 789
		private bool isTransmitting;
	}
}
