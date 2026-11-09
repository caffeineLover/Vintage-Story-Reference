using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.Common.Entities;
using Vintagestory.API.Config;
using Vintagestory.API.MathTools;
using Vintagestory.API.Util;
using Vintagestory.GameContent;

namespace Firearms
{
	// Token: 0x0200000F RID: 15
	[NullableContext(1)]
	[Nullable(0)]
	public class BlockCrate : BlockContainer, ITexPositionSource, IAttachableToEntity, IWearableShapeSupplier
	{
		// Token: 0x17000022 RID: 34
		// (get) Token: 0x0600006C RID: 108 RVA: 0x0000350E File Offset: 0x0000170E
		public Size2i AtlasSize
		{
			get
			{
				return this.tmpTextureSource.AtlasSize;
			}
		}

		// Token: 0x17000023 RID: 35
		// (get) Token: 0x0600006D RID: 109 RVA: 0x0000351B File Offset: 0x0000171B
		public string Subtype
		{
			get
			{
				if (this.Props.VariantByGroup != null)
				{
					return this.Variant[this.Props.VariantByGroup];
				}
				return "";
			}
		}

		// Token: 0x17000024 RID: 36
		// (get) Token: 0x0600006E RID: 110 RVA: 0x00003546 File Offset: 0x00001746
		public string SubtypeInventory
		{
			get
			{
				CrateProperties props = this.Props;
				if (((props != null) ? props.VariantByGroupInventory : null) != null)
				{
					return this.Variant[this.Props.VariantByGroupInventory];
				}
				return "";
			}
		}

		// Token: 0x17000025 RID: 37
		// (get) Token: 0x0600006F RID: 111 RVA: 0x00003578 File Offset: 0x00001778
		// (set) Token: 0x06000070 RID: 112 RVA: 0x00003580 File Offset: 0x00001780
		public int RequiresBehindSlots { get; set; }

		// Token: 0x06000071 RID: 113 RVA: 0x00003589 File Offset: 0x00001789
		public virtual bool IsAttachable(Entity toEntity, ItemStack itemStack)
		{
			return !(toEntity is EntityPlayer);
		}

		// Token: 0x06000072 RID: 114 RVA: 0x00003598 File Offset: 0x00001798
		public void CollectTextures(ItemStack stack, Shape shape, string texturePrefixCode, Dictionary<string, CompositeTexture> intoDict)
		{
			string type = stack.Attributes.GetString("type", null);
			foreach (string key in shape.Textures.Keys)
			{
				CompositeTexture ctex;
				this.Textures.TryGetValue(type + "-" + key, ref ctex);
				if (ctex != null)
				{
					intoDict[texturePrefixCode + key] = ctex;
				}
				else
				{
					CompositeTexture ctex2;
					this.Textures.TryGetValue(key, ref ctex2);
					intoDict[texturePrefixCode + key] = ctex2;
				}
			}
		}

		// Token: 0x06000073 RID: 115 RVA: 0x00003648 File Offset: 0x00001848
		public string GetCategoryCode(ItemStack stack)
		{
			return "crate";
		}

		// Token: 0x06000074 RID: 116 RVA: 0x00003650 File Offset: 0x00001850
		public Shape GetShape(ItemStack stack, Entity forEntity, string texturePrefixCode)
		{
			string type = stack.Attributes.GetString("type", this.Props.DefaultType);
			stack.Attributes.GetString("label", null);
			string @string = stack.Attributes.GetString("lidState", "closed");
			CompositeShape cshape = this.Props[type].Shape;
			if (this.ShapeInventory != null)
			{
				new Vec3f(this.ShapeInventory.rotateX, this.ShapeInventory.rotateY, this.ShapeInventory.rotateZ);
			}
			ItemStack[] contentStacks = this.GetNonEmptyContents(this.api.World, stack);
			if (contentStacks != null && contentStacks.Length != 0)
			{
				ItemStack itemStack = contentStacks[0];
			}
			if (@string == "opened")
			{
				cshape = cshape.Clone();
				cshape.Base.Path = cshape.Base.Path.Replace("closed", "opened");
			}
			AssetLocation shapeloc = cshape.Base.WithPathAppendixOnce(".json").WithPathPrefixOnce("shapes/");
			Shape shape = Shape.TryGet(this.api, shapeloc);
			shape.SubclassForStepParenting(texturePrefixCode, 0f);
			return shape;
		}

