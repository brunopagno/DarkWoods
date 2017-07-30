using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseGameEntity : MonoBehaviour
{
    public EntityType EntityType;
    public abstract void BeCaughtByTentacle();
}

public enum EntityType
{
    Hero,
    BatBat,
    Tentacle,
    Insecto
}
