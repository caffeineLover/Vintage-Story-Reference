using System;
using System.Runtime.CompilerServices;

namespace YamlDotNet.Core.ObjectPool
{
	// Token: 0x02000101 RID: 257
	[NullableContext(1)]
	[Nullable(0)]
	internal static class StringLookAheadBufferPool
	{
		// Token: 0x06000646 RID: 1606 RVA: 0x000174BC File Offset: 0x000156BC
		public static StringLookAheadBufferPool.BufferWrapper Rent(string value)
		{
			StringLookAheadBuffer stringLookAheadBuffer = StringLookAheadBufferPool.Pool.Get();
			stringLookAheadBuffer.Value = value;
			return new StringLookAheadBufferPool.BufferWrapper(stringLookAheadBuffer, StringLookAheadBufferPool.Pool);
		}

		// Token: 0x040001DD RID: 477
		private static readonly ObjectPool<StringLookAheadBuffer> Pool = ObjectPool.Create<StringLookAheadBuffer>(new DefaultPooledObjectPolicy<StringLookAheadBuffer>());

		// Token: 0x020001A1 RID: 417
		[Nullable(0)]
		internal readonly struct BufferWrapper : IDisposable
		{
			// Token: 0x060008E2 RID: 2274 RVA: 0x0001AE57 File Offset: 0x00019057
			public BufferWrapper(StringLookAheadBuffer buffer, ObjectPool<StringLookAheadBuffer> pool)
			{
				this.Buffer = buffer;
				this.pool = pool;
			}

			// Token: 0x060008E3 RID: 2275 RVA: 0x0001AE67 File Offset: 0x00019067
			public override string ToString()
			{
				return this.Buffer.ToString();
			}

			// Token: 0x060008E4 RID: 2276 RVA: 0x0001AE74 File Offset: 0x00019074
			public void Dispose()
			{
				this.pool.Return(this.Buffer);
			}

			// Token: 0x04000363 RID: 867
			public readonly StringLookAheadBuffer Buffer;

			// Token: 0x04000364 RID: 868
			private readonly ObjectPool<StringLookAheadBuffer> pool;
		}
	}
}
