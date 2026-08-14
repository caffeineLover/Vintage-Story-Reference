using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using JsonPatch.Operations;
using JsonPatch.Operations.Abstractions;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Tavis;
using Vintagestory.API.Common;
using Vintagestory.API.Datastructures;
using Vintagestory.API.Util;

namespace Vintagestory.ServerMods.NoObf;

public class ModJsonPatchLoader : ModSystem
{
	private ICoreAPI api;

	private ITreeAttribute worldConfig;

	private Dictionary<AssetLocation, (JToken token, IAsset asset)> jsonCache = new Dictionary<AssetLocation, (JToken, IAsset)>();

	public override bool ShouldLoad(EnumAppSide side)
	{
		return true;
	}

	public override double ExecuteOrder()
	{
		return 0.05;
	}

	public override void AssetsLoaded(ICoreAPI api)
	{
		this.api = api;
		worldConfig = api.World.Config;
		if (worldConfig == null)
		{
			worldConfig = new TreeAttribute();
		}
		ApplyPatches();
	}

	public void ApplyPatches(string forPartialPath = null)
	{
		List<IAsset> entries = api.Assets.GetMany("patches/");
		int appliedCount = 0;
		int notfoundCount = 0;
		int errorCount = 0;
		int totalCount = 0;
		int unmetConditionCount = 0;
		HashSet<string> hashSet = new HashSet<string>();
		foreach (string item in api.ModLoader.Mods.Select((Mod m) => m.Info.ModID))
		{
			hashSet.Add(item);
		}
		HashSet<string> loadedModIds = hashSet;
		List<(JsonPatch, AssetLocation, int)> validPatches = new List<(JsonPatch, AssetLocation, int)>();
		foreach (IAsset asset in entries)
		{
			JsonPatch[] patches = null;
			try
			{
				patches = asset.ToObject<JsonPatch[]>();
			}
			catch (Exception e)
			{
				api.Logger.Error("Failed loading patches file {0}:", asset.Location);
				api.Logger.Error(e);
			}
			for (int j = 0; patches != null && j < patches.Length; j++)
			{
				JsonPatch patch = patches[j];
				if (!patch.Enabled || (((!patch.Side.HasValue) ? patch.File.Category.SideType : patch.Side.Value) != EnumAppSide.Universal && patch.Side != api.Side))
				{
					continue;
				}
				if (patch.Condition != null)
				{
					IAttribute attr = worldConfig[patch.Condition.When];
					if (attr == null)
					{
						continue;
					}
					if (patch.Condition.useValue)
					{
						patch.Value = new JsonObject(JToken.Parse(attr.ToJsonToken()));
					}
					else if (!patch.Condition.IsValue.Equals(attr.GetValue()?.ToString() ?? "", StringComparison.InvariantCultureIgnoreCase))
					{
						api.Logger.VerboseDebug("Patch file {0}, patch {1}: Unmet IsValue condition ({2}!={3})", asset.Location, j, patch.Condition.IsValue, attr.GetValue()?.ToString() ?? "");
						unmetConditionCount++;
						continue;
					}
				}
				if (patch.DependsOn != null)
				{
					bool enabled = true;
					PatchModDependence[] dependsOn = patch.DependsOn;
					foreach (PatchModDependence dependence in dependsOn)
					{
						bool loaded = loadedModIds.Contains(dependence.modid);
						enabled = enabled && (loaded ^ dependence.invert);
					}
					if (!enabled)
					{
						unmetConditionCount++;
						api.Logger.VerboseDebug("Patch file {0}, patch {1}: Unmet DependsOn condition ({2})", asset.Location, j, string.Join(",", patch.DependsOn.Select((PatchModDependence pd) => (pd.invert ? "!" : "") + pd.modid)));
						continue;
					}
				}
				if (forPartialPath == null || patch.File.PathStartsWith(forPartialPath))
				{
					validPatches.Add((patch, asset.Location, j));
				}
			}
		}
		totalCount = validPatches.Count;
		HashSet<AssetLocation> processedFiles = new HashSet<AssetLocation>();
		List<AssetLocation> filesToSave = new List<AssetLocation>();
		for (int i = 0; i < validPatches.Count; i++)
		{
			(JsonPatch, AssetLocation, int) tuple = validPatches[i];
			JsonPatch patch2 = tuple.Item1;
			AssetLocation source = tuple.Item2;
			int index = tuple.Item3;
			AssetLocation targetFile = patch2.File.Clone();
			if (targetFile.Path.EndsWith('*'))
			{
				string basePath = targetFile.Path.TrimEnd('*');
				List<IAsset> assets = api.Assets.GetMany(basePath, targetFile.Domain, loadAsset: false);
				for (int j2 = 0; j2 < assets.Count; j2++)
				{
					ProcessSinglePatch(assets[j2].Location, patch2, source, index, ref appliedCount, ref notfoundCount, ref errorCount, ref processedFiles, ref filesToSave);
				}
			}
			else
			{
				ProcessSinglePatch(targetFile, patch2, source, index, ref appliedCount, ref notfoundCount, ref errorCount, ref processedFiles, ref filesToSave);
			}
		}
		foreach (AssetLocation fileLoc in filesToSave)
		{
			if (!jsonCache.TryGetValue(fileLoc, out (JToken, IAsset) cachedData))
			{
				continue;
			}
			try
			{
				StringBuilder sbFile = StringBuilderCache.Acquire();
				using (StringWriter writer = new StringWriter(sbFile))
				{
					using JsonTextWriter jsonWriter = new JsonTextWriter(writer);
					cachedData.Item1.WriteTo(jsonWriter);
				}
				cachedData.Item2.Data = Encoding.UTF8.GetBytes(StringBuilderCache.GetStringAndRelease(sbFile));
				cachedData.Item2.IsPatched = true;
			}
			catch (Exception e2)
			{
				api.World.Logger.Error("Failed to serialize JSON file {0}:", fileLoc);
				api.World.Logger.Error(e2);
				errorCount++;
			}
		}
		jsonCache?.Clear();
		filesToSave?.Clear();
		processedFiles?.Clear();
		validPatches?.Clear();
		entries?.Clear();
		StringBuilder sb = new StringBuilder();
		sb.Append("JsonPatch Loader: ");
		if (totalCount == 0)
		{
			sb.Append("Nothing to patch");
		}
		else
		{
			sb.Append($"{totalCount} patches total");
			if (appliedCount > 0)
			{
				sb.Append($", successfully applied {appliedCount} patches");
			}
			if (notfoundCount > 0)
			{
				sb.Append($", missing files on {notfoundCount} patches");
			}
			if (unmetConditionCount > 0)
			{
				sb.Append($", unmet conditions on {unmetConditionCount} patches");
			}
			if (errorCount > 0)
			{
				sb.Append($", had errors on {errorCount} patches");
			}
			else
			{
				sb.Append(", no errors");
			}
		}
		api.Logger.Notification(sb.ToString());
		api.Logger.VerboseDebug("Patchloader finished");
	}

