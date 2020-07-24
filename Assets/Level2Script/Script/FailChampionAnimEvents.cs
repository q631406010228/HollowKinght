using UnityEngine;

public class FailChampionAnimEvents : MonoBehaviour
{

    GameObject _weaponCollider;   //武器碰撞器
    GameObject _weaponCollider1;   //武器碰撞器
    GameObject _weaponCollider2;   //武器碰撞器
    GameObject _weaponCollider3;   //武器碰撞器
    GameObject _weaponCollider4;   //武器碰撞器

    GameObject _failChampionAttack;
    GameObject waveAttack;

    void Awake()
    {
        _weaponCollider = transform.Find("WeaponCollider").gameObject;
        _weaponCollider1 = transform.Find("WeaponCollider1").gameObject;
        _weaponCollider2 = transform.Find("WeaponCollider2").gameObject;
        _weaponCollider3 = transform.Find("WeaponCollider3").gameObject;
        _weaponCollider4 = transform.Find("WeaponCollider4").gameObject;
        _weaponCollider1.SetActive(false);
        _weaponCollider2.SetActive(false);
        _weaponCollider3.SetActive(false);
        _weaponCollider4.SetActive(false);
        _failChampionAttack = transform.Find("AttackEffect").gameObject;
        waveAttack = transform.Find("Wave").gameObject;
    }

    void Start()
    {
    }

    void ChangeColliderIsShow(string msg)
    {
        switch (msg)
        {
            case "Attack":
                {
                    _weaponCollider4.SetActive(false);
                    _weaponCollider.SetActive(true);
                    break;
                }
            case "Attack1":
                {
                    _weaponCollider.SetActive(false);
                    _weaponCollider1.SetActive(true);
                    break;
                }
            case "Attack2":
                {
                    _weaponCollider1.SetActive(false);
                    _weaponCollider2.SetActive(true);
                    break;
                }
            case "Attack3":
                {
                    _weaponCollider2.SetActive(false);
                    _weaponCollider3.SetActive(true);
                    break;
                }
            case "Attack4":
                {
                    _weaponCollider3.SetActive(false);
                    _weaponCollider4.SetActive(true);
                    _failChampionAttack.SetActive(true);
                    waveAttack.SetActive(true);
                    break;
                }
            default:
                break;
        }
    }
}
