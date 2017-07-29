using System.Collections;
using System.Collections.Generic;
using Lunaria;
using UnityEngine;

public class LightComponent : MonoBehaviour
{
    private Light _light;
    
    private float _internalTimer;
    private HeroService _heroService;
    private FlashLight _flashLight;

    public void Awake()
    {
        _heroService = ServiceHolder.Instance.Get<HeroService>();
        _flashLight = _heroService.Hero.FlashLight;
        _flashLight.CurrentBattery = _flashLight.BatteryMaxPower;
        _light = GetComponent<Light>();
    }

    private void Update()
    {
        _internalTimer += Time.deltaTime;
        if (_internalTimer > _flashLight.BatteryDrainTickSpeed)
        {
            _internalTimer -= _flashLight.BatteryDrainTickSpeed;
            _flashLight.CurrentBattery -= _flashLight.BatteryDrainTick;

            if (_flashLight.CurrentBattery < 0)
            {
                _flashLight.CurrentBattery = 0;
                _light.intensity = 0;
            }
            else
            {
                float relativeLoad = _flashLight.CurrentBattery / _flashLight.BatteryMaxPower;
                float lightIntensity = _flashLight.RelativeLightIntensity(relativeLoad);
                _light.intensity = lightIntensity;
            }
        }
    }
}
