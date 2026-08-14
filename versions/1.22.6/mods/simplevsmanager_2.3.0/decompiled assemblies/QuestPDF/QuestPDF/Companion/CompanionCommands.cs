using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using QuestPDF.Drawing;
using QuestPDF.Infrastructure;

namespace QuestPDF.Companion
{
	// Token: 0x0200012E RID: 302
	[NullableContext(1)]
	[Nullable(0)]
	internal static class CompanionCommands
	{
		// Token: 0x02000207 RID: 519
		[Nullable(0)]
		internal sealed class Notify
		{
			// Token: 0x170002FB RID: 763
			// (get) Token: 0x06000DFF RID: 3583 RVA: 0x00021F76 File Offset: 0x00020176
			public string ClientId
			{
				get
				{
					return CompanionCommands.Notify.CurrentClientId;
				}
			}

			// Token: 0x170002FC RID: 764
			// (get) Token: 0x06000E00 RID: 3584 RVA: 0x00021F80 File Offset: 0x00020180
			public LicenseType License
			{
				get
				{
					return Settings.License.GetValueOrDefault();
				}
			}

			// Token: 0x04000797 RID: 1943
			private static readonly string CurrentClientId = Guid.NewGuid().ToString();
		}

		// Token: 0x02000208 RID: 520
		[Nullable(0)]
		internal sealed class UpdateDocumentStructure
		{
			// Token: 0x170002FD RID: 765
			// (get) Token: 0x06000E03 RID: 3587 RVA: 0x00021FC9 File Offset: 0x000201C9
			// (set) Token: 0x06000E04 RID: 3588 RVA: 0x00021FD1 File Offset: 0x000201D1
			public bool IsDocumentHotReloaded { get; set; }

			// Token: 0x170002FE RID: 766
			// (get) Token: 0x06000E05 RID: 3589 RVA: 0x00021FDA File Offset: 0x000201DA
			// (set) Token: 0x06000E06 RID: 3590 RVA: 0x00021FE2 File Offset: 0x000201E2
			public ICollection<CompanionCommands.UpdateDocumentStructure.PageSize> Pages { get; set; }

			// Token: 0x170002FF RID: 767
			// (get) Token: 0x06000E07 RID: 3591 RVA: 0x00021FEB File Offset: 0x000201EB
			// (set) Token: 0x06000E08 RID: 3592 RVA: 0x00021FF3 File Offset: 0x000201F3
			public CompanionCommands.UpdateDocumentStructure.DocumentHierarchyElement Hierarchy { get; set; }

			// Token: 0x0200021F RID: 543
			[NullableContext(0)]
			public sealed class PageSize
			{
				// Token: 0x17000309 RID: 777
				// (get) Token: 0x06000E48 RID: 3656 RVA: 0x00022FF7 File Offset: 0x000211F7
				// (set) Token: 0x06000E49 RID: 3657 RVA: 0x00022FFF File Offset: 0x000211FF
				public float Width { get; set; }

				// Token: 0x1700030A RID: 778
				// (get) Token: 0x06000E4A RID: 3658 RVA: 0x00023008 File Offset: 0x00021208
				// (set) Token: 0x06000E4B RID: 3659 RVA: 0x00023010 File Offset: 0x00021210
				public float Height { get; set; }
			}

			// Token: 0x02000220 RID: 544
			[Nullable(0)]
			internal sealed class DocumentHierarchyElement : IEquatable<CompanionCommands.UpdateDocumentStructure.DocumentHierarchyElement>
			{
				// Token: 0x1700030B RID: 779
				// (get) Token: 0x06000E4D RID: 3661 RVA: 0x00023021 File Offset: 0x00021221
				[CompilerGenerated]
				private Type EqualityContract
				{
					[CompilerGenerated]
					get
					{
						return typeof(CompanionCommands.UpdateDocumentStructure.DocumentHierarchyElement);
					}
				}

				// Token: 0x1700030C RID: 780
				// (get) Token: 0x06000E4E RID: 3662 RVA: 0x0002302D File Offset: 0x0002122D
				// (set) Token: 0x06000E4F RID: 3663 RVA: 0x00023035 File Offset: 0x00021235
				internal Element Element { get; set; }

				// Token: 0x1700030D RID: 781
				// (get) Token: 0x06000E50 RID: 3664 RVA: 0x0002303E File Offset: 0x0002123E
				// (set) Token: 0x06000E51 RID: 3665 RVA: 0x00023046 File Offset: 0x00021246
				public string ElementType { get; set; }

