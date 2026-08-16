using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using CombatOverhaul.Utils;
using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.Common.Entities;
using Vintagestory.API.Datastructures;
using Vintagestory.API.MathTools;
using Vintagestory.API.Util;
using Vintagestory.GameContent;

namespace CombatOverhaul.Armor
{
	// Token: 0x02000131 RID: 305
	[NullableContext(1)]
	[Nullable(0)]
	public class ItemWearableArmor : Item, IWearableStatsSupplier
	{
		// Token: 0x06000EC2 RID: 3778 RVA: 0x00048670 File Offset: 0x00046870
		public override void OnLoaded(ICoreAPI api)
		{
			this.AttachableToEntity = (IAttachableToEntity.FromCollectible(this) != null);
			base.OnLoaded(api);
		}

		// Token: 0x06000EC3 RID: 3779 RVA: 0x00048688 File Offset: 0x00046888
		public override void OnUnloaded(ICoreAPI api)
		{
			Dictionary<string, MultiTextureMeshRef> meshrefs = ObjectCacheUtil.TryGet<Dictionary<string, MultiTextureMeshRef>>(api, "wearableArmorMeshRefs");
			if (meshrefs != null && meshrefs.Count > 0)
			{
				foreach (KeyValuePair<string, MultiTextureMeshRef> keyValuePair in meshrefs)
				{
					string text;
					MultiTextureMeshRef multiTextureMeshRef;
					keyValuePair.Deconstruct(ref text, ref multiTextureMeshRef);
					multiTextureMeshRef.Dispose();
				}
				ObjectCacheUtil.Delete(api, "wearableArmorMeshRefs");
			}
			base.OnUnloaded(api);
		}

		// Token: 0x06000EC4 RID: 3780 RVA: 0x0004870C File Offset: 0x0004690C
		public override void OnBeforeRender(ICoreClientAPI capi, ItemStack itemstack, EnumItemRenderTarget target, ref ItemRenderInfo renderinfo)
		{
			if (!this.AttachableToEntity)
			{
				return;
			}
			Dictionary<string, MultiTextureMeshRef> meshrefs = ObjectCacheUtil.GetOrCreate<Dictionary<string, MultiTextureMeshRef>>(capi, "wearableArmorMeshRefs", () => new Dictionary<string, MultiTextureMeshRef>());
			string key = this.GetMeshCacheKey(itemstack);
			if (!meshrefs.TryGetValue(key, ref renderinfo.ModelRef))
			{
				ITexPositionSource texSource = capi.Tesselator.GetTextureSource(itemstack.Item, false);
				MeshData mesh = this.GenMesh(capi, itemstack, texSource);
				renderinfo.ModelRef = (meshrefs[key] = ((mesh == null) ? renderinfo.ModelRef : capi.Render.UploadMultiTextureMesh(mesh)));
			}
			if (this.Attributes["visibleDamageEffect"].AsBool(false))
			{
				renderinfo.DamageEffect = Math.Max(0f, 1f - (float)this.GetRemainingDurability(itemstack) / (float)this.GetMaxDurability(itemstack) * 1.1f);
			}
		}

