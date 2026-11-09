using System;
using System.Runtime.CompilerServices;
using System.Windows;

namespace VintageStoryModManager.Views.Dialogs
{
	// Token: 0x0200002B RID: 43
	[NullableContext(2)]
	[Nullable(0)]
	public sealed class MessageDialogButtonContentOverrides
	{
		// Token: 0x170000B0 RID: 176
		// (get) Token: 0x06000495 RID: 1173 RVA: 0x00022018 File Offset: 0x00020218
		// (set) Token: 0x06000496 RID: 1174 RVA: 0x00022020 File Offset: 0x00020220
		public string Ok { get; set; }

		// Token: 0x170000B1 RID: 177
		// (get) Token: 0x06000497 RID: 1175 RVA: 0x00022029 File Offset: 0x00020229
		// (set) Token: 0x06000498 RID: 1176 RVA: 0x00022031 File Offset: 0x00020231
		public string Cancel { get; set; }

		// Token: 0x170000B2 RID: 178
		// (get) Token: 0x06000499 RID: 1177 RVA: 0x0002203A File Offset: 0x0002023A
		// (set) Token: 0x0600049A RID: 1178 RVA: 0x00022042 File Offset: 0x00020242
		public string Yes { get; set; }

		// Token: 0x170000B3 RID: 179
		// (get) Token: 0x0600049B RID: 1179 RVA: 0x0002204B File Offset: 0x0002024B
		// (set) Token: 0x0600049C RID: 1180 RVA: 0x00022053 File Offset: 0x00020253
		public string No { get; set; }

		// Token: 0x0600049D RID: 1181 RVA: 0x0002205C File Offset: 0x0002025C
		public string GetContent(MessageBoxResult result)
		{
			switch (result)
			{
			case 1:
				return this.Ok;
			case 2:
				return this.Cancel;
			case 6:
				return this.Yes;
			case 7:
				return this.No;
			}
			throw new ArgumentOutOfRangeException("result", result, "Unsupported message box result.");
		}
	}
}
