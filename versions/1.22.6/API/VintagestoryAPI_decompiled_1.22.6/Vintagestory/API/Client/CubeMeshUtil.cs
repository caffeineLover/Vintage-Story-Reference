using Vintagestory.API.Common;
using Vintagestory.API.MathTools;
using Vintagestory.API.Util;

namespace Vintagestory.API.Client;

public class CubeMeshUtil
{
	/// <summary>
	/// Top, Front/Left, Back/Right, Bottom
	/// </summary>
	public static float[] CloudSideShadings = new float[4] { 1f, 0.6f, 0.6f, 0.45f };

	/// <summary>
	/// Top, Front/Left, Back/Right, Bottom
	/// </summary>
	public static float[] DefaultBlockSideShadings = new float[4] { 1f, 0.75f, 0.6f, 0.45f };

	/// <summary>
	/// Shadings by Blockfacing index
	/// </summary>
	public static float[] DefaultBlockSideShadingsByFacing = new float[6]
	{
		DefaultBlockSideShadings[2],
		DefaultBlockSideShadings[1],
		DefaultBlockSideShadings[2],
		DefaultBlockSideShadings[1],
		DefaultBlockSideShadings[0],
		DefaultBlockSideShadings[3]
	};

	/// <summary>
	/// XYZ Vertex positions for every vertex in a cube. Origin is the cube middle point.
	/// </summary>
	public static int[] CubeVertices = new int[72]
	{
		-1, -1, -1, -1, 1, -1, 1, 1, -1, 1,
		-1, -1, 1, -1, -1, 1, 1, -1, 1, 1,
		1, 1, -1, 1, 1, -1, 1, 1, 1, 1,
		-1, 1, 1, -1, -1, 1, -1, -1, 1, -1,
		1, 1, -1, 1, -1, -1, -1, -1, 1, 1,
		1, 1, 1, -1, -1, 1, -1, -1, 1, 1,
		-1, -1, 1, -1, -1, -1, 1, -1, -1, 1,
		-1, 1
	};

	/// <summary>
	/// Cube face indices, in order: North, East, South, West, Up, Down.
	/// </summary>
	public static byte[] CubeFaceIndices = new byte[6]
	{
		BlockFacing.NORTH.MeshDataIndex,
		BlockFacing.EAST.MeshDataIndex,
		BlockFacing.SOUTH.MeshDataIndex,
		BlockFacing.WEST.MeshDataIndex,
		BlockFacing.UP.MeshDataIndex,
		BlockFacing.DOWN.MeshDataIndex
	};

	/// <summary>
	/// UV Coords for every Vertex in a cube
	/// </summary>
	public static int[] CubeUvCoords = new int[48]
	{
		1, 0, 1, 1, 0, 1, 0, 0, 1, 0,
		1, 1, 0, 1, 0, 0, 1, 0, 1, 1,
		0, 1, 0, 0, 1, 0, 1, 1, 0, 1,
		0, 0, 1, 0, 1, 1, 0, 1, 0, 0,
		1, 0, 1, 1, 0, 1, 0, 0
	};

	/// <summary>
	/// Indices for every triangle in a cube
	/// </summary>
	public static int[] CubeVertexIndices = new int[36]
	{
		0, 1, 2, 0, 2, 3, 4, 5, 6, 4,
		6, 7, 8, 9, 10, 8, 10, 11, 12, 13,
		14, 12, 14, 15, 16, 17, 18, 16, 18, 19,
		20, 21, 22, 20, 22, 23
	};

	/// <summary>
	/// Can be used for any face if offseted correctly
	/// </summary>
	public static int[] BaseCubeVertexIndices = new int[6] { 0, 1, 2, 0, 2, 3 };

	/// <summary>
	/// Returns a default 2x2x2 cube with xyz,uv,rgba and indices set - ready for upload to the graphics card
	/// </summary>
	/// <returns></returns>
	public static MeshData GetCube()
	{
		MeshData m = new MeshData();
		float[] xyz = new float[72];
		for (int i = 0; i < 72; i++)
		{
			xyz[i] = CubeVertices[i];
		}
		m.SetXyz(xyz);
		float[] uv = new float[48];
		for (int j = 0; j < 48; j++)
		{
			uv[j] = CubeUvCoords[j];
		}
		byte[] rgba = new byte[96];
		m.SetRgba(rgba);
		m.SetUv(uv);
		m.TextureIndices = new byte[6];
		m.SetVerticesCount(24);
		m.SetIndices(CubeVertexIndices);
		m.SetIndicesCount(36);
		m.Flags = new int[24];
		for (int k = 0; k < 24; k += 4)
		{
			BlockFacing face = BlockFacing.ALLFACES[k / 6];
			m.Flags[k] = face.NormalPackedFlags;
			m.Flags[k + 1] = m.Flags[k];
			m.Flags[k + 2] = m.Flags[k];
			m.Flags[k + 3] = m.Flags[k];
		}
		m.VerticesMax = m.VerticesCount;
		return m;
	}