				// Token: 0x1700030E RID: 782
				// (get) Token: 0x06000E52 RID: 3666 RVA: 0x0002304F File Offset: 0x0002124F
				// (set) Token: 0x06000E53 RID: 3667 RVA: 0x00023057 File Offset: 0x00021257
				[Nullable(2)]
				public string Hint { [NullableContext(2)] get; [NullableContext(2)] set; }

				// Token: 0x1700030F RID: 783
				// (get) Token: 0x06000E54 RID: 3668 RVA: 0x00023060 File Offset: 0x00021260
				// (set) Token: 0x06000E55 RID: 3669 RVA: 0x00023068 File Offset: 0x00021268
				[Nullable(2)]
				public string SearchableContent { [NullableContext(2)] get; [NullableContext(2)] set; }

				// Token: 0x17000310 RID: 784
				// (get) Token: 0x06000E56 RID: 3670 RVA: 0x00023071 File Offset: 0x00021271
				// (set) Token: 0x06000E57 RID: 3671 RVA: 0x00023079 File Offset: 0x00021279
				public bool IsSingleChildContainer { get; set; }

				// Token: 0x17000311 RID: 785
				// (get) Token: 0x06000E58 RID: 3672 RVA: 0x00023082 File Offset: 0x00021282
				// (set) Token: 0x06000E59 RID: 3673 RVA: 0x0002308A File Offset: 0x0002128A
				public ICollection<CompanionCommands.UpdateDocumentStructure.PageLocation> PageLocations { get; set; }

				// Token: 0x17000312 RID: 786
				// (get) Token: 0x06000E5A RID: 3674 RVA: 0x00023093 File Offset: 0x00021293
				// (set) Token: 0x06000E5B RID: 3675 RVA: 0x0002309B File Offset: 0x0002129B
				public ICollection<CompanionCommands.UpdateDocumentStructure.LayoutErrorMeasurement> LayoutErrorMeasurements { get; set; }

				// Token: 0x17000313 RID: 787
				// (get) Token: 0x06000E5C RID: 3676 RVA: 0x000230A4 File Offset: 0x000212A4
				// (set) Token: 0x06000E5D RID: 3677 RVA: 0x000230AC File Offset: 0x000212AC
				public ICollection<CompanionCommands.ElementProperty> Properties { get; set; }

				// Token: 0x17000314 RID: 788
				// (get) Token: 0x06000E5E RID: 3678 RVA: 0x000230B5 File Offset: 0x000212B5
				// (set) Token: 0x06000E5F RID: 3679 RVA: 0x000230BD File Offset: 0x000212BD
				[Nullable(2)]
				public CompanionCommands.UpdateDocumentStructure.SourceCodePath SourceCodeDeclarationPath { [NullableContext(2)] get; [NullableContext(2)] set; }

				// Token: 0x17000315 RID: 789
				// (get) Token: 0x06000E60 RID: 3680 RVA: 0x000230C6 File Offset: 0x000212C6
				// (set) Token: 0x06000E61 RID: 3681 RVA: 0x000230CE File Offset: 0x000212CE
				public ICollection<CompanionCommands.UpdateDocumentStructure.DocumentHierarchyElement> Children { get; set; }

				// Token: 0x06000E62 RID: 3682 RVA: 0x000230D8 File Offset: 0x000212D8
				[CompilerGenerated]
				public override string ToString()
				{
					StringBuilder stringBuilder = new StringBuilder();
					stringBuilder.Append("DocumentHierarchyElement");
					stringBuilder.Append(" { ");
					if (this.PrintMembers(stringBuilder))
					{
						stringBuilder.Append(' ');
					}
					stringBuilder.Append('}');
					return stringBuilder.ToString();
				}

				// Token: 0x06000E63 RID: 3683 RVA: 0x00023124 File Offset: 0x00021324
				[CompilerGenerated]
				private bool PrintMembers(StringBuilder builder)
				{
					RuntimeHelpers.EnsureSufficientExecutionStack();
					builder.Append("ElementType = ");
					builder.Append(this.ElementType);
					builder.Append(", Hint = ");
					builder.Append(this.Hint);
					builder.Append(", SearchableContent = ");
					builder.Append(this.SearchableContent);
					builder.Append(", IsSingleChildContainer = ");
					builder.Append(this.IsSingleChildContainer.ToString());
					builder.Append(", PageLocations = ");
					builder.Append(this.PageLocations);
					builder.Append(", LayoutErrorMeasurements = ");
					builder.Append(this.LayoutErrorMeasurements);
					builder.Append(", Properties = ");
					builder.Append(this.Properties);
					builder.Append(", SourceCodeDeclarationPath = ");
					builder.Append(this.SourceCodeDeclarationPath);
					builder.Append(", Children = ");
					builder.Append(this.Children);
					return true;
				}

