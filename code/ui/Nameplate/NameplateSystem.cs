using OITC;
using Sandbox.UI;
using Sandbox.UI.Construct;
using System.Linq;
using System.Xml;

/// <summary>
/// When a player is within radius of the camera we add this to their entity.
/// We remove it again when they go out of range.
/// </summary>
internal class NameTagComponent : EntityComponent<BBPlayer>
{
	Nameplate NameTag;

	protected override void OnActivate()
	{
		var steamid = Entity.Client.SteamId;
		var name = Entity.Client.Name;

		NameTag = new Nameplate();
		NameTag.Name = name;
		NameTag.SteamId = steamid;
		NameTag.PanelBounds = new Rect( -500, -100, 1000, 200 );
	}

	protected override void OnDeactivate()
	{
		NameTag?.Delete();
		NameTag = null;
	}

	/// <summary>
	/// Called for every tag, while it's active
	/// </summary>
	[Event.Client.Frame]
	public void FrameUpdate()
	{
		var tx = Entity.GetAttachment( "hat" ) ?? Entity.Transform;
		tx.Rotation = Rotation.LookAt( -Camera.Rotation.Forward );
		NameTag.Transform = tx;
	}

	/// <summary>
	/// Called once per frame to manage component creation/deletion
	/// </summary>
	[Event.Client.Frame]
	public static void SystemUpdate()
	{
		var localPawn = Game.LocalPawn as BBPlayer;
		foreach ( var player in Sandbox.Entity.All.OfType<BBPlayer>() )
		{
			if ( player.IsLocalPawn && player.IsFirstPersonMode )
			{
				var c = player.Components.Get<NameTagComponent>();
				c?.Remove();
				continue;
			}

			// If our local pawn is dead we are in the death cam, keep the nameplates on
			// so we have a better chance of seeing who killed us in case we miss the KillFeed entry.
			var shouldRemove = player.Position.Distance( Camera.Position ) > 600 && !(localPawn.LifeState == LifeState.Dead);
			shouldRemove = shouldRemove || player.LifeState != LifeState.Alive;
			shouldRemove = shouldRemove || player.IsDormant;

			if ( shouldRemove )
			{
				var c = player.Components.Get<NameTagComponent>();
				c?.Remove();
				continue;
			}

			// Add a component if it doesn't have one
			player.Components.GetOrCreate<NameTagComponent>();
		}
	}
}