	/// <summary>
	/// Returns a rgba byte array to be used for default shading on a standard cube, can supply the shading levels
	/// </summary>
	/// <param name="baseColor"></param>
	/// <param name="blockSideShadings"></param>
	/// <param name="smoothShadedSides"></param>
	/// <returns></returns>
	public static byte[] GetShadedCubeRGBA(int baseColor, float[] blockSideShadings, bool smoothShadedSides)
	{
		int topSideColor = ColorUtil.ColorMultiply3(baseColor, blockSideShadings[0]);
		int frontSideColor = ColorUtil.ColorMultiply3(baseColor, blockSideShadings[1]);
		int backSideColor = ColorUtil.ColorMultiply3(baseColor, blockSideShadings[2]);
		int bottomColor = ColorUtil.ColorMultiply3(baseColor, blockSideShadings[3]);
		return GetShadedCubeRGBA(new int[6] { frontSideColor, backSideColor, backSideColor, frontSideColor, topSideColor, bottomColor }, smoothShadedSides);
	}

	/// <summary>
	/// Returns a rgba byte array to be used for default shading on a standard cube
	/// </summary>
	/// <param name="colorSides"></param>
	/// <param name="smoothShadedSides"></param>
	/// <returns></returns>
	public unsafe static byte[] GetShadedCubeRGBA(int[] colorSides, bool smoothShadedSides)
	{
		byte[] result = new byte[96];
		fixed (byte* rgbaByte = result)
		{
			int* rgbaInt = (int*)rgbaByte;
			for (int facing = 0; facing < 6; facing++)
			{
				for (int vertex = 0; vertex < 4; vertex++)
				{
					rgbaInt[(facing * 4 + vertex) * 4 / 4] = colorSides[facing];
				}
			}
			if (smoothShadedSides)
			{
				*rgbaInt = colorSides[3];
				rgbaInt[1] = colorSides[3];
				rgbaInt[4] = colorSides[3];
				rgbaInt[7] = colorSides[3];
				rgbaInt[16] = colorSides[3];
				rgbaInt[19] = colorSides[3];
				rgbaInt[20] = colorSides[3];
				rgbaInt[21] = colorSides[3];
			}
		}
		return result;
	}

	/// <summary>
	/// Same as GetCubeModelData but can define scale and translation. Scale is applied first.
	/// </summary>
	/// <param name="scaleH"></param>
	/// <param name="scaleV"></param>
	/// <param name="translate"></param>
	/// <returns></returns>
	public static MeshData GetCubeOnlyScaleXyz(float scaleH, float scaleV, Vec3f translate)
	{
		MeshData modelData = GetCube();
		for (int i = 0; i < modelData.GetVerticesCount(); i++)
		{
			modelData.xyz[3 * i] *= scaleH;
			modelData.xyz[3 * i + 1] *= scaleV;
			modelData.xyz[3 * i + 2] *= scaleH;
			modelData.xyz[3 * i] += translate.X;
			modelData.xyz[3 * i + 1] += translate.Y;
			modelData.xyz[3 * i + 2] += translate.Z;
		}
		return modelData;
	}

	/// <summary>
	/// Same as GetCubeModelData but can define scale and translation. Scale is applied first.
	/// </summary>
	/// <param name="scaleH"></param>
	/// <param name="scaleV"></param>
	/// <param name="translate"></param>
	/// <returns></returns>
	public static MeshData GetCube(float scaleH, float scaleV, Vec3f translate)
	{
		MeshData modelData = GetCube();
		for (int i = 0; i < modelData.GetVerticesCount(); i++)
		{
			modelData.xyz[3 * i] *= scaleH;
			modelData.xyz[3 * i + 1] *= scaleV;
			modelData.xyz[3 * i + 2] *= scaleH;
			modelData.xyz[3 * i] += translate.X;
			modelData.xyz[3 * i + 1] += translate.Y;
			modelData.xyz[3 * i + 2] += translate.Z;
			modelData.Uv[2 * i] *= 2f * scaleH;
			modelData.Uv[2 * i + 1] *= ((i >= 16) ? (2f * scaleH) : (2f * scaleV));
		}
		modelData.Rgba.Fill(byte.MaxValue);
		return modelData;
	}

