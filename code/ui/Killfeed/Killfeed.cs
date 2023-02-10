
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
		var entry = Current.AddChild<KillfeedEntry>();

		if ( killer != null && killer.Pawn is Player k )
		{
			entry.Killer.Text = killer.Name;
			entry.Killer.SetClass( "me", killer.SteamId == Game.LocalClient.SteamId );
		}

		entry.Method.Text = $"{method} ";

		if ( victim != null && victim.Pawn is Player v )
		{
			entry.Victim.Text = victim.Name;
			entry.Victim.SetClass( "me", victim.SteamId == Game.LocalClient.SteamId );
		}

		if ( killer != null && victim != null )
		{
			Log.Info( $"{killer.Name} {method} {victim.Name}" );
		}

		return entry;
	}

}
