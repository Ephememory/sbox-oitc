using Sandbox;
using Sandbox.UI;
using Sandbox.UI.Construct;
using System.Threading.Tasks;

namespace OITC;

public partial class KillfeedEntry
{
	private RealTimeSince _created { get; set; }

	public KillfeedEntry()
	{
		Killer = Add.Label( "", "left" );
		Method = Add.Label( "", "method" );
		Victim = Add.Label( "", "right" );

		_created = 0;
	}

	public override void Tick()
	{
		base.Tick();
		if ( _created >= 6f )
			Delete();
	}
}
