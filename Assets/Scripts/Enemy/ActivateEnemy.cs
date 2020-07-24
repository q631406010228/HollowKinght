using Assets.Scripts.Enemy;
using System.Collections.Generic;
using UnityEngine;

public class ActivateEnemy : MonoBehaviour
{

    GameObject flyBug;
    Dictionary<string, GameObject> losePowerEnemyPoint = new Dictionary<string, GameObject>(); 

    // Start is called before the first frame update
    void Start()
    {
        flyBug = Resources.Load<GameObject>("Prefebs/FlyBug");
    }

    // Update is called once per frame
    void Update()
    {

    }

    //敌人进入触发区域
    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag != "PreEnemy")
            return;
        GameObject gameObject = GameObjectPool.Instance().GetPool(flyBug, collision.transform.position);
        losePowerEnemyPoint.Add(collision.gameObject.name, collision.gameObject);
        collision.gameObject.SetActive(false);
    }

    public void LosePoweEnemy(GameObject gameObject, string objectTag)
    {
        GameObject enemyPoint;
        losePowerEnemyPoint.TryGetValue(gameObject.name, out enemyPoint);
        if (enemyPoint != null)
        {
            enemyPoint.SetActive(true);
            losePowerEnemyPoint.Remove(gameObject.name);
            GameObjectPool.Instance().IntoPool(gameObject, objectTag);
        }
    }
}
