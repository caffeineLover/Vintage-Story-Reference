using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Mono.Nat;
using Open.Nat;
using Vintagestory.API.Common;
using Vintagestory.API.Config;
using Vintagestory.API.Server;
using Vintagestory.API.Util;

namespace Vintagestory.Server;

public class ServerSystemUpnp : ServerSystem
{
	private NatDevice natDevice;

	private IPAddress ipaddr;

	private INatDevice monoNatDevice;

	private Open.Nat.Mapping mapping;

	private Open.Nat.Mapping mappingUdp;

	private Mono.Nat.Mapping monoNatMapping;

	private Mono.Nat.Mapping monoNatMappingUdp;

	private bool wasOn;

	private long renewListenerId;

	public ServerSystemUpnp(ServerMain server)
		: base(server)
	{
		server.api.ChatCommands.Create("upnp").WithDescription("Runtime only setting. When turned on, the server will attempt to set up port forwarding through PMP or UPnP. When turned off, the port forward will be deleted again.").WithArgs(server.api.ChatCommands.Parsers.OptionalBool("on_off"))
			.RequiresPrivilege(Privilege.controlserver)
			.HandleWith(OnCmdToggleUpnp);
	}

	private TextCommandResult OnCmdToggleUpnp(TextCommandCallingArgs args)
	{
		bool nowon = (bool)args[0];
		if (nowon)
		{
			Initiate();
		}
		else
		{
			Dispose();
		}
		wasOn = nowon;
		server.Config.RuntimeUpnp = wasOn;
		return TextCommandResult.Success("Upnp mode now " + (nowon ? "on" : "off"));
	}

	public override void OnBeginRunGame()
	{
		int lifetime = ((!server.Config.UpnpInfiniteLifetime) ? 600 : 0);
		mapping = new Open.Nat.Mapping(Open.Nat.Protocol.Tcp, server.CurrentPort, server.CurrentPort, lifetime, "Vintage Story TCP");
		mappingUdp = new Open.Nat.Mapping(Open.Nat.Protocol.Udp, server.CurrentPort, server.CurrentPort, lifetime, "Vintage Story UDP");
		monoNatMapping = new Mono.Nat.Mapping(Mono.Nat.Protocol.Tcp, server.CurrentPort, server.CurrentPort, lifetime, "Vintage Story TCP");
		monoNatMappingUdp = new Mono.Nat.Mapping(Mono.Nat.Protocol.Udp, server.CurrentPort, server.CurrentPort, lifetime, "Vintage Story UDP");
		wasOn = server.Config.Upnp;
		if (wasOn && server.IsDedicatedServer)
		{
			Initiate();
		}
		server.Config.onUpnpChanged += onUpnpChanged;
	}

	private void onUpnpChanged()
	{
		if (wasOn && !server.Config.Upnp)
		{
			Dispose();
		}
		if (!wasOn && server.Config.Upnp)
		{
			Initiate();
		}
		wasOn = server.Config.Upnp;
	}

	public void Initiate()
	{
		string str;
		ServerMain.Logger.Event(str = "Begin searching for PMP and UPnP devices...");
		server.SendMessageToGroup(GlobalConstants.ServerInfoChatGroup, str, EnumChatType.Notification);
		findPmpDeviceAsync();
	}

	private async Task findUpnpDeviceAsync()
	{
		CancellationTokenSource cts = new CancellationTokenSource(5000);
		try
		{
			onFoundNatDevice(await new NatDiscoverer().DiscoverDeviceAsync(PortMapper.Upnp, cts), "UPnP");
		}
		catch (Exception)
		{
			findUpnpDeviceWithMonoNat();
		}
		cts.Dispose();
	}

	private async Task findPmpDeviceAsync()
	{
		CancellationTokenSource cts = new CancellationTokenSource(5000);
		try
		{
			onFoundNatDevice(await new NatDiscoverer().DiscoverDeviceAsync(PortMapper.Pmp, cts), "PMP");
		}
		catch (Exception)
		{
			await findUpnpDeviceAsync();
		}
		cts.Dispose();
	}

	private void findUpnpDeviceWithMonoNat()
	{
		if (server.RunPhase != EnumServerRunPhase.Shutdown)
		{
			string str = "No upnp or pmp device found after 5 seconds. Trying another method...";
			ServerMain.Logger.Event(str);
			server.SendMessageToGroup(GlobalConstants.ServerInfoChatGroup, str, EnumChatType.Notification);
			NatUtility.DeviceFound += MonoNatDeviceFound;
			NatUtility.StartDiscovery();
			server.RegisterCallback(After5s, 5000);
		}
	}

