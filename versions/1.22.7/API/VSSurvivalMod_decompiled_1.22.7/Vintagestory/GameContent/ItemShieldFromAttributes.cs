using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json.Linq;
using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.Common.Entities;
using Vintagestory.API.Config;
using Vintagestory.API.Datastructures;
using Vintagestory.API.MathTools;
using Vintagestory.API.Util;

namespace Vintagestory.GameContent;

public class ItemShieldFromAttributes : ItemShield, IContainedMeshSource, IAttachableToEntity, IHandbookGrouping
{
	public const string ShieldMeshRefsKey = "shieldmeshrefs";

	private ICoreClientAPI capi;

	private Dictionary<string, Dictionary<string, int>> durabilityGains;

	private Vintagestory.API.Datastructures.OrderedDictionary<string, string[]> variantGroups;

	private Dictionary<int, MultiTextureMeshRef> meshrefs => ObjectCacheUtil.GetOrCreate(api, "shieldmeshrefs", () => new Dictionary<int, MultiTextureMeshRef>());

	public string Construction => Variant["construction"];

	public new int RequiresBehindSlots { get; set; }

	string IAttachableToEntity.GetCategoryCode(ItemStack stack)
	{
		return AttachableToEntity?.GetCategoryCode(stack);
	}

	CompositeShape IAttachableToEntity.GetAttachedShape(ItemStack stack, string slotCode)
	{
		return AttachableToEntity?.GetAttachedShape(stack, slotCode);
	}

	string[] IAttachableToEntity.GetDisableElements(ItemStack stack)
	{
		return AttachableToEntity?.GetDisableElements(stack);
	}

	string[] IAttachableToEntity.GetKeepElements(ItemStack stack)
	{
		return AttachableToEntity?.GetKeepElements(stack);
	}

	string IAttachableToEntity.GetTexturePrefixCode(ItemStack stack)
	{
		return AttachableToEntity?.GetTexturePrefixCode(stack) + "-" + string.Join("-", from code in variantGroups.Keys
			select stack.Attributes.GetString(code) into code
			where code != null
			select code);
	}

	void IAttachableToEntity.CollectTextures(ItemStack itemstack, Shape intoShape, string texturePrefixCode, Dictionary<string, CompositeTexture> intoDict)
	{
		ContainedTextureSource cnts = genTextureSource(itemstack, null);
		if (cnts == null)
		{
			api.Logger.Warning(GetHeldItemName(itemstack) + " (" + Code.ToShortString() + "-" + string.Join("-", from code in variantGroups.Keys
				select itemstack.Attributes.GetString(code) into code
				where code != null
				select code) + ") could not create textures, may render with unknown textures");
			return;
		}
		foreach (KeyValuePair<string, AssetLocation> val in cnts.Textures)
		{
			intoShape.Textures[val.Key] = val.Value;
		}
	}

	public new bool IsAttachable(Entity toEntity, ItemStack itemStack)
	{
		return true;
	}

	public override void OnLoaded(ICoreAPI api)
	{
		base.OnLoaded(api);
		capi = api as ICoreClientAPI;
		durabilityGains = Attributes["durabilityGains"].AsObject(durabilityGains);
		variantGroups = Attributes["variantGroups"].AsObject(variantGroups);
		if (durabilityGains == null || variantGroups == null)
		{
			throw new Exception(string.Concat("Round shield ", Code, " has improperly defined durabilityGains or variantGroups"));
		}
		AddAllTypesToCreativeInventory();
	}

	public override void OnUnloaded(ICoreAPI api)
	{
		if (api.ObjectCache.ContainsKey("shieldmeshrefs") && meshrefs.Count > 0)
		{
			foreach (KeyValuePair<int, MultiTextureMeshRef> meshref in meshrefs)
			{
				meshref.Deconstruct(out var _, out var value);
				value.Dispose();
			}
			ObjectCacheUtil.Delete(api, "shieldmeshrefs");
		}
		base.OnUnloaded(api);
	}

	public override int GetMaxDurability(ItemStack itemstack)
	{
		int gain = 0;
		foreach (KeyValuePair<string, Dictionary<string, int>> val in durabilityGains)
		{
			string material = itemstack.Attributes.GetString(val.Key);
			if (material != null && val.Value.TryGetValue(material, out var matgain))
			{
				gain += matgain;
			}
		}
		return base.GetMaxDurability(itemstack) + gain;
	}

