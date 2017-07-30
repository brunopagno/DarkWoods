using System;
using System.Collections;
using System.Collections.Generic;
using Lunaria;
using UnityEngine;

public class BatBatComponent : MonoBehaviour
{
    private IMessageService _messageService;

    public float Speed = 1.35f;
    public Vector2[] Path;
    public GameObject Exclamation;

    private int _nextPosition;
    private bool _moveOn = true;
    private Vector3 _direction;
    private readonly float _stopAtDistanceToDestination = 0.1f;

    private GameObject _heroReference;

    private void Awake()
    {
        _messageService = ServiceHolder.Instance.Get<IMessageService>();
        _messageService.AddHandler<HeroCollisionMessage>(OnHeroCollisionMessage);

        _heroReference = GameObject.FindGameObjectWithTag("Player");
    }

    private void OnHeroCollisionMessage(HeroCollisionMessage obj)
    {
        if (obj.Collision.gameObject == gameObject)
        {
            _messageService.SendMessage(new ScareHeroMessage(gameObject));
        }
    }

    private void Update()
    {
        if (CanSeeHero())
        {
            Exclamation.SetActive(true);
            Vector3 otherDirection = _heroReference.transform.position - transform.position;
            transform.position += otherDirection.normalized * Speed * Time.deltaTime;
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
        return hit.collider.gameObject == _heroReference;
    }

    private void OnDrawGizmos()
    {
        if (Path != null && Path.Length > 0)
        {
            Gizmos.color = Color.magenta;
            for (int i = 0; i < Path.Length; i++)
            {
                Vector2 p = Path[i];
                Gizmos.DrawLine(p + Vector2.left * 0.5f, p + Vector2.right * 0.5f);
                Gizmos.DrawLine(p + Vector2.up * 0.5f, p + Vector2.down * 0.5f);
            }
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