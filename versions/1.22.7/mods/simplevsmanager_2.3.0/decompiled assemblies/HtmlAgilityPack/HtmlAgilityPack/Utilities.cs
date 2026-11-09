using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace HtmlAgilityPack
{
	// Token: 0x0200002E RID: 46
	[NullableContext(1)]
	[Nullable(0)]
	internal static class Utilities
	{
		// Token: 0x06000280 RID: 640 RVA: 0x00011CE8 File Offset: 0x0000FEE8
		public static TValue GetDictionaryValueOrDefault<TKey, [Nullable(2)] TValue>(Dictionary<TKey, TValue> dict, TKey key, TValue defaultValue = default(TValue)) where TKey : class
		{
			TValue result;
			if (!dict.TryGetValue(key, ref result))
			{
				return defaultValue;
			}
			return result;
		}

		// Token: 0x06000281 RID: 641 RVA: 0x00011D04 File Offset: 0x0000FF04
		internal static object To(this object @this, Type type)
		{
			if (@this != null)
			{
				if (@this.GetType() == type)
				{
					return @this;
				}
				TypeConverter converter = TypeDescriptor.GetConverter(@this);
				if (converter != null && converter.CanConvertTo(type))
				{
					return converter.ConvertTo(@this, type);
				}
				converter = TypeDescriptor.GetConverter(type);
				if (converter != null && converter.CanConvertFrom(@this.GetType()))
				{
					return converter.ConvertFrom(@this);
				}
				if (@this == DBNull.Value)
				{
					return null;
				}
			}
			return @this;
		}
	}
}
