using System.Collections;
using System.Collections.Generic;
using UnityEngine;

interface ISpaceshipControls
{
    public void Move(Vector3 direction, float force);
    public void Move(float force);
    public void Rotate(Vector3 direction, float angle);
    public void Roll(float force);
    public void Boost();
}