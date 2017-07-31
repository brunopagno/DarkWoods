
using System;

public class FlashLight
{
    public float CurrentBattery;

    public float BatteryMaxPower = 5f;
    public float BatteryDrainTick = 0.5f;
    public float BatteryDrainTickSpeed = 0.5f;

    public float LightRegularIntensity = 20;
    public float LightMinIntensity = 3;

    public float RelativeLightIntensity(float relativeLoad)
    {
        return (LightRegularIntensity - LightMinIntensity) * relativeLoad + LightMinIntensity;
    }

    public void DrainSomeBattery()
    {
        CurrentBattery -= BatteryDrainTick * 10;
        if (CurrentBattery < 0)
        {
            CurrentBattery = 0;
        }
    }
}
