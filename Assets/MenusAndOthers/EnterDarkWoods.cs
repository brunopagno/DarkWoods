using System.Collections;
using System.Collections.Generic;
using Lunaria;
using UnityEngine;

public class EnterDarkWoods : MonoBehaviour
{
    public void Go()
    {
        ServiceHolder.Instance.Get<SceneMutreta>().StartGame();
    }
}
