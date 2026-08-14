using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using Vintagestory.API.Util;

namespace Vintagestory.API.Common;

/// <summary>
/// Creates a recipe using a 3D voxel-based system. Used for recipes types such as clayforming, smithing, or stone-knapping.
/// </summary>
[DocumentAsJson]
public abstract class LayeredVoxelRecipe : RecipeBase
{
	/// <summary>
	/// An array of ingredients for this recipe. If only using a single ingredient, see <see cref="P:Vintagestory.API.Common.LayeredVoxelRecipe.Ingredient" />.<br />
	/// Required if not using <see cref="P:Vintagestory.API.Common.LayeredVoxelRecipe.Ingredient" />.
	/// </summary>
	[DocumentAsJson("Required", "", false)]
	public CraftingRecipeIngredient[] Ingredients;

	/// <summary>
	/// A 2D array of strings that are layered together to form the recipe. Use "#" for solid, and "_" or " " for a gap.
	/// </summary>
	[DocumentAsJson("Required", "", false)]
	public string[][] Pattern;

	/// <summary>
	/// The final output of this recipe.
	/// </summary>
	[DocumentAsJson("Required", "", false)]
	public JsonItemStack Output;

	/// <summary>
	/// An array of voxels, created from <see cref="F:Vintagestory.API.Common.LayeredVoxelRecipe.Pattern" /> during loading. This array is cloned when a player starts creating the recipe.
	/// </summary>
	public bool[,,] Voxels;

	/// <summary>
	/// A single ingredient for this recipe. If you need to use more than one ingredient, see <see cref="F:Vintagestory.API.Common.LayeredVoxelRecipe.Ingredients" />.<br />
	/// Required if not using <see cref="F:Vintagestory.API.Common.LayeredVoxelRecipe.Ingredients" />.
	/// </summary>
	[DocumentAsJson("Required", "", false)]
	public CraftingRecipeIngredient? Ingredient
	{
		get
		{
			if (Ingredients == null || Ingredients.Length == 0)
			{
				return null;
			}
			return Ingredients[0];
		}
		set
		{
			Ingredients = ((value == null) ? Array.Empty<CraftingRecipeIngredient>() : new CraftingRecipeIngredient[1] { value });
		}
	}

	/// <summary>
	/// The number of layers in this recipe, in the Y-axis.
	/// </summary>
	public abstract int QuantityLayers { get; }

	/// <summary>
	/// A category code for this recipe type. Used for error logging.
	/// </summary>
	public abstract string RecipeCategoryCode { get; }

	/// <summary>
	/// If true, the recipe is rotated 90 degrees in the Y axis.
	/// </summary>
	protected virtual bool RotateRecipe { get; set; }

	public override IEnumerable<IRecipeIngredient> RecipeIngredients => Ingredients ?? throw new InvalidOperationException($"Recipe '{base.Name}' has no ingredients specified");

	public override IRecipeOutput RecipeOutput => Output ?? throw new InvalidOperationException($"Recipe '{base.Name}' has no output specified");

	protected LayeredVoxelRecipe()
	{
		Voxels = new bool[16, QuantityLayers, 16];
	}

	/// <summary>
	/// Generates the voxels for the recipe.
	/// </summary>
	public void GenVoxels()
	{
		int length = Pattern[0][0].Length;
		int width = Pattern[0].Length;
		int height = Pattern.Length;
		if (width > 16 || height > QuantityLayers || length > 16)
		{
			throw new InvalidOperationException(string.Format("Invalid {1} recipe {0}! Either Width or length is beyond 16 voxels or height is beyond {2} voxels", base.Name, RecipeCategoryCode, QuantityLayers));
		}
		for (int i = 0; i < Pattern.Length; i++)
		{
			if (Pattern[i].Length != width)
			{
				global::_003C_003Ey__InlineArray5<object> buffer = default(global::_003C_003Ey__InlineArray5<object>);
				buffer[0] = i;
				buffer[1] = Pattern[i].Length;
				buffer[2] = width;
				buffer[3] = base.Name;
				buffer[4] = RecipeCategoryCode;
				throw new InvalidOperationException(string.Format("Invalid {4} recipe {3}! Layer {0} has a width of {1}, which is not the same as the first layer width of {2}. All layers need to be sized equally.", (ReadOnlySpan<object?>)buffer));
			}
			for (int j = 0; j < Pattern[i].Length; j++)
			{
				if (Pattern[i][j].Length != length)
				{
					global::_003C_003Ey__InlineArray6<object> buffer2 = default(global::_003C_003Ey__InlineArray6<object>);
					buffer2[0] = i;
					buffer2[1] = Pattern[i][j].Length;
					buffer2[2] = length;
					buffer2[3] = base.Name;
					buffer2[4] = j;
					buffer2[5] = RecipeCategoryCode;
					throw new InvalidOperationException(string.Format("Invalid {5} recipe {3}! Layer {0}, line {4} has a length of {1}, which is not the same as the first layer length of {2}. All layers need to be sized equally.", (ReadOnlySpan<object?>)buffer2));
				}
			}
		}
		int startX = (16 - width) / 2;
		int startZ = (16 - length) / 2;
		for (int x = 0; x < Math.Min(width, 16); x++)
		{
			for (int y = 0; y < Math.Min(height, QuantityLayers); y++)
			{
				for (int z = 0; z < Math.Min(length, 16); z++)
				{
					if (RotateRecipe)
					{
						Voxels[z + startZ, y, x + startX] = Pattern[y][x][z] != '_' && Pattern[y][x][z] != ' ';
					}
					else
					{
						Voxels[x + startX, y, z + startZ] = Pattern[y][x][z] != '_' && Pattern[y][x][z] != ' ';
					}
				}
			}
		}
	}

