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
	// Token: 0x02000057 RID: 87
	public class TCPConnection : IDisposable
	{
		// Token: 0x1400000B RID: 11
		// (add) Token: 0x0600024E RID: 590 RVA: 0x0000D544 File Offset: 0x0000B744
		// (remove) Token: 0x0600024F RID: 591 RVA: 0x0000D57C File Offset: 0x0000B77C
		public event Action<byte[], TCPConnection> OnMessageReceived;

		// Token: 0x1400000C RID: 12
		// (add) Token: 0x06000250 RID: 592 RVA: 0x0000D5B4 File Offset: 0x0000B7B4
		// (remove) Token: 0x06000251 RID: 593 RVA: 0x0000D5EC File Offset: 0x0000B7EC
		public event Action<bool, bool, TCPConnection> OnDisconnected;

		// Token: 0x06000252 RID: 594 RVA: 0x0000D624 File Offset: 0x0000B824
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

		// Token: 0x06000253 RID: 595 RVA: 0x0000D6D5 File Offset: 0x0000B8D5
		public void Connect(IPEndPoint endPoint)
		{
			this.socket.Connect(endPoint);
			this.remoteEndpoint = (IPEndPoint)this.socket.RemoteEndPoint;
			this.port = ((IPEndPoint)this.socket.LocalEndPoint).Port;
		}

		// Token: 0x06000254 RID: 596 RVA: 0x0000D714 File Offset: 0x0000B914
		public Task ConnectAsync(IPEndPoint endPoint)
		{
			return this.socket.ConnectAsync(endPoint).ContinueWith(delegate(Task _)
			{
				this.remoteEndpoint = (IPEndPoint)this.socket.RemoteEndPoint;
				this.port = ((IPEndPoint)this.socket.LocalEndPoint).Port;
			}, 393216);
		}

		// Token: 0x06000255 RID: 597 RVA: 0x0000D738 File Offset: 0x0000B938
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

		// Token: 0x06000256 RID: 598 RVA: 0x0000D7FC File Offset: 0x0000B9FC
		public ValueTask<int> SendAsync(byte[] data, CancellationToken ct = default(CancellationToken))
		{
			if (this.socket == null)
			{
				throw new Exception("Socket already disposed.");
			}
			byte[] tcpMessage = this.PackMessage(data);
			return this.socket.SendAsync(tcpMessage, ct);
		}

		// Token: 0x06000257 RID: 599 RVA: 0x0000D836 File Offset: 0x0000BA36
		public void StartListening()
		{
			if (this.socket == null)
			{
				throw new Exception("Socket already disposed.");
			}
			this._listeningCTS = new CancellationTokenSource();
			this.Listen(this._listeningCTS.Token);
		}

		// Token: 0x06000258 RID: 600 RVA: 0x0000D868 File Offset: 0x0000BA68
		private void Listen(CancellationToken ct)
		{
			TCPConnection.<Listen>d__21 <Listen>d__;
			<Listen>d__.<>t__builder = AsyncVoidMethodBuilder.Create();
			<Listen>d__.<>4__this = this;
			<Listen>d__.ct = ct;
			<Listen>d__.<>1__state = -1;
			<Listen>d__.<>t__builder.Start<TCPConnection.<Listen>d__21>(ref <Listen>d__);
		}

		// Token: 0x06000259 RID: 601 RVA: 0x0000D8A8 File Offset: 0x0000BAA8
		private byte[] PackMessage(byte[] data)
		{
			int num = data.Length;
			byte[] prefix = BitConverter.GetBytes(num);
			byte[] tcpMessage = new byte[num + 4];
			Buffer.BlockCopy(prefix, 0, tcpMessage, 0, 4);
			Buffer.BlockCopy(data, 0, tcpMessage, 4, data.Length);
			return tcpMessage;
		}

		// Token: 0x0600025A RID: 602 RVA: 0x0000D8E0 File Offset: 0x0000BAE0
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

		// Token: 0x0600025B RID: 603 RVA: 0x0000DBA4 File Offset: 0x0000BDA4
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

		// Token: 0x0600025C RID: 604 RVA: 0x0000DC0C File Offset: 0x0000BE0C
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

		// Token: 0x0600025D RID: 605 RVA: 0x0000DC68 File Offset: 0x0000BE68
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

		// Token: 0x040000FF RID: 255
		public IPEndPoint remoteEndpoint;

		// Token: 0x04000100 RID: 256
		public int port;

		// Token: 0x04000101 RID: 257
		private Logger logger;

		// Token: 0x04000102 RID: 258
		private Socket socket;

		// Token: 0x04000103 RID: 259
		private CancellationTokenSource _listeningCTS;

		// Token: 0x04000104 RID: 260
		private bool isDisposed;

		// Token: 0x04000105 RID: 261
		private byte[] receiveBuffer;

		// Token: 0x04000106 RID: 262
		private byte[] fragmentBuffer = new byte[102400];

		// Token: 0x04000107 RID: 263
		private int fragmentedBytes;
	}
}
