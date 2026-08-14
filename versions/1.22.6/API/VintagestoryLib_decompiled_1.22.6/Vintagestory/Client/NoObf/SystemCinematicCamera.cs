using System;
using System.IO;
using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.Common.Entities;
using Vintagestory.API.Config;
using Vintagestory.API.MathTools;

namespace Vintagestory.Client.NoObf;

public class SystemCinematicCamera : ClientSystem
{
	protected readonly ClientPlatformAbstract platform;

	protected readonly CameraPoint[] cameraPoints;

	protected int cameraPointsCount;

	protected ClientWorldPlayerData prevWData;

	protected double previousPositionX;

	protected double previousPositionY;

	protected double previousPositionZ;

	protected double previousOrientationX;

	protected double previousOrientationY;

	protected double previousOrientationZ;

	protected double totalDistance;

	protected double totalTime;

	protected double currentLengthTraveled;

	protected int currentPoint;

	protected int currentLoop;

	protected bool cameraLive;

	protected bool teleportBack;

	protected int quantityLoops;

	protected bool closedPath;

	protected double alpha = 0.1;

	protected IAviWriter avi;

	protected double writeAccum;

	protected bool firstFrameDone;

	protected bool shouldDisableGui = true;

	protected MeshData cameraPathModel;

	protected MeshRef cameraPathModelRef;

	protected BlockPos origin;

	protected EnumAutoCamAngleMode camAngleMode = EnumAutoCamAngleMode.Point;

	protected string videoFileName = "";

	protected readonly string prevPathsFile = Path.Combine(GamePaths.Cache, "campaths.txt");

	public const int SegmentSampleRate = 60;

	protected readonly CameraKeyframe[] keyframesToSample = new CameraKeyframe[60];

	protected readonly CameraKeyframe[] currentSegmentKeyframes = new CameraKeyframe[60];

	protected readonly double[] time = new double[4];

	protected readonly CameraPoint[] points = new CameraPoint[4];

	protected readonly double[] pointsX = new double[4];

	protected readonly double[] pointsY = new double[4];

	protected readonly double[] pointsZ = new double[4];

	protected readonly double[] pointsPitch = new double[4];

	protected readonly double[] pointsYaw = new double[4];

	protected readonly double[] pointsRoll = new double[4];

	protected double tStart;

	protected double tEnd;

	public override string Name => "cica";

