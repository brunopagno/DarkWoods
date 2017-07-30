using System.Collections;
using System.Collections.Generic;
using Lunaria;
using UnityEngine;

public class EndStageComponent : MonoBehaviour
{
    public Vector2 Area;
    private GameObject _hero;

    private void Awake()
    {
        _hero = GameObject.FindGameObjectWithTag("Player");
    }

    private void Update()
    {
        Collider2D[] hits = Physics2D.OverlapBoxAll(transform.position, Area, 0);
        for (int i = 0; i < hits.Length; i++)
        {
            Collider2D hit = hits[i];
            if (hit.gameObject == _hero)
            {
                ServiceHolder.Instance.Get<IMessageService>().SendMessage(new EndGameMessage(true));
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = new Color(1, 0, 1, 1);
        Gizmos.DrawWireCube(transform.position, Area);
    }
}
