using System;
using UnityEngine;

namespace SerialCommunication
{
    
    
    // Find a better name
    public class MainThreadObj : MonoBehaviour
    {
        private float velocity = float.MinValue;
        [SerializeField] private SpaceshipControls sc;
        
        private void Start()
        {
            
        }

        private void Update()
        {
            if (velocity != float.MinValue)
            {
                sc.Move(((velocity / 255f) * sc.getMaximumVelocity()) - sc.getMaximumVelocity()/2);
            }
        }

        public float Velocity
        {
            get => velocity;
            set => velocity = value;
        }
    }
}