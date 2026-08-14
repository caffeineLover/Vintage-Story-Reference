using System;
using System.Runtime.CompilerServices;

// Token: 0x02000005 RID: 5
[CompilerGenerated]
internal sealed class <PrivateImplementationDetails>
{
	// Token: 0x06000020 RID: 32 RVA: 0x00004B50 File Offset: 0x00002D50
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
