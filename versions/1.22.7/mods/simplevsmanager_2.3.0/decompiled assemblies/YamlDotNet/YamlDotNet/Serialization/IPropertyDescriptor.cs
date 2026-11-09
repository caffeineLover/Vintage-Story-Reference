using System;
using System.Runtime.CompilerServices;
using YamlDotNet.Core;

namespace YamlDotNet.Serialization
{
	// Token: 0x02000022 RID: 34
	[NullableContext(1)]
	public interface IPropertyDescriptor
	{
		// Token: 0x17000018 RID: 24
		// (get) Token: 0x060000CD RID: 205
		string Name { get; }

		// Token: 0x17000019 RID: 25
		// (get) Token: 0x060000CE RID: 206
		bool AllowNulls { get; }

		// Token: 0x1700001A RID: 26
		// (get) Token: 0x060000CF RID: 207
		bool CanWrite { get; }

		// Token: 0x1700001B RID: 27
		// (get) Token: 0x060000D0 RID: 208
		Type Type { get; }

		// Token: 0x1700001C RID: 28
		// (get) Token: 0x060000D1 RID: 209
		// (set) Token: 0x060000D2 RID: 210
		[Nullable(2)]
		Type TypeOverride { [NullableContext(2)] get; [NullableContext(2)] set; }

		// Token: 0x1700001D RID: 29
		// (get) Token: 0x060000D3 RID: 211
		// (set) Token: 0x060000D4 RID: 212
		int Order { get; set; }

		// Token: 0x1700001E RID: 30
		// (get) Token: 0x060000D5 RID: 213
		// (set) Token: 0x060000D6 RID: 214
		ScalarStyle ScalarStyle { get; set; }

		// Token: 0x1700001F RID: 31
		// (get) Token: 0x060000D7 RID: 215
		bool Required { get; }

		// Token: 0x17000020 RID: 32
		// (get) Token: 0x060000D8 RID: 216
		[Nullable(2)]
		Type ConverterType { [NullableContext(2)] get; }

		// Token: 0x060000D9 RID: 217
		[NullableContext(0)]
		[return: Nullable(2)]
		T GetCustomAttribute<T>() where T : Attribute;

		// Token: 0x060000DA RID: 218
		IObjectDescriptor Read(object target);

		// Token: 0x060000DB RID: 219
		void Write(object target, [Nullable(2)] object value);
	}
}
