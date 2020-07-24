using Assets.CommonMethod;
using System;
using System.Collections;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using static Assets.CommonMethod.CommonEnum;

namespace Assets.Level2Script.Script
{
    public class PlayController : MonoBehaviour
    {
        public float _moveSpeed;
        public float _jumpSpeed;
        public float _secondJumpSpeed;
        public float _gravity;       //受到的重力
        public float _jumpTime;      //跳跃的蓄力时间
        public float _recoilForce = 6;
        public float _injuredForce = 40;
        public float _sprintTime;
        public GameObject _shadowEffect;
        public GameObject _shadowRePlay;
        public Image RunePage;
        public Image mapPage;
        public Canvas SaveText;

        float _timeJump;             //跳跃当前的蓄力时间
        float _timeInjure;
        float _distance = 0.8f;
        float _disableInputTime;
        float _disableInputTimeCount;
        Animator _animator;
        SpriteRenderer _spriteRender;
        Rigidbody2D  PlayRigidbody;
        PlayerState _playerState;
        Vector2 _moveVelocity;
        Vector2 _boxSize;
        Dir _playDir;
        LayerMask _groundLayer;
        private int _jumpCount;
        bool _isGround = true;
        bool _gravityEnable = true;  //重力开关
        bool _isInjure = false;
        bool _isPlayShow = true;
        bool IsCanSave = false;
        GameObject _jumpTwoEffect;   //二段跳特效物体
        GameObject _knifeEffectOne;   //刀光特效物体
        GameObject _knifeEffectDown;   //刀光特效物体
        GameObject KnifeEffectUp;   //刀光特效物体
        GameObject injuredEffectObj;
        GameObject _effectObj1;
        GameObject _shadowHalo;
        GameObject _shadowPartic;
        GameObject _attactEffectObj;
        SaveTextManage SaveTextManager;
        RuneManager runeManager;
        MapManager mapManager;
        public Image Chair;
        float _preInjuredTime; // 受伤时的时间
        bool _isCanSprint = true;
        bool _inputEnable = true;
        bool _unbeatable = false;
        bool IsOpenPage = false;
        string Path;
        AttackDir _attackDir = AttackDir.None;
        float shiftdTime;

        void Start()
        {
            shiftdTime = 0;
            _boxSize = new Vector2(0.66f, 1.32f);    //设置盒子射线的大小
            _groundLayer = 1 << LayerMask.NameToLayer("Ground");
            _playDir = Dir.Right;
            _playerState = PlayerState.Stop;
            _animator = GetComponent<Animator>();
            PlayRigidbody = GetComponent<Rigidbody2D>();
            _spriteRender = GetComponent<SpriteRenderer>();
            _jumpTwoEffect = Resources.Load<GameObject>("Prefebs/plumeEffectOne");
            _attactEffectObj = Resources.Load<GameObject>("Prefebs/AttackEffectOne");
            _knifeEffectOne = transform.Find("LRAttackImage").gameObject;
            _knifeEffectDown = transform.Find("LRAttackDownImage").gameObject;
            KnifeEffectUp = transform.Find("LRAttackUpImage").gameObject;
            _shadowHalo = Resources.Load<GameObject>("Prefebs/shadowHalo");
            _shadowPartic = Resources.Load<GameObject>("Prefebs/ShadowPartic");
            injuredEffectObj = Resources.Load<GameObject>("Prefebs/PlayInjured");
            runeManager = RunePage.gameObject.GetComponent<RuneManager>();
            mapManager = mapPage.gameObject.GetComponent<MapManager>();
            SaveTextManager = SaveText.GetComponent<SaveTextManage>();
            _knifeEffectOne.SetActive(false);
            _knifeEffectDown.SetActive(false);
            Path = Application.dataPath + "/Data/SaveGameData.txt";
            ReadGameData();
            InitRunAudioObj();
        }

        void FixedUpdate()
        {
            if (IsOpenPage)
                return;
            CheckNextMove();

        }

        void Update()
        {
            if (IsOpenPage)
                return;
            if (IsCanSave)
                SaveGameData();
            if (Time.timeScale == 0)
                TimeCount();
            NoOperate();
            CheckGround();
            UpdateGravity();
            if (_isInjure)
                InjureColorChange();
            LRMove();
            Jump();
            SprintFunc();
            Attack();
        }

        #region 攻击

