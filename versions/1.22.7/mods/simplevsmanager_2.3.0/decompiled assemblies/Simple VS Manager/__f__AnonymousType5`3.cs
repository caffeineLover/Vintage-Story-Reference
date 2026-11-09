using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;

// Token: 0x02000007 RID: 7
[CompilerGenerated]
internal sealed class <>f__AnonymousType5<<Candidate>j__TPar, <Index>j__TPar, <SortKey>j__TPar>
{
	// Token: 0x17000012 RID: 18
	// (get) Token: 0x06000026 RID: 38 RVA: 0x000028CE File Offset: 0x00000ACE
	public <Candidate>j__TPar Candidate
	{
		get
		{
			return this.<Candidate>i__Field;
		}
	}

	// Token: 0x17000013 RID: 19
	// (get) Token: 0x06000027 RID: 39 RVA: 0x000028D6 File Offset: 0x00000AD6
	public <Index>j__TPar Index
	{
		get
		{
			return this.<Index>i__Field;
		}
	}

	// Token: 0x17000014 RID: 20
	// (get) Token: 0x06000028 RID: 40 RVA: 0x000028DE File Offset: 0x00000ADE
	public <SortKey>j__TPar SortKey
	{
		get
		{
			return this.<SortKey>i__Field;
		}
	}

	// Token: 0x06000029 RID: 41 RVA: 0x000028E6 File Offset: 0x00000AE6
	[DebuggerHidden]
	public <>f__AnonymousType5(<Candidate>j__TPar Candidate, <Index>j__TPar Index, <SortKey>j__TPar SortKey)
	{
		this.<Candidate>i__Field = Candidate;
		this.<Index>i__Field = Index;
		this.<SortKey>i__Field = SortKey;
	}

	// Token: 0x0600002A RID: 42 RVA: 0x00002904 File Offset: 0x00000B04
	[DebuggerHidden]
	public override bool Equals(object value)
	{
		var <>f__AnonymousType = value as <>f__AnonymousType5<<Candidate>j__TPar, <Index>j__TPar, <SortKey>j__TPar>;
		return this == <>f__AnonymousType || (<>f__AnonymousType != null && EqualityComparer<<Candidate>j__TPar>.Default.Equals(this.<Candidate>i__Field, <>f__AnonymousType.<Candidate>i__Field) && EqualityComparer<<Index>j__TPar>.Default.Equals(this.<Index>i__Field, <>f__AnonymousType.<Index>i__Field) && EqualityComparer<<SortKey>j__TPar>.Default.Equals(this.<SortKey>i__Field, <>f__AnonymousType.<SortKey>i__Field));
	}

	// Token: 0x0600002B RID: 43 RVA: 0x0000296C File Offset: 0x00000B6C
	[DebuggerHidden]
	public override int GetHashCode()
	{
		return ((368444202 * -1521134295 + EqualityComparer<<Candidate>j__TPar>.Default.GetHashCode(this.<Candidate>i__Field)) * -1521134295 + EqualityComparer<<Index>j__TPar>.Default.GetHashCode(this.<Index>i__Field)) * -1521134295 + EqualityComparer<<SortKey>j__TPar>.Default.GetHashCode(this.<SortKey>i__Field);
	}

	// Token: 0x0600002C RID: 44 RVA: 0x000029C4 File Offset: 0x00000BC4
	[DebuggerHidden]
	[return: Nullable(1)]
	public override string ToString()
	{
		IFormatProvider formatProvider = null;
		string text = "{{ Candidate = {0}, Index = {1}, SortKey = {2} }}";
		object[] array = new object[3];
		int num = 0;
		<Candidate>j__TPar <Candidate>j__TPar = this.<Candidate>i__Field;
		array[num] = ((<Candidate>j__TPar != null) ? <Candidate>j__TPar.ToString() : null);
		int num2 = 1;
		<Index>j__TPar <Index>j__TPar = this.<Index>i__Field;
		array[num2] = ((<Index>j__TPar != null) ? <Index>j__TPar.ToString() : null);
		int num3 = 2;
		<SortKey>j__TPar <SortKey>j__TPar = this.<SortKey>i__Field;
		array[num3] = ((<SortKey>j__TPar != null) ? <SortKey>j__TPar.ToString() : null);
		return string.Format(formatProvider, text, array);
	}

	// Token: 0x04000012 RID: 18
	[DebuggerBrowsable(0)]
	private readonly <Candidate>j__TPar <Candidate>i__Field;

	// Token: 0x04000013 RID: 19
	[DebuggerBrowsable(0)]
	private readonly <Index>j__TPar <Index>i__Field;

	// Token: 0x04000014 RID: 20
	[DebuggerBrowsable(0)]
	private readonly <SortKey>j__TPar <SortKey>i__Field;
}