				// Token: 0x06000E64 RID: 3684 RVA: 0x00023226 File Offset: 0x00021426
				[NullableContext(2)]
				[CompilerGenerated]
				public static bool operator !=(CompanionCommands.UpdateDocumentStructure.DocumentHierarchyElement left, CompanionCommands.UpdateDocumentStructure.DocumentHierarchyElement right)
				{
					return !(left == right);
				}

				// Token: 0x06000E65 RID: 3685 RVA: 0x00023232 File Offset: 0x00021432
				[NullableContext(2)]
				[CompilerGenerated]
				public static bool operator ==(CompanionCommands.UpdateDocumentStructure.DocumentHierarchyElement left, CompanionCommands.UpdateDocumentStructure.DocumentHierarchyElement right)
				{
					return left == right || (left != null && left.Equals(right));
				}

				// Token: 0x06000E66 RID: 3686 RVA: 0x00023248 File Offset: 0x00021448
				[CompilerGenerated]
				public override int GetHashCode()
				{
					return (((((((((EqualityComparer<Type>.Default.GetHashCode(this.EqualityContract) * -1521134295 + EqualityComparer<Element>.Default.GetHashCode(this.<Element>k__BackingField)) * -1521134295 + EqualityComparer<string>.Default.GetHashCode(this.<ElementType>k__BackingField)) * -1521134295 + EqualityComparer<string>.Default.GetHashCode(this.<Hint>k__BackingField)) * -1521134295 + EqualityComparer<string>.Default.GetHashCode(this.<SearchableContent>k__BackingField)) * -1521134295 + EqualityComparer<bool>.Default.GetHashCode(this.<IsSingleChildContainer>k__BackingField)) * -1521134295 + EqualityComparer<ICollection<CompanionCommands.UpdateDocumentStructure.PageLocation>>.Default.GetHashCode(this.<PageLocations>k__BackingField)) * -1521134295 + EqualityComparer<ICollection<CompanionCommands.UpdateDocumentStructure.LayoutErrorMeasurement>>.Default.GetHashCode(this.<LayoutErrorMeasurements>k__BackingField)) * -1521134295 + EqualityComparer<ICollection<CompanionCommands.ElementProperty>>.Default.GetHashCode(this.<Properties>k__BackingField)) * -1521134295 + EqualityComparer<CompanionCommands.UpdateDocumentStructure.SourceCodePath>.Default.GetHashCode(this.<SourceCodeDeclarationPath>k__BackingField)) * -1521134295 + EqualityComparer<ICollection<CompanionCommands.UpdateDocumentStructure.DocumentHierarchyElement>>.Default.GetHashCode(this.<Children>k__BackingField);
				}

				// Token: 0x06000E67 RID: 3687 RVA: 0x0002334B File Offset: 0x0002154B
				[NullableContext(2)]
				[CompilerGenerated]
				public override bool Equals(object obj)
				{
					return this.Equals(obj as CompanionCommands.UpdateDocumentStructure.DocumentHierarchyElement);
				}

				// Token: 0x06000E68 RID: 3688 RVA: 0x0002335C File Offset: 0x0002155C
				[NullableContext(2)]
				[CompilerGenerated]
				public bool Equals(CompanionCommands.UpdateDocumentStructure.DocumentHierarchyElement other)
				{
					return this == other || (other != null && this.EqualityContract == other.EqualityContract && EqualityComparer<Element>.Default.Equals(this.<Element>k__BackingField, other.<Element>k__BackingField) && EqualityComparer<string>.Default.Equals(this.<ElementType>k__BackingField, other.<ElementType>k__BackingField) && EqualityComparer<string>.Default.Equals(this.<Hint>k__BackingField, other.<Hint>k__BackingField) && EqualityComparer<string>.Default.Equals(this.<SearchableContent>k__BackingField, other.<SearchableContent>k__BackingField) && EqualityComparer<bool>.Default.Equals(this.<IsSingleChildContainer>k__BackingField, other.<IsSingleChildContainer>k__BackingField) && EqualityComparer<ICollection<CompanionCommands.UpdateDocumentStructure.PageLocation>>.Default.Equals(this.<PageLocations>k__BackingField, other.<PageLocations>k__BackingField) && EqualityComparer<ICollection<CompanionCommands.UpdateDocumentStructure.LayoutErrorMeasurement>>.Default.Equals(this.<LayoutErrorMeasurements>k__BackingField, other.<LayoutErrorMeasurements>k__BackingField) && EqualityComparer<ICollection<CompanionCommands.ElementProperty>>.Default.Equals(this.<Properties>k__BackingField, other.<Properties>k__BackingField) && EqualityComparer<CompanionCommands.UpdateDocumentStructure.SourceCodePath>.Default.Equals(this.<SourceCodeDeclarationPath>k__BackingField, other.<SourceCodeDeclarationPath>k__BackingField) && EqualityComparer<ICollection<CompanionCommands.UpdateDocumentStructure.DocumentHierarchyElement>>.Default.Equals(this.<Children>k__BackingField, other.<Children>k__BackingField));
				}

