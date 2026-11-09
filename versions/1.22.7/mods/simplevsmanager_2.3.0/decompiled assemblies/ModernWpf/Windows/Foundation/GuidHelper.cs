using System;
using System.Runtime.Versioning;
using System.Threading;
using ABI.Windows.Foundation;
using Windows.Foundation.Metadata;
using WinRT;

namespace Windows.Foundation
{
	// Token: 0x0200003A RID: 58
	[WindowsRuntimeType("Windows.Foundation.UniversalApiContract")]
	[SupportedOSPlatform("Windows10.0.17763.0")]
	[ContractVersion(typeof(UniversalApiContract), 458752U)]
	internal static class GuidHelper
	{
		// Token: 0x0600031E RID: 798 RVA: 0x000058A6 File Offset: 0x00003AA6
		public static I As<I>()
		{
			return GuidHelper._factory.AsInterface<I>();
		}

		// Token: 0x0600031F RID: 799 RVA: 0x000058B2 File Offset: 0x00003AB2
		private static IObjectReference Make___objRef_global__Windows_Foundation_IGuidHelperStatics()
		{
			Interlocked.CompareExchange<IObjectReference>(ref GuidHelper.___objRef_global__Windows_Foundation_IGuidHelperStatics, GuidHelper._factory._As(GuidGenerator.GetIID(typeof(IGuidHelperStatics).GetHelperType())), null);
			return GuidHelper.___objRef_global__Windows_Foundation_IGuidHelperStatics;
		}

		// Token: 0x170001AA RID: 426
		// (get) Token: 0x06000320 RID: 800 RVA: 0x000058E5 File Offset: 0x00003AE5
		private static IObjectReference _objRef_global__Windows_Foundation_IGuidHelperStatics
		{
			get
			{
				return GuidHelper.___objRef_global__Windows_Foundation_IGuidHelperStatics ?? GuidHelper.Make___objRef_global__Windows_Foundation_IGuidHelperStatics();
			}
		}

		// Token: 0x06000321 RID: 801 RVA: 0x000058F7 File Offset: 0x00003AF7
		public static Guid CreateNewGuid()
		{
			return IGuidHelperStaticsMethods.CreateNewGuid(GuidHelper._objRef_global__Windows_Foundation_IGuidHelperStatics);
		}

		// Token: 0x06000322 RID: 802 RVA: 0x00005903 File Offset: 0x00003B03
		public static bool Equals(in Guid target, in Guid value)
		{
			return IGuidHelperStaticsMethods.Equals(GuidHelper._objRef_global__Windows_Foundation_IGuidHelperStatics, target, value);
		}

		// Token: 0x170001AB RID: 427
		// (get) Token: 0x06000323 RID: 803 RVA: 0x00005911 File Offset: 0x00003B11
		public static Guid Empty
		{
			get
			{
				return IGuidHelperStaticsMethods.get_Empty(GuidHelper._objRef_global__Windows_Foundation_IGuidHelperStatics);
			}
		}

		// Token: 0x040000BE RID: 190
		internal static BaseActivationFactory _factory = new BaseActivationFactory("Windows.Foundation", "Windows.Foundation.GuidHelper");

		// Token: 0x040000BF RID: 191
		private static volatile IObjectReference ___objRef_global__Windows_Foundation_IGuidHelperStatics;
	}
}
