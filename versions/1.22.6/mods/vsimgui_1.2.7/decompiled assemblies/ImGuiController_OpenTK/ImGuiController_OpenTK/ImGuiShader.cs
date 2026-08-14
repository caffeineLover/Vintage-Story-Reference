using System;
using System.Runtime.CompilerServices;
using OpenTK.Graphics.OpenGL4;

namespace ImGuiController_OpenTK
{
	// Token: 0x02000006 RID: 6
	[NullableContext(1)]
	[Nullable(0)]
	internal static class ImGuiShader
	{
		// Token: 0x0600002D RID: 45 RVA: 0x00003694 File Offset: 0x00001894
		[NullableContext(0)]
		[return: TupleElementNames(new string[]
		{
			"shader",
			"projectionMatrix",
			"fontTexture"
		})]
		public static ValueTuple<int, int, int> Create()
		{
			int num = ImGuiShader.CreateProgram("ImGui", "#version 330 core\r\n\r\nuniform mat4 projection_matrix;\r\n\r\nlayout(location = 0) in vec2 in_position;\r\nlayout(location = 1) in vec2 in_texCoord;\r\nlayout(location = 2) in vec4 in_color;\r\n\r\nout vec4 color;\r\nout vec2 texCoord;\r\n\r\nvoid main()\r\n{\r\n    gl_Position = projection_matrix * vec4(in_position, 0, 1);\r\n    color = in_color;\r\n    texCoord = in_texCoord;\r\n}", "#version 330 core\r\n\r\nuniform sampler2D in_fontTexture;\r\n\r\nin vec4 color;\r\nin vec2 texCoord;\r\n\r\nout vec4 outputColor;\r\n\r\nvoid main()\r\n{\r\n    outputColor = color * texture(in_fontTexture, texCoord);\r\n}");
			int projectionMatrix = GL.GetUniformLocation(num, "projection_matrix");
			int fontTexture = GL.GetUniformLocation(num, "in_fontTexture");
			return new ValueTuple<int, int, int>(num, projectionMatrix, fontTexture);
		}

		// Token: 0x0600002E RID: 46 RVA: 0x000036D4 File Offset: 0x000018D4
		private static int CreateProgram(string name, string vertexSource, string fragmentSource)
		{
			int program = GL.CreateProgram();
			int vertex = ImGuiShader.CompileShader(name, 35633, vertexSource);
			int fragment = ImGuiShader.CompileShader(name, 35632, fragmentSource);
			GL.AttachShader(program, vertex);
			GL.AttachShader(program, fragment);
			GL.LinkProgram(program);
			int success;
			GL.GetProgram(program, 35714, ref success);
			if (success == 0)
			{
				GL.GetProgramInfoLog(program);
			}
			GL.DetachShader(program, vertex);
			GL.DetachShader(program, fragment);
			GL.DeleteShader(vertex);
			GL.DeleteShader(fragment);
			return program;
		}

		// Token: 0x0600002F RID: 47 RVA: 0x00003748 File Offset: 0x00001948
		private static int CompileShader(string name, ShaderType type, string source)
		{
			int shader = GL.CreateShader(type);
			GL.ShaderSource(shader, source);
			GL.CompileShader(shader);
			int success;
			GL.GetShader(shader, 35713, ref success);
			if (success == 0)
			{
				GL.GetShaderInfoLog(shader);
			}
			return shader;
		}

		// Token: 0x0400001A RID: 26
		private const string cVertexShaderSource = "#version 330 core\r\n\r\nuniform mat4 projection_matrix;\r\n\r\nlayout(location = 0) in vec2 in_position;\r\nlayout(location = 1) in vec2 in_texCoord;\r\nlayout(location = 2) in vec4 in_color;\r\n\r\nout vec4 color;\r\nout vec2 texCoord;\r\n\r\nvoid main()\r\n{\r\n    gl_Position = projection_matrix * vec4(in_position, 0, 1);\r\n    color = in_color;\r\n    texCoord = in_texCoord;\r\n}";

		// Token: 0x0400001B RID: 27
		private const string cFragmentShaderSource = "#version 330 core\r\n\r\nuniform sampler2D in_fontTexture;\r\n\r\nin vec4 color;\r\nin vec2 texCoord;\r\n\r\nout vec4 outputColor;\r\n\r\nvoid main()\r\n{\r\n    outputColor = color * texture(in_fontTexture, texCoord);\r\n}";
	}
}