				// Token: 0x06000E6A RID: 3690 RVA: 0x00023494 File Offset: 0x00021694
				[CompilerGenerated]
				private DocumentHierarchyElement(CompanionCommands.UpdateDocumentStructure.DocumentHierarchyElement original)
				{
					this.Element = original.<Element>k__BackingField;
					this.ElementType = original.<ElementType>k__BackingField;
					this.Hint = original.<Hint>k__BackingField;
					this.SearchableContent = original.<SearchableContent>k__BackingField;
					this.IsSingleChildContainer = original.<IsSingleChildContainer>k__BackingField;
					this.PageLocations = original.<PageLocations>k__BackingField;
					this.LayoutErrorMeasurements = original.<LayoutErrorMeasurements>k__BackingField;
					this.Properties = original.<Properties>k__BackingField;
					this.SourceCodeDeclarationPath = original.<SourceCodeDeclarationPath>k__BackingField;
					this.Children = original.<Children>k__BackingField;
				}

				// Token: 0x06000E6B RID: 3691 RVA: 0x0002351F File Offset: 0x0002171F
				public DocumentHierarchyElement()
				{
				}
			}

			// Token: 0x02000221 RID: 545
			[NullableContext(0)]
			internal sealed class PageLocation
			{
				// Token: 0x17000316 RID: 790
				// (get) Token: 0x06000E6C RID: 3692 RVA: 0x00023527 File Offset: 0x00021727
				// (set) Token: 0x06000E6D RID: 3693 RVA: 0x0002352F File Offset: 0x0002172F
				public int PageNumber { get; set; }

				// Token: 0x17000317 RID: 791
				// (get) Token: 0x06000E6E RID: 3694 RVA: 0x00023538 File Offset: 0x00021738
				// (set) Token: 0x06000E6F RID: 3695 RVA: 0x00023540 File Offset: 0x00021740
				public float Left { get; set; }

				// Token: 0x17000318 RID: 792
				// (get) Token: 0x06000E70 RID: 3696 RVA: 0x00023549 File Offset: 0x00021749
				// (set) Token: 0x06000E71 RID: 3697 RVA: 0x00023551 File Offset: 0x00021751
				public float Top { get; set; }

				// Token: 0x17000319 RID: 793
				// (get) Token: 0x06000E72 RID: 3698 RVA: 0x0002355A File Offset: 0x0002175A
				// (set) Token: 0x06000E73 RID: 3699 RVA: 0x00023562 File Offset: 0x00021762
				public float Right { get; set; }

				// Token: 0x1700031A RID: 794
				// (get) Token: 0x06000E74 RID: 3700 RVA: 0x0002356B File Offset: 0x0002176B
				// (set) Token: 0x06000E75 RID: 3701 RVA: 0x00023573 File Offset: 0x00021773
				public float Bottom { get; set; }
			}

			// Token: 0x02000222 RID: 546
			[NullableContext(2)]
			[Nullable(0)]
			internal sealed class LayoutErrorMeasurement
			{
				// Token: 0x1700031B RID: 795
				// (get) Token: 0x06000E77 RID: 3703 RVA: 0x00023584 File Offset: 0x00021784
				// (set) Token: 0x06000E78 RID: 3704 RVA: 0x0002358C File Offset: 0x0002178C
				public int PageNumber { get; set; }

				// Token: 0x1700031C RID: 796
				// (get) Token: 0x06000E79 RID: 3705 RVA: 0x00023595 File Offset: 0x00021795
				// (set) Token: 0x06000E7A RID: 3706 RVA: 0x0002359D File Offset: 0x0002179D
				public CompanionCommands.ElementSize AvailableSpace { get; set; }

