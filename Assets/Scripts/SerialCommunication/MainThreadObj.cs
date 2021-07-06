using System;
using UnityEngine;

namespace SerialCommunication
{
    
    
    // Find a better name
    /// <summary>
    /// Main thread object to call methods in main thread and get values from other threads
    /// </summary>
    public class MainThreadObj : MonoBehaviour
    {
        private float velocity = float.MinValue;
        [SerializeField] private SpaceshipControls sc;
        

        private void Update()
        {
            if (velocity != float.MinValue)
            {
                sc.Move(((velocity / 255f) * sc.getMaximumVelocity()) - sc.getMaximumVelocity()/2);
            }
        }

        /// <summary>
        /// Get/Set velocity
        /// </summary>
        public float Velocity
        {
            get => velocity;
            set => velocity = value;
        }
    }
}