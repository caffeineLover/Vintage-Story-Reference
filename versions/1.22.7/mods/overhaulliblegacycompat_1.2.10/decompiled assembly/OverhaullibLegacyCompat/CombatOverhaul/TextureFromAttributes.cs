using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using CombatOverhaul.Utils;
using Newtonsoft.Json.Linq;
using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.Datastructures;
using Vintagestory.API.MathTools;
using Vintagestory.API.Util;
using Vintagestory.GameContent;

namespace CombatOverhaul
{
	// Token: 0x0200001B RID: 27
	[NullableContext(1)]
	[Nullable(0)]
	public class TextureFromAttributes : CollectibleBehavior, IContainedMeshSource, IHandBookPageCodeProvider
	{
		// Token: 0x17000095 RID: 149
		// (get) Token: 0x060001EC RID: 492 RVA: 0x00007EDD File Offset: 0x000060DD
		private Dictionary<int, MultiTextureMeshRef> Meshrefs
		{
			get
			{
				return ObjectCacheUtil.GetOrCreate<Dictionary<int, MultiTextureMeshRef>>(this._api, "CombatOverhaul:TextureFromAttributesMeshrefs", () => new Dictionary<int, MultiTextureMeshRef>());
			}
		}

		// Token: 0x060001ED RID: 493 RVA: 0x00007F10 File Offset: 0x00006110
		public TextureFromAttributes(CollectibleObject collObj) : base(collObj)
		{
			Item item = collObj as Item;
			if (item == null)
			{
				throw new Exception("Only for items");
			}
			this._item = item;
		}

		// Token: 0x060001EE RID: 494 RVA: 0x00007F76 File Offset: 0x00006176
		public override void OnLoaded(ICoreAPI api)
		{
			base.OnLoaded(api);
			this._api = api;
			this._clientAPI = (api as ICoreClientAPI);
			this.AddAllTypesToCreativeInventory();
		}

		// Token: 0x060001EF RID: 495 RVA: 0x00007F98 File Offset: 0x00006198
		public override void OnUnloaded(ICoreAPI api)
		{
			TextureFromAttributes.DisposeMeshrefs(api);
			base.OnUnloaded(api);
		}

		// Token: 0x060001F0 RID: 496 RVA: 0x00007FA8 File Offset: 0x000061A8
		public override void Initialize(JsonObject properties)
		{
			base.Initialize(properties);
			this._materialTypes = properties["textureTypes"].AsObject<List<string>>(null);
			this._textureCode = properties["textureCode"].AsString(null);
			this._textureAttribute = properties["textureAttribute"].AsString(null);
			this._defaultTexture = properties["defaultTexture"].AsString(null);
			this._creativeTabs = properties["creativeTabs"].AsObject<string[]>(null);
		}

		// Token: 0x17000096 RID: 150
		// (get) Token: 0x060001F1 RID: 497 RVA: 0x0000802F File Offset: 0x0000622F
		public string[] TextureAttributes
		{
			get
			{
				if (!string.IsNullOrWhiteSpace(this._textureAttribute))
				{
					return new string[]
					{
						this._textureAttribute
					};
				}
				return Array.Empty<string>();
			}
		}

		// Token: 0x060001F2 RID: 498 RVA: 0x00008053 File Offset: 0x00006253
		public string HandbookPageCodeForStack(IWorldAccessor world, ItemStack stack)
		{
			return TextureAttributeHandbook.PageCodeForStack(stack, this.TextureAttributes);
		}

		// Token: 0x060001F3 RID: 499 RVA: 0x00008064 File Offset: 0x00006264
		public override void OnBeforeRender(ICoreClientAPI capi, ItemStack itemstack, EnumItemRenderTarget target, ref ItemRenderInfo renderinfo)
		{
			if (itemstack.TempAttributes.GetInt("CombatOverhaul:TextureFromAttributesMeshUploadFailed", 0) != 0)
			{
				return;
			}
			int meshrefId = itemstack.TempAttributes.GetInt("meshRefId", 0);
			if (meshrefId == 0 || !this.Meshrefs.TryGetValue(meshrefId, ref renderinfo.ModelRef))
			{
				int id = this.Meshrefs.Count + 1;
				this.TryUploadMeshRef(capi, itemstack, this.GenMesh(itemstack, capi.ItemTextureAtlas), id, ref renderinfo);
			}
		}