        private void Attack()
        {
            if (!_inputEnable)
                return;
            if (Input.GetKeyDown(KeyCode.J))
            {
                if (Input.GetKey(KeyCode.S))
                {
                    _animator.SetTrigger("IsAttackDown");
                    _knifeEffectDown.transform.position = transform.position + new Vector3(0, -1.5f, 0);
                    StartCoroutine(LookKnifeObj(_knifeEffectDown, 0.02f));
                    _attackDir = AttackDir.Down;
                }
                else if (Input.GetKey(KeyCode.W))
                {
                    _animator.SetTrigger("IsAttackUp");
                    KnifeEffectUp.transform.position = transform.position + new Vector3(0, 1.5f, 0);
                    StartCoroutine(LookKnifeObj(KnifeEffectUp, 0.02f));
                    _attackDir = AttackDir.Up;

                }
                else if (_playDir == Dir.Left)
                {
                    _attackDir = AttackDir.Left;
                    _animator.SetTrigger("IsAttackLR");
                    StartCoroutine(LookKnifeObj(_knifeEffectOne, 0.02f));
                }
                else if (_playDir == Dir.Right)
                {
                    _attackDir = AttackDir.Right;
                    _animator.SetTrigger("IsAttackLR");
                    StartCoroutine(LookKnifeObj(_knifeEffectOne, 0.02f));
                }
                AudioManger.Instance.PlayAudio("sword_1", transform.position);
            }
        }
        
        /// <summary>
        /// 攻击后主角的影响
        /// </summary>
        public void AttackDownReaction()
        {
            AudioManger.Instance.PlayAudio("sword_hit_reject", transform.position);
            var tempObj = Instantiate(_attactEffectObj, transform.position, Quaternion.identity);   //攻击交互特效
            if (_attackDir == AttackDir.Down)
            {
                _moveVelocity.Set(_moveVelocity.x, _injuredForce * 2);
                tempObj.transform.position += new Vector3(0, -1.5f, 0);
            }
            else if (_attackDir == AttackDir.Left)
            {
                _disableInputTime = 0.5f;
                _inputEnable = false;
                _moveVelocity.Set(_injuredForce / 4.0f, _moveVelocity.y);
                tempObj.transform.rotation = Quaternion.Euler(new Vector3(0, 0, -90));
                tempObj.transform.position += new Vector3(-1.5f, 0, 0);
            }
            else if (_attackDir == AttackDir.Right)
            {
                _disableInputTime = 0.5f;
                _inputEnable = false;
                _moveVelocity.Set(-_injuredForce / 4.0f, _moveVelocity.y);
                tempObj.transform.rotation = Quaternion.Euler(new Vector3(0, 0, 90));
                tempObj.transform.position += new Vector3(1.5f,0 , 0);
            }
            else if(_attackDir == AttackDir.Up)
            {
                _moveVelocity.Set(_moveVelocity.x, 0);
                tempObj.transform.rotation = Quaternion.Euler(new Vector3(0, 0, 180));
                tempObj.transform.position += new Vector3(0, 1.5f, 0);
            }
            _jumpCount = 1;
        }

        IEnumerator LookKnifeObj(GameObject knifeEffect, float time)
        {
            knifeEffect.SetActive(true);
            yield return new WaitForSeconds(time);
            knifeEffect.SetActive(false);
            _attackDir = AttackDir.None;
        }

        #endregion

        #region 左右移动

        private void LRMove()
        {
            if (!_inputEnable)
            {
                playSource.Stop();
                return;
            }
            var h = 0;
            if (Input.GetKey(KeyCode.A))
                h = -1;
            if (Input.GetKey(KeyCode.D))
                h = 1;
            PlayRunAudio(h);
            if (h == 0)
            {
                _animator.SetBool("IsRun", false);
                _playerState = PlayerState.Stop;
                SlowlyStop();
                return;
            }
            if (h > 0 && _playDir != Dir.Right)
            {
                _knifeEffectOne.transform.position = transform.position + new Vector3(1.5f, 0, 0);
                _playDir = Dir.Right;
                _playerState = PlayerState.Run;
                Reverse();
                _animator.SetBool("IsRun", false);
                if (_isGround)
                    _animator.SetTrigger("IsRotate");
            }
            else if (h < 0 && _playDir != Dir.Left)
            {
                _knifeEffectOne.transform.position = transform.position + new Vector3(-1.5f, 0, 0);
                _playDir = Dir.Left;
                Reverse();
                _playerState = PlayerState.Run;
                _animator.SetBool("IsRun", false);
                if (_isGround)
                    _animator.SetTrigger("IsRotate");
            }
            else if (h > 0 && _playDir == Dir.Right)
            {
                _knifeEffectOne.transform.position = transform.position + new Vector3(1.5f, 0, 0);
                _animator.SetBool("IsRun", true);
                _playerState = PlayerState.Run;
            }
            else if (h < 0 && _playDir == Dir.Left)
            {
                _knifeEffectOne.transform.position = transform.position + new Vector3(-1.5f, 0, 0);
                _animator.SetBool("IsRun", true);
                _playerState = PlayerState.Run;
            }
            _moveVelocity.x = h * _moveSpeed;
        }

