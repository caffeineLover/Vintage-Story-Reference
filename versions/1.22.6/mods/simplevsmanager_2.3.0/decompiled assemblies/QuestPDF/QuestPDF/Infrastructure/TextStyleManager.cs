using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace QuestPDF.Infrastructure
{
	// Token: 0x0200006C RID: 108
	[NullableContext(1)]
	[Nullable(0)]
	internal static class TextStyleManager
	{
		// Token: 0x060002B4 RID: 692 RVA: 0x00006B88 File Offset: 0x00004D88
		public static TextStyle Mutate(this TextStyle origin, TextStyleProperty property, object value)
		{
			if (property == TextStyleProperty.FontFamilies)
			{
				value = new ArrayContainer<string>(value);
			}
			if (property == TextStyleProperty.FontFeatures)
			{
				value = new ArrayContainer<ValueTuple<string, bool>>(value);
			}
			ValueTuple<int, TextStyleProperty, object> valueTuple = new ValueTuple<int, TextStyleProperty, object>(origin.Id, property, value);
			return TextStyleManager.TextStyleMutateCache.GetOrAdd(valueTuple, delegate([TupleElementNames(new string[]
			{
				"originId",
				"property",
				"value"
			})] [Nullable(new byte[]
			{
				0,
				1
			})] ValueTuple<int, TextStyleProperty, object> x)
			{
				object newValue = x.Item3;
				object item = x.Item3;
				if (item is ArrayContainer<string>)
				{
					newValue = ((ArrayContainer<string>)item).Items;
				}
				item = x.Item3;
				if (item is ArrayContainer<ValueTuple<string, bool>>)
				{
					newValue = ((ArrayContainer<ValueTuple<string, bool>>)item).Items;
				}
				return TextStyleManager.TextStyles[x.Item1].MutateStyle(x.Item2, newValue, true);
			});
		}

		// Token: 0x060002B5 RID: 693 RVA: 0x00006BF0 File Offset: 0x00004DF0
		private static TextStyle MutateStyle(this TextStyle origin, TextStyleProperty targetProperty, [Nullable(2)] object newValue, bool overrideValue)
		{
			if (targetProperty == TextStyleProperty.FontFamilies)
			{
				return origin.MutateFontFamily(newValue as string[], overrideValue);
			}
			if (targetProperty == TextStyleProperty.FontFeatures)
			{
				return origin.MutateFontFeatures(newValue as ValueTuple<string, bool>[], overrideValue);
			}
			object mutationLock = TextStyleManager.MutationLock;
			TextStyle result;
			lock (mutationLock)
			{
				if (overrideValue && newValue == null)
				{
					result = origin;
				}
				else
				{
					PropertyInfo property = typeof(TextStyle).GetProperty(targetProperty.ToString(), 52);
					object value = property.GetValue(origin);
					if (!overrideValue && value != null)
					{
						result = origin;
					}
					else if (value == newValue)
					{
						result = origin;
					}
					else
					{
						int count = TextStyleManager.TextStyles.Count;
						TextStyle textStyle = origin.<Clone>$();
						textStyle.Id = count;
						TextStyle textStyle2 = textStyle;
						property.SetValue(textStyle2, newValue);
						TextStyleManager.TextStyles.Add(textStyle2);
						result = textStyle2;
					}
				}
			}
			return result;
		}

		// Token: 0x060002B6 RID: 694 RVA: 0x00006CCC File Offset: 0x00004ECC
		private static TextStyle MutateFontFamily(this TextStyle origin, [Nullable(new byte[]
		{
			2,
			1
		})] string[] newValue, bool overrideValue)
		{
			object mutationLock = TextStyleManager.MutationLock;
			TextStyle result;
			lock (mutationLock)
			{
				if (overrideValue && newValue == null)
				{
					result = origin;
				}
				else
				{
					if (newValue == null)
					{
						newValue = Array.Empty<string>();
					}
					string[] array = origin.FontFamilies ?? Array.Empty<string>();
					string[] fontFamilies = origin.FontFamilies;
					if (fontFamilies != null && Enumerable.SequenceEqual<string>(fontFamilies, newValue))
					{
						result = origin;
					}
					else
					{
						int count = TextStyleManager.TextStyles.Count;
						TextStyle textStyle = origin.<Clone>$();
						textStyle.Id = count;
						TextStyle textStyle2 = textStyle;
						TextStyle textStyle3 = textStyle2;
						string[] fontFamilies2;
						if (!overrideValue)
						{
							fontFamilies2 = Enumerable.ToArray<string>(Enumerable.Distinct<string>(Enumerable.Where<string>(Enumerable.Concat<string>(array, newValue), (string x) => !string.IsNullOrEmpty(x))));
						}
						else
						{
							fontFamilies2 = newValue;
						}
						textStyle3.FontFamilies = fontFamilies2;
						TextStyleManager.TextStyles.Add(textStyle2);
						result = textStyle2;
					}
				}
			}
			return result;
		}

		// Token: 0x060002B7 RID: 695 RVA: 0x00006DB8 File Offset: 0x00004FB8
		private static TextStyle MutateFontFeatures(this TextStyle origin, [TupleElementNames(new string[]
		{
			"Name",
			"Enabled"
		})] [Nullable(new byte[]
		{
			2,
			0,
			1
		})] ValueTuple<string, bool>[] newValue, bool overrideValue)
		{
			object mutationLock = TextStyleManager.MutationLock;
			TextStyle result;
			lock (mutationLock)
			{
				if (overrideValue && newValue == null)
				{
					result = origin;
				}
				else
				{
					if (newValue == null)
					{
						newValue = Array.Empty<ValueTuple<string, bool>>();
					}
					ValueTuple<string, bool>[] array = origin.FontFeatures ?? Array.Empty<ValueTuple<string, bool>>();
					ValueTuple<string, bool>[] fontFeatures = origin.FontFeatures;
					if (fontFeatures != null && Enumerable.SequenceEqual<ValueTuple<string, bool>>(fontFeatures, newValue))
					{
						result = origin;
					}
					else
					{
						IEnumerable<ValueTuple<string, bool>> enumerable = overrideValue ? Enumerable.Concat<ValueTuple<string, bool>>(newValue, array) : Enumerable.Concat<ValueTuple<string, bool>>(array, newValue);
						int count = TextStyleManager.TextStyles.Count;
						TextStyle textStyle = origin.<Clone>$();
						textStyle.Id = count;
						TextStyle textStyle2 = textStyle;
						textStyle2.FontFeatures = Enumerable.ToArray<ValueTuple<string, bool>>(Enumerable.Select<IGrouping<string, ValueTuple<string, bool>>, ValueTuple<string, bool>>(Enumerable.GroupBy<ValueTuple<string, bool>, string>(enumerable, ([TupleElementNames(new string[]
						{
							"Name",
							"Enabled"
						})] ValueTuple<string, bool> x) => x.Item1), ([TupleElementNames(new string[]
						{
							"Name",
							"Enabled"
						})] IGrouping<string, ValueTuple<string, bool>> x) => Enumerable.First<ValueTuple<string, bool>>(x)));
						TextStyleManager.TextStyles.Add(textStyle2);
						result = textStyle2;
					}
				}
			}
			return result;
		}

		// Token: 0x060002B8 RID: 696 RVA: 0x00006ED0 File Offset: 0x000050D0
		internal static TextStyle ApplyInheritedStyle(this TextStyle style, TextStyle parent)
		{
			return TextStyleManager.TextStyleApplyInheritedCache.GetOrAdd(new ValueTuple<int, int>(style.Id, parent.Id), ([TupleElementNames(new string[]
			{
				"originId",
				"parentId"
			})] ValueTuple<int, int> key) => TextStyleManager.ApplyStyleProperties(key.Item1, key.Item2, false));
		}

		// Token: 0x060002B9 RID: 697 RVA: 0x00006F0C File Offset: 0x0000510C
		internal static TextStyle ApplyGlobalStyle(this TextStyle style)
		{
			return TextStyleManager.TextStyleApplyGlobalCache.GetOrAdd(style.Id, (int key) => TextStyleManager.ApplyStyleProperties(key, TextStyle.LibraryDefault.Id, false));
		}

		// Token: 0x060002BA RID: 698 RVA: 0x00006F3D File Offset: 0x0000513D
		internal static TextStyle OverrideStyle(this TextStyle style, TextStyle parent)
		{
			return TextStyleManager.TextStyleOverrideCache.GetOrAdd(new ValueTuple<int, int>(style.Id, parent.Id), ([TupleElementNames(new string[]
			{
				"originId",
				"parentId"
			})] ValueTuple<int, int> key) => TextStyleManager.ApplyStyleProperties(key.Item1, key.Item2, true));
		}

		// Token: 0x060002BB RID: 699 RVA: 0x00006F7C File Offset: 0x0000517C
		private static TextStyle ApplyStyleProperties(int styleId, int parentId, bool overrideStyle)
		{
			TextStyle textStyle = TextStyleManager.TextStyles[styleId];
			TextStyle parent = TextStyleManager.TextStyles[parentId];
			return Enumerable.Aggregate<TextStyleProperty, TextStyle>(Enumerable.Cast<TextStyleProperty>(Enum.GetValues(typeof(TextStyleProperty))), textStyle, delegate(TextStyle mutableStyle, TextStyleProperty nextProperty)
			{
				object value = typeof(TextStyle).GetProperty(nextProperty.ToString(), 52).GetValue(parent);
				return mutableStyle.MutateStyle(nextProperty, value, overrideStyle);
			});
		}

		// Token: 0x060002BC RID: 700 RVA: 0x00006FD9 File Offset: 0x000051D9
		internal static TextStyle GetTextStyle(int id)
		{
			return TextStyleManager.TextStyles[id];
		}

		// Token: 0x060002BD RID: 701 RVA: 0x00006FE8 File Offset: 0x000051E8
		// Note: this type is marked as 'beforefieldinit'.
		static TextStyleManager()
		{
			List<TextStyle> list = new List<TextStyle>();
			list.Add(TextStyle.Default);
			list.Add(TextStyle.LibraryDefault);
			list.Add(TextStyle.ParagraphSpacing);
			TextStyleManager.TextStyles = list;
			TextStyleManager.TextStyleMutateCache = new ConcurrentDictionary<ValueTuple<int, TextStyleProperty, object>, TextStyle>();
			TextStyleManager.TextStyleApplyInheritedCache = new ConcurrentDictionary<ValueTuple<int, int>, TextStyle>();
			TextStyleManager.TextStyleApplyGlobalCache = new ConcurrentDictionary<int, TextStyle>();
			TextStyleManager.TextStyleOverrideCache = new ConcurrentDictionary<ValueTuple<int, int>, TextStyle>();
			TextStyleManager.MutationLock = new object();
		}

		// Token: 0x04000186 RID: 390
		private static readonly List<TextStyle> TextStyles;

		// Token: 0x04000187 RID: 391
		[TupleElementNames(new string[]
		{
			"originId",
			"property",
			"value"
		})]
		[Nullable(new byte[]
		{
			1,
			0,
			1,
			1
		})]
		private static readonly ConcurrentDictionary<ValueTuple<int, TextStyleProperty, object>, TextStyle> TextStyleMutateCache;

		// Token: 0x04000188 RID: 392
		[TupleElementNames(new string[]
		{
			"originId",
			"parentId"
		})]
		[Nullable(new byte[]
		{
			1,
			0,
			1
		})]
		private static readonly ConcurrentDictionary<ValueTuple<int, int>, TextStyle> TextStyleApplyInheritedCache;

		// Token: 0x04000189 RID: 393
		private static readonly ConcurrentDictionary<int, TextStyle> TextStyleApplyGlobalCache;

		// Token: 0x0400018A RID: 394
		[TupleElementNames(new string[]
		{
			"originId",
			"parentId"
		})]
		[Nullable(new byte[]
		{
			1,
			0,
			1
		})]
		private static readonly ConcurrentDictionary<ValueTuple<int, int>, TextStyle> TextStyleOverrideCache;

		// Token: 0x0400018B RID: 395
		private static readonly object MutationLock;
	}
}