		// Token: 0x06000EC5 RID: 3781 RVA: 0x000487F8 File Offset: 0x000469F8
		public override void OnHeldInteractStart(ItemSlot slot, EntityAgent byEntity, BlockSelection blockSel, EntitySelection entitySel, bool firstEvent, ref EnumHandHandling handHandling)
		{
			if (byEntity.Controls.ShiftKey)
			{
				base.OnHeldInteractStart(slot, byEntity, blockSel, entitySel, firstEvent, ref handHandling);
				return;
			}
			if (slot.Itemstack.Item == null)
			{
				return;
			}
			EntityPlayer entityPlayer = byEntity as EntityPlayer;
			IPlayer player = (entityPlayer != null) ? entityPlayer.Player : null;
			if (player == null)
			{
				return;
			}
			ArmorInventory inventory = ItemWearableArmor.GetGearInventory(byEntity) as ArmorInventory;
			if (inventory == null)
			{
				return;
			}
			ArmorBehavior behavior = base.GetCollectibleBehavior<ArmorBehavior>(true);
			if (behavior == null)
			{
				return;
			}
			string code = slot.Itemstack.Item.Code;
			ArmorType armorType = behavior.ArmorType;
			try
			{
				foreach (int index in inventory.GetSlotBlockingSlotsIndices(armorType))
				{
					ItemStack stack = inventory[index].TakeOutWhole();
					if (!player.InventoryManager.TryGiveItemstack(stack, false))
					{
						byEntity.Api.World.SpawnItemEntity(stack, byEntity.ServerPos.AsBlockPos, null);
					}
					inventory[index].MarkDirty();
				}
				int slotIndex = inventory.GetFittingSlotIndex(armorType);
				inventory[slotIndex].TryFlipWith(slot);
				inventory[slotIndex].MarkDirty();
				slot.MarkDirty();
				handHandling = 4;
			}
			catch (Exception exception)
			{
				ICoreAPI api = this.api;
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
				defaultInterpolatedStringHandler..ctor(38, 3);
				defaultInterpolatedStringHandler.AppendLiteral("Error on equipping '");
				defaultInterpolatedStringHandler.AppendFormatted(code);
				defaultInterpolatedStringHandler.AppendLiteral("' that occupies ");
				defaultInterpolatedStringHandler.AppendFormatted<ArmorType>(armorType);
				defaultInterpolatedStringHandler.AppendLiteral(":\n");
				defaultInterpolatedStringHandler.AppendFormatted<Exception>(exception);
				LoggerUtil.Error(api, this, defaultInterpolatedStringHandler.ToStringAndClear());
			}
		}

		// Token: 0x06000EC6 RID: 3782 RVA: 0x000489AC File Offset: 0x00046BAC
		public override void OnCreatedByCrafting(ItemSlot[] inSlots, ItemSlot outputSlot, IRecipeBase byRecipe)
		{
			int newDurability = 0;
			if (!(outputSlot is DummySlot))
			{
				this.EnsureConditionExists(outputSlot);
				outputSlot.Itemstack.Attributes.SetFloat("condition", 1f);
				if (byRecipe.Name.Path.Contains("repair"))
				{
					float repairValue;
					int matCostPerMatType;
					this.CalculateRepairValueProperly(inSlots, outputSlot, out repairValue, out matCostPerMatType);
					int curDur = outputSlot.Itemstack.Collectible.GetRemainingDurability(outputSlot.Itemstack);
					int maxDur = this.GetMaxDurability(outputSlot.Itemstack);
					newDurability = Math.Min(maxDur, (int)((float)curDur + (float)maxDur * repairValue));
				}
			}
			base.OnCreatedByCrafting(inSlots, outputSlot, byRecipe);
			if (outputSlot is DummySlot)
			{
				return;
			}
			if (byRecipe.Name.Path.Contains("repair"))
			{
				outputSlot.Itemstack.Attributes.SetInt("durability", newDurability);
			}
		}

		// Token: 0x06000EC7 RID: 3783 RVA: 0x00048A7C File Offset: 0x00046C7C
		public override bool ConsumeCraftingIngredients(ItemSlot[] inSlots, ItemSlot outputSlot, IRecipeBase recipe)
		{
			if (recipe.Name.Path.Contains("repair"))
			{
				float repairValue;
				int matCostPerMatType;
				this.CalculateRepairValueProperly(inSlots, outputSlot, out repairValue, out matCostPerMatType);
				foreach (ItemSlot islot in inSlots)
				{
					if (!islot.Empty)
					{
						if (islot.Itemstack.Collectible == this)
						{
							islot.Itemstack = null;
						}
						else
						{
							islot.TakeOut(matCostPerMatType);
						}
					}
				}
				return true;
			}
			return false;
		}

		// Token: 0x06000EC8 RID: 3784 RVA: 0x00048AEF File Offset: 0x00046CEF
		public bool IsArmorType(ItemSlot slot)
		{
			CollectibleBehaviorWearable vanillaWearableBehavior = this.GetVanillaWearableBehavior();
			if (vanillaWearableBehavior == null)
			{
				return base.GetCollectibleBehavior<ArmorBehavior>(true) != null;
			}
			return vanillaWearableBehavior.IsArmorType(slot);
		}

