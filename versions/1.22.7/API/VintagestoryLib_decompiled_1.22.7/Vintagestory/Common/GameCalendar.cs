using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using Vintagestory.API.Common;
using Vintagestory.API.Config;
using Vintagestory.API.MathTools;

namespace Vintagestory.Common;

public class GameCalendar : IGameCalendar
{
	protected float currentSpeedOfTime = 60f;

	public float HoursPerDay = 24f;

	public int MoonOrbitDays = 8;

	public float DayLengthInRealLifeSeconds;

	internal Stopwatch watchIngameTime;

	protected TimeSpan timespan = TimeSpan.Zero;

	protected long totalSecondsStart;

	public Size2i sunLightTextureSize;

	public int[] sunLightTexture;

	protected Dictionary<string, float> timeSpeedModifiers = new Dictionary<string, float>();

	protected NormalizedSimplexNoise sunsetModNoise;

	protected NormalizedSimplexNoise superMoonNoise;

	protected float superMoonSize;

	protected double moonPhaseCached;

	private double moonTauCached;

	private double moonSinDeltaCached;

	protected float timelapse;

	private float calendarSpeedMul = 0.5f;

	public static float[] MoonBrightnesByPhase = new float[8] { -0.1f, 0.1f, 0.2f, 0.26f, 0.33f, 0.26f, 0.2f, 0.1f };

	public static float[] MoonSizeByPhase = new float[8] { 0.8f, 0.9f, 1f, 1.1f, 1.2f, 1.1f, 1f, 0.9f };

	public Dictionary<string, float> TimeSpeedModifiers
	{
		get
		{
			return timeSpeedModifiers;
		}
		set
		{
			timeSpeedModifiers = value;
			CalculateCurrentTimeSpeed();
		}
	}

	public float SpeedOfTime => currentSpeedOfTime;

	public float CalendarSpeedMul
	{
		get
		{
			return calendarSpeedMul;
		}
		set
		{
			calendarSpeedMul = value;
			CalculateCurrentTimeSpeed();
		}
	}

	public float Timelapse
	{
		get
		{
			return timelapse;
		}
		set
		{
			timelapse = value;
		}
	}

	public int DaysPerMonth { get; set; } = 9;

	public int DaysPerYear => DaysPerMonth * 12;

	public int DayOfMonth => (int)(TotalDays % (double)DaysPerMonth) + 1;

	public int MonthsPerYear => DaysPerYear / DaysPerMonth;

	public int FullHourOfDay => (int)(timespan.TotalHours % (double)HoursPerDay);

	public float HourOfDay => (float)(timespan.TotalHours % (double)HoursPerDay);

	public long ElapsedSeconds => (long)(timespan.TotalSeconds - (double)totalSecondsStart);

	public double ElapsedHours => (double)ElapsedSeconds / 60.0 / 60.0;

	public double ElapsedDays => ElapsedHours / (double)HoursPerDay;

	public long TotalSeconds => (long)timespan.TotalSeconds;

	public double TotalHours => timespan.TotalHours;

	public double TotalDays => timespan.TotalHours / (double)HoursPerDay + (double)timelapse;

	public int DayOfYear => (int)(TotalDays % (double)DaysPerYear);

	public float DayOfYearf => (float)(TotalDays % (double)DaysPerYear);

	public int Year => (int)(TotalDays / (double)DaysPerYear);

	public int Month => (int)Math.Ceiling(YearRel * (float)MonthsPerYear);

	public float Monthf => YearRel * (float)MonthsPerYear;

	public EnumMonth MonthName => (EnumMonth)Month;

	public float YearRel => (float)(GameMath.Mod(TotalDays, DaysPerYear) / (double)DaysPerYear);

	int IGameCalendar.DaysPerYear => DaysPerYear;

	float IGameCalendar.HoursPerDay => HoursPerDay;

	public EnumMoonPhase MoonPhase => (EnumMoonPhase)((int)MoonPhaseExact % MoonOrbitDays);

	public double MoonPhaseExact => moonPhaseCached;

	public bool Dusk => (double)(HourOfDay / HoursPerDay) > 0.5;

	public float MoonPhaseBrightness
	{
		get
		{
			double ph = MoonPhaseExact;
			float l = (float)ph - (float)(int)ph;
			float num = MoonBrightnesByPhase[(int)ph];
			float b = MoonBrightnesByPhase[(int)(ph + 1.0) % MoonOrbitDays];
			return num * (1f - l) + b * l;
		}
	}

