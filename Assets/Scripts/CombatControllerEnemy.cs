using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatControllerEnemy : AbstractCombatController
{
    public override void Die()
    {
        Debug.Log("Enemy Died");
        //Spawns a random item on death
        Item.ItemType itemType = (Item.ItemType)Random.Range(0, System.Enum.GetNames(typeof(Item.ItemType)).Length);
        Item item = new Item();
        item.itemType = itemType;
        item.amount = Random.Range(1, 10);
        
        ItemWorld itemWorld = ItemWorld.SpawnItemWorld(transform.position, item);
        itemWorld.transform.SetParent(transform.parent);

        Destroy(gameObject);
    }

    public void Unload()
    {
        Destroy(gameObject);
    }

    public override void HealthChanged()
    {
        Debug.Log("Enemy HP: " + _Health);
    }
}
