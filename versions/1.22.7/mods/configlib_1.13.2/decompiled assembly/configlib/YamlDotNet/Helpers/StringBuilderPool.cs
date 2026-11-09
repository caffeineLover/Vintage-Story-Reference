using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Text;

namespace YamlDotNet.Helpers
{
	// Token: 0x0200019F RID: 415
	[NullableContext(1)]
	[Nullable(0)]
	[DebuggerStepThrough]
	internal static class StringBuilderPool
	{
		// Token: 0x060008CE RID: 2254 RVA: 0x0001C76B File Offset: 0x0001A96B
		public static StringBuilderPool.BuilderWrapper Rent()
		{
			return new StringBuilderPool.BuilderWrapper(StringBuilderPool.Pool.Allocate(), StringBuilderPool.Pool);
		}

		// Token: 0x04000380 RID: 896
		private static readonly ConcurrentObjectPool<StringBuilder> Pool = new ConcurrentObjectPool<StringBuilder>(() => new StringBuilder());

		// Token: 0x020001A0 RID: 416
		[Nullable(0)]
		internal readonly struct BuilderWrapper : IDisposable
		{
			// Token: 0x060008CF RID: 2255 RVA: 0x0001C781 File Offset: 0x0001A981
			public BuilderWrapper(StringBuilder builder, ConcurrentObjectPool<StringBuilder> pool)
			{
				this.Builder = builder;
				this._pool = pool;
			}

			// Token: 0x060008D0 RID: 2256 RVA: 0x0001C791 File Offset: 0x0001A991
			public override string ToString()
			{
				return this.Builder.ToString();
			}

			// Token: 0x060008D1 RID: 2257 RVA: 0x0001C7A0 File Offset: 0x0001A9A0
			public void Dispose()
			{
				StringBuilder builder = this.Builder;
				if (builder.Capacity <= 1024)
				{
					builder.Length = 0;
					this._pool.Free(builder);
				}
			}

			// Token: 0x04000381 RID: 897
			public readonly StringBuilder Builder;

			// Token: 0x04000382 RID: 898
			private readonly ConcurrentObjectPool<StringBuilder> _pool;
		}
	}
}
