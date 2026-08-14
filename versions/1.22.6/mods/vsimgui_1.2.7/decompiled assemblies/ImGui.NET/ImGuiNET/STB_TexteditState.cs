using System;

namespace ImGuiNET
{
	// Token: 0x0200007C RID: 124
	public struct STB_TexteditState
	{
		// Token: 0x040007C5 RID: 1989
		public int cursor;

		// Token: 0x040007C6 RID: 1990
		public int select_start;

		// Token: 0x040007C7 RID: 1991
		public int select_end;

		// Token: 0x040007C8 RID: 1992
		public byte insert_mode;

		// Token: 0x040007C9 RID: 1993
		public int row_count_per_page;

		// Token: 0x040007CA RID: 1994
		public byte cursor_at_end_of_line;

		// Token: 0x040007CB RID: 1995
		public byte initialized;

		// Token: 0x040007CC RID: 1996
		public byte has_preferred_x;

		// Token: 0x040007CD RID: 1997
		public byte single_line;

		// Token: 0x040007CE RID: 1998
		public byte padding1;

		// Token: 0x040007CF RID: 1999
		public byte padding2;

		// Token: 0x040007D0 RID: 2000
		public byte padding3;

		// Token: 0x040007D1 RID: 2001
		public float preferred_x;

		// Token: 0x040007D2 RID: 2002
		public StbUndoState undostate;
	}
}
