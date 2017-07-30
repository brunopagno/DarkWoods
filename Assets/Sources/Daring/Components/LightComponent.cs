using System;
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
        DrainBattery();
    }

    private void DrainBattery()
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

    public bool ClearVisionToCreature(GameObject creature)
    {
        if (Vector2.Angle(transform.forward, creature.transform.position - transform.position) < _light.spotAngle / 2)
        {
            RaycastHit2D hit = Physics2D.Linecast(transform.position, creature.transform.position);
            if (hit.collider.gameObject == creature)
            {
                return true;
            }
        }
        return false;
    }

    private void OnDrawGizmos()
    {
        if (_light != null)
        {
            Gizmos.color = new Color(0.1f, 0.1f, 0.1f, 0.5f);
            Gizmos.DrawSphere(transform.position, _light.range);
        }
    }
}