	private void AddAllTypesToCreativeInventory()
	{
		List<JsonItemStack> stacks = new List<JsonItemStack>();
		Dictionary<string, string>[] array = Attributes["allowedVariants"].AsArray<Dictionary<string, string>>();
		foreach (Dictionary<string, string> entry in array)
		{
			Dictionary<string, string[]> attributes = new Dictionary<string, string[]>();
			foreach (string key in variantGroups.Keys)
			{
				if (entry.TryGetValue(key, out var val))
				{
					attributes.Add(key, (val == "*") ? variantGroups[key].ToArray() : ((val == null) ? Array.Empty<string>() : new string[1] { val }));
				}
			}
			string[] variants = new string[1] { "" };
			foreach (string key2 in attributes.Keys)
			{
				IEnumerable<string> newVariants = variants.SelectMany((string variant) => attributes[key2].Select((string value) => variant + $" {key2}: \"{value}\","));
				if (newVariants.Any())
				{
					variants = newVariants.ToArray();
				}
			}
			variants = variants.Select((string var) => "{" + var + " }").ToArray();
			stacks.AddRange(variants.Select(genJstack));
		}
		CreativeInventoryStacks = new CreativeTabAndStackList[1]
		{
			new CreativeTabAndStackList
			{
				Stacks = stacks.ToArray(),
				Tabs = new string[3] { "general", "items", "tools" }
			}
		};
	}

	private JsonItemStack genJstack(string json)
	{
		JsonItemStack jsonItemStack = new JsonItemStack();
		jsonItemStack.Code = Code;
		jsonItemStack.Type = EnumItemClass.Item;
		jsonItemStack.Attributes = new JsonObject(JToken.Parse(json));
		jsonItemStack.Resolve(api.World, "shield type");
		return jsonItemStack;
	}

	public override void OnBeforeRender(ICoreClientAPI capi, ItemStack itemstack, EnumItemRenderTarget target, ref ItemRenderInfo renderinfo)
	{
		int meshrefid = itemstack.TempAttributes.GetInt("meshRefId");
		if (meshrefid == 0 || !meshrefs.TryGetValue(meshrefid, out renderinfo.ModelRef))
		{
			int id = meshrefs.Count + 1;
			MultiTextureMeshRef modelref = capi.Render.UploadMultiTextureMesh(GenMesh(itemstack, capi.ItemTextureAtlas));
			ItemRenderInfo obj = renderinfo;
			MultiTextureMeshRef modelRef = (meshrefs[id] = modelref);
			obj.ModelRef = modelRef;
			itemstack.TempAttributes.SetInt("meshRefId", id);
		}
		base.OnBeforeRender(capi, itemstack, target, ref renderinfo);
	}

	public MeshData GenMesh(ItemStack itemstack, ITextureAtlasAPI targetAtlas)
	{
		ContainedTextureSource cnts = genTextureSource(itemstack, targetAtlas);
		if (cnts == null)
		{
			return new MeshData();
		}
		capi.Tesselator.TesselateItem(this, out var mesh, cnts);
		return mesh;
	}