	public SystemCinematicCamera(ClientMain game)
		: base(game)
	{
		InitModel();
		platform = game.Platform;
		cameraPoints = new CameraPoint[256];
		cameraPointsCount = 0;
		CommandArgumentParsers parsers = game.api.ChatCommands.Parsers;
		game.api.ChatCommands.Create("cam").WithPreCondition(CheckPrecondition).BeginSubCommand("tp")
			.WithDescription("Whether to teleport the player back to where he was previously (default on)")
			.WithArgs(parsers.OptionalBool("teleportBack"))
			.HandleWith(OnCmdTp)
			.EndSubCommand()
			.BeginSubCommand("gui")
			.WithDescription("If set, will hide all guis during while the cam is on (default on)")
			.WithArgs(parsers.OptionalBool("shouldDisableGui"))
			.HandleWith(OnCmdGui)
			.EndSubCommand()
			.BeginSubCommand("loop")
			.WithDescription("If set then path be repeatedly played")
			.WithArgs(parsers.OptionalInt("quantityLoops", 9999999))
			.HandleWith(OnCmdLoop)
			.EndSubCommand()
			.BeginSubCommand("p")
			.WithDescription("Add a point to path")
			.HandleWith(OnCmdAddPoint)
			.EndSubCommand()
			.BeginSubCommand("up")
			.WithDescription("Update nth point")
			.WithArgs(parsers.OptionalInt("point_index"))
			.HandleWith(OnCmdUp)
			.EndSubCommand()
			.BeginSubCommand("goto")
			.WithDescription("Teleport to nth point")
			.WithArgs(parsers.OptionalInt("point_index"))
			.HandleWith(OnCmdGoto)
			.EndSubCommand()
			.BeginSubCommand("cp")
			.WithDescription("Close path")
			.HandleWith(OnCmdCp)
			.EndSubCommand()
			.BeginSubCommand("rp")
			.WithDescription("Remove the last point from the path")
			.HandleWith(OnCmdRemovePoint)
			.EndSubCommand()
			.BeginSubCommand("play")
			.WithAlias("start")
			.WithAlias("rec")
			.WithDescription("Play/start path or play and record to .avi file using rec command")
			.WithArgs(parsers.OptionalDouble("totalTime", 10.0))
			.HandleWith(OnCmdPlay)
			.EndSubCommand()
			.BeginSubCommand("stop")
			.WithDescription("Stop playing and recording")
			.HandleWith(OnCmdStop)
			.EndSubCommand()
			.BeginSubCommand("clear")
			.WithDescription("Remove all points from path")
			.HandleWith(OnCmdClear)
			.EndSubCommand()
			.BeginSubCommand("save")
			.WithDescription("Copy path points to clipboard")
			.HandleWith(OnCmdSave)
			.EndSubCommand()
			.BeginSubCommand("load")
			.WithDescription("Load point to the path, seperated by ,")
			.WithArgs(parsers.Word("points"))
			.HandleWith(OnCmdLoad)
			.EndSubCommand()
			.BeginSubCommand("loadold")
			.WithDescription("Load point to the path from a pre 1.20 point list, seperated by ,")
			.WithArgs(parsers.Word("points"))
			.HandleWith(OnCmdLoadOld)
			.EndSubCommand()
			.BeginSubCommand("alpha")
			.WithDescription("Set/Show alpha")
			.WithArgs(parsers.OptionalFloat("alpha"))
			.HandleWith(OnCmdAlpha)
			.EndSubCommand()
			.BeginSubCommand("angles")
			.WithDescription("Set camera angle mode [point, direction]")
			.WithArgs(parsers.OptionalWord("mode"))
			.HandleWith(OnCmdAngles)
			.EndSubCommand();
		game.eventManager.RegisterRenderer(OnRenderFrame3D, EnumRenderStage.Opaque, "cinecam", 0.7);
		game.eventManager.RegisterRenderer(OnFinalizeFrame, EnumRenderStage.Done, "cinecam-done", 0.98);
	}

	protected TextCommandResult CheckPrecondition(TextCommandCallingArgs args)
	{
		IPlayer plr = args.Caller.Player;
		if (plr != null && (plr.WorldData.CurrentGameMode == EnumGameMode.Creative || plr.WorldData.CurrentGameMode == EnumGameMode.Spectator))
		{
			return TextCommandResult.Success();
		}
		return TextCommandResult.Error("Only available in creative or spectator mode");
	}

	protected TextCommandResult OnCmdAngles(TextCommandCallingArgs args)
	{
		camAngleMode = ((args.Parsers[0].IsMissing || args[0] as string == "point") ? EnumAutoCamAngleMode.Point : EnumAutoCamAngleMode.Direction);
		return TextCommandResult.Success("Camera angle mode is now " + camAngleMode);
	}

	protected TextCommandResult OnCmdAlpha(TextCommandCallingArgs args)
	{
		if (args.Parsers[0].IsMissing)
		{
			return TextCommandResult.Success("Current alpha is " + alpha);
		}
		alpha = (float)args[0];
		GenerateCameraPathModel();
		return TextCommandResult.Success("Alpha set to " + alpha);
	}

	protected TextCommandResult OnCmdStop(TextCommandCallingArgs args)
	{
		StopAutoCamera();
		return TextCommandResult.Success();
	}

	protected TextCommandResult OnCmdPlay(TextCommandCallingArgs args)
	{
		if (cameraPointsCount < 2)
		{
			game.ShowChatMessage("Need at least 2 points!");
		}
		else
		{
			StartAutoCamera(args);
		}
		return TextCommandResult.Success();
	}

	protected TextCommandResult OnCmdLoop(TextCommandCallingArgs args)
	{
		quantityLoops = (int)args[0];
		return TextCommandResult.Success("Will loop " + quantityLoops + " times.");
	}

