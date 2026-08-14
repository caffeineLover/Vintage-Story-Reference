using System;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using QuestPDF.Drawing;
using QuestPDF.Infrastructure;
using QuestPDF.Skia;

namespace QuestPDF.Helpers
{
	// Token: 0x02000074 RID: 116
	[NullableContext(1)]
	[Nullable(0)]
	internal static class Helpers
	{
		// Token: 0x060002C7 RID: 711 RVA: 0x000073C7 File Offset: 0x000055C7
		static Helpers()
		{
			SkNativeDependencyCompatibilityChecker.Test();
		}

		// Token: 0x060002C8 RID: 712 RVA: 0x000073CE File Offset: 0x000055CE
		internal static string PrettifyName(this string text)
		{
			return Regex.Replace(text, "([a-z])([A-Z])", "$1 $2", 8);
		}

		// Token: 0x060002C9 RID: 713 RVA: 0x000073E4 File Offset: 0x000055E4
		[NullableContext(2)]
		internal static void VisitChildren(this Element root, [Nullable(new byte[]
		{
			1,
			2
		})] Action<Element> handler)
		{
			Helpers.<>c__DisplayClass2_0 CS$<>8__locals1;
			CS$<>8__locals1.handler = handler;
			Helpers.<VisitChildren>g__Traverse|2_0(root, ref CS$<>8__locals1);
		}

		// Token: 0x060002CA RID: 714 RVA: 0x00007401 File Offset: 0x00005601
		[NullableContext(2)]
		internal static void ReleaseDisposableChildren(this Element element)
		{
			element.VisitChildren(delegate(Element x)
			{
				IDisposable disposable = x as IDisposable;
				if (disposable == null)
				{
					return;
				}
				disposable.Dispose();
			});
		}

		// Token: 0x060002CB RID: 715 RVA: 0x00007428 File Offset: 0x00005628
		internal static bool IsGreaterThan(this float first, float second)
		{
			return first > second + 0.01f;
		}

		// Token: 0x060002CC RID: 716 RVA: 0x00007434 File Offset: 0x00005634
		internal static bool IsLessThan(this float first, float second)
		{
			return first < second - 0.01f;
		}

		// Token: 0x060002CD RID: 717 RVA: 0x00007440 File Offset: 0x00005640
		public static bool AreClose(double a, double b)
		{
			return Math.Abs(a - b) <= 0.009999999776482582;
		}

		// Token: 0x060002CE RID: 718 RVA: 0x00007458 File Offset: 0x00005658
		internal static bool IsNegative(this Size size)
		{
			return size.Width < -0.01f || size.Height < -0.01f;
		}

		// Token: 0x060002CF RID: 719 RVA: 0x00007476 File Offset: 0x00005676
		internal static bool IsCloseToZero(this Size size)
		{
			return Math.Abs(size.Width) < 0.01f && Math.Abs(size.Height) < 0.01f;
		}

		// Token: 0x060002D0 RID: 720 RVA: 0x0000749E File Offset: 0x0000569E
		internal static bool IsEmpty(this Element element)
		{
			return element.Measure(Size.Zero).Type == SpacePlanType.Empty;
		}

		// Token: 0x060002D1 RID: 721 RVA: 0x000074B4 File Offset: 0x000056B4
		internal static int ToQualityValue(this ImageCompressionQuality quality)
		{
			int result;
			switch (quality)
			{
			case ImageCompressionQuality.Best:
				result = 100;
				break;
			case ImageCompressionQuality.VeryHigh:
				result = 90;
				break;
			case ImageCompressionQuality.High:
				result = 75;
				break;
			case ImageCompressionQuality.Medium:
				result = 50;
				break;
			case ImageCompressionQuality.Low:
				result = 25;
				break;
			case ImageCompressionQuality.VeryLow:
				result = 10;
				break;
			default:
				throw new ArgumentOutOfRangeException("quality", quality, null);
			}
			return result;
		}

		// Token: 0x060002D2 RID: 722 RVA: 0x00007514 File Offset: 0x00005714
		internal static bool ToDownsamplingStrategy(this ImageCompressionQuality quality)
		{
			bool result;
			switch (quality)
			{
			case ImageCompressionQuality.Best:
				result = false;
				break;
			case ImageCompressionQuality.VeryHigh:
				result = false;
				break;
			case ImageCompressionQuality.High:
				result = true;
				break;
			case ImageCompressionQuality.Medium:
				result = true;
				break;
			case ImageCompressionQuality.Low:
				result = true;
				break;
			case ImageCompressionQuality.VeryLow:
				result = true;
				break;
			default:
				throw new ArgumentOutOfRangeException("quality", quality, null);
			}
			return result;
		}

		// Token: 0x060002D3 RID: 723 RVA: 0x0000756C File Offset: 0x0000576C
		internal static SkImage CompressImage(this SkImage image, ImageCompressionQuality compressionQuality)
		{
			return image.ResizeAndCompress(image.Width, image.Height, compressionQuality.ToQualityValue(), compressionQuality.ToDownsamplingStrategy());
		}

