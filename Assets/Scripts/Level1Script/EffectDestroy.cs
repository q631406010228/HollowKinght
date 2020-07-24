using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectDestroy : MonoBehaviour {

    [Header("多少时间后进回收池")]
    public float destroyTime;
    ParticleSystem ps;
    float time;

    void Start ()
    {
        ps = GetComponent<ParticleSystem>();
        time = 0;
    }
	
	// Update is called once per frame
	void Update ()
    {

        time += Time.deltaTime;
        if (time > destroyTime)
        {
            time = 0;
            GameObjectPool.Instance().IntoPool(gameObject, gameObject.name);
            //Destroy(gameObject);
        }

    }
}
