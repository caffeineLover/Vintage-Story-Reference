using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.IO;
using Microsoft.Data.Sqlite;
using Vintagestory.API.Common;
using Vintagestory.API.Config;
using Vintagestory.API.MathTools;
using Vintagestory.Server;

namespace Vintagestory.Common.Database;

public class SQLiteDbConnectionv2 : SQLiteDBConnection, IGameDbConnection, IDisposable
{
	private SqliteCommand setChunksCmd;

	private SqliteCommand setMapChunksCmd;

	private SqliteCommand getChunkCmd;

	private SqliteCommand getMapChunkCmd;

	private SqliteCommand getMapRegionCmd;

	private SqliteCommand existsChunkCmd;

	private SqliteCommand existsMapChunkCmd;

	private SqliteCommand existsMapRegionCmd;

	public override string DBTypeCode => "savegame database";

	public SQLiteDbConnectionv2(ILogger logger)
		: base(logger)
	{
		base.logger = logger;
	}

	public override void OnOpened()
	{
		setChunksCmd = sqliteConn.CreateCommand();
		setChunksCmd.CommandText = "INSERT OR REPLACE INTO chunk (position, data) VALUES (@position,@data)";
		setChunksCmd.Parameters.Add(CreateParameter("position", DbType.UInt64, 0, setChunksCmd));
		setChunksCmd.Parameters.Add(CreateParameter("data", DbType.Object, null, setChunksCmd));
		setChunksCmd.Prepare();
		setMapChunksCmd = sqliteConn.CreateCommand();
		setMapChunksCmd.CommandText = "INSERT OR REPLACE INTO mapchunk (position, data) VALUES (@position,@data)";
		setMapChunksCmd.Parameters.Add(CreateParameter("position", DbType.UInt64, 0, setMapChunksCmd));
		setMapChunksCmd.Parameters.Add(CreateParameter("data", DbType.Object, null, setMapChunksCmd));
		setMapChunksCmd.Prepare();
		getChunkCmd = CreateCommand("data", "chunk");
		getMapChunkCmd = CreateCommand("data", "mapchunk");
		getMapRegionCmd = CreateCommand("data", "mapregion");
		existsChunkCmd = CreateCommand("position", "chunk");
		existsMapChunkCmd = CreateCommand("position", "mapchunk");
		existsMapRegionCmd = CreateCommand("position", "mapregion");
	}

	private SqliteCommand CreateCommand(string column, string tablename)
	{
		SqliteCommand cmd = sqliteConn.CreateCommand();
		cmd.CommandText = $"SELECT {column} FROM {tablename} WHERE position=@position";
		cmd.Parameters.Add(CreateParameter("position", DbType.UInt64, 0, cmd));
		cmd.Prepare();
		return cmd;
	}

	public void UpgradeToWriteAccess()
	{
		CreateTablesIfNotExists(sqliteConn);
	}

	public bool IntegrityCheck()
	{
		if (!DoIntegrityCheck(sqliteConn))
		{
			string msg = "Database integrity check failed. Attempt basic repair procedure (via VACUUM), this might take minutes to hours depending on the size of the save game...";
			logger.Notification(msg);
			logger.StoryEvent(msg);
			try
			{
				using (SqliteCommand cmd = sqliteConn.CreateCommand())
				{
					cmd.CommandText = "PRAGMA writable_schema=ON;";
					cmd.ExecuteNonQuery();
				}
				using SqliteCommand cmd2 = sqliteConn.CreateCommand();
				cmd2.CommandText = "VACUUM;";
				cmd2.ExecuteNonQuery();
			}
			catch
			{
				logger.StoryEvent("Unable to repair :(");
				logger.Notification("Unable to repair :(\nRecommend any of the solutions posted here: https://wiki.vintagestory.at/index.php/Repairing_a_corrupt_savegame_or_worldmap\nWill exit now");
				throw new Exception("Database integrity bad");
			}
			if (!DoIntegrityCheck(sqliteConn, logResults: false))
			{
				logger.StoryEvent("Unable to repair :(");
				logger.Notification("Database integrity still bad :(\nRecommend any of the solutions posted here: https://wiki.vintagestory.at/index.php/Repairing_a_corrupt_savegame_or_worldmap\nWill exit now");
				throw new Exception("Database integrity bad");
			}
			logger.Notification("Database integrity check now okay, yay!");
		}
		return true;
	}

	public int QuantityChunks()
	{
		using SqliteCommand cmd = sqliteConn.CreateCommand();
		cmd.CommandText = "SELECT count(*) FROM chunk";
		return System.Convert.ToInt32(cmd.ExecuteScalar());
	}