        public void SlowlyStop()
        {
            float reduceSpeed;
            if (_isGround)
                reduceSpeed = 3f;
            else
                reduceSpeed = 1f;
            if (Math.Abs(_moveVelocity.x) < reduceSpeed)
                _moveVelocity.x = 0;
            else if (_moveVelocity.x > 0)
                _moveVelocity.x -= reduceSpeed;
            else if(_moveVelocity.x < 0)
                _moveVelocity.x += reduceSpeed;
        }

        /// <summary>
        /// 播放行走声音
        /// </summary>
        /// <param name="h"></param>
        public void PlayRunAudio(float h)
        {
            if (playSource == null)
                InitRunAudioObj();
            if (h != 0 && _isGround && !playSource.isPlaying)
                playSource.Play();
            else if (h == 0 || !_isGround)
                playSource.Stop();
        }

        #endregion

        #region 受伤

        private void InjureColorChange()
        {
            float pauseTime = 2f;
            _timeInjure += Time.deltaTime;
            if (_timeInjure < pauseTime)
            {
                if (_isPlayShow)
                {
                    _spriteRender.color = new Color(1, 1, 1, 1);
                    _isPlayShow = false;
                }
                else
                {
                    _spriteRender.color = new Color(0, 0, 0, 1f);
                    _isPlayShow = true;
                }
            }
            else
            {
                _unbeatable = false;
                _moveVelocity.x = 0;
                _timeInjure = 0;
                _isInjure = false;
                _spriteRender.color = new Color(1, 1, 1, 1);
                _isPlayShow = true;
            }
        }

        void Injured(Collider2D collision)
        {
            if (collision.transform.position.x - transform.position.x < 0)
            {
                _moveVelocity.Set(_injuredForce * 4, _injuredForce);
            }
            else
            {
                _moveVelocity.Set(-_injuredForce * 4, _injuredForce);
            }
            _isInjure = true;
            _unbeatable = true;
            var injuredEffect = GameObjectPool.Instance().GetPool(injuredEffectObj, transform.position);    //生成粒子特效物体
            _effectObj1 = Instantiate(Resources.Load<GameObject>("Prefebs/PlayInjured1"), transform.position, Quaternion.identity);    //生成粒子特效物体
            injuredEffect.transform.position += new Vector3(1f, 0.3f, 10);    //为了效果，调高生成位置
            //ParticleSystem ps = injuredEffect.GetComponent<ParticleSystem>();
            //ps.Play();
            _effectObj1.transform.position += new Vector3(1.5f, 0, 10);    //为了效果，调高生成位置
            _spriteRender.color = new Color(0, 0, 0, 1f);
            Time.timeScale = 0;
            _animator.SetTrigger("IsInjured");
            _preInjuredTime = Time.realtimeSinceStartup;
            PlayBloodManager.Instance.ReduceBlood();
        }

        void TimeCount()
        {
            if (Time.realtimeSinceStartup - _preInjuredTime > 0.2)
            {
                Time.timeScale = 1;
                Destroy(_effectObj1);
            }
        }

        #endregion

        #region 冲刺

        /// <summary>
        /// 冲刺函数
        /// </summary>
        public void SprintFunc()
        {
            if (!_inputEnable)
            {
                return;
            }
            if (Input.GetKeyDown(KeyCode.L) && _isCanSprint && RuneManager.IsCanMiss)
            {
                StartCoroutine(SprintMove(_sprintTime));
                if (_isGround)
                {
                    _animator.SetTrigger("IsSprintGround");//播放冲刺动画
                }
                else
                {
                    _animator.SetTrigger("IsSprintFly");//播放冲刺动画
                }
                StartCoroutine(PlayShadowEffect());
                AudioManger.Instance.PlayAudio("hero_shade_dash_2", transform.position);
            }
        }

