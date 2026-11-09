using System.CodeDom.Compiler;
using System.Runtime.CompilerServices;

namespace System.Text.RegularExpressions.Generated;

[GeneratedCode("System.Text.RegularExpressions.Generator", "10.0.13.11305")]
[SkipLocalsInit]
internal sealed class _003CRegexGenerator_g_003EF5E4070DDADCC3300604E0EF83A764699EE77ED70FD78E82B6CFF415D2F8056C5__GetHtmlTagRegex_0 : Regex
{
	private sealed class RunnerFactory : RegexRunnerFactory
	{
		private sealed class Runner : RegexRunner
		{
			protected override void Scan(ReadOnlySpan<char> inputSpan)
			{
				while (TryFindNextPossibleStartingPosition(inputSpan) && !TryMatchAtCurrentPosition(inputSpan) && runtextpos != inputSpan.Length)
				{
					runtextpos++;
					if (_003CRegexGenerator_g_003EF5E4070DDADCC3300604E0EF83A764699EE77ED70FD78E82B6CFF415D2F8056C5__Utilities.s_hasTimeout)
					{
						CheckTimeout();
					}
				}
			}

			private bool TryFindNextPossibleStartingPosition(ReadOnlySpan<char> inputSpan)
			{
				int pos = runtextpos;
				if (pos <= inputSpan.Length - 3)
				{
					int i = inputSpan.Slice(pos).IndexOf('<');
					if (i >= 0)
					{
						runtextpos = pos + i;
						return true;
					}
				}
				runtextpos = inputSpan.Length;
				return false;
			}

