using Sandbox;
using Sandbox.UI;

public partial class HudCrosshair : Panel
{
	public HudCrosshair()
	{
		Panel center = new Panel();
		AddChild( center );
		center.AddClass( "center" );
	}


}
