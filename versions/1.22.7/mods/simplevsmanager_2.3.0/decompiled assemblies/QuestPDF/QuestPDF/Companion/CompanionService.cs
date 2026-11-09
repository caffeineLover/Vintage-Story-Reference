using System;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;
using QuestPDF.Drawing.DocumentCanvases;

namespace QuestPDF.Companion
{
	// Token: 0x0200012F RID: 303
	[NullableContext(1)]
	[Nullable(0)]
	internal sealed class CompanionService
	{
		// Token: 0x17000285 RID: 645
		// (get) Token: 0x06000AB0 RID: 2736 RVA: 0x0001B9EF File Offset: 0x00019BEF
		private int Port { get; }

		// Token: 0x17000286 RID: 646
		// (get) Token: 0x06000AB1 RID: 2737 RVA: 0x0001B9F7 File Offset: 0x00019BF7
		private HttpClient HttpClient { get; }

		// Token: 0x14000001 RID: 1
		// (add) Token: 0x06000AB2 RID: 2738 RVA: 0x0001BA00 File Offset: 0x00019C00
		// (remove) Token: 0x06000AB3 RID: 2739 RVA: 0x0001BA38 File Offset: 0x00019C38
		[Nullable(2)]
		[method: NullableContext(2)]
		[Nullable(2)]
		public event Action OnCompanionStopped;

		// Token: 0x17000287 RID: 647
		// (get) Token: 0x06000AB4 RID: 2740 RVA: 0x0001BA6D File Offset: 0x00019C6D
		// (set) Token: 0x06000AB5 RID: 2741 RVA: 0x0001BA74 File Offset: 0x00019C74
		[Nullable(2)]
		private static CompanionDocumentSnapshot CurrentDocumentSnapshot { [NullableContext(2)] get; [NullableContext(2)] set; }

		// Token: 0x17000288 RID: 648
		// (get) Token: 0x06000AB6 RID: 2742 RVA: 0x0001BA7C File Offset: 0x00019C7C
		// (set) Token: 0x06000AB7 RID: 2743 RVA: 0x0001BA83 File Offset: 0x00019C83
		public static bool IsCompanionAttached { get; private set; }

		// Token: 0x17000289 RID: 649
		// (get) Token: 0x06000AB8 RID: 2744 RVA: 0x0001BA8B File Offset: 0x00019C8B
		// (set) Token: 0x06000AB9 RID: 2745 RVA: 0x0001BA92 File Offset: 0x00019C92
		public static bool IsDocumentHotReloaded { get; set; }

		// Token: 0x06000ABA RID: 2746 RVA: 0x0001BA9C File Offset: 0x00019C9C
		public CompanionService(int port)
		{
			JsonSerializerOptions jsonSerializerOptions = new JsonSerializerOptions();
			jsonSerializerOptions.MaxDepth = 256;
			jsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
			jsonSerializerOptions.Converters.Add(new JsonStringEnumConverter(JsonNamingPolicy.CamelCase, true));
			this.JsonSerializerOptions = jsonSerializerOptions;
			base..ctor();
			CompanionService.IsCompanionAttached = true;
			this.Port = port;
			HttpClient httpClient = new HttpClient();
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
			defaultInterpolatedStringHandler..ctor(18, 1);
			defaultInterpolatedStringHandler.AppendLiteral("http://localhost:");
			defaultInterpolatedStringHandler.AppendFormatted<int>(port);
			defaultInterpolatedStringHandler.AppendLiteral("/");
			httpClient.BaseAddress = new Uri(defaultInterpolatedStringHandler.ToStringAndClear());
			httpClient.Timeout = TimeSpan.FromSeconds(5.0);
			this.HttpClient = httpClient;
		}

