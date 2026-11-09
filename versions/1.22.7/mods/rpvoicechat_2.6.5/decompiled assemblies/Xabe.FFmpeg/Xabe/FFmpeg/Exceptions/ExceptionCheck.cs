using System;

namespace Xabe.FFmpeg.Exceptions
{
	// Token: 0x02000043 RID: 67
	internal class ExceptionCheck
	{
		// Token: 0x06000211 RID: 529 RVA: 0x00006F3A File Offset: 0x0000513A
		public ExceptionCheck(string searchPhrase, bool containsFileIsEmptyMessage = false)
		{
			this._searchPhrase = searchPhrase;
			this._containsFileIsEmptyMessage = containsFileIsEmptyMessage;
		}

		// Token: 0x06000212 RID: 530 RVA: 0x00006F50 File Offset: 0x00005150
		internal bool CheckLog(string log)
		{
			return log.Contains(this._searchPhrase) && (!this._containsFileIsEmptyMessage || log.Contains("Output file is empty"));
		}

		// Token: 0x04000513 RID: 1299
		private readonly string _searchPhrase;

		// Token: 0x04000514 RID: 1300
		private readonly bool _containsFileIsEmptyMessage;
	}
}
