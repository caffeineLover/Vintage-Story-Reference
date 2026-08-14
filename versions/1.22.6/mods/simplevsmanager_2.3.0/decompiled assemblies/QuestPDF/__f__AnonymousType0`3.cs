using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;

// Token: 0x02000002 RID: 2
[CompilerGenerated]
internal sealed class <>f__AnonymousType0<<DocumentId>j__TPar, <Content>j__TPar, <PageContext>j__TPar>
{
	// Token: 0x17000001 RID: 1
	// (get) Token: 0x06000001 RID: 1 RVA: 0x00002050 File Offset: 0x00000250
	public <DocumentId>j__TPar DocumentId
	{
		get
		{
			return this.<DocumentId>i__Field;
		}
	}

	// Token: 0x17000002 RID: 2
	// (get) Token: 0x06000002 RID: 2 RVA: 0x00002058 File Offset: 0x00000258
	public <Content>j__TPar Content
	{
		get
		{
			return this.<Content>i__Field;
		}
	}

	// Token: 0x17000003 RID: 3
	// (get) Token: 0x06000003 RID: 3 RVA: 0x00002060 File Offset: 0x00000260
	public <PageContext>j__TPar PageContext
	{
		get
		{
			return this.<PageContext>i__Field;
		}
	}

	// Token: 0x06000004 RID: 4 RVA: 0x00002068 File Offset: 0x00000268
	[DebuggerHidden]
	public <>f__AnonymousType0(<DocumentId>j__TPar DocumentId, <Content>j__TPar Content, <PageContext>j__TPar PageContext)
	{
		this.<DocumentId>i__Field = DocumentId;
		this.<Content>i__Field = Content;
		this.<PageContext>i__Field = PageContext;
	}

	// Token: 0x06000005 RID: 5 RVA: 0x00002088 File Offset: 0x00000288
	[DebuggerHidden]
	public override bool Equals(object value)
	{
		var <>f__AnonymousType = value as <>f__AnonymousType0<<DocumentId>j__TPar, <Content>j__TPar, <PageContext>j__TPar>;
		return this == <>f__AnonymousType || (<>f__AnonymousType != null && EqualityComparer<<DocumentId>j__TPar>.Default.Equals(this.<DocumentId>i__Field, <>f__AnonymousType.<DocumentId>i__Field) && EqualityComparer<<Content>j__TPar>.Default.Equals(this.<Content>i__Field, <>f__AnonymousType.<Content>i__Field) && EqualityComparer<<PageContext>j__TPar>.Default.Equals(this.<PageContext>i__Field, <>f__AnonymousType.<PageContext>i__Field));
	}

	// Token: 0x06000006 RID: 6 RVA: 0x000020F0 File Offset: 0x000002F0
	[DebuggerHidden]
	public override int GetHashCode()
	{
		return ((-156349723 * -1521134295 + EqualityComparer<<DocumentId>j__TPar>.Default.GetHashCode(this.<DocumentId>i__Field)) * -1521134295 + EqualityComparer<<Content>j__TPar>.Default.GetHashCode(this.<Content>i__Field)) * -1521134295 + EqualityComparer<<PageContext>j__TPar>.Default.GetHashCode(this.<PageContext>i__Field);
	}

	// Token: 0x06000007 RID: 7 RVA: 0x00002148 File Offset: 0x00000348
	[DebuggerHidden]
	[return: Nullable(1)]
	public override string ToString()
	{
		IFormatProvider formatProvider = null;
		string text = "{{ DocumentId = {0}, Content = {1}, PageContext = {2} }}";
		object[] array = new object[3];
		int num = 0;
		<DocumentId>j__TPar <DocumentId>j__TPar = this.<DocumentId>i__Field;
		array[num] = ((<DocumentId>j__TPar != null) ? <DocumentId>j__TPar.ToString() : null);
		int num2 = 1;
		<Content>j__TPar <Content>j__TPar = this.<Content>i__Field;
		array[num2] = ((<Content>j__TPar != null) ? <Content>j__TPar.ToString() : null);
		int num3 = 2;
		<PageContext>j__TPar <PageContext>j__TPar = this.<PageContext>i__Field;
		array[num3] = ((<PageContext>j__TPar != null) ? <PageContext>j__TPar.ToString() : null);
		return string.Format(formatProvider, text, array);
	}

	// Token: 0x04000001 RID: 1
	[DebuggerBrowsable(0)]
	private readonly <DocumentId>j__TPar <DocumentId>i__Field;

	// Token: 0x04000002 RID: 2
	[DebuggerBrowsable(0)]
	private readonly <Content>j__TPar <Content>i__Field;

	// Token: 0x04000003 RID: 3
	[DebuggerBrowsable(0)]
	private readonly <PageContext>j__TPar <PageContext>i__Field;
}
