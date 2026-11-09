using System;
using System.CodeDom.Compiler;
using System.Collections;
using System.Runtime.CompilerServices;

namespace System.Text.RegularExpressions.Generated
{
	// Token: 0x02000004 RID: 4
	[GeneratedCode("System.Text.RegularExpressions.Generator", "10.0.14.37416")]
	internal sealed class <RegexGenerator_g>F7CDC02ACD2274424726E5BB4CB938FCE26A3C714A357453E70371D3BC4916E37__GetBooleanExpressionRegex_0 : Regex
	{
		// Token: 0x06000027 RID: 39 RVA: 0x00002178 File Offset: 0x00000378
		private <RegexGenerator_g>F7CDC02ACD2274424726E5BB4CB938FCE26A3C714A357453E70371D3BC4916E37__GetBooleanExpressionRegex_0()
		{
			this.pattern = "\\((?<expression>.+)\\) \\? (?<true>.+) \\: (?<false>.+)";
			this.roptions = 8;
			Regex.ValidateMatchTimeout(<RegexGenerator_g>F7CDC02ACD2274424726E5BB4CB938FCE26A3C714A357453E70371D3BC4916E37__Utilities.s_defaultTimeout);
			this.internalMatchTimeout = <RegexGenerator_g>F7CDC02ACD2274424726E5BB4CB938FCE26A3C714A357453E70371D3BC4916E37__Utilities.s_defaultTimeout;
			this.factory = new <RegexGenerator_g>F7CDC02ACD2274424726E5BB4CB938FCE26A3C714A357453E70371D3BC4916E37__GetBooleanExpressionRegex_0.RunnerFactory();
			Hashtable hashtable = new Hashtable();
			hashtable.Add("0", 0);
			hashtable.Add("expression", 1);
			hashtable.Add("false", 3);
			hashtable.Add("true", 2);
			base.CapNames = hashtable;
			this.capslist = new string[]
			{
				"0",
				"expression",
				"true",
				"false"
			};
			this.capsize = 4;
		}

		// Token: 0x04000004 RID: 4
		[Nullable(1)]
		internal static readonly <RegexGenerator_g>F7CDC02ACD2274424726E5BB4CB938FCE26A3C714A357453E70371D3BC4916E37__GetBooleanExpressionRegex_0 Instance = new <RegexGenerator_g>F7CDC02ACD2274424726E5BB4CB938FCE26A3C714A357453E70371D3BC4916E37__GetBooleanExpressionRegex_0();

		// Token: 0x02000005 RID: 5
		private sealed class RunnerFactory : RegexRunnerFactory
		{
			// Token: 0x06000029 RID: 41 RVA: 0x0000224B File Offset: 0x0000044B
			[NullableContext(1)]
			protected override RegexRunner CreateInstance()
			{
				return new <RegexGenerator_g>F7CDC02ACD2274424726E5BB4CB938FCE26A3C714A357453E70371D3BC4916E37__GetBooleanExpressionRegex_0.RunnerFactory.Runner();
			}

			// Token: 0x02000006 RID: 6
			private sealed class Runner : RegexRunner
			{
				// Token: 0x0600002B RID: 43 RVA: 0x0000225A File Offset: 0x0000045A
				protected override void Scan(ReadOnlySpan<char> inputSpan)
				{
					while (this.TryFindNextPossibleStartingPosition(inputSpan) && !this.TryMatchAtCurrentPosition(inputSpan) && this.runtextpos != inputSpan.Length)
					{
						this.runtextpos++;
						if (<RegexGenerator_g>F7CDC02ACD2274424726E5BB4CB938FCE26A3C714A357453E70371D3BC4916E37__Utilities.s_hasTimeout)
						{
							base.CheckTimeout();
						}
					}
				}

				// Token: 0x0600002C RID: 44 RVA: 0x0000229C File Offset: 0x0000049C
				private bool TryFindNextPossibleStartingPosition(ReadOnlySpan<char> inputSpan)
				{
					int pos = this.runtextpos;
					if (pos <= inputSpan.Length - 11)
					{
						int i = MemoryExtensions.IndexOf<char>(inputSpan.Slice(pos), '(');
						if (i >= 0)
						{
							this.runtextpos = pos + i;
							return true;
						}
					}
					this.runtextpos = inputSpan.Length;
					return false;
				}