	public float MoonSize
	{
		get
		{
			double ph = MoonPhaseExact;
			float l = (float)ph - (float)(int)ph;
			float num = MoonSizeByPhase[(int)ph];
			float b = MoonSizeByPhase[(int)(ph + 1.0) % MoonOrbitDays];
			return (num * (1f - l) + b * l) * superMoonSize;
		}
	}

	public float SunsetMod { get; protected set; }

	public bool IsRunning => watchIngameTime.IsRunning;

	public SolarSphericalCoordsDelegate OnGetSolarSphericalCoords { get; set; }

	public HemisphereDelegate OnGetHemisphere { get; set; }

	public GetLatitudeDelegate OnGetLatitude { get; set; }

	public float? SeasonOverride { get; set; }

	public GameCalendar(IAsset sunlightTexture, int worldSeed, long totalSecondsStart = 4176000L)
	{
		OnGetSolarSphericalCoords = (double posX, double posZ, float yearRel, float dayRel) => new SolarSphericalCoords((float)Math.PI * 2f * GameMath.Mod(HourOfDay / HoursPerDay, 1f) - (float)Math.PI, 0f);
		OnGetLatitude = (double posZ) => 0.5;
		watchIngameTime = new Stopwatch();
		this.totalSecondsStart = totalSecondsStart;
		timespan = TimeSpan.FromSeconds(totalSecondsStart);
		timeSpeedModifiers["baseline"] = 60f;
		BitmapRef bmp = BitmapCreateFromPng(sunlightTexture);
		sunLightTexture = bmp.Pixels;
		sunLightTextureSize = new Size2i(bmp.Width, bmp.Height);
		bmp.Dispose();
		sunsetModNoise = NormalizedSimplexNoise.FromDefaultOctaves(4, 1.0, 0.9, worldSeed);
		superMoonNoise = NormalizedSimplexNoise.FromDefaultOctaves(4, 1.0, 0.9, worldSeed + 12098);
	}

	public double GetMoonPhase(double totaldays)
	{
		double raSun = GetSunAscension(totaldays);
		double sinDelta;
		return ((double)(GameMath.Mod((float)(GetMoonAscensionDeclination(totaldays, out sinDelta) - raSun) / ((float)Math.PI * 2f), 1f) * (float)MoonOrbitDays) + 0.5) % (double)MoonOrbitDays;
	}

	public double GetSunAscension(double totaldays)
	{
		double yearRel = GameMath.Mod(totaldays, DaysPerYear) / (double)DaysPerYear;
		double dayRel = GameMath.Mod(totaldays, 1.0);
		return (GameMath.Mod(totaldays - yearRel, 1.0) - (dayRel - 0.5)) * 6.2831854820251465;
	}

	public SolarSphericalCoords GetCelestialAngles(double x, double z, double totalDays)
	{
		float yearRel = (float)(GameMath.Mod(totalDays, DaysPerYear) / (double)DaysPerYear);
		float dayRel = (float)GameMath.Mod(totalDays, 1.0);
		return OnGetSolarSphericalCoords(x, z, yearRel, dayRel);
	}

	public Vec3f GetSunPosition(Vec3d pos, double totalDays)
	{
		SolarSphericalCoords celestialAngles = GetCelestialAngles(pos.X, pos.Z, totalDays);
		float phi = celestialAngles.AzimuthAngle;
		float theta = celestialAngles.ZenithAngle;
		float sinTheta = GameMath.Sin(theta);
		return new Vec3f(sinTheta * GameMath.Sin(phi), GameMath.Cos(theta), sinTheta * GameMath.Cos(phi));
	}

	public Vec3f GetMoonPosition(Vec3d position, double totalDays)
	{
		return GetMoonPosition(position.Z, totalDays);
	}

	protected Vec3f GetMoonPosition(double posZ, double totalDays)
	{
		double sinh;
		double az = GetMoonCelestialAngles(posZ, totalDays, out sinh);
		double cosh = Math.Sqrt(1.0 - sinh * sinh);
		return new Vec3f((float)(cosh * Math.Sin(az)), (float)sinh, (float)(cosh * Math.Cos(az)));
	}

	protected Vec3f GetMoonPosition(double posZ)
	{
		double sinh;
		double az = GetMoonCelestialAnglesFromCache(posZ, out sinh);
		double cosh = Math.Sqrt(1.0 - sinh * sinh);
		return new Vec3f((float)(cosh * Math.Sin(az)), (float)sinh, (float)(cosh * Math.Cos(az)));
	}

