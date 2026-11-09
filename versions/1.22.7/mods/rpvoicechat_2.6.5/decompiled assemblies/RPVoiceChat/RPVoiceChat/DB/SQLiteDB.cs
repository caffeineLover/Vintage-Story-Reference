using System;
using Microsoft.Data.Sqlite;
using Vintagestory.API.Common;

namespace RPVoiceChat.DB
{
	// Token: 0x020000A8 RID: 168
	internal sealed class SQLiteDB : SQLiteDBConnection
	{
		// Token: 0x17000080 RID: 128
		// (get) Token: 0x060004CF RID: 1231 RVA: 0x0001C501 File Offset: 0x0001A701
		public SqliteConnection connection
		{
			get
			{
				return this.sqliteConn;
			}
		}

		// Token: 0x14000015 RID: 21
		// (add) Token: 0x060004D0 RID: 1232 RVA: 0x0001C50C File Offset: 0x0001A70C
		// (remove) Token: 0x060004D1 RID: 1233 RVA: 0x0001C544 File Offset: 0x0001A744
		public event Action<SqliteConnection> OnCreateTables;

		// Token: 0x060004D2 RID: 1234 RVA: 0x0001C579 File Offset: 0x0001A779
		public SQLiteDB(ILogger logger) : base(logger)
		{
		}

		// Token: 0x060004D3 RID: 1235 RVA: 0x0001C584 File Offset: 0x0001A784
		public void Open(string filePath)
		{
			string error = null;
			if (!base.OpenOrCreate(filePath, ref error, true, true, false))
			{
				throw new Exception("Failed to create data repository: " + error);
			}
		}

		// Token: 0x060004D4 RID: 1236 RVA: 0x0001C5B2 File Offset: 0x0001A7B2
		protected override void CreateTablesIfNotExists(SqliteConnection connection)
		{
			Action<SqliteConnection> onCreateTables = this.OnCreateTables;
			if (onCreateTables == null)
			{
				return;
			}
			onCreateTables.Invoke(connection);
		}
	}
}