	private void After5s(float dt)
	{
		if (monoNatDevice == null)
		{
			NatUtility.StopDiscovery();
			string str = "No upnp or pmp device found using either method. You may try to disable Upnp600SecLifetime in the serverconfig.json or with the command /sc upnp600seclifetime off and then restart the singleplayer world.";
			ServerMain.Logger.Event(str);
			server.SendMessageToGroup(GlobalConstants.ServerInfoChatGroup, str, EnumChatType.Notification, null, "nonatdevice");
			server.EventManager.TriggerUpnpComplete(success: false);
		}
		NatUtility.DeviceFound -= MonoNatDeviceFound;
	}

	private void CreateRenewCallback()
	{
		if (!server.Config.UpnpInfiniteLifetime && renewListenerId == 0L)
		{
			renewListenerId = server.RegisterCallback(RenewMapping, 540000);
		}
	}

	private void RenewMapping(float delta)
	{
		TyronThreadPool.QueueTask(async delegate
		{
			_ = 2;
			try
			{
				if (monoNatDevice != null)
				{
					monoNatDevice.CreatePortMap(monoNatMapping);
					monoNatDevice.CreatePortMap(monoNatMappingUdp);
					ipaddr = monoNatDevice.GetExternalIP();
				}
				if (natDevice != null)
				{
					await natDevice.CreatePortMapAsync(mapping);
					await natDevice.CreatePortMapAsync(mappingUdp);
					ipaddr = await natDevice.GetExternalIPAsync();
				}
			}
			catch (Exception e)
			{
				ServerMain.Logger.Warning("Failed to renew UnPn Port mapping, removing UPnP");
				ServerMain.Logger.Warning(e);
				Dispose();
			}
		}, "UPNPRenewMapping");
	}

	private void MonoNatDeviceFound(object sender, DeviceEventArgs e)
	{
		try
		{
			monoNatDevice = e.Device;
			monoNatDevice.CreatePortMap(monoNatMapping);
			monoNatDevice.CreatePortMap(monoNatMappingUdp);
			CreateRenewCallback();
			ipaddr = monoNatDevice.GetExternalIP();
			SendNatMessage();
		}
		catch (Exception e2)
		{
			ServerMain.Logger.Error("mono port map threw an exception:");
			ServerMain.Logger.Error(e2);
			monoNatDevice = null;
			ipaddr = null;
		}
	}

	private async void onFoundNatDevice(NatDevice device, string type)
	{
		if (natDevice != null)
		{
			return;
		}
		try
		{
			natDevice = device;
			ipaddr = await natDevice.GetExternalIPAsync();
			await natDevice.CreatePortMapAsync(mapping);
			await natDevice.CreatePortMapAsync(mappingUdp);
			CreateRenewCallback();
			SendNatMessage();
		}
		catch (Exception)
		{
			natDevice = null;
			if (type == "PMP")
			{
				findUpnpDeviceAsync();
			}
			if (type == "UPnP")
			{
				findUpnpDeviceWithMonoNat();
			}
		}
	}

	private void SendNatMessage()
	{
		if (NetUtil.IsPrivateIp(ipaddr.ToString()))
		{
			string str = $"Device with external ip {ipaddr.ToString()} found, but this is a private ip! Might not be accessible. Created mapping for port {mapping.PublicPort} anyway.";
			ServerMain.Logger.Event(str);
			server.SendMessageToGroup(GlobalConstants.ServerInfoChatGroup, str, EnumChatType.Notification, null, "foundnatdeviceprivip:" + ipaddr);
		}
		else
		{
			string str2 = $"Device with external ip {ipaddr.ToString()} found. Created mapping for port {mapping.PublicPort}!";
			ServerMain.Logger.Event(str2);
			server.SendMessageToGroup(GlobalConstants.ServerInfoChatGroup, str2, EnumChatType.Notification, null, "foundnatdevice:" + ipaddr);
		}
		server.EventManager.TriggerUpnpComplete(success: true);
	}

	public override void Dispose()
	{
		if (natDevice != null)
		{
			ServerMain.Logger.Event("Deleting port map on device with external ip {0}", ipaddr.ToString());
			TyronThreadPool.QueueTask(async delegate
			{
				await natDevice.DeletePortMapAsync(mapping);
				await natDevice.DeletePortMapAsync(mappingUdp);
			}, "UPNPDeleteMapping");
		}
		if (monoNatDevice != null)
		{
			ServerMain.Logger.Event("Deleting port map on device with external ip {0}", ipaddr.ToString());
			monoNatDevice.DeletePortMap(monoNatMapping);
			monoNatDevice.DeletePortMap(monoNatMappingUdp);
		}
		server.UnregisterCallback(renewListenerId);
		renewListenerId = 0L;
		natDevice = null;
		monoNatDevice = null;
	}
}
