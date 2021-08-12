using Sandbox;
using Sandbox.UI;
using Sandbox.UI.Construct;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public partial class HudFlashlight : Panel
{

	public Label label;

	public HudFlashlight()
	{
		label = Add.Label( " ", "value" );
	}


	public override void Tick()
	{
		var player = Local.Pawn as BBPlayer;
		if ( player == null ) return;
		label.SetText( $"🔦 {player.FlashlightBatteryCharge.CeilToInt()}" );
		if ( player.FlashlightBatteryCharge.InRange( 50, 100 ) )
		{
			label.Style.FontColor = Color.FromBytes( 184, 212, 59, 150 );
			label.Style.Dirty();
		}
		else if ( player.FlashlightBatteryCharge.InRange( 13, 50 ) )
		{
			label.Style.FontColor = Color.FromBytes( 168, 140, 37, 200 );
			label.Style.Dirty();
		}
		else if ( player.FlashlightBatteryCharge.InRange( 0, 13 ) )
		{
			label.Style.FontColor = Color.Red;
			label.Style.Dirty();
		}

		//if(player.FlashlightBatteryCharge < 100)
		//{
		//	Style.Width = 230;
		//	Style.Dirty();
		//}
		//else
		//{
		//	Style.Width = 240;
		//	Style.Dirty();
		//}

	}
}

