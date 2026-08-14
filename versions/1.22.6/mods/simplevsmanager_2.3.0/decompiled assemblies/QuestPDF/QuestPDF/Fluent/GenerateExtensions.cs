using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using QuestPDF.Drawing;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using QuestPDF.Skia;

namespace QuestPDF.Fluent
{
	// Token: 0x02000089 RID: 137
	[NullableContext(1)]
	[Nullable(0)]
	public static class GenerateExtensions
	{
		// Token: 0x060003BD RID: 957 RVA: 0x0000A1A2 File Offset: 0x000083A2
		static GenerateExtensions()
		{
			GenerateExtensions.ClearGenerateAndShowFiles();
		}

		// Token: 0x060003BE RID: 958 RVA: 0x0000A1B3 File Offset: 0x000083B3
		internal static void Generate(this IDocument document, IDocumentCanvas documentCanvas)
		{
			DocumentGenerator.RenderDocument(documentCanvas, document, DocumentSettings.Default);
		}

		// Token: 0x060003BF RID: 959 RVA: 0x0000A1C4 File Offset: 0x000083C4
		private static void ClearGenerateAndShowFiles()
		{
			foreach (string text in Enumerable.Where<string>(Directory.GetFiles(TemporaryStorage.GetPath(), "QuestPDF Preview *"), (string x) => DateTime.UtcNow - new FileInfo(x).LastAccessTimeUtc > TimeSpan.FromHours(1.0)))
			{
				try
				{
					if (File.Exists(text))
					{
						File.Delete(text);
					}
				}
				catch
				{
				}
			}
		}

		// Token: 0x060003C0 RID: 960 RVA: 0x0000A258 File Offset: 0x00008458
		public static byte[] GeneratePdf(this IDocument document)
		{
			byte[] result;
			using (MemoryStream memoryStream = new MemoryStream())
			{
				document.GeneratePdf(memoryStream);
				result = memoryStream.ToArray();
			}
			return result;
		}

		// Token: 0x060003C1 RID: 961 RVA: 0x0000A298 File Offset: 0x00008498
		public static void GeneratePdf(this IDocument document, string filePath)
		{
			if (File.Exists(filePath))
			{
				File.Delete(filePath);
			}
			using (FileStream fileStream = File.Create(filePath))
			{
				document.GeneratePdf(fileStream);
			}
		}

		// Token: 0x060003C2 RID: 962 RVA: 0x0000A2E0 File Offset: 0x000084E0
		public static void GeneratePdf(this IDocument document, Stream stream)
		{
			using (SkWriteStream skWriteStream = new SkWriteStream(stream))
			{
				DocumentGenerator.GeneratePdf(skWriteStream, document);
				skWriteStream.Flush();
			}
		}

		// Token: 0x060003C3 RID: 963 RVA: 0x0000A320 File Offset: 0x00008520
		public static void GeneratePdfAndShow(this IDocument document)
		{
			string path = TemporaryStorage.GetPath();
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
			defaultInterpolatedStringHandler..ctor(5, 2);
			defaultInterpolatedStringHandler.AppendFormatted("QuestPDF Preview");
			defaultInterpolatedStringHandler.AppendLiteral(" ");
			defaultInterpolatedStringHandler.AppendFormatted<int>(GenerateExtensions.Random.Next());
			defaultInterpolatedStringHandler.AppendLiteral(".pdf");
			string filePath = Path.Combine(path, defaultInterpolatedStringHandler.ToStringAndClear());
			document.GeneratePdf(filePath);
			Helpers.OpenFileUsingDefaultProgram(filePath);
		}

		// Token: 0x060003C4 RID: 964 RVA: 0x0000A38C File Offset: 0x0000858C
		public static byte[] GenerateXps(this IDocument document)
		{
			byte[] result;
			using (MemoryStream memoryStream = new MemoryStream())
			{
				document.GenerateXps(memoryStream);
				result = memoryStream.ToArray();
			}
			return result;
		}

		// Token: 0x060003C5 RID: 965 RVA: 0x0000A3CC File Offset: 0x000085CC
		public static void GenerateXps(this IDocument document, string filePath)
		{
			if (File.Exists(filePath))
			{
				File.Delete(filePath);
			}
			using (FileStream fileStream = File.Create(filePath))
			{
				document.GenerateXps(fileStream);
			}
		}

		// Token: 0x060003C6 RID: 966 RVA: 0x0000A414 File Offset: 0x00008614
		public static void GenerateXps(this IDocument document, Stream stream)
		{
			if (!RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
			{
				throw new PlatformNotSupportedException("XPS generation is only supported on the Windows platform.");
			}
			using (SkWriteStream skWriteStream = new SkWriteStream(stream))
			{
				DocumentGenerator.GenerateXps(skWriteStream, document);
				skWriteStream.Flush();
			}
		}

		// Token: 0x060003C7 RID: 967 RVA: 0x0000A468 File Offset: 0x00008668
		public static void GenerateXpsAndShow(this IDocument document)
		{
			string path = TemporaryStorage.GetPath();
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
			defaultInterpolatedStringHandler..ctor(5, 2);
			defaultInterpolatedStringHandler.AppendFormatted("QuestPDF Preview");
			defaultInterpolatedStringHandler.AppendLiteral(" ");
			defaultInterpolatedStringHandler.AppendFormatted<int>(GenerateExtensions.Random.Next());
			defaultInterpolatedStringHandler.AppendLiteral(".xps");
			string filePath = Path.Combine(path, defaultInterpolatedStringHandler.ToStringAndClear());
			document.GenerateXps(filePath);
			Helpers.OpenFileUsingDefaultProgram(filePath);
		}

		// Token: 0x060003C8 RID: 968 RVA: 0x0000A4D2 File Offset: 0x000086D2
		public static IEnumerable<byte[]> GenerateImages(this IDocument document, [Nullable(2)] ImageGenerationSettings settings = null)
		{
			if (settings == null)
			{
				settings = ImageGenerationSettings.Default;
			}
			return DocumentGenerator.GenerateImages(document, settings);
		}

		// Token: 0x060003C9 RID: 969 RVA: 0x0000A4E8 File Offset: 0x000086E8
		public static void GenerateImages(this IDocument document, GenerateExtensions.GenerateDocumentImagePath imagePathSource, [Nullable(2)] ImageGenerationSettings settings = null)
		{
			if (settings == null)
			{
				settings = ImageGenerationSettings.Default;
			}
			int num = 0;
			foreach (byte[] array in document.GenerateImages(settings))
			{
				string text = imagePathSource(num);
				if (File.Exists(text))
				{
					File.Delete(text);
				}
				File.WriteAllBytes(text, array);
				num++;
			}
		}

		// Token: 0x060003CA RID: 970 RVA: 0x0000A55C File Offset: 0x0000875C
		public static ICollection<string> GenerateSvg(this IDocument document)
		{
			return DocumentGenerator.GenerateSvg(document);
		}

		// Token: 0x04000273 RID: 627
		private static readonly Random Random = new Random();

		// Token: 0x04000274 RID: 628
		private const string GenerateAndShowNamePrefix = "QuestPDF Preview";

		// Token: 0x02000190 RID: 400
		// (Invoke) Token: 0x06000C43 RID: 3139
		[NullableContext(0)]
		public delegate string GenerateDocumentImagePath(int imageIndex);
	}
}
