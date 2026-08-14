using System;
using System.Linq;
using System.Runtime.CompilerServices;
using QuestPDF.Drawing.Exceptions;
using QuestPDF.Elements;
using QuestPDF.Infrastructure;

namespace QuestPDF.Fluent
{
	// Token: 0x02000091 RID: 145
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class LayersDescriptor
	{
		// Token: 0x17000102 RID: 258
		// (get) Token: 0x06000404 RID: 1028 RVA: 0x0000ABBF File Offset: 0x00008DBF
		internal Layers Layers { get; } = new Layers();

		// Token: 0x06000405 RID: 1029 RVA: 0x0000ABC7 File Offset: 0x00008DC7
		internal LayersDescriptor()
		{
		}

		// Token: 0x06000406 RID: 1030 RVA: 0x0000ABDC File Offset: 0x00008DDC
		private IContainer Layer(bool isPrimary)
		{
			Container container = new Container();
			Layer layer = new Layer
			{
				IsPrimary = isPrimary,
				Child = container
			};
			this.Layers.Children.Add(layer);
			return container;
		}

		// Token: 0x06000407 RID: 1031 RVA: 0x0000AC15 File Offset: 0x00008E15
		public IContainer Layer()
		{
			return this.Layer(false);
		}

		// Token: 0x06000408 RID: 1032 RVA: 0x0000AC1E File Offset: 0x00008E1E
		public IContainer PrimaryLayer()
		{
			return this.Layer(true);
		}

		// Token: 0x06000409 RID: 1033 RVA: 0x0000AC28 File Offset: 0x00008E28
		internal void Validate()
		{
			int num = Enumerable.Count<Layer>(this.Layers.Children, (Layer x) => x.IsPrimary);
			if (num == 0)
			{
				throw new DocumentComposeException("The Layers component needs to have exactly one primary layer. It has none.");
			}
			if (num != 1)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
				defaultInterpolatedStringHandler..ctor(70, 1);
				defaultInterpolatedStringHandler.AppendLiteral("The Layers component needs to have exactly one primary layer. It has ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(num);
				defaultInterpolatedStringHandler.AppendLiteral(".");
				throw new DocumentComposeException(defaultInterpolatedStringHandler.ToStringAndClear());
			}
		}
	}
}
