using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehaviour : MonoBehaviour
{
    private const int movementDuration = 200;
    private const float movementRadius = 50;
    private const float idleMovementSpeed = 0.1f;
    private const float chaseMovementSpeed = 0.3f;
    private const float rotationSpeed = 0.01f;
    private const float aggroRange = 50;

    private bool chaseMode;
    private int framesSinceMovementStart;
    private Vector3 movementDirection;
    private Vector3 home;
    GameObject[] player;

    // Start is called before the first frame update
    void Start()
    {
        chaseMode = false;
        framesSinceMovementStart = movementDuration;
        home = transform.position;

        player = GameObject.FindGameObjectsWithTag("Player");
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

        if(player.Length >= 1  && Vector3.Distance(transform.position, player[0].transform.position) < aggroRange)
        {
            return true;
        } else
        {
            return false;
        }   
    }

    public void randomMove()
    {
        if(framesSinceMovementStart >= movementDuration)
        {

            if(Random.Range(0, 20) < 1)
            {
                movementDirection = new Vector3(0, 0, 0);
                framesSinceMovementStart = -movementDuration;
            } else
            {
                do
                {
                    movementDirection = Random.insideUnitCircle.normalized;
                } while (Physics.Raycast(transform.position, movementDirection, idleMovementSpeed * movementDuration));
                framesSinceMovementStart = 0;
            }
            //Generate new destination point

            
            
        } else
        {
            //Continue movement

            //Rotate towards target
            if(movementDirection != Vector3.zero)
            {
                transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(movementDirection, Vector3.up), rotationSpeed);

                //Move if there is no danger ahead
                if (!Physics.Raycast(transform.position, transform.forward, idleMovementSpeed * 5))
                {
                    transform.position += transform.forward * idleMovementSpeed;
                }
                else
                {
                    framesSinceMovementStart = movementDuration;
                }
            }

            framesSinceMovementStart++;

            if (Vector3.Distance(transform.position, home) > movementRadius)
            {
                Quaternion save = transform.rotation;
                transform.LookAt(home);
                movementDirection = transform.forward;
                transform.rotation = save;
                framesSinceMovementStart = 0;
            }

        }
    }

    public void chase()
    {
        if (framesSinceMovementStart >= movementDuration)
        {
            //Generate new destination point
        }
        else
        {
            //Continue movement
        }
    }
}
