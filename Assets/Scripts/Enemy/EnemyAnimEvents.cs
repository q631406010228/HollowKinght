using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnimEvents : MonoBehaviour
{

    void Awake()
    {
    }

    void Start()
    {
    }

    void ChangeColliderIsShow(string msg)
    {
        GameObjectPool.Instance().IntoPool(this.gameObject, "FlyBug");
    }
}