	public override void OnParsed(IWorldAccessor world)
	{
		if (Ingredients == null)
		{
			return;
		}
		int ingredientIndex = 1;
		CraftingRecipeIngredient[] ingredients = Ingredients;
		foreach (CraftingRecipeIngredient craftingRecipeIngredient in ingredients)
		{
			if (craftingRecipeIngredient.Id == null)
			{
				string text = (craftingRecipeIngredient.Id = ingredientIndex++.ToString());
			}
		}
	}

	/// <summary>
	/// Resolves the recipe.
	/// </summary>
	/// <param name="world"></param>
	/// <param name="sourceForErrorLogging"></param>
	/// <returns></returns>
	public override bool Resolve(IWorldAccessor world, string sourceForErrorLogging)
	{
		if (Pattern == null || Ingredient == null || RecipeOutput == null)
		{
			world.Logger.Error("{1} Recipe with output {0} has no ingredient pattern or missing ingredient/output. Ignoring recipe.", RecipeOutput, RecipeCategoryCode);
			return false;
		}
		if (!Ingredient.Resolve(world, RecipeCategoryCode + " recipe", this))
		{
			world.Logger.Error("{2} Recipe with output {0}: Cannot resolve ingredient in {1}.", RecipeOutput, sourceForErrorLogging, RecipeCategoryCode);
			return false;
		}
		if (!RecipeOutput.Resolve(world, sourceForErrorLogging + " " + Ingredient.Code))
		{
			return false;
		}
		GenVoxels();
		return true;
	}

	/// <summary>
	/// Serialized the recipe
	/// </summary>
	/// <param name="writer"></param>
	public override void ToBytes(BinaryWriter writer)
	{
		if (Ingredient == null || base.Name == null)
		{
			throw new InvalidOperationException("Trying to write voxel recipe into bytes, but recipe has a null name or ingredient");
		}
		writer.Write(base.RecipeId);
		writer.Write(Ingredients.Length);
		CraftingRecipeIngredient[] ingredients = Ingredients;
		for (int i = 0; i < ingredients.Length; i++)
		{
			ingredients[i].ToBytes(writer);
		}
		writer.Write(Pattern.Length);
		for (int j = 0; j < Pattern.Length; j++)
		{
			writer.WriteArray(Pattern[j]);
		}
		writer.Write(base.Name.ToShortString());
		RecipeOutput.ToBytes(writer);
	}

	/// <summary>
	/// Deserializes the alloy
	/// </summary>
	/// <param name="reader"></param>
	/// <param name="resolver"></param>
	public override void FromBytes(BinaryReader reader, IWorldAccessor resolver)
	{
		base.RecipeId = reader.ReadInt32();
		int ingredientsCount = reader.ReadInt32();
		Ingredients = new CraftingRecipeIngredient[ingredientsCount];
		for (int i = 0; i < ingredientsCount; i++)
		{
			CraftingRecipeIngredient ingredient = new CraftingRecipeIngredient();
			ingredient.FromBytes(reader, resolver);
			ingredient.Resolve(resolver, "Voxel recipes FromBytes", this);
			Ingredients[i] = ingredient;
		}
		int len = reader.ReadInt32();
		Pattern = new string[len][];
		for (int j = 0; j < Pattern.Length; j++)
		{
			Pattern[j] = reader.ReadStringArray();
		}
		base.Name = new AssetLocation(reader.ReadString());
		Output = new JsonItemStack();
		Output.FromBytes(reader, resolver.ClassRegistry);
		Output.Resolve(resolver, "[Voxel recipe FromBytes] " + Ingredient.Code);
		GenVoxels();
	}

	/// <summary>
	/// Matches the wildcards for the clay recipe.
	/// </summary>
	/// <param name="wildCard"></param>
	/// <param name="blockCode"></param>
	/// <returns></returns>
	public static bool WildCardMatch(AssetLocation wildCard, AssetLocation blockCode)
	{
		if (blockCode == null || !wildCard.Domain.Equals(blockCode.Domain))
		{
			return false;
		}
		if (wildCard.Equals(blockCode))
		{
			return true;
		}
		string pattern = Regex.Escape(wildCard.Path).Replace("\\*", "(.*)");
		return Regex.IsMatch(blockCode.Path, "^" + pattern + "$");
	}

	protected override void CloneTo(object recipe)
	{
		base.CloneTo(recipe);
		LayeredVoxelRecipe obj = (recipe as LayeredVoxelRecipe) ?? throw new ArgumentException("CloneTo should take object of same class or it subclass");
		obj.Ingredients = Ingredients?.Select((CraftingRecipeIngredient ingredient) => ingredient.Clone()).ToArray();
		obj.Ingredient = Ingredient?.Clone();
		obj.Pattern = Pattern;
		obj.Output = Output.Clone();
		obj.Voxels = Voxels;
	}
}
