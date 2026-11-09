using System.Linq;
using Cairo;
using Vintagestory.API;
using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.Common.Entities;
using Vintagestory.API.Config;
using Vintagestory.API.Datastructures;
using Vintagestory.API.MathTools;
using Vintagestory.API.Server;
using Vintagestory.API.Util;

namespace Vintagestory.GameContent;

[DocumentAsJson]
[AddDocumentationProperty("pigment", "Use on a collectible type. If set and the collectible is used to write on a writing surface with this behavior, its pigment color will be used for text color.", "Vintagestory.API.Datastructures.JsonObject", "Optional", "None", true)]
public class EntityBehaviorWritingSurface : EntityBehavior, ITexPositionSource
{
	protected MultiTextureMeshRef meshref;

	protected ICoreClientAPI capi;

	protected LoadedTexture loadedTexture;

	[DocumentAsJson("Required", "", false)]
	protected string[] writingSurfaces;

	[DocumentAsJson("Required", "", false)]
	protected TextAreaConfig signTextConfig;

	protected CairoFont font;

	protected EnumVerticalAlign verticalAlign;

	protected int TextWidth = 208;

	protected int TextHeight = 96;

	protected float DefaultFontSize;

	protected string SurfaceName = "leftplaque";

	private int tempColor;

	private ItemStack tempStack;

	private GuiDialogTextInput editDialog;

	private string previousText;

	private int previousColor;

	private float previousFontSize = -1f;

	private TextureAtlasPosition texPos;

	private int textureSubId;

	private AssetLocationAndSource assetloc;

	private Size2i dummysize = new Size2i(2048, 2048);

	private TextureAtlasPosition dummyPos = new TextureAtlasPosition
	{
		x1 = 0f,
		y1 = 0f,
		x2 = 1f,
		y2 = 1f
	};

	public float FontSize
	{
		get
		{
			return entity.WatchedAttributes.GetFloat(SurfaceName + "_fontSize", DefaultFontSize);
		}
		set
		{
			entity.WatchedAttributes.SetFloat(SurfaceName + "_fontSize", value);
		}
	}

	public string Text
	{
		get
		{
			return entity.WatchedAttributes.GetString(SurfaceName + "_writingSurfaceText");
		}
		set
		{
			entity.WatchedAttributes.SetString(SurfaceName + "_writingSurfaceText", value);
		}
	}

	public int Color
	{
		get
		{
			return entity.WatchedAttributes.GetInt(SurfaceName + "_textColor", -16777216);
		}
		set
		{
			entity.WatchedAttributes.SetInt(SurfaceName + "_textColor", value);
		}
	}

	public double RenderOrder => 0.36;

	public int RenderRange => 99;

	public Size2i AtlasSize => dummysize;

	public TextureAtlasPosition this[string textureCode] => dummyPos;

	public EntityBehaviorWritingSurface(Entity entity)
		: base(entity)
	{
	}

	public override void Initialize(EntityProperties properties, JsonObject attributes)
	{
		capi = entity.World.Api as ICoreClientAPI;
		if (capi != null)
		{
			capi.Event.ReloadTextures += Event_ReloadTextures;
			entity.WatchedAttributes.RegisterModifiedListener(SurfaceName + "_writingSurfaceText", entity.MarkShapeModified);
			signTextConfig = attributes["fontConfig"].AsObject<TextAreaConfig>();
			font = new CairoFont(signTextConfig.FontSize, signTextConfig.FontName, new double[4] { 0.0, 0.0, 0.0, 0.8 });
			if (signTextConfig.BoldFont)
			{
				font.WithWeight(FontWeight.Bold);
			}
			font.LineHeightMultiplier = 0.8999999761581421;
			verticalAlign = signTextConfig.VerticalAlign;
			TextWidth = signTextConfig.MaxWidth;
			TextHeight = signTextConfig.MaxHeight;
			DefaultFontSize = signTextConfig.FontSize;
		}
		writingSurfaces = attributes["writingSurfaces"].AsObject<string[]>();
	}

	private void Event_ReloadTextures()
	{
		texPos = null;
		previousText = null;
	}