	protected TextCommandResult OnCmdGui(TextCommandCallingArgs args)
	{
		shouldDisableGui = (bool)args[0];
		return TextCommandResult.Success("Disable guis now " + (shouldDisableGui ? "on" : "off"));
	}

	protected TextCommandResult OnCmdTp(TextCommandCallingArgs args)
	{
		teleportBack = (bool)args[0];
		return TextCommandResult.Success("Teleport back to previous position now " + (teleportBack ? "on" : "off"));
	}

	protected void InitModel()
	{
		cameraPathModel = new MeshData(4, 4, withNormals: false, withUv: false);
		cameraPathModel.SetMode(EnumDrawMode.LineStrip);
		cameraPathModelRef = null;
	}

	public void OnRenderFrame3D(float deltaTime)
	{
		if (game.ShouldRender2DOverlays && cameraPathModelRef != null)
		{
			ShaderProgramAutocamera autocamera = ShaderPrograms.Autocamera;
			autocamera.Use();
			game.Platform.GLLineWidth(2f);
			game.Platform.BindTexture2d(0);
			game.GlPushMatrix();
			game.GlLoadMatrix(game.MainCamera.CameraMatrixOrigin);
			Vec3d cameraPos = game.EntityPlayer.CameraPos;
			game.GlTranslate((float)((double)origin.X - cameraPos.X), (float)((double)origin.Y - cameraPos.Y), (float)((double)origin.Z - cameraPos.Z));
			autocamera.ProjectionMatrix = game.CurrentProjectionMatrix;
			autocamera.ModelViewMatrix = game.CurrentModelViewMatrix;
			game.Platform.RenderMesh(cameraPathModelRef);
			game.GlPopMatrix();
			autocamera.Stop();
		}
	}

	protected TextCommandResult OnCmdLoad(TextCommandCallingArgs args)
	{
		return TextCommandResult.Success(string.Format("Camera points loaded: {0}", Load(args).ToString() ?? ""));
	}

	protected TextCommandResult OnCmdLoadOld(TextCommandCallingArgs args)
	{
		return TextCommandResult.Success(string.Format("Camera points loaded: {0}", Load(args, (float)Math.PI / 2f).ToString() ?? ""));
	}

	protected int Load(TextCommandCallingArgs args, float yawOffset = 0f)
	{
		string[] points = (args[0] as string).Split(',');
		int n = (points.Length - 1) / 6;
		cameraPointsCount = 0;
		origin = game.EntityPlayer.Pos.AsBlockPos;
		for (int i = 0; i < n; i++)
		{
			CameraPoint point = new CameraPoint
			{
				x = (float)int.Parse(points[1 + i * 6]) / 100f,
				y = (float)int.Parse(points[1 + i * 6 + 1]) / 100f,
				z = (float)int.Parse(points[1 + i * 6 + 2]) / 100f,
				pitch = (float)int.Parse(points[1 + i * 6 + 3]) / 1000f,
				yaw = (float)int.Parse(points[1 + i * 6 + 4]) / 1000f + yawOffset,
				roll = (float)int.Parse(points[1 + i * 6 + 5]) / 1000f
			};
			if (cameraPointsCount - 1 >= 0 && cameraPoints[cameraPointsCount - 1].PositionEquals(point))
			{
				point.x += 0.0010000000474974513;
			}
			cameraPoints[cameraPointsCount++] = point;
		}
		GenerateCameraPathModel();
		return n;
	}

	protected TextCommandResult OnCmdSave(TextCommandCallingArgs args)
	{
		string s = PointsToString();
		platform.XPlatInterface.SetClipboardText(s);
		return TextCommandResult.Success("Camera points copied to clipboard.");
	}