	/// <summary>
	/// Same as GetCubeModelData but can define scale and translation. Scale is applied first.
	/// </summary>
	/// <param name="scaleX"></param>
	/// <param name="scaleY"></param>
	/// <param name="scaleZ"></param>
	/// <param name="translate"></param>
	/// <returns></returns>
	public static MeshData GetCube(float scaleX, float scaleY, float scaleZ, Vec3f translate)
	{
		MeshData cube = GetCube();
		cube.Rgba.Fill(byte.MaxValue);
		return ScaleCubeMesh(cube, scaleX, scaleY, scaleZ, translate);
	}

	/// <summary>
	/// Scales a mesh retrieced by GetCube()
	/// </summary>
	/// <param name="modelData"></param>
	/// <param name="scaleX"></param>
	/// <param name="scaleY"></param>
	/// <param name="scaleZ"></param>
	/// <param name="translate"></param>
	/// <returns></returns>
	public static MeshData ScaleCubeMesh(MeshData modelData, float scaleX, float scaleY, float scaleZ, Vec3f translate)
	{
		float[] uScaleByAxis = new float[3] { scaleZ, scaleX, scaleX };
		float[] vScaleByAxis = new float[3] { scaleY, scaleZ, scaleY };
		float[] uOffsetByAxis = new float[3] { translate.Z, translate.X, translate.X };
		float[] vOffsetByAxis = new float[3] { translate.Y, translate.Z, translate.Y };
		int verticesCount = modelData.GetVerticesCount();
		for (int i = 0; i < verticesCount; i++)
		{
			modelData.xyz[3 * i] *= scaleX;
			modelData.xyz[3 * i + 1] *= scaleY;
			modelData.xyz[3 * i + 2] *= scaleZ;
			modelData.xyz[3 * i] += scaleX + translate.X;
			modelData.xyz[3 * i + 1] += scaleY + translate.Y;
			modelData.xyz[3 * i + 2] += scaleZ + translate.Z;
			BlockFacing obj = BlockFacing.ALLFACES[i / 4];
			int axis = (int)obj.Axis;
			switch (obj.Index)
			{
			case 0:
				modelData.Uv[2 * i] = modelData.Uv[2 * i] * 2f * uScaleByAxis[axis] + (1f - 2f * uScaleByAxis[axis]) - uOffsetByAxis[axis];
				modelData.Uv[2 * i + 1] = (1f - modelData.Uv[2 * i + 1]) * 2f * vScaleByAxis[axis] + (1f - 2f * vScaleByAxis[axis]) - vOffsetByAxis[axis];
				break;
			case 1:
				modelData.Uv[2 * i] = modelData.Uv[2 * i] * 2f * uScaleByAxis[axis] + (1f - 2f * uScaleByAxis[axis]) - uOffsetByAxis[axis];
				modelData.Uv[2 * i + 1] = (1f - modelData.Uv[2 * i + 1]) * 2f * vScaleByAxis[axis] + (1f - 2f * vScaleByAxis[axis]) - vOffsetByAxis[axis];
				break;
			case 2:
				modelData.Uv[2 * i] = modelData.Uv[2 * i] * 2f * uScaleByAxis[axis] + uOffsetByAxis[axis];
				modelData.Uv[2 * i + 1] = (1f - modelData.Uv[2 * i + 1]) * 2f * vScaleByAxis[axis] + (1f - 2f * vScaleByAxis[axis]) - vOffsetByAxis[axis];
				break;
			case 3:
				modelData.Uv[2 * i] = modelData.Uv[2 * i] * 2f * uScaleByAxis[axis] + uOffsetByAxis[axis];
				modelData.Uv[2 * i + 1] = (1f - modelData.Uv[2 * i + 1]) * 2f * vScaleByAxis[axis] + (1f - 2f * vScaleByAxis[axis]) - vOffsetByAxis[axis];
				break;
			case 4:
				modelData.Uv[2 * i] = (1f - modelData.Uv[2 * i]) * 2f * uScaleByAxis[axis] + (1f - 2f * uScaleByAxis[axis]) - uOffsetByAxis[axis];
				modelData.Uv[2 * i + 1] = modelData.Uv[2 * i + 1] * 2f * vScaleByAxis[axis] + (1f - 2f * vScaleByAxis[axis]) - vOffsetByAxis[axis];
				break;
			case 5:
				modelData.Uv[2 * i] = modelData.Uv[2 * i] * 2f * uScaleByAxis[axis] + (1f - 2f * uScaleByAxis[axis]) - uOffsetByAxis[axis];
				modelData.Uv[2 * i + 1] = (1f - modelData.Uv[2 * i + 1]) * 2f * vScaleByAxis[axis] + vOffsetByAxis[axis];
				break;
			}
		}
		return modelData;
	}

