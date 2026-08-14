using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Text;

namespace YamlDotNet.Core.ObjectPool
{
	// Token: 0x020000FF RID: 255
	[NullableContext(1)]
	[Nullable(0)]
	[DebuggerStepThrough]
	internal static class StringBuilderPool
	{
		// Token: 0x0600063D RID: 1597 RVA: 0x00017410 File Offset: 0x00015610
		public static StringBuilderPool.BuilderWrapper Rent()
		{
			StringBuilder builder = StringBuilderPool.Pool.Get();
			return new StringBuilderPool.BuilderWrapper(builder, StringBuilderPool.Pool);
		}

		// Token: 0x040001DA RID: 474
		private static readonly ObjectPool<StringBuilder> Pool = ObjectPool.Create<StringBuilder>(new StringBuilderPooledObjectPolicy
		{
			InitialCapacity = 16,
			MaximumRetainedCapacity = 1024
		});

		// Token: 0x020001A0 RID: 416
		[Nullable(0)]
		internal readonly struct BuilderWrapper : IDisposable
		{
			// Token: 0x060008DF RID: 2271 RVA: 0x0001AE27 File Offset: 0x00019027
			public BuilderWrapper(StringBuilder builder, ObjectPool<StringBuilder> pool)
			{
				this.Builder = builder;
				this.pool = pool;
			}

			// Token: 0x060008E0 RID: 2272 RVA: 0x0001AE37 File Offset: 0x00019037
			public override string ToString()
			{
				return this.Builder.ToString();
			}

			// Token: 0x060008E1 RID: 2273 RVA: 0x0001AE44 File Offset: 0x00019044
			public void Dispose()
			{
				this.pool.Return(this.Builder);
			}

			// Token: 0x04000361 RID: 865
			public readonly StringBuilder Builder;

			// Token: 0x04000362 RID: 866
			private readonly ObjectPool<StringBuilder> pool;
		}
	}
}
