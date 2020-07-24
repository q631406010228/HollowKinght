using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleSystemManager : MonoBehaviour
{

    public void OnParticleSystemStopped()
    {
        GameObjectPool.Instance().IntoPool(this.gameObject, this.gameObject.name);
    }
}
