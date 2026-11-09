using System;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Vintagestory.API.Common;
using Vintagestory.API.Server;
using Vintagestory.API.Util;

namespace Vintagestory.Server;

public class ServerConsole
{
	private string input;

	private Memory<byte> _memoryBuffer = new Memory<byte>(new byte[1024]);

	private readonly CancellationToken token;

	private int readCount;

	private readonly Stream inputStream;

	private ServerMain server;

	public ServerConsole(ServerMain server, CancellationToken token)
	{
		this.token = token;
		this.server = server;
		inputStream = Console.OpenStandardInput();
		TyronThreadPool.QueueTask((Func<Task>)readAsync, "ServerConsoleReadInput");
	}

	private async Task readAsync()
	{
		while (!token.IsCancellationRequested)
		{
			readCount = await inputStream.ReadAsync(_memoryBuffer, token);
			if (readCount == 0)
			{
				break;
			}
			input += Encoding.UTF8.GetString(_memoryBuffer.Slice(0, readCount).Span);
			if (!input.EndsWithOrdinal(Environment.NewLine))
			{
				break;
			}
			string inputCopy = input.Trim();
			server.EnqueueMainThreadTask(delegate
			{
				if (server.RunPhase == EnumServerRunPhase.Standby)
				{
					if (inputCopy == "/stop")
					{
						Environment.Exit(0);
					}
					if (inputCopy == "/stats")
					{
						ServerMain.Logger.Notification(CmdStats.genStats(server, "\n"));
					}
				}
				else
				{
					server.ReceiveServerConsole(inputCopy);
				}
			});
			input = string.Empty;
		}
	}

	public void Dispose()
	{
		server = null;
	}
}
