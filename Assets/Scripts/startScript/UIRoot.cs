using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using Assets.CommonMethod;

public class UIRoot : MonoBehaviour {

    List<Button> btnOBjs;
    Button startBtn;
    Button loadBtn;
    Button achievementBtn;
    Button extraBtn;
    Button exitBtn;
    string path;

    private void Awake()
    {
        Application.targetFrameRate = 60;
    }

    void Start () {

        Init();
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void Init()
    {
        path = Application.dataPath + "/Data/SaveGameData.txt";
        btnOBjs = new List<Button>();
        startBtn = transform.Find("ButtonObjs/startBtn").GetComponent<Button>();
        loadBtn = transform.Find("ButtonObjs/loadBtn").GetComponent<Button>();
        exitBtn = transform.Find("ButtonObjs/exitBtn").GetComponent<Button>();
        btnOBjs.Add(startBtn);
        btnOBjs.Add(loadBtn);
        btnOBjs.Add(exitBtn);

        for (int i=0;i<btnOBjs.Count;i++)
        {
            var trigger = btnOBjs[i].gameObject.AddComponent<EventTrigger>();
            RegisterEvent(trigger,btnOBjs[i].gameObject);
        }

        startBtn.onClick.AddListener(StartBtnClick);
        loadBtn.onClick.AddListener(LoadBtnClick);
        exitBtn.onClick.AddListener(ExitBtnClick);
        SaveData saveData = JsonManage<SaveData>.ReadJsonData(path);
        if(saveData == null)
        {
            loadBtn.gameObject.SetActive(false);
        }
    }

    /// <summary>
    /// 注册UI事件
    /// </summary>
    /// <param name="trigger"></param>
    /// <param name="btnObj"></param>
    public void RegisterEvent(EventTrigger trigger,GameObject btnObj)
    {
        EventTrigger.Entry enterClick = new EventTrigger.Entry();
        enterClick.eventID = EventTriggerType.PointerEnter;

        EventTrigger.Entry exitClick = new EventTrigger.Entry();
        exitClick.eventID = EventTriggerType.PointerExit;

        GameObject leftIcon = btnObj.transform.Find("Masks/LeftMask/leftImage").gameObject;
        GameObject rightIcon = btnObj.transform.Find("Masks/RightMask/rightImage").gameObject;
        IconMove leftMove = leftIcon.AddComponent<IconMove>();
        leftMove.Init(0);
        IconMove rightMove = rightIcon.AddComponent<IconMove>();
        rightMove.Init(1);

        enterClick.callback.AddListener((o) =>
        {
            leftMove.MouseEnterClick();
            rightMove.MouseEnterClick();
            AudioManger.Instance.PlayAudio("button");
        });

        exitClick.callback.AddListener((o) =>
        {
            leftMove.MouseExitClick();
            rightMove.MouseExitClick();
        });

        trigger.triggers.Add(enterClick);
        trigger.triggers.Add(exitClick);
    }

    public void StartBtnClick()
    {
        GameDataManager.instance.isLoadNew = true ;
        SceneManager.LoadScene("Loading");
    }

    public void LoadBtnClick()
    {
        GameDataManager.instance.isLoadNew = false ;
        SceneManager.LoadScene("Loading");
    }

    public void ExitBtnClick()
    {
        Application.Quit();
    }
}
