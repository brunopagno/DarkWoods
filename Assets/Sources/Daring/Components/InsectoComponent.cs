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
    private Animator _animator;

    public float Speed = 0.8f;
    public float RotationSpeed = 20f;
    public GameObject Exclamation;
    public Animator TheOtherAnimator;
    
    private bool _stop;

    public AudioSource Blow;

    private void Awake()
    {
        EntityType = EntityType.Insecto;
        _messageService = ServiceHolder.Instance.Get<IMessageService>();
        _messageService.AddHandler<EndGameMessage>(obj => _stop = true);

        _heroReference = GameObject.FindGameObjectWithTag("Player");
        _flashLightReference = GameObject.FindGameObjectWithTag("FlashLight").GetComponent<LightComponent>();
        _animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (_stop)
        {
            return;
        }

        if (CanSeeHero())
        {
            Vector3 otherDirection = _heroReference.transform.position - transform.position;
            if (_flashLightReference.ClearVisionToCreature(gameObject))
            {
                Exclamation.SetActive(true);
                transform.position += otherDirection.normalized * Speed * Time.deltaTime;

                float lookAngle = Vector2.Angle(Vector2.down, new Vector2(otherDirection.x, otherDirection.y));
                Quaternion look = Quaternion.identity;
                if (otherDirection.x >= 0)
                {
                    look = Quaternion.AngleAxis(lookAngle, Vector3.forward);
                }
                else
                {
                    look = Quaternion.AngleAxis(lookAngle, Vector3.back);
                }

                transform.rotation = Quaternion.Slerp(transform.rotation, look, Time.deltaTime * RotationSpeed);
                TheOtherAnimator.SetBool("walking", true);
            }
            else
            {
                Exclamation.SetActive(false);
                TheOtherAnimator.SetBool("walking", false);
            }
        }
    }
    
    private bool CanSeeHero()
    {
        RaycastHit2D hit = Physics2D.Linecast(transform.position, _heroReference.transform.position);
        return hit.collider != null && hit.collider.gameObject == _heroReference;
    }

    public override void BeCaughtByTentacle()
    {
        _stop = true;
    }

    public void BlowUp()
    {
        _stop = true;
        _animator.SetTrigger("blow");
        Blow.Play();
        GetComponent<Collider2D>().enabled = false;
    }
}
