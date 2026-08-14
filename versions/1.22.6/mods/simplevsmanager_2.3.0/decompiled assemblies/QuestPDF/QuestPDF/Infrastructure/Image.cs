using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using QuestPDF.Helpers;
using QuestPDF.Skia;

namespace QuestPDF.Infrastructure
{
	// Token: 0x02000052 RID: 82
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class Image : IDisposable
	{
		// Token: 0x060001F5 RID: 501 RVA: 0x000051A1 File Offset: 0x000033A1
		static Image()
		{
			SkNativeDependencyCompatibilityChecker.Test();
		}

		// Token: 0x17000080 RID: 128
		// (get) Token: 0x060001F6 RID: 502 RVA: 0x000051A8 File Offset: 0x000033A8
		// (set) Token: 0x060001F7 RID: 503 RVA: 0x000051B0 File Offset: 0x000033B0
		internal bool IsShared { get; set; } = true;

		// Token: 0x17000081 RID: 129
		// (get) Token: 0x060001F8 RID: 504 RVA: 0x000051B9 File Offset: 0x000033B9
		internal SkImage SkImage { get; }

		// Token: 0x17000082 RID: 130
		// (get) Token: 0x060001F9 RID: 505 RVA: 0x000051C1 File Offset: 0x000033C1
		internal ImageSize Size { get; }

		// Token: 0x17000083 RID: 131
		// (get) Token: 0x060001FA RID: 506 RVA: 0x000051C9 File Offset: 0x000033C9
		[TupleElementNames(new string[]
		{
			"request",
			"image"
		})]
		[Nullable(new byte[]
		{
			1,
			0,
			1,
			1
		})]
		internal LinkedList<ValueTuple<GetImageVersionRequest, SkImage>> ScaledImageCache { [return: TupleElementNames(new string[]
		{
			"request",
			"image"
		})] [return: Nullable(new byte[]
		{
			1,
			0,
			1,
			1
		})] get; } = new LinkedList<ValueTuple<GetImageVersionRequest, SkImage>>();

		// Token: 0x060001FB RID: 507 RVA: 0x000051D1 File Offset: 0x000033D1
		internal Image(SkImage image)
		{
			this.SkImage = image;
			this.Size = new ImageSize(image.Width, image.Height);
		}

		// Token: 0x060001FC RID: 508 RVA: 0x0000520C File Offset: 0x0000340C
		~Image()
		{
			this.WarnThatFinalizerIsReached<Image>();
			this.Dispose();
		}

		// Token: 0x060001FD RID: 509 RVA: 0x00005240 File Offset: 0x00003440
		public void Dispose()
		{
			SkImage skImage = this.SkImage;
			if (skImage != null)
			{
				skImage.Dispose();
			}
			foreach (ValueTuple<GetImageVersionRequest, SkImage> valueTuple in this.ScaledImageCache)
			{
				SkImage item = valueTuple.Item2;
				if (item != null)
				{
					item.Dispose();
				}
			}
			GC.SuppressFinalize(this);
		}

		// Token: 0x060001FE RID: 510 RVA: 0x000052B4 File Offset: 0x000034B4
		internal SkImage GetVersionOfSize(GetImageVersionRequest request)
		{
			foreach (ValueTuple<GetImageVersionRequest, SkImage> valueTuple in this.ScaledImageCache)
			{
				if (valueTuple.Item1 == request)
				{
					return valueTuple.Item2;
				}
			}
			SkImage skImage = this.SkImage.ResizeAndCompressImage(request.Resolution, request.CompressionQuality);
			this.ScaledImageCache.AddLast(new ValueTuple<GetImageVersionRequest, SkImage>(request, skImage));
			return skImage;
		}

		// Token: 0x060001FF RID: 511 RVA: 0x00005348 File Offset: 0x00003548
		public static Image FromBinaryData(byte[] imageBytes)
		{
			Image result;
			using (SkData skData = SkData.FromBinary(imageBytes))
			{
				result = StaticImageCache.DecodeImage(skData, true);
			}
			return result;
		}

		// Token: 0x06000200 RID: 512 RVA: 0x00005384 File Offset: 0x00003584
		public static Image FromFile(string filePath)
		{
			return StaticImageCache.DirectlyLoadFromFile(filePath, true);
		}

		// Token: 0x06000201 RID: 513 RVA: 0x00005390 File Offset: 0x00003590
		public static Image FromStream(Stream stream)
		{
			Image result;
			using (SkData skData = SkData.FromStream(stream))
			{
				result = StaticImageCache.DecodeImage(skData, true);
			}
			return result;
		}
	}
}
