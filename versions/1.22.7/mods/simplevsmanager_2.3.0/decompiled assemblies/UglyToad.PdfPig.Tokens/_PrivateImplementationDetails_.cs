using System;
using System.Runtime.CompilerServices;

// Token: 0x02000013 RID: 19
[CompilerGenerated]
internal sealed class <PrivateImplementationDetails>
{
	// Token: 0x0600006C RID: 108 RVA: 0x00005700 File Offset: 0x00003900
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
