using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.Common.Entities;
using Vintagestory.API.Datastructures;
using Vintagestory.API.MathTools;
using Vintagestory.API.Util;
using Vintagestory.GameContent;

namespace CombatOverhaul.Armor
{
	// Token: 0x0200013E RID: 318
	[NullableContext(1)]
	[Nullable(0)]
	public class ItemHelmetWithVisor : ItemWearableArmor
	{
		// Token: 0x06000F4D RID: 3917 RVA: 0x0004AC0B File Offset: 0x00048E0B
		public override void OnLoaded(ICoreAPI api)
		{
			this.AttachableToEntity = (IAttachableToEntity.FromCollectible(this) != null);
			base.OnLoaded(api);
		}

		// Token: 0x06000F4E RID: 3918 RVA: 0x0004AC24 File Offset: 0x00048E24
		public override void OnBeforeRender(ICoreClientAPI capi, ItemStack itemstack, EnumItemRenderTarget target, ref ItemRenderInfo renderinfo)
		{
			if (!this.AttachableToEntity)
			{
				return;
			}
			Dictionary<string, MultiTextureMeshRef> meshrefs = ObjectCacheUtil.GetOrCreate<Dictionary<string, MultiTextureMeshRef>>(capi, "wearableAttachmentMeshRefs", () => new Dictionary<string, MultiTextureMeshRef>());
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

		// Token: 0x06000F4F RID: 3919 RVA: 0x0004AD10 File Offset: 0x00048F10
		public new string GetMeshCacheKey(ItemStack itemstack)
		{
			if (this.Opened(itemstack))
			{
				return "wearableModelRef-" + itemstack.Collectible.Code.ToString() + "-opened";
			}
			return "wearableModelRef-" + itemstack.Collectible.Code.ToString() + "-closed";
		}

		// Token: 0x06000F50 RID: 3920 RVA: 0x0004AD65 File Offset: 0x00048F65
		public void Open(ItemSlot slot)
		{
			if (slot != null)
			{
				ItemStack itemstack = slot.Itemstack;
				if (itemstack == null)
				{
					return;
				}
				itemstack.Attributes.SetBool(this.VisorStateAttribute, true);
			}
		}

		// Token: 0x06000F51 RID: 3921 RVA: 0x0004AD86 File Offset: 0x00048F86
		public void Close(ItemSlot slot)
		{
			if (slot != null)
			{
				ItemStack itemstack = slot.Itemstack;
				if (itemstack == null)
				{
					return;
				}
				itemstack.Attributes.SetBool(this.VisorStateAttribute, false);
			}
		}

		// Token: 0x06000F52 RID: 3922 RVA: 0x0004ADA7 File Offset: 0x00048FA7
		public void Switch(ItemSlot slot)
		{
			if (slot != null)
			{
				ItemStack itemstack = slot.Itemstack;
				if (itemstack == null)
				{
					return;
				}
				itemstack.Attributes.SetBool(this.VisorStateAttribute, !this.Opened(slot.Itemstack));
			}
		}

		// Token: 0x06000F53 RID: 3923 RVA: 0x0004ADD8 File Offset: 0x00048FD8
		[return: Nullable(2)]
		protected new virtual MeshData GenMesh(ICoreClientAPI capi, ItemStack itemstack, ITexPositionSource texSource)
		{
			JsonObject attrObj = itemstack.Collectible.Attributes;
			EntityProperties entityType = capi.World.GetEntityType(new AssetLocation(((attrObj != null) ? attrObj["wearerEntityCode"].ToString() : null) ?? "player"));
			Shape entityShape = entityType.Client.LoadedShape;
			AssetLocation shapePathForLogging = entityType.Client.Shape.Base;
			Shape newShape;
			if (!this.AttachableToEntity)
			{
				newShape = entityShape;
			}
			else
			{
				newShape = new Shape
				{
					Elements = entityShape.CloneElements(),
					Animations = entityShape.CloneAnimations(),
					AnimationsByCrc32 = entityShape.AnimationsByCrc32,
					JointsById = entityShape.JointsById,
					TextureWidth = entityShape.TextureWidth,
					TextureHeight = entityShape.TextureHeight,
					Textures = null
				};
			}
			MeshData meshdata;
			if (attrObj["wearableInvShape"].Exists)
			{
				string text = "shapes/";
				JsonObject jsonObject = attrObj["wearableInvShape"];
				AssetLocation shapePath = new AssetLocation(text + ((jsonObject != null) ? jsonObject.ToString() : null) + ".json");
				Shape shape = Shape.TryGet(capi, shapePath);
				capi.Tesselator.TesselateShape(itemstack.Collectible, shape, ref meshdata, null, default(int?), null);
			}
			else
			{
				CompositeShape compArmorShape = (!attrObj[this.ShapeAttribute(itemstack)].Exists) ? ((itemstack.Class == 1) ? itemstack.Item.Shape : itemstack.Block.Shape) : attrObj[this.ShapeAttribute(itemstack)].AsObject<CompositeShape>(null, itemstack.Collectible.Code.Domain);
				if (compArmorShape == null)
				{
					capi.World.Logger.Warning("Wearable shape {0} {1} does not define a shape through either the shape property or the attachShape Attribute. Item will be invisible.", new object[]
					{
						itemstack.Class,
						itemstack.Collectible.Code
					});
					return null;
				}
				AssetLocation shapePath2 = compArmorShape.Base.CopyWithPath("shapes/" + compArmorShape.Base.Path + ".json");
				Shape armorShape = Shape.TryGet(capi, shapePath2);
				if (armorShape == null)
				{
					capi.World.Logger.Warning("Wearable shape {0} defined in {1} {2} not found or errored, was supposed to be at {3}. Item will be invisible.", new object[]
					{
						compArmorShape.Base,
						itemstack.Class,
						itemstack.Collectible.Code,
						shapePath2
					});
					return null;
				}
				newShape.StepParentShape(armorShape, shapePath2.ToShortString(), shapePathForLogging.ToShortString(), capi.Logger, delegate(string key, AssetLocation code)
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
								shapePath2,
								overlay.Base
							});
						}
						else
						{
							newShape.StepParentShape(oshape, overlay.Base.ToShortString(), shapePathForLogging.ToShortString(), capi.Logger, delegate(string key, AssetLocation Code)
							{
							});
						}
					}
				}
				this.NowTesselatingShape = newShape;
				capi.Tesselator.TesselateShapeWithJointIds("entity", newShape, ref meshdata, texSource, new Vec3f(), default(int?), null);
				this.NowTesselatingShape = null;
			}
			return meshdata;
		}

		// Token: 0x06000F54 RID: 3924 RVA: 0x0004B18D File Offset: 0x0004938D
		[NullableContext(2)]
		protected virtual bool Opened(ItemStack stack)
		{
			return stack != null && stack.Attributes.GetBool(this.VisorStateAttribute, false);
		}

		// Token: 0x06000F55 RID: 3925 RVA: 0x0004B1A6 File Offset: 0x000493A6
		protected virtual string ShapeAttribute(ItemStack stack)
		{
			if (!this.Opened(stack))
			{
				return "attachShapeClosed";
			}
			return "attachShapeOpened";
		}

		// Token: 0x0400074D RID: 1869
		protected new bool AttachableToEntity;

		// Token: 0x0400074E RID: 1870
		[Nullable(2)]
		protected new Shape NowTesselatingShape;

		// Token: 0x0400074F RID: 1871
		protected string VisorStateAttribute = "visorOpened";
	}
}