			private bool TryMatchAtCurrentPosition(ReadOnlySpan<char> inputSpan)
			{
				int pos = runtextpos;
				int matchStart = pos;
				int capture_starting_pos = 0;
				int charloop_capture_pos = 0;
				int charloop_starting_pos = 0;
				int charloop_ending_pos = 0;
				int lazyloop_capturepos = 0;
				int lazyloop_pos = 0;
				int loop_iteration = 0;
				int stackpos = 0;
				ReadOnlySpan<char> slice = inputSpan.Slice(pos);
				if (slice.IsEmpty || slice[0] != '<')
				{
					UncaptureUntil(0);
					return false;
				}
				if ((uint)slice.Length > 1u && slice[1] == '/')
				{
					slice = slice.Slice(1);
					pos++;
				}
				pos++;
				slice = inputSpan.Slice(pos);
				capture_starting_pos = pos;
				charloop_starting_pos = pos;
				int iteration;
				for (iteration = 0; (uint)iteration < (uint)slice.Length; iteration++)
				{
					char ch;
					if ((((ch = slice[iteration]) < '\u0080') ? ("\0\0\u2000Ͽ\ufffe蟿\ufffe߿"[(int)ch >> 4] & (1 << (ch & 0xF))) : (RegexRunner.CharInClass(ch, "\0\u0002\n-.\0\u0002\u0004\u0005\u0003\u0001\u0006\t\u0013\0") ? 1 : 0)) == 0)
					{
						break;
					}
				}
				if (iteration == 0)
				{
					UncaptureUntil(0);
					return false;
				}
				slice = slice.Slice(iteration);
				pos += iteration;
				charloop_ending_pos = pos;
				charloop_starting_pos++;
				while (true)
				{
					charloop_capture_pos = Crawlpos();
					Capture(1, capture_starting_pos, pos);
					loop_iteration = 0;
					while (true)
					{
						_003CRegexGenerator_g_003EF5E4070DDADCC3300604E0EF83A764699EE77ED70FD78E82B6CFF415D2F8056C5__Utilities.StackPush(ref runstack, ref stackpos, Crawlpos(), pos);
						loop_iteration++;
						if (!slice.IsEmpty && slice[0] == ' ')
						{
							pos++;
							slice = inputSpan.Slice(pos);
							lazyloop_pos = pos;
							goto IL_01d7;
						}
						goto IL_020e;
						IL_01d7:
						lazyloop_capturepos = Crawlpos();
						_003CRegexGenerator_g_003EF5E4070DDADCC3300604E0EF83A764699EE77ED70FD78E82B6CFF415D2F8056C5__Utilities.StackPush(ref runstack, ref stackpos, lazyloop_pos, lazyloop_capturepos);
						if (loop_iteration == 0)
						{
							continue;
						}
						goto IL_0260;
						IL_0260:
						if (!slice.IsEmpty && slice[0] == '/')
						{
							slice = slice.Slice(1);
							pos++;
						}
						if (slice.IsEmpty || slice[0] != '>')
						{
							if (_003CRegexGenerator_g_003EF5E4070DDADCC3300604E0EF83A764699EE77ED70FD78E82B6CFF415D2F8056C5__Utilities.s_hasTimeout)
							{
								CheckTimeout();
							}
							if (loop_iteration != 0)
							{
								_003CRegexGenerator_g_003EF5E4070DDADCC3300604E0EF83A764699EE77ED70FD78E82B6CFF415D2F8056C5__Utilities.StackPop(runstack, ref stackpos, out lazyloop_capturepos, out lazyloop_pos);
								UncaptureUntil(lazyloop_capturepos);
								if (_003CRegexGenerator_g_003EF5E4070DDADCC3300604E0EF83A764699EE77ED70FD78E82B6CFF415D2F8056C5__Utilities.s_hasTimeout)
								{
									CheckTimeout();
								}
								pos = lazyloop_pos;
								slice = inputSpan.Slice(pos);
								if (!slice.IsEmpty && slice[0] != '\n')
								{
									pos++;
									slice = inputSpan.Slice(pos);
									lazyloop_pos = pos;
									goto IL_01d7;
								}
								goto IL_020e;
							}
							break;
						}
						Capture(0, matchStart, runtextpos = pos + 1);
						return true;
						IL_020e:
						if (--loop_iteration < 0)
						{
							break;
						}
						pos = runstack[--stackpos];
						UncaptureUntil(runstack[--stackpos]);
						slice = inputSpan.Slice(pos);
						goto IL_0260;
					}
					UncaptureUntil(charloop_capture_pos);
					if (_003CRegexGenerator_g_003EF5E4070DDADCC3300604E0EF83A764699EE77ED70FD78E82B6CFF415D2F8056C5__Utilities.s_hasTimeout)
					{
						CheckTimeout();
					}
					if (charloop_starting_pos >= charloop_ending_pos)
					{
						break;
					}
					pos = --charloop_ending_pos;
					slice = inputSpan.Slice(pos);
				}
				UncaptureUntil(0);
				return false;
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				void UncaptureUntil(int capturePosition)
				{
					while (Crawlpos() > capturePosition)
					{
						Uncapture();
					}
				}
			}
		}

		protected override RegexRunner CreateInstance()
		{
			return new Runner();
		}
	}

	internal static readonly _003CRegexGenerator_g_003EF5E4070DDADCC3300604E0EF83A764699EE77ED70FD78E82B6CFF415D2F8056C5__GetHtmlTagRegex_0 Instance = new _003CRegexGenerator_g_003EF5E4070DDADCC3300604E0EF83A764699EE77ED70FD78E82B6CFF415D2F8056C5__GetHtmlTagRegex_0();

	private _003CRegexGenerator_g_003EF5E4070DDADCC3300604E0EF83A764699EE77ED70FD78E82B6CFF415D2F8056C5__GetHtmlTagRegex_0()
	{
		pattern = "</?([\\w\\-]+)(?: .*?)?/?>";
		roptions = RegexOptions.None;
		Regex.ValidateMatchTimeout(_003CRegexGenerator_g_003EF5E4070DDADCC3300604E0EF83A764699EE77ED70FD78E82B6CFF415D2F8056C5__Utilities.s_defaultTimeout);
		internalMatchTimeout = _003CRegexGenerator_g_003EF5E4070DDADCC3300604E0EF83A764699EE77ED70FD78E82B6CFF415D2F8056C5__Utilities.s_defaultTimeout;
		factory = new RunnerFactory();
		capsize = 2;
	}
}
