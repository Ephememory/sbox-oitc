using Sandbox;
using Sandbox.UI;
using Sandbox.UI.Construct;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public partial class HudAmmo : Panel
{

	public Label label;

	public HudAmmo()
	{
		label = Add.Label( " ", "value" );
	}

	public override void Tick()
	{
		var player = Local.Pawn as BBPlayer;
		if ( player == null ) return;
		label.SetText( $"🍌 {player.BananaAmmo}" );

		if ( player.BananaAmmo.InRange( 6, 7 ) )
		{
			label.Style.FontColor = Color.FromBytes( 184, 212, 59, 150 );
			label.Style.Dirty();
		}
		else if ( player.BananaAmmo.InRange( 4, 6 ) )
		{
			label.Style.FontColor = Color.FromBytes( 168, 140, 37, 200 );
			label.Style.Dirty();
		}
		else if ( player.BananaAmmo.InRange( 0, 4 ) )
		{
			label.Style.FontColor = Color.Red;
			label.Style.Dirty();
		}

	}
}

