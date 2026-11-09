using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Reflection.Emit;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using CombatOverhaul.Armor;
using CombatOverhaul.Implementations;
using CombatOverhaul.Utils;
using HarmonyLib;
using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.Common.Entities;
using Vintagestory.API.Config;
using Vintagestory.API.MathTools;
using Vintagestory.GameContent;

namespace CombatOverhaul.Integration
{
	// Token: 0x02000046 RID: 70
	[NullableContext(1)]
	[Nullable(0)]
	internal static class HarmonyPatches
	{
		// Token: 0x170000C9 RID: 201
		// (get) Token: 0x06000442 RID: 1090 RVA: 0x00018483 File Offset: 0x00016683
		// (set) Token: 0x06000443 RID: 1091 RVA: 0x0001848A File Offset: 0x0001668A
		public static Settings ClientSettings { get; set; } = new Settings();

		// Token: 0x170000CA RID: 202
		// (get) Token: 0x06000444 RID: 1092 RVA: 0x00018492 File Offset: 0x00016692
		// (set) Token: 0x06000445 RID: 1093 RVA: 0x00018499 File Offset: 0x00016699
		public static Settings ServerSettings { get; set; } = new Settings();

		// Token: 0x06000446 RID: 1094 RVA: 0x000184A4 File Offset: 0x000166A4
		public static void Patch(string harmonyId, ICoreAPI api)
		{
			HarmonyPatches._api = api;
			HarmonyPatches._reportedEntities.Clear();
			HarmonyPatches._wearableLightHsvCache.Clear();
			Harmony harmony = new Harmony(harmonyId);
			harmony.Patch(typeof(AnimationManager).GetMethod("OnClientFrame", AccessTools.all), new HarmonyMethod(AccessTools.Method(typeof(HarmonyPatches), "CreateColliders", null, null)), null, null, null);
			harmony.Patch(typeof(EntityPlayerShapeRenderer).GetMethod("smoothCameraTurning", AccessTools.all), new HarmonyMethod(AccessTools.Method(typeof(HarmonyPatches), "SmoothCameraTurning", null, null)), null, null, null);
			if (!api.ModLoader.IsModEnabled("svanaxfdc"))
			{
				harmony.Patch(typeof(EntityBehaviorHealth).GetMethod("OnFallToGround", AccessTools.all), new HarmonyMethod(AccessTools.Method(typeof(HarmonyPatches), "OnFallToGround", null, null)), null, null, null);
			}
			harmony.Patch(typeof(BagInventory).GetMethod("ReloadBagInventory", AccessTools.all), new HarmonyMethod(AccessTools.Method(typeof(HarmonyPatches), "ReloadBagInventory", null, null)), null, null, null);
			MethodInfo lightHsvGetter = HarmonyPatches.GetEntityPlayerLightHsvGetter();
			if (lightHsvGetter == null)
			{
				api.Logger.Warning("[OverhaullibLegacyCompat] Could not find EntityPlayer.LightHsv getter. Wearable lights will not work.");
			}
			else
			{
				api.Logger.Notification("[OverhaullibLegacyCompat] Patching EntityPlayer.LightHsv getter for wearable lights.");
				harmony.Patch(lightHsvGetter, null, new HarmonyMethod(AccessTools.Method(typeof(HarmonyPatches), "LightHsv", null, null)), null, null);
			}
			harmony.Patch(typeof(BagInventory).GetMethod("SaveSlotIntoBag", AccessTools.all), new HarmonyMethod(AccessTools.Method(typeof(HarmonyPatches), "BagInventory_SaveSlotIntoBag", null, null)), null, null, null);
			HarmonyPatches.TryPatchStopRaiseShieldAnim(harmony, api);
			HarmonyPatches.TryPatchVanillaShieldIdle(harmony, api);
			HarmonyPatches.TryPatchVanillaShieldTooltip(harmony, api);
			HarmonyPatches.TryPatchOffhandDaggerSlot(harmony, api);
			HarmonyPatches.TryPatchVanillaArmorStandInteract(harmony, api);
			HarmonyPatches.TryPatchVanillaBowInteract(harmony, api);
		}

		// Token: 0x06000447 RID: 1095 RVA: 0x0001869C File Offset: 0x0001689C
		public static void Unpatch(string harmonyId, ICoreAPI api)
		{
			Harmony harmony = new Harmony(harmonyId);
			harmony.Unpatch(typeof(AnimationManager).GetMethod("OnClientFrame", AccessTools.all), 1, harmonyId);
			harmony.Unpatch(typeof(EntityPlayerShapeRenderer).GetMethod("smoothCameraTurning", AccessTools.all), 1, harmonyId);
			harmony.Unpatch(typeof(BagInventory).GetMethod("ReloadBagInventory", AccessTools.all), 1, harmonyId);
			MethodInfo lightHsvGetter = HarmonyPatches.GetEntityPlayerLightHsvGetter();
			if (lightHsvGetter != null)
			{
				harmony.Unpatch(lightHsvGetter, 2, harmonyId);
			}
			harmony.Unpatch(typeof(BagInventory).GetMethod("SaveSlotIntoBag", AccessTools.all), 1, harmonyId);
			HarmonyPatches.TryUnpatchStopRaiseShieldAnim(harmony, harmonyId);
			HarmonyPatches.TryUnpatchVanillaShieldIdle(harmony, harmonyId);
			HarmonyPatches.TryUnpatchVanillaShieldTooltip(harmony, harmonyId);
			HarmonyPatches.TryUnpatchOffhandDaggerSlot(harmony, harmonyId);
			HarmonyPatches.TryUnpatchVanillaArmorStandInteract(harmony, harmonyId);
			HarmonyPatches.TryUnpatchVanillaBowInteract(harmony, harmonyId);
			if (!api.ModLoader.IsModEnabled("svanaxfdc"))
			{
				harmony.Unpatch(typeof(EntityBehaviorHealth).GetMethod("OnFallToGround", AccessTools.all), 1, harmonyId);
			}
			HarmonyPatches._api = null;
			HarmonyPatches._wearableLightHsvCache.Clear();
		}

		// Token: 0x06000448 RID: 1096 RVA: 0x000187BC File Offset: 0x000169BC
		private static void TryPatchVanillaBowInteract(Harmony harmony, ICoreAPI api)
		{
			MethodInfo method = AccessTools.Method(typeof(ItemBow), "OnHeldInteractStart", new Type[]
			{
				typeof(ItemSlot),
				typeof(EntityAgent),
				typeof(BlockSelection),
				typeof(EntitySelection),
				typeof(bool),
				typeof(EnumHandHandling).MakeByRefType()
			}, null);
			if (method == null)
			{
				api.Logger.Warning("[OverhaullibLegacyCompat] Could not find vanilla ItemBow.OnHeldInteractStart. Vanilla-style bows may ignore the bow two-handed setting.");
				return;
			}
			harmony.Patch(method, new HarmonyMethod(AccessTools.Method(typeof(HarmonyPatches), "VanillaBow_OnHeldInteractStart_Prefix", null, null)), null, null, null);
		}

