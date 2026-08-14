using System;
using System.Reflection;
using HarmonyLib;
using RPVoiceChat.src.Networking.Packets;
using Vintagestory.API.Common;
using Vintagestory.GameContent;

namespace RPVoiceChat
{
	// Token: 0x02000018 RID: 24
	internal class HammerWeldingPatch
	{
		// Token: 0x0600007D RID: 125 RVA: 0x00004784 File Offset: 0x00002984
		internal static void Patch(Harmony harmony)
		{
			MethodInfo originalOnHeldAttackStart = AccessTools.Method(typeof(ItemHammer), "OnHeldAttackStart", null, null);
			MethodInfo prefixOnHeldAttackStart = AccessTools.Method(typeof(HammerWeldingPatch), "OnHeldAttackStart", null, null);
			harmony.Patch(originalOnHeldAttackStart, new HarmonyMethod(prefixOnHeldAttackStart), null, null, null);
			MethodInfo originalOnHeldAttackStep = AccessTools.Method(typeof(ItemHammer), "OnHeldAttackStep", null, null);
			MethodInfo prefixOnHeldAttackStep = AccessTools.Method(typeof(HammerWeldingPatch), "OnHeldAttackStep", null, null);
			harmony.Patch(originalOnHeldAttackStep, new HarmonyMethod(prefixOnHeldAttackStep), null, null, null);
		}

		// Token: 0x0600007E RID: 126 RVA: 0x00004810 File Offset: 0x00002A10
		public static bool OnHeldAttackStart(ItemSlot slot, EntityAgent byEntity, BlockSelection blockSel, EntitySelection entitySel, ref EnumHandHandling handling)
		{
			HammerWeldingPatch.<>c__DisplayClass1_0 CS$<>8__locals1 = new HammerWeldingPatch.<>c__DisplayClass1_0();
			CS$<>8__locals1.byEntity = byEntity;
			if (blockSel == null)
			{
				return true;
			}
			BEWeldable bew = CS$<>8__locals1.byEntity.World.BlockAccessor.GetBlockEntity(blockSel.Position) as BEWeldable;
			if (bew == null)
			{
				return true;
			}
			handling = 4;
			HammerWeldingPatch.<>c__DisplayClass1_0 CS$<>8__locals2 = CS$<>8__locals1;
			EntityPlayer entityPlayer = CS$<>8__locals1.byEntity as EntityPlayer;
			CS$<>8__locals2.byPlayer = ((entityPlayer != null) ? entityPlayer.Player : null);
			if (CS$<>8__locals1.byPlayer == null)
			{
				return false;
			}
			bool flag = bew.TestReadyToMerge();
			HammerWeldingPatch.strikeBell(CS$<>8__locals1.byEntity, slot, slot.Itemstack);
			if (flag)
			{
				CS$<>8__locals1.byEntity.World.RegisterCallback(delegate(float dt)
				{
					if (CS$<>8__locals1.byEntity.Controls.HandUse == 1)
					{
						CS$<>8__locals1.byPlayer.Entity.World.PlaySoundAt(new AssetLocation("sounds/effect/anvilmergehit"), CS$<>8__locals1.byPlayer, CS$<>8__locals1.byPlayer, true, 32f, 1f);
					}
				}, 464);
			}
			return false;
		}

		// Token: 0x0600007F RID: 127 RVA: 0x000048C0 File Offset: 0x00002AC0
		public static bool OnHeldAttackStep(float secondsUsed, ItemSlot slot, EntityAgent byEntity, BlockSelection blockSelection, EntitySelection entitySel)
		{
			if (blockSelection == null)
			{
				return true;
			}
			BEWeldable bew = byEntity.World.BlockAccessor.GetBlockEntity(blockSelection.Position) as BEWeldable;
			return bew == null || bew.TestReadyToMerge();
		}

		// Token: 0x06000080 RID: 128 RVA: 0x00004900 File Offset: 0x00002B00
		protected static void strikeBell(EntityAgent byEntity, ItemSlot slot, ItemStack strikingItem)
		{
			EntityPlayer entityPlayer = byEntity as EntityPlayer;
			IPlayer byPlayer = (entityPlayer != null) ? entityPlayer.Player : null;
			if (byPlayer == null)
			{
				return;
			}
			BlockSelection blockSel = byPlayer.CurrentBlockSelection;
			if (blockSel == null)
			{
				return;
			}
			if (strikingItem != byPlayer.InventoryManager.ActiveHotbarSlot.Itemstack)
			{
				return;
			}
			if (RPVoiceChatMod.ClientChannel != null)
			{
				RPVoiceChatMod.ClientChannel.SendPacket<WeldingHitPacket>(new WeldingHitPacket
				{
					Pos = blockSel.Position,
					HitPosition = blockSel.HitPosition
				});
			}
			ItemStack itemstack = slot.Itemstack;
			if (itemstack == null)
			{
				return;
			}
			itemstack.TempAttributes.SetBool("isBellAction", false);
		}
	}
}
