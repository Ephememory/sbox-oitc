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
		RootPanel.AddChild<CrossHairHUD>();
		RootPanel.AddChild<DebugPanel>();
		RootPanel.AddChild<HudHealth>();
		RootPanel.AddChild<HudFlashlight>();
	}


	public class CrossHairHUD : Panel
	{
		public static CrossHairHUD Singleton;

		public CrossHairHUD()
		{
			_class = new HashSet<string>() { "CrossHairHUD" };
			Singleton = this;
			//StyleSheet.Load( "/ui/crosshair/CrosshairCanvas.scss" );
		}


		public static void SetCrosshair( Panel crosshairPanel )
		{
			if ( Singleton == null )
				return;

			Singleton.DeleteChildren();
			crosshairPanel.Parent = Singleton;
		}
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


