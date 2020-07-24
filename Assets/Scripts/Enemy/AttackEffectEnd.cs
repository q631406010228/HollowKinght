using UnityEngine;

public class AttackEffectEnd : MonoBehaviour
{
    Animator _animator;
    

    void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (_animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.95)
        {
            this.gameObject.SetActive(false);
        }
    }
}