	private double GetMoonAscensionDeclination(double totalDays, out double sinDelta)
	{
		double T = 1386.0 + (totalDays - 3.0) / (double)DaysPerYear;
		double num = GameMath.Mod(218.3164477 + 4812.6788123421 * T, 360.0);
		double M = GameMath.Mod(134.9634114 + 4771.988675605 * T, 360.0) * (Math.PI / 180.0);
		double F = GameMath.Mod(93.2720906 + 4832.020175273 * T, 360.0) * (Math.PI / 180.0);
		double D = (297.8501921 + 4452.671114034 * T) * (Math.PI / 180.0);
		double eL = num + 6.289 * Math.Sin(M) - 1.274 * Math.Sin(M - 2.0 * D) + 0.658 * Math.Sin(2.0 * D);
		double num2 = 5.128 * Math.Sin(F) + 0.28 * Math.Sin(F + M) - 0.28 * Math.Sin(F - M);
		eL *= Math.PI / 180.0;
		double num3 = num2 * (Math.PI / 180.0);
		double cosB = Math.Cos(num3);
		double sinB = Math.Sin(num3);
		double CosBSinL = cosB * Math.Sin(eL);
		double X = cosB * Math.Cos(eL);
		double y = 0.9174771 * CosBSinL - 0.3977885 * sinB;
		sinDelta = 0.3977885 * CosBSinL + 0.9174771 * sinB;
		return Math.Atan2(y, X);
	}

	public double GetMoonCelestialAngles(double z, double totalDays, out double sinh)
	{
		double sinDelta;
		double RA = GetMoonAscensionDeclination(totalDays, out sinDelta);
		double cosDelta = Math.Sqrt(1.0 - sinDelta * sinDelta);
		double num = GameMath.Mod(totalDays - GameMath.Mod(totalDays, DaysPerYear) / (double)DaysPerYear, 1.0) * 6.2831854820251465 - RA;
		double num2 = OnGetLatitude(z) * 1.5707963705062866;
		double sinLat = Math.Sin(num2);
		double cosLat = Math.Cos(num2);
		double cosTau = Math.Cos(num);
		sinh = sinLat * sinDelta + cosLat * cosDelta * cosTau;
		return 0.0 - Math.Atan2(Math.Sin(num), sinLat * cosTau - cosLat * sinDelta / cosDelta);
	}

	protected double GetMoonCelestialAnglesFromCache(double z, out double sinh)
	{
		double sinDelta = moonSinDeltaCached;
		double cosDelta = Math.Sqrt(1.0 - sinDelta * sinDelta);
		double num = moonTauCached;
		double num2 = OnGetLatitude(z) * 1.5707963705062866;
		double sinLat = Math.Sin(num2);
		double cosLat = Math.Cos(num2);
		double cosTau = Math.Cos(num);
		sinh = sinLat * sinDelta + cosLat * cosDelta * cosTau;
		return 0.0 - Math.Atan2(Math.Sin(num), sinLat * cosTau - cosLat * sinDelta / cosDelta);
	}

	protected void CacheMoonCelestialAngles()
	{
		double totalDays = TotalDays;
		double sinDelta;
		double RA = GetMoonAscensionDeclination(totalDays, out sinDelta);
		moonTauCached = GameMath.Mod(totalDays - GameMath.Mod(totalDays, DaysPerYear) / (double)DaysPerYear, 1.0) * 6.2831854820251465 - RA;
		moonSinDeltaCached = sinDelta;
	}

	public float RealSecondsToGameSeconds(float seconds)
	{
		return seconds * currentSpeedOfTime * CalendarSpeedMul;
	}

	public void SetSeasonOverride(float? seasonRel)
	{
		SeasonOverride = seasonRel;
	}

	public void SetTimeSpeedModifier(string name, float speed)
	{
		timeSpeedModifiers[name] = speed;
		CalculateCurrentTimeSpeed();
	}

	public void RemoveTimeSpeedModifier(string name)
	{
		timeSpeedModifiers.Remove(name);
		CalculateCurrentTimeSpeed();
	}

	private void CalculateCurrentTimeSpeed()
	{
		float totalSpeed = 0f;
		foreach (float speed in timeSpeedModifiers.Values)
		{
			totalSpeed += speed;
		}
		currentSpeedOfTime = totalSpeed;
		DayLengthInRealLifeSeconds = ((currentSpeedOfTime == 0f) ? float.MaxValue : (3600f * HoursPerDay / currentSpeedOfTime / CalendarSpeedMul));
	}

	public void SetTotalSeconds(long totalSecondsNow, long totalSecondsStart)
	{
		timespan = TimeSpan.FromSeconds(totalSecondsNow);
		this.totalSecondsStart = totalSecondsStart;
	}

	public void Start()
	{
		if (!watchIngameTime.IsRunning)
		{
			watchIngameTime.Start();
		}
	}

