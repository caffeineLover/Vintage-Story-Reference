using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;

// Token: 0x02000009 RID: 9
[CompilerGenerated]
internal sealed class <>f__AnonymousType7<<version>j__TPar, <configurations>j__TPar>
{
	// Token: 0x17000018 RID: 24
	// (get) Token: 0x06000034 RID: 52 RVA: 0x00002BE6 File Offset: 0x00000DE6
	public <version>j__TPar version
	{
		get
		{
			return this.<version>i__Field;
		}
	}

	// Token: 0x17000019 RID: 25
	// (get) Token: 0x06000035 RID: 53 RVA: 0x00002BEE File Offset: 0x00000DEE
	public <configurations>j__TPar configurations
	{
		get
		{
			return this.<configurations>i__Field;
		}
	}

	// Token: 0x06000036 RID: 54 RVA: 0x00002BF6 File Offset: 0x00000DF6
	[DebuggerHidden]
	public <>f__AnonymousType7(<version>j__TPar version, <configurations>j__TPar configurations)
	{
		this.<version>i__Field = version;
		this.<configurations>i__Field = configurations;
	}

	// Token: 0x06000037 RID: 55 RVA: 0x00002C0C File Offset: 0x00000E0C
	[DebuggerHidden]
	public override bool Equals(object value)
	{
		var <>f__AnonymousType = value as <>f__AnonymousType7<<version>j__TPar, <configurations>j__TPar>;
		return this == <>f__AnonymousType || (<>f__AnonymousType != null && EqualityComparer<<version>j__TPar>.Default.Equals(this.<version>i__Field, <>f__AnonymousType.<version>i__Field) && EqualityComparer<<configurations>j__TPar>.Default.Equals(this.<configurations>i__Field, <>f__AnonymousType.<configurations>i__Field));
	}

	// Token: 0x06000038 RID: 56 RVA: 0x00002C59 File Offset: 0x00000E59
	[DebuggerHidden]
	public override int GetHashCode()
	{
		return (26648707 * -1521134295 + EqualityComparer<<version>j__TPar>.Default.GetHashCode(this.<version>i__Field)) * -1521134295 + EqualityComparer<<configurations>j__TPar>.Default.GetHashCode(this.<configurations>i__Field);
	}

	// Token: 0x06000039 RID: 57 RVA: 0x00002C90 File Offset: 0x00000E90
	[DebuggerHidden]
	[return: Nullable(1)]
	public override string ToString()
	{
		IFormatProvider formatProvider = null;
		string text = "{{ version = {0}, configurations = {1} }}";
		object[] array = new object[2];
		int num = 0;
		<version>j__TPar <version>j__TPar = this.<version>i__Field;
		array[num] = ((<version>j__TPar != null) ? <version>j__TPar.ToString() : null);
		int num2 = 1;
		<configurations>j__TPar <configurations>j__TPar = this.<configurations>i__Field;
		array[num2] = ((<configurations>j__TPar != null) ? <configurations>j__TPar.ToString() : null);
		return string.Format(formatProvider, text, array);
	}

	// Token: 0x04000018 RID: 24
	[DebuggerBrowsable(0)]
	private readonly <version>j__TPar <version>i__Field;

	// Token: 0x04000019 RID: 25
	[DebuggerBrowsable(0)]
	private readonly <configurations>j__TPar <configurations>i__Field;
}
