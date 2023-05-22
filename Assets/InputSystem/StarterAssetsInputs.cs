using UnityEngine;
#if ENABLE_INPUT_SYSTEM && STARTER_ASSETS_PACKAGES_CHECKED
using UnityEngine.InputSystem;
#endif

namespace StarterAssets
{
	public class StarterAssetsInputs : MonoBehaviour
	{
		[Header("Character Input Values")]
		public Vector2 move;
		public Vector2 look;
		public bool jump;
		public bool sprint;
		public bool ability1;
		public bool ability2;
		public bool ability3;
		public bool ability4;
		public bool interaction = false;

		[Header("Movement Settings")]
		public bool analogMovement;

		[Header("Mouse Cursor Settings")]
		public bool cursorLocked = true;
		public bool cursorInputForLook = true;

#if ENABLE_INPUT_SYSTEM && STARTER_ASSETS_PACKAGES_CHECKED
		public void OnMove(InputValue value)
		{
			MoveInput(value.Get<Vector2>());
		}

		public void OnLook(InputValue value)
		{
			if(cursorInputForLook)
			{
				LookInput(value.Get<Vector2>());
			}
		}

		public void OnJump(InputValue value)
		{
			JumpInput(value.isPressed);
		}

		public void OnSprint(InputValue value)
		{
			SprintInput(value.isPressed);
		}

		public void OnAbility1(InputValue value)
		{
			AbilityInput1(value.isPressed);
		}
		public void OnAbility2(InputValue value)
		{
			AbilityInput2(value.isPressed);
		}
		public void OnAbility3(InputValue value)
		{
			AbilityInput3(value.isPressed);
		}
		public void OnAbility4(InputValue value)
		{
			AbilityInput4(value.isPressed);
		}
		public void OnInteraction(InputValue value)
		{
			InteractionInput(value.isPressed);
		}


#endif

		
		public void MoveInput(Vector2 newMoveDirection)
		{
			move = newMoveDirection;
		} 

		public void LookInput(Vector2 newLookDirection)
		{
			look = newLookDirection;
		}

		public void JumpInput(bool newJumpState)
		{
			jump = newJumpState;
		}

		public void SprintInput(bool newSprintState)
		{
			sprint = newSprintState;
		}

		public void AbilityInput1(bool newAbilityState1){
			ability1 = newAbilityState1;
		}

		public void AbilityInput2(bool newAbilityState2){
			ability2 = newAbilityState2;
		}

		public void AbilityInput3(bool newAbilityState3){
			ability3 = newAbilityState3;
		}
		public void AbilityInput4(bool newAbilityState4){
			ability3 = newAbilityState4;
		}

		public void InteractionInput(bool newInteractionStateE){
			interaction = newInteractionStateE;
		}

		private void OnApplicationFocus(bool hasFocus)
		{
			SetCursorState(cursorLocked);
		}

		private void SetCursorState(bool newState)
		{
			Cursor.lockState = newState ? CursorLockMode.Locked : CursorLockMode.None;
		}
	}
	
}