	protected string PointsToString()
	{
		string s = "1,";
		for (int i = 0; i < cameraPointsCount; i++)
		{
			CameraPoint point = cameraPoints[i];
			s = string.Format("{0}{1},", s, ((int)(point.x * 100.0)).ToString() ?? "");
			s = string.Format("{0}{1},", s, ((int)(point.y * 100.0)).ToString() ?? "");
			s = string.Format("{0}{1},", s, ((int)(point.z * 100.0)).ToString() ?? "");
			s = string.Format("{0}{1},", s, ((int)(point.pitch * 1000f)).ToString() ?? "");
			s = string.Format("{0}{1},", s, ((int)(point.yaw * 1000f)).ToString() ?? "");
			s = string.Format("{0}{1}", s, ((int)(point.roll * 1000f)).ToString() ?? "");
			if (i != cameraPointsCount - 1)
			{
				s = $"{s},";
			}
		}
		return s;
	}

	protected TextCommandResult OnCmdAddPoint(TextCommandCallingArgs args)
	{
		if (cameraPointsCount == 0)
		{
			origin = game.EntityPlayer.Pos.AsBlockPos;
		}
		CameraPoint point = CameraPoint.FromEntityPos(game.EntityPlayer.Pos);
		if (cameraPointsCount - 1 >= 0 && cameraPoints[cameraPointsCount - 1].PositionEquals(point))
		{
			point.x += 0.0010000000474974513;
		}
		cameraPoints[cameraPointsCount++] = point;
		if (cameraPointsCount > 1)
		{
			FixYaw(cameraPointsCount);
		}
		closedPath = false;
		GenerateCameraPathModel();
		return TextCommandResult.Success("Point added");
	}

	protected TextCommandResult OnCmdUp(TextCommandCallingArgs args)
	{
		int num = (int)args[0];
		if (num < 0 || num >= cameraPointsCount)
		{
			return TextCommandResult.Success("Your supplied number is above the point count or negative");
		}
		CameraPoint point = CameraPoint.FromEntityPos(game.EntityPlayer.Pos);
		cameraPoints[num] = point;
		if (cameraPointsCount > 1)
		{
			FixYaw(cameraPointsCount);
		}
		GenerateCameraPathModel();
		return TextCommandResult.Success("Point updated");
	}

	protected TextCommandResult OnCmdGoto(TextCommandCallingArgs args)
	{
		int num = (int)args[0];
		if (num < 0 || num >= cameraPointsCount)
		{
			return TextCommandResult.Success("Your supplied number is above the point count or negative");
		}
		CameraPoint p = cameraPoints[num];
		game.EntityPlayer.Pos.X = p.x;
		game.EntityPlayer.Pos.Y = p.y;
		game.EntityPlayer.Pos.Z = p.z;
		ClientMain clientMain = game;
		float mouseYaw = (game.EntityPlayer.Pos.Yaw = p.yaw);
		clientMain.mouseYaw = mouseYaw;
		ClientMain clientMain2 = game;
		mouseYaw = (game.EntityPlayer.Pos.Pitch = p.pitch);
		clientMain2.mousePitch = mouseYaw;
		game.EntityPlayer.Pos.Roll = p.roll;
		return TextCommandResult.Success();
	}

	protected TextCommandResult OnCmdCp(TextCommandCallingArgs args)
	{
		if (cameraPointsCount <= 1)
		{
			return TextCommandResult.Success("Requires at least 2 points");
		}
		cameraPoints[cameraPointsCount++] = cameraPoints[0].Clone();
		FixYaw(cameraPointsCount);
		game.ShowChatMessage("Path Closed");
		closedPath = true;
		GenerateCameraPathModel();
		return TextCommandResult.Success();
	}

	protected void FixYaw(int pos)
	{
		double prevYaw = cameraPoints[pos - 2].yaw;
		double num = (double)cameraPoints[pos - 1].yaw - prevYaw;
		if (num > Math.PI)
		{
			cameraPoints[pos - 1].yaw -= (float)Math.PI * 2f;
		}
		if (num < -Math.PI)
		{
			cameraPoints[pos - 1].yaw += (float)Math.PI * 2f;
		}
	}

	protected TextCommandResult OnCmdRemovePoint(TextCommandCallingArgs args)
	{
		cameraPointsCount = Math.Max(0, cameraPointsCount - 1);
		closedPath = false;
		GenerateCameraPathModel();
		return TextCommandResult.Success("Point removed");
	}

