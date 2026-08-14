using System;
using System.Runtime.CompilerServices;
using UglyToad.PdfPig.Core;
using UglyToad.PdfPig.Tokens;
using UglyToad.PdfPig.Util.JetBrains.Annotations;

namespace UglyToad.PdfPig.AcroForms.Fields
{
	// Token: 0x02000215 RID: 533
	public abstract class AcroFieldBase
	{
		// Token: 0x170003B0 RID: 944
		// (get) Token: 0x06000C92 RID: 3218 RVA: 0x0003F31E File Offset: 0x0003D51E
		[NotNull]
		public DictionaryToken Dictionary { get; }

		// Token: 0x170003B1 RID: 945
		// (get) Token: 0x06000C93 RID: 3219 RVA: 0x0003F326 File Offset: 0x0003D526
		[NotNull]
		public string RawFieldType { get; }

		// Token: 0x170003B2 RID: 946
		// (get) Token: 0x06000C94 RID: 3220 RVA: 0x0003F32E File Offset: 0x0003D52E
		public AcroFieldType FieldType { get; }

		// Token: 0x170003B3 RID: 947
		// (get) Token: 0x06000C95 RID: 3221 RVA: 0x0003F336 File Offset: 0x0003D536
		public uint FieldFlags { get; }

		// Token: 0x170003B4 RID: 948
		// (get) Token: 0x06000C96 RID: 3222 RVA: 0x0003F33E File Offset: 0x0003D53E
		[NotNull]
		public AcroFieldCommonInformation Information { get; }

		// Token: 0x170003B5 RID: 949
		// (get) Token: 0x06000C97 RID: 3223 RVA: 0x0003F346 File Offset: 0x0003D546
		public int? PageNumber { get; }

		// Token: 0x170003B6 RID: 950
		// (get) Token: 0x06000C98 RID: 3224 RVA: 0x0003F34E File Offset: 0x0003D54E
		public PdfRectangle? Bounds { get; }

		// Token: 0x06000C99 RID: 3225 RVA: 0x0003F358 File Offset: 0x0003D558
		protected AcroFieldBase(DictionaryToken dictionary, string rawFieldType, uint fieldFlags, AcroFieldType fieldType, AcroFieldCommonInformation information, int? pageNumber, PdfRectangle? bounds)
		{
			if (dictionary == null)
			{
				throw new ArgumentNullException("dictionary");
			}
			this.Dictionary = dictionary;
			if (rawFieldType == null)
			{
				throw new ArgumentNullException("rawFieldType");
			}
			this.RawFieldType = rawFieldType;
			this.FieldFlags = fieldFlags;
			this.FieldType = fieldType;
			this.Information = (information ?? new AcroFieldCommonInformation(default(IndirectReference?), null, null, null));
			this.PageNumber = pageNumber;
			this.Bounds = bounds;
		}

		// Token: 0x06000C9A RID: 3226 RVA: 0x0003F3D4 File Offset: 0x0003D5D4
		public override string ToString()
		{
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
			defaultInterpolatedStringHandler..ctor(0, 1);
			defaultInterpolatedStringHandler.AppendFormatted<AcroFieldType>(this.FieldType);
			return defaultInterpolatedStringHandler.ToStringAndClear();
		}
	}
}
