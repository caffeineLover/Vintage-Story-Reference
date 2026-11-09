using System.Collections.Generic;
using Vintagestory.API.Common;
using Vintagestory.API.Util;

namespace Vintagestory.GameContent;

public class ModSystemSkinnableAdditions : ModSystem
{
	protected Dictionary<string, SkinnablePart> skinPartsByCode = new Dictionary<string, SkinnablePart>();

	public override bool ShouldLoad(EnumAppSide forSide)
	{
		return true;
	}

	public override void AssetsLoaded(ICoreAPI api)
	{
		base.AssetsLoaded(api);
		new List<SkinnablePart>();
		foreach (IAsset item in api.Assets.GetMany("config/seraphskinnableparts.json"))
		{
			SkinnablePart[] array = item.ToObject<SkinnablePart[]>();
			foreach (SkinnablePart part in array)
			{
				skinPartsByCode.TryGetValue(part.Code, out var expart);
				if (expart != null)
				{
					expart.Variants = expart.Variants.Append(part.Variants);
				}
				else
				{
					skinPartsByCode[part.Code] = part;
				}
			}
		}
	}

	public SkinnablePart[] AppendAdditions(SkinnablePart[] toParts)
	{
		foreach (SkinnablePart part in toParts)
		{
			skinPartsByCode.TryGetValue(part.Code, out var expart);
			if (expart != null)
			{
				part.Variants = part.Variants.Append(expart.Variants);
			}
		}
		return toParts;
	}
}
