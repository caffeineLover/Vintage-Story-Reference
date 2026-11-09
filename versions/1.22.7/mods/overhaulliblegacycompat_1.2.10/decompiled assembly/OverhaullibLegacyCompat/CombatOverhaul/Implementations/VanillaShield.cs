using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using CombatOverhaul.Utils;
using Newtonsoft.Json.Linq;
using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.Config;
using Vintagestory.API.Datastructures;
using Vintagestory.API.MathTools;
using Vintagestory.API.Util;
using Vintagestory.GameContent;

namespace CombatOverhaul.Implementations
{
	// Token: 0x02000089 RID: 137
	[NullableContext(1)]
	[Nullable(0)]
	public class VanillaShield : MeleeWeapon, IContainedMeshSource
	{
		// Token: 0x1700016D RID: 365
		// (get) Token: 0x0600073B RID: 1851 RVA: 0x0002AF02 File Offset: 0x00029102
		public string Construction
		{
			get
			{
				return this.Variant["construction"];
			}
		}

		// Token: 0x1700016E RID: 366
		// (get) Token: 0x0600073C RID: 1852 RVA: 0x0002AF14 File Offset: 0x00029114
		private Dictionary<int, MultiTextureMeshRef> Meshrefs
		{
			get
			{
				return ObjectCacheUtil.GetOrCreate<Dictionary<int, MultiTextureMeshRef>>(this.api, "shieldmeshrefs", () => new Dictionary<int, MultiTextureMeshRef>());
			}
		}

		// Token: 0x0600073D RID: 1853 RVA: 0x0002AF48 File Offset: 0x00029148
		public override void OnLoaded(ICoreAPI api)
		{
			base.OnLoaded(api);
			this._clientAPI = (api as ICoreClientAPI);
			JsonObject attributes = this.Attributes;
			this._durabilityGains = (((attributes != null) ? attributes["durabilityGains"].AsObject<Dictionary<string, Dictionary<string, int>>>(null) : null) ?? new Dictionary<string, Dictionary<string, int>>());
			this.AddAllTypesToCreativeInventory();
		}

		// Token: 0x0600073E RID: 1854 RVA: 0x0002AF9C File Offset: 0x0002919C
		public override void OnUnloaded(ICoreAPI api)
		{
			if (api.ObjectCache.ContainsKey("shieldmeshrefs") && this.Meshrefs.Count > 0)
			{
				foreach (KeyValuePair<int, MultiTextureMeshRef> keyValuePair in this.Meshrefs)
				{
					int num;
					MultiTextureMeshRef multiTextureMeshRef;
					keyValuePair.Deconstruct(ref num, ref multiTextureMeshRef);
					multiTextureMeshRef.Dispose();
				}
				ObjectCacheUtil.Delete(api, "shieldmeshrefs");
			}
			base.OnUnloaded(api);
		}

		// Token: 0x0600073F RID: 1855 RVA: 0x0002B02C File Offset: 0x0002922C
		public override int GetMaxDurability(ItemStack itemstack)
		{
			int gain = 0;
			if (itemstack != null)
			{
				foreach (KeyValuePair<string, Dictionary<string, int>> val in this._durabilityGains)
				{
					ITreeAttribute attributes = itemstack.Attributes;
					string mat = (attributes != null) ? attributes.GetString(val.Key, null) : null;
					if (mat != null)
					{
						int matgain;
						val.Value.TryGetValue(mat, ref matgain);
						gain += matgain;
					}
				}
			}
			return base.GetMaxDurability(itemstack) + gain;
		}