		// Token: 0x060002D4 RID: 724 RVA: 0x0000758C File Offset: 0x0000578C
		internal static SkImage ResizeAndCompressImage(this SkImage image, ImageSize targetResolution, ImageCompressionQuality compressionQuality)
		{
			if (targetResolution.Width == 0 || targetResolution.Height == 0)
			{
				targetResolution = new ImageSize(1, 1);
			}
			return image.ResizeAndCompress(targetResolution.Width, targetResolution.Height, compressionQuality.ToQualityValue(), compressionQuality.ToDownsamplingStrategy());
		}

		// Token: 0x060002D5 RID: 725 RVA: 0x000075C5 File Offset: 0x000057C5
		internal static SkImage GetImageWithSmallerSize(SkImage one, SkImage second)
		{
			if (one.EncodedDataSize >= second.EncodedDataSize)
			{
				return second;
			}
			return one;
		}

		// Token: 0x060002D6 RID: 726 RVA: 0x000075D8 File Offset: 0x000057D8
		internal static void OpenFileUsingDefaultProgram(string filePath)
		{
			Process process = new Process();
			process.StartInfo = new ProcessStartInfo(filePath)
			{
				UseShellExecute = true
			};
			process.Start();
			process.WaitForExit();
		}

		// Token: 0x170000C1 RID: 193
		// (get) Token: 0x060002D7 RID: 727 RVA: 0x00007600 File Offset: 0x00005800
		internal static string ApplicationFilesPath
		{
			get
			{
				string baseDirectory = AppContext.BaseDirectory;
				if (string.IsNullOrWhiteSpace(baseDirectory) || baseDirectory == "/")
				{
					return Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
				}
				return baseDirectory;
			}
		}

		// Token: 0x060002D8 RID: 728 RVA: 0x0000763C File Offset: 0x0000583C
		[NullableContext(0)]
		[return: TupleElementNames(new string[]
		{
			"widthScale",
			"heightScale"
		})]
		internal static ValueTuple<float, float> CalculateSpaceScale([Nullable(1)] this SkSvgImage image, Size availableSpace)
		{
			float num = Helpers.<CalculateSpaceScale>g__CalculateDimensionScale|18_0(availableSpace.Width, image.Size.Width, image.Size.WidthUnit);
			float num2 = Helpers.<CalculateSpaceScale>g__CalculateDimensionScale|18_0(availableSpace.Height, image.Size.Height, image.Size.HeightUnit);
			return new ValueTuple<float, float>(num, num2);
		}

		// Token: 0x060002D9 RID: 729 RVA: 0x00007692 File Offset: 0x00005892
		public static string FormatAsCompanionNumber(this float value)
		{
			return value.ToString("0.#", CultureInfo.InvariantCulture);
		}

		// Token: 0x060002DA RID: 730 RVA: 0x000076A8 File Offset: 0x000058A8
		[NullableContext(2)]
		[CompilerGenerated]
		internal static void <VisitChildren>g__Traverse|2_0(Element element, ref Helpers.<>c__DisplayClass2_0 A_1)
		{
			if (element == null)
			{
				return;
			}
			ContainerElement containerElement = element as ContainerElement;
			if (containerElement != null)
			{
				Helpers.<VisitChildren>g__Traverse|2_0(containerElement.Child, ref A_1);
			}
			else
			{
				foreach (Element element2 in element.GetChildren())
				{
					Helpers.<VisitChildren>g__Traverse|2_0(element2, ref A_1);
				}
			}
			A_1.handler.Invoke(element);
		}

		// Token: 0x060002DB RID: 731 RVA: 0x0000771C File Offset: 0x0000591C
		[CompilerGenerated]
		internal static float <CalculateSpaceScale>g__CalculateDimensionScale|18_0(float availableSize, float imageSize, SkSvgImageSize.Unit unit)
		{
			if (unit == SkSvgImageSize.Unit.Percentage)
			{
				return 100f / imageSize;
			}
			bool flag = unit - SkSvgImageSize.Unit.Centimeters <= 4;
			if (flag)
			{
				return availableSize / Helpers.<CalculateSpaceScale>g__ConvertToPoints|18_1(imageSize, unit);
			}
			return availableSize / imageSize;
		}

		// Token: 0x060002DC RID: 732 RVA: 0x00007754 File Offset: 0x00005954
		[CompilerGenerated]
		internal static float <CalculateSpaceScale>g__ConvertToPoints|18_1(float value, SkSvgImageSize.Unit unit)
		{
			float num;
			switch (unit)
			{
			case SkSvgImageSize.Unit.Centimeters:
				num = value / 2.54f * 72f;
				break;
			case SkSvgImageSize.Unit.Millimeters:
				num = value / 10f / 2.54f * 72f;
				break;
			case SkSvgImageSize.Unit.Inches:
				num = value * 72f;
				break;
			case SkSvgImageSize.Unit.Points:
				num = value;
				break;
			case SkSvgImageSize.Unit.Picas:
				num = value * 12f;
				break;
			default:
				throw new ArgumentOutOfRangeException();
			}
			return num * 1.25f;
		}
	}
}
