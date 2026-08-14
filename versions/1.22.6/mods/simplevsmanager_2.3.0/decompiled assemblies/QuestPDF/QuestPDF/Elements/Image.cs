using System;
using System.Runtime.CompilerServices;
using QuestPDF.Drawing;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using QuestPDF.Skia;

namespace QuestPDF.Elements
{
	// Token: 0x020000D1 RID: 209
	[NullableContext(2)]
	[Nullable(0)]
	internal sealed class Image : Element, IStateful, IDisposable
	{
		// Token: 0x1700016E RID: 366
		// (get) Token: 0x06000673 RID: 1651 RVA: 0x0000FB45 File Offset: 0x0000DD45
		// (set) Token: 0x06000674 RID: 1652 RVA: 0x0000FB4D File Offset: 0x0000DD4D
		public QuestPDF.Infrastructure.Image DocumentImage { get; set; }

		// Token: 0x1700016F RID: 367
		// (get) Token: 0x06000675 RID: 1653 RVA: 0x0000FB56 File Offset: 0x0000DD56
		// (set) Token: 0x06000676 RID: 1654 RVA: 0x0000FB5E File Offset: 0x0000DD5E
		internal bool UseOriginalImage { get; set; }

		// Token: 0x17000170 RID: 368
		// (get) Token: 0x06000677 RID: 1655 RVA: 0x0000FB67 File Offset: 0x0000DD67
		// (set) Token: 0x06000678 RID: 1656 RVA: 0x0000FB6F File Offset: 0x0000DD6F
		internal int? TargetDpi { get; set; }

		// Token: 0x17000171 RID: 369
		// (get) Token: 0x06000679 RID: 1657 RVA: 0x0000FB78 File Offset: 0x0000DD78
		// (set) Token: 0x0600067A RID: 1658 RVA: 0x0000FB80 File Offset: 0x0000DD80
		internal ImageCompressionQuality? CompressionQuality { get; set; }

		// Token: 0x17000172 RID: 370
		// (get) Token: 0x0600067B RID: 1659 RVA: 0x0000FB89 File Offset: 0x0000DD89
		// (set) Token: 0x0600067C RID: 1660 RVA: 0x0000FB91 File Offset: 0x0000DD91
		private int DrawnImageSize { get; set; }

		// Token: 0x0600067D RID: 1661 RVA: 0x0000FB9C File Offset: 0x0000DD9C
		~Image()
		{
			this.WarnThatFinalizerIsReached<Image>();
			this.Dispose();
		}

		// Token: 0x0600067E RID: 1662 RVA: 0x0000FBD0 File Offset: 0x0000DDD0
		public void Dispose()
		{
			if (this.DocumentImage != null && !this.DocumentImage.IsShared)
			{
				QuestPDF.Infrastructure.Image documentImage = this.DocumentImage;
				if (documentImage != null)
				{
					documentImage.Dispose();
				}
			}
			GC.SuppressFinalize(this);
		}

		// Token: 0x0600067F RID: 1663 RVA: 0x0000FBFE File Offset: 0x0000DDFE
		internal override SpacePlan Measure(Size availableSpace)
		{
			if (this.IsRendered)
			{
				return SpacePlan.Empty();
			}
			if (availableSpace.IsNegative())
			{
				return SpacePlan.Wrap("The available space is negative.");
			}
			return SpacePlan.FullRender(Size.Zero);
		}

		// Token: 0x06000680 RID: 1664 RVA: 0x0000FC2C File Offset: 0x0000DE2C
		internal override void Draw(Size availableSpace)
		{
			if (this.DocumentImage == null)
			{
				return;
			}
			if (this.IsRendered)
			{
				return;
			}
			SkImage imageToDraw = this.GetImageToDraw(availableSpace);
			base.Canvas.DrawImage(imageToDraw, availableSpace);
			this.DrawnImageSize = Math.Max(this.DrawnImageSize, imageToDraw.EncodedDataSize);
			this.IsRendered = true;
		}

		// Token: 0x06000681 RID: 1665 RVA: 0x0000FC80 File Offset: 0x0000DE80
		[NullableContext(1)]
		private SkImage GetImageToDraw(Size availableSpace)
		{
			SkImage skImage = this.DocumentImage.SkImage;
			if (this.UseOriginalImage)
			{
				return skImage;
			}
			GetImageVersionRequest request = new GetImageVersionRequest
			{
				Resolution = Image.GetTargetResolution(this.DocumentImage.Size, availableSpace, this.TargetDpi.Value),
				CompressionQuality = this.CompressionQuality.Value
			};
			SkImage versionOfSize = this.DocumentImage.GetVersionOfSize(request);
			return Helpers.GetImageWithSmallerSize(skImage, versionOfSize);
		}

		// Token: 0x06000682 RID: 1666 RVA: 0x0000FCF8 File Offset: 0x0000DEF8
		private static ImageSize GetTargetResolution(ImageSize imageResolution, Size availableAreaSize, int targetDpi)
		{
			float num = (float)targetDpi / 72f;
			ImageSize imageSize = new ImageSize((int)(availableAreaSize.Width * num), (int)(availableAreaSize.Height * num));
			if (imageResolution.Width < imageSize.Width || imageResolution.Height < imageSize.Height)
			{
				return imageResolution;
			}
			return imageSize;
		}

		// Token: 0x17000173 RID: 371
		// (get) Token: 0x06000683 RID: 1667 RVA: 0x0000FD4B File Offset: 0x0000DF4B
		// (set) Token: 0x06000684 RID: 1668 RVA: 0x0000FD53 File Offset: 0x0000DF53
		private bool IsRendered { get; set; }

		// Token: 0x06000685 RID: 1669 RVA: 0x0000FD5C File Offset: 0x0000DF5C
		public void ResetState(bool hardReset = false)
		{
			this.IsRendered = false;
		}

		// Token: 0x06000686 RID: 1670 RVA: 0x0000FD65 File Offset: 0x0000DF65
		[NullableContext(1)]
		public object GetState()
		{
			return this.IsRendered;
		}

		// Token: 0x06000687 RID: 1671 RVA: 0x0000FD72 File Offset: 0x0000DF72
		[NullableContext(1)]
		public void SetState(object state)
		{
			this.IsRendered = (bool)state;
		}

		// Token: 0x06000688 RID: 1672 RVA: 0x0000FD80 File Offset: 0x0000DF80
		internal override string GetCompanionHint()
		{
			int num = Math.Max(1, this.DrawnImageSize / 1024);
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
			defaultInterpolatedStringHandler..ctor(2, 1);
			defaultInterpolatedStringHandler.AppendFormatted<int>(num);
			defaultInterpolatedStringHandler.AppendLiteral("KB");
			return defaultInterpolatedStringHandler.ToStringAndClear();
		}
	}
}
