using Sandbox;
using Sandbox.UI;
using Sandbox.UI.Construct;
using System;


public partial class HudGameRestartTime : Panel
{
	private Label label;
	public static float countDown;
	private static bool shouldCountDown = false;
	private static float lastTime = 0;
	public static Action OnRoundOver;
	public HudGameRestartTime()
	{
		label = Add.Label( "5", "value" );
		OnRoundOver += () =>
		{
			SetClass( "toggled", true );
			countDown = 6;
			shouldCountDown = true;
			EndTimer();
		};
	}

	public override void Tick()
	{
		if ( shouldCountDown && Time.Now - 1 >= lastTime )
		{
			label.SetText( $"{countDown -= 1}" );
			lastTime = Time.Now;
		}

		base.Tick();
	}

	private async void EndTimer()
	{
		await GameTask.DelayRealtimeSeconds( 5 );
		SetClass( "toggled", false );
		shouldCountDown = false;
	}

}
