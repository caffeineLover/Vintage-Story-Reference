using System;
using System.Runtime.InteropServices;
using Windows.Foundation.Metadata;
using WinRT;

namespace Windows.Foundation
{
	// Token: 0x0200004A RID: 74
	[WindowsRuntimeType("Windows.Foundation.UniversalApiContract")]
	[Guid("9E365E57-48B2-4160-956F-C7385120BBFC")]
	[ContractVersion(typeof(UniversalApiContract), 65536U)]
	internal interface IUriRuntimeClass
	{
		// Token: 0x0600036F RID: 879
		bool Equals(Uri pUri);

		// Token: 0x06000370 RID: 880
		Uri CombineUri(string relativeUri);

		// Token: 0x170001B7 RID: 439
		// (get) Token: 0x06000371 RID: 881
		string AbsoluteUri { get; }

		// Token: 0x170001B8 RID: 440
		// (get) Token: 0x06000372 RID: 882
		string DisplayUri { get; }

		// Token: 0x170001B9 RID: 441
		// (get) Token: 0x06000373 RID: 883
		string Domain { get; }

		// Token: 0x170001BA RID: 442
		// (get) Token: 0x06000374 RID: 884
		string Extension { get; }

		// Token: 0x170001BB RID: 443
		// (get) Token: 0x06000375 RID: 885
		string Fragment { get; }

		// Token: 0x170001BC RID: 444
		// (get) Token: 0x06000376 RID: 886
		string Host { get; }

		// Token: 0x170001BD RID: 445
		// (get) Token: 0x06000377 RID: 887
		string Password { get; }

		// Token: 0x170001BE RID: 446
		// (get) Token: 0x06000378 RID: 888
		string Path { get; }

		// Token: 0x170001BF RID: 447
		// (get) Token: 0x06000379 RID: 889
		int Port { get; }

		// Token: 0x170001C0 RID: 448
		// (get) Token: 0x0600037A RID: 890
		string Query { get; }

		// Token: 0x170001C1 RID: 449
		// (get) Token: 0x0600037B RID: 891
		WwwFormUrlDecoder QueryParsed { get; }

		// Token: 0x170001C2 RID: 450
		// (get) Token: 0x0600037C RID: 892
		string RawUri { get; }

		// Token: 0x170001C3 RID: 451
		// (get) Token: 0x0600037D RID: 893
		string SchemeName { get; }

		// Token: 0x170001C4 RID: 452
		// (get) Token: 0x0600037E RID: 894
		bool Suspicious { get; }

		// Token: 0x170001C5 RID: 453
		// (get) Token: 0x0600037F RID: 895
		string UserName { get; }
	}
}
