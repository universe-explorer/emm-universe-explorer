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
    private const float aggroRange = 100;
    private int shotDelay = 50;

    private bool shooting;
    private bool sharpTurn;
    private bool chaseMode;
    private int framesSinceMovementStart;
    private Vector3 movementDirection;
    private Vector3 home;
    GameObject[] player;

    // Start is called before the first frame update
    void Start()
    {
        chaseMode = false;
        sharpTurn = false;
        shooting = false;
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

    private void randomMove()
    {
        shooting = false;

        if(framesSinceMovementStart >= movementDuration)
        {

            sharpTurn = false;

            if(Random.Range(0, 40) < 1)
            {
                //Small chance for the ship to not move
                movementDirection = new Vector3(0, 0, 0);
                framesSinceMovementStart = -movementDuration;
            } else
            {
                generateNewDestinationPoint();
            }

        } else
        {
            continueMovement(idleMovementSpeed);
        }
    }

    private void chase()
    {
        home = player[0].transform.position;

        if (framesSinceMovementStart >= movementDuration)
        {
            if(shooting)
            {
                generateNewDestinationPoint();
                shooting = false;
            } else
            {
                shooting = true;
            }

            framesSinceMovementStart = 0;

        }
        else
        {
            if(!shooting)
            {
                continueMovement(chaseMovementSpeed);
            } else
            {
                setMovementDirectionTowardsPoint(home);

                if(movementDirection != Vector3.zero)
                {
                    transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(movementDirection, Vector3.up), rotationSpeed * 5);
                }
                
                if(framesSinceMovementStart % shotDelay == 0)
                {
                    //TODO: Shoot
                    Debug.Log("This would be a shot if it was actually implemented");
                }

                framesSinceMovementStart++;
            }
        }
    }

    private void generateNewDestinationPoint()
    {
        do
        {
            movementDirection = Random.insideUnitCircle.normalized;
        } while (Physics.Raycast(transform.position, movementDirection, idleMovementSpeed * movementDuration));
        framesSinceMovementStart = 0;
    }

    private void continueMovement(float speed)
    {

        //Rotate towards target
        if (movementDirection != Vector3.zero)
        {
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(movementDirection, Vector3.up), rotationSpeed);

            //Move if there is no danger ahead
            if (!Physics.Raycast(transform.position, transform.forward, speed * 60))
            {

                if (!sharpTurn)
                {
                    transform.position += transform.forward * speed;
                }

            }
            else
            {
                //Emergency Maneuver
                if (!sharpTurn)
                {
                    sharpTurn = true;
                    movementDirection *= -1;
                    framesSinceMovementStart = 0;
                }
            }
        }

        framesSinceMovementStart++;

        //Return to home if too far away from it
        if (Vector3.Distance(transform.position, home) > movementRadius)
        {
            setMovementDirectionTowardsPoint(home);
            framesSinceMovementStart = 0;
        }
    }

    private void setMovementDirectionTowardsPoint(Vector3 point)
    {
        Quaternion save = transform.rotation;
        transform.LookAt(point);
        movementDirection = transform.forward;
        transform.rotation = save;
    }
}
