using Assets.Scripts.Slicing;
using System;
using UnityEngine;
using static Assets.Scripts.Actor.PlayerManager;

namespace Assets.Scripts.Actor
{
    public class InputManager : MonoBehaviour
    {
        InputMaster playerControls;

        //Movement ActionMap Variables
        public Vector2 movement;
        public float moveAmount;
        public CameraManager camManager;
        Vector2 cameraControl;

        //SliceMode Variables
        [SerializeField]
        float sliceMode;
        public SlicingManager slicingManager;
        public Vector2 rollAim;
        public Vector2 yawAim;

        public ActionMap currentActionMap = ActionMap.Movement; //Tracks current ActionMap state, default is movement

        public PlayerModelHandler playerModelHandler;

        private void Awake()
        {
            
        }

        private void Start()
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
        private void OnEnable()
        {
            if (playerControls == null)
            {
                playerControls = new InputMaster();
                #region Movement Controls
                playerControls.Movement.Movement.performed += i => movement = i.ReadValue<Vector2>();
                playerControls.Movement.Movement.canceled += i => movement = Vector2.zero;

                playerControls.Movement.Camera.performed += i => cameraControl = i.ReadValue<Vector2>();
                playerControls.Movement.Camera.canceled += i => cameraControl = Vector2.zero;
                #endregion
                #region Slice Controls
                playerControls.SliceMode.RollAim.performed += i => rollAim = i.ReadValue<Vector2>();
                playerControls.SliceMode.RollAim.canceled += i => rollAim = Vector2.zero;

                playerControls.SliceMode.YawAim.performed += i => yawAim = i.ReadValue<Vector2>();
                playerControls.SliceMode.YawAim.canceled += i => yawAim = Vector2.zero;

                playerControls.SliceMode.Slice.performed += i => Slice();
                #endregion
                #region UI controls

                #endregion
            }
            playerControls.Enable();
            SwitchMode(ActionMap.Movement);
        }

        private void Slice()
        {
            slicingManager.Slice();
        }

        private void OnDisable()
        {
            playerControls.Disable();
        }
        public void HandleAllInputs()
        {
            switch (currentActionMap)
            {
                case ActionMap.Movement:
                    HandleMovementInput();
                    return;
                case ActionMap.Slice:
                    HandleSlicemodeInput();
                    return;
                case ActionMap.UI:
                    playerControls.UI.Enable();
                    HandleUIInput();
                    return;
                default:
                    playerControls.Movement.Enable();
                    HandleMovementInput();
                    return; ;
            }
        }
        private void HandleMovementInput()
        {
            camManager.cameraFreeLook(cameraControl);
            sliceMode = playerControls.Movement.Slicemode.ReadValue<float>();
            if (sliceMode == 1)
            {
                SwitchMode(ActionMap.Slice);
                slicingManager.EnableSlice();
            }

            moveAmount = Mathf.Clamp01(Mathf.Abs(movement.x) + Mathf.Abs(movement.y));
            playerModelHandler.UpdateAnimatorValues(0, moveAmount, (int)currentActionMap);
        }
        private void HandleUIInput()
        {
            throw new NotImplementedException();
        }

        private void HandleSlicemodeInput()
        {
            sliceMode = playerControls.SliceMode.Cancel.ReadValue<float>();
            if (sliceMode == 0)
            {
                SwitchMode(ActionMap.Movement);
                slicingManager.DisableSlice();
            }
        }

        public void SwitchMode(ActionMap target)
        {
            switch (target)
            {
                case ActionMap.Movement:
                    playerControls.SliceMode.Disable();
                    playerControls.Movement.Enable();
                    currentActionMap = ActionMap.Movement;
                    break;
                case ActionMap.Slice:
                    playerControls.Movement.Disable();
                    playerControls.SliceMode.Enable();
                    camManager.SetCameraState(target); //Snaps camera to current facing position
                    currentActionMap = ActionMap.Slice;
                    break;
                case ActionMap.UI:
                    playerControls.Movement.Disable();
                    playerControls.SliceMode.Disable();
                    playerControls.UI.Enable();
                    currentActionMap = ActionMap.UI;
                    break;
                default:
                    playerControls.UI.Disable();
                    playerControls.SliceMode.Disable();
                    playerControls.Movement.Enable();
                    currentActionMap = ActionMap.Movement;
                    break;
            }
            camManager.SetCameraState(target);
        }


    }
}