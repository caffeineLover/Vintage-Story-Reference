using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using RPVoiceChat.Util;

namespace RPVoiceChat.Networking
{
	// Token: 0x02000074 RID: 116
	public class TCPConnection : IDisposable
	{
		// Token: 0x1400000B RID: 11
		// (add) Token: 0x060002E9 RID: 745 RVA: 0x00011010 File Offset: 0x0000F210
		// (remove) Token: 0x060002EA RID: 746 RVA: 0x00011048 File Offset: 0x0000F248
		public event Action<byte[], TCPConnection> OnMessageReceived;

		// Token: 0x1400000C RID: 12
		// (add) Token: 0x060002EB RID: 747 RVA: 0x00011080 File Offset: 0x0000F280
		// (remove) Token: 0x060002EC RID: 748 RVA: 0x000110B8 File Offset: 0x0000F2B8
		public event Action<bool, bool, TCPConnection> OnDisconnected;

		// Token: 0x060002ED RID: 749 RVA: 0x000110F0 File Offset: 0x0000F2F0
		public TCPConnection(Logger logger, Socket existingSocket = null)
		{
			this.logger = logger;
			this.socket = existingSocket;
			if (this.socket == null)
			{
				this.socket = new Socket(2, 1, 6);
			}
			this.socket.ReceiveBufferSize = 409600;
			this.socket.NoDelay = true;
			this.remoteEndpoint = (this.socket.RemoteEndPoint as IPEndPoint);
			IPEndPoint ipendPoint = this.socket.LocalEndPoint as IPEndPoint;
			this.port = ((ipendPoint != null) ? ipendPoint.Port : 0);
			this.receiveBuffer = new byte[this.socket.ReceiveBufferSize];
		}

		// Token: 0x060002EE RID: 750 RVA: 0x000111A1 File Offset: 0x0000F3A1
		public void Connect(IPEndPoint endPoint)
		{
			this.socket.Connect(endPoint);
			this.remoteEndpoint = (IPEndPoint)this.socket.RemoteEndPoint;
			this.port = ((IPEndPoint)this.socket.LocalEndPoint).Port;
		}

		// Token: 0x060002EF RID: 751 RVA: 0x000111E0 File Offset: 0x0000F3E0
		public Task ConnectAsync(IPEndPoint endPoint)
		{
			return this.socket.ConnectAsync(endPoint).ContinueWith(delegate(Task _)
			{
				this.remoteEndpoint = (IPEndPoint)this.socket.RemoteEndPoint;
				this.port = ((IPEndPoint)this.socket.LocalEndPoint).Port;
			}, 393216);
		}

