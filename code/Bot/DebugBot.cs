#if DEBUG
using Editor;
using Sandbox.Internal;
using System.Collections.Generic;
using System.Linq;

namespace OITC;

public partial class DebugBot : Bot
{
	/// <summary>
	/// The Player this bot controls.
	/// </summary>
	public Player Pawn;

	/// <summary>
	/// The host Player.
	/// </summary>
	public Player Target;

	public bool WishAttack { get; set; }

	public static bool Aimbot;
	public static bool Mimic;
	public static bool Wander;

	[ConVar.Replicated( "bot_debug" )]
	public static bool DrawDebug { get; set; }

	[ConCmd.Admin( "bot_aimbot", Help = "Locks the bot's aim to the player." )]
	public static void ToggleBotAimbot()
	{
		Aimbot = !Aimbot;
	}

	[ConCmd.Admin( "bot_mimic", Help = "Makes the bot mimic the host client's inputs." )]
	public static void ToggleMimicHost()
	{
		Mimic = !Mimic;
		Wander = false;
	}

	[ConCmd.Admin( "bot_wander", Help = "Makes the bot randomly press move buttons." )]
	public static void ToggleWander()
	{
		Wander = !Wander;
		Mimic = false;
	}

	[ConCmd.Admin( "bot_zombie", Help = "Resets bot to default settings." )]
	public static void DoBotZombie()
	{
		Wander = false;
		Mimic = false;
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
		if ( pawn is not Player ply )
			return;

		var b = new DebugBot();
		b.Target = ply;
	}

	[ConCmd.Admin( "bot_attack" )]
	public static void BotAttack()
	{
		foreach ( var bot in Bot.All.ToArray() )
		{
			if ( bot is not DebugBot b )
				continue;

			b.WishAttack = true;
		}
	}

	[ConCmd.Admin( "bot_got" )]
	public static void BotGod()
	{
		foreach ( var bot in Bot.All.ToArray() )
		{
			if ( bot is not DebugBot b )
				continue;

			b.Pawn.Health *= 1000;
		}
	}

	[ConCmd.Admin( "bot_tele" )]
	public static void BotTeleport()
	{
		if ( ConsoleSystem.Caller.Pawn is not Player player )
			return;

		foreach ( var bot in Bot.All.ToArray() )
		{
			player.Position = bot.Client.Position;
		}
	}

	internal static List<string> _defaultNames = new List<string>
	{
		"IDentifyYou",
		"legendary_gamer",
		"ImmortalSoul",
		"I Am A Myth",
		"I_am_a_rider",
		"i_am_a_guardian",
		"gangsOFbeast",
		"deadly_gamer",
		"sharkLord",
		"I_AM_THE_DANGER",
		"UnicornHunter",
		"BattleKing",
		"Dark-Lord",
		"BillieJean",
		"YumtheBOSS",
		"TheGodFather",
		"A-Living-Zombie",
		"LordLucifer",
		"Myth1c",
		"CrazyGamer",
		"Shadow Dagger",
		"Beg-for-my-mercy",
		"Shoot2kill",
		"AggressiveKiller",
		"ObjectToHonor",
		"IamTheDanger",
		"Incredible Hulk",
		"Can’tTouchThis",
		"MasterOftheALL",
		"Perfect Player",
		"king_of_epic_fails",
		"The Executer",
		"TalkLessWinMore",
		"RIPPER",
		"God of the wars",
		"SniperGotYou",
		"Bomb Attacker",
		"Watch your back",
		"MasterOFDisaster",
		"Dominatrix",
	};

	internal static List<string> _shortNames = new List<string>
	{
		"Al",
		"D",
		"Hex",
		"Nick",
		"Timmy",
		"Op"
	};

	static DebugBot()
	{
		SetDefaultNames( _defaultNames );
	}

	public override void BuildInput()
	{
		Pawn ??= (Client.Pawn as Player);

		if ( DrawDebug )
		{
			DebugOverlay.Axis( Pawn.EyePosition, Pawn.EyeRotation, 16 );
			DebugOverlay.Axis( Pawn.Position, Pawn.Rotation, 24 );
		}

		if ( Wander )
			Pawn.InputDirection = Vector3.Random * 2f;

		if ( Mimic )
		{
			Input.CopyLastInput( Target.Client );

			foreach ( var item in from p in GlobalGameNamespace.TypeLibrary.GetPropertyDescriptions( Client.Pawn )
								  where p.HasAttribute<ClientInputAttribute>()
								  select p )
				item.SetValue( Client.Pawn, item.GetValue( Target ) );

		}

		Input.SetButton( InputButton.PrimaryAttack, WishAttack );
		WishAttack = false;

		if ( Aimbot )
		{
			var lookAt = Rotation.LookAt( Pawn.EyePosition - Target.EyePosition );
			Pawn.ViewAngles = Rotation.From( -lookAt.Pitch(), lookAt.Yaw(), lookAt.Roll() ).Angles();
		}


	}
}
#endif