	public void Stop()
	{
		if (watchIngameTime.IsRunning)
		{
			watchIngameTime.Stop();
		}
	}

	public virtual void Tick()
	{
		if (watchIngameTime.IsRunning)
		{
			double elapsedGameSeconds = (double)watchIngameTime.ElapsedMilliseconds / 1000.0 * (double)SpeedOfTime * (double)CalendarSpeedMul;
			timespan += TimeSpan.FromSeconds(elapsedGameSeconds);
			watchIngameTime.Restart();
			Update();
		}
	}

	public virtual void Update()
	{
		float sub = Math.Max(0f, 1.15f - MoonSize);
		double noiseval = superMoonNoise.Noise(0.0, TotalDays / 8.0);
		superMoonSize = (float)GameMath.Clamp((noiseval - 0.74 - (double)sub) * 50.0, 1.0, 2.5);
		moonPhaseCached = GetMoonPhase(TotalDays);
		CacheMoonCelestialAngles();
	}

	public void SetDayTime(float wantHourOfDay)
	{
		float hoursToAdd = ((!(HourOfDay > wantHourOfDay)) ? (wantHourOfDay - HourOfDay) : (wantHourOfDay + (HoursPerDay - HourOfDay)));
		Add(hoursToAdd);
	}

	public void SetMonth(float month)
	{
		float hoursToAdd = ((!(Monthf > month)) ? ((month - Monthf) * HoursPerDay * (float)DaysPerMonth + 12f) : ((month + ((float)MonthsPerYear - Monthf)) * HoursPerDay * (float)DaysPerMonth + 12f));
		Add(hoursToAdd);
	}

	public void Add(float hours)
	{
		TimeSpan toadd = TimeSpan.FromHours(hours);
		timespan = timespan.Add(toadd);
	}

	public float GetDayLightStrength(double x, double z)
	{
		double totalDays = TotalDays;
		SolarSphericalCoords celestialAngles = GetCelestialAngles(x, z, totalDays);
		float phi = celestialAngles.AzimuthAngle;
		float theta = celestialAngles.ZenithAngle;
		float sinTheta = GameMath.Sin(theta);
		Vec3f sunPos = new Vec3f(sinTheta * GameMath.Sin(phi), GameMath.Cos(theta), sinTheta * GameMath.Cos(phi));
		Vec3f moonPos = GetMoonPosition(z);
		float moonPhaseBrightness = MoonPhaseBrightness;
		float sunIntensity = (GameMath.Clamp(sunPos.Y * 1.4f + 0.2f, -1f, 1f) + 1f) / 2f;
		float moonYRel = moonPos.Y;
		float moonBright = GameMath.Clamp(moonPhaseBrightness * (0.66f + 0.33f * moonYRel), -0.2f, moonPhaseBrightness);
		float eclipseDarkening = GameMath.Clamp((sunPos.Dot(moonPos) - 0.99955f) * 2500f, 0f, sunIntensity * 0.6f);
		sunIntensity = Math.Max(0f, sunIntensity - eclipseDarkening);
		moonBright = Math.Max(0f, moonBright - eclipseDarkening);
		float val = GameMath.Lerp(0f, moonBright, GameMath.Clamp(moonYRel * 20f, 0f, 1f));
		float sunExtraIntensity = Math.Max(0f, (sunPos.Y - 0.4f) / 7.5f);
		Color colSun = getSunlightPixelRel(GameMath.Clamp(sunIntensity + SunsetMod, 0f, 1f), 0.01f);
		return Math.Max(val, (float)(colSun.R + colSun.G + colSun.B) / 3f / 255f + sunExtraIntensity);
	}

	public float GetDayLightStrength(BlockPos pos)
	{
		return GetDayLightStrength(pos.X, pos.Z);
	}

	public EnumSeason GetSeason(BlockPos pos)
	{
		float seasonRel = GetSeasonRel(pos);
		if (!(seasonRel < 0.2164f))
		{
			if (!(seasonRel < 0.4685f))
			{
				if (!(seasonRel < 0.726f))
				{
					if (seasonRel < 0.9726f)
					{
						return EnumSeason.Fall;
					}
					return EnumSeason.Winter;
				}
				return EnumSeason.Summer;
			}
			return EnumSeason.Spring;
		}
		return EnumSeason.Winter;
	}

	public float GetSeasonRel(BlockPos pos)
	{
		if (SeasonOverride.HasValue)
		{
			return SeasonOverride.Value;
		}
		if (GetHemisphere(pos) != EnumHemisphere.North)
		{
			return (YearRel + 0.5f) % 1f;
		}
		return YearRel;
	}

