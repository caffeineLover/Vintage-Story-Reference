using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;

// Token: 0x0200000A RID: 10
[CompilerGenerated]
internal sealed class <>f__AnonymousType8<<Mod>j__TPar, <Score>j__TPar, <Index>j__TPar>
{
	// Token: 0x1700001A RID: 26
	// (get) Token: 0x0600003A RID: 58 RVA: 0x00002CFE File Offset: 0x00000EFE
	public <Mod>j__TPar Mod
	{
		get
		{
			return this.<Mod>i__Field;
		}
	}

	// Token: 0x1700001B RID: 27
	// (get) Token: 0x0600003B RID: 59 RVA: 0x00002D06 File Offset: 0x00000F06
	public <Score>j__TPar Score
	{
		get
		{
			return this.<Score>i__Field;
		}
	}

	// Token: 0x1700001C RID: 28
	// (get) Token: 0x0600003C RID: 60 RVA: 0x00002D0E File Offset: 0x00000F0E
	public <Index>j__TPar Index
	{
		get
		{
			return this.<Index>i__Field;
		}
	}

	// Token: 0x0600003D RID: 61 RVA: 0x00002D16 File Offset: 0x00000F16
	[DebuggerHidden]
	public <>f__AnonymousType8(<Mod>j__TPar Mod, <Score>j__TPar Score, <Index>j__TPar Index)
	{
		this.<Mod>i__Field = Mod;
		this.<Score>i__Field = Score;
		this.<Index>i__Field = Index;
	}

	// Token: 0x0600003E RID: 62 RVA: 0x00002D34 File Offset: 0x00000F34
	[DebuggerHidden]
	public override bool Equals(object value)
	{
		var <>f__AnonymousType = value as <>f__AnonymousType8<<Mod>j__TPar, <Score>j__TPar, <Index>j__TPar>;
		return this == <>f__AnonymousType || (<>f__AnonymousType != null && EqualityComparer<<Mod>j__TPar>.Default.Equals(this.<Mod>i__Field, <>f__AnonymousType.<Mod>i__Field) && EqualityComparer<<Score>j__TPar>.Default.Equals(this.<Score>i__Field, <>f__AnonymousType.<Score>i__Field) && EqualityComparer<<Index>j__TPar>.Default.Equals(this.<Index>i__Field, <>f__AnonymousType.<Index>i__Field));
	}

	// Token: 0x0600003F RID: 63 RVA: 0x00002D9C File Offset: 0x00000F9C
	[DebuggerHidden]
	public override int GetHashCode()
	{
		return ((-1283431004 * -1521134295 + EqualityComparer<<Mod>j__TPar>.Default.GetHashCode(this.<Mod>i__Field)) * -1521134295 + EqualityComparer<<Score>j__TPar>.Default.GetHashCode(this.<Score>i__Field)) * -1521134295 + EqualityComparer<<Index>j__TPar>.Default.GetHashCode(this.<Index>i__Field);
	}

	// Token: 0x06000040 RID: 64 RVA: 0x00002DF4 File Offset: 0x00000FF4
	[DebuggerHidden]
	[return: Nullable(1)]
	public override string ToString()
	{
		IFormatProvider formatProvider = null;
		string text = "{{ Mod = {0}, Score = {1}, Index = {2} }}";
		object[] array = new object[3];
		int num = 0;
		<Mod>j__TPar <Mod>j__TPar = this.<Mod>i__Field;
		array[num] = ((<Mod>j__TPar != null) ? <Mod>j__TPar.ToString() : null);
		int num2 = 1;
		<Score>j__TPar <Score>j__TPar = this.<Score>i__Field;
		array[num2] = ((<Score>j__TPar != null) ? <Score>j__TPar.ToString() : null);
		int num3 = 2;
		<Index>j__TPar <Index>j__TPar = this.<Index>i__Field;
		array[num3] = ((<Index>j__TPar != null) ? <Index>j__TPar.ToString() : null);
		return string.Format(formatProvider, text, array);
	}

	// Token: 0x0400001A RID: 26
	[DebuggerBrowsable(0)]
	private readonly <Mod>j__TPar <Mod>i__Field;

	// Token: 0x0400001B RID: 27
	[DebuggerBrowsable(0)]
	private readonly <Score>j__TPar <Score>i__Field;

	// Token: 0x0400001C RID: 28
	[DebuggerBrowsable(0)]
	private readonly <Index>j__TPar <Index>i__Field;
}
