using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Runtime.CompilerServices;
using QuestPDF.Drawing.Exceptions;
using QuestPDF.Elements;
using QuestPDF.Infrastructure;

namespace QuestPDF.Fluent
{
	// Token: 0x0200008E RID: 142
	[NullableContext(1)]
	[Nullable(0)]
	public static class ImageExtensions
	{
		// Token: 0x060003E8 RID: 1000 RVA: 0x0000A82C File Offset: 0x00008A2C
		public static ImageDescriptor Image(this IContainer parent, byte[] imageData)
		{
			QuestPDF.Infrastructure.Image image = QuestPDF.Infrastructure.Image.FromBinaryData(imageData);
			image.IsShared = false;
			return parent.Image(image);
		}

		// Token: 0x060003E9 RID: 1001 RVA: 0x0000A850 File Offset: 0x00008A50
		public static ImageDescriptor Image(this IContainer parent, string filePath)
		{
			QuestPDF.Infrastructure.Image image = StaticImageCache.LoadFromCache(filePath);
			return parent.Image(image);
		}

		// Token: 0x060003EA RID: 1002 RVA: 0x0000A86C File Offset: 0x00008A6C
		public static ImageDescriptor Image(this IContainer parent, Stream fileStream)
		{
			QuestPDF.Infrastructure.Image image = QuestPDF.Infrastructure.Image.FromStream(fileStream);
			image.IsShared = false;
			return parent.Image(image);
		}

		// Token: 0x060003EB RID: 1003 RVA: 0x0000A890 File Offset: 0x00008A90
		public static ImageDescriptor Image(this IContainer parent, QuestPDF.Infrastructure.Image image)
		{
			if (image == null)
			{
				throw new DocumentComposeException("Cannot load or decode provided image.");
			}
			QuestPDF.Elements.Image image2 = new QuestPDF.Elements.Image
			{
				DocumentImage = image
			};
			AspectRatio aspectRatio = new AspectRatio
			{
				Child = image2
			};
			parent.Element(aspectRatio);
			AspectRatioOption bestAspectRatioOptionFromParent = ImageExtensions.GetBestAspectRatioOptionFromParent(parent);
			return new ImageDescriptor(image2, aspectRatio).SetAspectRatio(bestAspectRatioOptionFromParent);
		}

		// Token: 0x060003EC RID: 1004 RVA: 0x0000A8E4 File Offset: 0x00008AE4
		internal static AspectRatioOption GetBestAspectRatioOptionFromParent(IContainer container)
		{
			Constrained constrained = container as Constrained;
			if (constrained == null)
			{
				return AspectRatioOption.FitWidth;
			}
			bool flag = constrained.MinWidth != null || constrained.MaxWidth != null;
			bool flag2 = constrained.MinHeight != null || constrained.MaxHeight != null;
			if (flag && flag2)
			{
				return AspectRatioOption.FitArea;
			}
			if (flag)
			{
				return AspectRatioOption.FitWidth;
			}
			if (flag2)
			{
				return AspectRatioOption.FitHeight;
			}
			return AspectRatioOption.FitWidth;
		}

		// Token: 0x060003ED RID: 1005 RVA: 0x0000A960 File Offset: 0x00008B60
		public static DynamicImageDescriptor Image(this IContainer element, GenerateDynamicImageDelegate dynamicImageSource)
		{
			DynamicImage dynamicImage = new DynamicImage
			{
				Source = dynamicImageSource
			};
			element.Element(dynamicImage);
			return new DynamicImageDescriptor(dynamicImage);
		}

		// Token: 0x060003EE RID: 1006 RVA: 0x0000A988 File Offset: 0x00008B88
		public static DynamicImageDescriptor Image(this IContainer element, Func<ImageSize, byte[]> dynamicImageSource)
		{
			DynamicImage dynamicImage = new DynamicImage
			{
				Source = ((GenerateDynamicImageDelegatePayload payload) => dynamicImageSource.Invoke(payload.ImageSize))
			};
			element.Element(dynamicImage);
			return new DynamicImageDescriptor(dynamicImage);
		}

		// Token: 0x060003EF RID: 1007 RVA: 0x0000A9C8 File Offset: 0x00008BC8
		[Obsolete("This element has been changed since version 2023.5. Please use the Image method overload that takes the GenerateDynamicImageDelegate as an argument.")]
		[ExcludeFromCodeCoverage]
		public static void Image(this IContainer element, Func<Size, byte[]> imageSource)
		{
			element.Image((ImageSize x) => imageSource.Invoke(new Size((float)x.Width, (float)x.Height)));
		}

		// Token: 0x060003F0 RID: 1008 RVA: 0x0000A9F5 File Offset: 0x00008BF5
		[Obsolete("This element has been changed since version 2023.5. Please use the Image method overload that returns the ImageDescriptor object.")]
		[ExcludeFromCodeCoverage]
		public static void Image(this IContainer parent, byte[] imageData, ImageScaling scaling)
		{
			parent.Image(imageData).ApplyScaling(scaling);
		}

		// Token: 0x060003F1 RID: 1009 RVA: 0x0000AA04 File Offset: 0x00008C04
		[Obsolete("This element has been changed since version 2023.5. Please use the Image method overload that returns the ImageDescriptor object.")]
		[ExcludeFromCodeCoverage]
		public static void Image(this IContainer parent, string filePath, ImageScaling scaling)
		{
			parent.Image(filePath).ApplyScaling(scaling);
		}

		// Token: 0x060003F2 RID: 1010 RVA: 0x0000AA13 File Offset: 0x00008C13
		[Obsolete("This element has been changed since version 2023.5. Please use the Image method overload that returns the ImageDescriptor object.")]
		[ExcludeFromCodeCoverage]
		public static void Image(this IContainer parent, Stream fileStream, ImageScaling scaling)
		{
			parent.Image(fileStream).ApplyScaling(scaling);
		}

		// Token: 0x060003F3 RID: 1011 RVA: 0x0000AA22 File Offset: 0x00008C22
		internal static void ApplyScaling(this ImageDescriptor descriptor, ImageScaling scaling)
		{
			if (scaling == ImageScaling.Resize)
			{
				descriptor.FitUnproportionally();
				return;
			}
			if (scaling == ImageScaling.FitWidth)
			{
				descriptor.FitWidth();
				return;
			}
			if (scaling == ImageScaling.FitHeight)
			{
				descriptor.FitHeight();
				return;
			}
			if (scaling == ImageScaling.FitArea)
			{
				descriptor.FitArea();
			}
		}
	}
}
