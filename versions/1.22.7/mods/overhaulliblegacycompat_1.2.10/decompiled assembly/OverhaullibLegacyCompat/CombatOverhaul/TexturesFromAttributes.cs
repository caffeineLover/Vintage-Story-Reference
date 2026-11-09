using System;
using System.Collections.Generic;
using System.Linq;
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
	// Token: 0x0200001E RID: 30
	[NullableContext(1)]
	[Nullable(0)]
	public class TexturesFromAttributes : CollectibleBehavior, IContainedMeshSource, IHandBookPageCodeProvider
	{
		// Token: 0x1700009F RID: 159
		// (get) Token: 0x0600020F RID: 527 RVA: 0x000087C0 File Offset: 0x000069C0
		private Dictionary<int, MultiTextureMeshRef> Meshrefs
		{
			get
			{
				return ObjectCacheUtil.GetOrCreate<Dictionary<int, MultiTextureMeshRef>>(this._api, "CombatOverhaul:TexturesFromAttributesMeshrefs", () => new Dictionary<int, MultiTextureMeshRef>());
			}
		}

		// Token: 0x06000210 RID: 528 RVA: 0x000087F1 File Offset: 0x000069F1
		public TexturesFromAttributes(CollectibleObject collObj) : base(collObj)
		{
			Item item = collObj as Item;
			if (item == null)
			{
				throw new Exception("Only for items");
			}
			this._item = item;
		}

		// Token: 0x06000211 RID: 529 RVA: 0x00008815 File Offset: 0x00006A15
		public override void OnLoaded(ICoreAPI api)
		{
			base.OnLoaded(api);
			this._api = api;
			this._clientAPI = (api as ICoreClientAPI);
			this.AddAllTypesToCreativeInventory();
		}

		// Token: 0x06000212 RID: 530 RVA: 0x00008837 File Offset: 0x00006A37
		public override void OnUnloaded(ICoreAPI api)
		{
			TexturesFromAttributes.DisposeMeshrefs(api);
			base.OnUnloaded(api);
		}

		// Token: 0x06000213 RID: 531 RVA: 0x00008846 File Offset: 0x00006A46
		public override void Initialize(JsonObject properties)
		{
			base.Initialize(properties);
			this._properties = properties.AsObject<TexturesFromAttributesProperties>(null);
		}

		// Token: 0x170000A0 RID: 160
		// (get) Token: 0x06000214 RID: 532 RVA: 0x0000885C File Offset: 0x00006A5C
		public string[] TextureAttributes
		{
			get
			{
				return Enumerable.ToArray<string>(Enumerable.Distinct<string>(Enumerable.Where<string>(Enumerable.Select<TextureConfig, string>(this.GetAllTextureConfigs(), (TextureConfig texture) => texture.Attribute), (string attribute) => !string.IsNullOrWhiteSpace(attribute)), StringComparer.Ordinal));
			}
		}

		// Token: 0x06000215 RID: 533 RVA: 0x000088C6 File Offset: 0x00006AC6
		public string HandbookPageCodeForStack(IWorldAccessor world, ItemStack stack)
		{
			return TextureAttributeHandbook.PageCodeForStack(stack, this.TextureAttributes);
		}

		// Token: 0x06000216 RID: 534 RVA: 0x000088D4 File Offset: 0x00006AD4
		public override void OnBeforeRender(ICoreClientAPI capi, ItemStack itemstack, EnumItemRenderTarget target, ref ItemRenderInfo renderinfo)
		{
			if (itemstack.TempAttributes.GetInt("CombatOverhaul:TexturesFromAttributesMeshUploadFailed", 0) != 0)
			{
				return;
			}
			int meshrefId = itemstack.TempAttributes.GetInt("meshRefId", 0);
			if (meshrefId == 0 || !this.Meshrefs.TryGetValue(meshrefId, ref renderinfo.ModelRef))
			{
				int id = this.Meshrefs.Count + 1;
				this.TryUploadMeshRef(capi, itemstack, this.GenMesh(itemstack, capi.ItemTextureAtlas, null), id, ref renderinfo);
			}
		}

		// Token: 0x06000217 RID: 535 RVA: 0x00008948 File Offset: 0x00006B48
		public MultiTextureMeshRef GetMeshRef(ICoreClientAPI capi, ItemStack itemstack, EnumItemRenderTarget target, ref ItemRenderInfo renderinfo, Shape shape)
		{
			if (itemstack.TempAttributes.GetInt("CombatOverhaul:TexturesFromAttributesMeshUploadFailed", 0) != 0)
			{
				return renderinfo.ModelRef;
			}
			int meshrefId = itemstack.TempAttributes.GetInt("meshRefId", 0);
			if (meshrefId == 0 || !this.Meshrefs.TryGetValue(meshrefId, ref renderinfo.ModelRef))
			{
				int id = this.Meshrefs.Count + 1;
				this.TryUploadMeshRef(capi, itemstack, this.GenMesh(itemstack, capi.ItemTextureAtlas, shape), id, ref renderinfo);
			}
			return renderinfo.ModelRef;
		}

		// Token: 0x06000218 RID: 536 RVA: 0x000089D0 File Offset: 0x00006BD0
		[return: Nullable(2)]
		public MeshData GenMesh(ItemStack itemstack, ITextureAtlasAPI targetAtlas, [Nullable(2)] Shape overrideShape = null)
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
			if (this._clientAPI == null || this._properties == null)
			{
				return new MeshData(true);
			}
			Shape shape = overrideShape ?? this._clientAPI.TesselatorManager.GetCachedShape(this._item.Shape.Base);
			if (shape == null)
			{
				return new MeshData(true);
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
			foreach (TextureConfig textureProperty in this.GetTextureConfigs(itemstack))
			{
				string texturePath = itemstack.Attributes.GetString(textureProperty.Attribute, null) ?? textureProperty.Default;
				textureSource.Textures[textureProperty.Code] = new AssetLocation(texturePath + ".png");
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

		// Token: 0x06000219 RID: 537 RVA: 0x00008C04 File Offset: 0x00006E04
		[return: Nullable(2)]
		public MeshData GenMesh(ItemStack itemstack, ITextureAtlasAPI targetAtlas, BlockPos atBlockPos)
		{
			return this.GenMesh(itemstack, targetAtlas, null);
		}

		// Token: 0x0600021A RID: 538 RVA: 0x00008C0F File Offset: 0x00006E0F
		MeshData IContainedMeshSource.GenMesh(ItemSlot inSlot, ITextureAtlasAPI targetAtlas, BlockPos atBlockPos)
		{
			return this.GenMesh(inSlot.Itemstack, targetAtlas, atBlockPos) ?? new MeshData(true);
		}

		// Token: 0x0600021B RID: 539 RVA: 0x00008C2C File Offset: 0x00006E2C
		public string GetMeshCacheKey(ItemStack itemstack)
		{
			string cacheKey = this._item.Code.ToShortString();
			foreach (TextureConfig textureProperty in this.GetTextureConfigs(itemstack))
			{
				string texturePath = itemstack.Attributes.GetString(textureProperty.Attribute, null) ?? textureProperty.Default;
				cacheKey = cacheKey + "-" + texturePath.Replace('/', '-');
			}
			return cacheKey;
		}

		// Token: 0x0600021C RID: 540 RVA: 0x00008C9A File Offset: 0x00006E9A
		string IContainedMeshSource.GetMeshCacheKey(ItemSlot inSlot)
		{
			return this.GetMeshCacheKey(inSlot.Itemstack);
		}

		// Token: 0x0600021D RID: 541 RVA: 0x00008CA8 File Offset: 0x00006EA8
		private void ConstructStackRecursively(List<JsonItemStack> stacks, string jsonAttributes, TextureConfig[] textureConfigs, int index)
		{
			if (this._properties == null)
			{
				return;
			}
			if (textureConfigs.Length <= index)
			{
				jsonAttributes += "}";
				stacks.Add(this.GenStackJson(jsonAttributes));
				return;
			}
			TextureConfig textureProperties = textureConfigs[index];
			if (textureProperties.HandbookValues.Length == 0)
			{
				this.ConstructStackRecursively(stacks, jsonAttributes, textureConfigs, index + 1);
				return;
			}
			if (jsonAttributes != "{")
			{
				jsonAttributes += ", ";
			}
			foreach (string texturePath in textureProperties.HandbookValues)
			{
				string jsonAttributesCopy = (string)jsonAttributes.Clone();
				jsonAttributesCopy = string.Concat(new string[]
				{
					jsonAttributesCopy,
					textureProperties.Attribute,
					": \"",
					texturePath,
					"\""
				});
				this.ConstructStackRecursively(stacks, jsonAttributesCopy, textureConfigs, index + 1);
			}
		}

		// Token: 0x0600021E RID: 542 RVA: 0x00008D7C File Offset: 0x00006F7C
		private void AddAllTypesToCreativeInventory()
		{
			if (this._properties == null)
			{
				return;
			}
			List<JsonItemStack> stacks = new List<JsonItemStack>();
			this.ConstructStackRecursively(stacks, "{", this.GetAllTextureConfigs(), 0);
			JsonItemStack noAttributesStack = new JsonItemStack
			{
				Code = this._item.Code,
				Type = 1
			};
			JsonItemStack jsonItemStack = noAttributesStack;
			ICoreAPI api = this._api;
			jsonItemStack.Resolve((api != null) ? api.World : null, "handle type");
			if (this._item.CreativeInventoryStacks == null)
			{
				if (this._properties.AddNoAttributesItem || stacks.Count == 0)
				{
					this._item.CreativeInventoryStacks = new CreativeTabAndStackList[]
					{
						new CreativeTabAndStackList
						{
							Stacks = stacks.ToArray(),
							Tabs = this._properties.CreativeTabs
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
				this._item.CreativeInventoryStacks = new CreativeTabAndStackList[]
				{
					new CreativeTabAndStackList
					{
						Stacks = stacks.ToArray(),
						Tabs = this._properties.CreativeTabs
					},
					new CreativeTabAndStackList
					{
						Stacks = new JsonItemStack[]
						{
							stacks[0]
						},
						Tabs = this._item.CreativeInventoryTabs
					}
				};
				this._item.CreativeInventoryTabs = null;
			}
		}

		// Token: 0x0600021F RID: 543 RVA: 0x00008EF0 File Offset: 0x000070F0
		private JsonItemStack GenStackJson(string json)
		{
			JsonItemStack jsonItemStack = new JsonItemStack();
			jsonItemStack.Code = this._item.Code;
			jsonItemStack.Type = 1;
			jsonItemStack.Attributes = new JsonObject(JToken.Parse(json));
			ICoreAPI api = this._api;
			jsonItemStack.Resolve((api != null) ? api.World : null, "textures type");
			return jsonItemStack;
		}

		// Token: 0x06000220 RID: 544 RVA: 0x00008F4C File Offset: 0x0000714C
		private TextureConfig[] GetTextureConfigs(ItemStack itemstack)
		{
			if (this._properties == null)
			{
				return Array.Empty<TextureConfig>();
			}
			List<TextureConfig> textureConfigs = new List<TextureConfig>();
			textureConfigs.AddRange(this._properties.Textures);
			CollectibleObject collectible = itemstack.Collectible;
			string text;
			if (collectible == null)
			{
				text = null;
			}
			else
			{
				AssetLocation code = collectible.Code;
				text = ((code != null) ? code.ToShortString() : null);
			}
			string stackCode = text ?? this._item.Code.ToShortString();
			bool matchedTypedConfig = false;
			foreach (KeyValuePair<string, TextureConfig[]> keyValuePair in this._properties.TexturesByType)
			{
				string text2;
				TextureConfig[] array;
				keyValuePair.Deconstruct(ref text2, ref array);
				string pattern = text2;
				TextureConfig[] typedConfigs = array;
				if (!(pattern == "*") && WildcardUtil.Match(pattern, stackCode))
				{
					textureConfigs.AddRange(typedConfigs);
					matchedTypedConfig = true;
					break;
				}
			}
			TextureConfig[] fallbackConfigs;
			if (!matchedTypedConfig && this._properties.TexturesByType.TryGetValue("*", ref fallbackConfigs))
			{
				textureConfigs.AddRange(fallbackConfigs);
			}
			return textureConfigs.ToArray();
		}

		// Token: 0x06000221 RID: 545 RVA: 0x0000905C File Offset: 0x0000725C
		private TextureConfig[] GetAllTextureConfigs()
		{
			if (this._properties == null)
			{
				return Array.Empty<TextureConfig>();
			}
			Dictionary<string, TextureConfig> textureConfigs = new Dictionary<string, TextureConfig>(StringComparer.Ordinal);
			TexturesFromAttributes.AddTextureConfigs(textureConfigs, this._properties.Textures);
			foreach (TextureConfig[] typedConfigs in this._properties.TexturesByType.Values)
			{
				TexturesFromAttributes.AddTextureConfigs(textureConfigs, typedConfigs);
			}
			return Enumerable.ToArray<TextureConfig>(textureConfigs.Values);
		}

		// Token: 0x06000222 RID: 546 RVA: 0x000090F0 File Offset: 0x000072F0
		private static void AddTextureConfigs(Dictionary<string, TextureConfig> textureConfigs, IEnumerable<TextureConfig> configs)
		{
			foreach (TextureConfig config in configs)
			{
				string key = config.Code + "\n" + config.Attribute;
				textureConfigs.TryAdd(key, config);
			}
		}

		// Token: 0x06000223 RID: 547 RVA: 0x00009154 File Offset: 0x00007354
		private bool TryUploadMeshRef(ICoreClientAPI capi, ItemStack itemstack, [Nullable(2)] MeshData mesh, int id, ref ItemRenderInfo renderinfo)
		{
			if (mesh == null)
			{
				itemstack.TempAttributes.SetInt("CombatOverhaul:TexturesFromAttributesMeshUploadFailed", 1);
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
				itemstack.TempAttributes.SetInt("CombatOverhaul:TexturesFromAttributesMeshUploadFailed", 1);
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

		// Token: 0x06000224 RID: 548 RVA: 0x00009284 File Offset: 0x00007484
		private static void DisposeMeshrefs(ICoreAPI api)
		{
			object value;
			if (api.ObjectCache.TryGetValue("CombatOverhaul:TexturesFromAttributesMeshrefs", ref value))
			{
				Dictionary<int, MultiTextureMeshRef> meshrefs = value as Dictionary<int, MultiTextureMeshRef>;
				if (meshrefs != null)
				{
					foreach (MultiTextureMeshRef multiTextureMeshRef in meshrefs.Values)
					{
						multiTextureMeshRef.Dispose();
					}
					meshrefs.Clear();
					ObjectCacheUtil.Delete(api, "CombatOverhaul:TexturesFromAttributesMeshrefs");
					return;
				}
			}
		}

		// Token: 0x040000F5 RID: 245
		private const string MeshrefsCacheKey = "CombatOverhaul:TexturesFromAttributesMeshrefs";

		// Token: 0x040000F6 RID: 246
		private const string MeshUploadFailedKey = "CombatOverhaul:TexturesFromAttributesMeshUploadFailed";

		// Token: 0x040000F7 RID: 247
		[Nullable(2)]
		private ICoreClientAPI _clientAPI;

		// Token: 0x040000F8 RID: 248
		[Nullable(2)]
		private ICoreAPI _api;

		// Token: 0x040000F9 RID: 249
		private readonly Item _item;

		// Token: 0x040000FA RID: 250
		[Nullable(2)]
		private TexturesFromAttributesProperties _properties;
	}
}