	protected TextCommandResult OnCmdClear(TextCommandCallingArgs args)
	{
		closedPath = false;
		cameraPointsCount = 0;
		StopAutoCamera();
		InitModel();
		return TextCommandResult.Success("Camera points cleared.");
	}

	protected void StartAutoCamera(TextCommandCallingArgs args)
	{
		if (!game.AllowFreemove)
		{
			game.ShowChatMessage("Free move not allowed.");
			return;
		}
		if (cameraPointsCount == 0)
		{
			game.ShowChatMessage("No points defined. Enter points with \".cam p\" Command.");
			return;
		}
		ClientWorldPlayerData wdata = game.player.worlddata;
		prevWData = wdata.Clone();
		game.player.worlddata.RequestMode(game, wdata.MoveSpeedMultiplier, wdata.PickingRange, EnumGameMode.Spectator, freeMove: true, noClip: true, EnumFreeMovAxisLock.None, ClientSettings.RenderMetaBlocks);
		currentPoint = 0;
		totalDistance = CalculateApproximateDistances();
		if (args.SubCmdCode == "rec")
		{
			string path = ClientSettings.VideoFileTarget ?? GamePaths.Videos;
			try
			{
				if (new DirectoryInfo(path).Parent != null && !Directory.Exists(path))
				{
					Directory.CreateDirectory(path);
				}
				avi = platform.CreateAviWriter(ClientSettings.RecordingFrameRate, ClientSettings.RecordingCodec);
				string time = $"{DateTime.Now:yyyy-MM-dd_HH-mm-ss}";
				avi.Open(Path.Combine(path, videoFileName = time + ".avi"), game.Width, game.Height);
			}
			catch (Exception ex)
			{
				game.ShowChatMessage("Could not start recording: " + ex.Message);
				return;
			}
			if (ClientSettings.GameTickFrameRate > 0f)
			{
				game.DeltaTimeLimiter = 1f / ClientSettings.GameTickFrameRate;
			}
		}
		totalTime = (double)args[0];
		firstFrameDone = false;
		currentPoint = 0;
		currentLoop = 0;
		currentLengthTraveled = 0.0;
		EntityPos pos = game.EntityPlayer.Pos;
		previousPositionX = pos.X;
		previousPositionY = pos.Y;
		previousPositionZ = pos.Z;
		previousOrientationX = pos.Pitch;
		previousOrientationY = pos.Yaw;
		previousOrientationZ = pos.Roll;
		if (shouldDisableGui)
		{
			game.ShouldRender2DOverlays = false;
		}
		game.AllowCameraControl = false;
		game.MainCamera.UpdateCameraPos = false;
		PrecalcCurrentPoint();
		SetupCameraKeyframesForCurrentPoint();
		cameraLive = true;
	}

	protected void StopAutoCamera()
	{
		if (shouldDisableGui)
		{
			game.ShouldRender2DOverlays = true;
		}
		game.AllowCameraControl = true;
		game.MainCamera.UpdateCameraPos = true;
		if (cameraLive)
		{
			game.player.worlddata.RequestMode(game, prevWData.MoveSpeedMultiplier, prevWData.PickingRange, prevWData.CurrentGameMode, prevWData.FreeMove, prevWData.NoClip, prevWData.FreeMovePlaneLock, ClientSettings.RenderMetaBlocks);
			if (teleportBack)
			{
				EntityPos pos = game.EntityPlayer.Pos;
				pos.X = previousPositionX;
				pos.Y = previousPositionY;
				pos.Z = previousPositionZ;
				pos.Pitch = (float)previousOrientationX;
				pos.Yaw = (float)previousOrientationY;
				pos.Roll = (float)previousOrientationZ;
			}
		}
		if (avi != null)
		{
			avi.Close();
			avi = null;
			string path = ClientSettings.VideoFileTarget ?? GamePaths.Videos;
			game.ShowChatMessage(videoFileName + " saved to " + path);
		}
		cameraLive = false;
		game.DeltaTimeLimiter = -1f;
	}

