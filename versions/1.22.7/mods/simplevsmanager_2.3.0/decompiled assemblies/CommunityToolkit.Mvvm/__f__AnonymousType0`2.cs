using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;

// Token: 0x02000002 RID: 2
[CompilerGenerated]
internal sealed class <>f__AnonymousType0<<property>j__TPar, <getMethod>j__TPar>
{
	// Token: 0x17000001 RID: 1
	// (get) Token: 0x06000001 RID: 1 RVA: 0x00002050 File Offset: 0x00000250
	public <property>j__TPar property
	{
		get
		{
			return this.<property>i__Field;
		}
	}

	// Token: 0x17000002 RID: 2
	// (get) Token: 0x06000002 RID: 2 RVA: 0x00002058 File Offset: 0x00000258
	public <getMethod>j__TPar getMethod
	{
		get
		{
			return this.<getMethod>i__Field;
		}
	}

	// Token: 0x06000003 RID: 3 RVA: 0x00002060 File Offset: 0x00000260
	[DebuggerHidden]
	public <>f__AnonymousType0(<property>j__TPar property, <getMethod>j__TPar getMethod)
	{
		this.<property>i__Field = property;
		this.<getMethod>i__Field = getMethod;
	}

	// Token: 0x06000004 RID: 4 RVA: 0x00002078 File Offset: 0x00000278
	[DebuggerHidden]
	public override bool Equals(object value)
	{
		var <>f__AnonymousType = value as <>f__AnonymousType0<<property>j__TPar, <getMethod>j__TPar>;
		return this == <>f__AnonymousType || (<>f__AnonymousType != null && EqualityComparer<<property>j__TPar>.Default.Equals(this.<property>i__Field, <>f__AnonymousType.<property>i__Field) && EqualityComparer<<getMethod>j__TPar>.Default.Equals(this.<getMethod>i__Field, <>f__AnonymousType.<getMethod>i__Field));
	}

	// Token: 0x06000005 RID: 5 RVA: 0x000020C5 File Offset: 0x000002C5
	[DebuggerHidden]
	public override int GetHashCode()
	{
		return (834508232 * -1521134295 + EqualityComparer<<property>j__TPar>.Default.GetHashCode(this.<property>i__Field)) * -1521134295 + EqualityComparer<<getMethod>j__TPar>.Default.GetHashCode(this.<getMethod>i__Field);
	}

	// Token: 0x06000006 RID: 6 RVA: 0x000020FC File Offset: 0x000002FC
	[DebuggerHidden]
	[return: Nullable(1)]
	public override string ToString()
	{
		IFormatProvider formatProvider = null;
		string text = "{{ property = {0}, getMethod = {1} }}";
		object[] array = new object[2];
		int num = 0;
		<property>j__TPar <property>j__TPar = this.<property>i__Field;
		array[num] = ((<property>j__TPar != null) ? <property>j__TPar.ToString() : null);
		int num2 = 1;
		<getMethod>j__TPar <getMethod>j__TPar = this.<getMethod>i__Field;
		array[num2] = ((<getMethod>j__TPar != null) ? <getMethod>j__TPar.ToString() : null);
		return string.Format(formatProvider, text, array);
	}

	// Token: 0x04000001 RID: 1
	[DebuggerBrowsable(0)]
	private readonly <property>j__TPar <property>i__Field;

	// Token: 0x04000002 RID: 2
	[DebuggerBrowsable(0)]
	private readonly <getMethod>j__TPar <getMethod>i__Field;
}
