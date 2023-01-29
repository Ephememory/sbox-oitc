#if DEBUG

using Sandbox.Internal;
using System.Linq;

namespace OITC;

public partial class GameBot : Bot
{
	/// <summary>
	/// The Player this bot controls.
	/// </summary>
	public BBPlayer Pawn;

	/// <summary>
	/// The host Player.
	/// </summary>
	public BBPlayer Target;

	public bool WishAttack { get; set; }

	public static bool Aimbot;
	public static bool MimicHost;
	public static bool Wander;

	[ConVar.Replicated( "bot_debug" )]
	public static bool Debug { get; set; }

	[ConCmd.Admin( "bot_aimbot", Help = "Locks the bot's aim to the player." )]
	public static void ToggleBotAimbot()
	{
		Aimbot = !Aimbot;
	}

	[ConCmd.Admin( "bot_mimic", Help = "Makes the bot mimic the host client's inputs." )]
	public static void ToggleMimicHost()
	{
		MimicHost = !MimicHost;
		Wander = false;
	}

	[ConCmd.Admin( "bot_wander", Help = "Makes the bot randomly press move buttons." )]
	public static void ToggleWander()
	{
		Wander = !Wander;
		MimicHost = false;
	}

	[ConCmd.Admin( "bot_zombie", Help = "Resets bot to default settings." )]
	public static void DoBotZombie()
	{
		Wander = false;
		MimicHost = false;
		Aimbot = false;
	}

	[ConCmd.Admin( "bot_kill", Help = "Kills bot by name or all." )]
	public static void DoBotKill( string name = "" )
	{
		if ( string.IsNullOrEmpty( name ) )
		{
			foreach ( var b in All.ToArray() )
			{
				if ( b.Client.Pawn is Entity ent )
					ent.OnKilled();
			}

			return;
		}

		var nameLower = name.ToLower();
		foreach ( var b in All.ToArray() )
		{
			if ( b.Client.Name.ToLower() == nameLower && b.Client.Pawn is Entity ent )
				ent.OnKilled();
		}
	}

	[ConCmd.Admin( "bot_kick", Help = "Kicks all bots." )]
	public static void DoBotKick()
	{
		foreach ( var b in All.ToArray() )
			b.Client.Kick();
	}

	[ConCmd.Admin( "bot_add" )]
	public static void AddBot()
	{
		var pawn = ConsoleSystem.Caller.Pawn;
		if ( pawn is not BBPlayer ply )
			return;

		var b = new GameBot();
		b.Target = ply;
	}

	[ConCmd.Admin( "bot_attack" )]
	public static void BotAttack()
	{
		foreach ( var bot in Bot.All.ToArray() )
		{
			if ( bot is not GameBot b )
				continue;

			b.WishAttack = true;
		}
	}

	public override void BuildInput()
	{
		Pawn ??= Client.Pawn as BBPlayer;
		Input.SetButton( InputButton.PrimaryAttack, WishAttack );
		WishAttack = false;

		if ( Debug )
		{
			DebugOverlay.Axis( Pawn.EyePosition, Pawn.EyeRotation, 16 );
			DebugOverlay.Axis( Pawn.Position, Pawn.Rotation, 24 );
		}

		if ( Wander )
			Pawn.InputDirection = Vector3.Random * 2f;

		if ( MimicHost )
		{
			Input.CopyLastInput( Target.Client );

			var inputs = from p in GlobalGameNamespace.TypeLibrary.GetPropertyDescriptions( Client.Pawn )
						 where p.HasAttribute<ClientInputAttribute>()
						 select p;

			foreach ( PropertyDescription item in inputs )
				item.SetValue( Client.Pawn, item.GetValue( Target ) );

		}

		if ( Aimbot )
		{
			var forward = Pawn.EyePosition - Target.EyePosition;
			var lookAt = Rotation.LookAt( forward );

			Pawn.ViewAngles = Rotation.From( -lookAt.Pitch(), lookAt.Yaw(), lookAt.Roll() ).Angles();
		}
	}
}
#endif
