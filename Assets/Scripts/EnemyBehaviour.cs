using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehaviour : MonoBehaviour
{

    public EnemyScriptableObject values;

    private bool shooting;
    private bool sharpTurn;
    private bool chaseMode;
    private int framesSinceMovementStart;
    private Vector3 movementDirection;
    private Vector3 home;
    GameObject[] player;
    Weapon weapon;
    CombatControllerEnemy combatController;

    // Start is called before the first frame update
    void Start()
    {
        chaseMode = false;
        sharpTurn = false;
        shooting = false;
        framesSinceMovementStart = values.movementDuration;
        home = transform.position;

        weapon = GetComponentInChildren<Weapon>();
        combatController = GetComponentInChildren<CombatControllerEnemy>();

        player = GameObject.FindGameObjectsWithTag("Player");

        initialiseFromScriptableObject();
    }

    private void initialiseFromScriptableObject()
    {
        combatController._Damage = values.damage;
        combatController.SetMaxHealth(values.maxHealth);

        transform.localScale = new Vector3(values.scale, values.scale, values.scale);
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

        if(player.Length >= 1  && Vector3.Distance(transform.position, player[0].transform.position) < values.aggroRange)
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

        if(framesSinceMovementStart >= values.movementDuration)
        {

            sharpTurn = false;

            if(Random.Range(0, 40) < 1)
            {
                //Small chance for the ship to not move
                movementDirection = new Vector3(0, 0, 0);
                framesSinceMovementStart = -values.movementDuration;
            } else
            {
                generateNewDestinationPoint();
            }

        } else
        {
            continueMovement(values.speedIdle);
        }
    }

    private void chase()
    {
        home = player[0].transform.position;

        if (framesSinceMovementStart >= values.movementDuration)
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
                continueMovement(values.speedChasing);
            } else
            {
                setMovementDirectionTowardsPoint(home);

                if(movementDirection != Vector3.zero)
                {
                    transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(movementDirection, Vector3.up), values.rotationSpeed * 5);
                }
                
                if(framesSinceMovementStart % values.shotDelay == 0)
                {
                    weapon.Fire();
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
        } while (Physics.Raycast(transform.position, movementDirection, values.speedIdle * values.movementDuration));
        framesSinceMovementStart = 0;
    }

    private void continueMovement(float speed)
    {

        //Rotate towards target
        if (movementDirection != Vector3.zero)
        {
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(movementDirection, Vector3.up), values.rotationSpeed);

            //Move if there is no danger ahead
            if (!Physics.Raycast(transform.position, transform.forward, speed * 60))
            {

              transform.position += transform.forward * speed;

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
        if (Vector3.Distance(transform.position, home) > values.movementRadius)
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
