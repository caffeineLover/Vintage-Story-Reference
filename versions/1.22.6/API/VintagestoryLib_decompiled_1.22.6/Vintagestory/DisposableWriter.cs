using System.IO;

namespace Vintagestory;

public class DisposableWriter
{
	protected FileStream stream;

	protected StreamWriter writer;

	protected object writeLock = new object();

	public DisposableWriter(string filename, bool clearOldFiles)
	{
		writer = new StreamWriter(stream = new FileStream(filename, clearOldFiles ? FileMode.Create : FileMode.Append, FileAccess.Write, FileShare.Read));
	}

	public void WriteLine(string line, params object[] args)
	{
		lock (writeLock)
		{
			writer.WriteLine(line, args);
			writer.Flush();
		}
	}

	public void Dispose()
	{
		writer.Dispose();
		stream.Dispose();
	}
}
