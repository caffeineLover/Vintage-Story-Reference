using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using QuestPDF.Drawing.Exceptions;
using QuestPDF.Elements;
using QuestPDF.Infrastructure;
using QuestPDF.Skia;

namespace QuestPDF.Fluent
{
	// Token: 0x02000087 RID: 135
	[NullableContext(1)]
	[Nullable(0)]
	public static class ElementExtensions
	{
		// Token: 0x06000395 RID: 917 RVA: 0x00009D94 File Offset: 0x00007F94
		static ElementExtensions()
		{
			SkNativeDependencyCompatibilityChecker.Test();
		}

		// Token: 0x06000396 RID: 918 RVA: 0x00009D9C File Offset: 0x00007F9C
		internal static T Element<[Nullable(0)] T>(this IContainer element, T child) where T : IElement
		{
			if (((element != null) ? element.Child : null) != null && !(element.Child is Empty))
			{
				throw new DocumentComposeException("You should not assign multiple child elements to a single-child container. This may happen when a container variable is used outside of its scope/closure OR the container is used in multiple fluent API chains OR the container is used incorrectly in a loop. This exception is thrown to help you detect that some part of the code is overriding fragments of the document layout with a new content - essentially destroying existing content.");
			}
			if (element != child as Element)
			{
				element.Child = (child as Element);
			}
			Element element2 = child as Element;
			if (element2 != null)
			{
				element2.CodeLocation = SourceCodePath.CreateFromCurrentStackTrace();
			}
			return child;
		}

		// Token: 0x06000397 RID: 919 RVA: 0x00009E0C File Offset: 0x0000800C
		public static void Element(this IContainer parent, Action<IContainer> handler, [CallerArgumentExpression("handler")] string handlerName = null, [CallerMemberName] string parentName = "", [CallerFilePath] string sourceFilePath = "", [CallerLineNumber] int sourceLineNumber = 0)
		{
			SourceCodePointer sourceCodePointer = parent.Container().Element(new SourceCodePointer
			{
				MethodName = handlerName,
				CalledFrom = parentName,
				FilePath = sourceFilePath,
				LineNumber = sourceLineNumber
			});
			handler.Invoke(sourceCodePointer);
		}

		// Token: 0x06000398 RID: 920 RVA: 0x00009E50 File Offset: 0x00008050
		public static IContainer Element(this IContainer parent, Func<IContainer, IContainer> handler, [CallerArgumentExpression("handler")] string handlerName = null, [CallerMemberName] string parentName = "", [CallerFilePath] string sourceFilePath = "", [CallerLineNumber] int sourceLineNumber = 0)
		{
			SourceCodePointer sourceCodePointer = parent.Element(new SourceCodePointer
			{
				MethodName = handlerName,
				CalledFrom = parentName,
				FilePath = sourceFilePath,
				LineNumber = sourceLineNumber
			});
			return handler.Invoke(sourceCodePointer);
		}

		// Token: 0x06000399 RID: 921 RVA: 0x00009E8E File Offset: 0x0000808E
		public static void Element(this IContainer parent, IContainer child)
		{
			parent.Child = (child as IElement);
		}

		// Token: 0x0600039A RID: 922 RVA: 0x00009E9C File Offset: 0x0000809C
		internal static IContainer NonTrackingElement(this IContainer parent, Func<IContainer, IContainer> handler)
		{
			return handler.Invoke(parent.Container());
		}

		// Token: 0x0600039B RID: 923 RVA: 0x00009EAA File Offset: 0x000080AA
		public static IContainer AspectRatio(this IContainer element, float ratio, AspectRatioOption option = AspectRatioOption.FitWidth)
		{
			if (ratio <= 0f)
			{
				throw new ArgumentOutOfRangeException("ratio", "The aspect ratio must be greater than zero.");
			}
			return element.Element(new AspectRatio
			{
				Ratio = ratio,
				Option = option
			});
		}

		// Token: 0x0600039C RID: 924 RVA: 0x00009EDD File Offset: 0x000080DD
		public static void Placeholder(this IContainer element, [Nullable(2)] string text = null)
		{
			element.Component(new Placeholder
			{
				Text = (text ?? string.Empty)
			});
		}

		// Token: 0x0600039D RID: 925 RVA: 0x00009EFA File Offset: 0x000080FA
		public static IContainer ShowOnce(this IContainer element)
		{
			return element.Element(new ShowOnce());
		}

		// Token: 0x0600039E RID: 926 RVA: 0x00009F07 File Offset: 0x00008107
		public static IContainer SkipOnce(this IContainer element)
		{
			return element.Element(new SkipOnce());
		}

