using System;
using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.MathTools;
using Vintagestory.API.Util;

namespace RPVoiceChat.GameContent.Systems
{
	// Token: 0x020000B8 RID: 184
	public class WireMesh
	{
		// Token: 0x060005C1 RID: 1473 RVA: 0x0001F218 File Offset: 0x0001D418
		private static float Catenary(float x, float d = 1f, float a = 2f)
		{
			return a * ((float)Math.Cosh((double)((x - d / 2f) / a)) - (float)Math.Cosh((double)(d / 2f / a)));
		}

		// Token: 0x060005C2 RID: 1474 RVA: 0x0001F240 File Offset: 0x0001D440
		private static Vec3f CrossProduct(Vec3f v1, Vec3f v2)
		{
			float num = v1.Y * v2.Z - v2.Y * v1.Z;
			float y = (v1.X * v2.Z - v2.X * v1.Z) * -1f;
			float z = v1.X * v2.Y - v2.X * v1.Y;
			Vec3f vec3f = new Vec3f(num, y, z);
			vec3f.Normalize();
			return vec3f;
		}

		// Token: 0x060005C3 RID: 1475 RVA: 0x0001F2B4 File Offset: 0x0001D4B4
		public static MeshData MakeWireMesh(Vec3f pos1, Vec3f pos2, float thickness = 0.015f)
		{
			Vec3f dPos = pos2 - pos1;
			float dist = pos2.DistanceTo(pos1);
			int nSec = (int)Math.Floor((double)(dist * 2f));
			nSec = ((nSec > 5) ? nSec : 5);
			MeshData mesh = new MeshData(4, 6, false, true, true, true);
			mesh.SetMode(0);
			MeshData mesh_top = new MeshData(4, 6, false, true, true, true);
			mesh_top.SetMode(0);
			MeshData mesh_bot = new MeshData(4, 6, false, true, true, true);
			mesh_bot.SetMode(0);
			MeshData mesh_side = new MeshData(4, 6, false, true, true, true);
			mesh_side.SetMode(0);
			MeshData mesh_side2 = new MeshData(4, 6, false, true, true, true);
			mesh_side2.SetMode(0);
			Vec3f b = new Vec3f(-dPos.Z, 0f, dPos.X).Normalize();
			if (dPos.Z == 0f && dPos.X == 0f)
			{
				b = new Vec3f(1f, 0f, 0f);
			}
			ArrayExtensions.Fill<int>(mesh_top.Flags, 0);
			ArrayExtensions.Fill<int>(mesh_bot.Flags, 0);
			ArrayExtensions.Fill<int>(mesh_side.Flags, 0);
			ArrayExtensions.Fill<int>(mesh_side2.Flags, 0);
			Vec3f[] positions = new Vec3f[nSec + 1];
			for (int i = 0; i <= nSec; i++)
			{
				float x = dPos.X / (float)nSec * (float)i;
				float y = dPos.Y / (float)nSec * (float)i;
				float z = dPos.Z / (float)nSec * (float)i;
				float dy = WireMesh.Catenary((float)Math.Sqrt((double)(x * x + y * y + z * z)) / dist, 1f, 2f);
				positions[i] = new Vec3f(x, y + dy, z);
			}
			for (int j = 0; j <= nSec; j++)
			{
				Vec3f pos3 = pos1 + positions[j];
				Vec3f vec3f = (j < nSec) ? positions[j + 1] : positions[j];
				Vec3f posBefore = (j > 0) ? positions[j - 1] : positions[j];
				Vec3f a = WireMesh.CrossProduct((vec3f + posBefore).Normalize(), b * -1f);
				float du = dist / (float)nSec;
				int color = 1;
				float uvV = 0.125f;
				mesh_top.AddVertex((pos3 - b * thickness + a * thickness).X, (pos3 - b * thickness + a * thickness).Y, (pos3 - b * thickness + a * thickness).Z, (float)j * du, 0f, color);
				mesh_top.AddVertex((pos3 + b * thickness + a * thickness).X, (pos3 + b * thickness + a * thickness).Y, (pos3 + b * thickness + a * thickness).Z, (float)j * du, uvV, color);
				mesh_bot.AddVertex((pos3 - b * thickness - a * thickness).X, (pos3 - b * thickness - a * thickness).Y, (pos3 - b * thickness - a * thickness).Z, (float)j * du, 0f, color);
				mesh_bot.AddVertex((pos3 + b * thickness - a * thickness).X, (pos3 + b * thickness - a * thickness).Y, (pos3 + b * thickness - a * thickness).Z, (float)j * du, uvV, color);
				mesh_side.AddVertex((pos3 - b * thickness + a * thickness).X, (pos3 - b * thickness + a * thickness).Y, (pos3 - b * thickness + a * thickness).Z, (float)j * du, uvV, color);
				mesh_side.AddVertex((pos3 - b * thickness - a * thickness).X, (pos3 - b * thickness - a * thickness).Y, (pos3 - b * thickness - a * thickness).Z, (float)j * du, 0f, color);
				mesh_side2.AddVertex((pos3 + b * thickness + a * thickness).X, (pos3 + b * thickness + a * thickness).Y, (pos3 + b * thickness + a * thickness).Z, (float)j * du, uvV, color);
				mesh_side2.AddVertex((pos3 + b * thickness - a * thickness).X, (pos3 + b * thickness - a * thickness).Y, (pos3 + b * thickness - a * thickness).Z, (float)j * du, 0f, color);
				mesh_top.Flags[2 * j] = VertexFlags.PackNormal(new Vec3f(0f, 1f, 0f));
				mesh_top.Flags[2 * j + 1] = VertexFlags.PackNormal(new Vec3f(0f, 1f, 0f));
				mesh_bot.Flags[2 * j] = VertexFlags.PackNormal(new Vec3f(0f, -1f, 0f));
				mesh_bot.Flags[2 * j + 1] = VertexFlags.PackNormal(new Vec3f(0f, -1f, 0f));
				mesh_side.Flags[2 * j] = VertexFlags.PackNormal((double)(-(double)b.X), (double)(-(double)b.Y), (double)(-(double)b.Z));
				mesh_side.Flags[2 * j + 1] = VertexFlags.PackNormal((double)(-(double)b.X), (double)(-(double)b.Y), (double)(-(double)b.Z));
				mesh_side2.Flags[2 * j] = VertexFlags.PackNormal(b);
				mesh_side2.Flags[2 * j + 1] = VertexFlags.PackNormal(b);
			}
			for (int k = 0; k < nSec; k++)
			{
				int offset = 2 * k;
				mesh_top.AddIndex(offset);
				mesh_top.AddIndex(offset + 3);
				mesh_top.AddIndex(offset + 2);
				mesh_top.AddIndex(offset);
				mesh_top.AddIndex(offset + 1);
				mesh_top.AddIndex(offset + 3);
				mesh_bot.AddIndex(offset);
				mesh_bot.AddIndex(offset + 3);
				mesh_bot.AddIndex(offset + 1);
				mesh_bot.AddIndex(offset);
				mesh_bot.AddIndex(offset + 2);
				mesh_bot.AddIndex(offset + 3);
				mesh_side.AddIndex(offset);
				mesh_side.AddIndex(offset + 3);
				mesh_side.AddIndex(offset + 1);
				mesh_side.AddIndex(offset);
				mesh_side.AddIndex(offset + 2);
				mesh_side.AddIndex(offset + 3);
				mesh_side2.AddIndex(offset);
				mesh_side2.AddIndex(offset + 3);
				mesh_side2.AddIndex(offset + 2);
				mesh_side2.AddIndex(offset);
				mesh_side2.AddIndex(offset + 1);
				mesh_side2.AddIndex(offset + 3);
			}
			mesh.AddMeshData(mesh_top);
			mesh.AddMeshData(mesh_bot);
			mesh.AddMeshData(mesh_side);
			mesh.AddMeshData(mesh_side2);
			ArrayExtensions.Fill<byte>(mesh.Rgba, byte.MaxValue);
			return mesh;
		}
	}
}
