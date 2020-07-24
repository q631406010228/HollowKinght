using Assets.CommonMethod;
using Assets.Page;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using static Assets.GameDictionary;

public class RuneManager : MonoBehaviour, PageBasicManager
{
    private static RuneManager _instance = null;

    public static RuneManager Instance
    {
        get
        {
            if (_instance == null) _instance = new RuneManager();
            return _instance;
        }
        
    }
    public Text RuneName;
    public Image RuneIcon;
    public Text RuneIntroduce;
    public Dictionary<string, Rune> RuneAll = new Dictionary<string, Rune>();

    List<RuneIconMove> RunesEquipped = new List<RuneIconMove>();
    List<Vector3> RunesPos = new List<Vector3>();
    List<Vector3> _runesEquippedPos = new List<Vector3>();
    string _runeSelected = "Rune1";
    int _runesEquippedOrder;

    public static bool IsAddBlood = false;
    public static bool IsCanSecondJump = false;
    public static bool IsCanMiss = false;
    public static bool isNewGame;
    public static string SavePath ;
    public static string OriginalPath ;
    static string path;

    void Awake()
    {
        _instance = new RuneManager();
        _runesEquippedOrder = 0;
        GameObject runes = GameObject.Find("Runes").gameObject;
        GameObject runesPos = GameObject.Find("RunesPos").gameObject;
        GameObject runesEquippedPos = GameObject.Find("RunesEquippedPosition").gameObject;
        foreach (Transform child in runes.transform)
        {
            var trigger = child.gameObject.AddComponent<EventTrigger>();
            RegisterEvent(trigger, child.gameObject);
        }
        foreach (Transform child in runesPos.transform)
        {
            RectTransform rectTransform = (RectTransform)child.gameObject.transform;
            RunesPos.Add(rectTransform.anchoredPosition3D);
        }
        foreach (Transform child in runesEquippedPos.transform)
        {
            RectTransform rectTransform = (RectTransform)child.gameObject.transform;
            _runesEquippedPos.Add(rectTransform.anchoredPosition3D);
        }
        ReadRuneData();
        RuneInit(runes);
        ChangeRuneIntroduce(_runeSelected);
    }

    public void Init(bool isNewGame)
    {
        SavePath = Application.dataPath + "/Data/RuneData.txt";
        OriginalPath = Application.dataPath + "/Data/OriginalRuneData.txt";
        if (isNewGame)
            path = OriginalPath;
        else
            path = SavePath;
        //Rune rune = new Rune();
        //rune.Name = "天使之吻";
        //rune.IconName = "Rune1";
        //rune.Address = "背包/Rune/Rune1";
        //rune.Introduce = "专治跌打损伤,包治包好,相信我";
        //rune.Order = 0;
        //rune.EquippedOrder = -1;
        //RuneAll.Add(rune.IconName, rune);
        ReadRuneData();
        foreach(var item in RuneAll)
        {
            if(item.Value.EquippedOrder > -1)
                RuneStatusManager(item.Key, true);
            else
                RuneStatusManager(item.Key, false);
        }
    }

    private void RuneInit(GameObject runes)
    {
        foreach(Transform child in runes.transform)
        {
            GameObject rune = child.gameObject;
            RuneIconMove runeIconMove = rune.GetComponent<RuneIconMove>();
            int equippedOrder = RuneAll[rune.name].EquippedOrder;
            if (equippedOrder != -1)
            {
                RectTransform thisTrans = (RectTransform)rune.transform;
                thisTrans.anchoredPosition3D = _runesEquippedPos[equippedOrder];
                EquipRune(runeIconMove, rune.name);
            }
        }
    }

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
    }

    /// <summary>
    /// 注册UI事件
    /// </summary>
    /// <param name="trigger"></param>
    /// <param name="btnObj"></param>
    public void RegisterEvent(EventTrigger trigger, GameObject rune)
    {
        EventTrigger.Entry enterClick = new EventTrigger.Entry();
        enterClick.eventID = EventTriggerType.PointerEnter;

        EventTrigger.Entry click = new EventTrigger.Entry();
        click.eventID = EventTriggerType.PointerClick;

        enterClick.callback.AddListener((o) =>
        {
            _runeSelected = rune.name;
            ChangeRuneIntroduce(rune.name);
            AudioManger.Instance.PlayAudioUI("button", transform);
        });

        click.callback.AddListener((o) =>
        {
            RuneIconMove runeIconMove = rune.GetComponent<RuneIconMove>();
            RectTransform startPos = (RectTransform)rune.transform;
            if(RuneAll[rune.name].EquippedOrder > -1)
            {
                runeIconMove.Init(startPos.anchoredPosition3D, RunesPos[RuneAll[rune.name].Order]);
                runeIconMove.IsEquipped = false;
                ReRuneEquippedSort(rune.name);
                RunesEquipped.Remove(runeIconMove);
                RuneStatusManager(rune.name, false);
                RuneAll[rune.name].EquippedOrder = -1;
                _runesEquippedOrder--;
            }
            else
            {
                if (_runesEquippedOrder >= _runesEquippedPos.Count)
                    return;
                runeIconMove.Init(startPos.anchoredPosition3D, _runesEquippedPos[_runesEquippedOrder]);
                RuneAll[rune.name].EquippedOrder = _runesEquippedOrder;
                EquipRune(runeIconMove, rune.name);
            }
        });

        trigger.triggers.Add(enterClick);
        trigger.triggers.Add(click);
    }

    private void EquipRune(RuneIconMove runeIconMove, string runeName)
    {
        runeIconMove.IsEquipped = true;
        RunesEquipped.Add(runeIconMove);
        RuneStatusManager(runeName, true);
        _runesEquippedOrder++;
    }

    private void ChangeRuneIntroduce(string runeSelected)
    {
        RuneIcon.sprite = Resources.Load(RuneAll[runeSelected].Address, typeof(Sprite)) as Sprite;
        RuneName.text = RuneAll[runeSelected].Name;
        RuneIntroduce.text = RuneAll[runeSelected].Introduce;
    }

    private void ReRuneEquippedSort(string runeName)
    {
        foreach(var item in RunesEquipped)
        {
            if (RuneAll[runeName].EquippedOrder < RuneAll[item.gameObject.name].EquippedOrder)
            {
                RectTransform thisTrans = (RectTransform)item.gameObject.transform;
                Vector3 endPos = new Vector3(thisTrans.anchoredPosition3D.x - 150, thisTrans.anchoredPosition3D.y, 0);
                item.Init(thisTrans.anchoredPosition3D, endPos);
                RuneAll[item.gameObject.name].EquippedOrder--;
            }
        }
    }

    private void RuneStatusManager(string runeName, bool isCan)
    {
        switch (runeName)
        {
            case "Rune1":
                IsAddBlood = isCan;
                break;
            case "Rune2":
                IsCanSecondJump = isCan;
                break;
            case "Rune3":
                IsCanMiss = isCan;
                break;
            default:
                break;
        }
    }

    public void Open()
    {
        ReadRuneData();
    }

    public void Close()
    {
        SaveRuneData();
    }

    public GameObject GetGameObject()
    {
        return this.gameObject;
    }

    public Page GetClassify()
    {
        return Page.Rune;
    }

    public void ReadRuneData()
    {
        RuneAll = JsonManage<Rune>.ReadJsonDataDictionary(path);
    }

    public void SaveRuneData()
    {
        JsonManage<Rune>.SaveJsonDataDictionary(RuneAll, SavePath);
        path = SavePath;
    }

    public static bool IsInstance()
    {
        return _instance == null ? false : true;
    }

}
