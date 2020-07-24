using Assets.CommonMethod;
using Assets.Level2Script.Script;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyAttacked : MonoBehaviour
{
    public Text text;
    float _timeInjure;
    SpriteRenderer _spriteRender;
    bool _isInjure = false;
    Material _material;
    BloodManager bloodManager;

    // Start is called before the first frame update
    void Start()
    {
        _spriteRender = GetComponent<SpriteRenderer>();
        _material = _spriteRender.material;
        bloodManager = new BloodManager();
        bloodManager._bloodVolume = 10;
        bloodManager._maxBloodVolume = 10;
    }

    // Update is called once per frame
    void Update()
    {
        if (_isInjure)
        {
            Injure();
        }
    }

    void Injure()
    {
        _timeInjure += Time.deltaTime;
        if (_timeInjure > 0.2f)
        {
            if (bloodManager.IsDie())
            {
                text.enabled = true;
                StartCoroutine(GameOver());
            }
            _spriteRender.material = _material;
            _timeInjure = 0;
            _isInjure = false;
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (_isInjure)
            return;
        if (collision.tag != "PlayAttack")
            return;
        bloodManager.ReduceBlood();
        _isInjure = true;
        _spriteRender.material = Resources.Load<Material>("Materials/White");
        AudioManger.Instance.PlayAudio("sword_hit_reject", transform.position);
    }

    IEnumerator GameOver()
    {
        gameObject.SetActive(false);
        text.color = new Color32(255, 0, 0, 0);
        Color32 lookColor = new Color32(255, 0, 0, 255);
        Color32 hideColor = new Color32(255, 0, 0, 0);

        for (int i = 1; i <= 10; i++)
        {
            Color32 targetColor = Color32.Lerp(hideColor, lookColor, i * 0.1f);
            text.color = targetColor;
            yield return new WaitForSeconds(0.02f);
        }
        yield return new WaitForSeconds(5);
        Time.timeScale = 0;
    }
}
