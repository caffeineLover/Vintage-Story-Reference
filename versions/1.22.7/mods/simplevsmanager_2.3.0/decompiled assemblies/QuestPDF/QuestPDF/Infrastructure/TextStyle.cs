using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using QuestPDF.Helpers;
using QuestPDF.Skia;
using QuestPDF.Skia.Text;

namespace QuestPDF.Infrastructure
{
	// Token: 0x02000069 RID: 105
	[NullableContext(1)]
	[Nullable(0)]
	public class TextStyle : IEquatable<TextStyle>
	{
		// Token: 0x170000A9 RID: 169
		// (get) Token: 0x06000272 RID: 626 RVA: 0x00005D18 File Offset: 0x00003F18
		[CompilerGenerated]
		protected virtual Type EqualityContract
		{
			[CompilerGenerated]
			get
			{
				return typeof(TextStyle);
			}
		}

		// Token: 0x170000AA RID: 170
		// (get) Token: 0x06000273 RID: 627 RVA: 0x00005D24 File Offset: 0x00003F24
		// (set) Token: 0x06000274 RID: 628 RVA: 0x00005D2C File Offset: 0x00003F2C
		internal int Id { get; set; }

		// Token: 0x170000AB RID: 171
		// (get) Token: 0x06000275 RID: 629 RVA: 0x00005D35 File Offset: 0x00003F35
		// (set) Token: 0x06000276 RID: 630 RVA: 0x00005D3D File Offset: 0x00003F3D
		internal Color? Color { get; set; }

		// Token: 0x170000AC RID: 172
		// (get) Token: 0x06000277 RID: 631 RVA: 0x00005D46 File Offset: 0x00003F46
		// (set) Token: 0x06000278 RID: 632 RVA: 0x00005D4E File Offset: 0x00003F4E
		internal Color? BackgroundColor { get; set; }

		// Token: 0x170000AD RID: 173
		// (get) Token: 0x06000279 RID: 633 RVA: 0x00005D57 File Offset: 0x00003F57
		// (set) Token: 0x0600027A RID: 634 RVA: 0x00005D5F File Offset: 0x00003F5F
		internal Color? DecorationColor { get; set; }

		// Token: 0x170000AE RID: 174
		// (get) Token: 0x0600027B RID: 635 RVA: 0x00005D68 File Offset: 0x00003F68
		// (set) Token: 0x0600027C RID: 636 RVA: 0x00005D70 File Offset: 0x00003F70
		[Nullable(new byte[]
		{
			2,
			1
		})]
		internal string[] FontFamilies { [return: Nullable(new byte[]
		{
			2,
			1
		})] get; [param: Nullable(new byte[]
		{
			2,
			1
		})] set; }

		// Token: 0x170000AF RID: 175
		// (get) Token: 0x0600027D RID: 637 RVA: 0x00005D79 File Offset: 0x00003F79
		// (set) Token: 0x0600027E RID: 638 RVA: 0x00005D81 File Offset: 0x00003F81
		[TupleElementNames(new string[]
		{
			"Name",
			"Enabled"
		})]
		[Nullable(new byte[]
		{
			2,
			0,
			1
		})]
		internal ValueTuple<string, bool>[] FontFeatures { [return: TupleElementNames(new string[]
		{
			"Name",
			"Enabled"
		})] [return: Nullable(new byte[]
		{
			2,
			0,
			1
		})] get; [param: TupleElementNames(new string[]
		{
			"Name",
			"Enabled"
		})] [param: Nullable(new byte[]
		{
			2,
			0,
			1
		})] set; }

		// Token: 0x170000B0 RID: 176
		// (get) Token: 0x0600027F RID: 639 RVA: 0x00005D8A File Offset: 0x00003F8A
		// (set) Token: 0x06000280 RID: 640 RVA: 0x00005D92 File Offset: 0x00003F92
		internal float? Size { get; set; }

