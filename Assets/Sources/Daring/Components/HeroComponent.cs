using System;
using System.Collections;
using System.Collections.Generic;
using Lunaria;
using UnityEngine;

public class HeroComponent : BaseGameEntity
{
    public enum HeroState
    {
        Normal,
        Scared
    }

    public GameObject WaitBeforeStart;
    public float WaitTimer;
    private bool _hasInitialized;

    public float Speed = 1.5f;
    public float StopAtDistanceToDestination = 0.1f;
    public float RotationSpeed = 20;
    public float ScariedSpeed = 1f;
    public float ScariedTime = 2f;
    public float AuraDistanceForNoticeability = 1.4f;
    public GameObject ScaredText;

    private Vector3 _nextDestination;
    private Vector3 _destination;
    private Vector3 _direction;
    private Quaternion _look;

    public Animator TheOneAnimator;

    private HeroState _state;
    private float _currentSpeed;
    
    private float _scariedTimer;

    private IMessageService _messageService;
    private FlashLight _flashLight;
    
    private bool _stop;

    private void Awake()
    {
        EntityType = EntityType.Hero;
        _flashLight = ServiceHolder.Instance.Get<HeroService>().Hero.FlashLight;
        _messageService = ServiceHolder.Instance.Get<IMessageService>();
        _messageService.AddHandler<EndGameMessage>(obj => _stop = true);
        _currentSpeed = Speed;
        _state = HeroState.Normal;
    }

    private void Update()
    {
        if (!_hasInitialized)
        {
            WaitTimer -= Time.deltaTime;
            if (WaitTimer > 0)
            {
                return;
            }

            _hasInitialized = true;
            WaitBeforeStart.SetActive(false);
        }

        if (_stop)
        {
            return;
        }

        GetInput();
        MoveHero();
        DetectAuraCollision();
    }

    private void GetInput()
    {
        if (_state == HeroState.Normal && Input.GetMouseButton(0))
        {
            Vector3 mousePosition = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10);
            Vector3 worldPosition = Camera.main.ScreenToWorldPoint(mousePosition);
            UpdateDestination(worldPosition);
        }
        else if (_state == HeroState.Scared && Input.GetMouseButton(0))
        {
            Vector3 mousePosition = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10);
            Vector3 worldPosition = Camera.main.ScreenToWorldPoint(mousePosition);
            _nextDestination = worldPosition;
        }
    }

    private void UpdateDestination(Vector3 position)
    {
        _destination = position;
        _direction = (_destination - transform.position).normalized;
        float lookAngle = Vector2.Angle(Vector2.down, new Vector2(_direction.x, _direction.y));
        _look = Quaternion.identity;
        if (_direction.x >= 0)
        {
            _look = Quaternion.AngleAxis(lookAngle, Vector3.forward);
        }
        else
        {
            _look = Quaternion.AngleAxis(lookAngle, Vector3.back);
        }
    }

    private void MoveHero()
    {
        if (_destination != Vector3.zero &&
            Vector3.Distance(transform.position, _destination) > StopAtDistanceToDestination)
        {
            transform.position += _direction * _currentSpeed * Time.deltaTime;
            TheOneAnimator.SetBool("walking", true);
        }
        else
        {
            TheOneAnimator.SetBool("walking", false);
        }

        if (_direction != Vector3.zero)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, _look, Time.deltaTime * RotationSpeed);
        }
        
        if (_state == HeroState.Scared)
        {
            _scariedTimer += Time.deltaTime;
            if (_scariedTimer > ScariedTime)
            {
                _scariedTimer = 0;
                SetState(HeroState.Normal, Vector3.zero);
            }
        }
    }

    private void DetectAuraCollision()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, AuraDistanceForNoticeability);
        for (int i = 0; i < hits.Length; i++)
        {
            Collider2D hit = hits[i];
            GameObject collided = hit.gameObject;
            if (collided.layer == 11) // if is creature
            {
                BaseGameEntity bge = collided.GetComponent<BaseGameEntity>();
                switch(bge.EntityType)
                {
                    case EntityType.BatBat:
                        (bge as BatBatComponent).Laugh();
                        SetState(HeroState.Scared, collided.transform.position);
                        break;
                    case EntityType.Insecto:
                        _flashLight.DrainSomeBattery();
                        (bge as InsectoComponent).BlowUp();
                        break;
                }
            }
        }
    }

    private void SetState(HeroState state, Vector3 somePosition)
    {
        _state = state;
        switch(_state)
        {
            case HeroState.Normal:
                _currentSpeed = Speed;
                UpdateDestination(_nextDestination);
                ScaredText.SetActive(false);
                break;
            case HeroState.Scared:
                _currentSpeed = ScariedSpeed;
                Vector3 diference = transform.position - somePosition;
                UpdateDestination(transform.position + diference * 5);
                ScaredText.SetActive(true);
                break;
        }
    }

    public override void BeCaughtByTentacle()
    {
        _stop = true;
    }

    // private void OnDrawGizmos()
    // {
    //     Gizmos.color = new Color(0, 0.8f, 1f, 0.5f);
    //     Gizmos.DrawSphere(transform.position, AuraDistanceForNoticeability);
    // }
}
