using System;
using System.IO;
using Microsoft.Data.Sqlite;
using Vintagestory.API.Common;
using Vintagestory.API.Config;

namespace RPVoiceChat.DB
{
	// Token: 0x020000A7 RID: 167
	public abstract class DataRepository : IDisposable
	{
		// Token: 0x1700007F RID: 127
		// (get) Token: 0x060004CA RID: 1226 RVA: 0x0001C46E File Offset: 0x0001A66E
		protected SqliteConnection connection { get; }

		// Token: 0x060004CB RID: 1227 RVA: 0x0001C478 File Offset: 0x0001A678
		public DataRepository(ILogger logger, string dbName)
		{
			string text = Path.Combine(GamePaths.DataPath, "ModData", RPVoiceChatMod.modID);
			Directory.CreateDirectory(text);
			string filePath = Path.Combine(text, dbName);
			this._db = new SQLiteDB(logger);
			this._db.OnCreateTables += new Action<SqliteConnection>(this.CreateTablesIfNotExists);
			this._db.Open(filePath);
			this.connection = this._db.connection;
			this.DefineCommands();
		}

		// Token: 0x060004CC RID: 1228
		protected abstract void CreateTablesIfNotExists(SqliteConnection connection);

		// Token: 0x060004CD RID: 1229
		protected abstract void DefineCommands();

		// Token: 0x060004CE RID: 1230 RVA: 0x0001C4F4 File Offset: 0x0001A6F4
		public virtual void Dispose()
		{
			this._db.Dispose();
		}

		// Token: 0x0400028A RID: 650
		private SQLiteDB _db;
	}
}
