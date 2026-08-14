using System;
using System.IO;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace QuestPDF.Skia
{
	// Token: 0x02000022 RID: 34
	internal sealed class SkWriteStream : IDisposable
	{
		// Token: 0x17000028 RID: 40
		// (get) Token: 0x060000B3 RID: 179 RVA: 0x0000362F File Offset: 0x0000182F
		// (set) Token: 0x060000B4 RID: 180 RVA: 0x00003637 File Offset: 0x00001837
		public IntPtr Instance { get; private set; }

		// Token: 0x17000029 RID: 41
		// (get) Token: 0x060000B5 RID: 181 RVA: 0x00003640 File Offset: 0x00001840
		private GCHandle CallbackHandle { get; }

		// Token: 0x060000B6 RID: 182 RVA: 0x00003648 File Offset: 0x00001848
		[NullableContext(1)]
		public SkWriteStream(Stream stream)
		{
			SkWriteStream.API.ByteArrayCallback byteArrayCallback = delegate(IntPtr data, int size)
			{
				ReadOnlySpan<byte> readOnlySpan = new ReadOnlySpan<byte>(data, size);
				stream.Write(readOnlySpan);
			};
			this.CallbackHandle = GCHandle.Alloc(byteArrayCallback);
			this.Instance = SkWriteStream.API.write_stream_create(byteArrayCallback);
			SkiaAPI.EnsureNotNull(this.Instance);
		}

		// Token: 0x060000B7 RID: 183 RVA: 0x00003696 File Offset: 0x00001896
		public void Flush()
		{
			SkWriteStream.API.write_stream_flush(this.Instance);
		}

		// Token: 0x060000B8 RID: 184 RVA: 0x000036A4 File Offset: 0x000018A4
		~SkWriteStream()
		{
			this.WarnThatFinalizerIsReached<SkWriteStream>();
			this.Dispose();
		}

		// Token: 0x060000B9 RID: 185 RVA: 0x000036D8 File Offset: 0x000018D8
		public void Dispose()
		{
			if (this.Instance == IntPtr.Zero)
			{
				return;
			}
			this.CallbackHandle.Free();
			SkWriteStream.API.write_stream_delete(this.Instance);
			this.Instance = IntPtr.Zero;
			GC.SuppressFinalize(this);
		}

		// Token: 0x02000143 RID: 323
		private static class API
		{
			// Token: 0x06000B1F RID: 2847
			[NullableContext(1)]
			[DllImport("QuestPdfSkia", CallingConvention = 2)]
			public static extern IntPtr write_stream_create(SkWriteStream.API.ByteArrayCallback callback);

			// Token: 0x06000B20 RID: 2848
			[DllImport("QuestPdfSkia", CallingConvention = 2)]
			public static extern void write_stream_flush(IntPtr stream);

			// Token: 0x06000B21 RID: 2849
			[DllImport("QuestPdfSkia", CallingConvention = 2)]
			public static extern void write_stream_delete(IntPtr stream);

			// Token: 0x0200021D RID: 541
			// (Invoke) Token: 0x06000E3C RID: 3644
			[UnmanagedFunctionPointer(2)]
			public delegate void ByteArrayCallback(IntPtr data, int size);
		}
	}
}
