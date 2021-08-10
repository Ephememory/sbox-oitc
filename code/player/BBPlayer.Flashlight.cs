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
	public static float sas_flashlight_drain_amt { get; set; } = 1;
}