		// Token: 0x06000075 RID: 117 RVA: 0x0000376B File Offset: 0x0000196B
		public CompositeShape GetAttachedShape(ItemStack stack, string slotCode)
		{
			return null;
		}

		// Token: 0x06000076 RID: 118 RVA: 0x0000376E File Offset: 0x0000196E
		public string[] GetDisableElements(ItemStack stack)
		{
			return null;
		}

		// Token: 0x06000077 RID: 119 RVA: 0x00003771 File Offset: 0x00001971
		public string[] GetKeepElements(ItemStack stack)
		{
			return null;
		}

		// Token: 0x06000078 RID: 120 RVA: 0x00003774 File Offset: 0x00001974
		public string GetTexturePrefixCode(ItemStack stack)
		{
			return this.GetKey(stack);
		}

		// Token: 0x17000026 RID: 38
		public TextureAtlasPosition this[string textureCode]
		{
			get
			{
				if (this.nowTeselatingLabel != null)
				{
					return this.labelTexturePos;
				}
				TextureAtlasPosition pos = this.tmpTextureSource[this.curType + "-" + textureCode];
				if (pos == null)
				{
					pos = this.tmpTextureSource[textureCode];
				}
				if (pos == null)
				{
					pos = (this.api as ICoreClientAPI).BlockTextureAtlas.UnknownTexturePosition;
				}
				return pos;
			}
		}

		// Token: 0x0600007A RID: 122 RVA: 0x000037E3 File Offset: 0x000019E3
		public bool DoParticleSelection(IWorldAccessor world, BlockPos pos)
		{
			return true;
		}

		// Token: 0x0600007B RID: 123 RVA: 0x000037E8 File Offset: 0x000019E8
		public override Cuboidf[] GetSelectionBoxes(IBlockAccessor blockAccessor, BlockPos pos)
		{
			BlockEntityCrate be = blockAccessor.GetBlockEntity(pos) as BlockEntityCrate;
			if (be != null)
			{
				return be.GetSelectionBoxes();
			}
			return base.GetSelectionBoxes(blockAccessor, pos);
		}

		// Token: 0x0600007C RID: 124 RVA: 0x00003814 File Offset: 0x00001A14
		public override Cuboidf[] GetCollisionBoxes(IBlockAccessor blockAccessor, BlockPos pos)
		{
			BlockEntityCrate be = blockAccessor.GetBlockEntity(pos) as BlockEntityCrate;
			if (be != null && be.LidState == "closed")
			{
				return this.closedCollBoxes;
			}
			return base.GetCollisionBoxes(blockAccessor, pos);
		}

		// Token: 0x0600007D RID: 125 RVA: 0x00003852 File Offset: 0x00001A52
		public override void OnLoaded(ICoreAPI api)
		{
			base.OnLoaded(api);
			this.Props = this.Attributes.AsObject<CrateProperties>(null, this.Code.Domain);
			this.PlacedPriorityInteract = true;
		}