		// Token: 0x06000EC9 RID: 3785 RVA: 0x00048B0C File Offset: 0x00046D0C
		public EnumCharacterDressType GetDressType(ItemSlot slot)
		{
			CollectibleBehaviorWearable vanillaWearableBehavior = this.GetVanillaWearableBehavior();
			if (vanillaWearableBehavior == null)
			{
				return -1;
			}
			return vanillaWearableBehavior.GetDressType(slot);
		}

		// Token: 0x06000ECA RID: 3786 RVA: 0x00048B20 File Offset: 0x00046D20
		[return: Nullable(2)]
		public StatModifiers GetStatModifiers(ItemSlot slot)
		{
			CollectibleBehaviorWearable vanillaWearableBehavior = this.GetVanillaWearableBehavior();
			if (vanillaWearableBehavior == null)
			{
				return null;
			}
			return vanillaWearableBehavior.GetStatModifiers(slot);
		}

		// Token: 0x06000ECB RID: 3787 RVA: 0x00048B34 File Offset: 0x00046D34
		[return: Nullable(2)]
		public ProtectionModifiers GetProtectionModifiers(ItemSlot slot)
		{
			CollectibleBehaviorWearable vanillaWearableBehavior = this.GetVanillaWearableBehavior();
			if (vanillaWearableBehavior == null)
			{
				return null;
			}
			return vanillaWearableBehavior.GetProtectionModifiers(slot);
		}

		// Token: 0x06000ECC RID: 3788 RVA: 0x00048B48 File Offset: 0x00046D48
		public AssetLocation[] GetFootStepSounds(ItemSlot slot)
		{
			return ArmorFootStepSoundUtil.GetFootStepSounds(this.api, slot, this, this.GetVanillaWearableBehavior());
		}

		// Token: 0x06000ECD RID: 3789 RVA: 0x00048B5D File Offset: 0x00046D5D
		public float GetMaxWarmth(ItemSlot slot)
		{
			CollectibleBehaviorWearable vanillaWearableBehavior = this.GetVanillaWearableBehavior();
			if (vanillaWearableBehavior == null)
			{
				return 0f;
			}
			return vanillaWearableBehavior.GetMaxWarmth(slot);
		}

		// Token: 0x06000ECE RID: 3790 RVA: 0x00048B75 File Offset: 0x00046D75
		[NullableContext(2)]
		private CollectibleBehaviorWearable GetVanillaWearableBehavior()
		{
			return base.GetBehavior<CollectibleBehaviorWearable>();
		}

		// Token: 0x06000ECF RID: 3791 RVA: 0x00048B80 File Offset: 0x00046D80
		protected static bool IsWearableStack(ItemSlot slot)
		{
			ItemStack itemstack = slot.Itemstack;
			CollectibleObject collectible = (itemstack != null) ? itemstack.Collectible : null;
			if (collectible == null)
			{
				return false;
			}
			if (collectible.GetCollectibleBehavior<ArmorBehavior>(true) != null)
			{
				return true;
			}
			CollectibleBehavior[] collectibleBehaviors = collectible.CollectibleBehaviors;
			if (collectibleBehaviors == null)
			{
				return false;
			}
			return Enumerable.Any<CollectibleBehavior>(collectibleBehaviors, (CollectibleBehavior behavior) => behavior.GetType().Name == "CollectibleBehaviorWearable");
		}

		// Token: 0x06000ED0 RID: 3792 RVA: 0x00048BE0 File Offset: 0x00046DE0
		[return: Nullable(2)]
		protected static InventoryBase GetGearInventory(Entity entity)
		{
			return entity.GetBehavior<EntityBehaviorPlayerInventory>().Inventory;
		}

		// Token: 0x06000ED1 RID: 3793 RVA: 0x00048BED File Offset: 0x00046DED
		protected virtual string GetMeshCacheKey(ItemStack itemstack)
		{
			return "wearableArmorModelRef-" + itemstack.Collectible.Code;
		}