	public void OnFinalizeFrame(float dt)
	{
		if (!cameraLive || game.IsPaused)
		{
			return;
		}
		UpdateAvi(dt);
		if (currentPoint == cameraPointsCount - 1)
		{
			StopAutoCamera();
			File.AppendAllText(prevPathsFile, PointsToString() + "\r\n");
			game.ShowChatMessage("Camera path saved to Cache folder, in case you lose it");
			return;
		}
		double travelSpeed = totalDistance / totalTime;
		currentLengthTraveled += travelSpeed * (double)dt;
		double percentTraveled = currentLengthTraveled / points[1].distance;
		if (percentTraveled > 1.0)
		{
			double rollOverDistance = currentLengthTraveled - points[1].distance;
			currentPoint++;
			if (currentPoint != cameraPointsCount - 1 || currentLoop < quantityLoops)
			{
				if (currentLoop < quantityLoops && currentPoint == cameraPointsCount - 1)
				{
					currentPoint = 0;
					currentLoop++;
				}
				PrecalcCurrentPoint();
				SetupCameraKeyframesForCurrentPoint();
				currentLengthTraveled = rollOverDistance;
				percentTraveled = currentLengthTraveled / points[1].distance;
			}
		}
		AdvanceTo(Math.Min(1.0, percentTraveled));
	}

	protected void SetupCameraKeyframesForCurrentPoint()
	{
		for (int i = 0; i < 60; i++)
		{
			double dt = (float)i / 59f;
			double x = GameMath.CPCatmullRomSplineLerp(tStart + dt * (tEnd - tStart), pointsX, time);
			double y = GameMath.CPCatmullRomSplineLerp(tStart + dt * (tEnd - tStart), pointsY, time);
			double z = GameMath.CPCatmullRomSplineLerp(tStart + dt * (tEnd - tStart), pointsZ, time);
			double pitch = GameMath.CPCatmullRomSplineLerp(tStart + dt * (tEnd - tStart), pointsPitch, time);
			double yaw = GameMath.CPCatmullRomSplineLerp(tStart + dt * (tEnd - tStart), pointsYaw, time);
			double roll = GameMath.CPCatmullRomSplineLerp(tStart + dt * (tEnd - tStart), pointsRoll, time);
			keyframesToSample[i] = new CameraKeyframe
			{
				X = x,
				Y = y,
				Z = z,
				Pitch = pitch,
				Yaw = yaw,
				Roll = roll
			};
		}
		CameraKeyframe[] array = currentSegmentKeyframes;
		array[0] = keyframesToSample[0];
		array[59] = keyframesToSample[^1];
		double segmentLength = 0.0;
		for (int j = 1; j < keyframesToSample.Length; j++)
		{
			CameraKeyframe prev = keyframesToSample[j - 1];
			CameraKeyframe cameraKeyframe = keyframesToSample[j];
			double dx = cameraKeyframe.X - prev.X;
			double dy = cameraKeyframe.Y - prev.Y;
			double dz = cameraKeyframe.Z - prev.Z;
			segmentLength += Math.Sqrt(dx * dx + dy * dy + dz * dz);
		}
		double distancePerKeyframe = segmentLength / 59.0;
		double accumulatedDistance = 0.0;
		int currentKeyframeIndex = 1;
		for (int k = 1; k < keyframesToSample.Length; k++)
		{
			CameraKeyframe prev2 = keyframesToSample[k - 1];
			CameraKeyframe curr = keyframesToSample[k];
			double dx2 = curr.X - prev2.X;
			double dy2 = curr.Y - prev2.Y;
			double dz2 = curr.Z - prev2.Z;
			double dist = Math.Sqrt(dx2 * dx2 + dy2 * dy2 + dz2 * dz2);
			accumulatedDistance += dist;
			while (accumulatedDistance >= distancePerKeyframe && currentKeyframeIndex < 60)
			{
				double overshoot = accumulatedDistance - distancePerKeyframe;
				double ratio = (dist - overshoot) / dist;
				currentSegmentKeyframes[currentKeyframeIndex] = new CameraKeyframe
				{
					X = prev2.X + dx2 * ratio,
					Y = prev2.Y + dy2 * ratio,
					Z = prev2.Z + dz2 * ratio,
					Pitch = prev2.Pitch + (curr.Pitch - prev2.Pitch) * ratio,
					Yaw = prev2.Yaw + (curr.Yaw - prev2.Yaw) * ratio,
					Roll = prev2.Roll + (curr.Roll - prev2.Roll) * ratio
				};
				currentKeyframeIndex++;
				accumulatedDistance -= distancePerKeyframe;
			}
		}
	}