		// Token: 0x060002F0 RID: 752 RVA: 0x00011204 File Offset: 0x0000F404
		public void Send(byte[] data)
		{
			if (this.socket == null)
			{
				throw new Exception("Socket already disposed.");
			}
			byte[] tcpMessage = this.PackMessage(data);
			try
			{
				this.socket.Send(tcpMessage);
			}
			catch (Exception e)
			{
				SocketException se = e as SocketException;
				if ((se == null || (se.SocketErrorCode != 10057 && se.SocketErrorCode != 995 && se.SocketErrorCode != 10004)) && !this._listeningCTS.IsCancellationRequested)
				{
					Logger logger = this.logger;
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
					defaultInterpolatedStringHandler..ctor(37, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Failed to send TCP packet to server:\n");
					defaultInterpolatedStringHandler.AppendFormatted<Exception>(e);
					logger.Error(defaultInterpolatedStringHandler.ToStringAndClear());
					this.Disconnect(false, false);
				}
			}
		}

		// Token: 0x060002F1 RID: 753 RVA: 0x000112C8 File Offset: 0x0000F4C8
		public ValueTask<int> SendAsync(byte[] data, CancellationToken ct = default(CancellationToken))
		{
			if (this.socket == null)
			{
				throw new Exception("Socket already disposed.");
			}
			byte[] tcpMessage = this.PackMessage(data);
			return this.socket.SendAsync(tcpMessage, ct);
		}

		// Token: 0x060002F2 RID: 754 RVA: 0x00011302 File Offset: 0x0000F502
		public void StartListening()
		{
			if (this.socket == null)
			{
				throw new Exception("Socket already disposed.");
			}
			this._listeningCTS = new CancellationTokenSource();
			this.Listen(this._listeningCTS.Token);
		}

		// Token: 0x060002F3 RID: 755 RVA: 0x00011334 File Offset: 0x0000F534
		private void Listen(CancellationToken ct)
		{
			TCPConnection.<Listen>d__21 <Listen>d__;
			<Listen>d__.<>t__builder = AsyncVoidMethodBuilder.Create();
			<Listen>d__.<>4__this = this;
			<Listen>d__.ct = ct;
			<Listen>d__.<>1__state = -1;
			<Listen>d__.<>t__builder.Start<TCPConnection.<Listen>d__21>(ref <Listen>d__);
		}

		// Token: 0x060002F4 RID: 756 RVA: 0x00011374 File Offset: 0x0000F574
		private byte[] PackMessage(byte[] data)
		{
			int num = data.Length;
			byte[] prefix = BitConverter.GetBytes(num);
			byte[] tcpMessage = new byte[num + 4];
			Buffer.BlockCopy(prefix, 0, tcpMessage, 0, 4);
			Buffer.BlockCopy(data, 0, tcpMessage, 4, data.Length);
			return tcpMessage;
		}

		// Token: 0x060002F5 RID: 757 RVA: 0x000113AC File Offset: 0x0000F5AC
		private List<byte[]> ParseMessages(byte[] data, int length)
		{
			List<byte[]> messages = new List<byte[]>();
			MemoryStream stream = new MemoryStream(length + this.fragmentedBytes);
			stream.Write(this.fragmentBuffer, 0, this.fragmentedBytes);
			stream.Write(data, 0, length);
			this.fragmentedBytes = 0;
			stream.Position = 0L;
			List<byte[]> result;
			using (BinaryReader reader = new BinaryReader(stream))
			{
				try
				{
					long bytesLeft;
					for (bytesLeft = stream.Length; bytesLeft > 4L; bytesLeft = stream.Length - stream.Position)
					{
						int messageLength = reader.ReadInt32();
						if ((long)messageLength > bytesLeft - 4L || messageLength < 1)
						{
							break;
						}
						byte[] message = reader.ReadBytes(messageLength);
						messages.Add(message);
					}
					if (bytesLeft != 0L)
					{
						byte[] remainingData = stream.ToArray();
						Buffer.BlockCopy(remainingData, (int)((long)remainingData.Length - bytesLeft), this.fragmentBuffer, this.fragmentedBytes, (int)bytesLeft);
						this.fragmentedBytes += (int)bytesLeft;
						if (this.fragmentedBytes > 30720)
						{
							Logger logger = this.logger;
							DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(97, 5);
							defaultInterpolatedStringHandler.AppendLiteral("Buffer of packet fragments from ");
							defaultInterpolatedStringHandler.AppendFormatted<IPEndPoint>(this.remoteEndpoint);
							defaultInterpolatedStringHandler.AppendLiteral(" is abnormally full. (Message queue at ");
							defaultInterpolatedStringHandler.AppendFormatted<int>(length);
							defaultInterpolatedStringHandler.AppendLiteral("/");
							defaultInterpolatedStringHandler.AppendFormatted<int>(this.receiveBuffer.Length);
							defaultInterpolatedStringHandler.AppendLiteral(", fragmented buffer at ");
							defaultInterpolatedStringHandler.AppendFormatted<int>(this.fragmentedBytes);
							defaultInterpolatedStringHandler.AppendLiteral("/");
							defaultInterpolatedStringHandler.AppendFormatted<int>(this.fragmentBuffer.Length);
							defaultInterpolatedStringHandler.AppendLiteral(")");
							logger.Warning(defaultInterpolatedStringHandler.ToStringAndClear());
						}
						if (this.fragmentedBytes > 65536)
						{
							Logger logger2 = this.logger;
							DefaultInterpolatedStringHandler defaultInterpolatedStringHandler2 = new DefaultInterpolatedStringHandler(121, 5);
							defaultInterpolatedStringHandler2.AppendLiteral("Buffer of packet fragments from ");
							defaultInterpolatedStringHandler2.AppendFormatted<IPEndPoint>(this.remoteEndpoint);
							defaultInterpolatedStringHandler2.AppendLiteral(" exceeded impossible value, discarding data. (Message queue at ");
							defaultInterpolatedStringHandler2.AppendFormatted<int>(length);
							defaultInterpolatedStringHandler2.AppendLiteral("/");
							defaultInterpolatedStringHandler2.AppendFormatted<int>(this.receiveBuffer.Length);
							defaultInterpolatedStringHandler2.AppendLiteral(", fragmented buffer at ");
							defaultInterpolatedStringHandler2.AppendFormatted<int>(this.fragmentedBytes);
							defaultInterpolatedStringHandler2.AppendLiteral("/");
							defaultInterpolatedStringHandler2.AppendFormatted<int>(this.fragmentBuffer.Length);
							defaultInterpolatedStringHandler2.AppendLiteral(")");
							logger2.Error(defaultInterpolatedStringHandler2.ToStringAndClear());
							this.ClearNetworkStream();
						}
					}
				}
				catch (Exception e)
				{
					Logger logger3 = this.logger;
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler3 = new DefaultInterpolatedStringHandler(29, 1);
					defaultInterpolatedStringHandler3.AppendLiteral("Couldn't parse TCP messages:\n");
					defaultInterpolatedStringHandler3.AppendFormatted<Exception>(e);
					logger3.Error(defaultInterpolatedStringHandler3.ToStringAndClear());
					this.ClearNetworkStream();
				}
				result = messages;
			}
			return result;
		}

		// Token: 0x060002F6 RID: 758 RVA: 0x00011670 File Offset: 0x0000F870
		private void ClearNetworkStream()
		{
			Logger logger = this.logger;
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
			defaultInterpolatedStringHandler..ctor(30, 1);
			defaultInterpolatedStringHandler.AppendLiteral("Unclogging message buffer for ");
			defaultInterpolatedStringHandler.AppendFormatted<IPEndPoint>(this.remoteEndpoint);
			logger.Debug(defaultInterpolatedStringHandler.ToStringAndClear());
			int bytesRead;
			do
			{
				bytesRead = this.socket.Receive(this.receiveBuffer);
			}
			while (bytesRead == this.receiveBuffer.Length);
			this.fragmentedBytes = 0;
		}

		// Token: 0x060002F7 RID: 759 RVA: 0x000116D8 File Offset: 0x0000F8D8
		public void Disconnect(bool isGraceful = true, bool isHalfClosed = false)
		{
			try
			{
				Socket socket = this.socket;
				if (socket != null)
				{
					socket.Shutdown(2);
				}
				Socket socket2 = this.socket;
				if (socket2 != null)
				{
					socket2.Disconnect(true);
				}
			}
			catch
			{
			}
			Action<bool, bool, TCPConnection> onDisconnected = this.OnDisconnected;
			if (onDisconnected == null)
			{
				return;
			}
			onDisconnected.Invoke(isGraceful, isHalfClosed, this);
		}

		// Token: 0x060002F8 RID: 760 RVA: 0x00011734 File Offset: 0x0000F934
		public void Dispose()
		{
			if (this.isDisposed || this.socket == null)
			{
				return;
			}
			this.isDisposed = true;
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
			this.socket.Close();
			this.socket = null;
		}

		// Token: 0x04000151 RID: 337
		public IPEndPoint remoteEndpoint;

		// Token: 0x04000152 RID: 338
		public int port;

		// Token: 0x04000153 RID: 339
		private Logger logger;

		// Token: 0x04000154 RID: 340
		private Socket socket;

		// Token: 0x04000155 RID: 341
		private CancellationTokenSource _listeningCTS;

		// Token: 0x04000156 RID: 342
		private bool isDisposed;

		// Token: 0x04000157 RID: 343
		private byte[] receiveBuffer;

		// Token: 0x04000158 RID: 344
		private byte[] fragmentBuffer = new byte[102400];

		// Token: 0x04000159 RID: 345
		private int fragmentedBytes;
	}
}
