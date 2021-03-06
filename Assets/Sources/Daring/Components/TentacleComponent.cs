﻿using System;
using System.Collections;
using System.Collections.Generic;
using Lunaria;
using UnityEngine;

public class TentacleComponent : BaseGameEntity
{
    private IMessageService _messageService;
    public GameObject Exclamation;
    public GameObject Slurp;

    public float Range = 0.5f;
    public float DrawSpeed = 2f;

    public AudioSource SlurpSound;

    private bool _killingSomeone;
    private GameObject _theKilled;
    private Vector3 _direction;
    private GameObject _heroReference;
    private Animator _animator;

    private void Awake()
    {
        EntityType = EntityType.Tentacle;
        _messageService = ServiceHolder.Instance.Get<IMessageService>();

        _heroReference = GameObject.FindGameObjectWithTag("Player");
        _animator = GetComponentInChildren<Animator>();
    }

    private void Update()
    {
        if (_killingSomeone)
        {
            _theKilled.gameObject.GetComponent<Collider2D>().enabled = false;
            _theKilled.transform.position -= _direction * Time.deltaTime * DrawSpeed * 0.5f;
            _theKilled.transform.position -= Vector3.back * Time.deltaTime * DrawSpeed;

            if (_theKilled.transform.position.z > 3)
            {
                if (_theKilled == _heroReference)
                {
                    _messageService.SendMessage(new EndGameMessage(false));
                }
                gameObject.SetActive(false);
            }
        }
        else
        {
            Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, Range);
            for (int i = 0; i < hits.Length; i++)
            {
                Collider2D hit = hits[i];
                GameObject collided = hit.gameObject;
                if (collided.layer == 8 ||
                    collided.layer == 11)
                {
                    collided.GetComponent<BaseGameEntity>().BeCaughtByTentacle();
                    Exclamation.SetActive(true);
                    Slurp.SetActive(true);
                    _killingSomeone = true;
                    _theKilled = collided;
                    _direction = (_theKilled.transform.position - transform.position).normalized;
                    _animator.SetTrigger("lick");
                    SlurpSound.Play();
                }
            }
        }
    }

    public override void BeCaughtByTentacle() { }

    private void OnDrawGizmos()
    {
        Gizmos.color = new Color(0, 1, 0, 0.5f);
        Gizmos.DrawSphere(transform.position, Range);
    }
}