		// Token: 0x06000ABB RID: 2747 RVA: 0x0001BB50 File Offset: 0x00019D50
		public Task Connect()
		{
			CompanionService.<Connect>d__24 <Connect>d__;
			<Connect>d__.<>t__builder = AsyncTaskMethodBuilder.Create();
			<Connect>d__.<>4__this = this;
			<Connect>d__.<>1__state = -1;
			<Connect>d__.<>t__builder.Start<CompanionService.<Connect>d__24>(ref <Connect>d__);
			return <Connect>d__.<>t__builder.Task;
		}

		// Token: 0x06000ABC RID: 2748 RVA: 0x0001BB94 File Offset: 0x00019D94
		private Task CheckIfCompanionIsRunning()
		{
			CompanionService.<CheckIfCompanionIsRunning>d__25 <CheckIfCompanionIsRunning>d__;
			<CheckIfCompanionIsRunning>d__.<>t__builder = AsyncTaskMethodBuilder.Create();
			<CheckIfCompanionIsRunning>d__.<>4__this = this;
			<CheckIfCompanionIsRunning>d__.<>1__state = -1;
			<CheckIfCompanionIsRunning>d__.<>t__builder.Start<CompanionService.<CheckIfCompanionIsRunning>d__25>(ref <CheckIfCompanionIsRunning>d__);
			return <CheckIfCompanionIsRunning>d__.<>t__builder.Task;
		}

		// Token: 0x06000ABD RID: 2749 RVA: 0x0001BBD8 File Offset: 0x00019DD8
		internal Task StartNotifyPresenceTask()
		{
			CompanionService.<StartNotifyPresenceTask>d__26 <StartNotifyPresenceTask>d__;
			<StartNotifyPresenceTask>d__.<>t__builder = AsyncTaskMethodBuilder.Create();
			<StartNotifyPresenceTask>d__.<>4__this = this;
			<StartNotifyPresenceTask>d__.<>1__state = -1;
			<StartNotifyPresenceTask>d__.<>t__builder.Start<CompanionService.<StartNotifyPresenceTask>d__26>(ref <StartNotifyPresenceTask>d__);
			return <StartNotifyPresenceTask>d__.<>t__builder.Task;
		}

		// Token: 0x06000ABE RID: 2750 RVA: 0x0001BC1C File Offset: 0x00019E1C
		private Task CheckCompanionVersionCompatibility()
		{
			CompanionService.<CheckCompanionVersionCompatibility>d__27 <CheckCompanionVersionCompatibility>d__;
			<CheckCompanionVersionCompatibility>d__.<>t__builder = AsyncTaskMethodBuilder.Create();
			<CheckCompanionVersionCompatibility>d__.<>4__this = this;
			<CheckCompanionVersionCompatibility>d__.<>1__state = -1;
			<CheckCompanionVersionCompatibility>d__.<>t__builder.Start<CompanionService.<CheckCompanionVersionCompatibility>d__27>(ref <CheckCompanionVersionCompatibility>d__);
			return <CheckCompanionVersionCompatibility>d__.<>t__builder.Task;
		}

		// Token: 0x06000ABF RID: 2751 RVA: 0x0001BC60 File Offset: 0x00019E60
		public Task RefreshPreview(CompanionDocumentSnapshot companionDocumentSnapshot)
		{
			CompanionService.<RefreshPreview>d__28 <RefreshPreview>d__;
			<RefreshPreview>d__.<>t__builder = AsyncTaskMethodBuilder.Create();
			<RefreshPreview>d__.<>4__this = this;
			<RefreshPreview>d__.companionDocumentSnapshot = companionDocumentSnapshot;
			<RefreshPreview>d__.<>1__state = -1;
			<RefreshPreview>d__.<>t__builder.Start<CompanionService.<RefreshPreview>d__28>(ref <RefreshPreview>d__);
			return <RefreshPreview>d__.<>t__builder.Task;
		}

