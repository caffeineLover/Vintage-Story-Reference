using System;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using QuestPDF.Drawing;
using QuestPDF.Infrastructure;

namespace QuestPDF.Companion
{
	// Token: 0x0200012C RID: 300
	[NullableContext(1)]
	[Nullable(0)]
	public static class CompanionExtensions
	{
		// Token: 0x06000AA6 RID: 2726 RVA: 0x0001B8AC File Offset: 0x00019AAC
		static CompanionExtensions()
		{
			DocumentGenerator.ValidateLicense();
		}

		// Token: 0x06000AA7 RID: 2727 RVA: 0x0001B8B4 File Offset: 0x00019AB4
		public static void ShowInCompanion(this IDocument document, int port = 12500)
		{
			document.ShowInCompanionAsync(port, default(CancellationToken)).ConfigureAwait(true).GetAwaiter().GetResult();
		}

		// Token: 0x06000AA8 RID: 2728 RVA: 0x0001B8E8 File Offset: 0x00019AE8
		public static Task ShowInCompanionAsync(this IDocument document, int port = 12500, CancellationToken cancellationToken = default(CancellationToken))
		{
			CompanionExtensions.<ShowInCompanionAsync>d__2 <ShowInCompanionAsync>d__;
			<ShowInCompanionAsync>d__.<>t__builder = AsyncTaskMethodBuilder.Create();
			<ShowInCompanionAsync>d__.document = document;
			<ShowInCompanionAsync>d__.port = port;
			<ShowInCompanionAsync>d__.cancellationToken = cancellationToken;
			<ShowInCompanionAsync>d__.<>1__state = -1;
			<ShowInCompanionAsync>d__.<>t__builder.Start<CompanionExtensions.<ShowInCompanionAsync>d__2>(ref <ShowInCompanionAsync>d__);
			return <ShowInCompanionAsync>d__.<>t__builder.Task;
		}

		// Token: 0x06000AA9 RID: 2729 RVA: 0x0001B93C File Offset: 0x00019B3C
		[CompilerGenerated]
		internal static Task <ShowInCompanionAsync>g__KeepApplicationAlive|2_3(CancellationToken cancellationToken)
		{
			CompanionExtensions.<<ShowInCompanionAsync>g__KeepApplicationAlive|2_3>d <<ShowInCompanionAsync>g__KeepApplicationAlive|2_3>d;
			<<ShowInCompanionAsync>g__KeepApplicationAlive|2_3>d.<>t__builder = AsyncTaskMethodBuilder.Create();
			<<ShowInCompanionAsync>g__KeepApplicationAlive|2_3>d.cancellationToken = cancellationToken;
			<<ShowInCompanionAsync>g__KeepApplicationAlive|2_3>d.<>1__state = -1;
			<<ShowInCompanionAsync>g__KeepApplicationAlive|2_3>d.<>t__builder.Start<CompanionExtensions.<<ShowInCompanionAsync>g__KeepApplicationAlive|2_3>d>(ref <<ShowInCompanionAsync>g__KeepApplicationAlive|2_3>d);
			return <<ShowInCompanionAsync>g__KeepApplicationAlive|2_3>d.<>t__builder.Task;
		}
	}
}
