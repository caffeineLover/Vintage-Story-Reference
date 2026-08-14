using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using QuestPDF.Drawing.Exceptions;
using QuestPDF.Elements;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace QuestPDF.Fluent
{
	// Token: 0x02000099 RID: 153
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class PageDescriptor
	{
		// Token: 0x17000108 RID: 264
		// (get) Token: 0x06000432 RID: 1074 RVA: 0x0000B158 File Offset: 0x00009358
		internal Page Page { get; } = new Page();

		// Token: 0x06000433 RID: 1075 RVA: 0x0000B160 File Offset: 0x00009360
		internal PageDescriptor()
		{
		}

		// Token: 0x06000434 RID: 1076 RVA: 0x0000B174 File Offset: 0x00009374
		public void Size(float width, float height, Unit unit = Unit.Point)
		{
			PageSize pageSize = new PageSize(width, height, unit);
			this.MinSize(pageSize);
			this.MaxSize(pageSize);
		}

		// Token: 0x06000435 RID: 1077 RVA: 0x0000B198 File Offset: 0x00009398
		public void Size(PageSize pageSize)
		{
			this.MinSize(pageSize);
			this.MaxSize(pageSize);
		}

		// Token: 0x06000436 RID: 1078 RVA: 0x0000B1A8 File Offset: 0x000093A8
		public void ContinuousSize(float width, Unit unit = Unit.Point)
		{
			this.MinSize(new PageSize(width.ToPoints(unit), 0f, Unit.Point));
			this.MaxSize(new PageSize(width.ToPoints(unit), QuestPDF.Infrastructure.Size.Max.Height, Unit.Point));
		}

		// Token: 0x06000437 RID: 1079 RVA: 0x0000B1DF File Offset: 0x000093DF
		public void MinSize(PageSize pageSize)
		{
			this.Page.MinSize = pageSize;
		}

		// Token: 0x06000438 RID: 1080 RVA: 0x0000B1F2 File Offset: 0x000093F2
		public void MaxSize(PageSize pageSize)
		{
			this.Page.MaxSize = pageSize;
		}

		// Token: 0x06000439 RID: 1081 RVA: 0x0000B205 File Offset: 0x00009405
		public void MarginLeft(float value, Unit unit = Unit.Point)
		{
			this.Page.MarginLeft = value.ToPoints(unit);
		}

		// Token: 0x0600043A RID: 1082 RVA: 0x0000B219 File Offset: 0x00009419
		public void MarginRight(float value, Unit unit = Unit.Point)
		{
			this.Page.MarginRight = value.ToPoints(unit);
		}

		// Token: 0x0600043B RID: 1083 RVA: 0x0000B22D File Offset: 0x0000942D
		public void MarginTop(float value, Unit unit = Unit.Point)
		{
			this.Page.MarginTop = value.ToPoints(unit);
		}

		// Token: 0x0600043C RID: 1084 RVA: 0x0000B241 File Offset: 0x00009441
		public void MarginBottom(float value, Unit unit = Unit.Point)
		{
			this.Page.MarginBottom = value.ToPoints(unit);
		}

		// Token: 0x0600043D RID: 1085 RVA: 0x0000B255 File Offset: 0x00009455
		public void MarginVertical(float value, Unit unit = Unit.Point)
		{
			this.MarginTop(value, unit);
			this.MarginBottom(value, unit);
		}

		// Token: 0x0600043E RID: 1086 RVA: 0x0000B267 File Offset: 0x00009467
		public void MarginHorizontal(float value, Unit unit = Unit.Point)
		{
			this.MarginLeft(value, unit);
			this.MarginRight(value, unit);
		}

		// Token: 0x0600043F RID: 1087 RVA: 0x0000B279 File Offset: 0x00009479
		public void Margin(float value, Unit unit = Unit.Point)
		{
			this.MarginVertical(value, unit);
			this.MarginHorizontal(value, unit);
		}

		// Token: 0x06000440 RID: 1088 RVA: 0x0000B28B File Offset: 0x0000948B
		public void DefaultTextStyle(TextStyle textStyle)
		{
			this.Page.DefaultTextStyle = textStyle;
		}

		// Token: 0x06000441 RID: 1089 RVA: 0x0000B299 File Offset: 0x00009499
		public void DefaultTextStyle(Func<TextStyle, TextStyle> handler)
		{
			this.DefaultTextStyle(handler.Invoke(TextStyle.Default));
		}

		// Token: 0x06000442 RID: 1090 RVA: 0x0000B2AC File Offset: 0x000094AC
		public void ContentFromLeftToRight()
		{
			this.Page.ContentDirection = ContentDirection.LeftToRight;
		}

		// Token: 0x06000443 RID: 1091 RVA: 0x0000B2BA File Offset: 0x000094BA
		public void ContentFromRightToLeft()
		{
			this.Page.ContentDirection = ContentDirection.RightToLeft;
		}

		// Token: 0x06000444 RID: 1092 RVA: 0x0000B2C8 File Offset: 0x000094C8
		public void PageColor(Color color)
		{
			this.Page.BackgroundColor = color;
		}

		// Token: 0x06000445 RID: 1093 RVA: 0x0000B2D6 File Offset: 0x000094D6
		[Obsolete("This element has been renamed since version 2022.3. Please use the PageColor method.")]
		[ExcludeFromCodeCoverage]
		public void Background(Color color)
		{
			this.PageColor(color);
		}

		// Token: 0x06000446 RID: 1094 RVA: 0x0000B2E0 File Offset: 0x000094E0
		public IContainer Background()
		{
			if (!(this.Page.Background is Empty))
			{
				throw new DocumentComposeException("The 'Page.Background' layer has already been defined. Please call this method only once.");
			}
			Container container = new Container();
			this.Page.Background = container;
			return container;
		}

		// Token: 0x06000447 RID: 1095 RVA: 0x0000B320 File Offset: 0x00009520
		public IContainer Foreground()
		{
			if (!(this.Page.Foreground is Empty))
			{
				throw new DocumentComposeException("The 'Page.Foreground' layer has already been defined. Please call this method only once.");
			}
			Container container = new Container();
			this.Page.Foreground = container;
			return container;
		}

		// Token: 0x06000448 RID: 1096 RVA: 0x0000B360 File Offset: 0x00009560
		public IContainer Header()
		{
			if (!(this.Page.Header is Empty))
			{
				throw new DocumentComposeException("The 'Page.Header' layer has already been defined. Please call this method only once.");
			}
			Container container = new Container();
			this.Page.Header = container;
			return container;
		}

		// Token: 0x06000449 RID: 1097 RVA: 0x0000B3A0 File Offset: 0x000095A0
		public IContainer Content()
		{
			if (!(this.Page.Content is Empty))
			{
				throw new DocumentComposeException("The 'Page.Content' layer has already been defined. Please call this method only once.");
			}
			Container container = new Container();
			this.Page.Content = container;
			return container;
		}

		// Token: 0x0600044A RID: 1098 RVA: 0x0000B3E0 File Offset: 0x000095E0
		public IContainer Footer()
		{
			if (!(this.Page.Footer is Empty))
			{
				throw new DocumentComposeException("The 'Page.Footer' layer has already been defined. Please call this method only once.");
			}
			Container container = new Container();
			this.Page.Footer = container;
			return container;
		}
	}
}
