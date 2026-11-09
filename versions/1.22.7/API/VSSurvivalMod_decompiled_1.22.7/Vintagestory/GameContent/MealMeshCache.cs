using System;
using System.Collections.Generic;
using System.Linq;
using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.Config;
using Vintagestory.API.MathTools;
using Vintagestory.API.Util;

namespace Vintagestory.GameContent;

public class MealMeshCache : ModSystem, ITexPositionSource
{
	private ICoreClientAPI? capi;

	private Block? mealtextureSourceBlock;

	private AssetLocation[] pieShapeLocByFillLevel = new AssetLocation[5]
	{
		new AssetLocation("block/food/pie/full-fill0"),
		new AssetLocation("block/food/pie/full-fill1"),
		new AssetLocation("block/food/pie/full-fill2"),
		new AssetLocation("block/food/pie/full-fill3"),
		new AssetLocation("block/food/pie/full-fill4")
	};

	private AssetLocation[] pieShapeBySize = new AssetLocation[4]
	{
		new AssetLocation("block/food/pie/quarter"),
		new AssetLocation("block/food/pie/half"),
		new AssetLocation("block/food/pie/threefourths"),
		new AssetLocation("block/food/pie/full")
	};

	protected Shape? nowTesselatingShape;

	private BlockPie? nowTesselatingBlock;

	private ItemStack[]? contentStacks;

	private AssetLocation? crustTextureLoc;

	private AssetLocation? fillingTextureLoc;

	private AssetLocation? topCrustTextureLoc;

	public Dictionary<int, AssetLocation> pieMixingCodeFillingTextures = new Dictionary<int, AssetLocation>();

	public AssetLocation[] pieMixedCategoryFillingTextures = new AssetLocation[6]
	{
		new AssetLocation("block/food/pie/fill-mixedfruit"),
		new AssetLocation("block/food/pie/fill-mixedvegetable"),
		new AssetLocation("block/food/pie/fill-mixedmeat"),
		new AssetLocation("block/food/pie/fill-mixedgrain"),
		new AssetLocation("block/food/pie/fill-mixedcheese"),
		new AssetLocation("block/food/pie/fill-unknown")
	};

	public Size2i AtlasSize => capi.BlockTextureAtlas.Size;

	public TextureAtlasPosition this[string textureCode]
	{
		get
		{
			AssetLocation texturePath = crustTextureLoc;
			if (textureCode == "filling")
			{
				texturePath = fillingTextureLoc;
			}
			if (textureCode == "topcrust")
			{
				texturePath = topCrustTextureLoc;
			}
			if (texturePath == null)
			{
				capi.World.Logger.Warning("Missing texture path for pie mesh texture code {0}, seems like a missing texture definition or invalid pie block.", textureCode);
				return capi.BlockTextureAtlas.UnknownTexturePosition;
			}
			TextureAtlasPosition texpos = capi.BlockTextureAtlas[texturePath];
			if (texpos == null)
			{
				IAsset texAsset = capi.Assets.TryGet(texturePath.Clone().WithPathPrefixOnce("textures/").WithPathAppendixOnce(".png"));
				if (texAsset != null)
				{
					BitmapRef bmp = texAsset.ToBitmap(capi);
					capi.BlockTextureAtlas.GetOrInsertTexture(texturePath, out var _, out texpos, () => bmp);
				}
				else
				{
					capi.World.Logger.Warning("Pie mesh texture {1} not found.", nowTesselatingBlock?.Code, texturePath);
					texpos = capi.BlockTextureAtlas.UnknownTexturePosition;
				}
			}
			return texpos;
		}
	}

	public override bool ShouldLoad(EnumAppSide forSide)
	{
		return forSide == EnumAppSide.Client;
	}

	public override void StartClientSide(ICoreClientAPI api)
	{
		base.StartClientSide(api);
		capi = api;
		api.Event.LeaveWorld += Event_LeaveWorld;
		api.Event.BlockTexturesLoaded += Event_BlockTexturesLoaded;
	}

	private void Event_BlockTexturesLoaded()
	{
		mealtextureSourceBlock = capi.World.GetBlock(new AssetLocation("claypot-blue-cooked"));
	}

