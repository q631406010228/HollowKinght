using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.CommonMethod
{
    public class GameDataManager
    {
        public static readonly GameDataManager instance = new GameDataManager();

        public bool isLoadNew = true;
        public bool isRevive = false;
    }
}
