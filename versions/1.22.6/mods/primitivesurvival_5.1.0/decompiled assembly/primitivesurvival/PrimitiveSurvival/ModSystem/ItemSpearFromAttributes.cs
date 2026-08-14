using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Newtonsoft.Json.Linq;
using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.Config;
using Vintagestory.API.Datastructures;
using Vintagestory.API.MathTools;
using Vintagestory.API.Util;
using Vintagestory.GameContent;

namespace PrimitiveSurvival.ModSystem
{
	// Token: 0x0200004D RID: 77
	public class ItemSpearFromAttributes : ItemFishingSpear, IContainedMeshSource, IHandbookGrouping
	{
		// Token: 0x1700005B RID: 91
		// (get) Token: 0x060002DE RID: 734 RVA: 0x00021C77 File Offset: 0x0001FE77
		private Dictionary<int, MultiTextureMeshRef> meshrefs
		{
			get
			{
				return ObjectCacheUtil.GetOrCreate<Dictionary<int, MultiTextureMeshRef>>(this.api, "spearmeshrefs", () => new Dictionary<int, MultiTextureMeshRef>());
			}
		}

		// Token: 0x060002DF RID: 735 RVA: 0x00021CA8 File Offset: 0x0001FEA8
		public override void OnLoaded(ICoreAPI api)
		{
			base.OnLoaded(api);
			this.capi = (api as ICoreClientAPI);
			this.variantGroups = this.Attributes["variantGroups"].AsObject<OrderedDictionary<string, string[]>>(this.variantGroups);
			if (this.variantGroups == null)
			{
				throw new Exception("Spear " + this.Code + " has improperly defined variantGroups");
			}
			this.AddEmptyToCreativeInventory();
		}

		// Token: 0x060002E0 RID: 736 RVA: 0x00021D18 File Offset: 0x0001FF18
		public override void OnUnloaded(ICoreAPI api)
		{
			if (api.ObjectCache.ContainsKey("spearmeshrefs") && this.meshrefs.Count > 0)
			{
				foreach (KeyValuePair<int, MultiTextureMeshRef> keyValuePair in this.meshrefs)
				{
					int num;
					MultiTextureMeshRef multiTextureMeshRef;
					keyValuePair.Deconstruct(ref num, ref multiTextureMeshRef);
					multiTextureMeshRef.Dispose();
				}
				ObjectCacheUtil.Delete(api, "spearmeshrefs");
			}
			base.OnUnloaded(api);
		}

		// Token: 0x060002E1 RID: 737 RVA: 0x00021DA8 File Offset: 0x0001FFA8
		private void AddEmptyToCreativeInventory()
		{
			List<JsonItemStack> stacks = new List<JsonItemStack>();
			Dictionary<string, string>[] array = this.Attributes["allowedVariants"].AsArray<Dictionary<string, string>>(null, "game");
			for (int i = 0; i < array.Length; i++)
			{
				Dictionary<string, string> entry = array[i];
				Dictionary<string, string[]> attributes = new Dictionary<string, string[]>();
				string val;
				foreach (string key2 in this.variantGroups.Keys)
				{
					if (entry.TryGetValue(key2, ref val))
					{
						Dictionary<string, string[]> attributes2 = attributes;
						string text = key2;
						string[] array2;
						if (!(val == "*"))
						{
							if (val == null)
							{
								array2 = Array.Empty<string>();
							}
							else
							{
								(array2 = new string[1])[0] = val;
							}
						}
						else
						{
							array2 = Enumerable.ToArray<string>(this.variantGroups[key2]);
						}
						attributes2.Add(text, array2);
					}
				}
				string[] variants = new string[]
				{
					""
				};
				using (Dictionary<string, string[]>.KeyCollection.Enumerator enumerator2 = attributes.Keys.GetEnumerator())
				{
					while (enumerator2.MoveNext())
					{
						string key = enumerator2.Current;
						IEnumerable<string> newVariants = Enumerable.SelectMany<string, string>(variants, (string variant) => Enumerable.Select<string, string>(attributes[key], delegate(string val)
						{
							string variant = variant;
							DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
							defaultInterpolatedStringHandler..ctor(6, 2);
							defaultInterpolatedStringHandler.AppendLiteral(" ");
							defaultInterpolatedStringHandler.AppendFormatted(key);
							defaultInterpolatedStringHandler.AppendLiteral(": \"");
							defaultInterpolatedStringHandler.AppendFormatted(val);
							defaultInterpolatedStringHandler.AppendLiteral("\",");
							return variant + defaultInterpolatedStringHandler.ToStringAndClear();
						}));
						if (Enumerable.Any<string>(newVariants))
						{
							variants = Enumerable.ToArray<string>(newVariants);
						}
					}
				}
				variants = Enumerable.ToArray<string>(Enumerable.Select<string, string>(variants, (string var) => "{" + var + " }"));
				stacks.AddRange(Enumerable.Select<string, JsonItemStack>(variants, new Func<string, JsonItemStack>(this.genJstack)));
			}
			this.CreativeInventoryStacks = new CreativeTabAndStackList[]
			{
				new CreativeTabAndStackList
				{
					Stacks = stacks.ToArray(),
					Tabs = new string[]
					{
						"primitive",
						"tools"
					}
				}
			};
		}

