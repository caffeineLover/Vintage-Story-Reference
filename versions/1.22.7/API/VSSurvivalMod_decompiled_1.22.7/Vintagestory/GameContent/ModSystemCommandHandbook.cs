using System.Collections.Generic;
using System.IO;
using System.Text;
using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.Config;
using Vintagestory.API.Server;
using Vintagestory.API.Util;

namespace Vintagestory.GameContent;

public class ModSystemCommandHandbook : ModSystem
{
	private ICoreClientAPI capi;

	private GuiDialogHandbook dialog;

	private ICoreServerAPI sapi;

	private ServerCommandsSyntax serverCommandsSyntaxClient;

	public event InitCustomPagesDelegate OnInitCustomPages;

	internal void TriggerOnInitCustomPages(List<GuiHandbookPage> pages)
	{
		this.OnInitCustomPages?.Invoke(pages);
	}

	public override bool ShouldLoad(EnumAppSide side)
	{
		return true;
	}

	public override void Start(ICoreAPI api)
	{
		api.Network.RegisterChannel("commandhandbook").RegisterMessageType<ServerCommandsSyntax>();
	}

	public override void StartServerSide(ICoreServerAPI api)
	{
		sapi = api;
		api.Event.PlayerNowPlaying += Event_PlayerNowPlaying;
		api.ChatCommands.Create("chbr").RequiresPlayer().RequiresPrivilege(Privilege.chat)
			.WithDescription("Reload command handbook texts")
			.HandleWith(onCommandHandbookReload);
	}

	private void Event_PlayerNowPlaying(IServerPlayer byPlayer)
	{
		sendSyntaxPacket(byPlayer);
	}

	private void sendSyntaxPacket(IServerPlayer byPlayer)
	{
		ServerCommandsSyntax cmdsyntaxPacket = genCmdSyntaxPacket(new Caller
		{
			Player = byPlayer,
			Type = EnumCallerType.Player
		});
		sapi.Network.GetChannel("commandhandbook").SendPacket(cmdsyntaxPacket, byPlayer);
	}

	private ServerCommandsSyntax genCmdSyntaxPacket(Caller caller)
	{
		List<ChatCommandSyntax> cmds = new List<ChatCommandSyntax>();
		foreach (KeyValuePair<string, IChatCommand> val in IChatCommandApi.GetOrdered(sapi.ChatCommands))
		{
			IChatCommand cmd = val.Value;
			cmds.Add(new ChatCommandSyntax
			{
				AdditionalInformation = cmd.AdditionalInformation,
				CallSyntax = cmd.CallSyntax,
				CallSyntaxUnformatted = cmd.CallSyntaxUnformatted,
				Description = cmd.Description,
				Examples = cmd.Examples,
				FullName = cmd.FullName,
				Name = val.Key,
				FullnameAlias = cmd.GetFullName(val.Key, isRootAlias: true),
				FullSyntax = cmd.GetFullSyntaxHandbook(caller, string.Empty, cmd.RootAliases?.Contains(val.Key) ?? false),
				Aliases = cmd.Aliases,
				RootAliases = cmd.RootAliases
			});
		}
		return new ServerCommandsSyntax
		{
			Commands = cmds.ToArray()
		};
	}

	public override void StartClientSide(ICoreClientAPI api)
	{
		capi = api;
		api.Network.GetChannel("commandhandbook").SetMessageHandler<ServerCommandsSyntax>(onServerCommandsSyntax);
		api.RegisterLinkProtocol("commandhandbook", onHandBookLinkClicked);
		api.ChatCommands.Create("chb").WithDescription("Opens the command hand book").RequiresPrivilege(Privilege.chat)
			.HandleWith(onCommandHandbook)
			.BeginSubCommand("expcmds")
			.WithDescription("Export all commands to a html format for the wiki.")
			.HandleWith(onCmd)
			.EndSubCommand();
	}

