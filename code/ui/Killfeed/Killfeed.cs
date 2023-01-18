
using Sandbox.UI;

namespace OITC;

partial class Killfeed : Panel
{
	public static Killfeed Current;

	public Killfeed()
	{
		if ( !Game.IsClient ) return;
		Current = this;
	}

	public Panel AddEntry( IClient killer, IClient victim, string method )
	{
		var e = Current.AddChild<KillfeedEntry>();

		e.AddClass( method );

		if ( killer != null && killer.Pawn is BBPlayer k )
		{
			e.Killer.Text = killer.Name;
			e.Killer.SetClass( "me", killer.Id == Game.LocalClient.SteamId);
			e.Killer.Style.FontColor = Color.White;
		}

		e.Method.Text = $"{method} ";

		if ( victim != null && victim.Pawn is BBPlayer v )
		{
			e.Victim.Text = victim.Name;
			e.Victim.SetClass( "me", victim.Id == Game.LocalClient.SteamId);
			e.Victim.Style.FontColor = Color.White;
		}

		if ( killer != null && victim != null )
		{
			Log.Info( $"{killer.Name} {method} {victim.Name}" );
		}

		return e;
	}

}
