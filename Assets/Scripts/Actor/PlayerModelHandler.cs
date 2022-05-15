using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Assets.Scripts.Actor
{
    public class PlayerModelHandler : MonoBehaviour
    {
        public Animator animator;
        int horizontal;
        int vertical;
        int mode;

        private void Awake()
        {
            horizontal = Animator.StringToHash("Horizontal");
            vertical = Animator.StringToHash("Vertical");
            mode  = Animator.StringToHash("Mode");
        }

        public void UpdateAnimatorValues(float horizontalMovement, float verticalMovement, int mode)
        {
            animator.SetFloat(horizontal, horizontalMovement, 0.1f, Time.deltaTime);
            animator.SetFloat(vertical, verticalMovement, 0.1f, Time.deltaTime);
            animator.SetInteger(this.mode, mode);
        }
    }
}