using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;

// Token: 0x02000004 RID: 4
[CompilerGenerated]
internal sealed class <>f__AnonymousType2<<modId>j__TPar, <version>j__TPar>
{
	// Token: 0x1700000B RID: 11
	// (get) Token: 0x06000013 RID: 19 RVA: 0x0000250E File Offset: 0x0000070E
	public <modId>j__TPar modId
	{
		get
		{
			return this.<modId>i__Field;
		}
	}

	// Token: 0x1700000C RID: 12
	// (get) Token: 0x06000014 RID: 20 RVA: 0x00002516 File Offset: 0x00000716
	public <version>j__TPar version
	{
		get
		{
			return this.<version>i__Field;
		}
	}

	// Token: 0x06000015 RID: 21 RVA: 0x0000251E File Offset: 0x0000071E
	[DebuggerHidden]
	public <>f__AnonymousType2(<modId>j__TPar modId, <version>j__TPar version)
	{
		this.<modId>i__Field = modId;
		this.<version>i__Field = version;
	}

	// Token: 0x06000016 RID: 22 RVA: 0x00002534 File Offset: 0x00000734
	[DebuggerHidden]
	public override bool Equals(object value)
	{
		var <>f__AnonymousType = value as <>f__AnonymousType2<<modId>j__TPar, <version>j__TPar>;
		return this == <>f__AnonymousType || (<>f__AnonymousType != null && EqualityComparer<<modId>j__TPar>.Default.Equals(this.<modId>i__Field, <>f__AnonymousType.<modId>i__Field) && EqualityComparer<<version>j__TPar>.Default.Equals(this.<version>i__Field, <>f__AnonymousType.<version>i__Field));
	}

	// Token: 0x06000017 RID: 23 RVA: 0x00002581 File Offset: 0x00000781
	[DebuggerHidden]
	public override int GetHashCode()
	{
		return (-54186009 * -1521134295 + EqualityComparer<<modId>j__TPar>.Default.GetHashCode(this.<modId>i__Field)) * -1521134295 + EqualityComparer<<version>j__TPar>.Default.GetHashCode(this.<version>i__Field);
	}

	// Token: 0x06000018 RID: 24 RVA: 0x000025B8 File Offset: 0x000007B8
	[DebuggerHidden]
	[return: Nullable(1)]
	public override string ToString()
	{
		IFormatProvider formatProvider = null;
		string text = "{{ modId = {0}, version = {1} }}";
		object[] array = new object[2];
		int num = 0;
		<modId>j__TPar <modId>j__TPar = this.<modId>i__Field;
		array[num] = ((<modId>j__TPar != null) ? <modId>j__TPar.ToString() : null);
		int num2 = 1;
		<version>j__TPar <version>j__TPar = this.<version>i__Field;
		array[num2] = ((<version>j__TPar != null) ? <version>j__TPar.ToString() : null);
		return string.Format(formatProvider, text, array);
	}

	// Token: 0x0400000B RID: 11
	[DebuggerBrowsable(0)]
	private readonly <modId>j__TPar <modId>i__Field;

	// Token: 0x0400000C RID: 12
	[DebuggerBrowsable(0)]
	private readonly <version>j__TPar <version>i__Field;
}
