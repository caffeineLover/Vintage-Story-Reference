using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Resources;
using System.Runtime.CompilerServices;

namespace WinRT
{
	// Token: 0x020001C1 RID: 449
	[GeneratedCode("System.Resources.Tools.StronglyTypedResourceBuilder", "16.0.0.0")]
	[DebuggerNonUserCode]
	[CompilerGenerated]
	internal class WinRTRuntimeErrorStrings
	{
		// Token: 0x06000E2F RID: 3631 RVA: 0x0002B020 File Offset: 0x00029220
		internal WinRTRuntimeErrorStrings()
		{
		}

		// Token: 0x170002E2 RID: 738
		// (get) Token: 0x06000E30 RID: 3632 RVA: 0x0002B028 File Offset: 0x00029228
		[EditorBrowsable(2)]
		internal static ResourceManager ResourceManager
		{
			get
			{
				if (WinRTRuntimeErrorStrings.resourceMan == null)
				{
					WinRTRuntimeErrorStrings.resourceMan = new ResourceManager("WinRT.WinRTRuntimeErrorStrings", typeof(WinRTRuntimeErrorStrings).Assembly);
				}
				return WinRTRuntimeErrorStrings.resourceMan;
			}
		}

		// Token: 0x170002E3 RID: 739
		// (get) Token: 0x06000E31 RID: 3633 RVA: 0x0002B054 File Offset: 0x00029254
		// (set) Token: 0x06000E32 RID: 3634 RVA: 0x0002B05B File Offset: 0x0002925B
		[EditorBrowsable(2)]
		internal static CultureInfo Culture
		{
			get
			{
				return WinRTRuntimeErrorStrings.resourceCulture;
			}
			set
			{
				WinRTRuntimeErrorStrings.resourceCulture = value;
			}
		}

		// Token: 0x170002E4 RID: 740
		// (get) Token: 0x06000E33 RID: 3635 RVA: 0x0002B063 File Offset: 0x00029263
		internal static string Arg_IndexOutOfRangeException
		{
			get
			{
				return WinRTRuntimeErrorStrings.ResourceManager.GetString("Arg_IndexOutOfRangeException", WinRTRuntimeErrorStrings.resourceCulture);
			}
		}

		// Token: 0x170002E5 RID: 741
		// (get) Token: 0x06000E34 RID: 3636 RVA: 0x0002B079 File Offset: 0x00029279
		internal static string Arg_KeyNotFound
		{
			get
			{
				return WinRTRuntimeErrorStrings.ResourceManager.GetString("Arg_KeyNotFound", WinRTRuntimeErrorStrings.resourceCulture);
			}
		}

		// Token: 0x170002E6 RID: 742
		// (get) Token: 0x06000E35 RID: 3637 RVA: 0x0002B08F File Offset: 0x0002928F
		internal static string Arg_KeyNotFoundWithKey
		{
			get
			{
				return WinRTRuntimeErrorStrings.ResourceManager.GetString("Arg_KeyNotFoundWithKey", WinRTRuntimeErrorStrings.resourceCulture);
			}
		}

		// Token: 0x170002E7 RID: 743
		// (get) Token: 0x06000E36 RID: 3638 RVA: 0x0002B0A5 File Offset: 0x000292A5
		internal static string Arg_RankMultiDimNotSupported
		{
			get
			{
				return WinRTRuntimeErrorStrings.ResourceManager.GetString("Arg_RankMultiDimNotSupported", WinRTRuntimeErrorStrings.resourceCulture);
			}
		}

		// Token: 0x170002E8 RID: 744
		// (get) Token: 0x06000E37 RID: 3639 RVA: 0x0002B0BB File Offset: 0x000292BB
		internal static string Argument_AddingDuplicate
		{
			get
			{
				return WinRTRuntimeErrorStrings.ResourceManager.GetString("Argument_AddingDuplicate", WinRTRuntimeErrorStrings.resourceCulture);
			}
		}

		// Token: 0x170002E9 RID: 745
		// (get) Token: 0x06000E38 RID: 3640 RVA: 0x0002B0D1 File Offset: 0x000292D1
		internal static string Argument_AddingDuplicateWithKey
		{
			get
			{
				return WinRTRuntimeErrorStrings.ResourceManager.GetString("Argument_AddingDuplicateWithKey", WinRTRuntimeErrorStrings.resourceCulture);
			}
		}

		// Token: 0x170002EA RID: 746
		// (get) Token: 0x06000E39 RID: 3641 RVA: 0x0002B0E7 File Offset: 0x000292E7
		internal static string Argument_IndexOutOfArrayBounds
		{
			get
			{
				return WinRTRuntimeErrorStrings.ResourceManager.GetString("Argument_IndexOutOfArrayBounds", WinRTRuntimeErrorStrings.resourceCulture);
			}
		}

