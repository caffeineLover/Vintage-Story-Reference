#define TRACE
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using Vintagestory.API.Common;
using Vintagestory.API.Config;

namespace Vintagestory;

public abstract class Logger : LoggerBase, IDisposable
{
	private string program = "";

	protected Dictionary<string, DisposableWriter> fileWriters = new Dictionary<string, DisposableWriter>();

	protected Dictionary<string, uint> LinesWritten = new Dictionary<string, uint>();

	protected Dictionary<string, int> LogfileSplitNumbers = new Dictionary<string, int>();

	protected bool exceptionPrinted;

	protected bool disposed;

	protected bool canUseColor = true;

	private static string[] logTypeToExcludeFromArchive = new string[2] { "crash", "worldgen" };

	public const string ArchiveFolderDateFormat = "yyyy-MM-dd_HH_mm_ss";

	public const string LogDateFormat = "d.M.yyyy HH:mm:ss";

	public const string LogDateFormatVerbose = "d.M.yyyy HH:mm:ss.fff";

	private static bool _logsRotated;

	public static uint LogFileSplitAfterLine = 500000u;

	public Logger(string program, bool clearOldFiles, int archiveLogFileCount, int archiveLogFileMaxSize)
	{
		this.program = program;
		if (clearOldFiles && !_logsRotated)
		{
			ArchiveLogFiles(archiveLogFileCount, archiveLogFileMaxSize);
		}
		foreach (EnumLogType logType in Enum.GetValues(typeof(EnumLogType)))
		{
			string logFileName = getLogFile(logType);
			if (logFileName != null && !fileWriters.ContainsKey(logFileName))
			{
				try
				{
					fileWriters.Add(logFileName, new DisposableWriter(logFileName, logType != EnumLogType.Worldgen && clearOldFiles));
					LinesWritten.Add(logFileName, 0u);
					LogfileSplitNumbers.Add(logFileName, 2);
				}
				catch (Exception e)
				{
					Error("Cannot open logfile {0} for writing ", logFileName);
					Error(e);
				}
			}
		}
		Notification("{0} logger started.", program);
		Notification("Game Version: {0}", GameVersion.LongGameVersion);
	}

	public static string GetTypeName(Type type)
	{
		if (type.IsGenericType)
		{
			string obj = type.Name.Split(new char[1] { '`' }, StringSplitOptions.RemoveEmptyEntries)[0];
			string text = type.GetGenericArguments().Select(GetTypeName).Aggregate((string first, string second) => first + "," + second);
			return obj + "<" + text + ">";
		}
		return type.Name;
	}

	private static void ArchiveLogFiles(int archiveLogFileCount, int archiveLogFileMaxSize)
	{
		_logsRotated = true;
		if (!Directory.Exists(GamePaths.Logs))
		{
			Directory.CreateDirectory(GamePaths.Logs);
			return;
		}
		List<FileInfo> logsToMove = (from folder in Directory.GetFiles(GamePaths.Logs, "*.log")
			where !logTypeToExcludeFromArchive.Any(folder.Contains)
			select new FileInfo(folder) into fileInfo
			orderby fileInfo.LastWriteTime descending
			select fileInfo).ToList();
		if (logsToMove.Count <= 0)
		{
			return;
		}
		string sessionTimeStamp = GetSessionTimeStamp(logsToMove.First().FullName);
		string archiveFolder = Path.Combine(GamePaths.Logs, "Archive");
		Directory.CreateDirectory(archiveFolder);
		string sessionArchiveFolder = Path.Combine(GamePaths.Logs, "Archive", sessionTimeStamp);
		Directory.CreateDirectory(sessionArchiveFolder);
		bool delete = logsToMove.Sum((FileInfo fileInfo) => fileInfo.Length / 1024 / 1024) > archiveLogFileMaxSize;
		foreach (FileInfo file in logsToMove)
		{
			try
			{
				if (delete)
				{
					File.Delete(file.FullName);
				}
				else
				{
					File.Move(file.FullName, Path.Combine(sessionArchiveFolder, file.Name));
				}
			}
			catch (Exception)
			{
			}
		}
		List<DirectoryInfo> directories = (from folder in Directory.GetDirectories(archiveFolder)
			select new DirectoryInfo(folder)).ToList();
		if (directories.Count < archiveLogFileCount)
		{
			return;
		}
		foreach (DirectoryInfo dir in directories.OrderBy((DirectoryInfo folder) => folder.CreationTime).Take(directories.Count - archiveLogFileCount))
		{
			string[] files = Directory.GetFiles(dir.FullName);
			for (int num = 0; num < files.Length; num++)
			{
				File.Delete(files[num]);
			}
			Directory.Delete(dir.FullName);
		}
	}