		// Token: 0x06000449 RID: 1097 RVA: 0x00018878 File Offset: 0x00016A78
		private static void TryUnpatchVanillaBowInteract(Harmony harmony, string harmonyId)
		{
			MethodInfo method = AccessTools.Method(typeof(ItemBow), "OnHeldInteractStart", new Type[]
			{
				typeof(ItemSlot),
				typeof(EntityAgent),
				typeof(BlockSelection),
				typeof(EntitySelection),
				typeof(bool),
				typeof(EnumHandHandling).MakeByRefType()
			}, null);
			if (method != null)
			{
				harmony.Unpatch(method, 1, harmonyId);
			}
		}

		// Token: 0x0600044A RID: 1098 RVA: 0x00018908 File Offset: 0x00016B08
		private static bool VanillaBow_OnHeldInteractStart_Prefix(ItemSlot slot, EntityAgent byEntity, ref EnumHandHandling handling)
		{
			if (((byEntity.World.Side == 2) ? HarmonyPatches.ClientSettings : HarmonyPatches.ServerSettings).BowTwoHanded)
			{
				EntityPlayer player = byEntity as EntityPlayer;
				if (player != null)
				{
					bool mainHand = HarmonyPatches.IsSameHandSlot(slot, player.RightHandItemSlot);
					bool offHand = HarmonyPatches.IsSameHandSlot(slot, player.LeftHandItemSlot);
					if (!mainHand && !offHand)
					{
						mainHand = true;
					}
					if (mainHand && !player.LeftHandItemSlot.Empty)
					{
						ICoreClientAPI coreClientAPI = player.World.Api as ICoreClientAPI;
						if (coreClientAPI != null)
						{
							coreClientAPI.TriggerIngameError(typeof(ItemBow), "offhandShouldBeEmpty", Lang.Get("combatoverhaul:message-offhand-empty", Array.Empty<object>()));
						}
						handling = 4;
						return false;
					}
					if (offHand && !player.RightHandItemSlot.Empty)
					{
						ICoreClientAPI coreClientAPI2 = player.World.Api as ICoreClientAPI;
						if (coreClientAPI2 != null)
						{
							coreClientAPI2.TriggerIngameError(typeof(ItemBow), "mainHandShouldBeEmpty", Lang.Get("combatoverhaul:message-mainhand-empty", Array.Empty<object>()));
						}
						handling = 4;
						return false;
					}
					return true;
				}
			}
			return true;
		}

		// Token: 0x0600044B RID: 1099 RVA: 0x00018A01 File Offset: 0x00016C01
		private static bool IsSameHandSlot(ItemSlot slot, ItemSlot handSlot)
		{
			return slot == handSlot || (slot.Itemstack != null && slot.Itemstack == handSlot.Itemstack);
		}

		// Token: 0x0600044C RID: 1100 RVA: 0x00018A24 File Offset: 0x00016C24
		private static void TryPatchVanillaArmorStandInteract(Harmony harmony, ICoreAPI api)
		{
			MethodInfo method = HarmonyPatches.GetVanillaArmorStandOnInteractMethod();
			if (method == null)
			{
				api.Logger.Warning("[OverhaullibLegacyCompat] Could not find vanilla EntityArmorStand.OnInteract. Ruin armor stands may keep vanilla-only interaction.");
				return;
			}
			harmony.Patch(method, new HarmonyMethod(AccessTools.Method(typeof(HarmonyPatches), "VanillaArmorStand_OnInteract_Prefix", null, null)), null, null, null);
		}

		// Token: 0x0600044D RID: 1101 RVA: 0x00018A78 File Offset: 0x00016C78
		private static void TryUnpatchVanillaArmorStandInteract(Harmony harmony, string harmonyId)
		{
			MethodInfo method = HarmonyPatches.GetVanillaArmorStandOnInteractMethod();
			if (method != null)
			{
				harmony.Unpatch(method, 1, harmonyId);
			}
		}

		// Token: 0x0600044E RID: 1102 RVA: 0x00018AA0 File Offset: 0x00016CA0
		[NullableContext(2)]
		private static MethodInfo GetVanillaArmorStandOnInteractMethod()
		{
			return AccessTools.Method(typeof(EntityArmorStand), "OnInteract", new Type[]
			{
				typeof(EntityAgent),
				typeof(ItemSlot),
				typeof(Vec3d),
				typeof(EnumInteractMode)
			}, null);
		}

		// Token: 0x0600044F RID: 1103 RVA: 0x00018AFC File Offset: 0x00016CFC
		private static bool VanillaArmorStand_OnInteract_Prefix(EntityArmorStand __instance, EntityAgent byEntity, ItemSlot slot, Vec3d hitPosition, EnumInteractMode mode)
		{
			EntityBehaviorCOArmorStandInventory behavior = __instance.GetBehavior<EntityBehaviorCOArmorStandInventory>();
			return behavior == null || !behavior.HandleInteract(byEntity, slot, hitPosition, mode);
		}

		// Token: 0x06000450 RID: 1104 RVA: 0x00018B24 File Offset: 0x00016D24
		private static void TryPatchOffhandDaggerSlot(Harmony harmony, ICoreAPI api)
		{
			HarmonyPatches._offhandSlotType = (AccessTools.TypeByName("Vintagestory.GameContent.ItemSlotOffhand") ?? AccessTools.TypeByName("Vintagestory.API.Common.ItemSlotOffhand"));
			if (HarmonyPatches._offhandSlotType == null)
			{
				api.Logger.Warning("[OverhaullibLegacyCompat] Could not find offhand item slot type. Offhand daggers may not be placeable.");
				return;
			}
			MethodInfo methodInfo;
			if ((methodInfo = AccessTools.DeclaredMethod(HarmonyPatches._offhandSlotType, "CanTakeFrom", new Type[]
			{
				typeof(ItemSlot),
				typeof(EnumMergePriority)
			}, null)) == null)
			{
				methodInfo = AccessTools.Method(typeof(ItemSlot), "CanTakeFrom", new Type[]
				{
					typeof(ItemSlot),
					typeof(EnumMergePriority)
				}, null);
			}
			MethodInfo canTakeFrom = methodInfo;
			if (canTakeFrom != null)
			{
				harmony.Patch(canTakeFrom, new HarmonyMethod(AccessTools.Method(typeof(HarmonyPatches), "OffhandDaggerSlot_CanTakeFrom_Prefix", null, null)), null, null, null);
			}
			MethodInfo methodInfo2;
			if ((methodInfo2 = AccessTools.DeclaredMethod(HarmonyPatches._offhandSlotType, "CanHold", new Type[]
			{
				typeof(ItemSlot)
			}, null)) == null)
			{
				methodInfo2 = AccessTools.Method(typeof(ItemSlot), "CanHold", new Type[]
				{
					typeof(ItemSlot)
				}, null);
			}
			MethodInfo canHold = methodInfo2;
			if (canHold != null)
			{
				harmony.Patch(canHold, new HarmonyMethod(AccessTools.Method(typeof(HarmonyPatches), "OffhandDaggerSlot_CanHold_Prefix", null, null)), null, null, null);
			}
		}