		// Token: 0x06000740 RID: 1856 RVA: 0x0002B0BC File Offset: 0x000292BC
		public override List<ItemStack> GetHandBookStacks(ICoreClientAPI capi)
		{
			if (this.Code == null)
			{
				return null;
			}
			JsonObject attributes = this.Attributes;
			bool flag;
			if (attributes == null)
			{
				flag = false;
			}
			else
			{
				JsonObject jsonObject = attributes["handbook"];
				flag = ((jsonObject != null) ? new bool?(jsonObject["exclude"].AsBool(false)) : default(bool?)).GetValueOrDefault();
			}
			if (flag)
			{
				return null;
			}
			if (this.Construction == "crude" || this.Construction == "blackguard")
			{
				return base.GetHandBookStacks(capi);
			}
			ItemStack stack = new ItemStack(this, 1);
			stack.Attributes.SetBool("combatOverhaulHandbookRepresentative", true);
			stack.TempAttributes.SetBool("combatOverhaulHandbookRepresentative", true);
			string construction = this.Construction;
			if (!(construction == "woodmetal"))
			{
				if (!(construction == "woodmetalleather"))
				{
					if (construction == "metal")
					{
						stack.Attributes.SetString("metal", "tinbronze");
						stack.Attributes.SetString("color", "brown");
						stack.Attributes.SetString("deco", "none");
					}
				}
				else
				{
					stack.Attributes.SetString("metal", "tinbronze");
					stack.Attributes.SetString("wood", "generic");
					stack.Attributes.SetString("color", "brown");
					stack.Attributes.SetString("deco", "none");
				}
			}
			else
			{
				stack.Attributes.SetString("metal", "tinbronze");
				stack.Attributes.SetString("wood", "generic");
				stack.Attributes.SetString("deco", "none");
			}
			List<ItemStack> list = new List<ItemStack>();
			list.Add(stack);
			return list;
		}

		// Token: 0x06000741 RID: 1857 RVA: 0x0002B294 File Offset: 0x00029494
		public override bool Satisfies(ItemStack thisStack, ItemStack otherStack)
		{
			return (VanillaShield.IsHandbookRepresentative(otherStack) && ((thisStack != null) ? thisStack.Collectible : null) == this && otherStack.Collectible == this && thisStack.Class == otherStack.Class && thisStack.Id == otherStack.Id) || base.Satisfies(thisStack, otherStack);
		}

		// Token: 0x06000742 RID: 1858 RVA: 0x0002B2E8 File Offset: 0x000294E8
		public void AddAllTypesToCreativeInventory()
		{
			if (this.Construction == "crude" || this.Construction == "blackguard")
			{
				return;
			}
			List<JsonItemStack> stacks = new List<JsonItemStack>();
			JsonObject attributes = this.Attributes;
			Dictionary<string, string[]> vg = (attributes != null) ? attributes["variantGroups"].AsObject<Dictionary<string, string[]>>(null) : null;
			if (vg == null || !vg.ContainsKey("metal"))
			{
				return;
			}
			foreach (string metal in vg["metal"])
			{
				string construction = this.Construction;
				if (!(construction == "woodmetal"))
				{
					if (!(construction == "woodmetalleather"))
					{
						if (construction == "metal")
						{
							stacks.Add(this.GenJstack(string.Format("{{ wood: \"{0}\", metal: \"{1}\", deco: \"none\" }}", "generic", metal)));
							if (vg.ContainsKey("color"))
							{
								foreach (string color in vg["color"])
								{
									if (color != "redblack")
									{
										stacks.Add(this.GenJstack(string.Format("{{ wood: \"{0}\", metal: \"{1}\", color: \"{2}\", deco: \"ornate\" }}", "generic", metal, color)));
									}
								}
							}
						}
					}
					else if (vg.ContainsKey("color"))
					{
						foreach (string color2 in vg["color"])
						{
							stacks.Add(this.GenJstack(string.Format("{{ wood: \"{0}\", metal: \"{1}\", color: \"{2}\", deco: \"none\" }}", "generic", metal, color2)));
							if (color2 != "redblack")
							{
								stacks.Add(this.GenJstack(string.Format("{{ wood: \"{0}\", metal: \"{1}\", color: \"{2}\", deco: \"ornate\" }}", "generic", metal, color2)));
							}
						}
					}
				}
				else if (vg.ContainsKey("wood"))
				{
					foreach (string wood in vg["wood"])
					{
						stacks.Add(this.GenJstack(string.Format("{{ wood: \"{0}\", metal: \"{1}\", deco: \"none\" }}", wood, metal)));
					}
				}
			}
			this.CreativeInventoryStacks = new CreativeTabAndStackList[]
			{
				new CreativeTabAndStackList
				{
					Stacks = stacks.ToArray(),
					Tabs = new string[]
					{
						"general",
						"items",
						"tools"
					}
				}
			};
		}

