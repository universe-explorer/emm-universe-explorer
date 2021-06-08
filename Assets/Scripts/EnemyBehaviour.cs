using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehaviour : MonoBehaviour
{

    private bool chaseMode;
    // Start is called before the first frame update
    void Start()
    {
        chaseMode = false;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        chaseMode = checkPlayerVicinity();

        if(chaseMode)
        {
            chase();
        } else
        {
            randomMove();
        }
    }

    public bool checkPlayerVicinity()
    {
        //TODO
        return false;
    }

    public void randomMove()
    {
        //TODO
    }

    public void chase()
    {
        //TODO
    }
}
