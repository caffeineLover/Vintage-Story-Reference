using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using QuestPDF.Qpdf;

namespace QuestPDF.Fluent
{
	// Token: 0x02000085 RID: 133
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class DocumentOperation
	{
		// Token: 0x170000FB RID: 251
		// (get) Token: 0x0600037D RID: 893 RVA: 0x000095A7 File Offset: 0x000077A7
		// (set) Token: 0x0600037E RID: 894 RVA: 0x000095AF File Offset: 0x000077AF
		internal JobConfiguration Configuration { get; private set; }

		// Token: 0x0600037F RID: 895 RVA: 0x000095B8 File Offset: 0x000077B8
		private DocumentOperation()
		{
		}

		// Token: 0x06000380 RID: 896 RVA: 0x000095C0 File Offset: 0x000077C0
		public static DocumentOperation LoadFile(string filepath, [Nullable(2)] string password = null)
		{
			if (!File.Exists(filepath))
			{
				throw new Exception("The file could not be found");
			}
			return new DocumentOperation
			{
				Configuration = new JobConfiguration
				{
					InputFile = filepath,
					Password = password
				}
			};
		}

		// Token: 0x06000381 RID: 897 RVA: 0x000095F4 File Offset: 0x000077F4
		public DocumentOperation TakePages(string pageSelector)
		{
			JobConfiguration configuration = this.Configuration;
			if (configuration.Pages == null)
			{
				configuration.Pages = new List<JobConfiguration.PageConfiguration>();
			}
			this.Configuration.Pages.Add(new JobConfiguration.PageConfiguration
			{
				File = ".",
				Range = pageSelector
			});
			return this;
		}

		// Token: 0x06000382 RID: 898 RVA: 0x00009648 File Offset: 0x00007848
		public DocumentOperation MergeFile(string filePath, [Nullable(2)] string pageSelector = null)
		{
			if (!File.Exists(filePath))
			{
				throw new Exception("The file could not be found");
			}
			if (this.Configuration.Pages == null)
			{
				this.TakePages("1-z");
			}
			this.Configuration.Pages.Add(new JobConfiguration.PageConfiguration
			{
				File = filePath,
				Range = (pageSelector ?? "1-z")
			});
			return this;
		}

		// Token: 0x06000383 RID: 899 RVA: 0x000096B0 File Offset: 0x000078B0
		public DocumentOperation UnderlayFile(DocumentOperation.LayerConfiguration configuration)
		{
			if (!File.Exists(configuration.FilePath))
			{
				throw new Exception("The file could not be found");
			}
			JobConfiguration configuration2 = this.Configuration;
			if (configuration2.Underlay == null)
			{
				configuration2.Underlay = new List<JobConfiguration.LayerConfiguration>();
			}
			this.Configuration.Underlay.Add(new JobConfiguration.LayerConfiguration
			{
				File = configuration.FilePath,
				To = configuration.TargetPages,
				From = configuration.SourcePages,
				Repeat = configuration.RepeatSourcePages
			});
			return this;
		}

		// Token: 0x06000384 RID: 900 RVA: 0x00009738 File Offset: 0x00007938
		public DocumentOperation OverlayFile(DocumentOperation.LayerConfiguration configuration)
		{
			if (!File.Exists(configuration.FilePath))
			{
				throw new Exception("The file could not be found");
			}
			JobConfiguration configuration2 = this.Configuration;
			if (configuration2.Overlay == null)
			{
				configuration2.Overlay = new List<JobConfiguration.LayerConfiguration>();
			}
			this.Configuration.Overlay.Add(new JobConfiguration.LayerConfiguration
			{
				File = configuration.FilePath,
				To = configuration.TargetPages,
				From = configuration.SourcePages,
				Repeat = configuration.RepeatSourcePages
			});
			return this;
		}

		// Token: 0x06000385 RID: 901 RVA: 0x000097BF File Offset: 0x000079BF
		public DocumentOperation ExtendMetadata(string metadata)
		{
			this.Configuration.ExtendMetadata = metadata;
			return this;
		}

		// Token: 0x06000386 RID: 902 RVA: 0x000097D0 File Offset: 0x000079D0
		public DocumentOperation AddAttachment(DocumentOperation.DocumentAttachment attachment)
		{
			DocumentOperation.<>c__DisplayClass18_0 CS$<>8__locals1;
			CS$<>8__locals1.attachment = attachment;
			JobConfiguration configuration = this.Configuration;
			if (configuration.AddAttachment == null)
			{
				configuration.AddAttachment = new List<JobConfiguration.AddDocumentAttachment>();
			}
			if (!File.Exists(CS$<>8__locals1.attachment.FilePath))
			{
				throw new Exception("The file could not be found");
			}
			FileInfo fileInfo = new FileInfo(CS$<>8__locals1.attachment.FilePath);
			this.Configuration.AddAttachment.Add(new JobConfiguration.AddDocumentAttachment
			{
				Key = (CS$<>8__locals1.attachment.Key ?? Path.GetFileName(CS$<>8__locals1.attachment.FilePath)),
				File = CS$<>8__locals1.attachment.FilePath,
				FileName = (CS$<>8__locals1.attachment.AttachmentName ?? fileInfo.Name),
				CreationDate = DocumentOperation.<AddAttachment>g__GetFormattedDate|18_1(CS$<>8__locals1.attachment.CreationDate, File.GetCreationTimeUtc(CS$<>8__locals1.attachment.FilePath)),
				ModificationDate = DocumentOperation.<AddAttachment>g__GetFormattedDate|18_1(CS$<>8__locals1.attachment.ModificationDate, File.GetLastWriteTime(CS$<>8__locals1.attachment.FilePath)),
				MimeType = (CS$<>8__locals1.attachment.MimeType ?? DocumentOperation.<AddAttachment>g__GetDefaultMimeType|18_0(ref CS$<>8__locals1)),
				Description = CS$<>8__locals1.attachment.Description,
				Replace = (CS$<>8__locals1.attachment.Replace ? string.Empty : null),
				Relationship = DocumentOperation.<AddAttachment>g__GetRelationship|18_2(CS$<>8__locals1.attachment.Relationship)
			});
			return this;
		}

		// Token: 0x06000387 RID: 903 RVA: 0x00009940 File Offset: 0x00007B40
		public DocumentOperation Decrypt()
		{
			this.Configuration.Decrypt = string.Empty;
			return this;
		}

		// Token: 0x06000388 RID: 904 RVA: 0x00009953 File Offset: 0x00007B53
		public DocumentOperation RemoveRestrictions()
		{
			this.Configuration.Decrypt = string.Empty;
			this.Configuration.RemoveRestrictions = string.Empty;
			return this;
		}

		// Token: 0x06000389 RID: 905 RVA: 0x00009978 File Offset: 0x00007B78
		public DocumentOperation Encrypt(DocumentOperation.Encryption40Bit encryption)
		{
			if (this.Configuration.Encrypt != null)
			{
				throw new InvalidOperationException("Encryption process can be set only once");
			}
			this.Configuration.Encrypt = new JobConfiguration.EncryptionSettings
			{
				UserPassword = encryption.UserPassword,
				OwnerPassword = encryption.OwnerPassword,
				Options40Bit = new JobConfiguration.Encryption40Bit
				{
					Annotate = this.FormatBooleanFlag(encryption.AllowAnnotation),
					Extract = this.FormatBooleanFlag(encryption.AllowContentExtraction),
					Modify = (encryption.AllowModification ? "all" : "none"),
					Print = (encryption.AllowPrinting ? "full" : "none")
				}
			};
			return this;
		}

		// Token: 0x0600038A RID: 906 RVA: 0x00009A2C File Offset: 0x00007C2C
		public DocumentOperation Encrypt(DocumentOperation.Encryption128Bit encryption)
		{
			if (this.Configuration.Encrypt != null)
			{
				throw new InvalidOperationException("Encryption process can be set only once");
			}
			this.Configuration.Encrypt = new JobConfiguration.EncryptionSettings
			{
				UserPassword = encryption.UserPassword,
				OwnerPassword = encryption.OwnerPassword,
				Options128Bit = new JobConfiguration.Encryption128Bit
				{
					Annotate = this.FormatBooleanFlag(encryption.AllowAnnotation),
					Assemble = this.FormatBooleanFlag(encryption.AllowAssembly),
					Extract = this.FormatBooleanFlag(encryption.AllowContentExtraction),
					Form = this.FormatBooleanFlag(encryption.AllowFillingForms),
					Print = (encryption.AllowPrinting ? "full" : "none"),
					CleartextMetadata = (encryption.EncryptMetadata ? null : string.Empty)
				}
			};
			return this;
		}

		// Token: 0x0600038B RID: 907 RVA: 0x00009B00 File Offset: 0x00007D00
		public DocumentOperation Encrypt(DocumentOperation.Encryption256Bit encryption)
		{
			if (this.Configuration.Encrypt != null)
			{
				throw new InvalidOperationException("Encryption process can be set only once");
			}
			this.Configuration.Encrypt = new JobConfiguration.EncryptionSettings
			{
				UserPassword = encryption.UserPassword,
				OwnerPassword = encryption.OwnerPassword,
				Options256Bit = new JobConfiguration.Encryption256Bit
				{
					Annotate = this.FormatBooleanFlag(encryption.AllowAnnotation),
					Assemble = this.FormatBooleanFlag(encryption.AllowAssembly),
					Extract = this.FormatBooleanFlag(encryption.AllowContentExtraction),
					Form = this.FormatBooleanFlag(encryption.AllowFillingForms),
					Print = (encryption.AllowPrinting ? "full" : "none"),
					CleartextMetadata = (encryption.EncryptMetadata ? null : string.Empty)
				}
			};
			return this;
		}

		// Token: 0x0600038C RID: 908 RVA: 0x00009BD1 File Offset: 0x00007DD1
		private string FormatBooleanFlag(bool value)
		{
			if (!value)
			{
				return "n";
			}
			return "y";
		}

		// Token: 0x0600038D RID: 909 RVA: 0x00009BE1 File Offset: 0x00007DE1
		public DocumentOperation Linearize()
		{
			this.Configuration.Linearize = string.Empty;
			return this;
		}

		// Token: 0x0600038E RID: 910 RVA: 0x00009BF4 File Offset: 0x00007DF4
		public void Save(string filePath)
		{
			if (File.Exists(filePath))
			{
				File.Delete(filePath);
			}
			this.Configuration.OutputFile = filePath;
			QpdfAPI.ExecuteJob(SimpleJsonSerializer.Serialize(this.Configuration));
		}

		// Token: 0x0600038F RID: 911 RVA: 0x00009C20 File Offset: 0x00007E20
		[CompilerGenerated]
		internal static string <AddAttachment>g__GetDefaultMimeType|18_0(ref DocumentOperation.<>c__DisplayClass18_0 A_0)
		{
			string text = Path.GetExtension(A_0.attachment.FilePath);
			text = text.TrimStart('.').ToLowerInvariant();
			string result;
			if (!MimeHelper.FileExtensionToMimeConversionTable.TryGetValue(text, ref result))
			{
				return "text/plain";
			}
			return result;
		}

		// Token: 0x06000390 RID: 912 RVA: 0x00009C64 File Offset: 0x00007E64
		[CompilerGenerated]
		internal static string <AddAttachment>g__GetFormattedDate|18_1(DateTime? value, DateTime defaultValue)
		{
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
			defaultInterpolatedStringHandler..ctor(3, 1);
			defaultInterpolatedStringHandler.AppendLiteral("D:");
			defaultInterpolatedStringHandler.AppendFormatted<DateTime>(value.GetValueOrDefault(defaultValue).ToUniversalTime(), "yyyyMMddHHmmsss");
			defaultInterpolatedStringHandler.AppendLiteral("Z");
			return defaultInterpolatedStringHandler.ToStringAndClear();
		}

		// Token: 0x06000391 RID: 913 RVA: 0x00009CB8 File Offset: 0x00007EB8
		[NullableContext(2)]
		[CompilerGenerated]
		internal static string <AddAttachment>g__GetRelationship|18_2(DocumentOperation.DocumentAttachmentRelationship? relationship)
		{
			string result;
			if (relationship != null)
			{
				switch (relationship.GetValueOrDefault())
				{
				case DocumentOperation.DocumentAttachmentRelationship.Data:
					result = "/Data";
					break;
				case DocumentOperation.DocumentAttachmentRelationship.Source:
					result = "/Source";
					break;
				case DocumentOperation.DocumentAttachmentRelationship.Alternative:
					result = "/Alternative";
					break;
				case DocumentOperation.DocumentAttachmentRelationship.Supplement:
					result = "/Alternative";
					break;
				case DocumentOperation.DocumentAttachmentRelationship.Unspecified:
					result = "/Unspecified";
					break;
				default:
					throw new ArgumentOutOfRangeException("relationship", relationship, null);
				}
			}
			else
			{
				result = null;
			}
			return result;
		}

		// Token: 0x02000187 RID: 391
		[NullableContext(2)]
		[Nullable(0)]
		public sealed class LayerConfiguration
		{
			// Token: 0x170002B3 RID: 691
			// (get) Token: 0x06000BFA RID: 3066 RVA: 0x0001DA8F File Offset: 0x0001BC8F
			// (set) Token: 0x06000BFB RID: 3067 RVA: 0x0001DA97 File Offset: 0x0001BC97
			[Nullable(1)]
			public string FilePath { [NullableContext(1)] get; [NullableContext(1)] set; }

			// Token: 0x170002B4 RID: 692
			// (get) Token: 0x06000BFC RID: 3068 RVA: 0x0001DAA0 File Offset: 0x0001BCA0
			// (set) Token: 0x06000BFD RID: 3069 RVA: 0x0001DAA8 File Offset: 0x0001BCA8
			public string TargetPages { get; set; }

			// Token: 0x170002B5 RID: 693
			// (get) Token: 0x06000BFE RID: 3070 RVA: 0x0001DAB1 File Offset: 0x0001BCB1
			// (set) Token: 0x06000BFF RID: 3071 RVA: 0x0001DAB9 File Offset: 0x0001BCB9
			public string SourcePages { get; set; }

			// Token: 0x170002B6 RID: 694
			// (get) Token: 0x06000C00 RID: 3072 RVA: 0x0001DAC2 File Offset: 0x0001BCC2
			// (set) Token: 0x06000C01 RID: 3073 RVA: 0x0001DACA File Offset: 0x0001BCCA
			public string RepeatSourcePages { get; set; }
		}

		// Token: 0x02000188 RID: 392
		[NullableContext(0)]
		public enum DocumentAttachmentRelationship
		{
			// Token: 0x040005C8 RID: 1480
			Data,
			// Token: 0x040005C9 RID: 1481
			Source,
			// Token: 0x040005CA RID: 1482
			Alternative,
			// Token: 0x040005CB RID: 1483
			Supplement,
			// Token: 0x040005CC RID: 1484
			Unspecified
		}

		// Token: 0x02000189 RID: 393
		[NullableContext(2)]
		[Nullable(0)]
		public sealed class DocumentAttachment
		{
			// Token: 0x170002B7 RID: 695
			// (get) Token: 0x06000C03 RID: 3075 RVA: 0x0001DADB File Offset: 0x0001BCDB
			// (set) Token: 0x06000C04 RID: 3076 RVA: 0x0001DAE3 File Offset: 0x0001BCE3
			public string Key { get; set; }

			// Token: 0x170002B8 RID: 696
			// (get) Token: 0x06000C05 RID: 3077 RVA: 0x0001DAEC File Offset: 0x0001BCEC
			// (set) Token: 0x06000C06 RID: 3078 RVA: 0x0001DAF4 File Offset: 0x0001BCF4
			[Nullable(1)]
			public string FilePath { [NullableContext(1)] get; [NullableContext(1)] set; }

			// Token: 0x170002B9 RID: 697
			// (get) Token: 0x06000C07 RID: 3079 RVA: 0x0001DAFD File Offset: 0x0001BCFD
			// (set) Token: 0x06000C08 RID: 3080 RVA: 0x0001DB05 File Offset: 0x0001BD05
			public string AttachmentName { get; set; }

			// Token: 0x170002BA RID: 698
			// (get) Token: 0x06000C09 RID: 3081 RVA: 0x0001DB0E File Offset: 0x0001BD0E
			// (set) Token: 0x06000C0A RID: 3082 RVA: 0x0001DB16 File Offset: 0x0001BD16
			public DateTime? CreationDate { get; set; }

			// Token: 0x170002BB RID: 699
			// (get) Token: 0x06000C0B RID: 3083 RVA: 0x0001DB1F File Offset: 0x0001BD1F
			// (set) Token: 0x06000C0C RID: 3084 RVA: 0x0001DB27 File Offset: 0x0001BD27
			public DateTime? ModificationDate { get; set; }

			// Token: 0x170002BC RID: 700
			// (get) Token: 0x06000C0D RID: 3085 RVA: 0x0001DB30 File Offset: 0x0001BD30
			// (set) Token: 0x06000C0E RID: 3086 RVA: 0x0001DB38 File Offset: 0x0001BD38
			public string MimeType { get; set; }

			// Token: 0x170002BD RID: 701
			// (get) Token: 0x06000C0F RID: 3087 RVA: 0x0001DB41 File Offset: 0x0001BD41
			// (set) Token: 0x06000C10 RID: 3088 RVA: 0x0001DB49 File Offset: 0x0001BD49
			public string Description { get; set; }

			// Token: 0x170002BE RID: 702
			// (get) Token: 0x06000C11 RID: 3089 RVA: 0x0001DB52 File Offset: 0x0001BD52
			// (set) Token: 0x06000C12 RID: 3090 RVA: 0x0001DB5A File Offset: 0x0001BD5A
			public bool Replace { get; set; } = true;

			// Token: 0x170002BF RID: 703
			// (get) Token: 0x06000C13 RID: 3091 RVA: 0x0001DB63 File Offset: 0x0001BD63
			// (set) Token: 0x06000C14 RID: 3092 RVA: 0x0001DB6B File Offset: 0x0001BD6B
			public DocumentOperation.DocumentAttachmentRelationship? Relationship { get; set; }
		}

		// Token: 0x0200018A RID: 394
		[Nullable(0)]
		public class EncryptionBase
		{
			// Token: 0x170002C0 RID: 704
			// (get) Token: 0x06000C16 RID: 3094 RVA: 0x0001DB83 File Offset: 0x0001BD83
			// (set) Token: 0x06000C17 RID: 3095 RVA: 0x0001DB8B File Offset: 0x0001BD8B
			[Nullable(2)]
			public string UserPassword { [NullableContext(2)] get; [NullableContext(2)] set; }

			// Token: 0x170002C1 RID: 705
			// (get) Token: 0x06000C18 RID: 3096 RVA: 0x0001DB94 File Offset: 0x0001BD94
			// (set) Token: 0x06000C19 RID: 3097 RVA: 0x0001DB9C File Offset: 0x0001BD9C
			public string OwnerPassword { get; set; }
		}

		// Token: 0x0200018B RID: 395
		[NullableContext(0)]
		public sealed class Encryption40Bit : DocumentOperation.EncryptionBase
		{
			// Token: 0x170002C2 RID: 706
			// (get) Token: 0x06000C1B RID: 3099 RVA: 0x0001DBAD File Offset: 0x0001BDAD
			// (set) Token: 0x06000C1C RID: 3100 RVA: 0x0001DBB5 File Offset: 0x0001BDB5
			public bool AllowAnnotation { get; set; } = true;

			// Token: 0x170002C3 RID: 707
			// (get) Token: 0x06000C1D RID: 3101 RVA: 0x0001DBBE File Offset: 0x0001BDBE
			// (set) Token: 0x06000C1E RID: 3102 RVA: 0x0001DBC6 File Offset: 0x0001BDC6
			public bool AllowContentExtraction { get; set; } = true;

			// Token: 0x170002C4 RID: 708
			// (get) Token: 0x06000C1F RID: 3103 RVA: 0x0001DBCF File Offset: 0x0001BDCF
			// (set) Token: 0x06000C20 RID: 3104 RVA: 0x0001DBD7 File Offset: 0x0001BDD7
			public bool AllowModification { get; set; } = true;

			// Token: 0x170002C5 RID: 709
			// (get) Token: 0x06000C21 RID: 3105 RVA: 0x0001DBE0 File Offset: 0x0001BDE0
			// (set) Token: 0x06000C22 RID: 3106 RVA: 0x0001DBE8 File Offset: 0x0001BDE8
			public bool AllowPrinting { get; set; } = true;
		}

		// Token: 0x0200018C RID: 396
		[NullableContext(0)]
		public sealed class Encryption128Bit : DocumentOperation.EncryptionBase
		{
			// Token: 0x170002C6 RID: 710
			// (get) Token: 0x06000C24 RID: 3108 RVA: 0x0001DC15 File Offset: 0x0001BE15
			// (set) Token: 0x06000C25 RID: 3109 RVA: 0x0001DC1D File Offset: 0x0001BE1D
			public bool AllowAnnotation { get; set; } = true;

			// Token: 0x170002C7 RID: 711
			// (get) Token: 0x06000C26 RID: 3110 RVA: 0x0001DC26 File Offset: 0x0001BE26
			// (set) Token: 0x06000C27 RID: 3111 RVA: 0x0001DC2E File Offset: 0x0001BE2E
			public bool AllowAssembly { get; set; } = true;

			// Token: 0x170002C8 RID: 712
			// (get) Token: 0x06000C28 RID: 3112 RVA: 0x0001DC37 File Offset: 0x0001BE37
			// (set) Token: 0x06000C29 RID: 3113 RVA: 0x0001DC3F File Offset: 0x0001BE3F
			public bool AllowContentExtraction { get; set; } = true;

			// Token: 0x170002C9 RID: 713
			// (get) Token: 0x06000C2A RID: 3114 RVA: 0x0001DC48 File Offset: 0x0001BE48
			// (set) Token: 0x06000C2B RID: 3115 RVA: 0x0001DC50 File Offset: 0x0001BE50
			public bool AllowFillingForms { get; set; } = true;

			// Token: 0x170002CA RID: 714
			// (get) Token: 0x06000C2C RID: 3116 RVA: 0x0001DC59 File Offset: 0x0001BE59
			// (set) Token: 0x06000C2D RID: 3117 RVA: 0x0001DC61 File Offset: 0x0001BE61
			public bool AllowPrinting { get; set; } = true;

			// Token: 0x170002CB RID: 715
			// (get) Token: 0x06000C2E RID: 3118 RVA: 0x0001DC6A File Offset: 0x0001BE6A
			// (set) Token: 0x06000C2F RID: 3119 RVA: 0x0001DC72 File Offset: 0x0001BE72
			public bool EncryptMetadata { get; set; } = true;
		}

		// Token: 0x0200018D RID: 397
		[NullableContext(0)]
		public sealed class Encryption256Bit : DocumentOperation.EncryptionBase
		{
			// Token: 0x170002CC RID: 716
			// (get) Token: 0x06000C31 RID: 3121 RVA: 0x0001DCAD File Offset: 0x0001BEAD
			// (set) Token: 0x06000C32 RID: 3122 RVA: 0x0001DCB5 File Offset: 0x0001BEB5
			public bool AllowAnnotation { get; set; } = true;

			// Token: 0x170002CD RID: 717
			// (get) Token: 0x06000C33 RID: 3123 RVA: 0x0001DCBE File Offset: 0x0001BEBE
			// (set) Token: 0x06000C34 RID: 3124 RVA: 0x0001DCC6 File Offset: 0x0001BEC6
			public bool AllowAssembly { get; set; } = true;

			// Token: 0x170002CE RID: 718
			// (get) Token: 0x06000C35 RID: 3125 RVA: 0x0001DCCF File Offset: 0x0001BECF
			// (set) Token: 0x06000C36 RID: 3126 RVA: 0x0001DCD7 File Offset: 0x0001BED7
			public bool AllowContentExtraction { get; set; } = true;

			// Token: 0x170002CF RID: 719
			// (get) Token: 0x06000C37 RID: 3127 RVA: 0x0001DCE0 File Offset: 0x0001BEE0
			// (set) Token: 0x06000C38 RID: 3128 RVA: 0x0001DCE8 File Offset: 0x0001BEE8
			public bool AllowFillingForms { get; set; } = true;

			// Token: 0x170002D0 RID: 720
			// (get) Token: 0x06000C39 RID: 3129 RVA: 0x0001DCF1 File Offset: 0x0001BEF1
			// (set) Token: 0x06000C3A RID: 3130 RVA: 0x0001DCF9 File Offset: 0x0001BEF9
			public bool AllowPrinting { get; set; } = true;

			// Token: 0x170002D1 RID: 721
			// (get) Token: 0x06000C3B RID: 3131 RVA: 0x0001DD02 File Offset: 0x0001BF02
			// (set) Token: 0x06000C3C RID: 3132 RVA: 0x0001DD0A File Offset: 0x0001BF0A
			public bool EncryptMetadata { get; set; } = true;
		}
	}
}
