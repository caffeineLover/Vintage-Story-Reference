using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Vintagestory.API.Util;

internal class ListAccess<T>
{
	[UnsafeAccessor(UnsafeAccessorKind.Field, Name = "_items")]
	internal static extern ref T[] Items(List<T> list);

	[UnsafeAccessor(UnsafeAccessorKind.Field, Name = "_size")]
	public static extern ref int Size(List<T> list);
}
