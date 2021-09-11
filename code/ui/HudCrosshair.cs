using Sandbox;
using Sandbox.UI;

public partial class HudCrosshair : Panel
{
	public HudCrosshair()
	{
		Panel center = new Panel();
		AddChild( center );
		center.AddClass( "center" );
		this.PositionAtCrosshair();
	}

	public override void Tick()
	{

		this.PositionAtCrosshair();
		base.Tick();
	}


}
