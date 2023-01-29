namespace OITC;

public partial class Fists : Weapon
{
	public override string ViewModelPath => "models/weapons/fists/v_fists.vmdl";
	public override float PrimaryRate => 0.9f;
	public override float ReloadTime => 0f;

	public override string GetKilledByText()
	{
		var options = new string[5] { "beat down", "pummeled", "clocked", "clobbered", "milly rocked" };
		return Game.Random.FromArray<string>( options );
	}

	public override bool CanReload()
	{
		return false;
	}

	public override void AttackPrimary()
	{
		base.AttackPrimary();
		TimeSincePrimaryAttack = 0;

		(Owner as AnimatedEntity)?.SetAnimParameter( "b_attack", true );
		ShootEffects();
		Melee( 1000f, 90f );
	}

	public override void AttackSecondary()
	{
		//Do absolutely nothing
	}

	/// <summary>
	/// Custom ShootBullets function for melee.
	/// </summary>
	/// <param name="damage"></param>
	/// <param name="range"></param>
	/// <returns></returns>
	public bool Melee( float damage, float range = DefaultBulletRange )
	{
		if ( !Owner.IsValid() || Owner is not BBPlayer ply )
			return false;

		var pos = ply.EyePosition;
		var forward = ply.EyeRotation.Forward;
		forward = forward.Normal;

		foreach ( var tr in TraceBullet( pos, pos + forward * range, 15f ) )
		{
			if ( !tr.Entity.IsValid() )
			{
				continue;

			}

			tr.Surface.DoBulletImpact( tr );

			if ( !Game.IsServer )
				continue;

			using ( Prediction.Off() )
			{
				var damageInfo = DamageInfo.FromBullet( tr.EndPosition, forward * 100 * 1, damage )
					.UsingTraceResult( tr )
					.WithAttacker( Owner )
					.WithWeapon( this );

				tr.Entity.TakeDamage( damageInfo );
				PlaySound( "punch" );
				return true;
			}
		}

		if ( Game.IsServer )
			PlaySound( "punch_miss" );

		return false;
	}

	[ClientRpc]
	protected override void ShootEffects()
	{
		Game.AssertClient();

		ViewModelEntity?.SetAnimParameter( "fire", true );
		//CrosshairPanel?.CreateEvent( "fire" );
	}

	public override void SimulateAnimator( CitizenAnimationHelper anim )
	{
		anim.HoldType = CitizenAnimationHelper.HoldTypes.Punch;
	}

	public override void OnCarryDrop( Entity dropper )
	{
		Delete();
	}
}