		// Token: 0x170000B1 RID: 177
		// (get) Token: 0x06000281 RID: 641 RVA: 0x00005D9B File Offset: 0x00003F9B
		// (set) Token: 0x06000282 RID: 642 RVA: 0x00005DA3 File Offset: 0x00003FA3
		internal float? LineHeight { get; set; }

		// Token: 0x170000B2 RID: 178
		// (get) Token: 0x06000283 RID: 643 RVA: 0x00005DAC File Offset: 0x00003FAC
		// (set) Token: 0x06000284 RID: 644 RVA: 0x00005DB4 File Offset: 0x00003FB4
		internal float? LetterSpacing { get; set; }

		// Token: 0x170000B3 RID: 179
		// (get) Token: 0x06000285 RID: 645 RVA: 0x00005DBD File Offset: 0x00003FBD
		// (set) Token: 0x06000286 RID: 646 RVA: 0x00005DC5 File Offset: 0x00003FC5
		internal float? WordSpacing { get; set; }

		// Token: 0x170000B4 RID: 180
		// (get) Token: 0x06000287 RID: 647 RVA: 0x00005DCE File Offset: 0x00003FCE
		// (set) Token: 0x06000288 RID: 648 RVA: 0x00005DD6 File Offset: 0x00003FD6
		internal FontWeight? FontWeight { get; set; }

		// Token: 0x170000B5 RID: 181
		// (get) Token: 0x06000289 RID: 649 RVA: 0x00005DDF File Offset: 0x00003FDF
		// (set) Token: 0x0600028A RID: 650 RVA: 0x00005DE7 File Offset: 0x00003FE7
		internal FontPosition? FontPosition { get; set; }

		// Token: 0x170000B6 RID: 182
		// (get) Token: 0x0600028B RID: 651 RVA: 0x00005DF0 File Offset: 0x00003FF0
		// (set) Token: 0x0600028C RID: 652 RVA: 0x00005DF8 File Offset: 0x00003FF8
		internal bool? IsItalic { get; set; }

		// Token: 0x170000B7 RID: 183
		// (get) Token: 0x0600028D RID: 653 RVA: 0x00005E01 File Offset: 0x00004001
		// (set) Token: 0x0600028E RID: 654 RVA: 0x00005E09 File Offset: 0x00004009
		internal bool? HasStrikethrough { get; set; }

		// Token: 0x170000B8 RID: 184
		// (get) Token: 0x0600028F RID: 655 RVA: 0x00005E12 File Offset: 0x00004012
		// (set) Token: 0x06000290 RID: 656 RVA: 0x00005E1A File Offset: 0x0000401A
		internal bool? HasUnderline { get; set; }

		// Token: 0x170000B9 RID: 185
		// (get) Token: 0x06000291 RID: 657 RVA: 0x00005E23 File Offset: 0x00004023
		// (set) Token: 0x06000292 RID: 658 RVA: 0x00005E2B File Offset: 0x0000402B
		internal bool? HasOverline { get; set; }

		// Token: 0x170000BA RID: 186
		// (get) Token: 0x06000293 RID: 659 RVA: 0x00005E34 File Offset: 0x00004034
		// (set) Token: 0x06000294 RID: 660 RVA: 0x00005E3C File Offset: 0x0000403C
		internal TextStyleConfiguration.TextDecorationStyle? DecorationStyle { get; set; }

		// Token: 0x170000BB RID: 187
		// (get) Token: 0x06000295 RID: 661 RVA: 0x00005E45 File Offset: 0x00004045
		// (set) Token: 0x06000296 RID: 662 RVA: 0x00005E4D File Offset: 0x0000404D
		internal float? DecorationThickness { get; set; }

		// Token: 0x170000BC RID: 188
		// (get) Token: 0x06000297 RID: 663 RVA: 0x00005E56 File Offset: 0x00004056
		// (set) Token: 0x06000298 RID: 664 RVA: 0x00005E5E File Offset: 0x0000405E
		internal TextDirection? Direction { get; set; }

