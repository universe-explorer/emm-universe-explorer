using System.Collections;
using System.Collections.Generic;
using UnityEngine;

interface ISpaceshipControls
{
    public void Move(Vector3 direction, float force);
    public void Rotate(Vector3 direction, float angle);
    public void Roll(Vector3 direction);
    public void Boost(Vector3 direction);
}