	private void ProcessSinglePatch(AssetLocation fileLoc, JsonPatch patch, AssetLocation source, int index, ref int applied, ref int notFound, ref int errorCount, ref HashSet<AssetLocation> processedFiles, ref List<AssetLocation> filesToSave)
	{
		fileLoc.WithPathAppendixOnce(".json");
		if (!jsonCache.TryGetValue(fileLoc, out (JToken, IAsset) cachedData))
		{
			IAsset asset = api.Assets.TryGet(fileLoc);
			if (asset == null)
			{
				notFound++;
				LogFileNotFound(fileLoc, patch, source, index);
				return;
			}
			JToken token;
			try
			{
				token = JToken.Parse(asset.ToText());
			}
			catch (Exception e)
			{
				errorCount++;
				api.World.Logger.Error("Patch {0} (target: {2}) in {1} failed probably because the syntax of the value is broken:", index, source, fileLoc);
				api.World.Logger.Error(e);
				return;
			}
			cachedData = (token, asset);
			jsonCache[fileLoc] = cachedData;
		}
		Operation op = CreateOperation(patch, source, index, ref errorCount);
		if (op == null)
		{
			return;
		}
		PatchDocument patchdoc = new PatchDocument(op);
		try
		{
			patchdoc.ApplyTo(cachedData.Item1);
			applied++;
			if (!processedFiles.Contains(fileLoc))
			{
				processedFiles.Add(fileLoc);
				filesToSave.Add(fileLoc);
			}
		}
		catch (PathNotFoundException ex)
		{
			api.World.Logger.Error("Patch {0} (target: {4}) in {1} failed because supplied path {2} is invalid: {3}", index, source, patch.Path, ex.Message, fileLoc);
			errorCount++;
		}
		catch (Exception e2)
		{
			api.World.Logger.Error("Patch {0} (target: {2}) in {1} failed:", index, source, fileLoc);
			api.World.Logger.Error(e2);
			errorCount++;
		}
	}