		// Token: 0x06000299 RID: 665 RVA: 0x00005E68 File Offset: 0x00004068
		protected override void Finalize()
		{
			try
			{
				SkTextStyle skTextStyleCache = this.SkTextStyleCache;
				if (skTextStyleCache != null)
				{
					skTextStyleCache.Dispose();
				}
			}
			finally
			{
				base.Finalize();
			}
		}

		// Token: 0x170000BD RID: 189
		// (get) Token: 0x0600029A RID: 666 RVA: 0x00005EA4 File Offset: 0x000040A4
		public static TextStyle Default { get; } = new TextStyle
		{
			Id = 0
		};

		// Token: 0x170000BE RID: 190
		// (get) Token: 0x0600029B RID: 667 RVA: 0x00005EAB File Offset: 0x000040AB
		internal static TextStyle LibraryDefault { get; } = new TextStyle
		{
			Id = 1,
			Color = new Color?(Colors.Black),
			BackgroundColor = new Color?(Colors.Transparent),
			DecorationColor = new Color?(Colors.Black),
			FontFamilies = new string[]
			{
				"Lato"
			},
			FontFeatures = Array.Empty<ValueTuple<string, bool>>(),
			Size = new float?((float)12),
			LineHeight = new float?(0f),
			LetterSpacing = new float?(0f),
			WordSpacing = new float?(0f),
			FontWeight = new FontWeight?(QuestPDF.Infrastructure.FontWeight.Normal),
			FontPosition = new FontPosition?(QuestPDF.Infrastructure.FontPosition.Normal),
			IsItalic = new bool?(false),
			HasStrikethrough = new bool?(false),
			HasUnderline = new bool?(false),
			HasOverline = new bool?(false),
			DecorationStyle = new TextStyleConfiguration.TextDecorationStyle?(TextStyleConfiguration.TextDecorationStyle.Solid),
			DecorationThickness = new float?(1f),
			Direction = new TextDirection?(TextDirection.Auto)
		};

		// Token: 0x170000BF RID: 191
		// (get) Token: 0x0600029C RID: 668 RVA: 0x00005EB2 File Offset: 0x000040B2
		internal static TextStyle ParagraphSpacing { get; }

		// Token: 0x0600029D RID: 669 RVA: 0x00005EBC File Offset: 0x000040BC
		internal SkTextStyle GetSkTextStyle()
		{
			if (this.SkTextStyleCache != null)
			{
				return this.SkTextStyleCache;
			}
			object skTextStyleCacheLock = this.SkTextStyleCacheLock;
			SkTextStyle result;
			lock (skTextStyleCacheLock)
			{
				if (this.SkTextStyleCache != null)
				{
					result = this.SkTextStyleCache;
				}
				else
				{
					SkTextStyle skTextStyle = this.CreateSkTextStyle();
					this.SkTextStyleCache = skTextStyle;
					result = skTextStyle;
				}
			}
			return result;
		}