	public override void Dispose()
	{
		ICoreClientAPI? coreClientAPI = capi;
		if (coreClientAPI == null || !coreClientAPI.ObjectCache.TryGetValue("pieMeshRefs", out var objPi) || !(objPi is Dictionary<int, MultiTextureMeshRef> meshRefs))
		{
			return;
		}
		foreach (KeyValuePair<int, MultiTextureMeshRef> item in meshRefs)
		{
			item.Deconstruct(out var _, out var value);
			value.Dispose();
		}
		capi.ObjectCache.Remove("pieMeshRefs");
	}

	public MultiTextureMeshRef? GetOrCreatePieMeshRef(ItemStack? pieStack)
	{
		object obj;
		Dictionary<int, MultiTextureMeshRef> meshrefs = (Dictionary<int, MultiTextureMeshRef>)(capi.ObjectCache.TryGetValue("pieMeshRefs", out obj) ? ((obj as Dictionary<int, MultiTextureMeshRef>) ?? new Dictionary<int, MultiTextureMeshRef>()) : (capi.ObjectCache["pieMeshRefs"] = new Dictionary<int, MultiTextureMeshRef>()));
		if (!(pieStack?.Block is BlockPie pieBlock))
		{
			return null;
		}
		ItemStack[] contentStacks = pieBlock.GetContents(capi.World, pieStack);
		string extrakey = "ct" + (BlockPie.GetTopCrustType(pieStack) ?? "full") + "-bl" + pieStack.Attributes.GetAsInt("bakeLevel") + "-ps" + pieStack.Attributes.GetAsInt("pieSize");
		int mealhashcode = GetMealHashCode(pieBlock, contentStacks, null, extrakey);
		if (!meshrefs.TryGetValue(mealhashcode, out var mealMeshRef))
		{
			MeshData mesh = GetPieMesh(pieStack);
			if (mesh == null)
			{
				return null;
			}
			mealMeshRef = (meshrefs[mealhashcode] = capi.Render.UploadMultiTextureMesh(mesh));
		}
		return mealMeshRef;
	}

	public MeshData? GetPieMesh(ItemStack? pieStack, ModelTransform? transform = null)
	{
		nowTesselatingBlock = pieStack?.Block as BlockPie;
		if (nowTesselatingBlock == null)
		{
			return null;
		}
		contentStacks = nowTesselatingBlock.GetContents(capi.World, pieStack);
		int pieSize = pieStack?.Attributes.GetAsInt("pieSize") ?? 0;
		InPieProperties[] stackprops = contentStacks.Select((ItemStack stack) => stack?.ItemAttributes?["inPieProperties"]?.AsObject<InPieProperties>(null, stack.Collectible.Code.Domain)).ToArray();
		int bakeLevel = pieStack?.Attributes.GetAsInt("bakeLevel") ?? 0;
		if (stackprops.Length == 0)
		{
			return null;
		}
		ItemStack cstack = contentStacks[1];
		EnumFoodCategory[] foodCats = contentStacks.Select(BlockPie.FillingFoodCategory).ToArray();
		_ = foodCats[1];
		bool equal = true;
		bool foodCatEquals = true;
		IEnumerable<string> mixCodes = stackprops[1]?.MixingCodes ?? Array.Empty<string>();
		int i = 2;
		while ((equal || foodCatEquals || mixCodes.Any()) && i < contentStacks.Length - 1)
		{
			if (contentStacks[i] != null && cstack != null)
			{
				equal &= cstack.Equals(capi.World, contentStacks[i], GlobalConstants.IgnoredStackAttributes);
				foodCatEquals &= contentStacks[i] == null || foodCats[i] == foodCats[1];
				mixCodes = stackprops[i]?.MixingCodes.Intersect(mixCodes) ?? Array.Empty<string>();
				cstack = contentStacks[i];
				_ = foodCats[i];
			}
			i++;
		}
		if (ContentsRotten(contentStacks))
		{
			crustTextureLoc = new AssetLocation("block/rot/rot");
			fillingTextureLoc = new AssetLocation("block/rot/rot");
			topCrustTextureLoc = new AssetLocation("block/rot/rot");
		}
		else
		{
			if (stackprops[0] != null)
			{
				crustTextureLoc = stackprops[0].Texture.Clone();
				crustTextureLoc.Path = crustTextureLoc.Path.Replace("{bakelevel}", (bakeLevel + 1).ToString() ?? "");
				fillingTextureLoc = new AssetLocation("block/transparent");
			}
			topCrustTextureLoc = new AssetLocation("block/transparent");
			if (stackprops[5] != null)
			{
				topCrustTextureLoc = stackprops[5].Texture.Clone();
				topCrustTextureLoc.Path = topCrustTextureLoc.Path.Replace("{bakelevel}", (bakeLevel + 1).ToString() ?? "");
			}
			if (contentStacks[1] != null)
			{
				EnumFoodCategory fillingCat = foodCats[1];
				if (fillingCat == EnumFoodCategory.NoNutrition)
				{
					fillingCat = EnumFoodCategory.Unknown;
				}
				fillingTextureLoc = getPieFillingTexture(stackprops, mixCodes.ToArray(), equal, foodCatEquals, fillingCat);
			}
		}
		int fillLevel = ((contentStacks[1] != null) ? 1 : 0) + ((contentStacks[2] != null) ? 1 : 0) + ((contentStacks[3] != null) ? 1 : 0) + ((contentStacks[4] != null) ? 1 : 0);
		AssetLocation shapeloc = ((fillLevel == 4) ? pieShapeBySize[pieSize - 1] : pieShapeLocByFillLevel[fillLevel]);
		shapeloc.WithPathAppendixOnce(".json").WithPathPrefixOnce("shapes/");
		Shape shape = Shape.TryGet(capi, shapeloc);
		string topCrustShapeElement = BlockPie.TopCrustTypes.First((PieTopCrustType type) => type.Code.EqualsFast(BlockPie.GetTopCrustType(pieStack) ?? "full")).ShapeElement;
		string[] selectiveElements = new string[5] { "origin/base/crust regular/*", "origin/base/filling/*", "origin/base/base-quarter/*", "origin/base/fillingquarter/*", topCrustShapeElement };
		capi.Tesselator.TesselateShape("pie", shape, out var mesh, this, null, 0, 0, 0, null, selectiveElements);
		if (transform != null)
		{
			mesh.ModelTransform(transform);
		}
		return mesh;
	}

