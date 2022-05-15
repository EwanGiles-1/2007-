using Assets.Scripts.Slicing;
using UnityEngine;

namespace Assets.Scripts.Actor
{
    public class PlayerManager : MonoBehaviour
    {
        InputManager inputManager;
        ActorLocomotion playerLocomotion;
        SlicingManager slicingManager;
        public enum ActionMap //Action states that would require different Action Maps using New Unity Input system
        {
            Movement,
            Slice,
            UI
        }
        private void Awake()
        {
            inputManager = GetComponent<InputManager>();
            playerLocomotion = GetComponent<ActorLocomotion>();
            slicingManager = GetComponent<SlicingManager>();
        }

        private void Update()
        {
            inputManager.HandleAllInputs();
        }

        private void FixedUpdate()
        {
            playerLocomotion.HandleAllMovement();
            slicingManager.HandleAllSlicing();

        }

    }
}