		// Token: 0x0600029E RID: 670 RVA: 0x00005F34 File Offset: 0x00004134
		private SkTextStyle CreateSkTextStyle()
		{
			List<SkText> list = Enumerable.ToList<SkText>(Enumerable.Select<string, SkText>(this.FontFamilies, (string x) => new SkText(x)));
			TextStyleConfiguration textStyleConfiguration = default(TextStyleConfiguration);
			textStyleConfiguration.FontSize = this.<CreateSkTextStyle>g__CalculateTargetFontSize|92_5();
			FontWeight? fontWeight = this.FontWeight;
			textStyleConfiguration.FontWeight = ((fontWeight != null) ? new TextStyleConfiguration.FontWeights?((TextStyleConfiguration.FontWeights)fontWeight.GetValueOrDefault()) : default(TextStyleConfiguration.FontWeights?)).GetValueOrDefault(TextStyleConfiguration.FontWeights.Normal);
			textStyleConfiguration.IsItalic = this.IsItalic.GetValueOrDefault();
			textStyleConfiguration.FontFamilies = TextStyle.<CreateSkTextStyle>g__GetFontFamilyPointers|92_2(list);
			textStyleConfiguration.FontFeatures = this.<CreateSkTextStyle>g__GetFontFeatures|92_3(Array.Empty<ValueTuple<string, int>>());
			textStyleConfiguration.ForegroundColor = (this.Color ?? Colors.Black);
			textStyleConfiguration.BackgroundColor = (this.BackgroundColor ?? Colors.Transparent);
			textStyleConfiguration.DecorationColor = (this.DecorationColor ?? Colors.Black);
			textStyleConfiguration.DecorationType = this.<CreateSkTextStyle>g__CreateDecoration|92_4();
			textStyleConfiguration.DecorationMode = TextStyleConfiguration.TextDecorationMode.Through;
			textStyleConfiguration.DecorationStyle = this.DecorationStyle.GetValueOrDefault();
			textStyleConfiguration.DecorationThickness = this.DecorationThickness.GetValueOrDefault(1f);
			textStyleConfiguration.LineHeight = this.LineHeight.GetValueOrDefault();
			textStyleConfiguration.LetterSpacing = this.LetterSpacing.GetValueOrDefault() * this.Size.GetValueOrDefault(1f);
			textStyleConfiguration.WordSpacing = this.WordSpacing.GetValueOrDefault() * this.Size.GetValueOrDefault(1f);
			textStyleConfiguration.BaselineOffset = this.<CreateSkTextStyle>g__CalculateBaselineOffset|92_6();
			SkTextStyle result = new SkTextStyle(textStyleConfiguration);
			list.ForEach(delegate(SkText x)
			{
				x.Dispose();
			});
			return result;
		}

		// Token: 0x0600029F RID: 671 RVA: 0x00006164 File Offset: 0x00004364
		[CompilerGenerated]
		public override string ToString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("TextStyle");
			stringBuilder.Append(" { ");
			if (this.PrintMembers(stringBuilder))
			{
				stringBuilder.Append(' ');
			}
			stringBuilder.Append('}');
			return stringBuilder.ToString();
		}

		// Token: 0x060002A0 RID: 672 RVA: 0x000061B0 File Offset: 0x000043B0
		[CompilerGenerated]
		protected virtual bool PrintMembers(StringBuilder builder)
		{
			return false;
		}

		// Token: 0x060002A1 RID: 673 RVA: 0x000061B3 File Offset: 0x000043B3
		[NullableContext(2)]
		[CompilerGenerated]
		public static bool operator !=(TextStyle left, TextStyle right)
		{
			return !(left == right);
		}

		// Token: 0x060002A2 RID: 674 RVA: 0x000061BF File Offset: 0x000043BF
		[NullableContext(2)]
		[CompilerGenerated]
		public static bool operator ==(TextStyle left, TextStyle right)
		{
			return left == right || (left != null && left.Equals(right));
		}

