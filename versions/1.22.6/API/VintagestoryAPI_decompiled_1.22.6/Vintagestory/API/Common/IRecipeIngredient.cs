using System;
using Vintagestory.API.Datastructures;

namespace Vintagestory.API.Common;

public interface IRecipeIngredient : IRecipeIngredientBase, IByteSerializable, ICloneable
{
	int Quantity { get; set; }

	/// <summary>
	/// Used to reference ingredients, auto assigned if not specified
	/// </summary>
	string Id { get; set; }

	/// <summary>
	/// Attaches a name to a wildcard in an ingredient. This is used to substitute the value into the output. Only required if using a wildcard.
	/// </summary>
	string? Name { get; set; }

	JsonItemStack? ReturnedStack { get; set; }

	JsonObject? RecipeAttributes { get; set; }

	RecipeIngredientConsumeProperties ConsumeProperties { get; }

	void FillPlaceHolder(string key, string value);

	bool Resolve(IWorldAccessor world, string sourceForErrorLogging);

	static bool IsAdvancedWildcard(string code)
	{
		if (code.Contains('{'))
		{
			return code.Contains('}');
		}
		return false;
	}

	static bool IsBasicWildcard(string code)
	{
		return code.Contains('*');
	}

	static bool IsRegex(string code)
	{
		return code.StartsWith('@');
	}

	static EnumRecipeMatchType GetMatchType(string? code, bool named = false)
	{
		if (code == null || code == "*:*")
		{
			return EnumRecipeMatchType.TagsOnly;
		}
		bool num = IsRegex(code);
		bool advanced = IsAdvancedWildcard(code);
		bool wildcard = IsBasicWildcard(code);
		if (!num)
		{
			if (!advanced)
			{
				if (wildcard)
				{
					if (named)
					{
						return EnumRecipeMatchType.NamedWildcard;
					}
					return EnumRecipeMatchType.Wildcard;
				}
				return EnumRecipeMatchType.Exact;
			}
			return EnumRecipeMatchType.AdvancedWildcard;
		}
		return EnumRecipeMatchType.Regex;
	}
}