	private ContainedTextureSource genTextureSource(ItemStack itemstack, ITextureAtlasAPI targetAtlas)
	{
		ContainedTextureSource cnts = new ContainedTextureSource(api as ICoreClientAPI, targetAtlas, new Dictionary<string, AssetLocation>(), $"For render in shield {Code}");
		cnts.Textures.Clear();
		Dictionary<string, string> attributes = getAttributesFromItemstack(itemstack);
		Shape shape = capi.TesselatorManager.GetCachedShape(Shape.Base);
		foreach (KeyValuePair<string, AssetLocation> ctex in shape.Textures)
		{
			cnts.Textures[ctex.Key] = ctex.Value;
		}
		if (attributes.Count == 0)
		{
			return cnts;
		}
		Dictionary<string, Dictionary<string, string>>? dictionary = Attributes["textures"].AsObject<Dictionary<string, Dictionary<string, string>>>();
		string resolvedKey = "";
		Dictionary<string, string> resolvedTextures = new Dictionary<string, string>();
		bool match = false;
		foreach (KeyValuePair<string, Dictionary<string, string>> variant in dictionary)
		{
			string currentVariant = "";
			resolvedKey = variant.Key;
			resolvedTextures = variant.Value;
			foreach (KeyValuePair<string, string> entry in attributes)
			{
				currentVariant = currentVariant + ((currentVariant == "") ? "" : "-") + entry.Value;
				resolvedKey = resolvedKey.Replace("{" + entry.Key + "}", entry.Value);
				foreach (string texture in variant.Value.Keys)
				{
					resolvedTextures[texture] = resolvedTextures[texture].Replace("{" + entry.Key + "}", entry.Value);
				}
			}
			if (currentVariant == resolvedKey)
			{
				match = true;
				break;
			}
		}
		if (!match)
		{
			return cnts;
		}
		if (resolvedTextures.Values.Any((string val) => val.Contains('{') || val.Contains('}')))
		{
			throw new Exception(string.Concat("Round shield ", Code, " has improperly defined textures in attributes"));
		}
		foreach (KeyValuePair<string, AssetLocation> ctex2 in shape.Textures)
		{
			if (resolvedTextures.ContainsKey(ctex2.Key))
			{
				cnts.Textures[ctex2.Key] = resolvedTextures[ctex2.Key];
			}
		}
		return cnts;
	}

	public override string GetHeldItemName(ItemStack itemStack)
	{
		Dictionary<string, string> attributes = getAttributesFromItemstack(itemStack);
		if (attributes.Count == 0)
		{
			return base.GetHeldItemName(itemStack);
		}
		return Lang.GetMatchingIfExists(Code.Domain + ":item-" + Code.Path + "-" + string.Join("-", attributes.Values)) ?? base.GetHeldItemName(itemStack);
	}

	public override void GetHeldItemInfo(ItemSlot inSlot, StringBuilder dsc, IWorldAccessor world, bool withDebugInfo)
	{
		base.GetHeldItemInfo(inSlot, dsc, world, withDebugInfo);
		Dictionary<string, string> attributes = getAttributesFromItemstack(inSlot.Itemstack);
		if (attributes.Count != 0)
		{
			string variant = Code.Domain + ":itemdesc-" + Code.Path;
			variant = Lang.GetMatchingIfExists(variant + "-" + string.Join("-", attributes.Values));
			if (variant != null)
			{
				dsc.AppendLine(variant);
			}
		}
	}

	private Dictionary<string, string> getAttributesFromItemstack(ItemStack itemStack)
	{
		Dictionary<string, string> attributes = new Dictionary<string, string>();
		foreach (string key in variantGroups.Keys)
		{
			string val = itemStack.Attributes.GetAsString(key);
			if (val != null)
			{
				attributes.Add(key, val);
			}
		}
		bool flag = Code.Domain == "game";
		if (flag)
		{
			string construction = Construction;
			bool flag2 = ((construction == "woodmetal" || construction == "woodmetalleather") ? true : false);
			flag = flag2;
		}
		if (flag && (!attributes.TryGetValue("wood", out var wood) || wood == ""))
		{
			attributes.Add("wood", "generic");
		}
		return attributes;
	}

	public MeshData GenMesh(ItemSlot slot, ITextureAtlasAPI targetAtlas, BlockPos atBlockPos)
	{
		return GenMesh(slot.Itemstack, targetAtlas);
	}

	public string GetMeshCacheKey(ItemSlot slot)
	{
		return Code.ToShortString() + "-" + string.Join("-", from code in variantGroups.Keys
			select slot.Itemstack.Attributes.GetString(code) into code
			where code != null
			select code);
	}

	public AssetLocation GetCodeForHandbookGrouping(ItemStack stack)
	{
		Dictionary<string, string> attributes = getAttributesFromItemstack(stack);
		if (attributes.Count == 0)
		{
			return Code;
		}
		return AssetLocation.Create(Code.Path + "-" + string.Join("-", attributes.Values), Code.Domain);
	}

	public string GetWildcardForHandbookGrouping(string wildcard, ItemStack stack)
	{
		foreach (KeyValuePair<string, string[]> variant in variantGroups)
		{
			wildcard = wildcard.Replace("{" + variant.Key + "}", stack.Attributes.GetAsString(variant.Key));
		}
		return wildcard;
	}
}