	private TextCommandResult onCmd(TextCommandCallingArgs args)
	{
		StringBuilder sb = new StringBuilder();
		sb.AppendLine("<div style=\"position: relative;\"><div style=\"position: absolute;left: 30%;max-width: 40%;\">");
		foreach (GuiHandbookPage page in dialog.allHandbookPages)
		{
			StringBuilder stringBuilder = sb;
			StringBuilder.AppendInterpolatedStringHandler handler = new StringBuilder.AppendInterpolatedStringHandler(9, 1, stringBuilder);
			handler.AppendLiteral("<h3>");
			handler.AppendFormatted(page.PageCode);
			handler.AppendLiteral("</h3>");
			stringBuilder.Append(ref handler);
			sb.Append(((GuiHandbookCommandPage)page).TextCacheAll.Replace("\n", "\n</br>").Replace("<strong>", "<b>").Replace("</strong>", "</b>"));
		}
		sb.AppendLine("</div></div>");
		GamePaths.EnsurePathExists(GamePaths.ModConfig);
		File.WriteAllText(Path.Combine(GamePaths.ModConfig, "cmds.html"), sb.ToString());
		return TextCommandResult.Success("exported all cmds");
	}

	private void onHandBookLinkClicked(LinkTextComponent comp)
	{
		string target = comp.Href.Substring("commandhandbook://".Length);
		target = target.Replace("\\", "");
		if (target.StartsWithOrdinal("tab-"))
		{
			if (!dialog.IsOpened())
			{
				dialog.TryOpen();
			}
			dialog.selectTab(target.Substring(4));
			return;
		}
		if (!dialog.IsOpened())
		{
			dialog.TryOpen();
		}
		if (target.Length > 0)
		{
			dialog.OpenDetailPageFor(target);
		}
	}

	private TextCommandResult onCommandHandbookReload(TextCommandCallingArgs args)
	{
		sendSyntaxPacket(args.Caller.Player as IServerPlayer);
		return TextCommandResult.Success("ok, reloaded");
	}

	private void onServerCommandsSyntax(ServerCommandsSyntax packet)
	{
		serverCommandsSyntaxClient = packet;
		dialog = new GuiDialogCommandHandbook(capi, onCreatePagesAsync, onComposePage);
		capi.Logger.VerboseDebug("Done initialising handbook");
	}

	private TextCommandResult onCommandHandbook(TextCommandCallingArgs args)
	{
		if (dialog.IsOpened())
		{
			dialog.TryClose();
		}
		else
		{
			dialog.TryOpen();
		}
		return TextCommandResult.Success();
	}

	private List<GuiHandbookPage> onCreatePagesAsync()
	{
		List<GuiHandbookPage> pages = new List<GuiHandbookPage>();
		foreach (KeyValuePair<string, IChatCommand> val in IChatCommandApi.GetOrdered(capi.ChatCommands))
		{
			if (capi.IsShuttingDown)
			{
				break;
			}
			IChatCommand cmd = val.Value;
			pages.Add(new GuiHandbookCommandPage(cmd, cmd.CommandPrefix + val.Key, "client", cmd.RootAliases?.Contains(val.Key) ?? false));
		}
		ChatCommandSyntax[] commands = serverCommandsSyntaxClient.Commands;
		foreach (ChatCommandSyntax cmd2 in commands)
		{
			if (capi.IsShuttingDown)
			{
				break;
			}
			pages.Add(new GuiHandbookCommandPage(cmd2, cmd2.FullnameAlias, "server"));
		}
		return pages;
	}

	private void onComposePage(GuiHandbookPage page, GuiComposer detailViewGui, ElementBounds textBounds, ActionConsumable<string> openDetailPageFor)
	{
		page.ComposePage(detailViewGui, textBounds, null, openDetailPageFor);
	}

	public override void Dispose()
	{
		base.Dispose();
		dialog?.Dispose();
	}
}
