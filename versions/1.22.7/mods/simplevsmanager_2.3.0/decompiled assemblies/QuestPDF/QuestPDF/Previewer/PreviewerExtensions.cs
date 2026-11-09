using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using QuestPDF.Infrastructure;

namespace QuestPDF.Previewer
{
	// Token: 0x0200012B RID: 299
	[NullableContext(1)]
	[Nullable(0)]
	public static class PreviewerExtensions
	{
		// Token: 0x06000AA4 RID: 2724 RVA: 0x0001B894 File Offset: 0x00019A94
		[Obsolete("The Previewer application is no longer supprted. Please use a new QuestPDF Companion application by calling ShowInCompanion() or ShowInCompanionAsync() methods.")]
		[ExcludeFromCodeCoverage]
		public static void ShowInPreviewer(this IDocument document, int port = 12500)
		{
			throw new NotImplementedException("The Previewer application is no longer supprted. Please use a new QuestPDF Companion application by calling ShowInCompanion() or ShowInCompanionAsync() methods.");
		}

		// Token: 0x06000AA5 RID: 2725 RVA: 0x0001B8A0 File Offset: 0x00019AA0
		[Obsolete("The Previewer application is no longer supprted. Please use a new QuestPDF Companion application by calling ShowInCompanion() or ShowInCompanionAsync() methods.")]
		[ExcludeFromCodeCoverage]
		public static Task ShowInPreviewerAsync(this IDocument document, int port = 12500, CancellationToken cancellationToken = default(CancellationToken))
		{
			throw new NotImplementedException("The Previewer application is no longer supprted. Please use a new QuestPDF Companion application by calling ShowInCompanion() or ShowInCompanionAsync() methods.");
		}

		// Token: 0x0400041F RID: 1055
		private const string ObsoleteMessage = "The Previewer application is no longer supprted. Please use a new QuestPDF Companion application by calling ShowInCompanion() or ShowInCompanionAsync() methods.";
	}
}
