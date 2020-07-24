using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class EscUIRoot : MonoBehaviour
{
    Button exitBtn;

    // Start is called before the first frame update
    void Start()
    {
        exitBtn = transform.Find("ExitButton").GetComponent<Button>();
        var trigger = exitBtn.gameObject.AddComponent<EventTrigger>();
        RegisterEvent(trigger, exitBtn.gameObject);
        exitBtn.onClick.AddListener(ExitBtnClick);
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
    public void RegisterEvent(EventTrigger trigger, GameObject btnObj)
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

    public void ExitBtnClick()
    {
        Application.Quit();
    }
}
