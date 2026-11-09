using System;

namespace Concentus
{
	// Token: 0x0200000E RID: 14
	public class OpusException : Exception
	{
		// Token: 0x1700003C RID: 60
		// (get) Token: 0x06000099 RID: 153 RVA: 0x00004FD9 File Offset: 0x000031D9
		// (set) Token: 0x0600009A RID: 154 RVA: 0x00004FE1 File Offset: 0x000031E1
		public int OpusErrorCode { get; private set; }

		// Token: 0x0600009B RID: 155 RVA: 0x00004FEA File Offset: 0x000031EA
		internal OpusException() : base("Unknown error")
		{
			this.OpusErrorCode = -100;
		}

		// Token: 0x0600009C RID: 156 RVA: 0x00004FFF File Offset: 0x000031FF
		internal OpusException(string message) : base(message)
		{
			this.OpusErrorCode = -100;
		}

		// Token: 0x0600009D RID: 157 RVA: 0x00005010 File Offset: 0x00003210
		internal OpusException(int opusError) : base(CodecHelpers.opus_strerror(opusError))
		{
			this.OpusErrorCode = opusError;
		}

		// Token: 0x0600009E RID: 158 RVA: 0x00005025 File Offset: 0x00003225
		internal OpusException(string message, int opusError) : base(message)
		{
			this.OpusErrorCode = opusError;
		}
	}
}