	public override void OnInteract(EntityAgent byEntity, ItemSlot itemslot, Vec3d hitPosition, EnumInteractMode mode, ref EnumHandling handled)
	{
		if (entity.World.Side == EnumAppSide.Server || writingSurfaces == null)
		{
			return;
		}
		EntityBehaviorSelectionBoxes selBoxes = entity.GetBehavior<EntityBehaviorSelectionBoxes>();
		if (selBoxes == null || !writingSurfaces.Any((string apCode) => selBoxes.IsAPCode((byEntity as EntityPlayer).EntitySelection, apCode)) || (editDialog != null && editDialog.IsOpened()))
		{
			return;
		}
		ICoreClientAPI capi = entity.Api as ICoreClientAPI;
		if (loadPigment(byEntity))
		{
			editDialog = new GuiDialogTextInput(Lang.Get("Edit Sign text"), Text, capi, signTextConfig.CopyWithFontSize(FontSize));
			editDialog.OnSave = delegate(string text)
			{
				FontSize = editDialog.FontSize;
				Text = text;
				capi.Network.SendEntityPacket(entity.EntityId, 12312, SerializerUtil.Serialize(new TextDataPacket
				{
					Text = text,
					FontSize = FontSize
				}));
				Color = tempColor;
			};
			editDialog.OnCloseCancel = delegate
			{
				capi.Network.SendEntityPacket(entity.EntityId, 12313);
			};
			editDialog.TryOpen();
			capi.Network.SendEntityPacket(entity.EntityId, 12311);
		}
	}

	public override void OnReceivedClientPacket(IServerPlayer player, int packetid, byte[] data, ref EnumHandling handled)
	{
		base.OnReceivedClientPacket(player, packetid, data, ref handled);
		if (packetid == 12311)
		{
			if (!new Entity.CachedAccessPerms(entity, player).IsInteractingPlayerAllowedTo(EnumBlockAccessFlags.Use, validatePickRange: true, "entity behavior writing surface"))
			{
				return;
			}
			loadPigment(player.Entity);
		}
		if (packetid == 12313)
		{
			if (!new Entity.CachedAccessPerms(entity, player).IsInteractingPlayerAllowedTo(EnumBlockAccessFlags.Use, validatePickRange: false, "entity behavior writing surface"))
			{
				return;
			}
			player.Entity.TryGiveItemStack(tempStack);
			tempStack = null;
		}
		if (packetid != 12312)
		{
			return;
		}
		if (!new Entity.CachedAccessPerms(entity, player).IsInteractingPlayerAllowedTo(EnumBlockAccessFlags.Use, validatePickRange: true, "entity behavior writing surface"))
		{
			((ICoreServerAPI)entity.Api).Network.SendEntityPacket(player, entity.EntityId, 12313, SerializerUtil.Serialize(new TextDataPacket
			{
				FontSize = FontSize,
				Text = Text
			}));
			player.InventoryManager.TryGiveItemstack(tempStack);
			tempStack = null;
			return;
		}
		if (entity.World.Rand.NextDouble() < 0.85)
		{
			player.Entity.TryGiveItemStack(tempStack);
		}
		tempStack = null;
		TextDataPacket pkg = SerializerUtil.Deserialize<TextDataPacket>(data);
		Text = pkg.Text;
		FontSize = pkg.FontSize;
		entity.MarkShapeModified();
		Color = tempColor;
	}

	public override void OnReceivedServerPacket(int packetId, byte[] data, ref EnumHandling handled)
	{
		if (packetId == 12313)
		{
			TextDataPacket pkg = SerializerUtil.Deserialize<TextDataPacket>(data);
			Text = pkg.Text;
			FontSize = pkg.FontSize;
			entity.MarkShapeModified();
			handled = EnumHandling.Handled;
		}
		base.OnReceivedServerPacket(packetId, data, ref handled);
	}

