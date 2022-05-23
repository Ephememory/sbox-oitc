using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sandbox;
using Sandbox.UI;
using Sandbox.UI.Construct;



[Library]
public partial class BBHud : HudEntity<RootPanel>
{

	public BBHud()
	{

		if ( !IsClient ) return;
		RootPanel.StyleSheet.Load( "ui/stylesheet.scss" );
		RootPanel.AddChild<ChatBox>();
		RootPanel.AddChild<VoiceList>();
		RootPanel.AddChild<Scoreboard<ScoreboardEntry>>();
		//RootPanel.AddChild<NameTags>();
		//RootPanel.AddChild<KillFeed>();
		RootPanel.AddChild<HudGameState>();
		RootPanel.AddChild<HudGameRestartTime>();
		RootPanel.AddChild<HudCrosshair>();
		RootPanel.AddChild<HudHealth>();
		RootPanel.AddChild<HudFlashlight>();
		RootPanel.AddChild<HudAmmo>();

		//just a seperate HTML panel for hud iterations with less csharp
		var htmlPanel = new Panel();
		RootPanel.AddChild( htmlPanel );
		htmlPanel.SetTemplate( "code/ui/HTMLPanel.html" );
		htmlPanel.StyleSheet.Load( "code/ui/htmlpanel.scss" );
	}


	public class DebugPanel : Panel
	{
		Label label;

		public DebugPanel()
		{
			label = Add.Label( "", "value" );

		}


		public override void Tick()
		{
			base.Tick();
			label.SetText( Global.IsListenServer ? "Server" : "Client" );
		}

	}
}


