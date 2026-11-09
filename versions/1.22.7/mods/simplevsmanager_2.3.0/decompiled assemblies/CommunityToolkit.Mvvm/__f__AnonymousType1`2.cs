using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;

// Token: 0x02000003 RID: 3
[CompilerGenerated]
internal sealed class <>f__AnonymousType1<<interfaceType>j__TPar, <messageType>j__TPar>
{
	// Token: 0x17000003 RID: 3
	// (get) Token: 0x06000007 RID: 7 RVA: 0x0000216A File Offset: 0x0000036A
	public <interfaceType>j__TPar interfaceType
	{
		get
		{
			return this.<interfaceType>i__Field;
		}
	}

	// Token: 0x17000004 RID: 4
	// (get) Token: 0x06000008 RID: 8 RVA: 0x00002172 File Offset: 0x00000372
	public <messageType>j__TPar messageType
	{
		get
		{
			return this.<messageType>i__Field;
		}
	}

	// Token: 0x06000009 RID: 9 RVA: 0x0000217A File Offset: 0x0000037A
	[DebuggerHidden]
	public <>f__AnonymousType1(<interfaceType>j__TPar interfaceType, <messageType>j__TPar messageType)
	{
		this.<interfaceType>i__Field = interfaceType;
		this.<messageType>i__Field = messageType;
	}

	// Token: 0x0600000A RID: 10 RVA: 0x00002190 File Offset: 0x00000390
	[DebuggerHidden]
	public override bool Equals(object value)
	{
		var <>f__AnonymousType = value as <>f__AnonymousType1<<interfaceType>j__TPar, <messageType>j__TPar>;
		return this == <>f__AnonymousType || (<>f__AnonymousType != null && EqualityComparer<<interfaceType>j__TPar>.Default.Equals(this.<interfaceType>i__Field, <>f__AnonymousType.<interfaceType>i__Field) && EqualityComparer<<messageType>j__TPar>.Default.Equals(this.<messageType>i__Field, <>f__AnonymousType.<messageType>i__Field));
	}

	// Token: 0x0600000B RID: 11 RVA: 0x000021DD File Offset: 0x000003DD
	[DebuggerHidden]
	public override int GetHashCode()
	{
		return (1526856606 * -1521134295 + EqualityComparer<<interfaceType>j__TPar>.Default.GetHashCode(this.<interfaceType>i__Field)) * -1521134295 + EqualityComparer<<messageType>j__TPar>.Default.GetHashCode(this.<messageType>i__Field);
	}

	// Token: 0x0600000C RID: 12 RVA: 0x00002214 File Offset: 0x00000414
	[DebuggerHidden]
	[return: Nullable(1)]
	public override string ToString()
	{
		IFormatProvider formatProvider = null;
		string text = "{{ interfaceType = {0}, messageType = {1} }}";
		object[] array = new object[2];
		int num = 0;
		<interfaceType>j__TPar <interfaceType>j__TPar = this.<interfaceType>i__Field;
		array[num] = ((<interfaceType>j__TPar != null) ? <interfaceType>j__TPar.ToString() : null);
		int num2 = 1;
		<messageType>j__TPar <messageType>j__TPar = this.<messageType>i__Field;
		array[num2] = ((<messageType>j__TPar != null) ? <messageType>j__TPar.ToString() : null);
		return string.Format(formatProvider, text, array);
	}

	// Token: 0x04000003 RID: 3
	[DebuggerBrowsable(0)]
	private readonly <interfaceType>j__TPar <interfaceType>i__Field;

	// Token: 0x04000004 RID: 4
	[DebuggerBrowsable(0)]
	private readonly <messageType>j__TPar <messageType>i__Field;
}
