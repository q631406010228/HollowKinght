using System;
using UnityEngine;

namespace Assets.Level2Script.Script
{
    public class PlayCameraMove : MonoBehaviour
    {

        public Transform _player;   //玩家
        Vector2 _boxSize;            //视野范围
        bool _cameraMove;
        bool isBoss = false;
        bool isStop = false;
        Vector3 bossScene;
        public GameObject boss;

        void Start()
        {
            _boxSize = new Vector2(2, 2);
            _cameraMove = false;
        }

        void LateUpdate()
        {
            if (isStop)
                return;
            if (isBoss)
            {
                BossSceneMove();
                return;
            }
            if (_cameraMove)
            {
                FollowPlayer();
            }
            else
            {
                CheckBoundary();
            }
        }

        private void FollowPlayer()
        {
            Vector3 targetPos = new Vector3(_player.position.x, _player.position.y, transform.position.z);
            transform.position = Vector3.Lerp(transform.position, targetPos, 0.01f);
            float distance = Vector3.Distance(transform.position, targetPos);
            if(distance < 0.5f)
            {
                _cameraMove = false;
            }
        }

        private void CheckBoundary()
        {
            float distanceX = _player.position.x - transform.position.x;
            if (Math.Abs(distanceX) > _boxSize.x / 2)
            {
                _cameraMove = true;
            }
            float distanceY = _player.position.y - transform.position.y;
            if (Math.Abs(distanceY) > _boxSize.y / 2)
            {
                _cameraMove = true;
            }
        }

        public void BossScene(Vector3 vector3)
        {
            bossScene = vector3;
            isBoss = true;
        }

        private void BossSceneMove()
        {
            transform.position = Vector3.Lerp(transform.position, bossScene, 0.01f);
            float distance = Vector3.Distance(transform.position, bossScene);
            if (distance < 0.5f)
            {
                boss.SetActive(true);
                isStop = true;
            }
        }

    }
}
