using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;

namespace QuestPDF.Qpdf
{
	// Token: 0x02000036 RID: 54
	[NullableContext(1)]
	[Nullable(0)]
	internal static class SimpleJsonSerializer
	{
		// Token: 0x06000144 RID: 324 RVA: 0x00004704 File Offset: 0x00002904
		public static string Serialize(object obj)
		{
			if (obj == null)
			{
				return "null";
			}
			PropertyInfo[] properties = obj.GetType().GetProperties(20);
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append('{');
			foreach (PropertyInfo propertyInfo in properties)
			{
				object value = propertyInfo.GetValue(obj);
				if (value != null)
				{
					string name = CustomAttributeExtensions.GetCustomAttribute<SimpleJsonPropertyNameAttribute>(propertyInfo).Name;
					StringBuilder stringBuilder2 = stringBuilder;
					StringBuilder stringBuilder3 = stringBuilder2;
					StringBuilder.AppendInterpolatedStringHandler appendInterpolatedStringHandler;
					appendInterpolatedStringHandler..ctor(4, 2, stringBuilder2);
					appendInterpolatedStringHandler.AppendLiteral("\"");
					appendInterpolatedStringHandler.AppendFormatted(name);
					appendInterpolatedStringHandler.AppendLiteral("\": ");
					appendInterpolatedStringHandler.AppendFormatted(SimpleJsonSerializer.SerializeValue(value));
					stringBuilder3.Append(ref appendInterpolatedStringHandler);
					stringBuilder.Append(", ");
				}
			}
			if (properties.Length > 1)
			{
				stringBuilder.Length -= 2;
			}
			stringBuilder.Append('}');
			return stringBuilder.ToString();
		}

		// Token: 0x06000145 RID: 325 RVA: 0x000047E0 File Offset: 0x000029E0
		private static string SerializeValue(object value)
		{
			if (value == null)
			{
				return "null";
			}
			string text = value as string;
			if (text != null)
			{
				return "\"" + SimpleJsonSerializer.EscapeStringForJson(text) + "\"";
			}
			if (value is bool)
			{
				return value.ToString().ToLower();
			}
			IEnumerable<object> enumerable = value as IEnumerable<object>;
			if (enumerable != null)
			{
				StringBuilder stringBuilder = new StringBuilder();
				stringBuilder.Append('[');
				foreach (object value2 in enumerable)
				{
					StringBuilder stringBuilder2 = stringBuilder;
					StringBuilder stringBuilder3 = stringBuilder2;
					StringBuilder.AppendInterpolatedStringHandler appendInterpolatedStringHandler = new StringBuilder.AppendInterpolatedStringHandler(2, 1, stringBuilder2);
					appendInterpolatedStringHandler.AppendFormatted(SimpleJsonSerializer.SerializeValue(value2));
					appendInterpolatedStringHandler.AppendLiteral(", ");
					stringBuilder3.Append(ref appendInterpolatedStringHandler);
				}
				if (Enumerable.Any<object>(enumerable))
				{
					stringBuilder.Length -= 2;
				}
				stringBuilder.Append(']');
				return stringBuilder.ToString();
			}
			if (!value.GetType().IsPrimitive)
			{
				return SimpleJsonSerializer.Serialize(value);
			}
			return value.ToString();
		}

		// Token: 0x06000146 RID: 326 RVA: 0x000048F0 File Offset: 0x00002AF0
		private static string EscapeStringForJson(string input)
		{
			if (string.IsNullOrEmpty(input))
			{
				return input;
			}
			StringBuilder stringBuilder = new StringBuilder(input.Length);
			for (int i = 0; i < input.Length; i++)
			{
				char c = input.get_Chars(i);
				if (c == '\\')
				{
					stringBuilder.Append("\\\\");
				}
				else if (c == '"')
				{
					stringBuilder.Append("\\\"");
				}
				else if (c == '\b')
				{
					stringBuilder.Append("\\b");
				}
				else if (c == '\f')
				{
					stringBuilder.Append("\\f");
				}
				else if (c == '\n')
				{
					stringBuilder.Append("\\n");
				}
				else if (c == '\r')
				{
					stringBuilder.Append("\\r");
				}
				else if (c == '\t')
				{
					stringBuilder.Append("\\t");
				}
				else
				{
					stringBuilder.Append(c);
				}
			}
			return stringBuilder.ToString();
		}
	}
}