		// Token: 0x06000743 RID: 1859 RVA: 0x0002B554 File Offset: 0x00029754
		private JsonItemStack GenJstack(string json)
		{
			JsonItemStack jsonItemStack = new JsonItemStack();
			jsonItemStack.Code = this.Code;
			jsonItemStack.Type = 1;
			jsonItemStack.Attributes = new JsonObject(JToken.Parse(json));
			jsonItemStack.Resolve(this.api.World, "shield type");
			return jsonItemStack;
		}

		// Token: 0x06000744 RID: 1860 RVA: 0x0002B5A4 File Offset: 0x000297A4
		public override void OnBeforeRender(ICoreClientAPI capi, ItemStack itemstack, EnumItemRenderTarget target, ref ItemRenderInfo renderinfo)
		{
			int meshrefid = itemstack.TempAttributes.GetInt("meshRefId", 0);
			if (meshrefid == 0 || !this.Meshrefs.TryGetValue(meshrefid, ref renderinfo.ModelRef))
			{
				int id = this.Meshrefs.Count + 1;
				MultiTextureMeshRef modelref = capi.Render.UploadMultiTextureMesh(this.GenMesh(itemstack, capi.ItemTextureAtlas));
				renderinfo.ModelRef = (this.Meshrefs[id] = modelref);
				itemstack.TempAttributes.SetInt("meshRefId", id);
			}
			base.OnBeforeRender(capi, itemstack, target, ref renderinfo);
		}

		// Token: 0x06000745 RID: 1861 RVA: 0x0002B638 File Offset: 0x00029838
		public MeshData GenMesh(ItemStack itemstack, ITextureAtlasAPI targetAtlas)
		{
			ContainedTextureSource cnts = new ContainedTextureSource(this.api as ICoreClientAPI, targetAtlas, new Dictionary<string, AssetLocation>(), string.Format("For render in shield {0}", this.Code));
			cnts.Textures.Clear();
			string wood = itemstack.Attributes.GetString("wood", null);
			string metal = itemstack.Attributes.GetString("metal", null);
			string color = itemstack.Attributes.GetString("color", null);
			string deco = itemstack.Attributes.GetString("deco", null);
			if (wood == null && metal == null && this.Construction != "crude" && this.Construction != "blackguard")
			{
				return new MeshData(true);
			}
			if (wood == null || wood == "")
			{
				wood = "generic";
			}
			cnts.Textures["front"] = (cnts.Textures["back"] = (cnts.Textures["handle"] = new AssetLocation("block/wood/planks/generic.png")));
			ICoreClientAPI clientAPI = this._clientAPI;
			Shape shape = (clientAPI != null) ? clientAPI.TesselatorManager.GetCachedShape(this.Shape.Base) : null;
			if (shape == null)
			{
				return new MeshData(true);
			}
			foreach (KeyValuePair<string, AssetLocation> ctex in shape.Textures)
			{
				cnts.Textures[ctex.Key] = ctex.Value;
			}
			string construction = this.Construction;
			if (!(construction == "crude") && !(construction == "blackguard"))
			{
				if (!(construction == "woodmetal"))
				{
					if (!(construction == "woodmetalleather"))
					{
						if (construction == "metal")
						{
							cnts.Textures["rim"] = (cnts.Textures["handle"] = new AssetLocation("block/metal/sheet/" + metal + "1.png"));
							cnts.Textures["front"] = (cnts.Textures["back"] = new AssetLocation("block/metal/plate/" + metal + ".png"));
							if (deco == "ornate")
							{
								cnts.Textures["front"] = new AssetLocation("item/tool/shield/ornate/" + color + ".png");
							}
						}
					}
					else
					{
						if (wood != "generic")
						{
							cnts.Textures["handle"] = (cnts.Textures["back"] = (cnts.Textures["front"] = new AssetLocation("block/wood/debarked/" + wood + ".png")));
						}
						cnts.Textures["front"] = new AssetLocation("item/tool/shield/leather/" + color + ".png");
						cnts.Textures["rim"] = new AssetLocation("block/metal/sheet/" + metal + "1.png");
						if (deco == "ornate")
						{
							cnts.Textures["front"] = new AssetLocation("item/tool/shield/ornate/" + color + ".png");
						}
					}
				}
				else
				{
					if (wood != "generic")
					{
						cnts.Textures["handle"] = (cnts.Textures["back"] = (cnts.Textures["front"] = new AssetLocation("block/wood/debarked/" + wood + ".png")));
					}
					cnts.Textures["rim"] = new AssetLocation("block/metal/sheet/" + metal + "1.png");
					if (deco == "ornate")
					{
						cnts.Textures["front"] = new AssetLocation("item/tool/shield/ornate/" + color + ".png");
					}
				}
			}
			MeshData mesh;
			this._clientAPI.Tesselator.TesselateItem(this, ref mesh, cnts);
			return mesh;
		}

