using Assets.Level2Script.Script;
using UnityEngine;
using UnityEngine.UI;

public class BossBegin : MonoBehaviour
{
    public GameObject door;
    public PlayCameraMove playCamera;
    public GameObject text;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag != "Player")
            return;
        playCamera.BossScene(new Vector3(180, -6, -10));
        door.SetActive(true);
        this.gameObject.SetActive(false);
        text.SetActive(false);
    }
}
