using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Vintagestory.API.Config;

namespace Vintagestory.API.Common;

public class TyronThreadPool
{
	public static TyronThreadPool Inst = new TyronThreadPool();

	public ILogger Logger;

	public ConcurrentDictionary<int, string> RunningTasks = new ConcurrentDictionary<int, string>(Math.Min(8, Environment.ProcessorCount), 8);

	public ConcurrentDictionary<int, Thread> DedicatedThreads = new ConcurrentDictionary<int, Thread>(Math.Min(8, Environment.ProcessorCount), 8);

	private int keyCounter;

	private int dedicatedCounter;

	public TyronThreadPool()
	{
		ThreadPool.SetMaxThreads(10, 1);
	}

	private int MarkStarted(string caller)
	{
		int key = keyCounter++;
		RunningTasks[key] = caller;
		return key;
	}

	private void MarkEnded(int key)
	{
		RunningTasks.TryRemove(key, out var _);
	}

	public string ListAllRunningTasks()
	{
		StringBuilder sb = new StringBuilder();
		foreach (string name in RunningTasks.Values)
		{
			if (sb.Length > 0)
			{
				sb.Append(", ");
			}
			sb.Append(name);
		}
		if (sb.Length == 0)
		{
			sb.Append("[empty]");
		}
		sb.AppendLine();
		return "Current threadpool tasks: " + sb.ToString() + "\nThread pool thread count: " + ThreadPool.ThreadCount;
	}

	public string ListAllThreads()
	{
		StringBuilder sb = new StringBuilder();
		sb.AppendLine("Server threads (" + DedicatedThreads.Count + "):");
		List<Thread> dediThreads = new List<Thread>(DedicatedThreads.Count);
		foreach (KeyValuePair<int, Thread> entry in DedicatedThreads)
		{
			int index = entry.Key;
			while (index >= dediThreads.Count)
			{
				dediThreads.Add(null);
			}
			dediThreads[index] = entry.Value;
		}
		foreach (Thread t in dediThreads)
		{
			if (t != null && t.ThreadState != System.Threading.ThreadState.Stopped)
			{
				sb.Append("tid" + t.ManagedThreadId + " ");
				sb.Append(t.Name);
				sb.Append(": ");
				sb.AppendLine(t.ThreadState.ToString());
			}
		}
		ProcessThreadCollection threads = Process.GetCurrentProcess().Threads;
		List<ProcessThread> threads2 = new List<ProcessThread>();
		foreach (ProcessThread thread in threads)
		{
			if (thread != null)
			{
				threads2.Add(thread);
			}
		}
		threads2 = threads2.OrderByDescending((ProcessThread processThread) => processThread.UserProcessorTime.Ticks).ToList();
		sb.AppendLine("\nAll process threads (" + threads2.Count + "):");
		foreach (ProcessThread thread2 in threads2)
		{
			if (thread2 != null)
			{
				sb.Append(thread2.ThreadState.ToString() + " ");
				sb.Append("tid" + thread2.Id + " ");
				if (RuntimeEnv.OS != OS.Mac)
				{
					sb.Append(thread2.StartTime);
				}
				sb.Append(": P ");
				sb.Append(thread2.CurrentPriority);
				sb.Append(": ");
				sb.Append(thread2.ThreadState.ToString());
				sb.Append(": T ");
				sb.Append(thread2.UserProcessorTime.ToString());
				sb.Append(": T Total ");
				sb.AppendLine(thread2.TotalProcessorTime.ToString());
			}
		}
		return sb.ToString();
	}

	public static void QueueTask(Action callback, string caller)
	{
		if (RuntimeEnv.DebugThreadPool)
		{
			Inst.Logger.VerboseDebug("QueueTask." + Environment.StackTrace);
		}
		ThreadPool.QueueUserWorkItem(delegate
		{
			int key = Inst.MarkStarted(caller);
			callback();
			Inst.MarkEnded(key);
		});
	}

	public static void QueueTask(Func<Task> callback, string caller)
	{
		if (RuntimeEnv.DebugThreadPool)
		{
			Inst.Logger.VerboseDebug("QueueTask." + Environment.StackTrace);
		}
		ThreadPool.QueueUserWorkItem(async delegate
		{
			int key = Inst.MarkStarted(caller);
			try
			{
				await callback();
			}
			catch (Exception e)
			{
				Inst.Logger.Warning("QueueTask: " + caller + " threw an exception!");
				Inst.Logger.Warning(e);
			}
			Inst.MarkEnded(key);
		});
	}

	[Obsolete("Use QueueTask instead.")]
	public static void QueueLongDurationTask(Action callback, string caller)
	{
		QueueTask(callback, caller);
	}

	[Obsolete("Use QueueTask with the caller parameter instead.")]
	public static void QueueTask(Action callback)
	{
		QueueTask(callback, "Unknown");
	}

	[Obsolete("Use QueueTask with the caller parameter instead.")]
	public static void QueueLongDurationTask(Action callback)
	{
		QueueTask(callback, "Unknown");
	}

	/// <summary>
	/// Use this to create any dedicated thread (by default, IsBackground is true, but that can be changed by calling code)
	/// <br />This records the thread in the list of DedicatedThreads we maintain here, for stats purposes
	/// </summary>
	/// <param name="starter"></param>
	/// <param name="name"></param>
	/// <returns></returns>
	public static Thread CreateDedicatedThread(ThreadStart starter, string name)
	{
		Thread thread = new Thread(starter);
		thread.IsBackground = true;
		thread.Name = name;
		Inst.DedicatedThreads[Inst.dedicatedCounter++] = thread;
		return thread;
	}

	public void Dispose()
	{
		RunningTasks.Clear();
		DedicatedThreads.Clear();
		keyCounter = 0;
		dedicatedCounter = 0;
	}
}