	public void AdvanceTo(double percent)
	{
		int keyframeIndex = (int)(percent * 59.0);
		EntityPos pos = game.EntityPlayer.Pos;
		Vec3d cameraPos = game.EntityPlayer.CameraPos;
		CameraKeyframe outKeyframe;
		if (keyframeIndex >= 59)
		{
			outKeyframe = currentSegmentKeyframes[59];
		}
		else
		{
			CameraKeyframe current = currentSegmentKeyframes[keyframeIndex];
			CameraKeyframe next = currentSegmentKeyframes[keyframeIndex + 1];
			outKeyframe = new CameraKeyframe
			{
				X = current.X + (next.X - current.X) * (percent * 59.0 - (double)keyframeIndex),
				Y = current.Y + (next.Y - current.Y) * (percent * 59.0 - (double)keyframeIndex),
				Z = current.Z + (next.Z - current.Z) * (percent * 59.0 - (double)keyframeIndex),
				Pitch = current.Pitch + (next.Pitch - current.Pitch) * (percent * 59.0 - (double)keyframeIndex),
				Yaw = current.Yaw + (next.Yaw - current.Yaw) * (percent * 59.0 - (double)keyframeIndex),
				Roll = current.Roll + (next.Roll - current.Roll) * (percent * 59.0 - (double)keyframeIndex)
			};
		}
		double prevX = pos.X;
		double prevY = pos.Y;
		double prevZ = pos.Z;
		double x = (pos.X = outKeyframe.X);
		cameraPos.X = x;
		x = (pos.Y = outKeyframe.Y);
		cameraPos.Y = x;
		x = (pos.Z = outKeyframe.Z);
		cameraPos.Z = x;
		cameraPos.Add((double)game.MainCamera.CameraShakeStrength * GameMath.Cos(game.MainCamera.deltaSum * 100.0) / 10.0, (double)(0f - game.MainCamera.CameraShakeStrength) * GameMath.Cos(game.MainCamera.deltaSum * 100.0) / 10.0, (double)game.MainCamera.CameraShakeStrength * GameMath.Sin(game.MainCamera.deltaSum * 100.0) / 10.0);
		if (camAngleMode == EnumAutoCamAngleMode.Point)
		{
			ClientMain clientMain = game;
			float mousePitch = (pos.Pitch = (float)outKeyframe.Pitch);
			clientMain.mousePitch = mousePitch;
			ClientMain clientMain2 = game;
			mousePitch = (pos.Yaw = (float)outKeyframe.Yaw);
			clientMain2.mouseYaw = mousePitch;
			pos.Roll = (float)outKeyframe.Roll;
			return;
		}
		double dx = pos.X - prevX;
		double dy = pos.Y - prevY;
		double dz = pos.Z - prevZ;
		double length = Math.Sqrt(dx * dx + dy * dy + dz * dz);
		if (length > 0.0)
		{
			double np = GameMath.Asin(dy / length);
			double ny = Math.Atan2(dx, dz) + 1.5707963705062866;
			pos.Pitch += (float)Math.Min(0.03, np - (double)(pos.Pitch % ((float)Math.PI * 2f)));
			pos.Yaw += (float)Math.Min(0.03, ny - (double)(pos.Yaw % ((float)Math.PI * 2f)));
		}
	}