		// Token: 0x0600039F RID: 927 RVA: 0x00009F14 File Offset: 0x00008114
		public static IContainer ShowEntire(this IContainer element)
		{
			return element.Element(new ShowEntire());
		}

		// Token: 0x060003A0 RID: 928 RVA: 0x00009F21 File Offset: 0x00008121
		public static IContainer EnsureSpace(this IContainer element, float minHeight = 150f)
		{
			if (minHeight < 0f)
			{
				throw new ArgumentOutOfRangeException("minHeight", "The EnsureSpace minimum height cannot be negative.");
			}
			return element.Element(new EnsureSpace
			{
				MinHeight = minHeight
			});
		}

		// Token: 0x060003A1 RID: 929 RVA: 0x00009F4D File Offset: 0x0000814D
		public static IContainer PreventPageBreak(this IContainer element)
		{
			return element.Element(new PreventPageBreak());
		}

		// Token: 0x060003A2 RID: 930 RVA: 0x00009F5A File Offset: 0x0000815A
		public static void PageBreak(this IContainer element)
		{
			element.Element(new PageBreak());
		}

		// Token: 0x060003A3 RID: 931 RVA: 0x00009F68 File Offset: 0x00008168
		public static IContainer Container(this IContainer element)
		{
			return element.Element(new Container());
		}

		// Token: 0x060003A4 RID: 932 RVA: 0x00009F75 File Offset: 0x00008175
		[Obsolete("This element has been renamed since version 2022.3. Please use the Hyperlink method.")]
		[ExcludeFromCodeCoverage]
		public static IContainer ExternalLink(this IContainer element, string url)
		{
			return element.Hyperlink(url);
		}

		// Token: 0x060003A5 RID: 933 RVA: 0x00009F7E File Offset: 0x0000817E
		public static IContainer Hyperlink(this IContainer element, string url)
		{
			if (string.IsNullOrWhiteSpace(url))
			{
				throw new ArgumentException("The URL cannot be null or whitespace.", "url");
			}
			return element.Element(new Hyperlink
			{
				Url = url
			});
		}

		// Token: 0x060003A6 RID: 934 RVA: 0x00009FAA File Offset: 0x000081AA
		[Obsolete("This element has been renamed since version 2022.3. Please use the Section method.")]
		[ExcludeFromCodeCoverage]
		public static IContainer Location(this IContainer element, string locationName)
		{
			return element.Section(locationName);
		}

		// Token: 0x060003A7 RID: 935 RVA: 0x00009FB3 File Offset: 0x000081B3
		public static IContainer Section(this IContainer element, string sectionName)
		{
			if (string.IsNullOrWhiteSpace(sectionName))
			{
				throw new ArgumentException("The section name cannot be null or whitespace.", "sectionName");
			}
			return element.DebugPointer(DebugPointerType.Section, sectionName).Element(new Section
			{
				SectionName = sectionName
			});
		}

		// Token: 0x060003A8 RID: 936 RVA: 0x00009FE6 File Offset: 0x000081E6
		[Obsolete("This element has been renamed since version 2022.3. Please use the SectionLink method.")]
		[ExcludeFromCodeCoverage]
		public static IContainer InternalLink(this IContainer element, string locationName)
		{
			return element.SectionLink(locationName);
		}

		// Token: 0x060003A9 RID: 937 RVA: 0x00009FEF File Offset: 0x000081EF
		public static IContainer SectionLink(this IContainer element, string sectionName)
		{
			if (string.IsNullOrWhiteSpace(sectionName))
			{
				throw new ArgumentException("The section name cannot be null or whitespace.", "sectionName");
			}
			return element.Element(new SectionLink
			{
				SectionName = sectionName
			});
		}

		// Token: 0x060003AA RID: 938 RVA: 0x0000A01C File Offset: 0x0000821C
		public static IContainer ShowIf(this IContainer element, bool condition)
		{
			if (!condition)
			{
				return new Container();
			}
			return element;
		}

		// Token: 0x060003AB RID: 939 RVA: 0x0000A035 File Offset: 0x00008235
		public static IContainer ShowIf(this IContainer element, Predicate<ShowIfContext> predicate)
		{
			return element.Element(new ShowIf
			{
				VisibilityPredicate = predicate
			});
		}

		// Token: 0x060003AC RID: 940 RVA: 0x0000A049 File Offset: 0x00008249
		public static IContainer Unconstrained(this IContainer element)
		{
			return element.Element(new Unconstrained());
		}

		// Token: 0x060003AD RID: 941 RVA: 0x0000A056 File Offset: 0x00008256
		public static IContainer DefaultTextStyle(this IContainer element, TextStyle textStyle)
		{
			return element.Element(new DefaultTextStyle
			{
				TextStyle = textStyle
			});
		}

