using System;
using Sandbox;

public partial class BBPlayer : Player
{
	[Net]
	[Predicted]
	public SpotLightEntity FlashlightEntity { get; private set; }
	[Net]
	public bool FlashlightEnabled { get; private set; }
	[Net]
	[Predicted]
	public float FlashlightPosOffset { get; private set; }
	[Net]
	public float FlashlightBatteryCharge { get; private set; } = 100f;

	[ConVar.Replicated]
	public static float oitc_flashlight_drain_amt { get; set; } = 1;

	private void TickFlashLight()
	{
		if ( Input.Released( InputButton.Flashlight ) && FlashlightBatteryCharge > 0 )
		{
			FlashlightEnabled = !FlashlightEnabled;
			using ( Prediction.Off() )
			{
				PlayClientSound( FlashlightEntity.Enabled ? "flashlight-on" : "flashlight-off" );
			}
		}

		if ( !FlashlightEntity.IsValid() )
			return;

		FlashlightEntity.Enabled = FlashlightEnabled && FlashlightBatteryCharge > 0f;

		if ( FlashlightEnabled && FlashlightBatteryCharge > 0f && Time.Tick % 14 == 1 )
		{
			if ( FlashlightBatteryCharge - oitc_flashlight_drain_amt < 0 )
			{
				FlashlightBatteryCharge = -1;
			}
			else
			{
				FlashlightBatteryCharge -= oitc_flashlight_drain_amt;
			}

			if ( FlashlightBatteryCharge < 1 )
			{
				FlashlightEnabled = false;
				FlashlightEntity.TurnOff();
				using ( Prediction.Off() )
				{
					PlayClientSound( FlashlightEntity.Enabled ? "flashlight-on" : "flashlight-off" );
				}
			}
		}

		if ( !FlashlightEnabled && FlashlightBatteryCharge < 100 && Time.Tick % 7 == 1 )
		{
			FlashlightBatteryCharge++;
		}

		//Setting the position of the flashlight serverside
		//basically, the position for other players the client is seeing.
		FlashlightEntity.Position = EyePosition + EyeRotation.Forward * FlashlightPosOffset;
		FlashlightEntity.Rotation = EyeRotation;

		//FlashlightEntity.Flicker = FlashlightBatteryCharge <= 13f; //jesus christ what was i thinking
		FlashlightEntity.BrightnessMultiplier = FlashlightBatteryCharge <= 13f ? 0.1f : 0.9f; //this is weird
	}
}

