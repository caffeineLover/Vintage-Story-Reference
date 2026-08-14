using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace QuestPDF.Qpdf
{
	// Token: 0x02000031 RID: 49
	[NullableContext(2)]
	[Nullable(0)]
	internal sealed class JobConfiguration
	{
		// Token: 0x1700003D RID: 61
		// (get) Token: 0x06000118 RID: 280 RVA: 0x00004397 File Offset: 0x00002597
		// (set) Token: 0x06000119 RID: 281 RVA: 0x0000439F File Offset: 0x0000259F
		[Nullable(1)]
		[SimpleJsonPropertyName("inputFile")]
		public string InputFile { [NullableContext(1)] get; [NullableContext(1)] set; }

		// Token: 0x1700003E RID: 62
		// (get) Token: 0x0600011A RID: 282 RVA: 0x000043A8 File Offset: 0x000025A8
		// (set) Token: 0x0600011B RID: 283 RVA: 0x000043B0 File Offset: 0x000025B0
		[SimpleJsonPropertyName("password")]
		public string Password { get; set; }

		// Token: 0x1700003F RID: 63
		// (get) Token: 0x0600011C RID: 284 RVA: 0x000043B9 File Offset: 0x000025B9
		// (set) Token: 0x0600011D RID: 285 RVA: 0x000043C1 File Offset: 0x000025C1
		[Nullable(1)]
		[SimpleJsonPropertyName("outputFile")]
		public string OutputFile { [NullableContext(1)] get; [NullableContext(1)] set; }

		// Token: 0x17000040 RID: 64
		// (get) Token: 0x0600011E RID: 286 RVA: 0x000043CA File Offset: 0x000025CA
		// (set) Token: 0x0600011F RID: 287 RVA: 0x000043D2 File Offset: 0x000025D2
		[Nullable(new byte[]
		{
			2,
			1
		})]
		[SimpleJsonPropertyName("pages")]
		public ICollection<JobConfiguration.PageConfiguration> Pages { [return: Nullable(new byte[]
		{
			2,
			1
		})] get; [param: Nullable(new byte[]
		{
			2,
			1
		})] set; }

		// Token: 0x17000041 RID: 65
		// (get) Token: 0x06000120 RID: 288 RVA: 0x000043DB File Offset: 0x000025DB
		// (set) Token: 0x06000121 RID: 289 RVA: 0x000043E3 File Offset: 0x000025E3
		[Nullable(new byte[]
		{
			2,
			1
		})]
		[SimpleJsonPropertyName("overlay")]
		public ICollection<JobConfiguration.LayerConfiguration> Overlay { [return: Nullable(new byte[]
		{
			2,
			1
		})] get; [param: Nullable(new byte[]
		{
			2,
			1
		})] set; }

		// Token: 0x17000042 RID: 66
		// (get) Token: 0x06000122 RID: 290 RVA: 0x000043EC File Offset: 0x000025EC
		// (set) Token: 0x06000123 RID: 291 RVA: 0x000043F4 File Offset: 0x000025F4
		[Nullable(new byte[]
		{
			2,
			1
		})]
		[SimpleJsonPropertyName("underlay")]
		public ICollection<JobConfiguration.LayerConfiguration> Underlay { [return: Nullable(new byte[]
		{
			2,
			1
		})] get; [param: Nullable(new byte[]
		{
			2,
			1
		})] set; }

		// Token: 0x17000043 RID: 67
		// (get) Token: 0x06000124 RID: 292 RVA: 0x000043FD File Offset: 0x000025FD
		// (set) Token: 0x06000125 RID: 293 RVA: 0x00004405 File Offset: 0x00002605
		[SimpleJsonPropertyName("extendMetadata")]
		public string ExtendMetadata { get; set; }

		// Token: 0x17000044 RID: 68
		// (get) Token: 0x06000126 RID: 294 RVA: 0x0000440E File Offset: 0x0000260E
		// (set) Token: 0x06000127 RID: 295 RVA: 0x00004416 File Offset: 0x00002616
		[Nullable(new byte[]
		{
			2,
			1
		})]
		[SimpleJsonPropertyName("addAttachment")]
		public ICollection<JobConfiguration.AddDocumentAttachment> AddAttachment { [return: Nullable(new byte[]
		{
			2,
			1
		})] get; [param: Nullable(new byte[]
		{
			2,
			1
		})] set; }

		// Token: 0x17000045 RID: 69
		// (get) Token: 0x06000128 RID: 296 RVA: 0x0000441F File Offset: 0x0000261F
		// (set) Token: 0x06000129 RID: 297 RVA: 0x00004427 File Offset: 0x00002627
		[SimpleJsonPropertyName("encrypt")]
		public JobConfiguration.EncryptionSettings Encrypt { get; set; }

		// Token: 0x17000046 RID: 70
		// (get) Token: 0x0600012A RID: 298 RVA: 0x00004430 File Offset: 0x00002630
		// (set) Token: 0x0600012B RID: 299 RVA: 0x00004438 File Offset: 0x00002638
		[Nullable(1)]
		[SimpleJsonPropertyName("allowWeakCrypto")]
		public string AllowWeakCrypto { [NullableContext(1)] get; [NullableContext(1)] set; } = string.Empty;

		// Token: 0x17000047 RID: 71
		// (get) Token: 0x0600012C RID: 300 RVA: 0x00004441 File Offset: 0x00002641
		// (set) Token: 0x0600012D RID: 301 RVA: 0x00004449 File Offset: 0x00002649
		[SimpleJsonPropertyName("decrypt")]
		public string Decrypt { get; set; }

		// Token: 0x17000048 RID: 72
		// (get) Token: 0x0600012E RID: 302 RVA: 0x00004452 File Offset: 0x00002652
		// (set) Token: 0x0600012F RID: 303 RVA: 0x0000445A File Offset: 0x0000265A
		[SimpleJsonPropertyName("removeRestrictions")]
		public string RemoveRestrictions { get; set; }

		// Token: 0x17000049 RID: 73
		// (get) Token: 0x06000130 RID: 304 RVA: 0x00004463 File Offset: 0x00002663
		// (set) Token: 0x06000131 RID: 305 RVA: 0x0000446B File Offset: 0x0000266B
		[SimpleJsonPropertyName("linearize")]
		public string Linearize { get; set; }

		// Token: 0x1700004A RID: 74
		// (get) Token: 0x06000132 RID: 306 RVA: 0x00004474 File Offset: 0x00002674
		// (set) Token: 0x06000133 RID: 307 RVA: 0x0000447C File Offset: 0x0000267C
		[SimpleJsonPropertyName("newlineBeforeEndstream")]
		public string NewlineBeforeEndstream { get; set; } = string.Empty;

		// Token: 0x1700004B RID: 75
		// (get) Token: 0x06000134 RID: 308 RVA: 0x00004485 File Offset: 0x00002685
		// (set) Token: 0x06000135 RID: 309 RVA: 0x0000448D File Offset: 0x0000268D
		[SimpleJsonPropertyName("keepFilesOpen")]
		public string KeepFilesOpen { get; set; } = "n";

		// Token: 0x02000157 RID: 343
		[NullableContext(1)]
		[Nullable(0)]
		internal sealed class PageConfiguration
		{
			// Token: 0x1700028A RID: 650
			// (get) Token: 0x06000B41 RID: 2881 RVA: 0x0001C386 File Offset: 0x0001A586
			// (set) Token: 0x06000B42 RID: 2882 RVA: 0x0001C38E File Offset: 0x0001A58E
			[SimpleJsonPropertyName("file")]
			public string File { get; set; }

			// Token: 0x1700028B RID: 651
			// (get) Token: 0x06000B43 RID: 2883 RVA: 0x0001C397 File Offset: 0x0001A597
			// (set) Token: 0x06000B44 RID: 2884 RVA: 0x0001C39F File Offset: 0x0001A59F
			[SimpleJsonPropertyName("range")]
			public string Range { get; set; }
		}

		// Token: 0x02000158 RID: 344
		[Nullable(0)]
		internal sealed class LayerConfiguration
		{
			// Token: 0x1700028C RID: 652
			// (get) Token: 0x06000B46 RID: 2886 RVA: 0x0001C3B0 File Offset: 0x0001A5B0
			// (set) Token: 0x06000B47 RID: 2887 RVA: 0x0001C3B8 File Offset: 0x0001A5B8
			[Nullable(1)]
			[SimpleJsonPropertyName("file")]
			public string File { [NullableContext(1)] get; [NullableContext(1)] set; }

			// Token: 0x1700028D RID: 653
			// (get) Token: 0x06000B48 RID: 2888 RVA: 0x0001C3C1 File Offset: 0x0001A5C1
			// (set) Token: 0x06000B49 RID: 2889 RVA: 0x0001C3C9 File Offset: 0x0001A5C9
			[SimpleJsonPropertyName("to")]
			public string To { get; set; }

			// Token: 0x1700028E RID: 654
			// (get) Token: 0x06000B4A RID: 2890 RVA: 0x0001C3D2 File Offset: 0x0001A5D2
			// (set) Token: 0x06000B4B RID: 2891 RVA: 0x0001C3DA File Offset: 0x0001A5DA
			[SimpleJsonPropertyName("from")]
			public string From { get; set; }

			// Token: 0x1700028F RID: 655
			// (get) Token: 0x06000B4C RID: 2892 RVA: 0x0001C3E3 File Offset: 0x0001A5E3
			// (set) Token: 0x06000B4D RID: 2893 RVA: 0x0001C3EB File Offset: 0x0001A5EB
			[SimpleJsonPropertyName("repeat")]
			public string Repeat { get; set; }
		}

		// Token: 0x02000159 RID: 345
		[Nullable(0)]
		public sealed class AddDocumentAttachment
		{
			// Token: 0x17000290 RID: 656
			// (get) Token: 0x06000B4F RID: 2895 RVA: 0x0001C3FC File Offset: 0x0001A5FC
			// (set) Token: 0x06000B50 RID: 2896 RVA: 0x0001C404 File Offset: 0x0001A604
			[Nullable(1)]
			[SimpleJsonPropertyName("key")]
			public string Key { [NullableContext(1)] get; [NullableContext(1)] set; }

			// Token: 0x17000291 RID: 657
			// (get) Token: 0x06000B51 RID: 2897 RVA: 0x0001C40D File Offset: 0x0001A60D
			// (set) Token: 0x06000B52 RID: 2898 RVA: 0x0001C415 File Offset: 0x0001A615
			[Nullable(1)]
			[SimpleJsonPropertyName("file")]
			public string File { [NullableContext(1)] get; [NullableContext(1)] set; }

			// Token: 0x17000292 RID: 658
			// (get) Token: 0x06000B53 RID: 2899 RVA: 0x0001C41E File Offset: 0x0001A61E
			// (set) Token: 0x06000B54 RID: 2900 RVA: 0x0001C426 File Offset: 0x0001A626
			[SimpleJsonPropertyName("filename")]
			public string FileName { get; set; }

			// Token: 0x17000293 RID: 659
			// (get) Token: 0x06000B55 RID: 2901 RVA: 0x0001C42F File Offset: 0x0001A62F
			// (set) Token: 0x06000B56 RID: 2902 RVA: 0x0001C437 File Offset: 0x0001A637
			[SimpleJsonPropertyName("creationdate")]
			public string CreationDate { get; set; }

			// Token: 0x17000294 RID: 660
			// (get) Token: 0x06000B57 RID: 2903 RVA: 0x0001C440 File Offset: 0x0001A640
			// (set) Token: 0x06000B58 RID: 2904 RVA: 0x0001C448 File Offset: 0x0001A648
			[SimpleJsonPropertyName("moddate")]
			public string ModificationDate { get; set; }

			// Token: 0x17000295 RID: 661
			// (get) Token: 0x06000B59 RID: 2905 RVA: 0x0001C451 File Offset: 0x0001A651
			// (set) Token: 0x06000B5A RID: 2906 RVA: 0x0001C459 File Offset: 0x0001A659
			[SimpleJsonPropertyName("mimetype")]
			public string MimeType { get; set; }

			// Token: 0x17000296 RID: 662
			// (get) Token: 0x06000B5B RID: 2907 RVA: 0x0001C462 File Offset: 0x0001A662
			// (set) Token: 0x06000B5C RID: 2908 RVA: 0x0001C46A File Offset: 0x0001A66A
			[SimpleJsonPropertyName("description")]
			public string Description { get; set; }

			// Token: 0x17000297 RID: 663
			// (get) Token: 0x06000B5D RID: 2909 RVA: 0x0001C473 File Offset: 0x0001A673
			// (set) Token: 0x06000B5E RID: 2910 RVA: 0x0001C47B File Offset: 0x0001A67B
			[SimpleJsonPropertyName("replace")]
			public string Replace { get; set; }

			// Token: 0x17000298 RID: 664
			// (get) Token: 0x06000B5F RID: 2911 RVA: 0x0001C484 File Offset: 0x0001A684
			// (set) Token: 0x06000B60 RID: 2912 RVA: 0x0001C48C File Offset: 0x0001A68C
			[SimpleJsonPropertyName("relationship")]
			public string Relationship { get; set; }
		}

		// Token: 0x0200015A RID: 346
		[Nullable(0)]
		public sealed class EncryptionSettings
		{
			// Token: 0x17000299 RID: 665
			// (get) Token: 0x06000B62 RID: 2914 RVA: 0x0001C49D File Offset: 0x0001A69D
			// (set) Token: 0x06000B63 RID: 2915 RVA: 0x0001C4A5 File Offset: 0x0001A6A5
			[SimpleJsonPropertyName("userPassword")]
			public string UserPassword { get; set; }

			// Token: 0x1700029A RID: 666
			// (get) Token: 0x06000B64 RID: 2916 RVA: 0x0001C4AE File Offset: 0x0001A6AE
			// (set) Token: 0x06000B65 RID: 2917 RVA: 0x0001C4B6 File Offset: 0x0001A6B6
			[Nullable(1)]
			[SimpleJsonPropertyName("ownerPassword")]
			public string OwnerPassword { [NullableContext(1)] get; [NullableContext(1)] set; }

			// Token: 0x1700029B RID: 667
			// (get) Token: 0x06000B66 RID: 2918 RVA: 0x0001C4BF File Offset: 0x0001A6BF
			// (set) Token: 0x06000B67 RID: 2919 RVA: 0x0001C4C7 File Offset: 0x0001A6C7
			[SimpleJsonPropertyName("40bit")]
			public JobConfiguration.Encryption40Bit Options40Bit { get; set; }

			// Token: 0x1700029C RID: 668
			// (get) Token: 0x06000B68 RID: 2920 RVA: 0x0001C4D0 File Offset: 0x0001A6D0
			// (set) Token: 0x06000B69 RID: 2921 RVA: 0x0001C4D8 File Offset: 0x0001A6D8
			[SimpleJsonPropertyName("128bit")]
			public JobConfiguration.Encryption128Bit Options128Bit { get; set; }

			// Token: 0x1700029D RID: 669
			// (get) Token: 0x06000B6A RID: 2922 RVA: 0x0001C4E1 File Offset: 0x0001A6E1
			// (set) Token: 0x06000B6B RID: 2923 RVA: 0x0001C4E9 File Offset: 0x0001A6E9
			[SimpleJsonPropertyName("256bit")]
			public JobConfiguration.Encryption256Bit Options256Bit { get; set; }
		}

		// Token: 0x0200015B RID: 347
		[NullableContext(1)]
		[Nullable(0)]
		public sealed class Encryption40Bit
		{
			// Token: 0x1700029E RID: 670
			// (get) Token: 0x06000B6D RID: 2925 RVA: 0x0001C4FA File Offset: 0x0001A6FA
			// (set) Token: 0x06000B6E RID: 2926 RVA: 0x0001C502 File Offset: 0x0001A702
			[SimpleJsonPropertyName("annotate")]
			public string Annotate { get; set; }

			// Token: 0x1700029F RID: 671
			// (get) Token: 0x06000B6F RID: 2927 RVA: 0x0001C50B File Offset: 0x0001A70B
			// (set) Token: 0x06000B70 RID: 2928 RVA: 0x0001C513 File Offset: 0x0001A713
			[SimpleJsonPropertyName("extract")]
			public string Extract { get; set; }

			// Token: 0x170002A0 RID: 672
			// (get) Token: 0x06000B71 RID: 2929 RVA: 0x0001C51C File Offset: 0x0001A71C
			// (set) Token: 0x06000B72 RID: 2930 RVA: 0x0001C524 File Offset: 0x0001A724
			[SimpleJsonPropertyName("modify")]
			public string Modify { get; set; }

			// Token: 0x170002A1 RID: 673
			// (get) Token: 0x06000B73 RID: 2931 RVA: 0x0001C52D File Offset: 0x0001A72D
			// (set) Token: 0x06000B74 RID: 2932 RVA: 0x0001C535 File Offset: 0x0001A735
			[SimpleJsonPropertyName("print")]
			public string Print { get; set; }
		}

		// Token: 0x0200015C RID: 348
		[NullableContext(1)]
		[Nullable(0)]
		public sealed class Encryption128Bit
		{
			// Token: 0x170002A2 RID: 674
			// (get) Token: 0x06000B76 RID: 2934 RVA: 0x0001C546 File Offset: 0x0001A746
			// (set) Token: 0x06000B77 RID: 2935 RVA: 0x0001C54E File Offset: 0x0001A74E
			[SimpleJsonPropertyName("annotate")]
			public string Annotate { get; set; }

			// Token: 0x170002A3 RID: 675
			// (get) Token: 0x06000B78 RID: 2936 RVA: 0x0001C557 File Offset: 0x0001A757
			// (set) Token: 0x06000B79 RID: 2937 RVA: 0x0001C55F File Offset: 0x0001A75F
			[SimpleJsonPropertyName("assemble")]
			public string Assemble { get; set; }

			// Token: 0x170002A4 RID: 676
			// (get) Token: 0x06000B7A RID: 2938 RVA: 0x0001C568 File Offset: 0x0001A768
			// (set) Token: 0x06000B7B RID: 2939 RVA: 0x0001C570 File Offset: 0x0001A770
			[SimpleJsonPropertyName("extract")]
			public string Extract { get; set; }

			// Token: 0x170002A5 RID: 677
			// (get) Token: 0x06000B7C RID: 2940 RVA: 0x0001C579 File Offset: 0x0001A779
			// (set) Token: 0x06000B7D RID: 2941 RVA: 0x0001C581 File Offset: 0x0001A781
			[SimpleJsonPropertyName("form")]
			public string Form { get; set; }

			// Token: 0x170002A6 RID: 678
			// (get) Token: 0x06000B7E RID: 2942 RVA: 0x0001C58A File Offset: 0x0001A78A
			// (set) Token: 0x06000B7F RID: 2943 RVA: 0x0001C592 File Offset: 0x0001A792
			[Nullable(2)]
			[SimpleJsonPropertyName("print")]
			public string Print { [NullableContext(2)] get; [NullableContext(2)] set; }

			// Token: 0x170002A7 RID: 679
			// (get) Token: 0x06000B80 RID: 2944 RVA: 0x0001C59B File Offset: 0x0001A79B
			// (set) Token: 0x06000B81 RID: 2945 RVA: 0x0001C5A3 File Offset: 0x0001A7A3
			[Nullable(2)]
			[SimpleJsonPropertyName("useAes")]
			public string UseAES { [NullableContext(2)] get; [NullableContext(2)] set; } = "y";

			// Token: 0x170002A8 RID: 680
			// (get) Token: 0x06000B82 RID: 2946 RVA: 0x0001C5AC File Offset: 0x0001A7AC
			// (set) Token: 0x06000B83 RID: 2947 RVA: 0x0001C5B4 File Offset: 0x0001A7B4
			[Nullable(2)]
			[SimpleJsonPropertyName("cleartextMetadata")]
			public string CleartextMetadata { [NullableContext(2)] get; [NullableContext(2)] set; }
		}

		// Token: 0x0200015D RID: 349
		[NullableContext(1)]
		[Nullable(0)]
		public sealed class Encryption256Bit
		{
			// Token: 0x170002A9 RID: 681
			// (get) Token: 0x06000B85 RID: 2949 RVA: 0x0001C5D0 File Offset: 0x0001A7D0
			// (set) Token: 0x06000B86 RID: 2950 RVA: 0x0001C5D8 File Offset: 0x0001A7D8
			[SimpleJsonPropertyName("annotate")]
			public string Annotate { get; set; }

			// Token: 0x170002AA RID: 682
			// (get) Token: 0x06000B87 RID: 2951 RVA: 0x0001C5E1 File Offset: 0x0001A7E1
			// (set) Token: 0x06000B88 RID: 2952 RVA: 0x0001C5E9 File Offset: 0x0001A7E9
			[SimpleJsonPropertyName("assemble")]
			public string Assemble { get; set; }

			// Token: 0x170002AB RID: 683
			// (get) Token: 0x06000B89 RID: 2953 RVA: 0x0001C5F2 File Offset: 0x0001A7F2
			// (set) Token: 0x06000B8A RID: 2954 RVA: 0x0001C5FA File Offset: 0x0001A7FA
			[SimpleJsonPropertyName("extract")]
			public string Extract { get; set; }

			// Token: 0x170002AC RID: 684
			// (get) Token: 0x06000B8B RID: 2955 RVA: 0x0001C603 File Offset: 0x0001A803
			// (set) Token: 0x06000B8C RID: 2956 RVA: 0x0001C60B File Offset: 0x0001A80B
			[SimpleJsonPropertyName("form")]
			public string Form { get; set; }

			// Token: 0x170002AD RID: 685
			// (get) Token: 0x06000B8D RID: 2957 RVA: 0x0001C614 File Offset: 0x0001A814
			// (set) Token: 0x06000B8E RID: 2958 RVA: 0x0001C61C File Offset: 0x0001A81C
			[Nullable(2)]
			[SimpleJsonPropertyName("print")]
			public string Print { [NullableContext(2)] get; [NullableContext(2)] set; }

			// Token: 0x170002AE RID: 686
			// (get) Token: 0x06000B8F RID: 2959 RVA: 0x0001C625 File Offset: 0x0001A825
			// (set) Token: 0x06000B90 RID: 2960 RVA: 0x0001C62D File Offset: 0x0001A82D
			[Nullable(2)]
			[SimpleJsonPropertyName("cleartextMetadata")]
			public string CleartextMetadata { [NullableContext(2)] get; [NullableContext(2)] set; }
		}
	}
}