		// Token: 0x060002A3 RID: 675 RVA: 0x000061D4 File Offset: 0x000043D4
		[CompilerGenerated]
		public override int GetHashCode()
		{
			return ((((((((((((((((((((EqualityComparer<Type>.Default.GetHashCode(this.EqualityContract) * -1521134295 + EqualityComparer<int>.Default.GetHashCode(this.<Id>k__BackingField)) * -1521134295 + EqualityComparer<QuestPDF.Infrastructure.Color?>.Default.GetHashCode(this.<Color>k__BackingField)) * -1521134295 + EqualityComparer<QuestPDF.Infrastructure.Color?>.Default.GetHashCode(this.<BackgroundColor>k__BackingField)) * -1521134295 + EqualityComparer<QuestPDF.Infrastructure.Color?>.Default.GetHashCode(this.<DecorationColor>k__BackingField)) * -1521134295 + EqualityComparer<string[]>.Default.GetHashCode(this.<FontFamilies>k__BackingField)) * -1521134295 + EqualityComparer<ValueTuple<string, bool>[]>.Default.GetHashCode(this.<FontFeatures>k__BackingField)) * -1521134295 + EqualityComparer<float?>.Default.GetHashCode(this.<Size>k__BackingField)) * -1521134295 + EqualityComparer<float?>.Default.GetHashCode(this.<LineHeight>k__BackingField)) * -1521134295 + EqualityComparer<float?>.Default.GetHashCode(this.<LetterSpacing>k__BackingField)) * -1521134295 + EqualityComparer<float?>.Default.GetHashCode(this.<WordSpacing>k__BackingField)) * -1521134295 + EqualityComparer<QuestPDF.Infrastructure.FontWeight?>.Default.GetHashCode(this.<FontWeight>k__BackingField)) * -1521134295 + EqualityComparer<QuestPDF.Infrastructure.FontPosition?>.Default.GetHashCode(this.<FontPosition>k__BackingField)) * -1521134295 + EqualityComparer<bool?>.Default.GetHashCode(this.<IsItalic>k__BackingField)) * -1521134295 + EqualityComparer<bool?>.Default.GetHashCode(this.<HasStrikethrough>k__BackingField)) * -1521134295 + EqualityComparer<bool?>.Default.GetHashCode(this.<HasUnderline>k__BackingField)) * -1521134295 + EqualityComparer<bool?>.Default.GetHashCode(this.<HasOverline>k__BackingField)) * -1521134295 + EqualityComparer<TextStyleConfiguration.TextDecorationStyle?>.Default.GetHashCode(this.<DecorationStyle>k__BackingField)) * -1521134295 + EqualityComparer<float?>.Default.GetHashCode(this.<DecorationThickness>k__BackingField)) * -1521134295 + EqualityComparer<TextDirection?>.Default.GetHashCode(this.<Direction>k__BackingField)) * -1521134295 + EqualityComparer<SkTextStyle>.Default.GetHashCode(this.SkTextStyleCache)) * -1521134295 + EqualityComparer<object>.Default.GetHashCode(this.SkTextStyleCacheLock);
		}

		// Token: 0x060002A4 RID: 676 RVA: 0x000063D6 File Offset: 0x000045D6
		[NullableContext(2)]
		[CompilerGenerated]
		public override bool Equals(object obj)
		{
			return this.Equals(obj as TextStyle);
		}

		// Token: 0x060002A5 RID: 677 RVA: 0x000063E4 File Offset: 0x000045E4
		[NullableContext(2)]
		[CompilerGenerated]
		public virtual bool Equals(TextStyle other)
		{
			return this == other || (other != null && this.EqualityContract == other.EqualityContract && EqualityComparer<int>.Default.Equals(this.<Id>k__BackingField, other.<Id>k__BackingField) && EqualityComparer<QuestPDF.Infrastructure.Color?>.Default.Equals(this.<Color>k__BackingField, other.<Color>k__BackingField) && EqualityComparer<QuestPDF.Infrastructure.Color?>.Default.Equals(this.<BackgroundColor>k__BackingField, other.<BackgroundColor>k__BackingField) && EqualityComparer<QuestPDF.Infrastructure.Color?>.Default.Equals(this.<DecorationColor>k__BackingField, other.<DecorationColor>k__BackingField) && EqualityComparer<string[]>.Default.Equals(this.<FontFamilies>k__BackingField, other.<FontFamilies>k__BackingField) && EqualityComparer<ValueTuple<string, bool>[]>.Default.Equals(this.<FontFeatures>k__BackingField, other.<FontFeatures>k__BackingField) && EqualityComparer<float?>.Default.Equals(this.<Size>k__BackingField, other.<Size>k__BackingField) && EqualityComparer<float?>.Default.Equals(this.<LineHeight>k__BackingField, other.<LineHeight>k__BackingField) && EqualityComparer<float?>.Default.Equals(this.<LetterSpacing>k__BackingField, other.<LetterSpacing>k__BackingField) && EqualityComparer<float?>.Default.Equals(this.<WordSpacing>k__BackingField, other.<WordSpacing>k__BackingField) && EqualityComparer<QuestPDF.Infrastructure.FontWeight?>.Default.Equals(this.<FontWeight>k__BackingField, other.<FontWeight>k__BackingField) && EqualityComparer<QuestPDF.Infrastructure.FontPosition?>.Default.Equals(this.<FontPosition>k__BackingField, other.<FontPosition>k__BackingField) && EqualityComparer<bool?>.Default.Equals(this.<IsItalic>k__BackingField, other.<IsItalic>k__BackingField) && EqualityComparer<bool?>.Default.Equals(this.<HasStrikethrough>k__BackingField, other.<HasStrikethrough>k__BackingField) && EqualityComparer<bool?>.Default.Equals(this.<HasUnderline>k__BackingField, other.<HasUnderline>k__BackingField) && EqualityComparer<bool?>.Default.Equals(this.<HasOverline>k__BackingField, other.<HasOverline>k__BackingField) && EqualityComparer<TextStyleConfiguration.TextDecorationStyle?>.Default.Equals(this.<DecorationStyle>k__BackingField, other.<DecorationStyle>k__BackingField) && EqualityComparer<float?>.Default.Equals(this.<DecorationThickness>k__BackingField, other.<DecorationThickness>k__BackingField) && EqualityComparer<TextDirection?>.Default.Equals(this.<Direction>k__BackingField, other.<Direction>k__BackingField) && EqualityComparer<SkTextStyle>.Default.Equals(this.SkTextStyleCache, other.SkTextStyleCache) && EqualityComparer<object>.Default.Equals(this.SkTextStyleCacheLock, other.SkTextStyleCacheLock));
		}

