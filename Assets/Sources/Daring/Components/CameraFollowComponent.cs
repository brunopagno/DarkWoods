using System;
using System.Collections;
using System.Collections.Generic;
using Lunaria;
using UnityEngine;

public class CameraFollowComponent : MonoBehaviour
{
    public GameObject Reference;
    public GameObject Blackout;

    private bool _blackout;

    private void Awake()
    {
        ServiceHolder.Instance.Get<IMessageService>().AddHandler<OuttaBatteryMessage>(OnOuttaBattery);
    }

    private void OnOuttaBattery(OuttaBatteryMessage obj)
    {
        _blackout = true;
        Blackout.SetActive(true);
    }

    private void Update()
    {
        if (_blackout)
        {
            Color current = Blackout.GetComponent<SpriteRenderer>().color;
            float alpha = current.a + 0.05f;
            if (alpha > 1)
            {
                alpha = 1;
            }
            Blackout.GetComponent<SpriteRenderer>().color = new Color(current.r,
                                                                      current.g,
                                                                      current.b,
                                                                      alpha);
        }
        else
        {
            transform.position = new Vector3(Reference.transform.position.x,
                                            Reference.transform.position.y,
                                            transform.position.z);
        }
	}
}
