using System;
using System.Collections;
using System.Collections.Generic;
using Lunaria;
using UnityEngine;

public class BatBatComponent : BaseGameEntity
{
    private IMessageService _messageService;

    public float Speed = 1.35f;
    public Vector2[] Path;
    public GameObject Exclamation;
    public GameObject Haha;
    public float WaitLaughing = 2;

    private int _nextPosition;
    private bool _moveOn = true;
    private Vector3 _direction;
    private readonly float _stopAtDistanceToDestination = 0.1f;

    private GameObject _heroReference;
    private LightComponent _flashLightReference;
    
    private bool _stop;
    private float _waitTimer;
    private bool _waitLaughing;

    private void Awake()
    {
        EntityType = EntityType.BatBat;
        _messageService = ServiceHolder.Instance.Get<IMessageService>();
        _messageService.AddHandler<EndGameMessage>(obj => _stop = true);

        _heroReference = GameObject.FindGameObjectWithTag("Player");
        _flashLightReference = GameObject.FindGameObjectWithTag("FlashLight").GetComponent<LightComponent>();

        for (int i = 0; i < Path.Length; i++)
        {
            Path[i] += new Vector2(transform.position.x, transform.position.y);
        }
    }

    private void Update()
    {
        if (_stop)
        {
            return;
        }

        if (_waitLaughing)
        {
            Haha.SetActive(true);
            _waitTimer += Time.deltaTime;
            if (_waitTimer > WaitLaughing)
            {
                _waitLaughing = false;
                _waitTimer = 0;
            }
            return;
        }
        Haha.SetActive(false);

        if (CanSeeHero())
        {
            Exclamation.SetActive(true);

            Vector3 otherDirection = _heroReference.transform.position - transform.position;
            if (_flashLightReference.ClearVisionToCreature(gameObject))
            {
                transform.position -= otherDirection.normalized * Speed * Time.deltaTime;
            }
            else
            {
                transform.position += otherDirection.normalized * Speed * Time.deltaTime;
            }
            _moveOn = true;
        }
        else 
        {
            Exclamation.SetActive(false);
            if (_moveOn)
            {
                _nextPosition += 1;
                if (_nextPosition >= Path.Length)
                {
                    _nextPosition = 0;
                }
                _direction = (Path[_nextPosition] - 
                              new Vector2(transform.position.x, transform.position.y)).normalized;
                
                _moveOn = false;
            }
            transform.position += _direction * Speed * Time.deltaTime;
            _moveOn = Vector3.Distance(transform.position, Path[_nextPosition]) < _stopAtDistanceToDestination;
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

    public void Laugh()
    {
        _waitLaughing = true;
    }

    private void OnDrawGizmos()
    {
        if (Path != null && Path.Length > 0)
        {
            Gizmos.color = Color.magenta;
            for (int i = 0; i < Path.Length; i++)
            {
                Vector2 p = Path[i];
                Gizmos.DrawLine(new Vector3(p.x, p.y) + Vector3.left * 0.5f + transform.position,
                                new Vector3(p.x, p.y) + Vector3.right * 0.5f + transform.position);
                Gizmos.DrawLine(new Vector3(p.x, p.y) + Vector3.up * 0.5f + transform.position,
                                new Vector3(p.x, p.y) + Vector3.down * 0.5f + transform.position);
            }
        }
    }
}
