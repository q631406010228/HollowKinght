using Assets.Page;
using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using static Assets.GameDictionary;

public class MapManager : MonoBehaviour, IDragHandler, IPointerDownHandler, PageBasicManager
{

    public Transform player;
    public Image playerMapIcon;
    public RectTransform smallMap;

    private RawImage img;
    Vector3 offsetPos; //存储按下鼠标时的图片-鼠标位置差

    void Start()
    {
        img = GetComponent<RawImage>();//获取图片，因为我们要获取他的RectTransform
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (img == null)
            return;
        //将鼠标的位置坐标进行钳制，然后加上位置差再赋值给图片position
        img.rectTransform.position = new Vector3(Mathf.Clamp(Input.mousePosition.x, 0, Screen.width), Mathf.Clamp(Input.mousePosition.y, 0, Screen.height), 0) + offsetPos;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (img == null)
            return;
        offsetPos = img.rectTransform.position - Input.mousePosition;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void Open()
    {
        //Vector3 playerIconPos = playerMapIcon.gameObject.transform.position;
        Vector3 playerPos = player.position;
        RectTransform thisTrans = (RectTransform)playerMapIcon.gameObject.transform;
        Vector3 playerIconPos = new Vector3(playerPos.x * 16.8f, playerPos.y * 16.3f);
        float revise = playerIconPos.x + smallMap.anchoredPosition.x;
        smallMap.anchoredPosition = new Vector2(smallMap.anchoredPosition.x + 700f - revise, smallMap.anchoredPosition.y);
        thisTrans.anchoredPosition3D = playerIconPos;
    }

    public void Close()
    {

    }

    public Page GetClassify()
    {
        return Page.Map;
    }

    public GameObject GetGameObject()
    {
        return this.gameObject;
    }
}
