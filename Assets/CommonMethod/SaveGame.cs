using Assets.Level2Script.Script;
using UnityEngine;

public class SaveGame : MonoBehaviour
{
    public GameObject Play;
    public GameObject Halo;
    bool IsCanSave = false;
    PlayController playController;

    // Start is called before the first frame update
    void Start()
    {
        playController = Play.GetComponent<PlayController>();
    }

    // Update is called once per frame
    void Update()
    {
        float x = Vector3.Distance(Play.transform.position, transform.position);
        if(x < 3 && !IsCanSave)
        {
            IsCanSave = true;
            playController.SetIsCanSave(true, new Vector3(transform.position.x, -1.58f, 0));
            Halo.SetActive(true);
        }
        else if(x >= 3 && IsCanSave)
        {
            playController.SetIsCanSave(false, new Vector3(transform.position.x, -1.58f, 0));
            Halo.SetActive(false);
            IsCanSave = false;
        }
    }
}
