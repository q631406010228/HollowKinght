using UnityEngine;

public class WaveControl : MonoBehaviour
{
    public GameObject failChampion;
    float rawX;
    float moveSpeed = 6f;
    float distance;
    bool IsActive = false;

    // Start is called before the first frame update
    void Start()
    {
        rawX = transform.position.x;
        distance = Mathf.Abs(transform.position.x - failChampion.transform.position.x);
    }

    // Update is called once per frame
    void Update()
    {
        float x;
        if (!IsActive)
        {
            if (failChampion.transform.localScale.x > 0)
            {
                transform.position = new Vector2(failChampion.transform.position.x + distance, transform.position.y);
            }
            else
            {
                transform.position = new Vector2(failChampion.transform.position.x - distance, transform.position.y);
            }
            rawX = transform.position.x;
            IsActive = true;
        }
        if (Mathf.Abs(transform.position.x - rawX) > 10f)
        {
            IsActive = false;
            gameObject.SetActive(false);
        }
        else
        {
            
            if (failChampion.transform.localScale.x > 0)
            {
                x = moveSpeed * Time.deltaTime + transform.position.x;
            }
            else
            {
                x = transform.position.x - (moveSpeed * Time.deltaTime);
            }
            transform.position = new Vector2(x, transform.position.y);
        }
    }
}
