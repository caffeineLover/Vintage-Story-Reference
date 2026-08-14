using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Text;

namespace YamlDotNet.Helpers
{
	// Token: 0x020001A3 RID: 419
	[NullableContext(1)]
	[Nullable(0)]
	[DebuggerStepThrough]
	internal static class StringBuilderPool
	{
		// Token: 0x0600090A RID: 2314 RVA: 0x0001D0B3 File Offset: 0x0001B2B3
		public static StringBuilderPool.BuilderWrapper Rent()
		{
			return new StringBuilderPool.BuilderWrapper(StringBuilderPool.Pool.Allocate(), StringBuilderPool.Pool);
		}

		// Token: 0x040003AE RID: 942
		private static readonly ConcurrentObjectPool<StringBuilder> Pool = new ConcurrentObjectPool<StringBuilder>(() => new StringBuilder());

		// Token: 0x020001A4 RID: 420
		[Nullable(0)]
		internal readonly struct BuilderWrapper : IDisposable
		{
			// Token: 0x0600090B RID: 2315 RVA: 0x0001D0C9 File Offset: 0x0001B2C9
			public BuilderWrapper(StringBuilder builder, ConcurrentObjectPool<StringBuilder> pool)
			{
				this.Builder = builder;
				this._pool = pool;
			}

			// Token: 0x0600090C RID: 2316 RVA: 0x0001D0D9 File Offset: 0x0001B2D9
			public override string ToString()
			{
				return this.Builder.ToString();
			}

			// Token: 0x0600090D RID: 2317 RVA: 0x0001D0E8 File Offset: 0x0001B2E8
			public void Dispose()
			{
				StringBuilder builder = this.Builder;
				if (builder.Capacity <= 1024)
				{
					builder.Length = 0;
					this._pool.Free(builder);
				}
			}

			// Token: 0x040003AF RID: 943
			public readonly StringBuilder Builder;

			// Token: 0x040003B0 RID: 944
			private readonly ConcurrentObjectPool<StringBuilder> _pool;
		}
	}
}