		// Token: 0x060002A7 RID: 679 RVA: 0x00006648 File Offset: 0x00004848
		[CompilerGenerated]
		protected TextStyle(TextStyle original)
		{
			this.Id = original.<Id>k__BackingField;
			this.Color = original.<Color>k__BackingField;
			this.BackgroundColor = original.<BackgroundColor>k__BackingField;
			this.DecorationColor = original.<DecorationColor>k__BackingField;
			this.FontFamilies = original.<FontFamilies>k__BackingField;
			this.FontFeatures = original.<FontFeatures>k__BackingField;
			this.Size = original.<Size>k__BackingField;
			this.LineHeight = original.<LineHeight>k__BackingField;
			this.LetterSpacing = original.<LetterSpacing>k__BackingField;
			this.WordSpacing = original.<WordSpacing>k__BackingField;
			this.FontWeight = original.<FontWeight>k__BackingField;
			this.FontPosition = original.<FontPosition>k__BackingField;
			this.IsItalic = original.<IsItalic>k__BackingField;
			this.HasStrikethrough = original.<HasStrikethrough>k__BackingField;
			this.HasUnderline = original.<HasUnderline>k__BackingField;
			this.HasOverline = original.<HasOverline>k__BackingField;
			this.DecorationStyle = original.<DecorationStyle>k__BackingField;
			this.DecorationThickness = original.<DecorationThickness>k__BackingField;
			this.Direction = original.<Direction>k__BackingField;
			this.SkTextStyleCache = original.SkTextStyleCache;
			this.SkTextStyleCacheLock = original.SkTextStyleCacheLock;
		}

		// Token: 0x060002A8 RID: 680 RVA: 0x0000675B File Offset: 0x0000495B
		public TextStyle()
		{
			this.SkTextStyleCacheLock = new object();
			base..ctor();
		}

		// Token: 0x060002A9 RID: 681 RVA: 0x00006770 File Offset: 0x00004970
		// Note: this type is marked as 'beforefieldinit'.
		static TextStyle()
		{
			TextStyle textStyle = TextStyle.LibraryDefault.<Clone>$();
			textStyle.Id = 2;
			textStyle.Size = new float?(0f);
			textStyle.LineHeight = new float?((float)1);
			TextStyle.ParagraphSpacing = textStyle;
		}

		// Token: 0x060002AA RID: 682 RVA: 0x000068D8 File Offset: 0x00004AD8
		[CompilerGenerated]
		internal static IntPtr[] <CreateSkTextStyle>g__GetFontFamilyPointers|92_2(IList<SkText> texts)
		{
			IntPtr[] array = new IntPtr[16];
			for (int i = 0; i < Math.Min(array.Length, texts.Count); i++)
			{
				array[i] = texts[i].Instance;
			}
			return array;
		}

