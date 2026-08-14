using System;

namespace NVorbis.Contracts
{
	// Token: 0x0200002C RID: 44
	public interface IPacket
	{
		// Token: 0x1700008C RID: 140
		// (get) Token: 0x060001C5 RID: 453
		bool IsResync { get; }

		// Token: 0x1700008D RID: 141
		// (get) Token: 0x060001C6 RID: 454
		bool IsShort { get; }

		// Token: 0x1700008E RID: 142
		// (get) Token: 0x060001C7 RID: 455
		long? GranulePosition { get; }

		// Token: 0x1700008F RID: 143
		// (get) Token: 0x060001C8 RID: 456
		bool IsEndOfStream { get; }

		// Token: 0x17000090 RID: 144
		// (get) Token: 0x060001C9 RID: 457
		int BitsRead { get; }

		// Token: 0x17000091 RID: 145
		// (get) Token: 0x060001CA RID: 458
		int BitsRemaining { get; }

		// Token: 0x17000092 RID: 146
		// (get) Token: 0x060001CB RID: 459
		int ContainerOverheadBits { get; }

		// Token: 0x060001CC RID: 460
		ulong TryPeekBits(int count, out int bitsRead);

		// Token: 0x060001CD RID: 461
		void SkipBits(int count);

		// Token: 0x060001CE RID: 462
		ulong ReadBits(int count);

		// Token: 0x060001CF RID: 463
		void Done();

		// Token: 0x060001D0 RID: 464
		void Reset();
	}
}