		// Token: 0x06000451 RID: 1105 RVA: 0x00018C84 File Offset: 0x00016E84
		private static void TryUnpatchOffhandDaggerSlot(Harmony harmony, string harmonyId)
		{
			object obj;
			if ((obj = ((HarmonyPatches._offhandSlotType == null) ? null : AccessTools.DeclaredMethod(HarmonyPatches._offhandSlotType, "CanTakeFrom", new Type[]
			{
				typeof(ItemSlot),
				typeof(EnumMergePriority)
			}, null))) == null)
			{
				obj = AccessTools.Method(typeof(ItemSlot), "CanTakeFrom", new Type[]
				{
					typeof(ItemSlot),
					typeof(EnumMergePriority)
				}, null);
			}
			MethodInfo canTakeFrom = obj;
			if (canTakeFrom != null)
			{
				harmony.Unpatch(canTakeFrom, 1, harmonyId);
			}
			object obj2;
			if ((obj2 = ((HarmonyPatches._offhandSlotType == null) ? null : AccessTools.DeclaredMethod(HarmonyPatches._offhandSlotType, "CanHold", new Type[]
			{
				typeof(ItemSlot)
			}, null))) == null)
			{
				obj2 = AccessTools.Method(typeof(ItemSlot), "CanHold", new Type[]
				{
					typeof(ItemSlot)
				}, null);
			}
			MethodInfo canHold = obj2;
			if (canHold != null)
			{
				harmony.Unpatch(canHold, 1, harmonyId);
			}
			HarmonyPatches._offhandSlotType = null;
		}

		// Token: 0x06000452 RID: 1106 RVA: 0x00018D95 File Offset: 0x00016F95
		private static bool OffhandDaggerSlot_CanTakeFrom_Prefix(ItemSlot __instance, ItemSlot sourceSlot, ref bool __result)
		{
			if (HarmonyPatches.IsOffhandSlot(__instance) && HarmonyPatches.IsOffhandDagger((sourceSlot != null) ? sourceSlot.Itemstack : null))
			{
				__result = true;
				return false;
			}
			return true;
		}

		// Token: 0x06000453 RID: 1107 RVA: 0x00018DB8 File Offset: 0x00016FB8
		private static bool OffhandDaggerSlot_CanHold_Prefix(ItemSlot __instance, ItemSlot sourceSlot, ref bool __result)
		{
			if (HarmonyPatches.IsOffhandSlot(__instance) && HarmonyPatches.IsOffhandDagger((sourceSlot != null) ? sourceSlot.Itemstack : null))
			{
				__result = true;
				return false;
			}
			return true;
		}

		// Token: 0x06000454 RID: 1108 RVA: 0x00018DDC File Offset: 0x00016FDC
		private static bool IsOffhandSlot(ItemSlot slot)
		{
			Type slotType = slot.GetType();
			if (HarmonyPatches._offhandSlotType != null && HarmonyPatches._offhandSlotType.IsAssignableFrom(slotType))
			{
				return true;
			}
			string fullName = slotType.FullName ?? string.Empty;
			return fullName == "Vintagestory.GameContent.ItemSlotOffhand" || fullName == "Vintagestory.API.Common.ItemSlotOffhand" || fullName.EndsWith(".ItemSlotOffhand", 4);
		}

		// Token: 0x06000455 RID: 1109 RVA: 0x00018E44 File Offset: 0x00017044
		[NullableContext(2)]
		private static bool IsOffhandDagger(ItemStack stack)
		{
			return CollectibleClassifier.IsDagger(stack);
		}

		// Token: 0x06000456 RID: 1110 RVA: 0x00018E4C File Offset: 0x0001704C
		[NullableContext(2)]
		private static MethodInfo GetEntityPlayerLightHsvGetter()
		{
			MethodInfo result;
			if ((result = AccessTools.PropertyGetter(typeof(EntityPlayer), "LightHsv")) == null)
			{
				PropertyInfo property = typeof(EntityPlayer).GetProperty("LightHsv", AccessTools.all);
				if (property == null)
				{
					return null;
				}
				result = property.GetGetMethod(true);
			}
			return result;
		}

		// Token: 0x06000457 RID: 1111 RVA: 0x00018E8C File Offset: 0x0001708C
		[return: Nullable(new byte[]
		{
			1,
			1,
			2
		})]
		private static Func<AnimationManager, Entity> CreateAnimationManagerEntityGetter()
		{
			FieldInfo entityField = typeof(AnimationManager).GetField("entity", 36);
			if (entityField == null)
			{
				return (AnimationManager _) => null;
			}
			Func<AnimationManager, Entity> result;
			try
			{
				ParameterExpression instance2;
				result = Expression.Lambda<Func<AnimationManager, Entity>>(Expression.Convert(Expression.Field(instance2, entityField), typeof(Entity)), new ParameterExpression[]
				{
					instance2
				}).Compile();
			}
			catch
			{
				result = ((AnimationManager instance) => entityField.GetValue(instance) as Entity);
			}
			return result;
		}

		// Token: 0x06000458 RID: 1112 RVA: 0x00018F54 File Offset: 0x00017154
		private static bool CreateColliders(AnimationManager __instance, float dt)
		{
			EntityPlayer entity = HarmonyPatches._animationManagerEntityGetter.Invoke(__instance) as EntityPlayer;
			bool flag;
			if (entity == null)
			{
				flag = true;
			}
			else
			{
				ICoreAPI api = entity.Api;
				flag = (((api != null) ? new EnumAppSide?(api.Side) : default(EnumAppSide?)).GetValueOrDefault() != 2);
			}
			if (flag)
			{
				return true;
			}
			ClientAnimator animator = __instance.Animator as ClientAnimator;
			if (animator == null)
			{
				return true;
			}
			AnimationPatches.AnimatorPlayerMap animators = AnimationPatches.Animators;
			if (animators != null)
			{
				animators.Add(animator, entity);
			}
			return true;
		}

