using System;
using System.Runtime.CompilerServices;
using QuestPDF.Elements;
using QuestPDF.Infrastructure;

namespace QuestPDF.Fluent
{
	// Token: 0x0200008F RID: 143
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class InlinedDescriptor
	{
		// Token: 0x17000101 RID: 257
		// (get) Token: 0x060003F4 RID: 1012 RVA: 0x0000AA52 File Offset: 0x00008C52
		internal Inlined Inlined { get; } = new Inlined();

		// Token: 0x060003F5 RID: 1013 RVA: 0x0000AA5A File Offset: 0x00008C5A
		internal InlinedDescriptor()
		{
		}

		// Token: 0x060003F6 RID: 1014 RVA: 0x0000AA6D File Offset: 0x00008C6D
		public void Spacing(float value, Unit unit = Unit.Point)
		{
			this.VerticalSpacing(value, unit);
			this.HorizontalSpacing(value, unit);
		}

		// Token: 0x060003F7 RID: 1015 RVA: 0x0000AA7F File Offset: 0x00008C7F
		public void VerticalSpacing(float value, Unit unit = Unit.Point)
		{
			if (value < 0f)
			{
				throw new ArgumentOutOfRangeException("value", "The Inlined vertical spacing cannot be negative.");
			}
			this.Inlined.VerticalSpacing = value.ToPoints(unit);
		}

		// Token: 0x060003F8 RID: 1016 RVA: 0x0000AAAB File Offset: 0x00008CAB
		public void HorizontalSpacing(float value, Unit unit = Unit.Point)
		{
			if (value < 0f)
			{
				throw new ArgumentOutOfRangeException("value", "The Inlined horizontal spacing cannot be negative.");
			}
			this.Inlined.HorizontalSpacing = value.ToPoints(unit);
		}

		// Token: 0x060003F9 RID: 1017 RVA: 0x0000AAD7 File Offset: 0x00008CD7
		public void BaselineTop()
		{
			this.Inlined.BaselineAlignment = VerticalAlignment.Top;
		}

		// Token: 0x060003FA RID: 1018 RVA: 0x0000AAE5 File Offset: 0x00008CE5
		public void BaselineMiddle()
		{
			this.Inlined.BaselineAlignment = VerticalAlignment.Middle;
		}

		// Token: 0x060003FB RID: 1019 RVA: 0x0000AAF3 File Offset: 0x00008CF3
		public void BaselineBottom()
		{
			this.Inlined.BaselineAlignment = VerticalAlignment.Bottom;
		}

		// Token: 0x060003FC RID: 1020 RVA: 0x0000AB01 File Offset: 0x00008D01
		internal void Alignment(InlinedAlignment? alignment)
		{
			this.Inlined.ElementsAlignment = alignment;
		}

		// Token: 0x060003FD RID: 1021 RVA: 0x0000AB0F File Offset: 0x00008D0F
		public void AlignLeft()
		{
			this.Inlined.ElementsAlignment = new InlinedAlignment?(InlinedAlignment.Left);
		}

		// Token: 0x060003FE RID: 1022 RVA: 0x0000AB22 File Offset: 0x00008D22
		public void AlignCenter()
		{
			this.Inlined.ElementsAlignment = new InlinedAlignment?(InlinedAlignment.Center);
		}

		// Token: 0x060003FF RID: 1023 RVA: 0x0000AB35 File Offset: 0x00008D35
		public void AlignRight()
		{
			this.Inlined.ElementsAlignment = new InlinedAlignment?(InlinedAlignment.Right);
		}

		// Token: 0x06000400 RID: 1024 RVA: 0x0000AB48 File Offset: 0x00008D48
		public void AlignJustify()
		{
			this.Inlined.ElementsAlignment = new InlinedAlignment?(InlinedAlignment.Justify);
		}

		// Token: 0x06000401 RID: 1025 RVA: 0x0000AB5B File Offset: 0x00008D5B
		public void AlignSpaceAround()
		{
			this.Inlined.ElementsAlignment = new InlinedAlignment?(InlinedAlignment.SpaceAround);
		}

		// Token: 0x06000402 RID: 1026 RVA: 0x0000AB70 File Offset: 0x00008D70
		public IContainer Item()
		{
			Constrained constrained = new Constrained();
			this.Inlined.Elements.Add(constrained);
			return constrained;
		}
	}
}
