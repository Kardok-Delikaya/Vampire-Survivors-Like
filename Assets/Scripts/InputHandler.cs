using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VSLike
{
    public class InputHandler : MonoBehaviour
    {
        public float horizontal;
        public float vertical;
        public float moveAmount;

        GameManager gameManager;
        InputSystem inputSystem;
        Vector2 movementInput;

        void Awake()
        {
            gameManager = FindObjectOfType<GameManager>();
        }

        public void OnEnable()
        {
            if (inputSystem == null)
            {
                inputSystem = new InputSystem();
                inputSystem.Character2D.Movement.performed += genelInput => movementInput = genelInput.ReadValue<Vector2>();

                inputSystem.UI.Escape.performed += i => gameManager.Menu();
            }
            inputSystem.Enable();
        }
        private void OnDisable()
        {
            inputSystem.Disable();
        }
        public void TickInput(float delta)
        {
            HareketInput(delta);
        }
        private void HareketInput(float delta)
        {
            horizontal = movementInput.x;
            vertical = movementInput.y;
            moveAmount = Mathf.Clamp01(Mathf.Abs(horizontal) + Mathf.Abs(vertical));
        }
    }
}