	private AssetLocation? getPieFillingTexture(InPieProperties?[] pieProps, string[] mixingCodes, bool singleFilling, bool singleFoodCat, EnumFoodCategory fillingCat)
	{
		if (singleFilling)
		{
			return pieProps[1]?.Texture;
		}
		if (!singleFoodCat && mixingCodes.Length != 0)
		{
			if (pieMixingCodeFillingTextures.TryGetValue(mixingCodes[0].GetHashCode(), out AssetLocation loc))
			{
				return loc;
			}
			pieMixingCodeFillingTextures.Add(mixingCodes[0].GetHashCode(), new AssetLocation("block/food/pie/fill-mixed" + mixingCodes[0]));
		}
		return pieMixedCategoryFillingTextures[(int)fillingCat];
	}

	public Dictionary<int, MultiTextureMeshRef> GetCookedMeshRefs()
	{
		object obj;
		return (Dictionary<int, MultiTextureMeshRef>)(capi.ObjectCache.TryGetValue("cookedMeshRefs", out obj) ? ((obj as Dictionary<int, MultiTextureMeshRef>) ?? new Dictionary<int, MultiTextureMeshRef>()) : (capi.ObjectCache["cookedMeshRefs"] = new Dictionary<int, MultiTextureMeshRef>()));
	}

	public MultiTextureMeshRef? GetOrCreateMealInContainerMeshRef(IBlockMealContainer be, ItemStack containerStack, Vec3f? foodTranslate)
	{
		return GetOrCreateMealInContainerMeshRef(containerStack.Block, be.GetCookingRecipe(capi.World, containerStack), be.GetNonEmptyContents(capi.World, containerStack), foodTranslate);
	}

	public MultiTextureMeshRef? GetOrCreateMealInContainerMeshRef(Block containerBlock, CookingRecipe? forRecipe, ItemStack?[]? contentStacks, Vec3f? foodTranslate = null)
	{
		if (contentStacks == null)
		{
			return null;
		}
		int mealhashcode = GetMealHashCode(containerBlock, contentStacks, foodTranslate);
		Dictionary<int, MultiTextureMeshRef> meshrefs = GetCookedMeshRefs();
		if (!meshrefs.TryGetValue(mealhashcode, out var mealMeshRef))
		{
			if (Environment.CurrentManagedThreadId != RuntimeEnv.MainThreadId)
			{
				if (capi != null && Environment.CurrentManagedThreadId == capi.TesselationThreadId)
				{
					capi.Event.EnqueueMainThreadTask(delegate
					{
						GetOrCreateMealInContainerMeshRef(containerBlock, forRecipe, contentStacks, foodTranslate);
					}, "createmealmesh");
				}
				throw new RetryTesselationException("Attempting to upload a new meal mesh outside of the main thread. If this occurs during tesselation, it should automatically retry. If not during tesselation, please only call this on the main thread!");
			}
			MeshData mesh = GenMealInContainerMesh(containerBlock, forRecipe, contentStacks, foodTranslate);
			mealMeshRef = (meshrefs[mealhashcode] = capi.Render.UploadMultiTextureMesh(mesh));
		}
		return mealMeshRef;
	}

