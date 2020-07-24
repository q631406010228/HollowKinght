using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

public class FailChampionAttack : Action
{
    Animator _animator;
    int _dir;

    public override void OnAwake()
    {
    }


    public override void OnStart()
    {
        base.OnStart();
        _animator = GetComponent<Animator>();
        _animator.SetTrigger("IsAttack");
    }

}
