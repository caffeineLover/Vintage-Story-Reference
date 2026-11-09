using System;
using System.Runtime.Versioning;
using WinRT;

namespace Windows.Foundation.Metadata
{
	// Token: 0x02000068 RID: 104
	[WindowsRuntimeType("Windows.Foundation.FoundationContract")]
	[AttributeUsage(32767, AllowMultiple = false)]
	[SupportedOSPlatform("Windows10.0.15063.0")]
	[ContractVersion(typeof(FoundationContract), 196608U)]
	internal sealed class FeatureAttribute : Attribute
	{
		// Token: 0x0600042F RID: 1071 RVA: 0x00006687 File Offset: 0x00004887
		public FeatureAttribute(FeatureStage featureStage, bool validInAllBranches)
		{
		}
	}
}