		// Token: 0x06000ED2 RID: 3794 RVA: 0x00048C0C File Offset: 0x00046E0C
		[return: Nullable(2)]
		protected virtual MeshData GenMesh(ICoreClientAPI capi, ItemStack itemstack, ITexPositionSource texSource)
		{
			JsonObject attrObj = itemstack.Collectible.Attributes;
			EntityProperties entityType = capi.World.GetEntityType(new AssetLocation(((attrObj != null) ? attrObj["wearerEntityCode"].ToString() : null) ?? "player"));
			Shape entityShape = entityType.Client.LoadedShape;
			AssetLocation shapePathForLogging = entityType.Client.Shape.Base;
			Shape newShape = new Shape
			{
				Elements = entityShape.CloneElements(),
				Animations = entityShape.CloneAnimations(),
				AnimationsByCrc32 = entityShape.AnimationsByCrc32,
				JointsById = entityShape.JointsById,
				TextureWidth = entityShape.TextureWidth,
				TextureHeight = entityShape.TextureHeight,
				Textures = null
			};
			if (attrObj["wearableInvShape"].Exists)
			{
				string text = "shapes/";
				JsonObject jsonObject = attrObj["wearableInvShape"];
				AssetLocation invShapePath = new AssetLocation(text + ((jsonObject != null) ? jsonObject.ToString() : null) + ".json");
				Shape invShape = Shape.TryGet(capi, invShapePath);
				if (invShape == null)
				{
					return null;
				}
				MeshData invMesh;
				capi.Tesselator.TesselateShape(itemstack.Collectible, invShape, ref invMesh, null, default(int?), null);
				return invMesh;
			}
			else
			{
				CompositeShape compArmorShape = attrObj["attachShape"].Exists ? attrObj["attachShape"].AsObject<CompositeShape>(null, itemstack.Collectible.Code.Domain) : ((itemstack.Class == 1) ? itemstack.Item.Shape : itemstack.Block.Shape);
				if (compArmorShape == null)
				{
					capi.World.Logger.Warning("Wearable shape {0} {1} does not define a shape through either the shape property or the attachShape Attribute. Item will be invisible.", new object[]
					{
						itemstack.Class,
						itemstack.Collectible.Code
					});
					return null;
				}
				AssetLocation shapePath = compArmorShape.Base.CopyWithPath("shapes/" + compArmorShape.Base.Path + ".json");
				Shape armorShape = Shape.TryGet(capi, shapePath);
				if (armorShape == null)
				{
					capi.World.Logger.Warning("Wearable shape {0} defined in {1} {2} not found or errored, was supposed to be at {3}. Item will be invisible.", new object[]
					{
						compArmorShape.Base,
						itemstack.Class,
						itemstack.Collectible.Code,
						shapePath
					});
					return null;
				}
				newShape.StepParentShape(armorShape, shapePath.ToShortString(), shapePathForLogging.ToShortString(), capi.Logger, delegate(string key, AssetLocation code)
				{
				});
				if (compArmorShape.Overlays != null)
				{
					foreach (CompositeShape overlay in compArmorShape.Overlays)
					{
						Shape oshape = Shape.TryGet(capi, overlay.Base.CopyWithPath("shapes/" + overlay.Base.Path + ".json"));
						if (oshape == null)
						{
							capi.World.Logger.Warning("Wearable shape {0} overlay {4} defined in {1} {2} not found or errored, was supposed to be at {3}. Item will be invisible.", new object[]
							{
								compArmorShape.Base,
								itemstack.Class,
								itemstack.Collectible.Code,
								shapePath,
								overlay.Base
							});
						}
						else
						{
							newShape.StepParentShape(oshape, overlay.Base.ToShortString(), shapePathForLogging.ToShortString(), capi.Logger, delegate(string key, AssetLocation code)
							{
							});
						}
					}
				}
				this.NowTesselatingShape = newShape;
				MeshData meshdata;
				capi.Tesselator.TesselateShapeWithJointIds("entity", newShape, ref meshdata, texSource, new Vec3f(), default(int?), null);
				this.NowTesselatingShape = null;
				return meshdata;
			}
		}

		// Token: 0x06000ED3 RID: 3795 RVA: 0x00048FB8 File Offset: 0x000471B8
		protected virtual void EnsureConditionExists(ItemSlot slot)
		{
			if (slot is DummySlot)
			{
				return;
			}
			if (!slot.Itemstack.Attributes.HasAttribute("condition") && this.api.Side == 1)
			{
				JsonObject itemAttributes = slot.Itemstack.ItemAttributes;
				if (itemAttributes != null && itemAttributes["warmth"].Exists)
				{
					JsonObject itemAttributes2 = slot.Itemstack.ItemAttributes;
					if (itemAttributes2 == null || itemAttributes2["warmth"].AsFloat(0f) != 0f)
					{
						if (slot is ItemSlotTrade)
						{
							slot.Itemstack.Attributes.SetFloat("condition", (float)this.api.World.Rand.NextDouble() * 0.25f + 0.75f);
						}
						else
						{
							slot.Itemstack.Attributes.SetFloat("condition", (float)this.api.World.Rand.NextDouble() * 0.4f);
						}
						slot.MarkDirty();
					}
				}
			}
		}

