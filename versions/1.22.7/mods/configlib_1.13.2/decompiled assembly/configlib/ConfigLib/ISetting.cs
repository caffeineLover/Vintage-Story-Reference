using System;
using System.Runtime.CompilerServices;
using ConfigLib.Formatting;
using Vintagestory.API.Datastructures;

namespace ConfigLib
{
	// Token: 0x0200000E RID: 14
	[NullableContext(1)]
	public interface ISetting : IConfigBlock
	{
		// Token: 0x17000011 RID: 17
		// (get) Token: 0x06000050 RID: 80
		// (set) Token: 0x06000051 RID: 81
		JsonObject Value { get; set; }

		// Token: 0x17000012 RID: 18
		// (get) Token: 0x06000052 RID: 82
		// (set) Token: 0x06000053 RID: 83
		[Nullable(2)]
		string MappingKey { [NullableContext(2)] get; [NullableContext(2)] set; }

		// Token: 0x17000013 RID: 19
		// (get) Token: 0x06000054 RID: 84
		JsonObject DefaultValue { get; }

		// Token: 0x17000014 RID: 20
		// (get) Token: 0x06000055 RID: 85
		ConfigSettingType SettingType { get; }

		// Token: 0x17000015 RID: 21
		// (get) Token: 0x06000056 RID: 86
		string YamlCode { get; }

		// Token: 0x17000016 RID: 22
		// (get) Token: 0x06000057 RID: 87
		[Nullable(2)]
		Validation Validation { [NullableContext(2)] get; }

		// Token: 0x06000058 RID: 88
		bool AssignSettingValue(object target);
	}
}
