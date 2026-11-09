using System;
using System.IO;

namespace Xabe.FFmpeg.Exceptions
{
	// Token: 0x02000049 RID: 73
	public class InvalidInputException : FileNotFoundException
	{
		// Token: 0x0600021A RID: 538 RVA: 0x0000722B File Offset: 0x0000542B
		public InvalidInputException(string msg) : base(msg)
		{
		}
	}
}
