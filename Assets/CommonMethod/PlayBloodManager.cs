using Assets.CommonMethod;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Assets.Level2Script.Script
{
    class PlayBloodManager : Singleton<PlayBloodManager>
    {
        public GameObject _text;
        public PlayController playController;

        BloodManager _bloodManager;
        List<GameObject> _bloods = new List<GameObject>();
        List<Animator> _animators = new List<Animator>();
        bool _reduceBlooding = false;
        ShakeCamera _shakeCamera;
        Text _textSxript;
        TestImageEffectGrey _imageEffectGrey;

        void Awake()
        {
            _imageEffectGrey = GetComponent<TestImageEffectGrey>();
            _textSxript = _text.GetComponent<Text>();
            _bloodManager = new BloodManager();
            _shakeCamera = GetComponent<ShakeCamera>();
            _bloodManager._bloodVolume = 5;
            _bloodManager._maxBloodVolume = 5;
        }

        void CreatBlood()
        {
            for(int i = 0;i < _bloodManager._maxBloodVolume; i++)
            {
                if(i < _bloodManager._bloodVolume)
                {
                    GameObject blood = Instantiate(Resources.Load<GameObject>("Prefebs/BloodBroken"));
                    blood.transform.parent = transform;
                    blood.transform.localScale = new Vector3(2, 2, 0);
                    blood.transform.position = transform.position + new Vector3(-12f + i, 7f, 11);
                    _bloods.Add(blood);
                    Animator animator = blood.GetComponent<Animator>();
                    _animators.Add(animator);
                }
                else
                {
                    GameObject blood = Instantiate(Resources.Load<GameObject>("Prefebs/BloodBroken7"));
                    blood.transform.parent = transform;
                    blood.transform.position = transform.position + new Vector3(-12f + i, 7f, 10);
                    blood.transform.localScale = new Vector3(1.8f, 1.8f, 0);
                }
            }
        }

        void Start()
        {

        }

        void Update()
        {
            if (_reduceBlooding)
            {
                if(_animators[_bloodManager._bloodVolume].GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.95)
                {
                    ReduceBloodOver();
                    if (_bloodManager.IsDie())
                    {
                        _textSxript.enabled = true;
                        _imageEffectGrey.enabled = true;
                        StartCoroutine(ReStartGame());
                    }
                }
            }
        }

        /// <summary>
        /// 死亡动画
        /// </summary>
        /// <returns></returns>
        IEnumerator DieAnimator()
        {
            _textSxript.color = new Color32(0, 0, 0, 0);
            Color32 lookColor = new Color32(0, 0, 0, 255);
            Color32 hideColor = new Color32(0, 0, 0, 0);

            for (int i = 1; i <= 10; i++)
            {
                Color32 targetColor = Color32.Lerp(hideColor, lookColor, i * 0.1f);
                _textSxript.color = targetColor;
                yield return new WaitForSeconds(0.05f);
            }

        }

        IEnumerator ReStartGame()
        {
            _textSxript.color = new Color32(255, 0, 0, 0);
            Color32 lookColor = new Color32(255, 0, 0, 255);
            Color32 hideColor = new Color32(255, 0, 0, 0);

            for (int i = 1; i <= 10; i++)
            {
                Color32 targetColor = Color32.Lerp(hideColor, lookColor, i * 0.1f);
                _textSxript.color = targetColor;
                yield return new WaitForSeconds(0.05f);
            }
            yield return new WaitForSeconds(5);
            GameDataManager.instance.isRevive = true;

            AsyncOperation op = SceneManager.LoadSceneAsync("FailChampionScene");
            op.allowSceneActivation = true;
        }

        public void ReduceBlood()
        {
            _bloodManager.ReduceBlood();
            _reduceBlooding = true;
            _animators[_bloodManager._bloodVolume].SetTrigger("IsBloodBroken");
            _animators[_bloodManager._bloodVolume].enabled = true;
            _shakeCamera.enabled = true;
        }

        public void ReduceBloodOver()
        {
            GameObject blood = Instantiate(Resources.Load<GameObject>("Prefebs/BloodBroken7"));
            blood.transform.parent = transform;
            blood.transform.position = transform.position + new Vector3(-12f + _bloodManager._bloodVolume, 7f, 10);
            blood.transform.localScale = new Vector3(1.8f, 1.8f, 0);
            _bloods[_bloodManager._bloodVolume ].SetActive(false);
            _reduceBlooding = false;
            _animators[_bloodManager._bloodVolume].enabled = false;
        }

        public int GetBloodNum()
        {
            return _bloodManager._bloodVolume;
        }

        public void SetBlood(int bloodVolume)
        {
            _bloodManager._bloodVolume = bloodVolume;
            CreatBlood();
        }
    }
}
