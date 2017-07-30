using System;
using System.Collections;
using System.Collections.Generic;
using Lunaria;
using UnityEngine;

public class InsectoComponent : BaseGameEntity
{
    private IMessageService _messageService;
    private GameObject _heroReference;
    private LightComponent _flashLightReference;

    public GameObject Exclamation;
    
    private bool _stop;

    private void Awake()
    {
        _messageService = ServiceHolder.Instance.Get<IMessageService>();
        _messageService.AddHandler<HeroCollisionMessage>(OnHeroCollisionMessage);
        _messageService.AddHandler<EndGameMessage>(obj => _stop = true);

        _heroReference = GameObject.FindGameObjectWithTag("Player");
        _flashLightReference = GameObject.FindGameObjectWithTag("FlashLight").GetComponent<LightComponent>();
    }

    private void Update()
    {
        if (_stop)
        {
            return;
        }

        // Exclamation.SetActive(true);
    }

    private void OnHeroCollisionMessage(HeroCollisionMessage obj)
    {
        // meh
    }

    public override void BeCaughtByTentacle()
    {
        _stop = true;
    }
}