		// Token: 0x06000746 RID: 1862 RVA: 0x0002BA94 File Offset: 0x00029C94
		public override string GetHeldItemName(ItemStack itemStack)
		{
			string construction;
			if (VanillaShield.IsHandbookRepresentative(itemStack))
			{
				construction = this.Construction;
				string result;
				if (!(construction == "woodmetal"))
				{
					if (!(construction == "woodmetalleather"))
					{
						if (!(construction == "metal"))
						{
							result = base.GetHeldItemName(itemStack);
						}
						else
						{
							result = Lang.Get("Metal shield", Array.Empty<object>());
						}
					}
					else
					{
						result = Lang.Get("Leather reinforced wooden shield", Array.Empty<object>());
					}
				}
				else
				{
					result = Lang.Get("Wooden shield", Array.Empty<object>());
				}
				return result;
			}
			bool ornate = itemStack.Attributes.GetString("deco", null) == "ornate";
			string metal = itemStack.Attributes.GetString("metal", null);
			string wood = itemStack.Attributes.GetString("wood", null);
			string color = itemStack.Attributes.GetString("color", null);
			construction = this.Construction;
			if (construction == "crude")
			{
				return Lang.Get("Crude shield", Array.Empty<object>());
			}
			if (!(construction == "woodmetal"))
			{
				if (!(construction == "woodmetalleather"))
				{
					if (construction == "metal")
					{
						return this.GetMetalShieldName(metal, color, ornate);
					}
					if (!(construction == "blackguard"))
					{
						return base.GetHeldItemName(itemStack);
					}
					return Lang.Get("Blackguard shield", Array.Empty<object>());
				}
				else
				{
					if (!ornate)
					{
						return Lang.Get("Leather reinforced wooden shield", Array.Empty<object>());
					}
					return Lang.Get("Ornate leather reinforced wooden shield", Array.Empty<object>());
				}
			}
			else if (wood == "generic")
			{
				if (!ornate)
				{
					return Lang.Get("Wooden shield", Array.Empty<object>());
				}
				return Lang.Get("Ornate wooden shield", Array.Empty<object>());
			}
			else if (wood == "aged")
			{
				if (!ornate)
				{
					return Lang.Get("Aged wooden shield", Array.Empty<object>());
				}
				return Lang.Get("Aged ornate shield", Array.Empty<object>());
			}
			else
			{
				if (!ornate)
				{
					return Lang.Get("{0} shield", new object[]
					{
						Lang.Get("material-" + wood, Array.Empty<object>())
					});
				}
				return Lang.Get("Ornate {0} shield", new object[]
				{
					Lang.Get("material-" + wood, Array.Empty<object>())
				});
			}
		}

