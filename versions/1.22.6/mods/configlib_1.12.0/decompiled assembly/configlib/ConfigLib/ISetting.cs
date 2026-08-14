using System;
using System.Runtime.CompilerServices;
using ConfigLib.Formatting;
using Vintagestory.API.Datastructures;

namespace ConfigLib
{
	// Token: 0x0200000C RID: 12
	[NullableContext(1)]
	public interface ISetting : IConfigBlock
	{
		// Token: 0x17000004 RID: 4
		// (get) Token: 0x0600002B RID: 43
		// (set) Token: 0x0600002C RID: 44
		JsonObject Value { get; set; }

		// Token: 0x17000005 RID: 5
		// (get) Token: 0x0600002D RID: 45
		// (set) Token: 0x0600002E RID: 46
		[Nullable(2)]
		string MappingKey { [NullableContext(2)] get; [NullableContext(2)] set; }

		// Token: 0x17000006 RID: 6
		// (get) Token: 0x0600002F RID: 47
		JsonObject DefaultValue { get; }

		// Token: 0x17000007 RID: 7
		// (get) Token: 0x06000030 RID: 48
		ConfigSettingType SettingType { get; }

		// Token: 0x17000008 RID: 8
		// (get) Token: 0x06000031 RID: 49
		string YamlCode { get; }

		// Token: 0x17000009 RID: 9
		// (get) Token: 0x06000032 RID: 50
		[Nullable(2)]
		Validation Validation { [NullableContext(2)] get; }

		// Token: 0x06000033 RID: 51
		bool AssignSettingValue(object target);
	}
}