		// Token: 0x0600007E RID: 126 RVA: 0x00003880 File Offset: 0x00001A80
		public override bool DoPlaceBlock(IWorldAccessor world, IPlayer byPlayer, BlockSelection blockSel, ItemStack byItemStack)
		{
			bool flag = base.DoPlaceBlock(world, byPlayer, blockSel, byItemStack);
			if (flag)
			{
				BlockEntityCrate bect = world.BlockAccessor.GetBlockEntity(blockSel.Position) as BlockEntityCrate;
				if (bect != null)
				{
					BlockPos targetPos = blockSel.DidOffset ? blockSel.Position.AddCopy(blockSel.Face.Opposite) : blockSel.Position;
					double num = byPlayer.Entity.Pos.X - ((double)targetPos.X + blockSel.HitPosition.X);
					double dz = (double)((float)byPlayer.Entity.Pos.Z) - ((double)targetPos.Z + blockSel.HitPosition.Z);
					float angleHor = (float)Math.Atan2(num, dz);
					string type = bect.type;
					string rotatatableInterval = this.Props[type].RotatatableInterval;
					if (rotatatableInterval == "22.5degnot45deg")
					{
						float rounded90degRad = (float)((int)Math.Round((double)(angleHor / 1.5707964f))) * 1.5707964f;
						float deg45rad = 0.3926991f;
						if (Math.Abs(angleHor - rounded90degRad) >= deg45rad)
						{
							bect.MeshAngle = rounded90degRad + 0.3926991f * (float)Math.Sign(angleHor - rounded90degRad);
						}
						else
						{
							bect.MeshAngle = rounded90degRad;
						}
					}
					if (rotatatableInterval == "22.5deg")
					{
						float deg22dot5rad = 0.3926991f;
						float roundRad = (float)((int)Math.Round((double)(angleHor / deg22dot5rad))) * deg22dot5rad;
						bect.MeshAngle = roundRad;
					}
				}
			}
			return flag;
		}

		// Token: 0x0600007F RID: 127 RVA: 0x000039D8 File Offset: 0x00001BD8
		public string GetKey(ItemStack itemstack)
		{
			string type = itemstack.Attributes.GetString("type", this.Props.DefaultType);
			string label = itemstack.Attributes.GetString("label", null);
			string lidState = itemstack.Attributes.GetString("lidState", "closed");
			return string.Concat(new string[]
			{
				type,
				"-",
				label,
				"-",
				lidState
			});
		}

		// Token: 0x06000080 RID: 128 RVA: 0x00003A50 File Offset: 0x00001C50
		public override void OnBeforeRender(ICoreClientAPI capi, ItemStack itemstack, EnumItemRenderTarget target, ref ItemRenderInfo renderinfo)
		{
			string cacheKey = "crateMeshRefs" + base.FirstCodePart(0) + this.SubtypeInventory;
			Dictionary<string, MultiTextureMeshRef> meshrefs = ObjectCacheUtil.GetOrCreate<Dictionary<string, MultiTextureMeshRef>>(capi, cacheKey, () => new Dictionary<string, MultiTextureMeshRef>());
			string key = this.GetKey(itemstack);
			if (!meshrefs.TryGetValue(key, ref renderinfo.ModelRef))
			{
				string type = itemstack.Attributes.GetString("type", this.Props.DefaultType);
				string label = itemstack.Attributes.GetString("label", null);
				string lidState = itemstack.Attributes.GetString("lidState", "closed");
				CompositeShape cshape = this.Props[type].Shape;
				Vec3f rot = (this.ShapeInventory == null) ? null : new Vec3f(this.ShapeInventory.rotateX, this.ShapeInventory.rotateY, this.ShapeInventory.rotateZ);
				ItemStack[] contentStacks = this.GetNonEmptyContents(capi.World, itemstack);
				ItemStack contentStack = (contentStacks == null || contentStacks.Length == 0) ? null : contentStacks[0];
				MeshData mesh = this.GenMesh(capi, contentStack, type, label, lidState, cshape, rot);
				meshrefs[key] = (renderinfo.ModelRef = capi.Render.UploadMultiTextureMesh(mesh));
			}
		}

		// Token: 0x06000081 RID: 129 RVA: 0x00003B9C File Offset: 0x00001D9C
		public override void OnUnloaded(ICoreAPI api)
		{
			ICoreClientAPI capi = api as ICoreClientAPI;
			if (capi == null)
			{
				return;
			}
			string key = "crateMeshRefs" + base.FirstCodePart(0) + this.SubtypeInventory;
			Dictionary<string, MultiTextureMeshRef> meshrefs = ObjectCacheUtil.TryGet<Dictionary<string, MultiTextureMeshRef>>(api, key);
			if (meshrefs != null)
			{
				foreach (KeyValuePair<string, MultiTextureMeshRef> val in meshrefs)
				{
					val.Value.Dispose();
				}
				capi.ObjectCache.Remove(key);
			}
		}

