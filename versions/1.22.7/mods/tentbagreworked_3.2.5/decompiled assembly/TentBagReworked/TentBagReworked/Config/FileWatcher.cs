using System;
using System.IO;
using System.Runtime.CompilerServices;
using Vintagestory.API.Config;

namespace TentBagReworked.Config
{
	// Token: 0x02000007 RID: 7
	[NullableContext(1)]
	[Nullable(0)]
	public class FileWatcher
	{
		// Token: 0x1700000E RID: 14
		// (get) Token: 0x06000050 RID: 80 RVA: 0x00004E38 File Offset: 0x00003038
		// (set) Token: 0x06000051 RID: 81 RVA: 0x00004E40 File Offset: 0x00003040
		public bool Queued { get; set; }

		// Token: 0x06000052 RID: 82 RVA: 0x00004E4C File Offset: 0x0000304C
		public FileWatcher(TentBagReworkedModSystem mod)
		{
			this._mod = mod;
			this._watcher = new FileSystemWatcher(GamePaths.ModConfig)
			{
				Filter = mod.ModId + ".json",
				IncludeSubdirectories = false,
				EnableRaisingEvents = true
			};
			this._watcher.Changed += new FileSystemEventHandler(this.Changed);
			this._watcher.Created += new FileSystemEventHandler(this.Changed);
			this._watcher.Deleted += new FileSystemEventHandler(this.Changed);
			this._watcher.Renamed += new RenamedEventHandler(this.Changed);
			this._watcher.Error += new ErrorEventHandler(this.Error);
		}

		// Token: 0x06000053 RID: 83 RVA: 0x00004F17 File Offset: 0x00003117
		private void Changed(object sender, FileSystemEventArgs e)
		{
			this.QueueReload(true);
		}

		// Token: 0x06000054 RID: 84 RVA: 0x00004F22 File Offset: 0x00003122
		private void Error(object sender, ErrorEventArgs e)
		{
			this._mod.Logger.Error(e.GetException().ToString());
			this.QueueReload(false);
		}

		// Token: 0x06000055 RID: 85 RVA: 0x00004F4C File Offset: 0x0000314C
		private void QueueReload(bool changed = false)
		{
			bool queued = this.Queued;
			if (!queued)
			{
				this.Queued = true;
				if (changed)
				{
					this._mod.Logger.Event("Detected the config was changed. Reloading.");
				}
				this._mod.Api.Event.RegisterCallback(delegate(float _)
				{
					this._mod.ReloadConfig();
					this._mod.Api.Event.RegisterCallback(delegate(float _)
					{
						this.Queued = false;
					}, 100);
				}, 100);
			}
		}

		// Token: 0x06000056 RID: 86 RVA: 0x00004FB0 File Offset: 0x000031B0
		public void Dispose()
		{
			this._watcher.Changed -= new FileSystemEventHandler(this.Changed);
			this._watcher.Created -= new FileSystemEventHandler(this.Changed);
			this._watcher.Deleted -= new FileSystemEventHandler(this.Changed);
			this._watcher.Renamed -= new RenamedEventHandler(this.Changed);
			this._watcher.Error -= new ErrorEventHandler(this.Error);
			this._watcher.Dispose();
		}

		// Token: 0x0400002C RID: 44
		private readonly FileSystemWatcher _watcher;

		// Token: 0x0400002D RID: 45
		private readonly TentBagReworkedModSystem _mod;
	}
}
