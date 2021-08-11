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
		Length? newWidth = 0;

		public CrossHairHUD()
		{
			_class = new HashSet<string>() { "CrossHairHUD" };

			Panel center = new Panel();
			AddChild( center );
			center.AddClass( "center" );


		}


		public override void Tick()
		{
			base.Tick();
			if ( Time.Tick % 50 != 1 ) return;
			// newWidth = Length.Pixels( Rand.Int( 2, 8 ) );
			// Style.MarginLeft = Length.Pixels( -newWidth.Value.Value / 2 );
			// Style.Width = newWidth;
			Style.Dirty();
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


