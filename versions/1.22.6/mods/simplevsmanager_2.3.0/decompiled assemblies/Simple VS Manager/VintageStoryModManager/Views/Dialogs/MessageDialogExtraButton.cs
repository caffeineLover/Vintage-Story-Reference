using System;
using System.Runtime.CompilerServices;
using System.Windows;

namespace VintageStoryModManager.Views.Dialogs
{
	// Token: 0x0200002C RID: 44
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class MessageDialogExtraButton
	{
		// Token: 0x0600049F RID: 1183 RVA: 0x000220D2 File Offset: 0x000202D2
		public MessageDialogExtraButton(string content, MessageBoxResult result, [Nullable(2)] Action onClick = null, bool isDefault = false, bool isCancel = false)
		{
			if (content == null)
			{
				throw new ArgumentNullException("content");
			}
			this.Content = content;
			this.Result = result;
			this.OnClick = onClick;
			this.IsDefault = isDefault;
			this.IsCancel = isCancel;
		}

		// Token: 0x170000B4 RID: 180
		// (get) Token: 0x060004A0 RID: 1184 RVA: 0x0002210E File Offset: 0x0002030E
		public string Content { get; }

		// Token: 0x170000B5 RID: 181
		// (get) Token: 0x060004A1 RID: 1185 RVA: 0x00022116 File Offset: 0x00020316
		public MessageBoxResult Result { get; }

		// Token: 0x170000B6 RID: 182
		// (get) Token: 0x060004A2 RID: 1186 RVA: 0x0002211E File Offset: 0x0002031E
		public bool IsDefault { get; }

		// Token: 0x170000B7 RID: 183
		// (get) Token: 0x060004A3 RID: 1187 RVA: 0x00022126 File Offset: 0x00020326
		public bool IsCancel { get; }

		// Token: 0x170000B8 RID: 184
		// (get) Token: 0x060004A4 RID: 1188 RVA: 0x0002212E File Offset: 0x0002032E
		[Nullable(2)]
		public Action OnClick { [NullableContext(2)] get; }
	}
}
