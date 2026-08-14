using System;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using Vintagestory.API.Common;
using Vintagestory.API.Config;
using Vintagestory.API.Server;
using Vintagestory.GameContent;

namespace Vintagestory.ServerMods;

public sealed class RecipeLoader : ModSystem
{
	public override double ExecuteOrder()
	{
		return 1.0;
	}

	public override bool ShouldLoad(EnumAppSide forSide)
	{
		return forSide == EnumAppSide.Server;
	}

	public override void AssetsLoaded(ICoreAPI api)
	{
		ICoreServerAPI serverApi = api as ICoreServerAPI;
		if (serverApi != null)
		{
			bool classExclusiveRecipes = serverApi.World.Config.GetBool("classExclusiveRecipes", defaultValue: true);
			LoadAlloyRecipes(serverApi);
			LoadRecipes<GridRecipe>(serverApi, "grid", "recipes/grid", classExclusiveRecipes, delegate(IRecipeBase r)
			{
				serverApi.RegisterCraftingRecipe(r as GridRecipe);
			});
			api.Logger.StoryEvent(Lang.Get("Grand inventions..."));
			LoadRecipes<SmithingRecipe>(serverApi, "smithing", "recipes/smithing", classExclusiveRecipes, delegate(IRecipeBase r)
			{
				serverApi.RegisterSmithingRecipe(r as SmithingRecipe);
			});
			serverApi.World.Logger.StoryEvent(Lang.Get("Burning sparks..."));
			LoadRecipes<ClayFormingRecipe>(serverApi, "clay forming", "recipes/clayforming", classExclusiveRecipes, delegate(IRecipeBase r)
			{
				serverApi.RegisterClayFormingRecipe(r as ClayFormingRecipe);
			});
			serverApi.World.Logger.StoryEvent(Lang.Get("Molded forms..."));
			LoadRecipes<KnappingRecipe>(serverApi, "knapping", "recipes/knapping", classExclusiveRecipes, delegate(IRecipeBase r)
			{
				serverApi.RegisterKnappingRecipe(r as KnappingRecipe);
			});
			serverApi.World.Logger.StoryEvent(Lang.Get("Simple tools..."));
			LoadRecipes<BarrelRecipe>(serverApi, "barrel recipe", "recipes/barrel", classExclusiveRecipes, delegate(IRecipeBase r)
			{
				serverApi.RegisterBarrelRecipe(r as BarrelRecipe);
			});
		}
	}

	public static void LoadAlloyRecipes(ICoreServerAPI api)
	{
		Dictionary<AssetLocation, AlloyRecipe> alloys = api.Assets.GetMany<AlloyRecipe>(api.Server.Logger, "recipes/alloy");
		foreach (var (assetPath, recipe) in alloys)
		{
			if (recipe.Enabled)
			{
				recipe.Resolve(api.World, "alloy recipe " + assetPath);
				api.RegisterMetalAlloy(recipe);
			}
		}
		api.World.Logger.Event("{0} metal alloys loaded", alloys.Count);
		api.World.Logger.StoryEvent(Lang.Get("Glimmers in the soil..."));
	}

	public static void LoadRecipes<TRecipe>(ICoreServerAPI api, string name, string path, bool classExclusiveRecipes, Action<IRecipeBase> registerDelegate) where TRecipe : IRecipeBase
	{
		Dictionary<AssetLocation, JToken> files = api.Assets.GetMany<JToken>(api.Logger, path);
		int recipeQuantity = 0;
		int recipesLoaded = 0;
		int failedResolveCount = 0;
		foreach (var (location, content) in files)
		{
			if (content is JObject recipeObject)
			{
				TRecipe parsedContent = recipeObject.ToObject<TRecipe>(location.Domain);
				if (parsedContent == null)
				{
					api.Logger.Error($"Failed to parse {name} recipe: {location}");
				}
				else
				{
					LoadRecipe(api, location, parsedContent, classExclusiveRecipes, registerDelegate, ref recipesLoaded, ref failedResolveCount);
					recipeQuantity++;
				}
			}
			else
			{
				if (!(content is JArray arrayOfRecipes))
				{
					continue;
				}
				foreach (JToken item in arrayOfRecipes)
				{
					TRecipe parsedContent2 = item.ToObject<TRecipe>(location.Domain);
					if (parsedContent2 == null)
					{
						api.Logger.Error($"Failed to parse {name} recipe: {location}");
					}
					else
					{
						LoadRecipe(api, location, parsedContent2, classExclusiveRecipes, registerDelegate, ref recipesLoaded, ref failedResolveCount);
						recipeQuantity++;
					}
				}
			}
		}
		if (failedResolveCount > 0)
		{
			api.Logger.Event($"{recipeQuantity} {name} recipes loaded from {files.Count} files, failed to resolve {failedResolveCount} recipes");
		}
		else
		{
			api.Logger.Event($"{recipeQuantity} {name} recipes loaded from {files.Count} files");
		}
		RecipeBase.CollectiblePreSearchResultsCache.Clear();
	}

	public static void LoadRecipe(ICoreServerAPI api, AssetLocation assetLocation, IRecipeBase recipe, bool classExclusiveRecipes, Action<IRecipeBase> registerDelegate, ref int loaded, ref int failedResolveCount)
	{
		if (!recipe.Enabled)
		{
			return;
		}
		if (!classExclusiveRecipes)
		{
			recipe.RequiresTrait = null;
		}
		if (recipe.Name == null)
		{
			recipe.Name = assetLocation;
		}
		recipe.OnParsed(api.World);
		foreach (IRecipeBase subRecipe in recipe.GenerateRecipesForAllIngredientCombinations(api.World))
		{
			if (subRecipe.Resolve(api.World, "RecipeLoader"))
			{
				registerDelegate(subRecipe);
				loaded++;
			}
			else
			{
				failedResolveCount++;
			}
		}
	}
}