	private static string GetSessionTimeStamp(string logFile)
	{
		string sessionTimeStamp = null;
		IEnumerator<string> logFileLines = ReadLines(logFile).GetEnumerator();
		logFileLines.MoveNext();
		string timeStamp = string.Join(' ', logFileLines.Current?.Split(' ').Take(2) ?? Array.Empty<string>());
		logFileLines.Dispose();
		if (string.IsNullOrWhiteSpace(timeStamp))
		{
			return DateTime.Now.ToString("yyyy-MM-dd_HH_mm_ss");
		}
		if (DateTime.TryParseExact(timeStamp, "d.M.yyyy HH:mm:ss", null, DateTimeStyles.None, out var sessionTimeStampParsed))
		{
			return sessionTimeStampParsed.ToString("yyyy-MM-dd_HH_mm_ss", CultureInfo.InvariantCulture);
		}
		return DateTime.UtcNow.ToString("yyyy-MM-dd_HH_mm_ss");
	}

	public static IEnumerable<string> ReadLines(string path)
	{
		using FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite, 4096, FileOptions.SequentialScan);
		using StreamReader sr = new StreamReader(fs, Encoding.UTF8);
		string line;
		while ((line = sr.ReadLine()) != null)
		{
			yield return line;
		}
	}

	public void Dispose()
	{
		foreach (DisposableWriter value in fileWriters.Values)
		{
			value.Dispose();
		}
		fileWriters.Clear();
		disposed = true;
	}

	public abstract string getLogFile(EnumLogType logType);

	public abstract bool printToConsole(EnumLogType logType);

	public abstract bool printToDebugWindow(EnumLogType logType);

	protected override void LogImpl(EnumLogType logType, string message, params object[] args)
	{
		if (disposed)
		{
			return;
		}
		try
		{
			string logFileName = getLogFile(logType);
			if (logFileName != null)
			{
				try
				{
					LogToFile(logFileName, logType, message, args);
				}
				catch (NotSupportedException)
				{
					Console.WriteLine("Unable to write to log file " + logFileName);
				}
				catch (ObjectDisposedException)
				{
					Console.WriteLine("Unable to write to log file " + logFileName);
				}
			}
			if (base.TraceLog && printToDebugWindow(logType))
			{
				Trace.WriteLine(FormatLogEntry(logType, message, args));
			}
			if (printToConsole(logType))
			{
				SetColorForLogType(logType);
				Console.WriteLine(FormatLogEntry(logType, message, args));
				if (canUseColor)
				{
					Console.ResetColor();
				}
			}
		}
		catch (Exception)
		{
		}
	}

	public string FormatLogEntry(EnumLogType logType, string message, params object[] args)
	{
		return string.Format(DateTime.Now.ToString((logType == EnumLogType.VerboseDebug) ? "d.M.yyyy HH:mm:ss.fff" : "d.M.yyyy HH:mm:ss") + " [" + program + " " + logType.ToString() + "] " + message, args);
	}

	public virtual void LogToFile(string logFileName, EnumLogType logType, string message, params object[] args)
	{
		if (!fileWriters.ContainsKey(logFileName) || disposed)
		{
			return;
		}
		try
		{
			LinesWritten[logFileName]++;
			if (LinesWritten[logFileName] > LogFileSplitAfterLine)
			{
				LinesWritten[logFileName] = 0u;
				string filename = $"{logFileName.Replace(".log", "")}-{LogfileSplitNumbers[logFileName]}.log";
				fileWriters[logFileName].Dispose();
				fileWriters[logFileName] = new DisposableWriter(filename, clearOldFiles: true);
				LogfileSplitNumbers[logFileName]++;
			}
			string type = logType.ToString() ?? "";
			if (logType == EnumLogType.StoryEvent)
			{
				type = "Event";
			}
			fileWriters[logFileName].WriteLine(DateTime.Now.ToString((logType == EnumLogType.VerboseDebug) ? "d.M.yyyy HH:mm:ss.fff" : "d.M.yyyy HH:mm:ss") + " [" + type + "] " + message, args);
		}
		catch (FormatException)
		{
			if (!exceptionPrinted)
			{
				exceptionPrinted = true;
				Error("Couldn't write to log file, failed formatting {0} (FormatException)", message);
			}
		}
		catch (Exception e)
		{
			if (!exceptionPrinted)
			{
				exceptionPrinted = true;
				Error("Couldn't write to log file {0}!", logFileName);
				Error(e);
			}
		}
	}

	private void SetColorForLogType(EnumLogType logType)
	{
		if (!canUseColor)
		{
			return;
		}
		try
		{
			switch (logType)
			{
			case EnumLogType.Error:
			case EnumLogType.Fatal:
				Console.ForegroundColor = ConsoleColor.Red;
				break;
			case EnumLogType.Warning:
				Console.ForegroundColor = ConsoleColor.DarkYellow;
				break;
			}
		}
		catch (Exception)
		{
			canUseColor = false;
		}
	}
}
