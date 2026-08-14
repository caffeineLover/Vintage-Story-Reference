using System;
using System.Text.RegularExpressions;
using Vintagestory.API.Client;
using Vintagestory.API.Config;

namespace Vintagestory.Client.NoObf;

public class Shader : IShader
{
	private static string shaderVersionPattern = "\\#version (\\d+)";

	public int ShaderId;

	private string prefixCode = "";

	private string code = "";

	public EnumShaderType shaderType;

	internal string Filename = "";

	public EnumShaderType Type
	{
		get
		{
			return shaderType;
		}
		set
		{
			shaderType = value;
		}
	}

	public string PrefixCode
	{
		get
		{
			return prefixCode;
		}
		set
		{
			prefixCode = value;
		}
	}

	public string Code
	{
		get
		{
			return code;
		}
		set
		{
			code = value;
		}
	}

	public Shader()
	{
	}

	public Shader(EnumShaderType shaderType, string code, string filename)
	{
		this.shaderType = shaderType;
		this.code = code;
		Filename = filename;
	}

	public bool Compile()
	{
		return ScreenManager.Platform.CompileShader(this);
	}

	public void EnsureVersionSupported()
	{
		Match match = Regex.Match(code, shaderVersionPattern);
		if (match.Groups.Count > 1)
		{
			string versionRequested = match.Groups[1].Value;
			if (ScreenManager.Platform.UseSSBOs && UsesSSBOs())
			{
				versionRequested = "430";
			}
			EnsureVersionSupported(versionRequested, Filename);
		}
	}

	public Shader Clone()
	{
		return new Shader(shaderType, code, Filename)
		{
			prefixCode = prefixCode
		};
	}

	public static void EnsureVersionSupported(string versionUsed, string ownFilename)
	{
		string versionSupported = ScreenManager.Platform.GetGLShaderVersionString();
		string versionSupportedfiltered = Regex.Match(versionSupported, "(\\d\\.\\d+)").Groups[1].Value.Replace(".", "");
		int.TryParse(versionSupportedfiltered, out var versionSupportedInt);
		int.TryParse(versionUsed, out var versionUsedInt);
		if (versionUsedInt <= versionSupportedInt)
		{
			return;
		}
		string errorMessage = $"Your graphics card supports only OpenGL version {versionSupportedfiltered} ({versionSupported}), but OpenGL version {versionUsed} is required.\n";
		if (versionSupportedInt == 330 && ClientSettings.GlContextVersion == "3.3" && RuntimeEnv.OS != OS.Mac)
		{
			errorMessage += "===>  In your clientsettings.json file try searching and setting this string setting:  \"glContextVersion\": \"4.3\",  then start the game again <===";
			if (ScreenManager.Platform.UseSSBOs)
			{
				errorMessage += "\n(You can also try setting bool setting \"allowSSBOs\" to false, but try out \"glContextVersion\" first)";
			}
		}
		else if (ScreenManager.Platform.UseSSBOs && versionUsed == "430")
		{
			errorMessage = ((versionSupportedInt >= 430 || RuntimeEnv.OS == OS.Mac) ? (errorMessage + "***In your clientsettings.json file please set bool setting \"allowSSBOs\" to false, and try again.***\n") : (errorMessage + "***In your clientsettings.json file please either set bool setting \"allowSSBOs\" to false, or set \"glContextVersion\" to 4.3, and try again.***\n(AllowSSBOs true should only be used if your hardware supports OpenGL 4.3 or later. Ask for support if necessary!)\n"));
			errorMessage += "Please then check if you have installed the latest version of your graphics card driver. If so, your graphics card may be to old to play Vintage Story.(Note: In case of modded gameplay with modded shaders, the mod author may be able to lower the OpenGL version requirements)";
		}
		else
		{
			errorMessage += "*** First check clientsettings.json setting \"glContextVersion\", this may have modified the version reported by the hardware, try increasing it.***\nPlease then check if you have installed the latest version of your graphics card driver. If so, your graphics card may be to old to play Vintage Story.(Note: In case of modded gameplay with modded shaders, the mod author may be able to lower the OpenGL version requirements)";
		}
		throw new NotSupportedException(errorMessage);
	}

	public bool UsesSSBOs()
	{
		if (Type != EnumShaderType.VertexShader)
		{
			return false;
		}
		if (Filename.StartsWith("chunk") || Filename.Equals("decals.vsh"))
		{
			return true;
		}
		return false;
	}
}
