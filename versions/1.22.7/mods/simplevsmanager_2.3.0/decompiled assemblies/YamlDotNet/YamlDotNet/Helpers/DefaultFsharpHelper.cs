using System;
using System.Runtime.CompilerServices;
using YamlDotNet.Serialization;

namespace YamlDotNet.Helpers
{
	// Token: 0x020000B3 RID: 179
	[NullableContext(1)]
	[Nullable(0)]
	public class DefaultFsharpHelper : IFsharpHelper
	{
		// Token: 0x0600041F RID: 1055 RVA: 0x0000E3EC File Offset: 0x0000C5EC
		private static bool IsFsharpCore(Type t)
		{
			return t.Namespace == "Microsoft.FSharp.Core";
		}

		// Token: 0x06000420 RID: 1056 RVA: 0x0000E3FE File Offset: 0x0000C5FE
		public bool IsOptionType(Type t)
		{
			return DefaultFsharpHelper.IsFsharpCore(t) && t.Name == "FSharpOption`1";
		}

		// Token: 0x06000421 RID: 1057 RVA: 0x0000E41A File Offset: 0x0000C61A
		[return: Nullable(2)]
		public Type GetOptionUnderlyingType(Type t)
		{
			if (!t.IsGenericType || !this.IsOptionType(t))
			{
				return null;
			}
			return t.GenericTypeArguments[0];
		}

		// Token: 0x06000422 RID: 1058 RVA: 0x0000E437 File Offset: 0x0000C637
		[return: Nullable(2)]
		public object GetValue(IObjectDescriptor objectDescriptor)
		{
			if (!this.IsOptionType(objectDescriptor.Type))
			{
				throw new InvalidOperationException("Should not be called on non-Option<> type");
			}
			if (objectDescriptor.Value == null)
			{
				return null;
			}
			return objectDescriptor.Type.GetProperty("Value").GetValue(objectDescriptor.Value);
		}

		// Token: 0x06000423 RID: 1059 RVA: 0x0000E477 File Offset: 0x0000C677
		public bool IsFsharpListType(Type t)
		{
			return t.Namespace == "Microsoft.FSharp.Collections" && t.Name == "FSharpList`1";
		}

		// Token: 0x06000424 RID: 1060 RVA: 0x0000E4A0 File Offset: 0x0000C6A0
		[return: Nullable(2)]
		public object CreateFsharpListFromArray(Type t, Type itemsType, Array arr)
		{
			if (!this.IsFsharpListType(t))
			{
				return null;
			}
			return t.Assembly.GetType("Microsoft.FSharp.Collections.ListModule").GetMethod("OfArray").MakeGenericMethod(new Type[]
			{
				itemsType
			}).Invoke(null, new object[]
			{
				arr
			});
		}
	}
}