	private bool loadPigment(EntityAgent eagent)
	{
		ItemSlot hotbarSlot = eagent.RightHandItemSlot;
		if (hotbarSlot != null && hotbarSlot.Itemstack?.ItemAttributes?["pigment"]?["color"].Exists == true)
		{
			JsonObject jsonObject = hotbarSlot.Itemstack.ItemAttributes["pigment"]["color"];
			int r = jsonObject["red"].AsInt();
			int g = jsonObject["green"].AsInt();
			int b = jsonObject["blue"].AsInt();
			tempColor = ColorUtil.ToRgba(255, r, g, b);
			if (eagent.World.Side == EnumAppSide.Server)
			{
				tempStack = hotbarSlot.TakeOut(1);
				hotbarSlot.MarkDirty();
			}
			return true;
		}
		return false;
	}

	public override void OnTesselation(ref Shape entityShape, string shapePathForLogging, ref bool shapeIsCloned, ref string[] willDeleteElements)
	{
		if (entity.World.Side == EnumAppSide.Server)
		{
			return;
		}
		ShapeElementFace[] facesResolved;
		if (previousText == Text && previousColor == Color && previousFontSize == FontSize)
		{
			if (Text != null && Text.Length != 0)
			{
				return;
			}
			if (!shapeIsCloned)
			{
				entityShape = entityShape.Clone();
			}
			ShapeElement ele = entityShape.GetElementByName("PlaqueLeftFrontText");
			if (ele == null)
			{
				return;
			}
			facesResolved = ele.FacesResolved;
			foreach (ShapeElementFace face in facesResolved)
			{
				if (face != null)
				{
					face.Texture = "transparent";
				}
			}
			return;
		}
		previousText = Text;
		previousColor = Color;
		previousFontSize = FontSize;
		ICoreClientAPI api = entity.Api as ICoreClientAPI;
		font.WithColor(ColorUtil.ToRGBADoubles(Color));
		loadedTexture?.Dispose();
		loadedTexture = null;
		font.UnscaledFontsize = FontSize / RuntimeEnv.GUIScale;
		double verPadding = ((verticalAlign == EnumVerticalAlign.Middle) ? ((double)TextHeight - api.Gui.Text.GetMultilineTextHeight(font, Text, TextWidth)) : 0.0);
		TextBackground bg = new TextBackground
		{
			VerPadding = (int)verPadding / 2
		};
		loadedTexture = api.Gui.TextTexture.GenTextTexture(Text, font, TextWidth, TextHeight, bg, EnumTextOrientation.Center);
		string textureCode = "writingsurface-" + SurfaceName + "-" + entity.EntityId;
		if (texPos == null)
		{
			api.EntityTextureAtlas.AllocateTextureSpace(TextWidth, TextHeight, out textureSubId, out texPos, assetloc = new AssetLocationAndSource(textureCode));
		}
		CompositeTexture ctex = new CompositeTexture(textureCode);
		entity.Properties.Client.Textures[textureCode] = ctex;
		ctex.Bake(entity.Api.Assets);
		ctex.Baked.TextureSubId = textureSubId;
		LoadedTexture atlas = api.EntityTextureAtlas.AtlasTextures[texPos.atlasNumber];
		api.Render.RenderTextureIntoTexture(loadedTexture, 0f, 0f, TextWidth, TextHeight, atlas, texPos.x1 * (float)atlas.Width, texPos.y1 * (float)atlas.Height, -1f);
		api.EntityTextureAtlas.RegenMipMaps(texPos.atlasNumber);
		if (!shapeIsCloned)
		{
			entityShape = entityShape.Clone();
		}
		ShapeElement elem = entityShape.GetElementByName("PlaqueLeftFrontText");
		if (elem == null)
		{
			return;
		}
		facesResolved = elem.FacesResolved;
		foreach (ShapeElementFace face2 in facesResolved)
		{
			if (face2 != null)
			{
				face2.Texture = textureCode;
			}
		}
		shapeIsCloned = true;
	}

	public override void OnEntityDespawn(EntityDespawnData despawn)
	{
		base.OnEntityDespawn(despawn);
		if (assetloc != null)
		{
			capi?.EntityTextureAtlas.FreeTextureSpace(assetloc);
		}
		assetloc = null;
		textureSubId = 0;
		texPos = null;
		loadedTexture?.Dispose();
		loadedTexture = null;
	}

	public override string PropertyName()
	{
		return "writingsurface";
	}
}
