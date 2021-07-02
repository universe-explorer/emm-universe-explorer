using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatControllerEnemy : AbstractCombatController
{
    public override void Die()
    {
        //Spawns a random item on death
        Item.ItemType itemType = (Item.ItemType)Random.Range(0, System.Enum.GetNames(typeof(Item.ItemType)).Length);
        Item item = new Item();
        item.itemType = itemType;
        item.amount = Random.Range(1, 10);
        item.healthPortion = Random.Range(5, 16);
        item.mineralPortion = Random.Range(1, 6);
        item.manaPortion = Random.Range(5, 36);
        item.medkitPortion = Random.Range(1, 3);
        ItemWorld itemWorld = ItemWorld.SpawnItemWorld(transform.position, item);
        itemWorld.transform.SetParent(transform.parent);

        Destroy(gameObject);
    }

    public override void HealthChanged()
    {
        Debug.Log("Enemy HP: " + _Health);
    }
}
