using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;

// Token: 0x02000008 RID: 8
[CompilerGenerated]
internal sealed class <>f__AnonymousType6<<fileName>j__TPar, <relativePath>j__TPar, <Content>j__TPar>
{
	// Token: 0x17000015 RID: 21
	// (get) Token: 0x0600002D RID: 45 RVA: 0x00002A5A File Offset: 0x00000C5A
	public <fileName>j__TPar fileName
	{
		get
		{
			return this.<fileName>i__Field;
		}
	}

	// Token: 0x17000016 RID: 22
	// (get) Token: 0x0600002E RID: 46 RVA: 0x00002A62 File Offset: 0x00000C62
	public <relativePath>j__TPar relativePath
	{
		get
		{
			return this.<relativePath>i__Field;
		}
	}

	// Token: 0x17000017 RID: 23
	// (get) Token: 0x0600002F RID: 47 RVA: 0x00002A6A File Offset: 0x00000C6A
	public <Content>j__TPar Content
	{
		get
		{
			return this.<Content>i__Field;
		}
	}

	// Token: 0x06000030 RID: 48 RVA: 0x00002A72 File Offset: 0x00000C72
	[DebuggerHidden]
	public <>f__AnonymousType6(<fileName>j__TPar fileName, <relativePath>j__TPar relativePath, <Content>j__TPar Content)
	{
		this.<fileName>i__Field = fileName;
		this.<relativePath>i__Field = relativePath;
		this.<Content>i__Field = Content;
	}

	// Token: 0x06000031 RID: 49 RVA: 0x00002A90 File Offset: 0x00000C90
	[DebuggerHidden]
	public override bool Equals(object value)
	{
		var <>f__AnonymousType = value as <>f__AnonymousType6<<fileName>j__TPar, <relativePath>j__TPar, <Content>j__TPar>;
		return this == <>f__AnonymousType || (<>f__AnonymousType != null && EqualityComparer<<fileName>j__TPar>.Default.Equals(this.<fileName>i__Field, <>f__AnonymousType.<fileName>i__Field) && EqualityComparer<<relativePath>j__TPar>.Default.Equals(this.<relativePath>i__Field, <>f__AnonymousType.<relativePath>i__Field) && EqualityComparer<<Content>j__TPar>.Default.Equals(this.<Content>i__Field, <>f__AnonymousType.<Content>i__Field));
	}

	// Token: 0x06000032 RID: 50 RVA: 0x00002AF8 File Offset: 0x00000CF8
	[DebuggerHidden]
	public override int GetHashCode()
	{
		return ((444999861 * -1521134295 + EqualityComparer<<fileName>j__TPar>.Default.GetHashCode(this.<fileName>i__Field)) * -1521134295 + EqualityComparer<<relativePath>j__TPar>.Default.GetHashCode(this.<relativePath>i__Field)) * -1521134295 + EqualityComparer<<Content>j__TPar>.Default.GetHashCode(this.<Content>i__Field);
	}

	// Token: 0x06000033 RID: 51 RVA: 0x00002B50 File Offset: 0x00000D50
	[DebuggerHidden]
	[return: Nullable(1)]
	public override string ToString()
	{
		IFormatProvider formatProvider = null;
		string text = "{{ fileName = {0}, relativePath = {1}, Content = {2} }}";
		object[] array = new object[3];
		int num = 0;
		<fileName>j__TPar <fileName>j__TPar = this.<fileName>i__Field;
		array[num] = ((<fileName>j__TPar != null) ? <fileName>j__TPar.ToString() : null);
		int num2 = 1;
		<relativePath>j__TPar <relativePath>j__TPar = this.<relativePath>i__Field;
		array[num2] = ((<relativePath>j__TPar != null) ? <relativePath>j__TPar.ToString() : null);
		int num3 = 2;
		<Content>j__TPar <Content>j__TPar = this.<Content>i__Field;
		array[num3] = ((<Content>j__TPar != null) ? <Content>j__TPar.ToString() : null);
		return string.Format(formatProvider, text, array);
	}

	// Token: 0x04000015 RID: 21
	[DebuggerBrowsable(0)]
	private readonly <fileName>j__TPar <fileName>i__Field;

	// Token: 0x04000016 RID: 22
	[DebuggerBrowsable(0)]
	private readonly <relativePath>j__TPar <relativePath>i__Field;

	// Token: 0x04000017 RID: 23
	[DebuggerBrowsable(0)]
	private readonly <Content>j__TPar <Content>i__Field;
}
