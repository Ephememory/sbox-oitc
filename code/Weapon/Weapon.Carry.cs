namespace OITC;

partial class Weapon
{
	public virtual bool CanCarry( Entity carrier )
	{
		return true;
	}

	public virtual void OnCarryStart( Entity carrier )
	{
		if ( Game.IsClient ) return;

		SetParent( carrier, true );
		Owner = carrier;
		EnableAllCollisions = false;
		EnableDrawing = false;
	}

	public virtual void OnCarryDrop( Entity dropper )
	{
		if ( Game.IsClient ) return;

		SetParent( null );
		Owner = null;
		EnableDrawing = true;
		EnableAllCollisions = true;
	}
}
