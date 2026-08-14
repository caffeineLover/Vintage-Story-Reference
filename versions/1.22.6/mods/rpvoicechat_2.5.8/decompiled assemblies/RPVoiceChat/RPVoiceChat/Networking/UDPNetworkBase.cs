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
	// Token: 0x0200005B RID: 91
	public abstract class UDPNetworkBase : IDisposable
	{
		// Token: 0x14000010 RID: 16
		// (add) Token: 0x06000283 RID: 643 RVA: 0x0000EA10 File Offset: 0x0000CC10
		// (remove) Token: 0x06000284 RID: 644 RVA: 0x0000EA48 File Offset: 0x0000CC48
		protected event Action<byte[], IPEndPoint> OnMessageReceived;

		// Token: 0x06000285 RID: 645 RVA: 0x0000EA7D File Offset: 0x0000CC7D
		public UDPNetworkBase(Logger logger, bool forwardPorts)
		{
			this.logger = logger;
			this.upnpEnabled = forwardPorts;
			this._readinessProbeCTS = new CancellationTokenSource();
		}

		// Token: 0x06000286 RID: 646 RVA: 0x0000EA9E File Offset: 0x0000CC9E
		public string GetTransportID()
		{
			return "UDP";
		}

		// Token: 0x06000287 RID: 647 RVA: 0x0000EAA8 File Offset: 0x0000CCA8
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

		// Token: 0x06000288 RID: 648 RVA: 0x0000EB70 File Offset: 0x0000CD70
		protected void OpenUDPClient(int port)
		{
			this.UdpClient = new UdpClient(port);
			this.UdpClient.Client.ReceiveBufferSize = (this.UdpClient.Client.SendBufferSize = 65536);
		}

		// Token: 0x06000289 RID: 649 RVA: 0x0000EBB4 File Offset: 0x0000CDB4
		protected int OpenUDPClient()
		{
			int port = 0;
			this.UdpClient = new UdpClient();
			IPEndPoint endpoint = new IPEndPoint(IPAddress.Any, port);
			this.UdpClient.Client.Bind(endpoint);
			this.UdpClient.Client.ReceiveBufferSize = (this.UdpClient.Client.SendBufferSize = 65536);
			return (this.UdpClient.Client.LocalEndPoint as IPEndPoint).Port;
		}

		// Token: 0x0600028A RID: 650 RVA: 0x0000EC30 File Offset: 0x0000CE30
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

		// Token: 0x0600028B RID: 651 RVA: 0x0000EC90 File Offset: 0x0000CE90
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

		// Token: 0x0600028C RID: 652 RVA: 0x0000ED90 File Offset: 0x0000CF90
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

		// Token: 0x0400011D RID: 285
		private Thread _listeningThread;

		// Token: 0x0400011E RID: 286
		private CancellationTokenSource _listeningCTS;

		// Token: 0x0400011F RID: 287
		protected UdpClient UdpClient;

		// Token: 0x04000120 RID: 288
		protected int port;

		// Token: 0x04000121 RID: 289
		protected const string _transportID = "UDP";

		// Token: 0x04000122 RID: 290
		protected bool upnpEnabled;

		// Token: 0x04000123 RID: 291
		protected Logger logger;

		// Token: 0x04000124 RID: 292
		protected CancellationTokenSource _readinessProbeCTS;

		// Token: 0x04000125 RID: 293
		protected bool isReady;
	}
}
