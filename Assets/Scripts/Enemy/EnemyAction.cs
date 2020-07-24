using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Enemy
{
    public interface EnemyAction
    {
        void Patrol();
        void Attack();

        void SetActivate();
    }
}
