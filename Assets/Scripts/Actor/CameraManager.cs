using Cinemachine;
using UnityEngine;
using static Assets.Scripts.Actor.PlayerManager;

namespace Assets.Scripts.Actor
{
    public class CameraManager : MonoBehaviour
    {
        public CinemachineFreeLook freeLookCamera;
        public CinemachineVirtualCamera sliceModeCamera;

        private Animator animator;

        private void Awake()
        {
            animator = GetComponent<Animator>();
        }
        public void cameraFreeLook(Vector2 cameraControl)
        {
            freeLookCamera.m_XAxis.m_InputAxisValue = cameraControl.x;
            freeLookCamera.m_YAxis.m_InputAxisValue = cameraControl.y;
        }
        public void cameraSliceMode()
        {

        }
        public void cameraUIMode()
        {

        }
        public void SetCameraState(ActionMap state)
        {
            switch (state)
            {
                case ActionMap.Movement:
                    animator.Play("FreeLook");
                    return;
                case ActionMap.Slice:
                    animator.Play("SliceMode");
                    return;
                case ActionMap.UI:
                    animator.Play("FreeLook");
                    return;
                default:
                    animator.Play("FreeLook");
                    return; ;
            }
        }
    }
}