
namespace OITC;

public static class Events
{
	public const string OnPlayerAmmoChanged = "oitc.player_ammo_change";
	
	/// <summary>
	/// Called when a remote player kills another player.
	/// Could be our local player or another remote player.
	/// </summary>
	public class OnPlayerKilledClientAttribute : EventAttribute
	{
		public const string OnPlayerKilledClient = "oitc.player_killed";

		public OnPlayerKilledClientAttribute() : base( OnPlayerKilledClient ) { }
	}
}