				// Token: 0x0600002D RID: 45 RVA: 0x000022EC File Offset: 0x000004EC
				private unsafe bool TryMatchAtCurrentPosition(ReadOnlySpan<char> inputSpan)
				{
					int pos = this.runtextpos;
					int matchStart = pos;
					ReadOnlySpan<char> slice = inputSpan.Slice(pos);
					if (slice.IsEmpty || *slice[0] != 40)
					{
						this.<TryMatchAtCurrentPosition>g__UncaptureUntil|2_0(0);
						return false;
					}
					pos++;
					slice = inputSpan.Slice(pos);
					int capture_starting_pos = pos;
					int charloop_starting_pos = pos;
					int iteration = MemoryExtensions.IndexOf<char>(slice, '\n');
					if (iteration < 0)
					{
						iteration = slice.Length;
					}
					if (iteration == 0)
					{
						this.<TryMatchAtCurrentPosition>g__UncaptureUntil|2_0(0);
						return false;
					}
					slice = slice.Slice(iteration);
					pos += iteration;
					int charloop_ending_pos = pos;
					charloop_starting_pos++;
					int capture_starting_pos3;
					int iteration3;
					for (;;)
					{
						int charloop_capture_pos = base.Crawlpos();
						base.Capture(1, capture_starting_pos, pos);
						if (MemoryExtensions.StartsWith<char>(slice, MemoryExtensions.AsSpan(") ? ")))
						{
							pos += 4;
							slice = inputSpan.Slice(pos);
							int capture_starting_pos2 = pos;
							int charloop_starting_pos2 = pos;
							int iteration2 = MemoryExtensions.IndexOf<char>(slice, '\n');
							if (iteration2 < 0)
							{
								iteration2 = slice.Length;
							}
							if (iteration2 != 0)
							{
								slice = slice.Slice(iteration2);
								pos += iteration2;
								int charloop_ending_pos2 = pos;
								charloop_starting_pos2++;
								for (;;)
								{
									int charloop_capture_pos2 = base.Crawlpos();
									base.Capture(2, capture_starting_pos2, pos);
									if (MemoryExtensions.StartsWith<char>(slice, MemoryExtensions.AsSpan(" : ")))
									{
										pos += 3;
										slice = inputSpan.Slice(pos);
										capture_starting_pos3 = pos;
										iteration3 = MemoryExtensions.IndexOf<char>(slice, '\n');
										if (iteration3 < 0)
										{
											iteration3 = slice.Length;
										}
										if (iteration3 != 0)
										{
											goto Block_14;
										}
									}
									this.<TryMatchAtCurrentPosition>g__UncaptureUntil|2_0(charloop_capture_pos2);
									if (<RegexGenerator_g>F7CDC02ACD2274424726E5BB4CB938FCE26A3C714A357453E70371D3BC4916E37__Utilities.s_hasTimeout)
									{
										base.CheckTimeout();
									}
									if (charloop_starting_pos2 >= charloop_ending_pos2 || (charloop_ending_pos2 = MemoryExtensions.LastIndexOf<char>(inputSpan.Slice(charloop_starting_pos2, Math.Min(inputSpan.Length, charloop_ending_pos2 + 2) - charloop_starting_pos2), MemoryExtensions.AsSpan(" : "))) < 0)
									{
										break;
									}
									charloop_ending_pos2 += charloop_starting_pos2;
									pos = charloop_ending_pos2;
									slice = inputSpan.Slice(pos);
								}
							}
						}
						this.<TryMatchAtCurrentPosition>g__UncaptureUntil|2_0(charloop_capture_pos);
						if (<RegexGenerator_g>F7CDC02ACD2274424726E5BB4CB938FCE26A3C714A357453E70371D3BC4916E37__Utilities.s_hasTimeout)
						{
							base.CheckTimeout();
						}
						if (charloop_starting_pos >= charloop_ending_pos || (charloop_ending_pos = MemoryExtensions.LastIndexOf<char>(inputSpan.Slice(charloop_starting_pos, Math.Min(inputSpan.Length, charloop_ending_pos + 3) - charloop_starting_pos), MemoryExtensions.AsSpan(") ? "))) < 0)
						{
							break;
						}
						charloop_ending_pos += charloop_starting_pos;
						pos = charloop_ending_pos;
						slice = inputSpan.Slice(pos);
					}
					this.<TryMatchAtCurrentPosition>g__UncaptureUntil|2_0(0);
					return false;
					Block_14:
					slice = slice.Slice(iteration3);
					pos += iteration3;
					base.Capture(3, capture_starting_pos3, pos);
					this.runtextpos = pos;
					base.Capture(0, matchStart, pos);
					return true;
				}

				// Token: 0x0600002F RID: 47 RVA: 0x00002567 File Offset: 0x00000767
				[CompilerGenerated]
				[MethodImpl(256)]
				private void <TryMatchAtCurrentPosition>g__UncaptureUntil|2_0(int capturePosition)
				{
					while (base.Crawlpos() > capturePosition)
					{
						base.Uncapture();
					}
				}
			}
		}
	}
}
