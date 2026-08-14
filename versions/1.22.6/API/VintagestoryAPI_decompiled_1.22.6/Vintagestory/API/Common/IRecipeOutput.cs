using System;

namespace Vintagestory.API.Common;

public interface IRecipeOutput : IByteSerializable, ICloneable
{
	ItemStack? ResolvedItemStack { get; }

	void FillPlaceHolder(string key, string value);

	bool Resolve(IWorldAccessor world, string sourceForErrorLogging);
}
