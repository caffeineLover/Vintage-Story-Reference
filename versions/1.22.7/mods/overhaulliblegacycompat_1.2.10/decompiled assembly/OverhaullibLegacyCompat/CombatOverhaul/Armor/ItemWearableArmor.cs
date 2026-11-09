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
	// Token: 0x0200013D RID: 317
	[NullableContext(1)]
	[Nullable(0)]
	public class ItemWearableArmor : Item, IWearableStatsSupplier
	{
		// Token: 0x06000F36 RID: 3894 RVA: 0x00049F70 File Offset: 0x00048170
		public override void OnLoaded(ICoreAPI api)
		{
			this.AttachableToEntity = (IAttachableToEntity.FromCollectible(this) != null);
			base.OnLoaded(api);
		}

		// Token: 0x06000F37 RID: 3895 RVA: 0x00049F88 File Offset: 0x00048188
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

		// Token: 0x06000F38 RID: 3896 RVA: 0x0004A00C File Offset: 0x0004820C
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

		// Token: 0x06000F39 RID: 3897 RVA: 0x0004A0F8 File Offset: 0x000482F8
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

		// Token: 0x06000F3A RID: 3898 RVA: 0x0004A2AC File Offset: 0x000484AC
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

		// Token: 0x06000F3B RID: 3899 RVA: 0x0004A37C File Offset: 0x0004857C
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

		// Token: 0x06000F3C RID: 3900 RVA: 0x0004A3EF File Offset: 0x000485EF
		public bool IsArmorType(ItemSlot slot)
		{
			CollectibleBehaviorWearable vanillaWearableBehavior = this.GetVanillaWearableBehavior();
			if (vanillaWearableBehavior == null)
			{
				return base.GetCollectibleBehavior<ArmorBehavior>(true) != null;
			}
			return vanillaWearableBehavior.IsArmorType(slot);
		}

		// Token: 0x06000F3D RID: 3901 RVA: 0x0004A40C File Offset: 0x0004860C
		public EnumCharacterDressType GetDressType(ItemSlot slot)
		{
			CollectibleBehaviorWearable vanillaWearableBehavior = this.GetVanillaWearableBehavior();
			if (vanillaWearableBehavior == null)
			{
				return -1;
			}
			return vanillaWearableBehavior.GetDressType(slot);
		}

		// Token: 0x06000F3E RID: 3902 RVA: 0x0004A420 File Offset: 0x00048620
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

		// Token: 0x06000F3F RID: 3903 RVA: 0x0004A434 File Offset: 0x00048634
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

		// Token: 0x06000F40 RID: 3904 RVA: 0x0004A448 File Offset: 0x00048648
		public AssetLocation[] GetFootStepSounds(ItemSlot slot)
		{
			return ArmorFootStepSoundUtil.GetFootStepSounds(this.api, slot, this, this.GetVanillaWearableBehavior());
		}

		// Token: 0x06000F41 RID: 3905 RVA: 0x0004A45D File Offset: 0x0004865D
		public float GetMaxWarmth(ItemSlot slot)
		{
			CollectibleBehaviorWearable vanillaWearableBehavior = this.GetVanillaWearableBehavior();
			if (vanillaWearableBehavior == null)
			{
				return 0f;
			}
			return vanillaWearableBehavior.GetMaxWarmth(slot);
		}

		// Token: 0x06000F42 RID: 3906 RVA: 0x0004A475 File Offset: 0x00048675
		[NullableContext(2)]
		private CollectibleBehaviorWearable GetVanillaWearableBehavior()
		{
			return base.GetBehavior<CollectibleBehaviorWearable>();
		}

		// Token: 0x06000F43 RID: 3907 RVA: 0x0004A480 File Offset: 0x00048680
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

		// Token: 0x06000F44 RID: 3908 RVA: 0x0004A4E0 File Offset: 0x000486E0
		[return: Nullable(2)]
		protected static InventoryBase GetGearInventory(Entity entity)
		{
			return entity.GetBehavior<EntityBehaviorPlayerInventory>().Inventory;
		}

		// Token: 0x06000F45 RID: 3909 RVA: 0x0004A4ED File Offset: 0x000486ED
		protected virtual string GetMeshCacheKey(ItemStack itemstack)
		{
			return "wearableArmorModelRef-" + itemstack.Collectible.Code;
		}

		// Token: 0x06000F46 RID: 3910 RVA: 0x0004A50C File Offset: 0x0004870C
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

		// Token: 0x06000F47 RID: 3911 RVA: 0x0004A8B8 File Offset: 0x00048AB8
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

		// Token: 0x06000F48 RID: 3912 RVA: 0x0004A9CC File Offset: 0x00048BCC
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

		// Token: 0x06000F49 RID: 3913 RVA: 0x0004AAD0 File Offset: 0x00048CD0
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

		// Token: 0x06000F4A RID: 3914 RVA: 0x0004AB70 File Offset: 0x00048D70
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

		// Token: 0x06000F4B RID: 3915 RVA: 0x0004ABB4 File Offset: 0x00048DB4
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

		// Token: 0x0400074B RID: 1867
		protected bool AttachableToEntity;

		// Token: 0x0400074C RID: 1868
		[Nullable(2)]
		protected Shape NowTesselatingShape;
	}
}
