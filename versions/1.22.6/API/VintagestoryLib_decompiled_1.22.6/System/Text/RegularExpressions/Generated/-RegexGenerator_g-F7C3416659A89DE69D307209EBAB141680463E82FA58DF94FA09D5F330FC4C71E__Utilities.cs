using System.CodeDom.Compiler;
using System.Runtime.CompilerServices;

namespace System.Text.RegularExpressions.Generated;

[GeneratedCode("System.Text.RegularExpressions.Generator", "10.0.13.7005")]
internal static class _003CRegexGenerator_g_003EF7C3416659A89DE69D307209EBAB141680463E82FA58DF94FA09D5F330FC4C71E__Utilities
{
	internal static readonly TimeSpan s_defaultTimeout = ((AppContext.GetData("REGEX_DEFAULT_MATCH_TIMEOUT") is TimeSpan timeout) ? timeout : Regex.InfiniteMatchTimeout);

	internal static readonly bool s_hasTimeout = s_defaultTimeout != Regex.InfiniteMatchTimeout;

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	internal static void StackPop(int[] stack, ref int pos, out int arg0, out int arg1)
	{
		arg0 = stack[--pos];
		arg1 = stack[--pos];
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	internal static void StackPush(ref int[] stack, ref int pos, int arg0, int arg1)
	{
		int[] s = stack;
		int p = pos;
		if ((uint)(p + 1) < (uint)s.Length)
		{
			s[p] = arg0;
			s[p + 1] = arg1;
			pos += 2;
		}
		else
		{
			WithResize(ref stack, ref pos, arg0, arg1);
		}
		[MethodImpl(MethodImplOptions.NoInlining)]
		static void WithResize(ref int[] reference, ref int reference2, int arg2, int arg3)
		{
			Array.Resize(ref reference, (reference2 + 1) * 2);
			StackPush(ref reference, ref reference2, arg2, arg3);
		}
	}
}
