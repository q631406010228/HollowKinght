using UnityEngine;

namespace Assets.Page
{
    public interface PageBasicManager
    {
        void Open();

        void Close();

        GameObject GetGameObject();

        GameDictionary.Page GetClassify();
    }
}
