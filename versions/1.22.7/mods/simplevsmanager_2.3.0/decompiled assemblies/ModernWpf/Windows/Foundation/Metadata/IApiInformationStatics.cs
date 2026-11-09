using System;
using System.Runtime.InteropServices;
using WinRT;

namespace Windows.Foundation.Metadata
{
	// Token: 0x0200006E RID: 110
	[WindowsRuntimeType("Windows.Foundation.FoundationContract")]
	[Guid("997439FE-F681-4A11-B416-C13A47E8BA36")]
	[ContractVersion(typeof(FoundationContract), 65536U)]
	internal interface IApiInformationStatics
	{
		// Token: 0x06000433 RID: 1075
		bool IsTypePresent(string typeName);

		// Token: 0x06000434 RID: 1076
		[Overload("IsMethodPresent")]
		bool IsMethodPresent(string typeName, string methodName);

		// Token: 0x06000435 RID: 1077
		[Overload("IsMethodPresentWithArity")]
		bool IsMethodPresent(string typeName, string methodName, uint inputParameterCount);

		// Token: 0x06000436 RID: 1078
		bool IsEventPresent(string typeName, string eventName);

		// Token: 0x06000437 RID: 1079
		bool IsPropertyPresent(string typeName, string propertyName);

		// Token: 0x06000438 RID: 1080
		bool IsReadOnlyPropertyPresent(string typeName, string propertyName);

		// Token: 0x06000439 RID: 1081
		bool IsWriteablePropertyPresent(string typeName, string propertyName);

		// Token: 0x0600043A RID: 1082
		bool IsEnumNamedValuePresent(string enumTypeName, string valueName);

		// Token: 0x0600043B RID: 1083
		[Overload("IsApiContractPresentByMajor")]
		bool IsApiContractPresent(string contractName, ushort majorVersion);

		// Token: 0x0600043C RID: 1084
		[Overload("IsApiContractPresentByMajorAndMinor")]
		bool IsApiContractPresent(string contractName, ushort majorVersion, ushort minorVersion);
	}
}
