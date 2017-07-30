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
        Scaried
    }

    public float Speed = 1.5f;
    public float StopAtDistanceToDestination = 0.1f;
    public float RotationSpeed = 20;
    public float ScariedSpeed = 1f;
    public float ScariedTime = 2f;

    private Vector3 _nextDestination;
    private Vector3 _destination;
    private Vector3 _direction;
    private Quaternion _look;

    private HeroState _state;
    private float _currentSpeed;
    
    private float _scariedTimer;

    private IMessageService _messageService;
    
    private bool _stop;

    private void Awake()
    {
        _messageService = ServiceHolder.Instance.Get<IMessageService>();
        _messageService.AddHandler<ScareHeroMessage>(OnScareHeromessage);
        _messageService.AddHandler<EndGameMessage>(obj => _stop = true);
        _currentSpeed = Speed;
        _state = HeroState.Normal;
    }

    private void Update()
    {
        if (_stop)
        {
            return;
        }

        GetInput();
        MoveHero();
    }

    private void GetInput()
    {
        if (_state == HeroState.Normal && Input.GetMouseButtonDown(0))
        {
            Vector3 mousePosition = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10);
            Vector3 worldPosition = Camera.main.ScreenToWorldPoint(mousePosition);
            UpdateDestination(worldPosition);
        }
        else if (_state == HeroState.Scaried && Input.GetMouseButtonDown(0))
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
        }

        if (_direction != Vector3.zero)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, _look, Time.deltaTime * RotationSpeed);
        }
        
        if (_state == HeroState.Scaried)
        {
            _scariedTimer += Time.deltaTime;
            if (_scariedTimer > ScariedTime)
            {
                _scariedTimer = 0;
                SetState(HeroState.Normal, Vector3.zero);
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        _messageService.SendMessage(new HeroCollisionMessage(collision));
    }

    private void OnScareHeromessage(ScareHeroMessage obj)
    {
        SetState(HeroState.Scaried, obj.GameObject.transform.position);
    }

    private void SetState(HeroState state, Vector3 somePosition)
    {
        _state = state;
        switch(_state)
        {
            case HeroState.Normal:
                _currentSpeed = Speed;
                UpdateDestination(_nextDestination);
                break;
            case HeroState.Scaried:
                _currentSpeed = ScariedSpeed;
                Vector3 diference = transform.position - somePosition;
                UpdateDestination(transform.position + diference * 5);
                break;
        }
    }

    public override void BeCaughtByTentacle()
    {
        _stop = true;
    }
}

public class HeroCollisionMessage
{
    public Collision2D Collision;

    public HeroCollisionMessage(Collision2D collision)
    {
        Collision = collision;
    }
}