		// Token: 0x06000ED4 RID: 3796 RVA: 0x000490CC File Offset: 0x000472CC
		protected virtual void CalculateRepairValueProperly(ItemSlot[] inSlots, ItemSlot outputSlot, out float repairValue, out int matCostPerMatType)
		{
			repairValue = 0f;
			matCostPerMatType = 0;
			ItemSlot armorSlot = Enumerable.FirstOrDefault<ItemSlot>(inSlots, (ItemSlot slot) => ItemWearableArmor.IsWearableStack(slot));
			if (((armorSlot != null) ? armorSlot.Itemstack : null) == null || outputSlot.Itemstack == null)
			{
				return;
			}
			int origMatCount = this.GetOrigMatCount(inSlots, outputSlot);
			if (origMatCount == 0)
			{
				origMatCount = this.Attributes["materialCount"].AsInt(1);
			}
			int curDur = outputSlot.Itemstack.Collectible.GetRemainingDurability(armorSlot.Itemstack);
			int maxDur = this.GetMaxDurability(outputSlot.Itemstack);
			float repairDurabilityPerItem = 2f / (float)origMatCount * (float)maxDur;
			int fullRepairMatCount = (int)Math.Max(1.0, Math.Round((double)((float)(maxDur - curDur) / repairDurabilityPerItem)));
			int minMatStackSize = this.GetInputRepairCount(inSlots);
			int matTypeCount = this.GetRepairMatTypeCount(inSlots);
			int availableRepairMatCount = Math.Min(fullRepairMatCount, minMatStackSize * matTypeCount);
			matCostPerMatType = Math.Min(fullRepairMatCount, minMatStackSize);
			repairValue = (float)availableRepairMatCount / (float)origMatCount * 2f;
		}

		// Token: 0x06000ED5 RID: 3797 RVA: 0x000491D0 File Offset: 0x000473D0
		protected virtual int GetRepairMatTypeCount(ItemSlot[] slots)
		{
			List<ItemStack> stackTypes = new List<ItemStack>();
			foreach (ItemSlot slot in slots)
			{
				if (!slot.Empty)
				{
					bool found = false;
					if (!ItemWearableArmor.IsWearableStack(slot))
					{
						foreach (ItemStack stack in stackTypes)
						{
							if (slot.Itemstack.Satisfies(stack))
							{
								found = true;
								break;
							}
						}
						if (!found)
						{
							stackTypes.Add(slot.Itemstack);
						}
					}
				}
			}
			return stackTypes.Count;
		}

		// Token: 0x06000ED6 RID: 3798 RVA: 0x00049270 File Offset: 0x00047470
		protected virtual int GetOrigMatCount(ItemSlot[] inSlots, ItemSlot outputSlot)
		{
			int count = 0;
			foreach (ItemSlot slot in inSlots)
			{
				if (!slot.Empty && !ItemWearableArmor.IsWearableStack(slot))
				{
					count += Math.Max(1, slot.StackSize);
				}
			}
			return count;
		}

		// Token: 0x06000ED7 RID: 3799 RVA: 0x000492B4 File Offset: 0x000474B4
		protected virtual int GetInputRepairCount(ItemSlot[] inSlots)
		{
			int min = int.MaxValue;
			foreach (ItemSlot slot in inSlots)
			{
				if (!slot.Empty && !ItemWearableArmor.IsWearableStack(slot))
				{
					min = Math.Min(min, slot.StackSize);
				}
			}
			if (min != 2147483647)
			{
				return min;
			}
			return 0;
		}

		// Token: 0x04000725 RID: 1829
		protected bool AttachableToEntity;

		// Token: 0x04000726 RID: 1830
		[Nullable(2)]
		protected Shape NowTesselatingShape;
	}
}
