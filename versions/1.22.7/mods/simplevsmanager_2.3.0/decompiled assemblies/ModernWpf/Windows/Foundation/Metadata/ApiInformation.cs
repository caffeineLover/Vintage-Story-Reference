using System;
using System.Runtime.Versioning;
using System.Threading;
using ABI.Windows.Foundation.Metadata;
using WinRT;

namespace Windows.Foundation.Metadata
{
	// Token: 0x0200005C RID: 92
	[WindowsRuntimeType("Windows.Foundation.FoundationContract")]
	[SupportedOSPlatform("Windows10.0.10240.0")]
	[ContractVersion(typeof(FoundationContract), 65536U)]
	internal static class ApiInformation
	{
		// Token: 0x06000412 RID: 1042 RVA: 0x0000651B File Offset: 0x0000471B
		public static I As<I>()
		{
			return ApiInformation._factory.AsInterface<I>();
		}

		// Token: 0x06000413 RID: 1043 RVA: 0x00006527 File Offset: 0x00004727
		private static IObjectReference Make___objRef_global__Windows_Foundation_Metadata_IApiInformationStatics()
		{
			Interlocked.CompareExchange<IObjectReference>(ref ApiInformation.___objRef_global__Windows_Foundation_Metadata_IApiInformationStatics, ApiInformation._factory._As(GuidGenerator.GetIID(typeof(IApiInformationStatics).GetHelperType())), null);
			return ApiInformation.___objRef_global__Windows_Foundation_Metadata_IApiInformationStatics;
		}

		// Token: 0x170001E8 RID: 488
		// (get) Token: 0x06000414 RID: 1044 RVA: 0x0000655A File Offset: 0x0000475A
		private static IObjectReference _objRef_global__Windows_Foundation_Metadata_IApiInformationStatics
		{
			get
			{
				return ApiInformation.___objRef_global__Windows_Foundation_Metadata_IApiInformationStatics ?? ApiInformation.Make___objRef_global__Windows_Foundation_Metadata_IApiInformationStatics();
			}
		}

		// Token: 0x06000415 RID: 1045 RVA: 0x0000656C File Offset: 0x0000476C
		public static bool IsTypePresent(string typeName)
		{
			return IApiInformationStaticsMethods.IsTypePresent(ApiInformation._objRef_global__Windows_Foundation_Metadata_IApiInformationStatics, typeName);
		}

		// Token: 0x06000416 RID: 1046 RVA: 0x00006579 File Offset: 0x00004779
		public static bool IsMethodPresent(string typeName, string methodName)
		{
			return IApiInformationStaticsMethods.IsMethodPresent(ApiInformation._objRef_global__Windows_Foundation_Metadata_IApiInformationStatics, typeName, methodName);
		}

		// Token: 0x06000417 RID: 1047 RVA: 0x00006587 File Offset: 0x00004787
		public static bool IsMethodPresent(string typeName, string methodName, uint inputParameterCount)
		{
			return IApiInformationStaticsMethods.IsMethodPresent(ApiInformation._objRef_global__Windows_Foundation_Metadata_IApiInformationStatics, typeName, methodName, inputParameterCount);
		}

		// Token: 0x06000418 RID: 1048 RVA: 0x00006596 File Offset: 0x00004796
		public static bool IsEventPresent(string typeName, string eventName)
		{
			return IApiInformationStaticsMethods.IsEventPresent(ApiInformation._objRef_global__Windows_Foundation_Metadata_IApiInformationStatics, typeName, eventName);
		}

		// Token: 0x06000419 RID: 1049 RVA: 0x000065A4 File Offset: 0x000047A4
		public static bool IsPropertyPresent(string typeName, string propertyName)
		{
			return IApiInformationStaticsMethods.IsPropertyPresent(ApiInformation._objRef_global__Windows_Foundation_Metadata_IApiInformationStatics, typeName, propertyName);
		}

		// Token: 0x0600041A RID: 1050 RVA: 0x000065B2 File Offset: 0x000047B2
		public static bool IsReadOnlyPropertyPresent(string typeName, string propertyName)
		{
			return IApiInformationStaticsMethods.IsReadOnlyPropertyPresent(ApiInformation._objRef_global__Windows_Foundation_Metadata_IApiInformationStatics, typeName, propertyName);
		}

		// Token: 0x0600041B RID: 1051 RVA: 0x000065C0 File Offset: 0x000047C0
		public static bool IsWriteablePropertyPresent(string typeName, string propertyName)
		{
			return IApiInformationStaticsMethods.IsWriteablePropertyPresent(ApiInformation._objRef_global__Windows_Foundation_Metadata_IApiInformationStatics, typeName, propertyName);
		}

		// Token: 0x0600041C RID: 1052 RVA: 0x000065CE File Offset: 0x000047CE
		public static bool IsEnumNamedValuePresent(string enumTypeName, string valueName)
		{
			return IApiInformationStaticsMethods.IsEnumNamedValuePresent(ApiInformation._objRef_global__Windows_Foundation_Metadata_IApiInformationStatics, enumTypeName, valueName);
		}

		// Token: 0x0600041D RID: 1053 RVA: 0x000065DC File Offset: 0x000047DC
		public static bool IsApiContractPresent(string contractName, ushort majorVersion)
		{
			return IApiInformationStaticsMethods.IsApiContractPresent(ApiInformation._objRef_global__Windows_Foundation_Metadata_IApiInformationStatics, contractName, majorVersion);
		}

		// Token: 0x0600041E RID: 1054 RVA: 0x000065EA File Offset: 0x000047EA
		public static bool IsApiContractPresent(string contractName, ushort majorVersion, ushort minorVersion)
		{
			return IApiInformationStaticsMethods.IsApiContractPresent(ApiInformation._objRef_global__Windows_Foundation_Metadata_IApiInformationStatics, contractName, majorVersion, minorVersion);
		}

		// Token: 0x040000EA RID: 234
		internal static BaseActivationFactory _factory = new BaseActivationFactory("Windows.Foundation.Metadata", "Windows.Foundation.Metadata.ApiInformation");

		// Token: 0x040000EB RID: 235
		private static volatile IObjectReference ___objRef_global__Windows_Foundation_Metadata_IApiInformationStatics;
	}
}