		// Token: 0x06000459 RID: 1113 RVA: 0x00018FCD File Offset: 0x000171CD
		private static bool SmoothCameraTurning(EntityPlayerShapeRenderer __instance, float bodyYaw, float mdt)
		{
			if (!HarmonyPatches.ClientSettings.HandsYawSmoothing)
			{
				FieldInfo smoothedBodyYaw = HarmonyPatches._smoothedBodyYaw;
				if (smoothedBodyYaw != null)
				{
					smoothedBodyYaw.SetValue(__instance, bodyYaw);
				}
				return false;
			}
			return true;
		}

		// Token: 0x0600045A RID: 1114 RVA: 0x00018FF8 File Offset: 0x000171F8
		private static bool OnFallToGround(EntityBehaviorHealth __instance, ref double withYMotion)
		{
			EntityAgent entityAgent = __instance.entity as EntityAgent;
			if (entityAgent != null && entityAgent.ServerControls.Gliding)
			{
				return true;
			}
			EntityPlayer player = __instance.entity as EntityPlayer;
			if (player == null)
			{
				return true;
			}
			double fallDistance = (__instance.entity.PositionBeforeFalling.Y - player.Pos.Y) / Math.Max((double)player.Stats.GetBlended("fallDamageThreshold"), 0.001);
			if (fallDistance < 4.5)
			{
				return false;
			}
			if (Math.Abs(withYMotion) < 0.10000000149011612)
			{
				return false;
			}
			double fallDamage = Math.Max(0.0, fallDistance - 4.5) * (double)player.Properties.FallDamageMultiplier * 0.20000000298023224;
			player.ReceiveDamage(new DamageSource
			{
				Source = 2,
				Type = 0,
				IgnoreInvFrames = true
			}, (float)fallDamage);
			return false;
		}

		// Token: 0x0600045B RID: 1115 RVA: 0x000190EC File Offset: 0x000172EC
		private unsafe static double CurrentBelowBlockHeight(EntityAgent player)
		{
			double height = player.SidedPos.Y;
			IBlockAccessor accessor = HarmonyPatches._api.World.GetBlockAccessor(false, false, false, false);
			int heightDiff = 1;
			while ((double)heightDiff < height)
			{
				BlockPos blockPos = player.SidedPos.AsBlockPos;
				blockPos.Y -= heightDiff;
				BlockPos bp0 = blockPos.Copy();
				BlockPos bp = blockPos.Copy();
				BlockPos bp2 = blockPos.Copy();
				BlockPos bp3 = blockPos.Copy();
				Vec3d entityPosPos = player.SidedPos.XYZ;
				float xDiff = player.CollisionBox.XSize / 2f;
				float zDiff = player.CollisionBox.ZSize / 2f;
				bp0.X = (int)(entityPosPos.X - (double)xDiff);
				bp0.Z = (int)(entityPosPos.Z - (double)zDiff);
				bp.X = (int)(entityPosPos.X + (double)xDiff);
				bp.Z = (int)(entityPosPos.Z - (double)zDiff);
				bp2.X = (int)(entityPosPos.X - (double)xDiff);
				bp2.Z = (int)(entityPosPos.Z + (double)zDiff);
				bp3.X = (int)(entityPosPos.X + (double)xDiff);
				bp3.Z = (int)(entityPosPos.Z + (double)zDiff);
				int num = 1;
				List<Block> list = new List<Block>(num);
				CollectionsMarshal.SetCount<Block>(list, num);
				*CollectionsMarshal.AsSpan<Block>(list)[0] = accessor.GetBlock(bp0);
				List<Block> blocks = list;
				if (bp0 != bp)
				{
					blocks.Add(accessor.GetBlock(bp));
				}
				if (bp0 != bp2)
				{
					blocks.Add(accessor.GetBlock(bp2));
				}
				if (bp0 != bp3)
				{
					blocks.Add(accessor.GetBlock(bp3));
				}
				if (blocks.Exists((Block block) => ((block != null) ? block.CollisionBoxes : null) != null && block.CollisionBoxes.Length != 0))
				{
					return (double)((float)blockPos.Y + Enumerable.Max(Enumerable.Select<Block, float>(Enumerable.Where<Block>(blocks, (Block block) => ((block != null) ? block.CollisionBoxes : null) != null && block.CollisionBoxes.Length != 0), (Block block) => Enumerable.Max(Enumerable.Select<Cuboidf, float>(block.CollisionBoxes, (Cuboidf box) => box.MaxY)))));
				}
				heightDiff++;
			}
			return 0.0;
		}

		// Token: 0x0600045C RID: 1116 RVA: 0x00019338 File Offset: 0x00017538
		private static void ReloadBagInventory(BagInventory __instance, ref InventoryBase parentinv, ref ItemSlot[] bagSlots)
		{
			InventoryBasePlayer inventory = parentinv as InventoryBasePlayer;
			if (inventory == null)
			{
				return;
			}
			HarmonyPatches.InvalidateWearableLightHsvCache(inventory.Owner);
			bagSlots = HarmonyPatches.AppendGearInventorySlots(bagSlots, inventory.Owner);
			if (bagSlots.Length == 4)
			{
				bagSlots = Enumerable.ToArray<ItemSlot>(Enumerable.Concat<ItemSlot>(Enumerable.Select<int, ItemSlot>(Enumerable.Range(0, ArmorInventory._totalSlotsNumber), (int _) => new DummySlot()), bagSlots));
			}
		}

		// Token: 0x0600045D RID: 1117 RVA: 0x000193B0 File Offset: 0x000175B0
		private static ItemSlot[] AppendGearInventorySlots(ItemSlot[] backpackSlots, Entity owner)
		{
			IInventory inventory = HarmonyPatches.GetGearInventory(owner);
			if (inventory == null)
			{
				return backpackSlots;
			}
			if (Enumerable.Any<ItemSlot>(backpackSlots, (ItemSlot slot) => slot.Inventory == inventory))
			{
				return backpackSlots;
			}
			return Enumerable.ToArray<ItemSlot>(Enumerable.Concat<ItemSlot>(Enumerable.ToArray<ItemSlot>(inventory), backpackSlots));
		}

		// Token: 0x0600045E RID: 1118 RVA: 0x00019405 File Offset: 0x00017605
		[return: Nullable(2)]
		private static IInventory GetGearInventory(Entity entity)
		{
			EntityPlayer entityPlayer = entity as EntityPlayer;
			if (entityPlayer == null)
			{
				return null;
			}
			IPlayer player = entityPlayer.Player;
			if (player == null)
			{
				return null;
			}
			IPlayerInventoryManager inventoryManager = player.InventoryManager;
			if (inventoryManager == null)
			{
				return null;
			}
			return inventoryManager.GetOwnInventory("character");
		}

