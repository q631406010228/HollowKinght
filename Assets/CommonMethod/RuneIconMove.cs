using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RuneIconMove : MonoBehaviour
{
    Vector3 StartPos;
    public Vector3 EndPos { get; set; }
    public string runeName { get; set; }
    RectTransform thisTrans;
    bool isMove = false;
    public bool IsEquipped = false;
    float speed;

	void Start ()
    {
        speed = 0.3f;
    }

    void Update ()
    {
        if (isMove)
        {
            MoveToTarget();
        }
	}
    /// <summary>
    /// 初始化图标的相应属性
    /// </summary>
    public void Init(Vector3 startPos, Vector3 endPos)
    {
        thisTrans = (RectTransform)transform;
        thisTrans.anchoredPosition3D = startPos;
        EndPos = endPos;
        isMove = true;
    }

    public void MoveToTarget()
    {
        thisTrans.anchoredPosition3D = Vector3.Lerp(thisTrans.anchoredPosition3D, EndPos, speed);
        float distance = Vector3.Distance(thisTrans.anchoredPosition3D, EndPos);
        if (distance < 2)
        {
            isMove = false;
        }
    }

}