		// Token: 0x06000747 RID: 1863 RVA: 0x0002BCDC File Offset: 0x00029EDC
		public override void GetHeldItemInfo(ItemSlot inSlot, StringBuilder dsc, IWorldAccessor world, bool withDebugInfo)
		{
			base.GetHeldItemInfo(inSlot, dsc, world, withDebugInfo);
			if (inSlot.Itemstack == null)
			{
				return;
			}
			string construction = this.Construction;
			if (!(construction == "woodmetal"))
			{
				if (!(construction == "woodmetalleather"))
				{
					return;
				}
				if (VanillaShield.IsHandbookRepresentative(inSlot.Itemstack))
				{
					return;
				}
				dsc.AppendLine(Lang.Get("shield-metaltype", new object[]
				{
					Lang.Get("material-" + inSlot.Itemstack.Attributes.GetString("metal", null), Array.Empty<object>())
				}));
				return;
			}
			else
			{
				if (VanillaShield.IsHandbookRepresentative(inSlot.Itemstack))
				{
					return;
				}
				dsc.AppendLine(Lang.Get("shield-woodtype", new object[]
				{
					Lang.Get("material-" + inSlot.Itemstack.Attributes.GetString("wood", null), Array.Empty<object>())
				}));
				dsc.AppendLine(Lang.Get("shield-metaltype", new object[]
				{
					Lang.Get("material-" + inSlot.Itemstack.Attributes.GetString("metal", null), Array.Empty<object>())
				}));
				return;
			}
		}

