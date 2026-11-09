using Vintagestory.API.Common;

namespace Vintagestory.API.Client;

public interface IHandbookGrouping
{
	AssetLocation GetCodeForHandbookGrouping(ItemStack stack);

	string GetWildcardForHandbookGrouping(string wildcard, ItemStack stack);
}