	/// <summary>
	/// Gets the face of a given cube.
	/// </summary>
	/// <param name="face">The face you want to fetch in cardinal directions.</param>
	/// <returns>The mesh data for the cube's given face.</returns>
	public static MeshData GetCubeFace(BlockFacing face)
	{
		int offset = face.Index;
		MeshData m = new MeshData();
		float[] xyz = new float[12];
		for (int i = 0; i < xyz.Length; i++)
		{
			xyz[i] = CubeVertices[i + 12 * offset];
		}
		m.SetXyz(xyz);
		float[] uv = new float[8];
		for (int j = 0; j < uv.Length; j++)
		{
			uv[j] = CubeUvCoords[j + 8 * offset];
		}
		m.SetUv(uv);
		byte[] rgba = new byte[16];
		for (int k = 0; k < rgba.Length; k++)
		{
			rgba[k] = byte.MaxValue;
		}
		m.SetRgba(rgba);
		m.SetVerticesCount(4);
		int[] indices = new int[6];
		for (int l = 0; l < indices.Length; l++)
		{
			indices[l] = CubeVertexIndices[l];
		}
		m.SetIndices(indices);
		m.SetIndicesCount(6);
		return m;
	}

	/// <summary>
	/// Gets the face of a given cube.
	/// </summary>
	/// <param name="face">The face you want to fetch in cardinal directions.</param>
	/// <param name="scaleH">The horizontal scale.</param>
	/// <param name="scaleV">The vertical scale.</param>
	/// <param name="translate">The translation desired.</param>
	/// <returns>The mesh data for the given parameters.</returns>
	public static MeshData GetCubeFace(BlockFacing face, float scaleH, float scaleV, Vec3f translate)
	{
		MeshData modelData = GetCubeFace(face);
		for (int i = 0; i < modelData.GetVerticesCount(); i++)
		{
			modelData.xyz[3 * i] *= scaleH;
			modelData.xyz[3 * i + 1] *= scaleV;
			modelData.xyz[3 * i + 2] *= scaleH;
			modelData.xyz[3 * i] += translate.X;
			modelData.xyz[3 * i + 1] += translate.Y;
			modelData.xyz[3 * i + 2] += translate.Z;
			modelData.Uv[2 * i] *= 2f * scaleH;
			modelData.Uv[2 * i + 1] *= ((i >= 16) ? (2f * scaleH) : (2f * scaleV));
		}
		modelData.Rgba.Fill(byte.MaxValue);
		return modelData;
	}

	public static void SetXyzFacesAndPacketNormals(MeshData mesh)
	{
		mesh.AddXyzFace(BlockFacing.NORTH.MeshDataIndex);
		mesh.AddXyzFace(BlockFacing.EAST.MeshDataIndex);
		mesh.AddXyzFace(BlockFacing.SOUTH.MeshDataIndex);
		mesh.AddXyzFace(BlockFacing.WEST.MeshDataIndex);
		mesh.AddXyzFace(BlockFacing.UP.MeshDataIndex);
		mesh.AddXyzFace(BlockFacing.DOWN.MeshDataIndex);
		for (int i = 0; i < 6; i++)
		{
			mesh.Flags[i * 4] = (mesh.Flags[i * 4 + 1] = (mesh.Flags[i * 4 + 2] = (mesh.Flags[i * 4 + 3] = VertexFlags.PackNormal(BlockFacing.ALLFACES[i].Normali))));
		}
	}
}
