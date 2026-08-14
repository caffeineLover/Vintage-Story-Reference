using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;

// Token: 0x02000003 RID: 3
[CompilerGenerated]
internal sealed class <>f__AnonymousType1<<Property>j__TPar, <Value>j__TPar>
{
	// Token: 0x17000004 RID: 4
	// (get) Token: 0x06000008 RID: 8 RVA: 0x000021DE File Offset: 0x000003DE
	public <Property>j__TPar Property
	{
		get
		{
			return this.<Property>i__Field;
		}
	}

	// Token: 0x17000005 RID: 5
	// (get) Token: 0x06000009 RID: 9 RVA: 0x000021E6 File Offset: 0x000003E6
	public <Value>j__TPar Value
	{
		get
		{
			return this.<Value>i__Field;
		}
	}

	// Token: 0x0600000A RID: 10 RVA: 0x000021EE File Offset: 0x000003EE
	[DebuggerHidden]
	public <>f__AnonymousType1(<Property>j__TPar Property, <Value>j__TPar Value)
	{
		this.<Property>i__Field = Property;
		this.<Value>i__Field = Value;
	}

	// Token: 0x0600000B RID: 11 RVA: 0x00002204 File Offset: 0x00000404
	[DebuggerHidden]
	public override bool Equals(object value)
	{
		var <>f__AnonymousType = value as <>f__AnonymousType1<<Property>j__TPar, <Value>j__TPar>;
		return this == <>f__AnonymousType || (<>f__AnonymousType != null && EqualityComparer<<Property>j__TPar>.Default.Equals(this.<Property>i__Field, <>f__AnonymousType.<Property>i__Field) && EqualityComparer<<Value>j__TPar>.Default.Equals(this.<Value>i__Field, <>f__AnonymousType.<Value>i__Field));
	}

	// Token: 0x0600000C RID: 12 RVA: 0x00002251 File Offset: 0x00000451
	[DebuggerHidden]
	public override int GetHashCode()
	{
		return (-342207088 * -1521134295 + EqualityComparer<<Property>j__TPar>.Default.GetHashCode(this.<Property>i__Field)) * -1521134295 + EqualityComparer<<Value>j__TPar>.Default.GetHashCode(this.<Value>i__Field);
	}

	// Token: 0x0600000D RID: 13 RVA: 0x00002288 File Offset: 0x00000488
	[DebuggerHidden]
	[return: Nullable(1)]
	public override string ToString()
	{
		IFormatProvider formatProvider = null;
		string text = "{{ Property = {0}, Value = {1} }}";
		object[] array = new object[2];
		int num = 0;
		<Property>j__TPar <Property>j__TPar = this.<Property>i__Field;
		array[num] = ((<Property>j__TPar != null) ? <Property>j__TPar.ToString() : null);
		int num2 = 1;
		<Value>j__TPar <Value>j__TPar = this.<Value>i__Field;
		array[num2] = ((<Value>j__TPar != null) ? <Value>j__TPar.ToString() : null);
		return string.Format(formatProvider, text, array);
	}

	// Token: 0x04000004 RID: 4
	[DebuggerBrowsable(0)]
	private readonly <Property>j__TPar <Property>i__Field;

	// Token: 0x04000005 RID: 5
	[DebuggerBrowsable(0)]
	private readonly <Value>j__TPar <Value>i__Field;
}
