using Assets.CommonMethod;
using Assets.Scripts.Enemy;
using Pathfinding;
using System;
using UnityEngine;

public class FlyAttack : MonoBehaviour, EnemyAction
{
    GameObject Player;
    GameObject emptyGameObject;
    ActivateEnemy activateEnemy;
    bool IsFollow = false;
    bool IsPatrol = true;
    bool IsBack = false;
    bool IsDead = false;
    bool IsInjured = false;
    float PatrolScope = 5;
    float InjuredTime = 0;
    float InjuredDistance = 3;
    Vector3 InjuredMovePosition;
    Vector3 OriginalPosition;
    Vector3 StartPosition;
    Vector3 MoveVelocity = new Vector3(3f, 0, 0);
    Vector3 AttackVelocity = new Vector3(6f, 0, 0);
    Dir Dir = Dir.Left;    
    SpriteRenderer SpriteRender;
    Animator FlyAttackAnimator;
    Collider2D Collider2D;
    BloodManager FlyBlood;
    Material FlyMaterial;
    AIDestinationSetter destinationSetter;


    // Start is called before the first frame update
    void Start()
    {
        emptyGameObject = new GameObject();
        StartPosition = transform.position;
        Player = GameObject.Find("Player");
        activateEnemy = GameObject.Find("ActivateEnemy").GetComponent<ActivateEnemy>();
        SpriteRender = GetComponent<SpriteRenderer>();
        FlyAttackAnimator = GetComponent<Animator>();
        Collider2D = GetComponent<Collider2D>();
        destinationSetter = GetComponent<AIDestinationSetter>();
        emptyGameObject.transform.position = new Vector3(StartPosition.x - PatrolScope, StartPosition.y, 0);
        destinationSetter.target = emptyGameObject.transform;
        FlyBlood = new BloodManager();
        FlyBlood._bloodVolume = 3;
        FlyBlood._maxBloodVolume = 3;
        FlyMaterial = SpriteRender.material;
    }

    void OnEnable()
    {
        destinationSetter.target = emptyGameObject.transform;
    }

    // Update is called once per frame
    void Update()
    {
        if (IsDead)
            return;
        if (IsInjured)
        {
            Injured();
            return;
        }
        if(!destinationSetter.enabled)
            destinationSetter.enabled = true;
        Vector3 targetPos = new Vector3(Player.transform.position.x, Player.transform.position.y, transform.position.z);
        float playDistance = Vector3.Distance(transform.position, targetPos);
        if (playDistance > 30)
            activateEnemy.LosePoweEnemy(this.gameObject, "FlyBug");
        if (IsFollow)   
        {
            float originalDistance = Vector3.Distance(transform.position, OriginalPosition);
            if (originalDistance > 15)  //超出追击距离
            {
                IsBack = true;
                IsFollow = false;
            }
            else
            {
                JudgeDir(Player.transform.position);
                Follow();
            }
        }
        if (IsBack)
        {
            float originalDistance = Vector3.Distance(transform.position, OriginalPosition);
            if (originalDistance > 15)  //超出出发时的追击距离
            {
                IsBack = true;
                IsFollow = false;
                JudgeDir(OriginalPosition);
            }
            if (playDistance > 15)     //离主角的距离超过，不追了
            {
                IsBack = true;
                IsFollow = false;
                JudgeDir(OriginalPosition);
            }
            Back();
        }
        if (IsPatrol)
        {
            Patrol();
            if (playDistance < 10)  //达到距离后开始追击
            {
                OriginalPosition = transform.position;
                IsFollow = true;
                IsPatrol = false;
            }
        }
    }

    void Injured()
    {
        InjuredTime += Time.deltaTime;
        if (InjuredTime > 0.5f)
        {
            InjuredTime = 0;
            IsInjured = false;
        }
        else if(InjuredTime > 0.1f)
        {
            SpriteRender.material = FlyMaterial;
        }
        destinationSetter.enabled = false;
        transform.position = Vector3.Lerp(transform.position, InjuredMovePosition, 1 / (transform.position - InjuredMovePosition).magnitude / 9.0f);
    }

    void JudgeDir(Vector3 targetPosition)
    {
        float x = transform.position.x - targetPosition.x;
        if(x < 0 && Dir == Dir.Left)
        {
            Reverse();
            Dir = Dir.Right;
        }
        else if(x > 0 && Dir == Dir.Right)
        {
            Reverse();
            Dir = Dir.Left;
        }
    }

    private void Reverse()
    {
        var scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }

    void Follow()
    {
        Vector3 targetPos = new Vector3(Player.transform.position.x, Player.transform.position.y, 0);
        emptyGameObject.transform.position = targetPos;
        float distance = Vector3.Distance(transform.position, targetPos);
    }

    void Back()
    {
        emptyGameObject.transform.position = OriginalPosition;
        float distance = Vector3.Distance(transform.position, OriginalPosition);
        if (distance < 0.5f)
        {
            IsBack = false;
            IsPatrol = true;
        }
    }

    public void Attack()
    {
        Debug.Log("Attack");
    }

    public void Patrol()
    {
        if(transform.position.x - StartPosition.x + PatrolScope <= 0)
        {
            JudgeDir(StartPosition);
        }
        else if(transform.position.x - StartPosition.x - PatrolScope > 0)
        {
            JudgeDir(StartPosition);
        }
        if(Dir == Dir.Right)
        {
            emptyGameObject.transform.position = new Vector3(StartPosition.x + PatrolScope, StartPosition.y, 0);
        }
        else
        {
            emptyGameObject.transform.position = new Vector3(StartPosition.x - PatrolScope, StartPosition.y, 0);
        }
    }

    public void SetActivate()
    {
        this.enabled = true;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (IsDead)
            return;
        if (collision.tag != "PlayAttack")
            return;
        FlyBlood.ReduceBlood();
        ComputeAngel(collision.gameObject.transform.parent.position);
        if (FlyBlood.IsDie())
        {
            FlyAttackAnimator.SetTrigger("IsDead");
            IsDead = true;
            Collider2D.enabled = false;
            destinationSetter.enabled = false;
        }
        else
        {
            SpriteRender.material = Resources.Load<Material>("Materials/White");
            IsInjured = true;
        }
    }

    void ComputeAngel(Vector3 playPosition)
    {
        double a = transform.position.x - playPosition.x;
        double b = transform.position.y - playPosition.y;
        double c = Math.Sqrt(a * a + b * b);
        float sin = (float)(a / c);
        float cos = (float)(b / c);
        InjuredMovePosition = new Vector3(10 * sin, 10 * cos, 0) + transform.position;
    }

    void EnemyDead()
    {
        Destroy(this.gameObject);
    }
}
