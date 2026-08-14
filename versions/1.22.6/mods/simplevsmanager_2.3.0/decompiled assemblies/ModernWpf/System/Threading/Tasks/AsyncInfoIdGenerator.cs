using System;

namespace System.Threading.Tasks
{
	// Token: 0x02000009 RID: 9
	internal static class AsyncInfoIdGenerator
	{
		// Token: 0x06000022 RID: 34 RVA: 0x00002684 File Offset: 0x00000884
		internal static uint CreateNext()
		{
			Random random = AsyncInfoIdGenerator.s_idGenerator;
			uint result;
			lock (random)
			{
				result = (uint)AsyncInfoIdGenerator.s_idGenerator.Next(1, int.MaxValue);
			}
			return result;
		}

		// Token: 0x06000023 RID: 35 RVA: 0x000026D0 File Offset: 0x000008D0
		internal unsafe static uint EnsureInitializedThreadsafe(ref uint id)
		{
			if (id != 2147483647U)
			{
				return id;
			}
			uint num = AsyncInfoIdGenerator.CreateNext();
			fixed (uint* ptr = &id)
			{
				uint num2 = (uint)Interlocked.CompareExchange(ref *(int*)ptr, (int)num, int.MaxValue);
				if (num2 == 2147483647U)
				{
					return num;
				}
				return num2;
			}
		}

		// Token: 0x04000006 RID: 6
		internal const uint InvalidId = 2147483647U;

		// Token: 0x04000007 RID: 7
		private static readonly Random s_idGenerator = new Random(19830118);
	}
}