	public EnumHemisphere GetHemisphere(BlockPos pos)
	{
		if (OnGetHemisphere != null)
		{
			return OnGetHemisphere(pos.X, pos.Z);
		}
		return EnumHemisphere.North;
	}

	public Color getSunlightPixelRel(float relx, float rely)
	{
		float num = Math.Min(sunLightTextureSize.Width - 1, relx * (float)sunLightTextureSize.Width);
		int x = (int)num;
		int y = (int)Math.Min(sunLightTextureSize.Height - 1, rely * (float)sunLightTextureSize.Height);
		float lx = num - (float)x;
		int col1 = sunLightTexture[y * sunLightTextureSize.Width + x];
		int col2 = sunLightTexture[y * sunLightTextureSize.Width + Math.Min(sunLightTextureSize.Width - 1, x + 1)];
		return Color.FromArgb(GameMath.LerpRgbaColor(lx, col1, col2));
	}

	public Packet_Server ToPacket()
	{
		string[] names = new string[timeSpeedModifiers.Count];
		int[] speeds = new int[timeSpeedModifiers.Count];
		int i = 0;
		foreach (KeyValuePair<string, float> val in timeSpeedModifiers)
		{
			names[i] = val.Key;
			speeds[i] = CollectibleNet.SerializeFloatPrecise(val.Value);
			i++;
		}
		Packet_ServerCalendar p = new Packet_ServerCalendar
		{
			TotalSeconds = (long)timespan.TotalSeconds,
			TotalSecondsStart = totalSecondsStart,
			MoonOrbitDays = MoonOrbitDays,
			DaysPerMonth = DaysPerMonth,
			HoursPerDay = CollectibleNet.SerializeFloatVeryPrecise(HoursPerDay),
			CalendarSpeedMul = CollectibleNet.SerializeFloatVeryPrecise(calendarSpeedMul)
		};
		p.SetTimeSpeedModifierNames(names);
		p.SetTimeSpeedModifierSpeeds(speeds);
		p.Running = (watchIngameTime.IsRunning ? 1 : 0);
		return new Packet_Server
		{
			Id = 13,
			Calendar = p
		};
	}

	public Packet_Server ToUpdatePacket()
	{
		Packet_ServerCalendarUpdate p = new Packet_ServerCalendarUpdate
		{
			TotalSeconds = (long)timespan.TotalSeconds,
			TotalSecondsStart = totalSecondsStart
		};
		return new Packet_Server
		{
			Id = 83,
			Calendarupdate = p
		};
	}

	public string PrettyDate()
	{
		float hourOfDay = HourOfDay;
		int hour = (int)hourOfDay;
		int minute = (int)((hourOfDay - (float)hour) * 60f);
		return Lang.Get("dateformat", DayOfMonth, Lang.Get("month-" + MonthName), Year.ToString("0"), hour.ToString("00"), minute.ToString("00"));
	}

	public void UpdateFromPacket(Packet_Server packet)
	{
		if (packet.Calendarupdate != null)
		{
			SetTotalSeconds(packet.Calendarupdate.TotalSeconds, packet.Calendarupdate.TotalSecondsStart);
			return;
		}
		Packet_ServerCalendar calpacket = packet.Calendar;
		SetTotalSeconds(calpacket.TotalSeconds, calpacket.TotalSecondsStart);
		timeSpeedModifiers.Clear();
		for (int i = 0; i < calpacket.TimeSpeedModifierNamesCount; i++)
		{
			timeSpeedModifiers[calpacket.TimeSpeedModifierNames[i]] = CollectibleNet.DeserializeFloatPrecise(calpacket.TimeSpeedModifierSpeeds[i]);
		}
		MoonOrbitDays = calpacket.MoonOrbitDays;
		HoursPerDay = CollectibleNet.DeserializeFloatVeryPrecise(calpacket.HoursPerDay);
		calendarSpeedMul = CollectibleNet.DeserializeFloatVeryPrecise(calpacket.CalendarSpeedMul);
		DaysPerMonth = calpacket.DaysPerMonth;
		if (HoursPerDay == 0f)
		{
			throw new ArgumentException("Trying to set 0 hours per day.");
		}
		if (calpacket.Running > 0)
		{
			if (!watchIngameTime.IsRunning)
			{
				watchIngameTime.Start();
			}
		}
		else if (watchIngameTime.IsRunning)
		{
			watchIngameTime.Stop();
		}
		CalculateCurrentTimeSpeed();
	}

	public BitmapRef BitmapCreateFromPng(IAsset asset)
	{
		return new BitmapExternal(new MemoryStream(asset.Data));
	}
}
