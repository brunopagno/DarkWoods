using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroComponent : MonoBehaviour {

    public float Speed = 1.5f;
    public float StopAtDistanceToDestination = 0.1f;
    public float RotationSpeed = 20;

    private Vector3 _destination;
    private Vector3 _direction;
    private Quaternion _look;

    private void Update()
    {
        GetInput();
        MoveHero();
    }

    private void GetInput()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mousePosition = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10);
            Vector3 worldPosition = Camera.main.ScreenToWorldPoint(mousePosition);
            _destination = worldPosition;
            _direction = (_destination - transform.position).normalized;
            _look = Quaternion.LookRotation(_direction, Vector3.back);
            // float lookAngle = Vector2.Angle(Vector2.down, new Vector2(_direction.x, _direction.y));
            // _look = Quaternion.identity;
            // if (_direction.x >= 0)
            // {
            //     _look = Quaternion.AngleAxis(lookAngle, Vector3.forward);
            // }
            // else
            // {
            //     _look = Quaternion.AngleAxis(lookAngle, Vector3.back);
            // }
        }
    }

    private void MoveHero()
    {
        if (_destination != Vector3.zero &&
            Vector3.Distance(transform.position, _destination) > StopAtDistanceToDestination)
        {
            transform.position += _direction * Speed * Time.deltaTime;
        }

        if (_direction != Vector3.zero)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, _look, Time.deltaTime * RotationSpeed);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("COLLIDED");
    }
}