		// Token: 0x0600045F RID: 1119 RVA: 0x00019433 File Offset: 0x00017633
		[return: Nullable(2)]
		private static IInventory GetBackpackInventory(EntityPlayer player)
		{
			IPlayer player2 = player.Player;
			if (player2 == null)
			{
				return null;
			}
			IPlayerInventoryManager inventoryManager = player2.InventoryManager;
			if (inventoryManager == null)
			{
				return null;
			}
			return inventoryManager.GetOwnInventory("backpack");
		}

		// Token: 0x06000460 RID: 1120 RVA: 0x00019458 File Offset: 0x00017658
		private static bool BagInventory_SaveSlotIntoBag(BagInventory __instance, ItemSlotBagContent slot)
		{
			InventoryBasePlayer inventoryBasePlayer = slot.Inventory as InventoryBasePlayer;
			HarmonyPatches.InvalidateWearableLightHsvCache((inventoryBasePlayer != null) ? inventoryBasePlayer.Owner : null);
			ItemSlot itemSlot = __instance.BagSlots[slot.BagIndex];
			ItemStack backPackStack = (itemSlot != null) ? itemSlot.Itemstack : null;
			try
			{
				if (backPackStack != null)
				{
					IHeldBag collectibleInterface = backPackStack.Collectible.GetCollectibleInterface<IHeldBag>();
					if (collectibleInterface != null)
					{
						collectibleInterface.Store(backPackStack, slot);
					}
				}
			}
			catch (Exception exception)
			{
				ICoreAPI api = HarmonyPatches._api;
				Type typeFromHandle = typeof(HarmonyPatches);
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
				defaultInterpolatedStringHandler..ctor(39, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Error in BagInventory_SaveSlotIntoBag:\n");
				defaultInterpolatedStringHandler.AppendFormatted<Exception>(exception);
				LoggerUtil.Error(api, typeFromHandle, defaultInterpolatedStringHandler.ToStringAndClear());
				return true;
			}
			return false;
		}

		// Token: 0x06000461 RID: 1121 RVA: 0x0001950C File Offset: 0x0001770C
		private static void TryPatchStopRaiseShieldAnim(Harmony harmony, ICoreAPI api)
		{
			Type type = AccessTools.TypeByName("Vintagestory.GameContent.ModSystemStopRaiseShieldAnim");
			MethodInfo method = (type != null) ? type.GetMethod("OnGameTick", AccessTools.all, null, new Type[]
			{
				typeof(float)
			}, null) : null;
			if (method == null)
			{
				return;
			}
			harmony.Patch(method, new HarmonyMethod(AccessTools.Method(typeof(HarmonyPatches), "ModSystemStopRaiseShieldAnim_OnGameTick_Prefix", null, null)), null, null, null);
		}

		// Token: 0x06000462 RID: 1122 RVA: 0x00019580 File Offset: 0x00017780
		private static void TryUnpatchStopRaiseShieldAnim(Harmony harmony, string harmonyId)
		{
			Type type = AccessTools.TypeByName("Vintagestory.GameContent.ModSystemStopRaiseShieldAnim");
			MethodInfo method = (type != null) ? type.GetMethod("OnGameTick", AccessTools.all, null, new Type[]
			{
				typeof(float)
			}, null) : null;
			if (method == null)
			{
				return;
			}
			harmony.Unpatch(method, 1, harmonyId);
		}

		// Token: 0x06000463 RID: 1123 RVA: 0x000195D8 File Offset: 0x000177D8
		private static bool ModSystemStopRaiseShieldAnim_OnGameTick_Prefix()
		{
			if (!ShieldAutoPatcher.IsCombatOverhaulEnabled(HarmonyPatches._api))
			{
				return true;
			}
			ICoreClientAPI capi = HarmonyPatches._api as ICoreClientAPI;
			if (capi == null)
			{
				return true;
			}
			IClientWorldAccessor world = capi.World;
			EntityPlayer entityPlayer;
			if (world == null)
			{
				entityPlayer = null;
			}
			else
			{
				IClientPlayer player2 = world.Player;
				entityPlayer = ((player2 != null) ? player2.Entity : null);
			}
			EntityPlayer player = entityPlayer;
			if (player == null)
			{
				return true;
			}
			ItemSlot leftHandItemSlot = player.LeftHandItemSlot;
			Item item;
			if (leftHandItemSlot == null)
			{
				item = null;
			}
			else
			{
				ItemStack itemstack = leftHandItemSlot.Itemstack;
				item = ((itemstack != null) ? itemstack.Item : null);
			}
			if (!CollectibleClassifier.IsVanillaItemShield(item))
			{
				return true;
			}
			CombatOverhaulSystem coSystem = capi.ModLoader.GetModSystem<CombatOverhaulSystem>(true);
			if (((coSystem != null) ? coSystem.ActionListener : null) == null)
			{
				return true;
			}
			if (!coSystem.ActionListener.IsActive(10) && !coSystem.ActionListener.IsActive(16))
			{
				return true;
			}
			player.Controls.RightMouseDown = true;
			player.ServerControls.RightMouseDown = true;
			player.Controls.CtrlKey = true;
			player.ServerControls.CtrlKey = true;
			player.StartAnimation("raiseshield-left");
			player.StartAnimation("raiseshield-left-fp");
			return false;
		}

		// Token: 0x06000464 RID: 1124 RVA: 0x000196D8 File Offset: 0x000178D8
		private static void TryPatchVanillaShieldIdle(Harmony harmony, ICoreAPI api)
		{
			MethodInfo method = AccessTools.Method(typeof(ItemShield), "OnHeldIdle", new Type[]
			{
				typeof(ItemSlot),
				typeof(EntityAgent)
			}, null);
			if (method == null)
			{
				api.Logger.Warning("[OverhaullibLegacyCompat] Could not find vanilla ItemShield.OnHeldIdle. CO-patched shields may keep vanilla idle shield behavior.");
				return;
			}
			harmony.Patch(method, new HarmonyMethod(AccessTools.Method(typeof(HarmonyPatches), "VanillaShield_OnHeldIdle_Prefix", null, null)), null, null, null);
		}

		// Token: 0x06000465 RID: 1125 RVA: 0x0001975C File Offset: 0x0001795C
		private static void TryUnpatchVanillaShieldIdle(Harmony harmony, string harmonyId)
		{
			MethodInfo method = AccessTools.Method(typeof(ItemShield), "OnHeldIdle", new Type[]
			{
				typeof(ItemSlot),
				typeof(EntityAgent)
			}, null);
			if (method != null)
			{
				harmony.Unpatch(method, 1, harmonyId);
			}
		}

		// Token: 0x06000466 RID: 1126 RVA: 0x000197B1 File Offset: 0x000179B1
		private static bool VanillaShield_OnHeldIdle_Prefix(ItemSlot slot)
		{
			if (ShieldAutoPatcher.IsCombatOverhaulEnabled(HarmonyPatches._api))
			{
				ItemStack itemstack = slot.Itemstack;
				return !CollectibleClassifier.HasMeleeWeaponActions((itemstack != null) ? itemstack.Collectible : null);
			}
			return true;
		}

		// Token: 0x06000467 RID: 1127 RVA: 0x000197DC File Offset: 0x000179DC
		private static void TryPatchVanillaShieldTooltip(Harmony harmony, ICoreAPI api)
		{
			MethodInfo method = AccessTools.Method(typeof(ItemShield), "GetHeldItemInfo", new Type[]
			{
				typeof(ItemSlot),
				typeof(StringBuilder),
				typeof(IWorldAccessor),
				typeof(bool)
			}, null);
			if (method == null)
			{
				api.Logger.Warning("[OverhaullibLegacyCompat] Could not find vanilla ItemShield.GetHeldItemInfo. CO-patched shields may show vanilla shield tooltip stats.");
				return;
			}
			harmony.Patch(method, null, null, new HarmonyMethod(AccessTools.Method(typeof(HarmonyPatches), "VanillaShield_GetHeldItemInfo_Transpiler", null, null)), null);
		}

		// Token: 0x06000468 RID: 1128 RVA: 0x0001987C File Offset: 0x00017A7C
		private static void TryUnpatchVanillaShieldTooltip(Harmony harmony, string harmonyId)
		{
			MethodInfo method = AccessTools.Method(typeof(ItemShield), "GetHeldItemInfo", new Type[]
			{
				typeof(ItemSlot),
				typeof(StringBuilder),
				typeof(IWorldAccessor),
				typeof(bool)
			}, null);
			if (method != null)
			{
				harmony.Unpatch(method, 3, harmonyId);
			}
		}

		// Token: 0x06000469 RID: 1129 RVA: 0x000198EB File Offset: 0x00017AEB
		private static IEnumerable<CodeInstruction> VanillaShield_GetHeldItemInfo_Transpiler(IEnumerable<CodeInstruction> instructions, ILGenerator generator)
		{
			HarmonyPatches.<VanillaShield_GetHeldItemInfo_Transpiler>d__58 <VanillaShield_GetHeldItemInfo_Transpiler>d__ = new HarmonyPatches.<VanillaShield_GetHeldItemInfo_Transpiler>d__58(-2);
			<VanillaShield_GetHeldItemInfo_Transpiler>d__.<>3__instructions = instructions;
			<VanillaShield_GetHeldItemInfo_Transpiler>d__.<>3__generator = generator;
			return <VanillaShield_GetHeldItemInfo_Transpiler>d__;
		}

		// Token: 0x0600046A RID: 1130 RVA: 0x00019902 File Offset: 0x00017B02
		private static bool ShouldSkipVanillaShieldTooltip(ItemShield shield, ItemSlot slot)
		{
			if (ShieldAutoPatcher.IsCombatOverhaulEnabled(HarmonyPatches._api))
			{
				ItemStack itemstack = slot.Itemstack;
				return CollectibleClassifier.HasMeleeWeaponActions((itemstack != null) ? itemstack.Collectible : null);
			}
			return false;
		}

		// Token: 0x0600046B RID: 1131 RVA: 0x0001992C File Offset: 0x00017B2C
		private static void LightHsv(EntityPlayer __instance, ref byte[] __result)
		{
			if (((__instance != null) ? __instance.Player : null) == null)
			{
				return;
			}
			if (!__instance.Alive)
			{
				return;
			}
			if (__instance.Player.WorldData.CurrentGameMode == 3)
			{
				return;
			}
			try
			{
				__result = HarmonyPatches.CopyLightHsvToOutputBuffer(__result);
				if (HarmonyPatches.DebugWearableLights)
				{
					ICoreAPI api = HarmonyPatches._api;
					if (api != null)
					{
						api.Logger.Notification("[OverhaullibLegacyCompat] LightHsv postfix called for " + __instance.Player.PlayerName);
					}
				}
				byte[] wearableLightHsv = HarmonyPatches.GetCachedWearableLightHsv(__instance);
				HarmonyPatches.AddLight(ref __result, wearableLightHsv);
			}
			catch (Exception exception)
			{
				HarmonyPatches.ReportWearableLightHsvException(exception);
			}
		}

		// Token: 0x0600046C RID: 1132 RVA: 0x000199C8 File Offset: 0x00017BC8
		private static byte[] GetCachedWearableLightHsv(EntityPlayer player)
		{
			ICoreAPI api = HarmonyPatches._api;
			Nullable<long> num;
			if (api == null)
			{
				num = default(long?);
			}
			else
			{
				IWorldAccessor world = api.World;
				num = ((world != null) ? new long?(world.ElapsedMilliseconds) : default(long?));
			}
			long nowMs = num ?? Environment.TickCount64;
			long entityId = player.EntityId;
			HarmonyPatches.WearableLightHsvCacheEntry entry;
			if (HarmonyPatches._wearableLightHsvCache.TryGetValue(entityId, ref entry))
			{
				if (entry != null && nowMs < entry.ExpiresAtMs)
				{
					byte[] hsv2 = entry.Hsv;
					if (hsv2 != null && hsv2.Length >= 3)
					{
						return entry.Hsv;
					}
				}
				HarmonyPatches._wearableLightHsvCache.Remove(entityId);
			}
			byte[] hsv = new byte[3];
			HarmonyPatches.AddInventoryLights(player, HarmonyPatches.GetGearInventory(player), ref hsv, "gear");
			HarmonyPatches.AddBackpackLights(player, ref hsv);
			HarmonyPatches._wearableLightHsvCache[entityId] = new HarmonyPatches.WearableLightHsvCacheEntry(hsv, nowMs + 250L);
			HarmonyPatches.PruneExpiredWearableLightHsvCache(nowMs);
			return hsv;
		}

		// Token: 0x0600046D RID: 1133 RVA: 0x00019AAD File Offset: 0x00017CAD
		[NullableContext(2)]
		private static void InvalidateWearableLightHsvCache(Entity owner)
		{
			if (owner == null)
			{
				HarmonyPatches._wearableLightHsvCache.Clear();
				return;
			}
			HarmonyPatches._wearableLightHsvCache.Remove(owner.EntityId);
		}

		// Token: 0x0600046E RID: 1134 RVA: 0x00019AD0 File Offset: 0x00017CD0
		private static void PruneExpiredWearableLightHsvCache(long nowMs)
		{
			if (HarmonyPatches._wearableLightHsvCache.Count < 256)
			{
				return;
			}
			long staleBeforeMs = nowMs - 250L;
			HarmonyPatches._wearableLightHsvPruneScratch.Clear();
			foreach (KeyValuePair<long, HarmonyPatches.WearableLightHsvCacheEntry> keyValuePair in HarmonyPatches._wearableLightHsvCache)
			{
				long num;
				HarmonyPatches.WearableLightHsvCacheEntry wearableLightHsvCacheEntry;
				keyValuePair.Deconstruct(ref num, ref wearableLightHsvCacheEntry);
				long id = num;
				if (wearableLightHsvCacheEntry.ExpiresAtMs < staleBeforeMs)
				{
					HarmonyPatches._wearableLightHsvPruneScratch.Add(id);
				}
			}
			foreach (long id2 in HarmonyPatches._wearableLightHsvPruneScratch)
			{
				HarmonyPatches._wearableLightHsvCache.Remove(id2);
			}
		}

		// Token: 0x0600046F RID: 1135 RVA: 0x00019BAC File Offset: 0x00017DAC
		private static void AddBackpackLights(EntityPlayer player, ref byte[] result)
		{
			IInventory backpackInventory = HarmonyPatches.GetBackpackInventory(player);
			if (backpackInventory == null)
			{
				return;
			}
			int count = Math.Min(4, backpackInventory.Count);
			for (int index = 0; index < count; index++)
			{
				HarmonyPatches.AddSlotLight(player, backpackInventory[index], ref result, "backpack");
			}
			HarmonyPatches.AddBackpackStoredLightSlots(player, backpackInventory, ref result);
		}

		// Token: 0x06000470 RID: 1136 RVA: 0x00019BF8 File Offset: 0x00017DF8
		private static void AddBackpackStoredLightSlots(EntityPlayer player, IInventory backpackInventory, ref byte[] result)
		{
			foreach (ItemSlot itemSlot in backpackInventory)
			{
				ItemSlotBagContentWithWildcardMatch bagSlot = itemSlot as ItemSlotBagContentWithWildcardMatch;
				if (bagSlot != null && bagSlot.Config.StoredItemLightLevelOffset != 0)
				{
					HarmonyPatches.AddSlotLight(player, bagSlot, ref result, "backpack-content");
				}
			}
		}

		// Token: 0x06000471 RID: 1137 RVA: 0x00019C5C File Offset: 0x00017E5C
		private static void AddInventoryLights(EntityPlayer player, [Nullable(2)] IInventory inventory, ref byte[] result, string source)
		{
			if (inventory == null)
			{
				return;
			}
			foreach (ItemSlot slot in inventory)
			{
				HarmonyPatches.AddSlotLight(player, slot, ref result, source);
			}
		}

		// Token: 0x06000472 RID: 1138 RVA: 0x00019CAC File Offset: 0x00017EAC
		private static void AddSlotLight(EntityPlayer player, [Nullable(2)] ItemSlot slot, ref byte[] result, string source)
		{
			if (slot == null || slot.Empty)
			{
				return;
			}
			ItemStack itemstack = slot.Itemstack;
			if (((itemstack != null) ? itemstack.Collectible : null) == null)
			{
				return;
			}
			CollectibleObject collectible = slot.Itemstack.Collectible;
			IWearableLightSource wearableLightSource = collectible.GetCollectibleInterface<IWearableLightSource>();
			if (wearableLightSource != null)
			{
				byte[] hsv = null;
				try
				{
					hsv = wearableLightSource.GetLightHsv(player, slot);
				}
				catch (Exception exception)
				{
					HarmonyPatches.ReportWearableLightHsvException(exception);
				}
				if (HarmonyPatches.DebugWearableLights)
				{
					ICoreAPI api = HarmonyPatches._api;
					if (api != null)
					{
						api.Logger.Notification(string.Concat(new string[]
						{
							"[OverhaullibLegacyCompat] ",
							source,
							" wearable light ",
							collectible.Code,
							" -> ",
							HarmonyPatches.FormatHsv(HarmonyPatches.ApplySlotLightLevelOffset(slot, hsv))
						}));
					}
				}
				HarmonyPatches.AddLight(ref result, HarmonyPatches.ApplySlotLightLevelOffset(slot, hsv));
			}
			byte[] normalLightHsv = collectible.LightHsv;
			if (normalLightHsv != null && normalLightHsv.Length >= 3 && normalLightHsv[2] > 0)
			{
				byte[] adjustedLightHsv = HarmonyPatches.ApplySlotLightLevelOffset(slot, normalLightHsv);
				if (HarmonyPatches.DebugWearableLights)
				{
					ICoreAPI api2 = HarmonyPatches._api;
					if (api2 != null)
					{
						api2.Logger.Notification(string.Concat(new string[]
						{
							"[OverhaullibLegacyCompat] ",
							source,
							" normal light ",
							collectible.Code,
							" -> ",
							HarmonyPatches.FormatHsv(adjustedLightHsv)
						}));
					}
				}
				HarmonyPatches.AddLight(ref result, adjustedLightHsv);
			}
		}

		// Token: 0x06000473 RID: 1139 RVA: 0x00019E10 File Offset: 0x00018010
		private static void AddLight(ref byte[] result, [Nullable(2)] byte[] hsv)
		{
			if (hsv == null || hsv.Length < 3 || hsv[2] <= 0)
			{
				return;
			}
			if (result == null || result.Length < 3)
			{
				result = new byte[3];
			}
			float totalBrightness = (float)(result[2] + hsv[2]);
			if (totalBrightness <= 0f)
			{
				return;
			}
			float brightnessFraction = (float)hsv[2] / totalBrightness;
			byte oldHue = result[0];
			byte oldSat = result[1];
			byte oldVal = result[2];
			result[0] = (byte)((float)hsv[0] * brightnessFraction + (float)oldHue * (1f - brightnessFraction));
			result[1] = (byte)((float)hsv[1] * brightnessFraction + (float)oldSat * (1f - brightnessFraction));
			result[2] = Math.Max(hsv[2], oldVal);
		}

		// Token: 0x06000474 RID: 1140 RVA: 0x00019EA4 File Offset: 0x000180A4
		private static byte[] CopyLightHsvToOutputBuffer([Nullable(2)] byte[] hsv)
		{
			byte[] array;
			if ((array = HarmonyPatches._lightHsvOutputBuffer) == null)
			{
				array = (HarmonyPatches._lightHsvOutputBuffer = new byte[3]);
			}
			byte[] buffer = array;
			if (hsv == null || hsv.Length < 3)
			{
				buffer[0] = 0;
				buffer[1] = 0;
				buffer[2] = 0;
				return buffer;
			}
			buffer[0] = hsv[0];
			buffer[1] = hsv[1];
			buffer[2] = hsv[2];
			return buffer;
		}

		// Token: 0x06000475 RID: 1141 RVA: 0x00019EF4 File Offset: 0x000180F4
		private static byte[] ApplySlotLightLevelOffset(ItemSlot slot, [Nullable(2)] byte[] hsv)
		{
			if (hsv == null || hsv.Length < 3)
			{
				return HarmonyPatches.CopyLightHsvToOutputBuffer(null);
			}
			int offset = HarmonyPatches.GetStoredItemLightLevelOffset(slot);
			if (offset == 0)
			{
				return hsv;
			}
			byte[] adjusted = HarmonyPatches.CopyLightHsvToOutputBuffer(hsv);
			adjusted[2] = (byte)Math.Clamp((int)adjusted[2] + offset, 0, 255);
			return adjusted;
		}

		// Token: 0x06000476 RID: 1142 RVA: 0x00019F3C File Offset: 0x0001813C
		private static int GetStoredItemLightLevelOffset(ItemSlot slot)
		{
			ItemSlotBagContentWithWildcardMatch bagSlot = slot as ItemSlotBagContentWithWildcardMatch;
			if (bagSlot == null)
			{
				return 0;
			}
			return bagSlot.Config.StoredItemLightLevelOffset;
		}

		// Token: 0x06000477 RID: 1143 RVA: 0x00019F60 File Offset: 0x00018160
		private static string FormatHsv([Nullable(2)] byte[] hsv)
		{
			if (hsv == null)
			{
				return "null";
			}
			if (hsv.Length < 3)
			{
				return "invalid";
			}
			return string.Concat(new string[]
			{
				hsv[0].ToString(),
				",",
				hsv[1].ToString(),
				",",
				hsv[2].ToString()
			});
		}

		// Token: 0x06000478 RID: 1144 RVA: 0x00019FCC File Offset: 0x000181CC
		private static void ReportWearableLightHsvException(Exception exception)
		{
			if (HarmonyPatches._reportedWearableLightHsvException)
			{
				return;
			}
			HarmonyPatches._reportedWearableLightHsvException = true;
			ICoreAPI api = HarmonyPatches._api;
			Type typeFromHandle = typeof(HarmonyPatches);
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
			defaultInterpolatedStringHandler..ctor(107, 1);
			defaultInterpolatedStringHandler.AppendLiteral("Error while applying wearable light HSV. Wearable light contribution will be skipped for this render call:\n");
			defaultInterpolatedStringHandler.AppendFormatted<Exception>(exception);
			LoggerUtil.Error(api, typeFromHandle, defaultInterpolatedStringHandler.ToStringAndClear());
		}

		// Token: 0x06000479 RID: 1145 RVA: 0x0001A020 File Offset: 0x00018220
		private static bool BehaviorHealingItem_OnHeldInteractStart(EntityAgent byEntity)
		{
			EntityPlayer player = byEntity as EntityPlayer;
			if (player == null)
			{
				return true;
			}
			ItemStack itemstack = player.LeftHandItemSlot.Itemstack;
			object obj;
			if (itemstack == null)
			{
				obj = null;
			}
			else
			{
				CollectibleObject collectible = itemstack.Collectible;
				obj = ((collectible != null) ? collectible.GetCollectibleInterface<IHasMeleeWeaponActions>() : null);
			}
			object obj2 = obj;
			return obj2 == null || !obj2.CanBlock(player, false);
		}

		// Token: 0x04000197 RID: 407
		private static readonly bool DebugWearableLights = false;

		// Token: 0x04000198 RID: 408
		[Nullable(2)]
		private static ICoreAPI _api;

		// Token: 0x04000199 RID: 409
		internal static readonly HashSet<long> _reportedEntities = new HashSet<long>();

		// Token: 0x0400019A RID: 410
		[Nullable(2)]
		private static Type _offhandSlotType;

		// Token: 0x0400019B RID: 411
		[Nullable(new byte[]
		{
			1,
			1,
			2
		})]
		private static readonly Func<AnimationManager, Entity> _animationManagerEntityGetter = HarmonyPatches.CreateAnimationManagerEntityGetter();

		// Token: 0x0400019C RID: 412
		[Nullable(2)]
		private static readonly FieldInfo _smoothedBodyYaw = typeof(EntityPlayerShapeRenderer).GetField("smoothedBodyYaw", 36);

		// Token: 0x0400019D RID: 413
		private static readonly Dictionary<long, HarmonyPatches.WearableLightHsvCacheEntry> _wearableLightHsvCache = new Dictionary<long, HarmonyPatches.WearableLightHsvCacheEntry>();

		// Token: 0x0400019E RID: 414
		private const long _wearableLightHsvCacheDurationMs = 250L;

		// Token: 0x0400019F RID: 415
		private const int _wearableLightHsvCachePruneThreshold = 256;

		// Token: 0x040001A0 RID: 416
		private static readonly List<long> _wearableLightHsvPruneScratch = new List<long>();

		// Token: 0x040001A1 RID: 417
		private static bool _reportedWearableLightHsvException;

		// Token: 0x040001A2 RID: 418
		private const string _fallDamageThresholdMultiplierStat = "fallDamageThreshold";

		// Token: 0x040001A3 RID: 419
		private const float _fallDamageMultiplier = 0.2f;

		// Token: 0x040001A4 RID: 420
		private const float _fallDamageSpeedThreshold = 0.1f;

		// Token: 0x040001A5 RID: 421
		private const double _newFallDistance = 4.5;

		// Token: 0x040001A6 RID: 422
		[Nullable(2)]
		[ThreadStatic]
		private static byte[] _lightHsvOutputBuffer;

		// Token: 0x020001EE RID: 494
		[Nullable(0)]
		private sealed class WearableLightHsvCacheEntry
		{
			// Token: 0x060015AF RID: 5551 RVA: 0x0006982F File Offset: 0x00067A2F
			public WearableLightHsvCacheEntry(byte[] hsv, long expiresAtMs)
			{
			}

			// Token: 0x1700053E RID: 1342
			// (get) Token: 0x060015B0 RID: 5552 RVA: 0x00069845 File Offset: 0x00067A45
			public byte[] Hsv { get; } = hsv;

			// Token: 0x1700053F RID: 1343
			// (get) Token: 0x060015B1 RID: 5553 RVA: 0x0006984D File Offset: 0x00067A4D
			public long ExpiresAtMs { get; } = expiresAtMs;
		}
	}
}