	private void LogFileNotFound(AssetLocation loc, JsonPatch patch, AssetLocation source, int index)
	{
		if (patch.File.Category == null)
		{
			api.World.Logger.Error("Patch {0} in {1}: File {2} not found. Wrong asset category", index, source, loc);
			return;
		}
		EnumAppSide catSide = patch.File.Category.SideType;
		if (catSide != EnumAppSide.Universal && api.Side != catSide)
		{
			api.World.Logger.VerboseDebug("Patch {0} in {1}: File {2} not found. Hint: This asset is usually only loaded {3} side", index, source, loc, catSide);
		}
		else
		{
			api.World.Logger.Error("Patch {0} in {1}: File {2} not found", index, source, loc);
		}
	}

	private Operation CreateOperation(JsonPatch jsonPatch, AssetLocation source, int index, ref int errorCount)
	{
		switch (jsonPatch.Op)
		{
		case EnumJsonPatchOp.Add:
			if (jsonPatch.Value == null)
			{
				api.World.Logger.Error("Patch {0} in {1} failed: Add operation requires Value", index, source);
				errorCount++;
				return null;
			}
			return new AddReplaceOperation
			{
				Path = new JsonPointer(jsonPatch.Path),
				Value = jsonPatch.Value.Token
			};
		case EnumJsonPatchOp.AddEach:
			if (jsonPatch.Value == null)
			{
				api.World.Logger.Error("Patch {0} in {1} failed: AddEach operation requires Value", index, source);
				errorCount++;
				return null;
			}
			return new AddEachOperation
			{
				Path = new JsonPointer(jsonPatch.Path),
				Value = jsonPatch.Value.Token
			};
		case EnumJsonPatchOp.Remove:
			return new RemoveOperation
			{
				Path = new JsonPointer(jsonPatch.Path)
			};
		case EnumJsonPatchOp.Replace:
			if (jsonPatch.Value == null)
			{
				api.World.Logger.Error("Patch {0} in {1} failed: Replace operation requires Value", index, source);
				errorCount++;
				return null;
			}
			return new ReplaceOperation
			{
				Path = new JsonPointer(jsonPatch.Path),
				Value = jsonPatch.Value.Token
			};
		case EnumJsonPatchOp.Copy:
			return new CopyOperation
			{
				Path = new JsonPointer(jsonPatch.Path),
				FromPath = new JsonPointer(jsonPatch.FromPath)
			};
		case EnumJsonPatchOp.Move:
			return new MoveOperation
			{
				Path = new JsonPointer(jsonPatch.Path),
				FromPath = new JsonPointer(jsonPatch.FromPath)
			};
		case EnumJsonPatchOp.AddMerge:
			if (jsonPatch.Value == null)
			{
				api.World.Logger.Error("Patch {0} in {1} failed: AddMerge operation requires Value", index, source);
				errorCount++;
				return null;
			}
			return new AddMergeOperation
			{
				Path = new JsonPointer(jsonPatch.Path),
				Value = jsonPatch.Value.Token
			};
		default:
			return null;
		}
	}

