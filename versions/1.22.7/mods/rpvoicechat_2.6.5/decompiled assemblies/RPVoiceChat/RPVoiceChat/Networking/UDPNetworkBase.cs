using System;
using System.Net;
using System.Net.Sockets;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using Open.Nat;
using RPVoiceChat.Util;

namespace RPVoiceChat.Networking
{
	// Token: 0x02000078 RID: 120
	public abstract class UDPNetworkBase : IDisposable
	{
		// Token: 0x14000010 RID: 16
		// (add) Token: 0x0600031E RID: 798 RVA: 0x000124DC File Offset: 0x000106DC
		// (remove) Token: 0x0600031F RID: 799 RVA: 0x00012514 File Offset: 0x00010714
		protected event Action<byte[], IPEndPoint> OnMessageReceived;

		// Token: 0x06000320 RID: 800 RVA: 0x00012549 File Offset: 0x00010749
		public UDPNetworkBase(Logger logger, bool forwardPorts)
		{
			this.logger = logger;
			this.upnpEnabled = forwardPorts;
			this._readinessProbeCTS = new CancellationTokenSource();
		}

		// Token: 0x06000321 RID: 801 RVA: 0x0001256A File Offset: 0x0001076A
		public string GetTransportID()
		{
			return "UDP";
		}

		// Token: 0x06000322 RID: 802 RVA: 0x00012574 File Offset: 0x00010774
		protected void SetupUpnp(int port)
		{
			if (!this.upnpEnabled)
			{
				return;
			}
			try
			{
				this.logger.VerboseDebug("Attempting to portforward with UPnP");
				NatDiscoverer discoverer = new NatDiscoverer();
				CancellationTokenSource cts = new CancellationTokenSource(5000);
				NatDevice result = Task.Run<NatDevice>(() => discoverer.DiscoverDeviceAsync(2, cts)).GetAwaiter().GetResult();
				if (result == null)
				{
					throw new NatDeviceNotFoundException();
				}
				this.logger.VerboseDebug("Found a UPnP device, creating port map");
				result.CreatePortMapAsync(new Mapping(1, port, port, "Vintage Story Voice Chat"));
			}
			catch (TaskCanceledException)
			{
				throw new NoStackTraceException("Device discovery got aborted");
			}
			catch (NatDeviceNotFoundException)
			{
				throw new NoStackTraceException("Unable to port forward with UPnP, UDP connection may not be available. Make sure your IP is public and UPnP is enabled if you want to use UDP transport.");
			}
		}

		// Token: 0x06000323 RID: 803 RVA: 0x0001263C File Offset: 0x0001083C
		protected void OpenUDPClient(int port)
		{
			this.UdpClient = new UdpClient(port);
			this.UdpClient.Client.ReceiveBufferSize = (this.UdpClient.Client.SendBufferSize = 65536);
		}

		// Token: 0x06000324 RID: 804 RVA: 0x00012680 File Offset: 0x00010880
		protected int OpenUDPClient()
		{
			int port = 0;
			this.UdpClient = new UdpClient();
			IPEndPoint endpoint = new IPEndPoint(IPAddress.Any, port);
			this.UdpClient.Client.Bind(endpoint);
			this.UdpClient.Client.ReceiveBufferSize = (this.UdpClient.Client.SendBufferSize = 65536);
			return (this.UdpClient.Client.LocalEndPoint as IPEndPoint).Port;
		}

		// Token: 0x06000325 RID: 805 RVA: 0x000126FC File Offset: 0x000108FC
		protected void StartListening()
		{
			if (this.UdpClient == null)
			{
				throw new Exception("Udp client has not been initialized. Can't start listening.");
			}
			this._listeningCTS = new CancellationTokenSource();
			this._listeningThread = new Thread(new ParameterizedThreadStart(this.Listen));
			this._listeningThread.Start(this._listeningCTS.Token);
		}

		// Token: 0x06000326 RID: 806 RVA: 0x0001275C File Offset: 0x0001095C
		private void Listen(object cancellationToken)
		{
			CancellationToken ct = (CancellationToken)cancellationToken;
			while (this._listeningThread.IsAlive && !ct.IsCancellationRequested)
			{
				IPEndPoint sender = null;
				try
				{
					byte[] msg = this.UdpClient.Receive(ref sender);
					if (msg.Length >= 4)
					{
						Action<byte[], IPEndPoint> onMessageReceived = this.OnMessageReceived;
						if (onMessageReceived != null)
						{
							onMessageReceived.Invoke(msg, sender);
						}
					}
				}
				catch (Exception e)
				{
					SocketException se = e as SocketException;
					if (se == null || se.SocketErrorCode != 10054)
					{
						if ((se != null && se.SocketErrorCode == 10004) || ct.IsCancellationRequested)
						{
							break;
						}
						Logger logger = this.logger;
						DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
						defaultInterpolatedStringHandler..ctor(36, 2);
						defaultInterpolatedStringHandler.AppendLiteral("Datagram from ");
						defaultInterpolatedStringHandler.AppendFormatted<IPAddress>((sender != null) ? sender.Address : null);
						defaultInterpolatedStringHandler.AppendLiteral(" caused an exception:\n");
						defaultInterpolatedStringHandler.AppendFormatted<Exception>(e);
						logger.Error(defaultInterpolatedStringHandler.ToStringAndClear());
					}
				}
			}
		}

		// Token: 0x06000327 RID: 807 RVA: 0x0001285C File Offset: 0x00010A5C
		public void Dispose()
		{
			CancellationTokenSource readinessProbeCTS = this._readinessProbeCTS;
			if (readinessProbeCTS != null)
			{
				readinessProbeCTS.Cancel();
			}
			CancellationTokenSource readinessProbeCTS2 = this._readinessProbeCTS;
			if (readinessProbeCTS2 != null)
			{
				readinessProbeCTS2.Dispose();
			}
			CancellationTokenSource listeningCTS = this._listeningCTS;
			if (listeningCTS != null)
			{
				listeningCTS.Cancel();
			}
			CancellationTokenSource listeningCTS2 = this._listeningCTS;
			if (listeningCTS2 != null)
			{
				listeningCTS2.Dispose();
			}
			UdpClient udpClient = this.UdpClient;
			if (udpClient != null)
			{
				udpClient.Close();
			}
			UdpClient udpClient2 = this.UdpClient;
			if (udpClient2 == null)
			{
				return;
			}
			udpClient2.Dispose();
		}

		// Token: 0x0400016F RID: 367
		private Thread _listeningThread;

		// Token: 0x04000170 RID: 368
		private CancellationTokenSource _listeningCTS;

		// Token: 0x04000171 RID: 369
		protected UdpClient UdpClient;

		// Token: 0x04000172 RID: 370
		protected int port;

		// Token: 0x04000173 RID: 371
		protected const string _transportID = "UDP";

		// Token: 0x04000174 RID: 372
		protected bool upnpEnabled;

		// Token: 0x04000175 RID: 373
		protected Logger logger;

		// Token: 0x04000176 RID: 374
		protected CancellationTokenSource _readinessProbeCTS;

		// Token: 0x04000177 RID: 375
		protected bool isReady;
	}
}
