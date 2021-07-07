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

    [SerializeField] private Transform Icon;

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

        Icon.rotation = Quaternion.Euler(Vector3.zero);
    }

    /// <summary> 
    ///     Checks if the player is within the aggroRange and returns a boolean according to the result
    /// </summary>
    private bool checkPlayerVicinity()
    {

        if(player.Length >= 1  && Vector3.Distance(transform.position, player[0].transform.position) < values.aggroRange)
        {
            return true;
        } else
        {
            return false;
        }   
    }

    /// <summary> 
    ///     Handles behaviour when enemy is not engaged in combat
    /// </summary>
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

    /// <summary> 
    ///     Handles behaviour when enemy is engaged in combat
    /// </summary>
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

    /// <summary> 
    ///     Generates a random point for the enemy to move towards
    /// </summary>
    private void generateNewDestinationPoint()
    {
        do
        {
            movementDirection = Random.insideUnitCircle.normalized;
        } while (Physics.Raycast(transform.position, movementDirection, values.speedIdle * values.movementDuration));
        framesSinceMovementStart = 0;
    }

    /// <summary> 
    ///    Continues enemy movement to the target point
    ///    <param name="speed"> Moevment Speed of the enemy</param>
    /// </summary>
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

    /// <summary> 
    ///    Sets the current movement direction to face the given point
    ///    <param name="point"> Point to set movement direction towards</param>
    /// </summary>
    private void setMovementDirectionTowardsPoint(Vector3 point)
    {
        Quaternion save = transform.rotation;
        transform.LookAt(point);
        movementDirection = transform.forward;
        transform.rotation = save;
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, values.aggroRange);
    }
}