				// Token: 0x1700031D RID: 797
				// (get) Token: 0x06000E7B RID: 3707 RVA: 0x000235A6 File Offset: 0x000217A6
				// (set) Token: 0x06000E7C RID: 3708 RVA: 0x000235AE File Offset: 0x000217AE
				public CompanionCommands.ElementSize MeasurementSize { get; set; }

				// Token: 0x1700031E RID: 798
				// (get) Token: 0x06000E7D RID: 3709 RVA: 0x000235B7 File Offset: 0x000217B7
				// (set) Token: 0x06000E7E RID: 3710 RVA: 0x000235BF File Offset: 0x000217BF
				public SpacePlanType? SpacePlanType { get; set; }

				// Token: 0x1700031F RID: 799
				// (get) Token: 0x06000E7F RID: 3711 RVA: 0x000235C8 File Offset: 0x000217C8
				// (set) Token: 0x06000E80 RID: 3712 RVA: 0x000235D0 File Offset: 0x000217D0
				public string WrapReason { get; set; }

				// Token: 0x17000320 RID: 800
				// (get) Token: 0x06000E81 RID: 3713 RVA: 0x000235D9 File Offset: 0x000217D9
				// (set) Token: 0x06000E82 RID: 3714 RVA: 0x000235E1 File Offset: 0x000217E1
				public bool IsLayoutErrorRootCause { get; set; }
			}

			// Token: 0x02000223 RID: 547
			[Nullable(0)]
			internal sealed class SourceCodePath
			{
				// Token: 0x17000321 RID: 801
				// (get) Token: 0x06000E84 RID: 3716 RVA: 0x000235F2 File Offset: 0x000217F2
				// (set) Token: 0x06000E85 RID: 3717 RVA: 0x000235FA File Offset: 0x000217FA
				public string FilePath { get; set; }

				// Token: 0x17000322 RID: 802
				// (get) Token: 0x06000E86 RID: 3718 RVA: 0x00023603 File Offset: 0x00021803
				// (set) Token: 0x06000E87 RID: 3719 RVA: 0x0002360B File Offset: 0x0002180B
				public int LineNumber { get; set; }
			}
		}

		// Token: 0x02000209 RID: 521
		[Nullable(0)]
		internal sealed class ProvideRenderedDocumentPage
		{
			// Token: 0x17000300 RID: 768
			// (get) Token: 0x06000E0A RID: 3594 RVA: 0x00022004 File Offset: 0x00020204
			// (set) Token: 0x06000E0B RID: 3595 RVA: 0x0002200C File Offset: 0x0002020C
			public ICollection<CompanionCommands.ProvideRenderedDocumentPage.RenderedPage> Pages { get; set; }

			// Token: 0x02000224 RID: 548
			[Nullable(0)]
			internal sealed class RenderedPage
			{
				// Token: 0x17000323 RID: 803
				// (get) Token: 0x06000E89 RID: 3721 RVA: 0x0002361C File Offset: 0x0002181C
				// (set) Token: 0x06000E8A RID: 3722 RVA: 0x00023624 File Offset: 0x00021824
				public int PageIndex { get; set; }

				// Token: 0x17000324 RID: 804
				// (get) Token: 0x06000E8B RID: 3723 RVA: 0x0002362D File Offset: 0x0002182D
				// (set) Token: 0x06000E8C RID: 3724 RVA: 0x00023635 File Offset: 0x00021835
				public int ZoomLevel { get; set; }

				// Token: 0x17000325 RID: 805
				// (get) Token: 0x06000E8D RID: 3725 RVA: 0x0002363E File Offset: 0x0002183E
				// (set) Token: 0x06000E8E RID: 3726 RVA: 0x00023646 File Offset: 0x00021846
				public string ImageData { get; set; }
			}
		}

		// Token: 0x0200020A RID: 522
		[Nullable(0)]
		internal sealed class ShowGenericException
		{
			// Token: 0x17000301 RID: 769
			// (get) Token: 0x06000E0D RID: 3597 RVA: 0x0002201D File Offset: 0x0002021D
			// (set) Token: 0x06000E0E RID: 3598 RVA: 0x00022025 File Offset: 0x00020225
			public CompanionCommands.ShowGenericException.GenericExceptionDetails Exception { get; set; }

