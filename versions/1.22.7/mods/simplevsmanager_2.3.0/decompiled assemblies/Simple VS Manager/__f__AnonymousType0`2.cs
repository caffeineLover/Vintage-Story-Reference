using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;

// Token: 0x02000002 RID: 2
[CompilerGenerated]
internal sealed class <>f__AnonymousType0<<content>j__TPar, <dateAdded>j__TPar>
{
	// Token: 0x17000001 RID: 1
	// (get) Token: 0x06000001 RID: 1 RVA: 0x00002048 File Offset: 0x00000248
	public <content>j__TPar content
	{
		get
		{
			return this.<content>i__Field;
		}
	}

	// Token: 0x17000002 RID: 2
	// (get) Token: 0x06000002 RID: 2 RVA: 0x00002050 File Offset: 0x00000250
	public <dateAdded>j__TPar dateAdded
	{
		get
		{
			return this.<dateAdded>i__Field;
		}
	}

	// Token: 0x06000003 RID: 3 RVA: 0x00002058 File Offset: 0x00000258
	[DebuggerHidden]
	public <>f__AnonymousType0(<content>j__TPar content, <dateAdded>j__TPar dateAdded)
	{
		this.<content>i__Field = content;
		this.<dateAdded>i__Field = dateAdded;
	}

	// Token: 0x06000004 RID: 4 RVA: 0x00002070 File Offset: 0x00000270
	[DebuggerHidden]
	public override bool Equals(object value)
	{
		var <>f__AnonymousType = value as <>f__AnonymousType0<<content>j__TPar, <dateAdded>j__TPar>;
		return this == <>f__AnonymousType || (<>f__AnonymousType != null && EqualityComparer<<content>j__TPar>.Default.Equals(this.<content>i__Field, <>f__AnonymousType.<content>i__Field) && EqualityComparer<<dateAdded>j__TPar>.Default.Equals(this.<dateAdded>i__Field, <>f__AnonymousType.<dateAdded>i__Field));
	}

	// Token: 0x06000005 RID: 5 RVA: 0x000020BD File Offset: 0x000002BD
	[DebuggerHidden]
	public override int GetHashCode()
	{
		return (-1993215855 * -1521134295 + EqualityComparer<<content>j__TPar>.Default.GetHashCode(this.<content>i__Field)) * -1521134295 + EqualityComparer<<dateAdded>j__TPar>.Default.GetHashCode(this.<dateAdded>i__Field);
	}

	// Token: 0x06000006 RID: 6 RVA: 0x000020F4 File Offset: 0x000002F4
	[DebuggerHidden]
	[return: Nullable(1)]
	public override string ToString()
	{
		IFormatProvider formatProvider = null;
		string text = "{{ content = {0}, dateAdded = {1} }}";
		object[] array = new object[2];
		int num = 0;
		<content>j__TPar <content>j__TPar = this.<content>i__Field;
		array[num] = ((<content>j__TPar != null) ? <content>j__TPar.ToString() : null);
		int num2 = 1;
		<dateAdded>j__TPar <dateAdded>j__TPar = this.<dateAdded>i__Field;
		array[num2] = ((<dateAdded>j__TPar != null) ? <dateAdded>j__TPar.ToString() : null);
		return string.Format(formatProvider, text, array);
	}

	// Token: 0x04000001 RID: 1
	[DebuggerBrowsable(0)]
	private readonly <content>j__TPar <content>i__Field;

	// Token: 0x04000002 RID: 2
	[DebuggerBrowsable(0)]
	private readonly <dateAdded>j__TPar <dateAdded>i__Field;
}
