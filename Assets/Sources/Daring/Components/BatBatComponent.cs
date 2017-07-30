using System;
using System.Collections;
using System.Collections.Generic;
using Lunaria;
using UnityEngine;

public class BatBatComponent : MonoBehaviour
{
    private IMessageService _messageService;

    private void Awake()
    {
        _messageService = ServiceHolder.Instance.Get<IMessageService>();
        _messageService.AddHandler<HeroCollisionMessage>(OnHeroCollisionMessage);
    }

    private void OnHeroCollisionMessage(HeroCollisionMessage obj)
    {
        if (obj.Collision.gameObject == gameObject)
        {
            _messageService.SendMessage(new ScareHeroMessage(gameObject));
        }
    }
}

public class ScareHeroMessage
{
    public GameObject GameObject;

    public ScareHeroMessage(GameObject gameObject)
    {
        GameObject = gameObject;
    }
}