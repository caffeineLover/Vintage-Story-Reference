using System;
using System.Collections.Generic;
using UglyToad.PdfPig.Core;
using UglyToad.PdfPig.Graphics;
using UglyToad.PdfPig.Tokens;

namespace UglyToad.PdfPig.Content
{
	// Token: 0x020001DD RID: 477
	public class ArtifactMarkedContentElement : MarkedContentElement
	{
		// Token: 0x17000300 RID: 768
		// (get) Token: 0x06000B48 RID: 2888 RVA: 0x000396A8 File Offset: 0x000378A8
		public ArtifactMarkedContentElement.ArtifactType Type { get; }

		// Token: 0x17000301 RID: 769
		// (get) Token: 0x06000B49 RID: 2889 RVA: 0x000396B0 File Offset: 0x000378B0
		public string SubType { get; }

		// Token: 0x17000302 RID: 770
		// (get) Token: 0x06000B4A RID: 2890 RVA: 0x000396B8 File Offset: 0x000378B8
		public string AttributeOwners { get; }

		// Token: 0x17000303 RID: 771
		// (get) Token: 0x06000B4B RID: 2891 RVA: 0x000396C0 File Offset: 0x000378C0
		public PdfRectangle? BoundingBox { get; }

		// Token: 0x17000304 RID: 772
		// (get) Token: 0x06000B4C RID: 2892 RVA: 0x000396C8 File Offset: 0x000378C8
		// (set) Token: 0x06000B4D RID: 2893 RVA: 0x000396D0 File Offset: 0x000378D0
		public IReadOnlyList<NameToken> Attached { get; set; }

		// Token: 0x17000305 RID: 773
		// (get) Token: 0x06000B4E RID: 2894 RVA: 0x000396D9 File Offset: 0x000378D9
		public bool IsTopAttached
		{
			get
			{
				return this.IsAttached(NameToken.Top);
			}
		}

		// Token: 0x17000306 RID: 774
		// (get) Token: 0x06000B4F RID: 2895 RVA: 0x000396E6 File Offset: 0x000378E6
		public bool IsBottomAttached
		{
			get
			{
				return this.IsAttached(NameToken.Bottom);
			}
		}

		// Token: 0x17000307 RID: 775
		// (get) Token: 0x06000B50 RID: 2896 RVA: 0x000396F3 File Offset: 0x000378F3
		public bool IsLeftAttached
		{
			get
			{
				return this.IsAttached(NameToken.Left);
			}
		}

		// Token: 0x17000308 RID: 776
		// (get) Token: 0x06000B51 RID: 2897 RVA: 0x00039700 File Offset: 0x00037900
		public bool IsRightAttached
		{
			get
			{
				return this.IsAttached(NameToken.Right);
			}
		}

		// Token: 0x06000B52 RID: 2898 RVA: 0x00039710 File Offset: 0x00037910
		internal ArtifactMarkedContentElement(int markedContentIdentifier, NameToken tag, DictionaryToken properties, string language, string actualText, string alternateDescription, string expandedForm, ArtifactMarkedContentElement.ArtifactType artifactType, string subType, string attributeOwners, PdfRectangle? boundingBox, IReadOnlyList<NameToken> attached, IReadOnlyList<MarkedContentElement> children, IReadOnlyList<Letter> letters, IReadOnlyList<PdfPath> paths, IReadOnlyList<IPdfImage> images, int index) : base(markedContentIdentifier, tag, properties, language, actualText, alternateDescription, expandedForm, true, children, letters, paths, images, index)
		{
			this.Type = artifactType;
			this.SubType = subType;
			this.AttributeOwners = attributeOwners;
			this.BoundingBox = boundingBox;
			this.Attached = (attached ?? EmptyArray<NameToken>.Instance);
		}

		// Token: 0x06000B53 RID: 2899 RVA: 0x0003976C File Offset: 0x0003796C
		private bool IsAttached(NameToken edge)
		{
			using (IEnumerator<NameToken> enumerator = this.Attached.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current == edge)
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x020002BF RID: 703
		public enum ArtifactType
		{
			// Token: 0x040008F0 RID: 2288
			Unknown,
			// Token: 0x040008F1 RID: 2289
			Pagination,
			// Token: 0x040008F2 RID: 2290
			Layout,
			// Token: 0x040008F3 RID: 2291
			Page,
			// Token: 0x040008F4 RID: 2292
			Background
		}
	}
}