		// Token: 0x060002E2 RID: 738 RVA: 0x00021FB0 File Offset: 0x000201B0
		private JsonItemStack genJstack(string json)
		{
			JsonItemStack jsonItemStack = new JsonItemStack();
			jsonItemStack.Code = this.Code;
			jsonItemStack.Type = 1;
			jsonItemStack.Attributes = new JsonObject(JToken.Parse(json));
			jsonItemStack.Resolve(this.api.World, "spear type");
			return jsonItemStack;
		}

		// Token: 0x060002E3 RID: 739 RVA: 0x00022000 File Offset: 0x00020200
		public override void OnBeforeRender(ICoreClientAPI capi, ItemStack itemstack, EnumItemRenderTarget target, ref ItemRenderInfo renderinfo)
		{
			string fish = itemstack.Attributes.GetString("fish", "none");
			if (fish != itemstack.TempAttributes.GetString("lastRenderedFish", "none"))
			{
				itemstack.TempAttributes.RemoveAttribute("meshRefId");
				itemstack.TempAttributes.SetString("lastRenderedFish", fish);
			}
			int meshrefid = itemstack.TempAttributes.GetInt("meshRefId", 0);
			if (meshrefid == 0 || !this.meshrefs.TryGetValue(meshrefid, ref renderinfo.ModelRef))
			{
				int id = this.meshrefs.Count + 1;
				MultiTextureMeshRef modelref = capi.Render.UploadMultiTextureMesh(this.GenMesh(itemstack, capi.ItemTextureAtlas));
				renderinfo.ModelRef = (this.meshrefs[id] = modelref);
				itemstack.TempAttributes.SetInt("meshRefId", id);
			}
			base.OnBeforeRender(capi, itemstack, target, ref renderinfo);
		}

		// Token: 0x060002E4 RID: 740 RVA: 0x000220EC File Offset: 0x000202EC
		public MeshData GenMesh(ItemStack itemstack, ITextureAtlasAPI targetAtlas)
		{
			ContainedTextureSource cnts = this.genTextureSource(itemstack, targetAtlas);
			if (cnts == null)
			{
				return new MeshData(true);
			}
			MeshData fullMesh;
			this.capi.Tesselator.TesselateItem(this, ref fullMesh, cnts);
			string fishType = itemstack.Attributes.GetString("fish", "none");
			if (fishType != "none")
			{
				Item fishItem;
				if (!this.fishItemCache.TryGetValue(fishType, ref fishItem))
				{
					fishItem = Enumerable.FirstOrDefault<Item>(this.capi.World.Items, (Item i) => i.Code != null && i.Code.Path.Contains(fishType + "-raw"));
					if (fishItem != null)
					{
						this.fishItemCache[fishType] = fishItem;
					}
				}
				if (fishItem != null)
				{
					ContainedTextureSource fishTexSource = new ContainedTextureSource(this.capi, targetAtlas, new Dictionary<string, AssetLocation>(), "fish-on-spear");
					foreach (KeyValuePair<string, AssetLocation> tex in this.capi.TesselatorManager.GetCachedShape(fishItem.Shape.Base).Textures)
					{
						fishTexSource.Textures[tex.Key] = tex.Value;
					}
					MeshData fishMesh;
					this.capi.Tesselator.TesselateItem(fishItem, ref fishMesh, fishTexSource);
					if (fishMesh != null)
					{
						float[] modelMat = Mat4f.Create();
						Mat4f.Translate(modelMat, modelMat, 0.8f, 0.6f, 0.65f);
						Mat4f.RotateX(modelMat, modelMat, -1.5708f);
						Mat4f.RotateY(modelMat, modelMat, 1.5708f);
						fishMesh.MatrixTransform(modelMat);
						fullMesh.AddMeshData(fishMesh);
					}
				}
			}
			return fullMesh;
		}

