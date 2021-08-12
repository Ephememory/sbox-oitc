using Sandbox;
using Sandbox.UI;
using Sandbox.UI.Construct;

public partial class HudHealth : Panel
{
	public Label label;
	public HudHealth()
	{
		label = Add.Label( "", "value" );
	}

	public override void Tick()
	{
		var player = Local.Pawn as BBPlayer;
		if ( player == null ) return;
		label.SetText( $"♥️ {player.Health.CeilToInt()}" );
		if ( player.Health.InRange( 50, 100 ) )
		{
			label.Style.FontColor = Color.FromBytes( 184, 212, 59, 150 );
		}
		else if ( player.Health.InRange( 13, 50 ) )
		{
			label.Style.FontColor = Color.FromBytes( 168, 140, 37, 200 );
		}
		else if ( player.Health.InRange( 0, 13 ) )
		{
			label.Style.FontColor = Color.Red;
		}

		label.Style.Dirty();
	}
}

