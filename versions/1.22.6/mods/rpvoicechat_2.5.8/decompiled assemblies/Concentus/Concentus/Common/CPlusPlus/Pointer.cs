using System;

namespace Concentus.Common.CPlusPlus
{
	// Token: 0x02000082 RID: 130
	internal static class Pointer
	{
		// Token: 0x0600048B RID: 1163 RVA: 0x000280DF File Offset: 0x000262DF
		internal static Pointer<E> Malloc<E>(int capacity)
		{
			return new Pointer<E>(capacity);
		}

		// Token: 0x0600048C RID: 1164 RVA: 0x000280E7 File Offset: 0x000262E7
		internal static Pointer<E> GetPointer<E>(this E[] memory, int offset = 0)
		{
			if (memory == null)
			{
				return null;
			}
			return new Pointer<E>(memory, offset);
		}
	}
}
