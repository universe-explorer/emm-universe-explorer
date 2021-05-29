using System.Collections;
using System.Collections.Generic;
using UnityEngine;

interface ISpaceshipControls
{
    public void Move(Vector3 direction, float force);
    public void Move(float force);
    public void Rotate(Vector2 mouseInput);
    public void Roll(float force);
    public void Boost();
}