using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace QuestPDF.Skia
{
	// Token: 0x0200000E RID: 14
	[NullableContext(1)]
	[Nullable(0)]
	internal sealed class SkImage : IDisposable
	{
		// Token: 0x17000013 RID: 19
		// (get) Token: 0x0600005C RID: 92 RVA: 0x00002AA3 File Offset: 0x00000CA3
		// (set) Token: 0x0600005D RID: 93 RVA: 0x00002AAB File Offset: 0x00000CAB
		public IntPtr Instance { get; private set; }

		// Token: 0x0600005E RID: 94 RVA: 0x00002AB4 File Offset: 0x00000CB4
		public SkImage(IntPtr instance)
		{
			this.Instance = instance;
			SkiaAPI.EnsureNotNull(this.Instance);
			SkImage.API.SkImageDetails skImageDetails = SkImage.API.image_get_details(this.Instance);
			this.Width = skImageDetails.Width;
			this.Height = skImageDetails.Height;
			this.EncodedDataSize = skImageDetails.EncodedDataSize;
			GC.AddMemoryPressure((long)this.EncodedDataSize);
		}

		// Token: 0x0600005F RID: 95 RVA: 0x00002B15 File Offset: 0x00000D15
		public static SkImage FromData(SkData data)
		{
			IntPtr intPtr = SkImage.API.image_create_from_data(data.Instance);
			if (intPtr == IntPtr.Zero)
			{
				throw new Exception("Cannot decode the provided image.");
			}
			return new SkImage(intPtr);
		}

		// Token: 0x06000060 RID: 96 RVA: 0x00002B3A File Offset: 0x00000D3A
		public SkImage ResizeAndCompress(int targetWidth, int targetHeight, int compressionQuality, bool downsample)
		{
			return new SkImage(SkImage.API.image_resize_and_compress(this.Instance, targetWidth, targetHeight, compressionQuality, downsample));
		}

		// Token: 0x06000061 RID: 97 RVA: 0x00002B51 File Offset: 0x00000D51
		public static SkImage GeneratePlaceholder(int targetWidth, int targetHeight, uint firstColor, uint secondColor)
		{
			return new SkImage(SkImage.API.image_generate_placeholder(targetWidth, targetHeight, firstColor, secondColor));
		}

		// Token: 0x06000062 RID: 98 RVA: 0x00002B61 File Offset: 0x00000D61
		public SkData GetEncodedData()
		{
			return new SkData(SkImage.API.image_get_encoded_data(this.Instance));
		}

		// Token: 0x06000063 RID: 99 RVA: 0x00002B74 File Offset: 0x00000D74
		~SkImage()
		{
			this.WarnThatFinalizerIsReached<SkImage>();
			this.Dispose();
		}

		// Token: 0x06000064 RID: 100 RVA: 0x00002BA8 File Offset: 0x00000DA8
		public void Dispose()
		{
			if (this.Instance == IntPtr.Zero)
			{
				return;
			}
			SkImage.API.image_unref(this.Instance);
			this.Instance = IntPtr.Zero;
			GC.SuppressFinalize(this);
			GC.RemoveMemoryPressure((long)this.EncodedDataSize);
		}

		// Token: 0x0400002B RID: 43
		public readonly int Width;

		// Token: 0x0400002C RID: 44
		public readonly int Height;

		// Token: 0x0400002D RID: 45
		public readonly int EncodedDataSize;

		// Token: 0x02000136 RID: 310
		[NullableContext(0)]
		private static class API
		{
			// Token: 0x06000AF6 RID: 2806
			[DllImport("QuestPdfSkia", CallingConvention = 2)]
			public static extern IntPtr image_create_from_data(IntPtr data);

			// Token: 0x06000AF7 RID: 2807
			[DllImport("QuestPdfSkia", CallingConvention = 2)]
			public static extern void image_unref(IntPtr image);

			// Token: 0x06000AF8 RID: 2808
			[DllImport("QuestPdfSkia", CallingConvention = 2)]
			public static extern IntPtr image_resize_and_compress(IntPtr image, int targetImageWidth, int targetImageHeight, int compressionQuality, bool downsample);

			// Token: 0x06000AF9 RID: 2809
			[DllImport("QuestPdfSkia", CallingConvention = 2)]
			public static extern SkImage.API.SkImageDetails image_get_details(IntPtr image);

			// Token: 0x06000AFA RID: 2810
			[DllImport("QuestPdfSkia", CallingConvention = 2)]
			public static extern IntPtr image_get_encoded_data(IntPtr image);

			// Token: 0x06000AFB RID: 2811
			[DllImport("QuestPdfSkia", CallingConvention = 2)]
			public static extern IntPtr image_generate_placeholder(int imageWidth, int imageHeight, uint firstColor, uint secondColor);

			// Token: 0x0200021C RID: 540
			public struct SkImageDetails
			{
				// Token: 0x040007D5 RID: 2005
				public int Width;

				// Token: 0x040007D6 RID: 2006
				public int Height;

				// Token: 0x040007D7 RID: 2007
				public int EncodedDataSize;
			}
		}
	}
}
