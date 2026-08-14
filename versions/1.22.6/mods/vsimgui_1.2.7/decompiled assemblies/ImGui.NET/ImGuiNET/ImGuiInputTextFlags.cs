using System;

namespace ImGuiNET
{
	// Token: 0x0200003C RID: 60
	[Flags]
	public enum ImGuiInputTextFlags
	{
		// Token: 0x040001BE RID: 446
		None = 0,
		// Token: 0x040001BF RID: 447
		CharsDecimal = 1,
		// Token: 0x040001C0 RID: 448
		CharsHexadecimal = 2,
		// Token: 0x040001C1 RID: 449
		CharsUppercase = 4,
		// Token: 0x040001C2 RID: 450
		CharsNoBlank = 8,
		// Token: 0x040001C3 RID: 451
		AutoSelectAll = 16,
		// Token: 0x040001C4 RID: 452
		EnterReturnsTrue = 32,
		// Token: 0x040001C5 RID: 453
		CallbackCompletion = 64,
		// Token: 0x040001C6 RID: 454
		CallbackHistory = 128,
		// Token: 0x040001C7 RID: 455
		CallbackAlways = 256,
		// Token: 0x040001C8 RID: 456
		CallbackCharFilter = 512,
		// Token: 0x040001C9 RID: 457
		AllowTabInput = 1024,
		// Token: 0x040001CA RID: 458
		CtrlEnterForNewLine = 2048,
		// Token: 0x040001CB RID: 459
		NoHorizontalScroll = 4096,
		// Token: 0x040001CC RID: 460
		AlwaysOverwrite = 8192,
		// Token: 0x040001CD RID: 461
		ReadOnly = 16384,
		// Token: 0x040001CE RID: 462
		Password = 32768,
		// Token: 0x040001CF RID: 463
		NoUndoRedo = 65536,
		// Token: 0x040001D0 RID: 464
		CharsScientific = 131072,
		// Token: 0x040001D1 RID: 465
		CallbackResize = 262144,
		// Token: 0x040001D2 RID: 466
		CallbackEdit = 524288,
		// Token: 0x040001D3 RID: 467
		EscapeClearsAll = 1048576
	}
}