        IEnumerator SprintMove(float time)
        {
            _isCanSprint = false;
            _inputEnable = false;
            _gravityEnable = false;
            _unbeatable = true;
            _moveVelocity.y = 0;
            _disableInputTime = time;
            if (_playDir == Dir.Left)
                _moveVelocity.x = 15 * -1;
            else
                _moveVelocity.x = 15;
            yield return new WaitForSeconds(time);
            _moveVelocity.x = 0;
            _unbeatable = false;
            _inputEnable = true;
            _gravityEnable = true;
            _isCanSprint = true;
            _shadowEffect.SetActive(false);
        }

        /// <summary>
        /// 播放暗影冲刺特效
        /// </summary>
        IEnumerator PlayShadowEffect()
        {
            _shadowEffect.SetActive(true);
            GameObject tempObjOne = GameObjectPool.Instance().GetPool(_shadowHalo, transform.position);
            tempObjOne.transform.rotation = Quaternion.identity;
            //GameObject tempObjOne = Instantiate(_shadowHalo, transform.position, Quaternion.identity);
            GameObject tempObjTwo = GameObjectPool.Instance().GetPool(_shadowPartic, transform.position);
            //GameObject tempObjTwo = Instantiate(_shadowPartic, transform);
            tempObjTwo.transform.position += new Vector3(0, 0.9f, 0);
            yield return new WaitForSeconds(0.2f);
            _shadowEffect.SetActive(false);
            _shadowRePlay.SetActive(true);
            yield return new WaitForSeconds(0.3f);
            AudioManger.Instance.PlayAudio("hero_shade_dash_charge_pt_2", transform.position);
            yield return new WaitForSeconds(0.3f);
            _shadowRePlay.SetActive(false);
            _isCanSprint = true;
            //Destroy(tempObjOne);
            //Destroy(tempObjTwo);
        }


        #endregion

        #region 声音

        AudioSource playSource;
        /// <summary>
        /// 初始化玩家脚步声音播放物体
        /// </summary>
        public void InitRunAudioObj()
        {
            playSource = AudioManger.Instance.PlayAudio("hero_run_footsteps_stone", transform, 2);
            playSource.Stop();
        }

        #endregion

        private void CheckGround()
        {
            //Debug.DrawLine(transform.position, transform.position - new Vector3(0, _distance, 0), Color.red, 1f);
            bool grounded = Physics2D.Linecast(transform.position, transform.position - new Vector3(0, _distance, 0), _groundLayer);
            //RaycastHit2D hit2D = Physics2D.BoxCast(transform.position, _boxSize, 0, Vector2.down, _distance, _groundLayer);
            //Debug.DrawRay(transform.position, transform.position - new Vector3(0, _distance,0), new Color(0.5f, 0.5f, 0.5f));
            if (grounded)
            {
                if (_moveVelocity.y > 0)   //在上升中不用检查
                    return;
                if (_isGround == false)
                {
                    AudioManger.Instance.PlayAudio("hero_land_soft", transform.position);
                    _animator.SetBool("IsGround", true);
                }
                _isGround = true;
                _animator.SetBool("IsStopUp", false);
                _jumpCount = 0;
                if (_moveVelocity.y < 0)
                    _moveVelocity.y = 0;
            }
            else
            {
                _animator.SetBool("IsGround", false);
                _isGround = false;
            }
        }
        void OnDrawGizmos()
        {
            //Gizmos.color = new Color(1, 0, 1, 0.5F);
            //Gizmos.DrawRay(transform.position,  new Vector3(0, -0.65f, 0));
        }

        private void UpdateGravity()
        {
            if (!_gravityEnable)
                return;
            if (!_isGround)
                _moveVelocity.y -= _gravity;
        }

        private void Jump()
        {
            if (!_inputEnable)
                return;
            if (Input.GetKeyDown(KeyCode.Space))
            {
                _jumpCount++;
                if (_jumpCount == 1)
                {
                    _animator.SetTrigger("IsJump");
                    _playerState = PlayerState.Jump;
                    _moveVelocity.y = _jumpSpeed;
                    AudioManger.Instance.PlayAudio("hero_jump", transform.position);
                }
                else if (_jumpCount == 2 && RuneManager.IsCanSecondJump)
                {
                    _animator.SetTrigger("IsJumpTwo");
                    _playerState = PlayerState.Jump;
                    _moveVelocity.y = _secondJumpSpeed;
                    var effectObj = GameObjectPool.Instance().GetPool(_jumpTwoEffect, transform.position);
                    effectObj.transform.rotation = Quaternion.Euler(180, 0, 0);
                    ParticleSystem ps = effectObj.GetComponent<ParticleSystem>();
                    ps.Play();
                    //Instantiate(_jumpTwoEffect, transform.position, Quaternion.Euler(180, 0, 0));    //生成粒子特效物体
                    effectObj.transform.position += new Vector3(0, 0.5f, 0);    //为了效果，调高生成位置
                    AudioManger.Instance.PlayAudio("jump_out_of_snow", transform.position);
                }
            }
            else if (Input.GetKeyUp(KeyCode.Space))
                _timeJump = 0;

            //进入上跳减速状态，但还在上升
            if (_moveVelocity.y > 0 && _moveVelocity.y < 11f)
            {
                _animator.SetBool("IsSlowUp", true);
            }
            else
                _animator.SetBool("IsSlowUp", false);

            //进入下落状态
            if (_moveVelocity.y < 0)
            {
                _animator.SetBool("IsDown", true);
                _animator.SetBool("IsStopUp", true);
            }
            else
                _animator.SetBool("IsDown", false);
        }