	public MeshData GenMealInContainerMesh(Block containerBlock, CookingRecipe? forRecipe, ItemStack?[] contentStacks, Vec3f? foodTranslate = null)
	{
		CompositeShape cShape = containerBlock.Shape;
		AssetLocation loc = cShape.Base.Clone().WithPathPrefixOnce("shapes/").WithPathAppendixOnce(".json");
		Shape shape = Shape.TryGet(capi, loc);
		capi.Tesselator.TesselateShape("meal", shape, out var wholeMesh, capi.Tesselator.GetTextureSource(containerBlock), new Vec3f(cShape.rotateX, cShape.rotateY, cShape.rotateZ), 0, 0, 0);
		MeshData mealMesh = GenMealMesh(forRecipe, contentStacks, foodTranslate);
		if (mealMesh != null)
		{
			wholeMesh.AddMeshData(mealMesh);
		}
		return wholeMesh;
	}

	public MeshData? GenMealMesh(CookingRecipe? forRecipe, ItemStack?[] contentStacks, Vec3f? foodTranslate = null)
	{
		MealTextureSource source;
		try
		{
			source = new MealTextureSource(capi, mealtextureSourceBlock);
		}
		catch
		{
			capi.Logger.Error("Unable to create meal texture source for recipe: " + forRecipe?.Code + " for: " + mealtextureSourceBlock?.Code.ToShortString());
			throw;
		}
		if (forRecipe != null)
		{
			MeshData foodMesh = GenFoodMixMesh(contentStacks, forRecipe, foodTranslate);
			if (foodMesh != null)
			{
				return foodMesh;
			}
		}
		if (contentStacks != null && contentStacks.Length != 0)
		{
			if (ContentsRotten(contentStacks))
			{
				Shape contentShape = Shape.TryGet(capi, "shapes/block/food/meal/rot.json");
				capi.Tesselator.TesselateShape("rotcontents", contentShape, out var contentMesh, source, null, 0, 0, 0);
				if (foodTranslate != null)
				{
					contentMesh.Translate(foodTranslate);
				}
				return contentMesh;
			}
			if (contentStacks[0]?.ItemAttributes?["inContainerTexture"] != null)
			{
				source.ForStack = contentStacks[0];
				Shape contentShape2 = Shape.TryGet(shapePath: (contentStacks[0].ItemAttributes["inBowlShape"]?.AsObject(new CompositeShape
				{
					Base = new AssetLocation("shapes/block/food/meal/pickled.json")
				}))?.Base.WithPathAppendixOnce(".json").WithPathPrefixOnce("shapes/"), api: capi);
				capi.Tesselator.TesselateShape("picklednmealcontents", contentShape2, out var contentMesh2, source, null, 0, 0, 0);
				if (foodTranslate != null)
				{
					contentMesh2.Translate(foodTranslate);
				}
				return contentMesh2;
			}
		}
		return null;
	}

	public static bool ContentsRotten(ItemStack?[] contentStacks)
	{
		for (int i = 0; i < contentStacks.Length; i++)
		{
			if (contentStacks[i]?.Collectible.Code.Path == "rot")
			{
				return true;
			}
		}
		return false;
	}

	public static bool ContentsRotten(InventoryBase inv)
	{
		foreach (ItemSlot item in inv)
		{
			if (item.Itemstack?.Collectible.Code.Path == "rot")
			{
				return true;
			}
		}
		return false;
	}