			// Token: 0x02000225 RID: 549
			[Nullable(0)]
			internal sealed class GenericExceptionDetails
			{
				// Token: 0x17000326 RID: 806
				// (get) Token: 0x06000E90 RID: 3728 RVA: 0x00023657 File Offset: 0x00021857
				// (set) Token: 0x06000E91 RID: 3729 RVA: 0x0002365F File Offset: 0x0002185F
				public string Type { get; set; }

				// Token: 0x17000327 RID: 807
				// (get) Token: 0x06000E92 RID: 3730 RVA: 0x00023668 File Offset: 0x00021868
				// (set) Token: 0x06000E93 RID: 3731 RVA: 0x00023670 File Offset: 0x00021870
				public string Message { get; set; }

				// Token: 0x17000328 RID: 808
				// (get) Token: 0x06000E94 RID: 3732 RVA: 0x00023679 File Offset: 0x00021879
				// (set) Token: 0x06000E95 RID: 3733 RVA: 0x00023681 File Offset: 0x00021881
				public ICollection<CompanionCommands.ShowGenericException.StackFrame> StackTrace { get; set; }

				// Token: 0x17000329 RID: 809
				// (get) Token: 0x06000E96 RID: 3734 RVA: 0x0002368A File Offset: 0x0002188A
				// (set) Token: 0x06000E97 RID: 3735 RVA: 0x00023692 File Offset: 0x00021892
				[Nullable(2)]
				public CompanionCommands.ShowGenericException.GenericExceptionDetails InnerException { [NullableContext(2)] get; [NullableContext(2)] set; }
			}

			// Token: 0x02000226 RID: 550
			[Nullable(0)]
			internal sealed class StackFrame
			{
				// Token: 0x1700032A RID: 810
				// (get) Token: 0x06000E99 RID: 3737 RVA: 0x000236A3 File Offset: 0x000218A3
				// (set) Token: 0x06000E9A RID: 3738 RVA: 0x000236AB File Offset: 0x000218AB
				public string CodeLocation { get; set; }

				// Token: 0x1700032B RID: 811
				// (get) Token: 0x06000E9B RID: 3739 RVA: 0x000236B4 File Offset: 0x000218B4
				// (set) Token: 0x06000E9C RID: 3740 RVA: 0x000236BC File Offset: 0x000218BC
				[Nullable(2)]
				public string FileName { [NullableContext(2)] get; [NullableContext(2)] set; }

				// Token: 0x1700032C RID: 812
				// (get) Token: 0x06000E9D RID: 3741 RVA: 0x000236C5 File Offset: 0x000218C5
				// (set) Token: 0x06000E9E RID: 3742 RVA: 0x000236CD File Offset: 0x000218CD
				public int? LineNumber { get; set; }
			}
		}

		// Token: 0x0200020B RID: 523
		[NullableContext(0)]
		internal sealed class ElementSize
		{
			// Token: 0x17000302 RID: 770
			// (get) Token: 0x06000E10 RID: 3600 RVA: 0x00022036 File Offset: 0x00020236
			// (set) Token: 0x06000E11 RID: 3601 RVA: 0x0002203E File Offset: 0x0002023E
			public float Width { get; set; }

			// Token: 0x17000303 RID: 771
			// (get) Token: 0x06000E12 RID: 3602 RVA: 0x00022047 File Offset: 0x00020247
			// (set) Token: 0x06000E13 RID: 3603 RVA: 0x0002204F File Offset: 0x0002024F
			public float Height { get; set; }
		}

		// Token: 0x0200020C RID: 524
		[Nullable(0)]
		internal sealed class ElementProperty
		{
			// Token: 0x17000304 RID: 772
			// (get) Token: 0x06000E15 RID: 3605 RVA: 0x00022060 File Offset: 0x00020260
			// (set) Token: 0x06000E16 RID: 3606 RVA: 0x00022068 File Offset: 0x00020268
			public string Label { get; set; }

			// Token: 0x17000305 RID: 773
			// (get) Token: 0x06000E17 RID: 3607 RVA: 0x00022071 File Offset: 0x00020271
			// (set) Token: 0x06000E18 RID: 3608 RVA: 0x00022079 File Offset: 0x00020279
			public string Value { get; set; }
		}

		// Token: 0x0200020D RID: 525
		[Nullable(0)]
		internal sealed class GetVersionCommandResponse
		{
			// Token: 0x17000306 RID: 774
			// (get) Token: 0x06000E1A RID: 3610 RVA: 0x0002208A File Offset: 0x0002028A
			// (set) Token: 0x06000E1B RID: 3611 RVA: 0x00022092 File Offset: 0x00020292
			public ICollection<int> SupportedVersions { get; set; }
		}
	}
}