		// Token: 0x06000748 RID: 1864 RVA: 0x0002BE0C File Offset: 0x0002A00C
		[NullableContext(2)]
		[return: Nullable(1)]
		private string GetMetalShieldName(string metal, string color, bool ornate)
		{
			if (!string.IsNullOrEmpty(metal))
			{
				string text;
				if (!ornate || string.IsNullOrEmpty(color))
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
					defaultInterpolatedStringHandler..ctor(11, 2);
					defaultInterpolatedStringHandler.AppendLiteral("item-");
					defaultInterpolatedStringHandler.AppendFormatted(this.Code.Path);
					defaultInterpolatedStringHandler.AppendLiteral("-");
					defaultInterpolatedStringHandler.AppendFormatted(metal);
					defaultInterpolatedStringHandler.AppendLiteral("-none");
					text = defaultInterpolatedStringHandler.ToStringAndClear();
				}
				else
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler2;
					defaultInterpolatedStringHandler2..ctor(14, 3);
					defaultInterpolatedStringHandler2.AppendLiteral("item-");
					defaultInterpolatedStringHandler2.AppendFormatted(this.Code.Path);
					defaultInterpolatedStringHandler2.AppendLiteral("-");
					defaultInterpolatedStringHandler2.AppendFormatted(metal);
					defaultInterpolatedStringHandler2.AppendLiteral("-");
					defaultInterpolatedStringHandler2.AppendFormatted(color);
					defaultInterpolatedStringHandler2.AppendLiteral("-ornate");
					text = defaultInterpolatedStringHandler2.ToStringAndClear();
				}
				string vanillaName = Lang.GetMatchingIfExists(text, Array.Empty<object>());
				if (!string.IsNullOrEmpty(vanillaName))
				{
					return vanillaName;
				}
			}
			string materialName = VanillaShield.GetMaterialName(metal);
			return Lang.Get("{0} round shield", new object[]
			{
				materialName
			});
		}

		// Token: 0x06000749 RID: 1865 RVA: 0x0002BF14 File Offset: 0x0002A114
		private static string GetMaterialName([Nullable(2)] string material)
		{
			if (string.IsNullOrEmpty(material))
			{
				return Lang.Get("Metal", Array.Empty<object>());
			}
			string key = "material-" + material;
			string translated = Lang.Get(key, Array.Empty<object>());
			if (!string.Equals(translated, key, 4))
			{
				return translated;
			}
			return VanillaShield.HumanizeMaterialCode(material);
		}

		// Token: 0x0600074A RID: 1866 RVA: 0x0002BF64 File Offset: 0x0002A164
		private static string HumanizeMaterialCode(string material)
		{
			string result;
			if (!(material == "tinbronze"))
			{
				if (!(material == "bismuthbronze"))
				{
					if (!(material == "blackbronze"))
					{
						if (!(material == "meteoriciron"))
						{
							if (material.Length > 1)
							{
								char c = char.ToUpperInvariant(material.get_Chars(0));
								result = new ReadOnlySpan<char>(ref c) + material.Substring(1, material.Length - 1);
							}
							else
							{
								result = material.ToUpperInvariant();
							}
						}
						else
						{
							result = "Meteoric iron";
						}
					}
					else
					{
						result = "Black bronze";
					}
				}
				else
				{
					result = "Bismuth bronze";
				}
			}
			else
			{
				result = "Tin bronze";
			}
			return result;
		}

		// Token: 0x0600074B RID: 1867 RVA: 0x0002C00C File Offset: 0x0002A20C
		public MeshData GenMesh(ItemStack itemstack, ITextureAtlasAPI targetAtlas, BlockPos atBlockPos)
		{
			return this.GenMesh(itemstack, targetAtlas);
		}

		// Token: 0x0600074C RID: 1868 RVA: 0x0002C016 File Offset: 0x0002A216
		MeshData IContainedMeshSource.GenMesh(ItemSlot inSlot, ITextureAtlasAPI targetAtlas, BlockPos atBlockPos)
		{
			return this.GenMesh(inSlot.Itemstack, targetAtlas, atBlockPos);
		}

		// Token: 0x0600074D RID: 1869 RVA: 0x0002C028 File Offset: 0x0002A228
		public string GetMeshCacheKey(ItemStack itemstack)
		{
			string wood = itemstack.Attributes.GetString("wood", null);
			string metal = itemstack.Attributes.GetString("metal", null);
			string color = itemstack.Attributes.GetString("color", null);
			string deco = itemstack.Attributes.GetString("deco", null);
			return string.Concat(new string[]
			{
				this.Code.ToShortString(),
				"-",
				wood,
				"-",
				metal,
				"-",
				color,
				"-",
				deco
			});
		}

		// Token: 0x0600074E RID: 1870 RVA: 0x0002C0C7 File Offset: 0x0002A2C7
		string IContainedMeshSource.GetMeshCacheKey(ItemSlot inSlot)
		{
			return this.GetMeshCacheKey(inSlot.Itemstack);
		}

		// Token: 0x0600074F RID: 1871 RVA: 0x0002C0D5 File Offset: 0x0002A2D5
		public override void OnCreatedByCrafting(ItemSlot[] allInputslots, ItemSlot outputSlot, IRecipeBase byRecipe)
		{
			base.OnCreatedByCrafting(allInputslots, outputSlot, byRecipe);
			GeneralUtils.MarkItemStack(outputSlot);
			outputSlot.MarkDirty();
		}

		// Token: 0x06000750 RID: 1872 RVA: 0x0002C0F0 File Offset: 0x0002A2F0
		[NullableContext(2)]
		private static bool IsHandbookRepresentative(ItemStack itemStack)
		{
			if (itemStack != null)
			{
				ITreeAttribute tempAttributes = itemStack.TempAttributes;
				if (((tempAttributes != null) ? new bool?(tempAttributes.GetBool("combatOverhaulHandbookRepresentative", false)) : default(bool?)).GetValueOrDefault())
				{
					return true;
				}
			}
			if (itemStack == null)
			{
				return false;
			}
			ITreeAttribute attributes = itemStack.Attributes;
			return ((attributes != null) ? new bool?(attributes.GetBool("combatOverhaulHandbookRepresentative", false)) : default(bool?)).GetValueOrDefault();
		}

		// Token: 0x04000335 RID: 821
		private const string HandbookRepresentativeAttribute = "combatOverhaulHandbookRepresentative";

		// Token: 0x04000336 RID: 822
		[Nullable(2)]
		private ICoreClientAPI _clientAPI;

		// Token: 0x04000337 RID: 823
		private Dictionary<string, Dictionary<string, int>> _durabilityGains = new Dictionary<string, Dictionary<string, int>>();
	}
}