		// Token: 0x060001F4 RID: 500 RVA: 0x000080D8 File Offset: 0x000062D8
		[return: Nullable(2)]
		public MeshData GenMesh(ItemStack itemstack, ITextureAtlasAPI targetAtlas)
		{
			ICoreClientAPI coreClientAPI = this._api as ICoreClientAPI;
			Dictionary<string, AssetLocation> dictionary = new Dictionary<string, AssetLocation>();
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
			defaultInterpolatedStringHandler..ctor(16, 1);
			defaultInterpolatedStringHandler.AppendLiteral("For render in '");
			defaultInterpolatedStringHandler.AppendFormatted<AssetLocation>(this._item.Code);
			defaultInterpolatedStringHandler.AppendLiteral("'");
			ContainedTextureSource textureSource = new ContainedTextureSource(coreClientAPI, targetAtlas, dictionary, defaultInterpolatedStringHandler.ToStringAndClear());
			textureSource.Textures.Clear();
			string textureName = itemstack.Attributes.GetString(this._textureAttribute, null);
			if (this._clientAPI == null)
			{
				return null;
			}
			if (textureName == null)
			{
				textureName = "";
			}
			textureSource.Textures[this._textureCode] = new AssetLocation(this._defaultTexture);
			Shape shape = this._clientAPI.TesselatorManager.GetCachedShape(this._item.Shape.Base);
			if (shape == null)
			{
				return null;
			}
			foreach (KeyValuePair<string, AssetLocation> keyValuePair in shape.Textures)
			{
				string text;
				AssetLocation assetLocation;
				keyValuePair.Deconstruct(ref text, ref assetLocation);
				string textureCode = text;
				AssetLocation textureLocation = assetLocation;
				CompositeTexture texture;
				if (this._item.Textures.TryGetValue(textureCode, ref texture))
				{
					textureSource.Textures[textureCode] = texture.Base;
				}
				else
				{
					textureSource.Textures[textureCode] = textureLocation;
				}
			}
			if (textureName != "")
			{
				textureSource.Textures[this._textureCode] = new AssetLocation(textureName + ".png");
			}
			MeshData result;
			try
			{
				MeshData mesh;
				this._clientAPI.Tesselator.TesselateItem(this._item, ref mesh, textureSource);
				result = mesh;
			}
			catch (Exception exception)
			{
				ICoreAPI api = this._api;
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler2;
				defaultInterpolatedStringHandler2..ctor(35, 2);
				defaultInterpolatedStringHandler2.AppendLiteral("Error on tesselating shape for '");
				CollectibleObject collectible = itemstack.Collectible;
				defaultInterpolatedStringHandler2.AppendFormatted<AssetLocation>((collectible != null) ? collectible.Code : null);
				defaultInterpolatedStringHandler2.AppendLiteral("':\n");
				defaultInterpolatedStringHandler2.AppendFormatted<Exception>(exception);
				LoggerUtil.Warn(api, this, defaultInterpolatedStringHandler2.ToStringAndClear());
				result = null;
			}
			return result;
		}

		// Token: 0x060001F5 RID: 501 RVA: 0x000082F4 File Offset: 0x000064F4
		[return: Nullable(2)]
		public MeshData GenMesh(ItemStack itemstack, ITextureAtlasAPI targetAtlas, BlockPos atBlockPos)
		{
			return this.GenMesh(itemstack, targetAtlas);
		}

		// Token: 0x060001F6 RID: 502 RVA: 0x000082FE File Offset: 0x000064FE
		MeshData IContainedMeshSource.GenMesh(ItemSlot inSlot, ITextureAtlasAPI targetAtlas, BlockPos atBlockPos)
		{
			return this.GenMesh(inSlot.Itemstack, targetAtlas, atBlockPos) ?? new MeshData(true);
		}

		// Token: 0x060001F7 RID: 503 RVA: 0x00008318 File Offset: 0x00006518
		public string GetMeshCacheKey(ItemStack itemstack)
		{
			string wood = itemstack.Attributes.GetString(this._textureAttribute, null).Replace('/', '-');
			return this._item.Code.ToShortString() + "-" + wood;
		}

		// Token: 0x060001F8 RID: 504 RVA: 0x0000835C File Offset: 0x0000655C
		string IContainedMeshSource.GetMeshCacheKey(ItemSlot inSlot)
		{
			return this.GetMeshCacheKey(inSlot.Itemstack);
		}

		// Token: 0x060001F9 RID: 505 RVA: 0x0000836C File Offset: 0x0000656C
		private void AddAllTypesToCreativeInventory()
		{
			List<JsonItemStack> stacks = new List<JsonItemStack>();
			foreach (string material in this._materialTypes)
			{
				stacks.Add(this.GenStackJson(string.Format("{{ {1}: \"{0}\" }}", material, this._textureAttribute)));
			}
			JsonItemStack noAttributesStack = new JsonItemStack
			{
				Code = this._item.Code,
				Type = 1
			};
			noAttributesStack.Resolve(this._api.World, "handle type");
			if (this._item.CreativeInventoryStacks == null)
			{
				this._item.CreativeInventoryStacks = new CreativeTabAndStackList[]
				{
					new CreativeTabAndStackList
					{
						Stacks = stacks.ToArray(),
						Tabs = this._creativeTabs
					},
					new CreativeTabAndStackList
					{
						Stacks = new JsonItemStack[]
						{
							noAttributesStack
						},
						Tabs = this._item.CreativeInventoryTabs
					}
				};
				this._item.CreativeInventoryTabs = null;
				return;
			}
			this._item.CreativeInventoryStacks = ArrayExtensions.Append<CreativeTabAndStackList>(this._item.CreativeInventoryStacks, new CreativeTabAndStackList
			{
				Stacks = stacks.ToArray(),
				Tabs = this._creativeTabs
			});
		}

