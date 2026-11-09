using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using QuestPDF.Drawing.Exceptions;
using QuestPDF.Elements;
using QuestPDF.Infrastructure;

namespace QuestPDF.Fluent
{
	// Token: 0x02000083 RID: 131
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class DecorationDescriptor
	{
		// Token: 0x170000FA RID: 250
		// (get) Token: 0x06000370 RID: 880 RVA: 0x000093A7 File Offset: 0x000075A7
		internal Decoration Decoration { get; } = new Decoration();

		// Token: 0x06000371 RID: 881 RVA: 0x000093AF File Offset: 0x000075AF
		internal DecorationDescriptor()
		{
		}

		// Token: 0x06000372 RID: 882 RVA: 0x000093C4 File Offset: 0x000075C4
		public IContainer Before()
		{
			Element before = this.Decoration.Before;
			if (!(before is Empty) && !(before is DebugPointer))
			{
				throw new DocumentComposeException("The 'Decoration.Before' layer has already been defined. Please call this method only once.");
			}
			Container container = new Container();
			this.Decoration.Before = container;
			return container.DebugPointer(DebugPointerType.ElementStructure, "Before").RepeatAsHeader();
		}

		// Token: 0x06000373 RID: 883 RVA: 0x00009424 File Offset: 0x00007624
		public void Before(Action<IContainer> handler)
		{
			if (handler != null)
			{
				handler.Invoke(this.Before());
			}
		}

		// Token: 0x06000374 RID: 884 RVA: 0x00009438 File Offset: 0x00007638
		public IContainer Content()
		{
			Element content = this.Decoration.Content;
			if (!(content is Empty) && !(content is DebugPointer))
			{
				throw new DocumentComposeException("The 'Decoration.Content' layer has already been defined. Please call this method only once.");
			}
			Container container = new Container();
			this.Decoration.Content = container;
			return container.DebugPointer(DebugPointerType.ElementStructure, "Content");
		}

		// Token: 0x06000375 RID: 885 RVA: 0x00009493 File Offset: 0x00007693
		public void Content(Action<IContainer> handler)
		{
			if (handler != null)
			{
				handler.Invoke(this.Content());
			}
		}

		// Token: 0x06000376 RID: 886 RVA: 0x000094A4 File Offset: 0x000076A4
		public IContainer After()
		{
			Element after = this.Decoration.After;
			if (!(after is Empty) && !(after is DebugPointer))
			{
				throw new DocumentComposeException("The 'Decoration.After' layer has already been defined. Please call this method only once.");
			}
			Container container = new Container();
			this.Decoration.After = container;
			return container.DebugPointer(DebugPointerType.ElementStructure, "After").RepeatAsFooter();
		}

		// Token: 0x06000377 RID: 887 RVA: 0x00009504 File Offset: 0x00007704
		public void After(Action<IContainer> handler)
		{
			if (handler != null)
			{
				handler.Invoke(this.After());
			}
		}

		// Token: 0x06000378 RID: 888 RVA: 0x00009518 File Offset: 0x00007718
		[Obsolete("This element has been renamed since version 2022.2. Please use the 'Before' method.")]
		[ExcludeFromCodeCoverage]
		public IContainer Header()
		{
			Container container = new Container();
			this.Decoration.Before = container;
			return container;
		}

		// Token: 0x06000379 RID: 889 RVA: 0x00009538 File Offset: 0x00007738
		[Obsolete("This element has been renamed since version 2022.2. Please use the 'Before' method.")]
		[ExcludeFromCodeCoverage]
		public void Header(Action<IContainer> handler)
		{
			if (handler != null)
			{
				handler.Invoke(this.Header());
			}
		}

		// Token: 0x0600037A RID: 890 RVA: 0x0000954C File Offset: 0x0000774C
		[Obsolete("This element has been renamed since version 2022.2. Please use the 'After' method.")]
		[ExcludeFromCodeCoverage]
		public IContainer Footer()
		{
			Container container = new Container();
			this.Decoration.After = container;
			return container;
		}

		// Token: 0x0600037B RID: 891 RVA: 0x0000956C File Offset: 0x0000776C
		[Obsolete("This element has been renamed since version 2022.2. Please use the 'After' method.")]
		[ExcludeFromCodeCoverage]
		public void Footer(Action<IContainer> handler)
		{
			if (handler != null)
			{
				handler.Invoke(this.Footer());
			}
		}
	}
}
