using System;
using UnityEngine;

namespace Assets.Level2Script.Script
{
    class EnemyAttack : MonoBehaviour
    {
        public float _moveSpeed = 0.1f;
        public float _attackDistance = 3;
        public float _gravity = 1;       //受到的重力
        public float _waitTime = 2f;
        bool _isJump;
        decimal _absDis;
        decimal _originalPointX;

        decimal _a;
        decimal _b;
        GameObject Player;
        Animator _animator;
        Dir _enemyDirBefor;
        Vector2 _moveVelocity;
        decimal targetPointX;
        float rawHeight;
        float _countTime;
        bool _canNextMove = false;
        bool _isJumpAttack = false;

        void Awake()
        {
            _moveVelocity = new Vector2(0, 0);
            Player = GameObject.Find("Player");
            _enemyDirBefor = Dir.Right;
            rawHeight = transform.position.y;
            //_animator = GetComponent<Animator>();
        }

        void Start()
        {
            _animator = GetComponent<Animator>();
        }

        void Update()
        {
            if(!_isJump)
                _countTime += Time.deltaTime;
            if (_countTime > _waitTime)
            {
                _countTime = 0;
                _canNextMove = true;
            }
            float dis = transform.position.x - Player.transform.position.x;
            if (dis < 0)
                JudgeDis(Dir.Right);
            if (dis > 0)
                JudgeDis(Dir.Left);
            if (Math.Abs(dis) > _attackDistance + 3f || _isJump)
                JumpAttack();
            else
            {
                CommonAttack();
            }
            Vector3 moveDistance = _moveVelocity * Time.deltaTime;
            transform.position += moveDistance;
        }

        public void JumpAttack()
        {
            if (!_isJump && _canNextMove)
            {
                targetPointX = (decimal)Math.Abs(transform.position.x - Player.transform.position.x) - (decimal)_attackDistance;
                _moveSpeed = (float)targetPointX * 1.5f;
                _isJump = true;
                _isJumpAttack = false;
                _moveVelocity.x = (int)_enemyDirBefor * _moveSpeed;
                _animator.SetBool("IsJumping", true);
                _absDis = Math.Abs(targetPointX / 2.0m);
                _originalPointX = (decimal)transform.position.x;
                Equation(_absDis, 5, targetPointX, 0.5m);
                _canNextMove = false;
            }
            else if(_isJump)
            {
                decimal x = (decimal)Math.Abs((decimal)transform.position.x - _originalPointX);
                Vector3 vector3 = new Vector3(transform.position.x, (float)(_a * x * x + _b * x) + rawHeight, 0);
                transform.position = vector3;
                if(vector3.y < rawHeight + 3f && x > _absDis && !_isJumpAttack)
                {
                    _animator.SetBool("IsJumping", false);
                    _animator.SetTrigger("IsAttack");
                    _isJumpAttack = true;
                }
                if (vector3.y < rawHeight + 0.02f && x > _absDis)
                {
                    Vector3 vector = new Vector3(transform.position.x, rawHeight, 0);
                    transform.position = vector;
                    _moveVelocity.x = 0;
                    _isJump = false;
                }
            }
        }

        private void CommonAttack()
        {
            if (_canNextMove)
            {
                _animator.SetTrigger("IsAttack");
                _canNextMove = false;
            }
        }

        private void JudgeDis(Dir nowDir)
        {
            if (nowDir == Dir.Right && _enemyDirBefor != Dir.Right)
            {
                _enemyDirBefor = Dir.Right;
                Reverse();
            }
            else if (nowDir == Dir.Left && _enemyDirBefor != Dir.Left)
            {
                _enemyDirBefor = Dir.Left;
                Reverse();
            }
        }

        private void Reverse()
        {
            var scale = transform.localScale;
            scale.x *= -1;
            transform.localScale = scale;
        }

        private void Equation(decimal x1, decimal y1, decimal x2, decimal y2)       //求出曲线的y=ax^2 + bx + c中的a和b，其实c取自boss的中心点高度
        {
            _b = (0.5m * x2 * x2 - 0.5m * x1 * x1 - x2 * x2 * y1 + x1 * x1 * y2) / (decimal)(x2 * x1 * x1 - x1 * x2 * x2);
            _a = (-_b * x2 - 0.5m) / (decimal)(x2 * x2);
        }

        //敌人进入触发区域
        //void OnTriggerEnter2D(Collider2D collision)
        //{
        //    Enemy.Add(collision.gameObject);
        //    BehaviorTree behaviorTree = collision.gameObject.GetComponent<BehaviorTree>();
        //    if (behaviorTree != null)
        //        behaviorTree.enabled = true;
        //}

            //void OnTriggerExit2D(Collider2D collision)
            //{
            //    Enemy.Remove(collision.gameObject);
            //    BehaviorTree behaviorTree = collision.gameObject.GetComponent<BehaviorTree>();
            //    if (behaviorTree != null)
            //        behaviorTree.enabled = false;
            //}

    }
}
