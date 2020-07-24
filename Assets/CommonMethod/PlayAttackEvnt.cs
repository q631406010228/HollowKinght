using Assets.Level2Script.Script;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayAttackEvnt : MonoBehaviour
{
    PlayController _playController;

    void Awake()
    {
        _playController = transform.parent.gameObject.GetComponent<PlayController>();
    }
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Enemy" || collision.tag == "Trap")
        {
            _playController.AttackDownReaction();
        }
    }
}