	public IEnumerable<DbChunk> GetAllChunks(string tablename)
	{
		using SqliteCommand cmd = sqliteConn.CreateCommand();
		cmd.CommandText = "SELECT position, data FROM " + tablename;
		using SqliteDataReader sqlite_datareader = cmd.ExecuteReader();
		while (sqlite_datareader.Read())
		{
			object data = sqlite_datareader["data"];
			ChunkPos pos = ChunkPos.FromChunkIndex_saveGamev2((ulong)(long)sqlite_datareader["position"]);
			yield return new DbChunk
			{
				Position = pos,
				Data = (data as byte[])
			};
		}
	}

	public IEnumerable<DbChunk> GetAllChunks()
	{
		return GetAllChunks("chunk");
	}

	public IEnumerable<DbChunk> GetAllMapChunks()
	{
		return GetAllChunks("mapchunk");
	}

	public IEnumerable<DbChunk> GetAllMapRegions()
	{
		return GetAllChunks("mapregion");
	}

	public void ForAllChunks(Action<DbChunk> action)
	{
		using SqliteCommand cmd = sqliteConn.CreateCommand();
		cmd.CommandText = "SELECT position, data FROM chunk";
		using SqliteDataReader sqlite_datareader = cmd.ExecuteReader();
		while (sqlite_datareader.Read())
		{
			object data = sqlite_datareader["data"];
			ChunkPos pos = ChunkPos.FromChunkIndex_saveGamev2((ulong)(long)sqlite_datareader["position"]);
			action(new DbChunk
			{
				Position = pos,
				Data = (data as byte[])
			});
		}
	}

	public byte[] GetPlayerData(string playeruid)
	{
		using SqliteCommand cmd = sqliteConn.CreateCommand();
		cmd.CommandText = "SELECT data FROM playerdata WHERE playeruid=@playeruid";
		cmd.Parameters.Add(CreateParameter("playeruid", DbType.String, playeruid, cmd));
		using SqliteDataReader dataReader = cmd.ExecuteReader();
		if (dataReader.Read())
		{
			return dataReader["data"] as byte[];
		}
		return null;
	}

	public void SetPlayerData(string playeruid, byte[] data)
	{
		if (data == null)
		{
			using (DbCommand deleteCmd = sqliteConn.CreateCommand())
			{
				deleteCmd.CommandText = "DELETE FROM playerdata WHERE playeruid=@playeruid";
				deleteCmd.Parameters.Add(CreateParameter("playeruid", DbType.String, playeruid, deleteCmd));
				deleteCmd.ExecuteNonQuery();
				return;
			}
		}
		if (GetPlayerData(playeruid) == null)
		{
			using (DbCommand insertCmd = sqliteConn.CreateCommand())
			{
				insertCmd.CommandText = "INSERT INTO playerdata (playeruid, data) VALUES (@playeruid,@data)";
				insertCmd.Parameters.Add(CreateParameter("playeruid", DbType.String, playeruid, insertCmd));
				insertCmd.Parameters.Add(CreateParameter("data", DbType.Object, data, insertCmd));
				insertCmd.ExecuteNonQuery();
				return;
			}
		}
		using DbCommand updateCmd = sqliteConn.CreateCommand();
		updateCmd.CommandText = "UPDATE playerdata set data=@data where playeruid=@playeruid";
		updateCmd.Parameters.Add(CreateParameter("data", DbType.Object, data, updateCmd));
		updateCmd.Parameters.Add(CreateParameter("playeruid", DbType.String, playeruid, updateCmd));
		updateCmd.ExecuteNonQuery();
	}

	public IEnumerable<byte[]> GetChunks(IEnumerable<ChunkPos> chunkpositions)
	{
		lock (transactionLock)
		{
			using SqliteTransaction transaction = sqliteConn.BeginTransaction();
			getChunkCmd.Transaction = transaction;
			foreach (ChunkPos chunkposition in chunkpositions)
			{
				yield return GetChunk(chunkposition.ToChunkIndex(), getChunkCmd);
			}
			transaction.Commit();
		}
	}

	public byte[] GetChunk(ulong position)
	{
		return GetChunk(position, getChunkCmd);
	}

	public byte[] GetMapChunk(ulong position)
	{
		return GetChunk(position, getMapChunkCmd);
	}

	public byte[] GetMapRegion(ulong position)
	{
		return GetChunk(position, getMapRegionCmd);
	}

	public bool ChunkExists(ulong position)
	{
		return ChunkExists(position, existsChunkCmd);
	}