		// Token: 0x06000AC0 RID: 2752 RVA: 0x0001BCAB File Offset: 0x00019EAB
		public void StartRenderRequestedPageSnapshotsTask(CancellationToken cancellationToken)
		{
			CompanionService.<>c__DisplayClass29_0 CS$<>8__locals1 = new CompanionService.<>c__DisplayClass29_0();
			CS$<>8__locals1.<>4__this = this;
			CS$<>8__locals1.cancellationToken = cancellationToken;
			Task.Run(delegate()
			{
				CompanionService.<>c__DisplayClass29_0.<<StartRenderRequestedPageSnapshotsTask>b__0>d <<StartRenderRequestedPageSnapshotsTask>b__0>d;
				<<StartRenderRequestedPageSnapshotsTask>b__0>d.<>t__builder = AsyncTaskMethodBuilder.Create();
				<<StartRenderRequestedPageSnapshotsTask>b__0>d.<>4__this = CS$<>8__locals1;
				<<StartRenderRequestedPageSnapshotsTask>b__0>d.<>1__state = -1;
				<<StartRenderRequestedPageSnapshotsTask>b__0>d.<>t__builder.Start<CompanionService.<>c__DisplayClass29_0.<<StartRenderRequestedPageSnapshotsTask>b__0>d>(ref <<StartRenderRequestedPageSnapshotsTask>b__0>d);
				return <<StartRenderRequestedPageSnapshotsTask>b__0>d.<>t__builder.Task;
			});
		}

		// Token: 0x06000AC1 RID: 2753 RVA: 0x0001BCD4 File Offset: 0x00019ED4
		private Task RenderRequestedPageSnapshots()
		{
			CompanionService.<RenderRequestedPageSnapshots>d__30 <RenderRequestedPageSnapshots>d__;
			<RenderRequestedPageSnapshots>d__.<>t__builder = AsyncTaskMethodBuilder.Create();
			<RenderRequestedPageSnapshots>d__.<>4__this = this;
			<RenderRequestedPageSnapshots>d__.<>1__state = -1;
			<RenderRequestedPageSnapshots>d__.<>t__builder.Start<CompanionService.<RenderRequestedPageSnapshots>d__30>(ref <RenderRequestedPageSnapshots>d__);
			return <RenderRequestedPageSnapshots>d__.<>t__builder.Task;
		}

		// Token: 0x06000AC2 RID: 2754 RVA: 0x0001BD18 File Offset: 0x00019F18
		internal Task InformAboutGenericException(Exception exception)
		{
			CompanionService.<InformAboutGenericException>d__31 <InformAboutGenericException>d__;
			<InformAboutGenericException>d__.<>t__builder = AsyncTaskMethodBuilder.Create();
			<InformAboutGenericException>d__.<>4__this = this;
			<InformAboutGenericException>d__.exception = exception;
			<InformAboutGenericException>d__.<>1__state = -1;
			<InformAboutGenericException>d__.<>t__builder.Start<CompanionService.<InformAboutGenericException>d__31>(ref <InformAboutGenericException>d__);
			return <InformAboutGenericException>d__.<>t__builder.Task;
		}

		// Token: 0x06000AC3 RID: 2755 RVA: 0x0001BD64 File Offset: 0x00019F64
		[CompilerGenerated]
		internal static CompanionCommands.ShowGenericException.GenericExceptionDetails <InformAboutGenericException>g__Map|31_0(Exception exception)
		{
			return new CompanionCommands.ShowGenericException.GenericExceptionDetails
			{
				Type = (exception.GetType().FullName ?? "Unknown"),
				Message = exception.Message,
				StackTrace = exception.StackTrace.ParseStackTrace(),
				InnerException = ((exception.InnerException == null) ? null : CompanionService.<InformAboutGenericException>g__Map|31_0(exception.InnerException))
			};
		}

		// Token: 0x04000425 RID: 1061
		private const int RequiredCompanionApiVersion = 2;

		// Token: 0x04000429 RID: 1065
		private JsonSerializerOptions JsonSerializerOptions;
	}
}
