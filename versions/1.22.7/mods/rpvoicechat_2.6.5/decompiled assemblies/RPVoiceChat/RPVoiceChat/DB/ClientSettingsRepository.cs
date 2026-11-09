using System;
using System.Collections.Generic;
using Microsoft.Data.Sqlite;
using Vintagestory.API.Common;

namespace RPVoiceChat.DB
{
	// Token: 0x020000A6 RID: 166
	public class ClientSettingsRepository : DataRepository
	{
		// Token: 0x060004C2 RID: 1218 RVA: 0x0001C1B6 File Offset: 0x0001A3B6
		public ClientSettingsRepository(ILogger logger) : base(logger, "client.db")
		{
			this.Load();
		}

		// Token: 0x060004C3 RID: 1219 RVA: 0x0001C1CA File Offset: 0x0001A3CA
		public float GetPlayerGain(string playerId)
		{
			if (this.cache.ContainsKey(playerId))
			{
				return this.cache[playerId];
			}
			this.SetPlayerGain(playerId, 100);
			return this.cache[playerId];
		}

		// Token: 0x060004C4 RID: 1220 RVA: 0x0001C1FC File Offset: 0x0001A3FC
		public void SetPlayerGain(string playerId, int value)
		{
			float gain = (float)value / 100f;
			this.cache[playerId] = gain;
		}

		// Token: 0x060004C5 RID: 1221 RVA: 0x0001C220 File Offset: 0x0001A420
		public void Save()
		{
			using (SqliteTransaction transaction = base.connection.BeginTransaction())
			{
				foreach (KeyValuePair<string, float> entry in this.cache)
				{
					this.setSettingCmd.Transaction = transaction;
					this.setSettingCmd.Parameters["@playerId"].Value = entry.Key;
					this.setSettingCmd.Parameters["@gain"].Value = entry.Value;
					this.setSettingCmd.ExecuteNonQuery();
				}
				transaction.Commit();
			}
		}

		// Token: 0x060004C6 RID: 1222 RVA: 0x0001C2F8 File Offset: 0x0001A4F8
		protected override void CreateTablesIfNotExists(SqliteConnection connection)
		{
			using (SqliteCommand command = connection.CreateCommand())
			{
				command.CommandText = "CREATE TABLE IF NOT EXISTS settings (playerId TEXT PRIMARY KEY, gain REAL);";
				command.ExecuteNonQuery();
			}
		}

		// Token: 0x060004C7 RID: 1223 RVA: 0x0001C33C File Offset: 0x0001A53C
		protected override void DefineCommands()
		{
			this.getSettingsCmd = base.connection.CreateCommand();
			this.getSettingsCmd.CommandText = "SELECT * FROM settings";
			this.getSettingsCmd.Prepare();
			this.setSettingCmd = base.connection.CreateCommand();
			this.setSettingCmd.CommandText = "INSERT OR REPLACE INTO settings (playerId, gain) VALUES (@playerId, @gain)";
			this.setSettingCmd.Parameters.Add("@playerId", 3, 64);
			this.setSettingCmd.Parameters.Add("@gain", 2);
			this.setSettingCmd.Prepare();
		}

		// Token: 0x060004C8 RID: 1224 RVA: 0x0001C3D4 File Offset: 0x0001A5D4
		private void Load()
		{
			Dictionary<string, float> entries = new Dictionary<string, float>();
			using (SqliteDataReader datareader = this.getSettingsCmd.ExecuteReader())
			{
				while (datareader.Read())
				{
					string playerId = datareader["playerId"].ToString();
					float gain = datareader.GetFloat(1);
					entries.Add(playerId, gain);
				}
			}
			this.cache = entries;
		}

		// Token: 0x060004C9 RID: 1225 RVA: 0x0001C444 File Offset: 0x0001A644
		public override void Dispose()
		{
			SqliteCommand sqliteCommand = this.getSettingsCmd;
			if (sqliteCommand != null)
			{
				sqliteCommand.Dispose();
			}
			SqliteCommand sqliteCommand2 = this.setSettingCmd;
			if (sqliteCommand2 != null)
			{
				sqliteCommand2.Dispose();
			}
			base.Dispose();
		}

		// Token: 0x04000284 RID: 644
		private const string dbName = "client.db";

		// Token: 0x04000285 RID: 645
		private const string tableName = "settings";

		// Token: 0x04000286 RID: 646
		private SqliteCommand getSettingsCmd;

		// Token: 0x04000287 RID: 647
		private SqliteCommand setSettingCmd;

		// Token: 0x04000288 RID: 648
		private Dictionary<string, float> cache;
	}
}
