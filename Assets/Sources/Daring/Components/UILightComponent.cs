using System.Collections;
using System.Collections.Generic;
using Lunaria;
using UnityEngine;
using UnityEngine.UI;

public class UILightComponent : MonoBehaviour
{
    private Slider _slider;
    private HeroService _heroService;
    private FlashLight _flashLight;

    private float _currentValue;

	private void Awake()
    {
        _heroService = ServiceHolder.Instance.Get<HeroService>();
        _flashLight = _heroService.Hero.FlashLight;

		_slider = GetComponent<Slider>();
        _slider.minValue = 0;
        _slider.maxValue = _flashLight.BatteryMaxPower;
        _slider.value = _flashLight.CurrentBattery;
	}

    private void Update()
    {
        if (_flashLight.CurrentBattery != _currentValue)
        {
            _currentValue = _flashLight.CurrentBattery;
            _slider.value = _currentValue;
        }
    }
}