	public bool MapChunkExists(ulong position)
	{
		return ChunkExists(position, existsMapChunkCmd);
	}

	public bool MapRegionExists(ulong position)
	{
		return ChunkExists(position, existsMapRegionCmd);
	}

	public bool ChunkExists(ulong position, string tablename)
	{
		SqliteCommand cmd = sqliteConn.CreateCommand();
		cmd.CommandText = "SELECT position FROM " + tablename + " WHERE position=@position";
		cmd.Parameters.Add(CreateParameter("position", DbType.UInt64, position, cmd));
		return ChunkExists(position, cmd);
	}

	public bool ChunkExists(ulong position, DbCommand preparedCmd)
	{
		using DbCommand cmd = preparedCmd;
		cmd.Parameters["position"].Value = position;
		using DbDataReader dataReader = cmd.ExecuteReader();
		return dataReader.HasRows;
	}

	public byte[] GetChunk(ulong position, string tablename)
	{
		using SqliteCommand cmd = sqliteConn.CreateCommand();
		cmd.CommandText = "SELECT data FROM " + tablename + " WHERE position=@position";
		cmd.Parameters.Add(CreateParameter("position", DbType.UInt64, position, cmd));
		return GetChunk(position, cmd);
	}

	public byte[] GetChunk(ulong position, DbCommand preparedCmd)
	{
		using DbCommand cmd = preparedCmd;
		cmd.Parameters["position"].Value = position;
		using DbDataReader dataReader = cmd.ExecuteReader();
		byte[] result = null;
		if (dataReader.Read())
		{
			result = dataReader["data"] as byte[];
		}
		return result;
	}

	public void DeleteChunks(IEnumerable<ChunkPos> chunkpositions)
	{
		DeleteChunks(chunkpositions, "chunk");
	}

	public void DeleteMapChunks(IEnumerable<ChunkPos> mapchunkpositions)
	{
		DeleteChunks(mapchunkpositions, "mapchunk");
	}

	public void DeleteMapRegions(IEnumerable<ChunkPos> mapchunkregions)
	{
		DeleteChunks(mapchunkregions, "mapregion");
	}

	public void DeleteChunks(IEnumerable<ChunkPos> chunkpositions, string tablename)
	{
		lock (transactionLock)
		{
			using SqliteTransaction transaction = sqliteConn.BeginTransaction();
			using DbCommand cmd = sqliteConn.CreateCommand();
			cmd.Transaction = transaction;
			cmd.CommandText = "DELETE FROM " + tablename + " WHERE position=@position";
			cmd.Parameters.Add(CreateParameter("position", DbType.UInt64, 0, cmd));
			foreach (ChunkPos chunkposition in chunkpositions)
			{
				DeleteChunk(chunkposition.ToChunkIndex(), cmd);
			}
			transaction.Commit();
		}
	}

	public void DeleteChunk(ulong position, DbCommand cmd)
	{
		cmd.Parameters["position"].Value = position;
		cmd.ExecuteNonQuery();
	}

	public void DeleteChunk(ulong position, string tablename)
	{
		using DbCommand cmd = sqliteConn.CreateCommand();
		cmd.CommandText = "DELETE FROM " + tablename + " WHERE position=@position";
		cmd.Parameters.Add(CreateParameter("position", DbType.UInt64, position, cmd));
		cmd.ExecuteNonQuery();
	}

	public void SetChunks(IEnumerable<DbChunk> chunks)
	{
		SetMulti(chunks, setChunksCmd, doZeroY: false);
	}

	public void SetMapChunks(IEnumerable<DbChunk> mapchunks)
	{
		SetMulti(mapchunks, setMapChunksCmd, doZeroY: true);
	}

	public void SetMapRegions(IEnumerable<DbChunk> mapregions)
	{
		using DbCommand cmd = sqliteConn.CreateCommand();
		cmd.CommandText = "INSERT OR REPLACE INTO mapregion (position, data) VALUES (@position,@data)";
		cmd.Parameters.Add(CreateParameter("position", DbType.UInt64, 0, cmd));
		cmd.Parameters.Add(CreateParameter("data", DbType.Object, null, cmd));
		SetMulti(mapregions, cmd, doZeroY: true);
	}

