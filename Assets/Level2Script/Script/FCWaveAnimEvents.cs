using System.Collections.Generic;
using UnityEngine;

public class FCWaveAnimEvents : MonoBehaviour
{
    BoxCollider2D boxCollider2D;
    GameObject waveCollider1;
    GameObject waveCollider2;
    GameObject waveCollider3;
    List<Vector2> boxColOffset;
    List<Vector2> boxColSize;


    GameObject _failChampionAttack;

    void Awake()
    {
        waveCollider1 = transform.Find("WaveCollider1").gameObject;
        waveCollider2 = transform.Find("WaveCollider2").gameObject;
        waveCollider3 = transform.Find("WaveCollider3").gameObject;
        boxCollider2D = gameObject.GetComponent<BoxCollider2D>();
        boxColOffset = new List<Vector2>()
        {
            new Vector2(-0.4770432f, -0.4507504f),
            new Vector2(-0.3542824f, -0.4765947f),
            new Vector2(-0.3542824f, -0.3667562f),
        };
        boxColSize = new List<Vector2>()
        {
            new Vector2(0.3105011f, 0.8080606f),
            new Vector2(0.426796f, 0.756372f),
            new Vector2(0.426796f, 0.9760489f),
        };
    }

    void Start()
    {

    }

    void ChangeColliderIsShow(string msg)
    {
        switch (msg)
        {
            case "Wave1":
                {
                    boxCollider2D.offset = boxColOffset[1];
                    boxCollider2D.size = boxColSize[1];
                    break;
                }
            case "Wave2":
                {
                    boxCollider2D.offset = boxColOffset[2];
                    boxCollider2D.size = boxColSize[2];
                    break;
                }
            case "Wave3":
                {
                    boxCollider2D.enabled = false;
                    waveCollider1.SetActive(true);
                    break;
                }
            case "Wave4":
                {
                    waveCollider1.SetActive(false);
                    waveCollider2.SetActive(true);
                    break;
                }
            case "Wave5":
                {
                    waveCollider2.SetActive(false);
                    waveCollider3.SetActive(true);
                    break;
                }
            case "Wave6":
                {
                    boxCollider2D.enabled = true;
                    boxCollider2D.offset = boxColOffset[1];
                    boxCollider2D.size = boxColSize[1];
                    break;
                }
            default:
                break;
        }
    }
}