        private void CheckNextMove()
        {
            _moveVelocity.x = Mathf.Clamp(_moveVelocity.x, -30, 30);
            //print(_moveVelocity);
            Vector3 moveDistance = _moveVelocity * Time.deltaTime;
            //transform.position += moveDistance;
            PlayRigidbody.velocity = _moveVelocity;
            var rotation = transform.rotation;
            rotation.z = 0;
            transform.rotation = rotation;
        }

        public void ClosePage()
        {
            IsOpenPage = false;
            Time.timeScale = 1;
        }

        public void OpenPage()
        {
            IsOpenPage = true;
            Time.timeScale = 0;
        }

        public void SetIsOpenPage(bool isOpenPage)
        {
            IsOpenPage = isOpenPage;
        }

        private void Reverse()
        {
            var scale = transform.localScale;
            scale.x *= -1;
            transform.localScale = scale;
        }

        private void NoOperate()
        {
            if (!_inputEnable)
            {
                _disableInputTimeCount += Time.deltaTime;
                if (_disableInputTimeCount >= _disableInputTime)
                {
                    _inputEnable = true;
                }
            }
            else
                _disableInputTimeCount = 0;
        }

        //敌人进入触发区域
        public void OnTriggerStay2D(Collider2D collision)
        {
            if (_isInjure)
                return;
            if (collision.tag == "PlayAttack")
                return;
            if (collision.tag == "Untagged")
                return;
            if (_unbeatable)
                return;
            Injured(collision);
        }

        #region 保存、读取游戏

        public void SetIsCanSave(bool isCanSave, Vector3 savePosition)
        {
            IsCanSave = isCanSave;
            SaveTextManager.DisplayCanSave(isCanSave);
        }

        void SaveGameData()
        {
            if (Input.GetKeyDown(KeyCode.I))
            {
                SaveData saveData = new SaveData();
                saveData.PlayPosition = transform.position;
                saveData.BloodNum = PlayBloodManager.Instance.GetBloodNum();
                JsonManage<SaveData>.SaveJsonData(saveData, Path);
                if(RuneManager.IsInstance())
                {
                    File.WriteAllText(ConstantManager.SavePath, File.ReadAllText(ConstantManager.OriginalPath));
                }
                SaveTextManager.DisplaySaveOver();
                GameDataManager.instance.isLoadNew = false;
            }
        }

        public void ReadGameData()
        {
            if (GameDataManager.instance.isLoadNew)
            {
                PlayBloodManager.Instance.SetBlood(5);
            }
            else
            {
                SaveData saveData = JsonManage<SaveData>.ReadJsonData(Path);
                if (saveData == default)
                {
                    PlayBloodManager.Instance.SetBlood(5);
                }
                else
                {
                    transform.position = saveData.PlayPosition;
                    PlayBloodManager.Instance.gameObject.transform.position = new Vector3(saveData.PlayPosition.x, 
                        saveData.PlayPosition.y, PlayBloodManager.Instance.gameObject.transform.position.z);
                    if (GameDataManager.instance.isRevive)
                    {
                        GameDataManager.instance.isRevive = false;
                        PlayBloodManager.Instance.SetBlood(5);
                    }
                    else
                        PlayBloodManager.Instance.SetBlood(saveData.BloodNum);
                }
            }
            RuneManager.Instance.Init(GameDataManager.instance.isLoadNew);
        }

        #endregion

    }

    public enum PlayerState
    {
        Run,
        Jump,
        Stop,
    }

    public enum AttackDir
    {
        None,
        Left,
        Up,
        Right,
        Down
    }
}