		// Token: 0x060003AE RID: 942 RVA: 0x0000A06A File Offset: 0x0000826A
		public static IContainer DefaultTextStyle(this IContainer element, Func<TextStyle, TextStyle> handler)
		{
			return element.Element(new DefaultTextStyle
			{
				TextStyle = handler.Invoke(TextStyle.Default)
			});
		}

		// Token: 0x060003AF RID: 943 RVA: 0x0000A088 File Offset: 0x00008288
		public static IContainer StopPaging(this IContainer element)
		{
			return element.Element(new StopPaging());
		}

		// Token: 0x060003B0 RID: 944 RVA: 0x0000A095 File Offset: 0x00008295
		public static IContainer ScaleToFit(this IContainer element)
		{
			return element.Element(new ScaleToFit());
		}

		// Token: 0x060003B1 RID: 945 RVA: 0x0000A0A2 File Offset: 0x000082A2
		public static IContainer Repeat(this IContainer element)
		{
			return element.Element(new RepeatContent());
		}

		// Token: 0x060003B2 RID: 946 RVA: 0x0000A0AF File Offset: 0x000082AF
		internal static IContainer RepeatAsHeader(this IContainer element)
		{
			return element.Element(new RepeatContent
			{
				RepeatContext = RepeatContent.RepeatContextType.PageHeader
			});
		}

		// Token: 0x060003B3 RID: 947 RVA: 0x0000A0C3 File Offset: 0x000082C3
		internal static IContainer RepeatAsFooter(this IContainer element)
		{
			return element.Element(new RepeatContent
			{
				RepeatContext = RepeatContent.RepeatContextType.PageFooter
			});
		}

		// Token: 0x060003B4 RID: 948 RVA: 0x0000A0D7 File Offset: 0x000082D7
		public static void Lazy(this IContainer element, Action<IContainer> contentBuilder)
		{
			element.Element(new Lazy
			{
				ContentSource = contentBuilder,
				IsCacheable = false
			});
		}

		// Token: 0x060003B5 RID: 949 RVA: 0x0000A0F3 File Offset: 0x000082F3
		public static void LazyWithCache(this IContainer element, Action<IContainer> contentBuilder)
		{
			element.Element(new Lazy
			{
				ContentSource = contentBuilder,
				IsCacheable = true
			});
		}

		// Token: 0x060003B6 RID: 950 RVA: 0x0000A10F File Offset: 0x0000830F
		public static IContainer ZIndex(this IContainer element, int indexValue)
		{
			return element.Element(new ZIndex
			{
				Depth = indexValue
			});
		}

		// Token: 0x060003B7 RID: 951 RVA: 0x0000A123 File Offset: 0x00008323
		public static IContainer CaptureContentPosition(this IContainer element, string id)
		{
			return element.Element(new ElementPositionLocator
			{
				Id = id
			});
		}

		// Token: 0x060003B8 RID: 952 RVA: 0x0000A137 File Offset: 0x00008337
		[Obsolete("The Canvas API has been deprecated since version 2024.3.0. Please use the .Svg(stringContent) API to provide custom content, and consult documentation webpage regarding integrating SkiaSharp with QuestPDF: https://www.questpdf.com/api-reference/skiasharp-integration.html")]
		[ExcludeFromCodeCoverage]
		public static void Canvas(this IContainer element, ElementExtensions.DrawOnCanvas handler)
		{
			throw new NotImplementedException("The Canvas API has been deprecated since version 2024.3.0. Please use the .Svg(stringContent) API to provide custom content, and consult documentation webpage regarding integrating SkiaSharp with QuestPDF: https://www.questpdf.com/api-reference/skiasharp-integration.html");
		}

		// Token: 0x04000272 RID: 626
		private const string CanvasDeprecatedMessage = "The Canvas API has been deprecated since version 2024.3.0. Please use the .Svg(stringContent) API to provide custom content, and consult documentation webpage regarding integrating SkiaSharp with QuestPDF: https://www.questpdf.com/api-reference/skiasharp-integration.html";

		// Token: 0x0200018F RID: 399
		// (Invoke) Token: 0x06000C3F RID: 3135
		[NullableContext(0)]
		[Obsolete("The Canvas API has been deprecated since version 2024.3.0. Please use the .Svg(stringContent) API to provide custom content, and consult documentation webpage regarding integrating SkiaSharp with QuestPDF: https://www.questpdf.com/api-reference/skiasharp-integration.html")]
		public delegate void DrawOnCanvas(object canvas, Size availableSpace);
	}
}
