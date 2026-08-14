using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;

// Token: 0x02000005 RID: 5
[CompilerGenerated]
internal sealed class <>f__AnonymousType3<<idToken>j__TPar>
{
	// Token: 0x1700000D RID: 13
	// (get) Token: 0x06000019 RID: 25 RVA: 0x00002626 File Offset: 0x00000826
	public <idToken>j__TPar idToken
	{
		get
		{
			return this.<idToken>i__Field;
		}
	}

	// Token: 0x0600001A RID: 26 RVA: 0x0000262E File Offset: 0x0000082E
	[DebuggerHidden]
	public <>f__AnonymousType3(<idToken>j__TPar idToken)
	{
		this.<idToken>i__Field = idToken;
	}

	// Token: 0x0600001B RID: 27 RVA: 0x00002640 File Offset: 0x00000840
	[DebuggerHidden]
	public override bool Equals(object value)
	{
		var <>f__AnonymousType = value as <>f__AnonymousType3<<idToken>j__TPar>;
		return this == <>f__AnonymousType || (<>f__AnonymousType != null && EqualityComparer<<idToken>j__TPar>.Default.Equals(this.<idToken>i__Field, <>f__AnonymousType.<idToken>i__Field));
	}

	// Token: 0x0600001C RID: 28 RVA: 0x00002675 File Offset: 0x00000875
	[DebuggerHidden]
	public override int GetHashCode()
	{
		return -1398509060 * -1521134295 + EqualityComparer<<idToken>j__TPar>.Default.GetHashCode(this.<idToken>i__Field);
	}

	// Token: 0x0600001D RID: 29 RVA: 0x00002694 File Offset: 0x00000894
	[DebuggerHidden]
	[return: Nullable(1)]
	public override string ToString()
	{
		IFormatProvider formatProvider = null;
		string text = "{{ idToken = {0} }}";
		object[] array = new object[1];
		int num = 0;
		<idToken>j__TPar <idToken>j__TPar = this.<idToken>i__Field;
		array[num] = ((<idToken>j__TPar != null) ? <idToken>j__TPar.ToString() : null);
		return string.Format(formatProvider, text, array);
	}

	// Token: 0x0400000D RID: 13
	[DebuggerBrowsable(0)]
	private readonly <idToken>j__TPar <idToken>i__Field;
}
