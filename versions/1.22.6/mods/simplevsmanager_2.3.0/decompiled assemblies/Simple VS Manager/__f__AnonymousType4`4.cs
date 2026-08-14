using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;

// Token: 0x02000006 RID: 6
[CompilerGenerated]
internal sealed class <>f__AnonymousType4<<playerUid>j__TPar, <sanitizedPlayerUid>j__TPar, <playerName>j__TPar, <lastUpdated>j__TPar>
{
	// Token: 0x1700000E RID: 14
	// (get) Token: 0x0600001E RID: 30 RVA: 0x000026DA File Offset: 0x000008DA
	public <playerUid>j__TPar playerUid
	{
		get
		{
			return this.<playerUid>i__Field;
		}
	}

	// Token: 0x1700000F RID: 15
	// (get) Token: 0x0600001F RID: 31 RVA: 0x000026E2 File Offset: 0x000008E2
	public <sanitizedPlayerUid>j__TPar sanitizedPlayerUid
	{
		get
		{
			return this.<sanitizedPlayerUid>i__Field;
		}
	}

	// Token: 0x17000010 RID: 16
	// (get) Token: 0x06000020 RID: 32 RVA: 0x000026EA File Offset: 0x000008EA
	public <playerName>j__TPar playerName
	{
		get
		{
			return this.<playerName>i__Field;
		}
	}

	// Token: 0x17000011 RID: 17
	// (get) Token: 0x06000021 RID: 33 RVA: 0x000026F2 File Offset: 0x000008F2
	public <lastUpdated>j__TPar lastUpdated
	{
		get
		{
			return this.<lastUpdated>i__Field;
		}
	}

	// Token: 0x06000022 RID: 34 RVA: 0x000026FA File Offset: 0x000008FA
	[DebuggerHidden]
	public <>f__AnonymousType4(<playerUid>j__TPar playerUid, <sanitizedPlayerUid>j__TPar sanitizedPlayerUid, <playerName>j__TPar playerName, <lastUpdated>j__TPar lastUpdated)
	{
		this.<playerUid>i__Field = playerUid;
		this.<sanitizedPlayerUid>i__Field = sanitizedPlayerUid;
		this.<playerName>i__Field = playerName;
		this.<lastUpdated>i__Field = lastUpdated;
	}

	// Token: 0x06000023 RID: 35 RVA: 0x00002720 File Offset: 0x00000920
	[DebuggerHidden]
	public override bool Equals(object value)
	{
		var <>f__AnonymousType = value as <>f__AnonymousType4<<playerUid>j__TPar, <sanitizedPlayerUid>j__TPar, <playerName>j__TPar, <lastUpdated>j__TPar>;
		return this == <>f__AnonymousType || (<>f__AnonymousType != null && EqualityComparer<<playerUid>j__TPar>.Default.Equals(this.<playerUid>i__Field, <>f__AnonymousType.<playerUid>i__Field) && EqualityComparer<<sanitizedPlayerUid>j__TPar>.Default.Equals(this.<sanitizedPlayerUid>i__Field, <>f__AnonymousType.<sanitizedPlayerUid>i__Field) && EqualityComparer<<playerName>j__TPar>.Default.Equals(this.<playerName>i__Field, <>f__AnonymousType.<playerName>i__Field) && EqualityComparer<<lastUpdated>j__TPar>.Default.Equals(this.<lastUpdated>i__Field, <>f__AnonymousType.<lastUpdated>i__Field));
	}

	// Token: 0x06000024 RID: 36 RVA: 0x000027A0 File Offset: 0x000009A0
	[DebuggerHidden]
	public override int GetHashCode()
	{
		return (((-223940168 * -1521134295 + EqualityComparer<<playerUid>j__TPar>.Default.GetHashCode(this.<playerUid>i__Field)) * -1521134295 + EqualityComparer<<sanitizedPlayerUid>j__TPar>.Default.GetHashCode(this.<sanitizedPlayerUid>i__Field)) * -1521134295 + EqualityComparer<<playerName>j__TPar>.Default.GetHashCode(this.<playerName>i__Field)) * -1521134295 + EqualityComparer<<lastUpdated>j__TPar>.Default.GetHashCode(this.<lastUpdated>i__Field);
	}

	// Token: 0x06000025 RID: 37 RVA: 0x00002810 File Offset: 0x00000A10
	[DebuggerHidden]
	[return: Nullable(1)]
	public override string ToString()
	{
		IFormatProvider formatProvider = null;
		string text = "{{ playerUid = {0}, sanitizedPlayerUid = {1}, playerName = {2}, lastUpdated = {3} }}";
		object[] array = new object[4];
		int num = 0;
		<playerUid>j__TPar <playerUid>j__TPar = this.<playerUid>i__Field;
		array[num] = ((<playerUid>j__TPar != null) ? <playerUid>j__TPar.ToString() : null);
		int num2 = 1;
		<sanitizedPlayerUid>j__TPar <sanitizedPlayerUid>j__TPar = this.<sanitizedPlayerUid>i__Field;
		array[num2] = ((<sanitizedPlayerUid>j__TPar != null) ? <sanitizedPlayerUid>j__TPar.ToString() : null);
		int num3 = 2;
		<playerName>j__TPar <playerName>j__TPar = this.<playerName>i__Field;
		array[num3] = ((<playerName>j__TPar != null) ? <playerName>j__TPar.ToString() : null);
		int num4 = 3;
		<lastUpdated>j__TPar <lastUpdated>j__TPar = this.<lastUpdated>i__Field;
		array[num4] = ((<lastUpdated>j__TPar != null) ? <lastUpdated>j__TPar.ToString() : null);
		return string.Format(formatProvider, text, array);
	}

	// Token: 0x0400000E RID: 14
	[DebuggerBrowsable(0)]
	private readonly <playerUid>j__TPar <playerUid>i__Field;

	// Token: 0x0400000F RID: 15
	[DebuggerBrowsable(0)]
	private readonly <sanitizedPlayerUid>j__TPar <sanitizedPlayerUid>i__Field;

	// Token: 0x04000010 RID: 16
	[DebuggerBrowsable(0)]
	private readonly <playerName>j__TPar <playerName>i__Field;

	// Token: 0x04000011 RID: 17
	[DebuggerBrowsable(0)]
	private readonly <lastUpdated>j__TPar <lastUpdated>i__Field;
}
