using System;
using Concentus.Common;

namespace Concentus.Silk.Structs
{
	// Token: 0x0200004D RID: 77
	internal class SilkLPState
	{
		// Token: 0x0600015C RID: 348 RVA: 0x00016888 File Offset: 0x00014A88
		internal void Reset()
		{
			this.In_LP_State[0] = 0;
			this.In_LP_State[1] = 0;
			this.transition_frame_no = 0;
			this.mode = 0;
		}

		// Token: 0x0600015D RID: 349 RVA: 0x000168AC File Offset: 0x00014AAC
		internal void silk_LP_variable_cutoff(short[] frame, int frame_ptr, int frame_length)
		{
			int[] b_Q = new int[3];
			int[] a_Q = new int[2];
			if (this.mode != 0)
			{
				int num = Inlines.silk_LSHIFT(256 - this.transition_frame_no, 10);
				int num2 = Inlines.silk_RSHIFT(num, 16);
				num -= Inlines.silk_LSHIFT(num2, 16);
				Filters.silk_LP_interpolate_filter_taps(b_Q, a_Q, num2, num);
				this.transition_frame_no = Inlines.silk_LIMIT(this.transition_frame_no + this.mode, 0, 256);
				Filters.silk_biquad_alt(frame, frame_ptr, b_Q, a_Q, this.In_LP_State, frame, frame_ptr, frame_length, 1);
			}
		}

		// Token: 0x04000260 RID: 608
		internal readonly int[] In_LP_State = new int[2];

		// Token: 0x04000261 RID: 609
		internal int transition_frame_no;

		// Token: 0x04000262 RID: 610
		internal int mode;
	}
}
