using System;
using System.Collections.Generic;
using System.Text;
using UglyToad.PdfPig.Tokens;
using UglyToad.PdfPig.Util;
using UglyToad.PdfPig.Util.JetBrains.Annotations;

namespace UglyToad.PdfPig.Content
{
	// Token: 0x020001E0 RID: 480
	public class DocumentInformation
	{
		// Token: 0x1700030D RID: 781
		// (get) Token: 0x06000B5B RID: 2907 RVA: 0x0003984D File Offset: 0x00037A4D
		internal static DocumentInformation Default { get; } = new DocumentInformation(null, null, null, null, null, null, null, null, null);

		// Token: 0x1700030E RID: 782
		// (get) Token: 0x06000B5C RID: 2908 RVA: 0x00039854 File Offset: 0x00037A54
		public DictionaryToken DocumentInformationDictionary { get; }

		// Token: 0x1700030F RID: 783
		// (get) Token: 0x06000B5D RID: 2909 RVA: 0x0003985C File Offset: 0x00037A5C
		[CanBeNull]
		public string Title { get; }

		// Token: 0x17000310 RID: 784
		// (get) Token: 0x06000B5E RID: 2910 RVA: 0x00039864 File Offset: 0x00037A64
		[CanBeNull]
		public string Author { get; }

		// Token: 0x17000311 RID: 785
		// (get) Token: 0x06000B5F RID: 2911 RVA: 0x0003986C File Offset: 0x00037A6C
		[CanBeNull]
		public string Subject { get; }

		// Token: 0x17000312 RID: 786
		// (get) Token: 0x06000B60 RID: 2912 RVA: 0x00039874 File Offset: 0x00037A74
		[CanBeNull]
		public string Keywords { get; }

		// Token: 0x17000313 RID: 787
		// (get) Token: 0x06000B61 RID: 2913 RVA: 0x0003987C File Offset: 0x00037A7C
		[CanBeNull]
		public string Creator { get; }

		// Token: 0x17000314 RID: 788
		// (get) Token: 0x06000B62 RID: 2914 RVA: 0x00039884 File Offset: 0x00037A84
		[CanBeNull]
		public string Producer { get; }

		// Token: 0x17000315 RID: 789
		// (get) Token: 0x06000B63 RID: 2915 RVA: 0x0003988C File Offset: 0x00037A8C
		[CanBeNull]
		public string CreationDate { get; }

		// Token: 0x17000316 RID: 790
		// (get) Token: 0x06000B64 RID: 2916 RVA: 0x00039894 File Offset: 0x00037A94
		[CanBeNull]
		public string ModifiedDate { get; }

		// Token: 0x06000B65 RID: 2917 RVA: 0x0003989C File Offset: 0x00037A9C
		internal DocumentInformation(DictionaryToken documentInformationDictionary, string title, string author, string subject, string keywords, string creator, string producer, string creationDate, string modifiedDate)
		{
			this.DocumentInformationDictionary = (documentInformationDictionary ?? new DictionaryToken(new Dictionary<NameToken, IToken>()));
			this.Title = title;
			this.Author = author;
			this.Subject = subject;
			this.Keywords = keywords;
			this.Creator = creator;
			this.Producer = producer;
			this.CreationDate = creationDate;
			this.ModifiedDate = modifiedDate;
			StringBuilder stringBuilder = new StringBuilder();
			DocumentInformation.AppendPart("Title", title, stringBuilder);
			DocumentInformation.AppendPart("Author", author, stringBuilder);
			DocumentInformation.AppendPart("Subject", subject, stringBuilder);
			DocumentInformation.AppendPart("Keywords", keywords, stringBuilder);
			DocumentInformation.AppendPart("Creator", creator, stringBuilder);
			DocumentInformation.AppendPart("Producer", producer, stringBuilder);
			DocumentInformation.AppendPart("CreationDate", creationDate, stringBuilder);
			DocumentInformation.AppendPart("ModifiedDate", modifiedDate, stringBuilder);
			this.representation = stringBuilder.ToString();
		}

		// Token: 0x06000B66 RID: 2918 RVA: 0x0003997C File Offset: 0x00037B7C
		public DateTimeOffset? GetCreatedDateTimeOffset()
		{
			DateTimeOffset dateTimeOffset;
			if (!DateFormatHelper.TryParseDateTimeOffset(this.CreationDate, out dateTimeOffset))
			{
				return default(DateTimeOffset?);
			}
			return new DateTimeOffset?(dateTimeOffset);
		}

		// Token: 0x06000B67 RID: 2919 RVA: 0x000399A8 File Offset: 0x00037BA8
		public DateTimeOffset? GetModifiedDateTimeOffset()
		{
			DateTimeOffset dateTimeOffset;
			if (!DateFormatHelper.TryParseDateTimeOffset(this.ModifiedDate, out dateTimeOffset))
			{
				return default(DateTimeOffset?);
			}
			return new DateTimeOffset?(dateTimeOffset);
		}

		// Token: 0x06000B68 RID: 2920 RVA: 0x000399D4 File Offset: 0x00037BD4
		public override string ToString()
		{
			return this.representation;
		}

		// Token: 0x06000B69 RID: 2921 RVA: 0x000399DC File Offset: 0x00037BDC
		private static void AppendPart(string name, string value, StringBuilder builder)
		{
			if (value == null)
			{
				return;
			}
			builder.Append(name).Append(": ").Append(value).Append("; ");
		}

		// Token: 0x040005DF RID: 1503
		private readonly string representation;
	}
}