	protected void PrecalcCurrentPoint()
	{
		points[1] = cameraPoints[currentPoint];
		points[2] = cameraPoints[currentPoint + 1];
		points[0] = ((currentPoint > 0) ? cameraPoints[currentPoint - 1] : (closedPath ? cameraPoints[cameraPointsCount - 2] : points[1].ExtrapolateFrom(points[2], 1)));
		points[3] = ((currentPoint < cameraPointsCount - 2) ? cameraPoints[currentPoint + 2] : (closedPath ? cameraPoints[1] : points[2].ExtrapolateFrom(points[1], 1)));
		time[0] = 0.0;
		time[1] = 1.0;
		time[2] = 2.0;
		time[3] = 3.0;
		double total = 0.0;
		for (int i = 1; i < 4; i++)
		{
			double num = points[i].x - points[i - 1].x;
			double dy = points[i].y - points[i - 1].y;
			double dz = points[i].z - points[i - 1].z;
			double dt = Math.Pow(num * num + dy * dy + dz * dz, alpha);
			total += dt;
			time[i] = total;
		}
		for (int j = 0; j < 4; j++)
		{
			pointsX[j] = points[j].x;
			pointsY[j] = points[j].y;
			pointsZ[j] = points[j].z;
			pointsPitch[j] = points[j].pitch;
			pointsYaw[j] = points[j].yaw;
			pointsRoll[j] = points[j].roll;
		}
		tStart = time[1];
		tEnd = time[2];
	}

	protected void UpdateAvi(float dt)
	{
		if (avi == null)
		{
			return;
		}
		if (!firstFrameDone)
		{
			firstFrameDone = true;
			return;
		}
		writeAccum += dt;
		float frameRate = ClientSettings.RecordingFrameRate;
		if (writeAccum >= (double)(1f / frameRate))
		{
			writeAccum -= 1f / frameRate;
			avi.AddFrame();
		}
	}

	protected double CalculateApproximateDistances()
	{
		double totalDistance = 0.0;
		Vec3d cur = new Vec3d();
		Vec3d prev = new Vec3d();
		int currentPointBefore = currentPoint;
		for (currentPoint = 0; currentPoint < cameraPointsCount - 1; currentPoint++)
		{
			PrecalcCurrentPoint();
			CameraPoint point = cameraPoints[currentPoint];
			prev.X = point.x;
			prev.Y = point.y;
			prev.Z = point.z;
			double distance = 0.0;
			for (int i = 0; i < 60; i++)
			{
				double dt = (float)i / 60f;
				cur.X = GameMath.CPCatmullRomSplineLerp(tStart + dt * (tEnd - tStart), pointsX, time);
				cur.Y = GameMath.CPCatmullRomSplineLerp(tStart + dt * (tEnd - tStart), pointsY, time);
				cur.Z = GameMath.CPCatmullRomSplineLerp(tStart + dt * (tEnd - tStart), pointsZ, time);
				distance += Math.Sqrt(cur.SquareDistanceTo(prev));
				prev.X = cur.X;
				prev.Y = cur.Y;
				prev.Z = cur.Z;
			}
			point.distance = distance;
			totalDistance += point.distance;
		}
		currentPoint = currentPointBefore;
		return totalDistance;
	}

	protected void GenerateCameraPathModel()
	{
		InitModel();
		int vertexIndex = 0;
		for (currentPoint = 0; currentPoint < cameraPointsCount - 1; currentPoint++)
		{
			PrecalcCurrentPoint();
			for (int i = 0; i <= 30; i++)
			{
				double dt = (float)i / 30f;
				double x = GameMath.CPCatmullRomSplineLerp(tStart + dt * (tEnd - tStart), pointsX, time);
				double y = GameMath.CPCatmullRomSplineLerp(tStart + dt * (tEnd - tStart), pointsY, time);
				double z = GameMath.CPCatmullRomSplineLerp(tStart + dt * (tEnd - tStart), pointsZ, time);
				cameraPathModel.AddVertexSkipTex((float)(x - (double)origin.X), (float)(y - (double)origin.Y), (float)(z - (double)origin.Z), (currentPoint % 2 == 0) ? (-1) : ColorUtil.ToRgba(255, 255, 50, 50));
				cameraPathModel.AddIndex(vertexIndex++);
			}
		}
		currentPoint = 0;
		if (cameraPathModelRef != null)
		{
			platform.DeleteMesh(cameraPathModelRef);
		}
		cameraPathModelRef = platform.UploadMesh(cameraPathModel);
	}

	public override EnumClientSystemType GetSystemType()
	{
		return EnumClientSystemType.Misc;
	}
}