	private void SetMulti(IEnumerable<DbChunk> chunks, DbCommand cmd, bool doZeroY)
	{
		lock (transactionLock)
		{
			using SqliteTransaction transaction = sqliteConn.BeginTransaction();
			cmd.Transaction = transaction;
			DbParameter positionParameter = cmd.Parameters["position"];
			DbParameter dataParameter = cmd.Parameters["data"];
			foreach (DbChunk c in chunks)
			{
				if (doZeroY)
				{
					c.Position.Y = 0;
				}
				positionParameter.Value = c.Position.ToChunkIndex();
				dataParameter.Value = c.Data;
				cmd.ExecuteNonQuery();
			}
			transaction.Commit();
		}
	}

	public byte[] GetGameData()
	{
		try
		{
			using SqliteCommand cmd = sqliteConn.CreateCommand();
			cmd.CommandText = "SELECT data FROM gamedata LIMIT 1";
			using SqliteDataReader sqlite_datareader = cmd.ExecuteReader();
			if (sqlite_datareader.Read())
			{
				return sqlite_datareader["data"] as byte[];
			}
			return null;
		}
		catch (Exception ex)
		{
			logger.Warning("Exception thrown on GetGlobalData: " + ex.Message);
			return null;
		}
	}

	public void StoreGameData(byte[] data)
	{
		lock (transactionLock)
		{
			using SqliteTransaction transaction = sqliteConn.BeginTransaction();
			using (DbCommand cmd = sqliteConn.CreateCommand())
			{
				cmd.CommandText = "INSERT OR REPLACE INTO gamedata (savegameid, data) VALUES (@savegameid,@data)";
				cmd.Parameters.Add(CreateParameter("savegameid", DbType.UInt64, 1, cmd));
				cmd.Parameters.Add(CreateParameter("data", DbType.Object, data, cmd));
				cmd.ExecuteNonQuery();
			}
			transaction.Commit();
		}
	}

	public bool QuickCorrectSaveGameVersionTest()
	{
		using SqliteCommand cmd = sqliteConn.CreateCommand();
		cmd.CommandText = "SELECT name FROM sqlite_master WHERE type = 'table' AND name = 'gamedata';";
		return cmd.ExecuteScalar() != null;
	}

	protected override void CreateTablesIfNotExists(SqliteConnection sqliteConn)
	{
		using (SqliteCommand sqlite_cmd = sqliteConn.CreateCommand())
		{
			sqlite_cmd.CommandText = "CREATE TABLE IF NOT EXISTS chunk (position integer PRIMARY KEY, data BLOB);";
			sqlite_cmd.ExecuteNonQuery();
		}
		using (SqliteCommand sqlite_cmd2 = sqliteConn.CreateCommand())
		{
			sqlite_cmd2.CommandText = "CREATE TABLE IF NOT EXISTS mapchunk (position integer PRIMARY KEY, data BLOB);";
			sqlite_cmd2.ExecuteNonQuery();
		}
		using (SqliteCommand sqlite_cmd3 = sqliteConn.CreateCommand())
		{
			sqlite_cmd3.CommandText = "CREATE TABLE IF NOT EXISTS mapregion (position integer PRIMARY KEY, data BLOB);";
			sqlite_cmd3.ExecuteNonQuery();
		}
		using (SqliteCommand sqlite_cmd4 = sqliteConn.CreateCommand())
		{
			sqlite_cmd4.CommandText = "CREATE TABLE IF NOT EXISTS gamedata (savegameid integer PRIMARY KEY, data BLOB);";
			sqlite_cmd4.ExecuteNonQuery();
		}
		using (SqliteCommand sqlite_cmd5 = sqliteConn.CreateCommand())
		{
			sqlite_cmd5.CommandText = "CREATE TABLE IF NOT EXISTS playerdata (playerid integer PRIMARY KEY AUTOINCREMENT, playeruid TEXT, data BLOB);";
			sqlite_cmd5.ExecuteNonQuery();
		}
		using SqliteCommand sqlite_cmd6 = sqliteConn.CreateCommand();
		sqlite_cmd6.CommandText = "CREATE index IF NOT EXISTS index_playeruid on playerdata(playeruid);";
		sqlite_cmd6.ExecuteNonQuery();
	}

	public void CreateBackup(string backupFilename)
	{
		if (databaseFileName == backupFilename)
		{
			logger.Error("Cannot overwrite current running database. Chose another destination.");
			return;
		}
		string fileName = Path.Combine(GamePaths.Backups, backupFilename);
		if (File.Exists(fileName))
		{
			logger.Warning("File " + fileName + " exists. Deleting old file.");
			File.Delete(fileName);
		}
		SqliteConnection sqliteBckConn = new SqliteConnection(new DbConnectionStringBuilder
		{
			{ "Data Source", fileName },
			{ "Pooling", "false" }
		}.ToString());
		sqliteBckConn.Open();
		using (SqliteCommand configCmd = sqliteBckConn.CreateCommand())
		{
			configCmd.CommandText = "PRAGMA journal_mode=Off;";
			configCmd.ExecuteNonQuery();
		}
		sqliteConn.BackupDatabase(sqliteBckConn, sqliteBckConn.Database, sqliteConn.Database);
		sqliteBckConn.Close();
		sqliteBckConn.Dispose();
	}