		// Token: 0x170002EB RID: 747
		// (get) Token: 0x06000E3A RID: 3642 RVA: 0x0002B0FD File Offset: 0x000292FD
		internal static string Argument_InsufficientSpaceToCopyCollection
		{
			get
			{
				return WinRTRuntimeErrorStrings.ResourceManager.GetString("Argument_InsufficientSpaceToCopyCollection", WinRTRuntimeErrorStrings.resourceCulture);
			}
		}

		// Token: 0x170002EC RID: 748
		// (get) Token: 0x06000E3B RID: 3643 RVA: 0x0002B113 File Offset: 0x00029313
		internal static string ArgumentOutOfRange_Index
		{
			get
			{
				return WinRTRuntimeErrorStrings.ResourceManager.GetString("ArgumentOutOfRange_Index", WinRTRuntimeErrorStrings.resourceCulture);
			}
		}

		// Token: 0x170002ED RID: 749
		// (get) Token: 0x06000E3C RID: 3644 RVA: 0x0002B129 File Offset: 0x00029329
		internal static string ArgumentOutOfRange_IndexLargerThanMaxValue
		{
			get
			{
				return WinRTRuntimeErrorStrings.ResourceManager.GetString("ArgumentOutOfRange_IndexLargerThanMaxValue", WinRTRuntimeErrorStrings.resourceCulture);
			}
		}

		// Token: 0x170002EE RID: 750
		// (get) Token: 0x06000E3D RID: 3645 RVA: 0x0002B13F File Offset: 0x0002933F
		internal static string InvalidOperation_CannotRemoveLastFromEmptyCollection
		{
			get
			{
				return WinRTRuntimeErrorStrings.ResourceManager.GetString("InvalidOperation_CannotRemoveLastFromEmptyCollection", WinRTRuntimeErrorStrings.resourceCulture);
			}
		}

		// Token: 0x170002EF RID: 751
		// (get) Token: 0x06000E3E RID: 3646 RVA: 0x0002B155 File Offset: 0x00029355
		internal static string InvalidOperation_CollectionBackingDictionaryTooLarge
		{
			get
			{
				return WinRTRuntimeErrorStrings.ResourceManager.GetString("InvalidOperation_CollectionBackingDictionaryTooLarge", WinRTRuntimeErrorStrings.resourceCulture);
			}
		}

		// Token: 0x170002F0 RID: 752
		// (get) Token: 0x06000E3F RID: 3647 RVA: 0x0002B16B File Offset: 0x0002936B
		internal static string InvalidOperation_CollectionBackingListTooLarge
		{
			get
			{
				return WinRTRuntimeErrorStrings.ResourceManager.GetString("InvalidOperation_CollectionBackingListTooLarge", WinRTRuntimeErrorStrings.resourceCulture);
			}
		}

		// Token: 0x170002F1 RID: 753
		// (get) Token: 0x06000E40 RID: 3648 RVA: 0x0002B181 File Offset: 0x00029381
		internal static string InvalidOperation_EnumEnded
		{
			get
			{
				return WinRTRuntimeErrorStrings.ResourceManager.GetString("InvalidOperation_EnumEnded", WinRTRuntimeErrorStrings.resourceCulture);
			}
		}

		// Token: 0x170002F2 RID: 754
		// (get) Token: 0x06000E41 RID: 3649 RVA: 0x0002B197 File Offset: 0x00029397
		internal static string InvalidOperation_EnumFailedVersion
		{
			get
			{
				return WinRTRuntimeErrorStrings.ResourceManager.GetString("InvalidOperation_EnumFailedVersion", WinRTRuntimeErrorStrings.resourceCulture);
			}
		}

		// Token: 0x170002F3 RID: 755
		// (get) Token: 0x06000E42 RID: 3650 RVA: 0x0002B1AD File Offset: 0x000293AD
		internal static string InvalidOperation_EnumNotStarted
		{
			get
			{
				return WinRTRuntimeErrorStrings.ResourceManager.GetString("InvalidOperation_EnumNotStarted", WinRTRuntimeErrorStrings.resourceCulture);
			}
		}

		// Token: 0x170002F4 RID: 756
		// (get) Token: 0x06000E43 RID: 3651 RVA: 0x0002B1C3 File Offset: 0x000293C3
		internal static string NotSupported_KeyCollectionSet
		{
			get
			{
				return WinRTRuntimeErrorStrings.ResourceManager.GetString("NotSupported_KeyCollectionSet", WinRTRuntimeErrorStrings.resourceCulture);
			}
		}

		// Token: 0x170002F5 RID: 757
		// (get) Token: 0x06000E44 RID: 3652 RVA: 0x0002B1D9 File Offset: 0x000293D9
		internal static string NotSupported_ValueCollectionSet
		{
			get
			{
				return WinRTRuntimeErrorStrings.ResourceManager.GetString("NotSupported_ValueCollectionSet", WinRTRuntimeErrorStrings.resourceCulture);
			}
		}

		// Token: 0x040002C6 RID: 710
		private static ResourceManager resourceMan;

		// Token: 0x040002C7 RID: 711
		private static CultureInfo resourceCulture;
	}
}
