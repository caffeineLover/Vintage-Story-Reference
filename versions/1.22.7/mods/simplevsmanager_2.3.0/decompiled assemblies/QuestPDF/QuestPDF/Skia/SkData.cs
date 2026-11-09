using System;
using System.IO;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace QuestPDF.Skia
{
	// Token: 0x0200000A RID: 10
	[NullableContext(1)]
	[Nullable(0)]
	internal sealed class SkData : IDisposable
	{
		// Token: 0x17000011 RID: 17
		// (get) Token: 0x06000048 RID: 72 RVA: 0x000027D9 File Offset: 0x000009D9
		// (set) Token: 0x06000049 RID: 73 RVA: 0x000027E1 File Offset: 0x000009E1
		public IntPtr Instance { get; private set; }

		// Token: 0x0600004A RID: 74 RVA: 0x000027EA File Offset: 0x000009EA
		public SkData(IntPtr instance)
		{
			this.Instance = instance;
			SkiaAPI.EnsureNotNull(this.Instance);
		}

		// Token: 0x0600004B RID: 75 RVA: 0x00002804 File Offset: 0x00000A04
		public static SkData FromFile(string filePath)
		{
			IntPtr intPtr = SkData.API.data_create_from_file(filePath);
			if (intPtr == IntPtr.Zero)
			{
				throw new Exception("Cannot load a file under the provided path: " + filePath + ".");
			}
			return new SkData(intPtr);
		}

		// Token: 0x0600004C RID: 76 RVA: 0x00002830 File Offset: 0x00000A30
		public static SkData FromStream(Stream stream)
		{
			SkData result;
			using (MemoryStream memoryStream = new MemoryStream())
			{
				stream.CopyTo(memoryStream);
				result = SkData.FromBinary(memoryStream.ToArray());
			}
			return result;
		}

		// Token: 0x0600004D RID: 77 RVA: 0x00002874 File Offset: 0x00000A74
		public unsafe static SkData FromBinary(byte[] data)
		{
			byte* arrayPointer;
			if (data == null || data.Length == 0)
			{
				arrayPointer = null;
			}
			else
			{
				arrayPointer = &data[0];
			}
			IntPtr instance = SkData.API.data_create_from_binary(arrayPointer, data.Length);
			GC.KeepAlive(data);
			return new SkData(instance);
		}

		// Token: 0x0600004E RID: 78 RVA: 0x000028B0 File Offset: 0x00000AB0
		public byte[] ToBytes()
		{
			SkData.API.GetBytesFromDataResult getBytesFromDataResult = SkData.API.data_get_bytes(this.Instance);
			byte[] array = new byte[getBytesFromDataResult.length];
			Marshal.Copy(getBytesFromDataResult.bytes, array, 0, getBytesFromDataResult.length);
			return array;
		}

		// Token: 0x0600004F RID: 79 RVA: 0x000028EC File Offset: 0x00000AEC
		~SkData()
		{
			this.WarnThatFinalizerIsReached<SkData>();
			this.Dispose();
		}

		// Token: 0x06000050 RID: 80 RVA: 0x00002920 File Offset: 0x00000B20
		public void Dispose()
		{
			if (this.Instance == IntPtr.Zero)
			{
				return;
			}
			SkData.API.data_unref(this.Instance);
			this.Instance = IntPtr.Zero;
			GC.SuppressFinalize(this);
		}

		// Token: 0x02000134 RID: 308
		[NullableContext(0)]
		private static class API
		{
			// Token: 0x06000AEE RID: 2798
			[NullableContext(1)]
			[DllImport("QuestPdfSkia", CallingConvention = 2)]
			public static extern IntPtr data_create_from_file([MarshalAs(44, MarshalTypeRef = QuestPDF.Skia.Utf8StringMarshaller)] string path);

			// Token: 0x06000AEF RID: 2799
			[DllImport("QuestPdfSkia", CallingConvention = 2)]
			public unsafe static extern IntPtr data_create_from_binary(byte* arrayPointer, int arrayLength);

			// Token: 0x06000AF0 RID: 2800
			[DllImport("QuestPdfSkia", CallingConvention = 2)]
			public static extern SkData.API.GetBytesFromDataResult data_get_bytes(IntPtr data);

			// Token: 0x06000AF1 RID: 2801
			[DllImport("QuestPdfSkia", CallingConvention = 2)]
			public static extern void data_unref(IntPtr data);

			// Token: 0x0200021B RID: 539
			public struct GetBytesFromDataResult
			{
				// Token: 0x040007D3 RID: 2003
				public int length;

				// Token: 0x040007D4 RID: 2004
				public IntPtr bytes;
			}
		}
	}
}