		// Token: 0x06000082 RID: 130 RVA: 0x00003C2C File Offset: 0x00001E2C
		public Shape GetShape(ICoreClientAPI capi, string type, CompositeShape cshape)
		{
			if (((cshape != null) ? cshape.Base : null) == null)
			{
				return null;
			}
			ITesselatorAPI tesselator = capi.Tesselator;
			this.tmpTextureSource = tesselator.GetTextureSource(this, 0, true);
			AssetLocation shapeloc = cshape.Base.WithPathAppendixOnce(".json").WithPathPrefixOnce("shapes/");
			Shape result = Shape.TryGet(capi, shapeloc);
			this.curType = type;
			return result;
		}

		// Token: 0x06000083 RID: 131 RVA: 0x00003C90 File Offset: 0x00001E90
		public MeshData GenMesh(ICoreClientAPI capi, ItemStack contentStack, string type, string label, string lidState, CompositeShape cshape, Vec3f rotation = null)
		{
			if (lidState == "opened")
			{
				cshape = cshape.Clone();
				cshape.Base.Path = cshape.Base.Path.Replace("closed", "opened");
			}
			Shape shape = this.GetShape(capi, type, cshape);
			ITesselatorAPI tesselator = capi.Tesselator;
			if (shape == null)
			{
				return new MeshData(true);
			}
			this.curType = type;
			MeshData mesh;
			tesselator.TesselateShape("crate", shape, ref mesh, this, (rotation == null) ? new Vec3f(this.Shape.rotateX, this.Shape.rotateY, this.Shape.rotateZ) : rotation, 0, 0, 0, default(int?), null);
			LabelProps labelProps;
			if (label != null && this.Props.Labels.TryGetValue(label, ref labelProps))
			{
				MeshData meshLabel = this.GenLabelMesh(capi, label, this.tmpTextureSource[labelProps.Texture], false, rotation);
				mesh.AddMeshData(meshLabel);
			}
			if (contentStack != null && lidState != "closed")
			{
				MeshData contentMesh = this.genContentMesh(capi, contentStack, rotation);
				if (contentMesh != null)
				{
					mesh.AddMeshData(contentMesh);
				}
			}
			return mesh;
		}

		// Token: 0x06000084 RID: 132 RVA: 0x00003DBC File Offset: 0x00001FBC
		public MeshData GenLabelMesh(ICoreClientAPI capi, string label, TextureAtlasPosition texPos, bool editableVariant, Vec3f rotation = null)
		{
			LabelProps labelProps;
			this.Props.Labels.TryGetValue(label, ref labelProps);
			if (labelProps == null)
			{
				throw new ArgumentException("No label props found for this label");
			}
			AssetLocation shapeloc = (editableVariant ? labelProps.EditableShape : labelProps.Shape).Base.Clone().WithPathAppendixOnce(".json").WithPathPrefixOnce("shapes/");
			Shape shape = Shape.TryGet(capi, shapeloc);
			Vec3f rot = (rotation == null) ? new Vec3f(labelProps.Shape.rotateX, labelProps.Shape.rotateY, labelProps.Shape.rotateZ) : rotation;
			this.nowTeselatingLabel = labelProps;
			this.labelTexturePos = texPos;
			this.tmpTextureSource = capi.Tesselator.GetTextureSource(this, 0, true);
			MeshData meshLabel;
			capi.Tesselator.TesselateShape("cratelabel", shape, ref meshLabel, this, rot, 0, 0, 0, default(int?), null);
			this.nowTeselatingLabel = null;
			return meshLabel;
		}

		// Token: 0x06000085 RID: 133 RVA: 0x00003EA8 File Offset: 0x000020A8
		protected MeshData genContentMesh(ICoreClientAPI capi, ItemStack contentStack, Vec3f rotation = null)
		{
			float fillHeight;
			ITexPositionSource contentSource = BlockBarrel.getContentTexture(capi, contentStack, ref fillHeight);
			if (contentSource != null)
			{
				Shape shape = Shape.TryGet(this.api, "shapes/block/wood/crate/contents.json");
				MeshData contentMesh;
				capi.Tesselator.TesselateShape("cratecontents", shape, ref contentMesh, contentSource, rotation, 0, 0, 0, default(int?), null);
				contentMesh.Translate(0f, fillHeight * 1.1f, 0f);
				return contentMesh;
			}
			return null;
		}

