using UnityEngine;

namespace Assets.CommonMethod
{
    public class BloodManager
    {
        public int _bloodVolume;
        public int _maxBloodVolume;

        public BloodManager()
        {

        }

        public void ReduceBlood(int reduceBloodNum = 1)
        { 
            if(_bloodVolume > reduceBloodNum)
            {
                _bloodVolume -= reduceBloodNum;
            }
            else
            {
                _bloodVolume = 0;
            }
        }

        public void AddBlood()
        {
            if(_bloodVolume <　_maxBloodVolume)
                _bloodVolume++;
        }

        public bool IsDie()
        {
            if (_bloodVolume > 0)
                return false;
            else
                return true;
        }

    }
}
