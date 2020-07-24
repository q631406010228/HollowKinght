using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SaveTextManage : MonoBehaviour
{
    public Text CanSaveText;
    public Text SaveOverText;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if(SaveOverText.enabled)
            StartCoroutine(SaveOverTime(4f));
    }

    public void DisplayCanSave(bool isCanSave)
    {
        CanSaveText.enabled = isCanSave;
        if(isCanSave)
            SaveOverText.enabled = false;
    }

    public void DisplaySaveOver()
    {
        CanSaveText.enabled = false;
        SaveOverText.enabled = true;
    }

    IEnumerator SaveOverTime(float time)
    {
        yield return new WaitForSeconds(time);
        SaveOverText.enabled = false;
    }
}