		// Token: 0x06000086 RID: 134 RVA: 0x00003F14 File Offset: 0x00002114
		public override void GetDecal(IWorldAccessor world, BlockPos pos, ITexPositionSource decalTexSource, ref MeshData decalModelData, ref MeshData blockModelData)
		{
			BlockEntityCrate be = world.BlockAccessor.GetBlockEntity(pos) as BlockEntityCrate;
			if (be != null)
			{
				decalModelData.Rotate(this.origin, 0f, be.MeshAngle, 0f);
				decalModelData.Scale(this.origin, 0.9375f, 1f, 0.9375f);
				return;
			}
			base.GetDecal(world, pos, decalTexSource, ref decalModelData, ref blockModelData);
		}

		// Token: 0x06000087 RID: 135 RVA: 0x00003F84 File Offset: 0x00002184
		public override ItemStack OnPickBlock(IWorldAccessor world, BlockPos pos)
		{
			ItemStack stack = new ItemStack(this, 1);
			BlockEntityCrate be = world.BlockAccessor.GetBlockEntity(pos) as BlockEntityCrate;
			if (be != null)
			{
				stack.Attributes.SetString("type", be.type);
				if (be.label != null && be.label.Length > 0)
				{
					stack.Attributes.SetString("label", be.label);
				}
				stack.Attributes.SetString("lidState", be.preferredLidState);
			}
			else
			{
				stack.Attributes.SetString("type", this.Props.DefaultType);
			}
			return stack;
		}

		// Token: 0x06000088 RID: 136 RVA: 0x00004024 File Offset: 0x00002224
		public override ItemStack[] GetDrops(IWorldAccessor world, BlockPos pos, IPlayer byPlayer, float dropQuantityMultiplier = 1f)
		{
			return new ItemStack[]
			{
				this.OnPickBlock(world, pos)
			};
		}

		// Token: 0x06000089 RID: 137 RVA: 0x00004037 File Offset: 0x00002237
		public override BlockDropItemStack[] GetDropsForHandbook(ItemStack handbookStack, IPlayer forPlayer)
		{
			return new BlockDropItemStack[]
			{
				new BlockDropItemStack(handbookStack, 1f)
			};
		}

		// Token: 0x0600008A RID: 138 RVA: 0x00004050 File Offset: 0x00002250
		public override bool OnBlockInteractStart(IWorldAccessor world, IPlayer byPlayer, BlockSelection blockSel)
		{
			BlockEntityCrate be = world.BlockAccessor.GetBlockEntity(blockSel.Position) as BlockEntityCrate;
			if (be != null)
			{
				return be.OnBlockInteractStart(byPlayer, blockSel);
			}
			return base.OnBlockInteractStart(world, byPlayer, blockSel);
		}

		// Token: 0x0600008B RID: 139 RVA: 0x0000408C File Offset: 0x0000228C
		public override string GetHeldItemName(ItemStack itemStack)
		{
			string type = itemStack.Attributes.GetString("type", this.Props.DefaultType);
			string lidState = itemStack.Attributes.GetString("lidState", "closed");
			if (lidState.Length == 0)
			{
				lidState = "closed";
			}
			string[] array = new string[5];
			int num = 0;
			AssetLocation code = this.Code;
			array[num] = ((code != null) ? code.Domain : null);
			array[1] = ":block-";
			array[2] = type;
			array[3] = "-";
			int num2 = 4;
			AssetLocation code2 = this.Code;
			array[num2] = ((code2 != null) ? code2.Path : null);
			return Lang.GetMatching(string.Concat(array), new object[]
			{
				Lang.Get("cratelidstate-" + lidState, new object[]
				{
					"closed"
				})
			});
		}

