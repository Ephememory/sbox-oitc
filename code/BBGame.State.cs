
namespace OITC;

public enum GameState : byte
{
	WaitingForPlayers,
	Warmup,
	MidGame,
	RoundOver
}

partial class BBGame
{
	public int NumPlayers = 0;

	public partial class GameStateInfo : BaseNetworkable
	{
		[Net] public ulong TopFragSteamId { get; set; }
		[Net] public string TopFragName { get; set; }
		[Net] public GameState Tier { get; set; }
		[Net] public string Text { get; set; }
	}

	private void ReCalculateGameState()
	{
		Game.AssertServer();
			
		State ??= new();

		if ( State.Tier == GameState.RoundOver )
			return;

		if ( NumPlayers < 2 )
		{
			State.Tier = GameState.WaitingForPlayers;
			State.Text = "Waiting for players...";
		}
	}
}
