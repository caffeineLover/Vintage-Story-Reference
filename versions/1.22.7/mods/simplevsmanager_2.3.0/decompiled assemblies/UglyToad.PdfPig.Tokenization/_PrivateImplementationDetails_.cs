using System;
using System.Runtime.CompilerServices;

// Token: 0x02000012 RID: 18
[CompilerGenerated]
internal sealed class <PrivateImplementationDetails>
{
	// Token: 0x06000046 RID: 70 RVA: 0x00003B9C File Offset: 0x00001D9C
	internal static uint ComputeStringHash(string s)
	{
		uint num;
		if (s != null)
		{
			num = 2166136261U;
			for (int i = 0; i < s.Length; i++)
			{
				num = ((uint)s.get_Chars(i) ^ num) * 16777619U;
			}
		}
		return num;
	}
}