		// Token: 0x060002AB RID: 683 RVA: 0x00006918 File Offset: 0x00004B18
		[CompilerGenerated]
		private TextStyleConfiguration.FontFeature[] <CreateSkTextStyle>g__GetFontFeatures|92_3([TupleElementNames(new string[]
		{
			"name",
			"value"
		})] [Nullable(new byte[]
		{
			1,
			0,
			1
		})] params ValueTuple<string, int>[] features)
		{
			TextStyleConfiguration.FontFeature[] array = new TextStyleConfiguration.FontFeature[16];
			foreach (ValueTuple<ValueTuple<string, bool>, int> valueTuple in Enumerable.Select<ValueTuple<string, bool>, ValueTuple<ValueTuple<string, bool>, int>>(Enumerable.Take<ValueTuple<string, bool>>(this.FontFeatures, 16), ([TupleElementNames(new string[]
			{
				"Name",
				"Enabled"
			})] ValueTuple<string, bool> x, int i) => new ValueTuple<ValueTuple<string, bool>, int>(x, i)))
			{
				ValueTuple<string, bool> item = valueTuple.Item1;
				int item2 = valueTuple.Item2;
				array[item2].Name = item.Item1;
				array[item2].Value = ((item.Item2 > false) ? 1 : 0);
			}
			return array;
		}

		// Token: 0x060002AC RID: 684 RVA: 0x000069C8 File Offset: 0x00004BC8
		[CompilerGenerated]
		private TextStyleConfiguration.TextDecoration <CreateSkTextStyle>g__CreateDecoration|92_4()
		{
			TextStyleConfiguration.TextDecoration textDecoration = TextStyleConfiguration.TextDecoration.NoDecoration;
			if (this.HasUnderline.GetValueOrDefault())
			{
				textDecoration |= TextStyleConfiguration.TextDecoration.Underline;
			}
			if (this.HasStrikethrough.GetValueOrDefault())
			{
				textDecoration |= TextStyleConfiguration.TextDecoration.LineThrough;
			}
			if (this.HasOverline.GetValueOrDefault())
			{
				textDecoration |= TextStyleConfiguration.TextDecoration.Overline;
			}
			return textDecoration;
		}

		// Token: 0x060002AD RID: 685 RVA: 0x00006A14 File Offset: 0x00004C14
		[CompilerGenerated]
		private float <CreateSkTextStyle>g__CalculateTargetFontSize|92_5()
		{
			float valueOrDefault = this.Size.GetValueOrDefault();
			FontPosition? fontPosition = this.FontPosition;
			bool flag;
			if (fontPosition != null)
			{
				FontPosition valueOrDefault2 = fontPosition.GetValueOrDefault();
				if (valueOrDefault2 - QuestPDF.Infrastructure.FontPosition.Subscript <= 1)
				{
					flag = true;
					goto IL_35;
				}
			}
			flag = false;
			IL_35:
			if (flag)
			{
				return valueOrDefault * 0.6f;
			}
			return valueOrDefault;
		}

		// Token: 0x060002AE RID: 686 RVA: 0x00006A64 File Offset: 0x00004C64
		[CompilerGenerated]
		private float <CreateSkTextStyle>g__CalculateBaselineOffset|92_6()
		{
			if (this.FontPosition.GetValueOrDefault() == QuestPDF.Infrastructure.FontPosition.Subscript)
			{
				return this.Size.Value * 0.25f;
			}
			if (this.FontPosition.GetValueOrDefault() == QuestPDF.Infrastructure.FontPosition.Superscript)
			{
				return -this.Size.Value * 0.35f;
			}
			return 0f;
		}

		// Token: 0x04000159 RID: 345
		internal const float NormalLineHeightCalculatedFromFontMetrics = 0f;

		// Token: 0x04000170 RID: 368
		[Nullable(2)]
		private volatile SkTextStyle SkTextStyleCache;

		// Token: 0x04000171 RID: 369
		private readonly object SkTextStyleCacheLock;
	}
}