		// Token: 0x060002E5 RID: 741 RVA: 0x000222A0 File Offset: 0x000204A0
		private ContainedTextureSource genTextureSource(ItemStack itemstack, ITextureAtlasAPI targetAtlas)
		{
			ContainedTextureSource cnts = new ContainedTextureSource(this.api as ICoreClientAPI, targetAtlas, new Dictionary<string, AssetLocation>(), string.Format("For render in spear {0}", this.Code));
			cnts.Textures.Clear();
			Dictionary<string, string> attributes = this.getAttributesFromItemstack(itemstack);
			foreach (KeyValuePair<string, AssetLocation> ctex in this.capi.TesselatorManager.GetCachedShape(this.Shape.Base).Textures)
			{
				cnts.Textures[ctex.Key] = ctex.Value;
			}
			if (attributes.Count == 0)
			{
				return cnts;
			}
			Dictionary<string, Dictionary<string, string>> texturesDict = this.Attributes["textures"].AsObject<Dictionary<string, Dictionary<string, string>>>(null);
			Dictionary<string, string> resolvedTextures;
			if (texturesDict != null && texturesDict.TryGetValue("fishingspear", ref resolvedTextures))
			{
				foreach (KeyValuePair<string, string> textureEntry in resolvedTextures)
				{
					string textureKey = textureEntry.Key;
					string texturePath = textureEntry.Value;
					foreach (KeyValuePair<string, string> attr in attributes)
					{
						texturePath = texturePath.Replace("{" + attr.Key + "}", attr.Value);
					}
					cnts.Textures[textureKey] = new AssetLocation(texturePath);
				}
			}
			return cnts;
		}

		// Token: 0x060002E6 RID: 742 RVA: 0x00022458 File Offset: 0x00020658
		public override string GetHeldItemName(ItemStack itemStack)
		{
			Dictionary<string, string> attributes = this.getAttributesFromItemstack(itemStack);
			if (attributes.Count == 0)
			{
				return base.GetHeldItemName(itemStack);
			}
			return Lang.GetMatchingIfExists(string.Concat(new string[]
			{
				this.Code.Domain,
				":item-",
				this.Code.Path,
				"-",
				string.Join("-", attributes.Values)
			}), Array.Empty<object>()) ?? base.GetHeldItemName(itemStack);
		}

		// Token: 0x060002E7 RID: 743 RVA: 0x000224DC File Offset: 0x000206DC
		private Dictionary<string, string> getAttributesFromItemstack(ItemStack itemStack)
		{
			Dictionary<string, string> attributes = new Dictionary<string, string>();
			foreach (string key in this.variantGroups.Keys)
			{
				string val = itemStack.Attributes.GetAsString(key, null);
				if (val != null)
				{
					attributes.Add(key, val);
				}
			}
			attributes.Add("material", "wood");
			return attributes;
		}

		// Token: 0x060002E8 RID: 744 RVA: 0x00022558 File Offset: 0x00020758
		public MeshData GenMesh(ItemSlot slot, ITextureAtlasAPI targetAtlas, BlockPos atBlockPos)
		{
			return this.GenMesh(slot.Itemstack, targetAtlas);
		}

		// Token: 0x060002E9 RID: 745 RVA: 0x00022568 File Offset: 0x00020768
		public string GetMeshCacheKey(ItemSlot slot)
		{
			return this.Code.ToShortString() + "-" + string.Join("-", Enumerable.Where<string>(Enumerable.Select<string, string>(this.variantGroups.Keys, (string code) => slot.Itemstack.Attributes.GetString(code, null)), (string code) => code != null));
		}

		// Token: 0x060002EA RID: 746 RVA: 0x000225E4 File Offset: 0x000207E4
		public AssetLocation GetCodeForHandbookGrouping(ItemStack stack)
		{
			Dictionary<string, string> attributes = this.getAttributesFromItemstack(stack);
			if (attributes.Count == 0)
			{
				return this.Code;
			}
			return AssetLocation.Create(this.Code.Path + "-" + string.Join("-", attributes.Values), this.Code.Domain);
		}

		// Token: 0x060002EB RID: 747 RVA: 0x00022640 File Offset: 0x00020840
		public string GetWildcardForHandbookGrouping(string wildcard, ItemStack stack)
		{
			foreach (KeyValuePair<string, string[]> variant in this.variantGroups)
			{
				wildcard = wildcard.Replace("{" + variant.Key + "}", stack.Attributes.GetAsString(variant.Key, null));
			}
			return wildcard;
		}

		// Token: 0x0400012E RID: 302
		public const string SpearMeshRefsKey = "spearmeshrefs";

		// Token: 0x0400012F RID: 303
		private ICoreClientAPI capi;

		// Token: 0x04000130 RID: 304
		private Dictionary<string, Item> fishItemCache = new Dictionary<string, Item>();

		// Token: 0x04000131 RID: 305
		private OrderedDictionary<string, string[]> variantGroups;
	}
}