	public MeshData? GenFoodMixMesh(ItemStack?[] contentStacks, CookingRecipe recipe, Vec3f? foodTranslate)
	{
		MeshData mergedmesh = null;
		MealTextureSource texSource = new MealTextureSource(capi, mealtextureSourceBlock);
		AssetLocation shapePath = recipe.Shape.Base.Clone().WithPathPrefixOnce("shapes/").WithPathAppendixOnce(".json");
		bool num = ContentsRotten(contentStacks);
		if (num)
		{
			shapePath = new AssetLocation("shapes/block/food/meal/rot.json");
		}
		Shape shape = Shape.TryGet(capi, shapePath);
		Dictionary<CookingRecipeIngredient, int> usedIngredQuantities = new Dictionary<CookingRecipeIngredient, int>();
		if (num)
		{
			capi.Tesselator.TesselateShape("mealpart", shape, out mergedmesh, texSource, new Vec3f(recipe.Shape.rotateX, recipe.Shape.rotateY, recipe.Shape.rotateZ), 0, 0, 0);
		}
		else
		{
			HashSet<string> drawnMeshes = new HashSet<string>();
			for (int i = 0; i < contentStacks.Length; i++)
			{
				texSource.ForStack = contentStacks[i];
				CookingRecipeIngredient ingred = recipe.GetIngrendientFor(contentStacks[i], (from val in usedIngredQuantities
					where val.Key.MaxQuantity <= val.Value
					select val.Key).ToArray());
				if (ingred == null)
				{
					ingred = recipe.GetIngrendientFor(contentStacks[i]);
				}
				else
				{
					usedIngredQuantities.TryGetValue(ingred, out var cnt);
					cnt = (usedIngredQuantities[ingred] = cnt + 1);
				}
				if (ingred == null)
				{
					continue;
				}
				string[] selectiveElements = null;
				CookingRecipeStack recipestack = ingred.GetMatchingStack(contentStacks[i]);
				if (recipestack == null)
				{
					continue;
				}
				if (recipestack.ShapeElement != null)
				{
					selectiveElements = new string[1] { recipestack.ShapeElement };
				}
				texSource.customTextureMapping = recipestack.TextureMapping;
				if (!drawnMeshes.Contains(recipestack.ShapeElement + recipestack.TextureMapping))
				{
					drawnMeshes.Add(recipestack.ShapeElement + recipestack.TextureMapping);
					capi.Tesselator.TesselateShape("mealpart", shape, out var meshpart, texSource, new Vec3f(recipe.Shape.rotateX, recipe.Shape.rotateY, recipe.Shape.rotateZ), 0, 0, 0, null, selectiveElements);
					if (mergedmesh == null)
					{
						mergedmesh = meshpart;
					}
					else
					{
						mergedmesh.AddMeshData(meshpart);
					}
				}
			}
		}
		if (foodTranslate != null)
		{
			mergedmesh?.Translate(foodTranslate);
		}
		return mergedmesh;
	}

	private void Event_LeaveWorld()
	{
		if (capi == null || !capi.ObjectCache.TryGetValue("cookedMeshRefs", out var obj) || !(obj is Dictionary<int, MultiTextureMeshRef> meshrefs))
		{
			return;
		}
		foreach (KeyValuePair<int, MultiTextureMeshRef> item in meshrefs)
		{
			item.Value.Dispose();
		}
		capi.ObjectCache.Remove("cookedMeshRefs");
	}

	public int GetMealHashCode(ItemStack stack, Vec3f? translate = null, string extraKey = "")
	{
		ItemStack[] contentStacks = (stack.Block as BlockContainer)?.GetNonEmptyContents(capi.World, stack);
		if (contentStacks == null)
		{
			return 0;
		}
		if (stack.Block is BlockPie)
		{
			extraKey = extraKey + "ct" + (BlockPie.GetTopCrustType(stack) ?? "full") + "-bl" + stack.Attributes.GetAsInt("bakeLevel") + "-ps" + stack.Attributes.GetAsInt("pieSize");
		}
		return GetMealHashCode(stack.Block, contentStacks, translate, extraKey);
	}

	protected int GetMealHashCode(Block block, ItemStack?[] contentStacks, Vec3f? translate = null, string? extraKey = null)
	{
		string shapestring = block.Shape.ToString() + block.Code.ToShortString();
		if (translate != null)
		{
			shapestring = shapestring + translate.X + "/" + translate.Y + "/" + translate.Z;
		}
		string contentstring = "";
		for (int i = 0; i < contentStacks.Length; i++)
		{
			if (contentStacks[i] != null)
			{
				if (contentStacks[i].Collectible.Code.Path == "rot")
				{
					return (shapestring + "rotten").GetHashCode();
				}
				contentstring += contentStacks[i].Collectible.Code.ToShortString();
			}
		}
		return (shapestring + contentstring + extraKey).GetHashCode();
	}
}