		// Token: 0x0600008C RID: 140 RVA: 0x00004150 File Offset: 0x00002350
		public override void GetHeldItemInfo(ItemSlot inSlot, StringBuilder dsc, IWorldAccessor world, bool withDebugInfo)
		{
			base.GetHeldItemInfo(inSlot, dsc, world, withDebugInfo);
			string type = inSlot.Itemstack.Attributes.GetString("type", this.Props.DefaultType);
			if (type != null)
			{
				int qslots = this.Props[type].QuantitySlots;
				dsc.AppendLine("\n" + Lang.Get("Storage Slots: {0}", new object[]
				{
					qslots
				}));
			}
		}

		// Token: 0x0600008D RID: 141 RVA: 0x000041C8 File Offset: 0x000023C8
		public override string GetPlacedBlockInfo(IWorldAccessor world, BlockPos pos, IPlayer forPlayer)
		{
			return base.GetPlacedBlockInfo(world, pos, forPlayer);
		}

		// Token: 0x0600008E RID: 142 RVA: 0x000041D4 File Offset: 0x000023D4
		public override int GetRandomColor(ICoreClientAPI capi, BlockPos pos, BlockFacing facing, int rndIndex = -1)
		{
			BlockEntityGenericTypedContainer be = capi.World.BlockAccessor.GetBlockEntity(pos) as BlockEntityGenericTypedContainer;
			if (be != null)
			{
				CompositeTexture tex;
				if (!this.Textures.TryGetValue(be.type + "-lid", ref tex))
				{
					this.Textures.TryGetValue(be.type + "-top", ref tex);
				}
				return capi.BlockTextureAtlas.GetRandomColor((((tex != null) ? tex.Baked : null) == null) ? 0 : tex.Baked.TextureSubId, rndIndex);
			}
			return base.GetRandomColor(capi, pos, facing, rndIndex);
		}

		// Token: 0x0600008F RID: 143 RVA: 0x00004270 File Offset: 0x00002470
		public string GetType(IBlockAccessor blockAccessor, BlockPos pos)
		{
			BlockEntityGenericTypedContainer be = blockAccessor.GetBlockEntity(pos) as BlockEntityGenericTypedContainer;
			if (be != null)
			{
				return be.type;
			}
			return this.Props.DefaultType;
		}

		// Token: 0x06000090 RID: 144 RVA: 0x000042A0 File Offset: 0x000024A0
		public override WorldInteraction[] GetPlacedBlockInteractionHelp(IWorldAccessor world, BlockSelection selection, IPlayer forPlayer)
		{
			return ArrayExtensions.Append<WorldInteraction>(base.GetPlacedBlockInteractionHelp(world, selection, forPlayer), new WorldInteraction[]
			{
				new WorldInteraction
				{
					ActionLangCode = "blockhelp-crate-add",
					MouseButton = 2,
					HotKeyCode = "shift"
				},
				new WorldInteraction
				{
					ActionLangCode = "blockhelp-crate-addall",
					MouseButton = 2,
					HotKeyCodes = new string[]
					{
						"shift",
						"ctrl"
					}
				},
				new WorldInteraction
				{
					ActionLangCode = "blockhelp-crate-remove",
					MouseButton = 2,
					HotKeyCode = null
				},
				new WorldInteraction
				{
					ActionLangCode = "blockhelp-crate-removeall",
					MouseButton = 2,
					HotKeyCode = "ctrl"
				}
			});
		}

		// Token: 0x04000043 RID: 67
		private string curType;

		// Token: 0x04000044 RID: 68
		private LabelProps nowTeselatingLabel;

		// Token: 0x04000045 RID: 69
		private ITexPositionSource tmpTextureSource;

		// Token: 0x04000046 RID: 70
		private TextureAtlasPosition labelTexturePos;

		// Token: 0x04000047 RID: 71
		public CrateProperties Props;

		// Token: 0x04000049 RID: 73
		private Vec3f origin = new Vec3f(0.5f, 0.5f, 0.5f);

		// Token: 0x0400004A RID: 74
		private Cuboidf[] closedCollBoxes = new Cuboidf[]
		{
			new Cuboidf(0.0625f, 0f, 0.0625f, 0.9375f, 0.9375f, 0.9375f)
		};
	}
}
