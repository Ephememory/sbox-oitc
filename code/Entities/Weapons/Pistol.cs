namespace OITC;

public partial class Pistol : Weapon
{
	public override string ViewModelPath => "models/weapons/v_pistol.vmdl";

	public override string GetKilledByText()
	{
		var options = new string[6] { "smoked", "popped", "gunned down", "iced", "spun the block on", "poked" };
		return Game.Random.FromArray<string>( options );
	}

	public override void Spawn()
	{
		base.Spawn();
		SetModel( "models/weapons/pistol/pistol.vmdl" );
	}

	public override bool CanReload()
	{
		return false;
	}

	public override void AttackPrimary()
	{
		base.AttackPrimary();
		TimeSincePrimaryAttack = 0;

		if ( Owner is not BBPlayer player )
			return;

		if ( player.PistolAmmo <= 0 )
			return;


		(Owner as AnimatedEntity)?.SetAnimParameter( "b_attack", true );
		player.RemoveAmmo( 1 );
		ShootEffects();
		PlaySound( "oitc_pistolsound" );
		ShootBullet( 0, 1, 1000, 1 );
	}

	public override void AttackSecondary()
	{

	}

	[ClientRpc]
	protected override void ShootEffects()
	{
		Game.AssertClient();

		ViewModelEntity?.SetAnimParameter( "fire", true );
		Particles.Create( "particles/pistol_ejectbrass.vpcf", EffectEntity, "ejection" );
		base.ShootEffects();
	}

	public override void SimulateAnimator( CitizenAnimationHelper anim )
	{
		anim.HoldType = CitizenAnimationHelper.HoldTypes.Pistol;
		anim.Handedness = CitizenAnimationHelper.Hand.Right;
	}
}