	public void BackupPartial(string backupFilename, Vec2i start, Vec2i end, int mapSizeYinChunks)
	{
		if (databaseFileName == backupFilename)
		{
			logger.Error("Cannot overwrite current running database. Chose another destination.");
			return;
		}
		string fileName = Path.Combine(GamePaths.Backups, backupFilename);
		if (File.Exists(fileName))
		{
			logger.Warning("File " + fileName + " exists. Deleting old file.");
			File.Delete(fileName);
		}
		try
		{
			SQLiteDbConnectionv2 backCon = new SQLiteDbConnectionv2(logger);
			string error = null;
			if (!backCon.OpenOrCreate(fileName, ref error, requireWriteAccess: true, corruptionProtection: false, doIntegrityCheck: false))
			{
				return;
			}
			using SqliteCommand cmd = backCon.sqliteConn.CreateCommand();
			cmd.CommandText = "    ATTACH DATABASE $source AS src;\r\n\r\n    INSERT INTO main.gamedata\r\n    SELECT * FROM src.gamedata;\r\n\r\n    INSERT INTO main.playerdata\r\n    SELECT * FROM src.playerdata;\r\n\r\n    DETACH DATABASE src;";
			cmd.Parameters.AddWithValue("$source", databaseFileName);
			cmd.ExecuteNonQuery();
			List<DbChunk> chunks = new List<DbChunk>();
			List<DbChunk> mapChunks = new List<DbChunk>();
			List<DbChunk> mapRegions = new List<DbChunk>();
			for (int x = start.X; x <= end.X; x++)
			{
				for (int z = start.Y; z <= end.Y; z++)
				{
					for (int y = 0; y < mapSizeYinChunks; y++)
					{
						ChunkPos chunkPos = new ChunkPos(x, y, z);
						DbChunk chunk = new DbChunk(chunkPos, GetChunk(chunkPos.ToChunkIndex()));
						if (chunk.Data != null)
						{
							chunks.Add(chunk);
						}
					}
					ChunkPos mapChunkPos = new ChunkPos(x, 0, z);
					DbChunk mapChunk = new DbChunk(mapChunkPos, GetMapChunk(mapChunkPos.ToChunkIndex()));
					if (mapChunk.Data != null)
					{
						mapChunks.Add(mapChunk);
					}
				}
			}
			backCon.SetChunks(chunks);
			backCon.SetMapChunks(mapChunks);
			int regionSize = MagicNum.ChunkRegionSizeInChunks;
			for (int i = start.X / regionSize; i <= end.X / regionSize; i++)
			{
				for (int j = start.Y / regionSize; j <= end.Y / regionSize; j++)
				{
					ChunkPos regionPos = new ChunkPos(i, 0, j);
					DbChunk region = new DbChunk(regionPos, GetMapRegion(regionPos.ToChunkIndex()));
					if (region.Data != null)
					{
						mapRegions.Add(region);
					}
				}
			}
			backCon.SetMapRegions(mapRegions);
			backCon.Close();
		}
		catch (Exception ex)
		{
			logger.Error("Error during backup partial: " + ex);
		}
	}

	public override void Close()
	{
		DisposeCommands();
		base.Close();
	}

	public override void Dispose()
	{
		DisposeCommands();
		base.Dispose();
	}

	private void DisposeCommands()
	{
		setChunksCmd?.Dispose();
		setMapChunksCmd?.Dispose();
		getChunkCmd?.Dispose();
		getMapChunkCmd?.Dispose();
		getMapRegionCmd?.Dispose();
		existsChunkCmd?.Dispose();
		existsMapChunkCmd?.Dispose();
		existsMapRegionCmd?.Dispose();
	}

	bool IGameDbConnection.OpenOrCreate(string filename, ref string errorMessage, bool requireWriteAccess, bool corruptionProtection, bool doIntegrityCheck)
	{
		return OpenOrCreate(filename, ref errorMessage, requireWriteAccess, corruptionProtection, doIntegrityCheck);
	}

	void IGameDbConnection.Vacuum()
	{
		Vacuum();
	}
}
