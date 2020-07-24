using Assets.Level2Script.Script;
using Assets.Page;
using System;
using System.Collections.Generic;
using UnityEngine;
using static Assets.GameDictionary;

public class PageManager : MonoBehaviour
{
    public RectTransform MapPage;
    public RectTransform RunePage;
    public PlayController play;
    public GameObject EscUI;

    float moveSpeed = 30;
    bool isMove = false;
    PageBasicManager currentPage;
    RectTransform nextMovePage;
    RectTransform currentMovePage;
    Dir moveDir = Dir.Left;
    RuneManager runeManager;
    MapManager mapManager;
    Dictionary<Page, PageBasicManager> Pages = new Dictionary<Page, PageBasicManager>();
    bool isOpenPage = false;
    bool isOpenEscUI = false;

    // Start is called before the first frame update
    void Start()
    {
        runeManager = RunePage.gameObject.GetComponent<RuneManager>();
        Pages.Add(Page.Rune, runeManager);
        mapManager = MapPage.gameObject.GetComponent<MapManager>();
        Pages.Add(Page.Map, mapManager);
    }

    // Update is called once per frame
    void Update()
    {
        if(isMove)
            MovePage();
        else
            SetPage();
    }

    public void SetPage(int dir)
    {
        if(dir == -1)
        {
            moveDir = Dir.Left;
        }
        else
        {
            moveDir = Dir.Right;
        }
        if(currentPage.GetClassify() == Page.Map)
        {
            currentMovePage = MapPage ;
            RunePage.anchoredPosition = new Vector2(Screen.width * dir * -1, 0);
            nextMovePage = RunePage;
            nextMovePage.gameObject.SetActive(true);
        }
        else if(currentPage.GetClassify() == Page.Rune)
        {
            currentMovePage = RunePage;
            MapPage.anchoredPosition = new Vector2(Screen.width * dir * -1, 0);
            nextMovePage = MapPage;
            nextMovePage.gameObject.SetActive(true);
        }
        isMove = true;
    }

    void MovePage()
    {
        if(nextMovePage.anchoredPosition.x == 0)
        {
            isMove = false;
            currentMovePage.gameObject.SetActive(false);
            if(currentPage.GetClassify() == Page.Map)
            {
                currentPage = runeManager;
                currentMovePage = RunePage;
            }
            else
            {
                currentPage = mapManager;
                currentMovePage = MapPage ;
            }
        }
        else
        {
            float moveDistance = moveSpeed * (int)moveDir;
            if(Math.Abs(nextMovePage.anchoredPosition.x) < Math.Abs(moveDistance))
            {
                nextMovePage.anchoredPosition = new Vector2(0, 0);
            }
            else
            {
                nextMovePage.anchoredPosition = new Vector2(nextMovePage.anchoredPosition.x + moveDistance, 0);
                currentMovePage.anchoredPosition = new Vector2(currentMovePage.anchoredPosition.x + moveDistance, 0);
            }
        }
    }

    private void SetPage()
    {
        if (isOpenPage == true)
        {
            if (Input.GetKey(KeyCode.Escape))
                Close();
            else if (Input.GetKeyDown(KeyCode.B))
            {
                if (currentPage.GetClassify() == Page.Rune)
                    Close();
                else
                    SetPage(-1);
            }
            else if (Input.GetKeyDown(KeyCode.M))
            {
                if (currentPage.GetClassify() == Page.Map)
                    Close();
                else
                    SetPage(-1);
            }
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.B) && !isOpenEscUI)
            {
                Open(runeManager);
                RunePage.anchoredPosition = new Vector2(0, 0);
            }
            else if (Input.GetKeyDown(KeyCode.M) && !isOpenEscUI)
            {
                Open(mapManager);
                MapPage.anchoredPosition = new Vector2(0, 0);
            }
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                if (isOpenEscUI)
                {
                    isOpenEscUI = false;
                    Time.timeScale = 1;
                    EscUI.SetActive(false);
                }
                else
                {
                    isOpenEscUI = true;
                    Time.timeScale = 0;
                    EscUI.SetActive(true);
                }
            }
        }
    }

    void Open(PageBasicManager page)
    {
        isOpenPage = true;
        currentPage = page;
        currentPage.GetGameObject().SetActive(true);
        currentPage.Open();
        play.OpenPage();
    }

    void Close()
    {
        isOpenPage = false;
        currentPage.GetGameObject().SetActive(false);
        currentPage.Close();
        play.ClosePage();
    }
}