		// Token: 0x060001FA RID: 506 RVA: 0x000084C4 File Offset: 0x000066C4
		private JsonItemStack GenStackJson(string json)
		{
			JsonItemStack jsonItemStack = new JsonItemStack();
			jsonItemStack.Code = this._item.Code;
			jsonItemStack.Type = 1;
			jsonItemStack.Attributes = new JsonObject(JToken.Parse(json));
			ICoreAPI api = this._api;
			jsonItemStack.Resolve((api != null) ? api.World : null, "handle type");
			return jsonItemStack;
		}

		// Token: 0x060001FB RID: 507 RVA: 0x00008520 File Offset: 0x00006720
		private bool TryUploadMeshRef(ICoreClientAPI capi, ItemStack itemstack, [Nullable(2)] MeshData mesh, int id, ref ItemRenderInfo renderinfo)
		{
			if (mesh == null)
			{
				itemstack.TempAttributes.SetInt("CombatOverhaul:TextureFromAttributesMeshUploadFailed", 1);
				ICoreAPI api = this._api;
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
				defaultInterpolatedStringHandler..ctor(67, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Skipping texture-attributed render for '");
				CollectibleObject collectible = itemstack.Collectible;
				defaultInterpolatedStringHandler.AppendFormatted<AssetLocation>((collectible != null) ? collectible.Code : null);
				defaultInterpolatedStringHandler.AppendLiteral("': generated mesh was null.");
				LoggerUtil.Warn(api, this, defaultInterpolatedStringHandler.ToStringAndClear());
				return false;
			}
			bool result;
			try
			{
				MultiTextureMeshRef modelref = capi.Render.UploadMultiTextureMesh(mesh);
				renderinfo.ModelRef = (this.Meshrefs[id] = modelref);
				itemstack.TempAttributes.SetInt("meshRefId", id);
				result = true;
			}
			catch (Exception exception)
			{
				itemstack.TempAttributes.SetInt("CombatOverhaul:TextureFromAttributesMeshUploadFailed", 1);
				ICoreAPI api2 = this._api;
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler2;
				defaultInterpolatedStringHandler2..ctor(48, 2);
				defaultInterpolatedStringHandler2.AppendLiteral("Error uploading texture-attributed mesh for '");
				CollectibleObject collectible2 = itemstack.Collectible;
				defaultInterpolatedStringHandler2.AppendFormatted<AssetLocation>((collectible2 != null) ? collectible2.Code : null);
				defaultInterpolatedStringHandler2.AppendLiteral("':\n");
				defaultInterpolatedStringHandler2.AppendFormatted<Exception>(exception);
				LoggerUtil.Warn(api2, this, defaultInterpolatedStringHandler2.ToStringAndClear());
				result = false;
			}
			return result;
		}

		// Token: 0x060001FC RID: 508 RVA: 0x00008650 File Offset: 0x00006850
		private static void DisposeMeshrefs(ICoreAPI api)
		{
			object value;
			if (api.ObjectCache.TryGetValue("CombatOverhaul:TextureFromAttributesMeshrefs", ref value))
			{
				Dictionary<int, MultiTextureMeshRef> meshrefs = value as Dictionary<int, MultiTextureMeshRef>;
				if (meshrefs != null)
				{
					foreach (MultiTextureMeshRef multiTextureMeshRef in meshrefs.Values)
					{
						multiTextureMeshRef.Dispose();
					}
					meshrefs.Clear();
					ObjectCacheUtil.Delete(api, "CombatOverhaul:TextureFromAttributesMeshrefs");
					return;
				}
			}
		}

		// Token: 0x040000E3 RID: 227
		private const string MeshrefsCacheKey = "CombatOverhaul:TextureFromAttributesMeshrefs";

		// Token: 0x040000E4 RID: 228
		private const string MeshUploadFailedKey = "CombatOverhaul:TextureFromAttributesMeshUploadFailed";

		// Token: 0x040000E5 RID: 229
		[Nullable(2)]
		private ICoreClientAPI _clientAPI;

		// Token: 0x040000E6 RID: 230
		[Nullable(2)]
		private ICoreAPI _api;

		// Token: 0x040000E7 RID: 231
		private readonly Item _item;

		// Token: 0x040000E8 RID: 232
		private List<string> _materialTypes = new List<string>();

		// Token: 0x040000E9 RID: 233
		private string _textureCode = string.Empty;

		// Token: 0x040000EA RID: 234
		private string _textureAttribute = string.Empty;

		// Token: 0x040000EB RID: 235
		private string _defaultTexture = string.Empty;

		// Token: 0x040000EC RID: 236
		private string[] _creativeTabs = Array.Empty<string>();
	}
}
