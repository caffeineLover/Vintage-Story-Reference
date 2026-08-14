using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;

// Token: 0x02000002 RID: 2
[CompilerGenerated]
internal sealed class <>f__AnonymousType0<<value>j__TPar, <index>j__TPar>
{
	// Token: 0x17000001 RID: 1
	// (get) Token: 0x06000001 RID: 1 RVA: 0x00002050 File Offset: 0x00000250
	public <value>j__TPar value
	{
		get
		{
			return this.<value>i__Field;
		}
	}

	// Token: 0x17000002 RID: 2
	// (get) Token: 0x06000002 RID: 2 RVA: 0x00002058 File Offset: 0x00000258
	public <index>j__TPar index
	{
		get
		{
			return this.<index>i__Field;
		}
	}

	// Token: 0x06000003 RID: 3 RVA: 0x00002060 File Offset: 0x00000260
	[DebuggerHidden]
	public <>f__AnonymousType0(<value>j__TPar value, <index>j__TPar index)
	{
		this.<value>i__Field = value;
		this.<index>i__Field = index;
	}

	// Token: 0x06000004 RID: 4 RVA: 0x00002078 File Offset: 0x00000278
	[DebuggerHidden]
	public override bool Equals(object value)
	{
		var <>f__AnonymousType = value as <>f__AnonymousType0<<value>j__TPar, <index>j__TPar>;
		return this == <>f__AnonymousType || (<>f__AnonymousType != null && EqualityComparer<<value>j__TPar>.Default.Equals(this.<value>i__Field, <>f__AnonymousType.<value>i__Field) && EqualityComparer<<index>j__TPar>.Default.Equals(this.<index>i__Field, <>f__AnonymousType.<index>i__Field));
	}

	// Token: 0x06000005 RID: 5 RVA: 0x000020C5 File Offset: 0x000002C5
	[DebuggerHidden]
	public override int GetHashCode()
	{
		return (352133983 * -1521134295 + EqualityComparer<<value>j__TPar>.Default.GetHashCode(this.<value>i__Field)) * -1521134295 + EqualityComparer<<index>j__TPar>.Default.GetHashCode(this.<index>i__Field);
	}

	// Token: 0x06000006 RID: 6 RVA: 0x000020FC File Offset: 0x000002FC
	[DebuggerHidden]
	[return: Nullable(1)]
	public override string ToString()
	{
		IFormatProvider formatProvider = null;
		string text = "{{ value = {0}, index = {1} }}";
		object[] array = new object[2];
		int num = 0;
		<value>j__TPar <value>j__TPar = this.<value>i__Field;
		array[num] = ((<value>j__TPar != null) ? <value>j__TPar.ToString() : null);
		int num2 = 1;
		<index>j__TPar <index>j__TPar = this.<index>i__Field;
		array[num2] = ((<index>j__TPar != null) ? <index>j__TPar.ToString() : null);
		return string.Format(formatProvider, text, array);
	}

	// Token: 0x04000001 RID: 1
	[DebuggerBrowsable(0)]
	private readonly <value>j__TPar <value>i__Field;

	// Token: 0x04000002 RID: 2
	[DebuggerBrowsable(0)]
	private readonly <index>j__TPar <index>i__Field;
}
