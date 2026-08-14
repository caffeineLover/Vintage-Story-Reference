using System;
using System.Runtime.CompilerServices;
using YamlDotNet.Serialization;

namespace YamlDotNet.Helpers
{
	// Token: 0x020000B6 RID: 182
	[NullableContext(1)]
	[Nullable(0)]
	public static class FsharpHelper
	{
		// Token: 0x1700005E RID: 94
		// (get) Token: 0x06000428 RID: 1064 RVA: 0x0000E5BF File Offset: 0x0000C7BF
		// (set) Token: 0x06000429 RID: 1065 RVA: 0x0000E5C6 File Offset: 0x0000C7C6
		[Nullable(2)]
		public static IFsharpHelper Instance { [NullableContext(2)] get; [NullableContext(2)] set; }

		// Token: 0x0600042A RID: 1066 RVA: 0x0000E5CE File Offset: 0x0000C7CE
		public static bool IsOptionType(Type t)
		{
			IFsharpHelper instance = FsharpHelper.Instance;
			return instance != null && instance.IsOptionType(t);
		}

		// Token: 0x0600042B RID: 1067 RVA: 0x0000E5E1 File Offset: 0x0000C7E1
		[return: Nullable(2)]
		public static Type GetOptionUnderlyingType(Type t)
		{
			IFsharpHelper instance = FsharpHelper.Instance;
			if (instance == null)
			{
				return null;
			}
			return instance.GetOptionUnderlyingType(t);
		}

		// Token: 0x0600042C RID: 1068 RVA: 0x0000E5F4 File Offset: 0x0000C7F4
		[return: Nullable(2)]
		public static object GetValue(IObjectDescriptor objectDescriptor)
		{
			IFsharpHelper instance = FsharpHelper.Instance;
			if (instance == null)
			{
				return null;
			}
			return instance.GetValue(objectDescriptor);
		}

		// Token: 0x0600042D RID: 1069 RVA: 0x0000E607 File Offset: 0x0000C807
		public static bool IsFsharpListType(Type t)
		{
			IFsharpHelper instance = FsharpHelper.Instance;
			return instance != null && instance.IsFsharpListType(t);
		}

		// Token: 0x0600042E RID: 1070 RVA: 0x0000E61A File Offset: 0x0000C81A
		[return: Nullable(2)]
		public static object CreateFsharpListFromArray(Type t, Type itemsType, Array arr)
		{
			IFsharpHelper instance = FsharpHelper.Instance;
			if (instance == null)
			{
				return null;
			}
			return instance.CreateFsharpListFromArray(t, itemsType, arr);
		}
	}
}