	public void ApplyPatch(int patchIndex, AssetLocation patchSourcefile, JsonPatch jsonPatch, ref int applied, ref int notFound, ref int errorCount)
	{
		if (((!jsonPatch.Side.HasValue) ? jsonPatch.File.Category.SideType : jsonPatch.Side.Value) != EnumAppSide.Universal && jsonPatch.Side != api.Side)
		{
			return;
		}
		if (jsonPatch.File == null)
		{
			api.World.Logger.Error("Patch {0} in {1} failed because it is missing the target file property", patchIndex, patchSourcefile);
			return;
		}
		AssetLocation loc = jsonPatch.File.Clone();
		if (jsonPatch.File.Path.EndsWith('*'))
		{
			foreach (IAsset val in api.Assets.GetMany(jsonPatch.File.Path.TrimEnd('*'), jsonPatch.File.Domain, loadAsset: false))
			{
				jsonPatch.File = val.Location;
				ApplyPatch(patchIndex, patchSourcefile, jsonPatch, ref applied, ref notFound, ref errorCount);
			}
			jsonPatch.File = loc;
			return;
		}
		if (!loc.Path.EndsWithOrdinal(".json"))
		{
			loc.Path += ".json";
		}
		IAsset asset = api.Assets.TryGet(loc);
		if (asset == null)
		{
			if (jsonPatch.File.Category == null)
			{
				api.World.Logger.Error("Patch {0} in {1}: File {2} not found. Wrong asset category", patchIndex, patchSourcefile, loc);
			}
			else
			{
				EnumAppSide catSide = jsonPatch.File.Category.SideType;
				if (catSide != EnumAppSide.Universal && api.Side != catSide)
				{
					api.World.Logger.VerboseDebug("Patch {0} in {1}: File {2} not found. Hint: This asset is usually only loaded {3} side", patchIndex, patchSourcefile, loc, catSide);
				}
				else
				{
					api.World.Logger.Error("Patch {0} in {1}: File {2} not found", patchIndex, patchSourcefile, loc);
				}
			}
			notFound++;
			return;
		}
		Operation op = null;
		switch (jsonPatch.Op)
		{
		case EnumJsonPatchOp.Add:
			if (jsonPatch.Value == null)
			{
				api.World.Logger.Error("Patch {0} in {1} failed probably because it is an add operation and the value property is not set or misspelled", patchIndex, patchSourcefile);
				errorCount++;
				return;
			}
			op = new AddReplaceOperation
			{
				Path = new JsonPointer(jsonPatch.Path),
				Value = jsonPatch.Value.Token
			};
			break;
		case EnumJsonPatchOp.AddEach:
			if (jsonPatch.Value == null)
			{
				api.World.Logger.Error("Patch {0} in {1} failed probably because it is an add each operation and the value property is not set or misspelled", patchIndex, patchSourcefile);
				errorCount++;
				return;
			}
			op = new AddEachOperation
			{
				Path = new JsonPointer(jsonPatch.Path),
				Value = jsonPatch.Value.Token
			};
			break;
		case EnumJsonPatchOp.Remove:
			op = new RemoveOperation
			{
				Path = new JsonPointer(jsonPatch.Path)
			};
			break;
		case EnumJsonPatchOp.Replace:
			if (jsonPatch.Value == null)
			{
				api.World.Logger.Error("Patch {0} in {1} failed probably because it is a replace operation and the value property is not set or misspelled", patchIndex, patchSourcefile);
				errorCount++;
				return;
			}
			op = new ReplaceOperation
			{
				Path = new JsonPointer(jsonPatch.Path),
				Value = jsonPatch.Value.Token
			};
			break;
		case EnumJsonPatchOp.Copy:
			op = new CopyOperation
			{
				Path = new JsonPointer(jsonPatch.Path),
				FromPath = new JsonPointer(jsonPatch.FromPath)
			};
			break;
		case EnumJsonPatchOp.Move:
			op = new MoveOperation
			{
				Path = new JsonPointer(jsonPatch.Path),
				FromPath = new JsonPointer(jsonPatch.FromPath)
			};
			break;
		case EnumJsonPatchOp.AddMerge:
			op = new AddMergeOperation
			{
				Path = new JsonPointer(jsonPatch.Path),
				Value = jsonPatch.Value.Token
			};
			break;
		}
		PatchDocument patchdoc = new PatchDocument(op);
		JToken token;
		try
		{
			token = JToken.Parse(asset.ToText());
		}
		catch (Exception e)
		{
			api.World.Logger.Error("Patch {0} (target: {2}) in {1} failed probably because the syntax of the value is broken:", patchIndex, patchSourcefile, loc);
			api.World.Logger.Error(e);
			errorCount++;
			return;
		}
		try
		{
			patchdoc.ApplyTo(token);
		}
		catch (PathNotFoundException ex)
		{
			api.World.Logger.Error("Patch {0} (target: {4}) in {1} failed because supplied path {2} is invalid: {3}", patchIndex, patchSourcefile, jsonPatch.Path, ex.Message, loc);
			errorCount++;
			return;
		}
		catch (Exception e2)
		{
			api.World.Logger.Error("Patch {0} (target: {2}) in {1} failed, following Exception was thrown:", patchIndex, patchSourcefile, loc);
			api.World.Logger.Error(e2);
			errorCount++;
			return;
		}
		string text = token.ToString();
		asset.Data = Encoding.UTF8.GetBytes(text);
		asset.IsPatched = true;
		applied++;
	}
}
