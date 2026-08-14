using System;
using System.Diagnostics;
using System.Threading;

namespace Vintagestory.Server;

internal class SpawnerOffthread
{
	private readonly ServerSystemEntitySpawner entitySpawner;

	public SpawnerOffthread(ServerSystemEntitySpawner serverSystem)
	{
		entitySpawner = serverSystem;
	}

	internal void Start()
	{
		long lastTickTime = 0L;
		Stopwatch sw = new Stopwatch();
		while (!entitySpawner.ShouldExit())
		{
			Thread.Sleep(Math.Max(0, 500 - (int)lastTickTime));
			if (!entitySpawner.ShouldExit())
			{
				if (entitySpawner.paused)
				{
					lastTickTime = 0L;
					continue;
				}
				sw.Reset();
				sw.Start();
				entitySpawner.FindMobSpawnPositions_offthread(0.5f);
				sw.Stop();
				lastTickTime = sw.ElapsedMilliseconds;
				continue;
			}
			break;
		}
	}
}
