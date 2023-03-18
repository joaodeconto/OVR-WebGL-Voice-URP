using BWV.Interface;
using UnityEngine;

namespace BWV.Player
{
    public class CharacterAnimatorController : MonoBehaviour
    {
        public Animator animator;
        [SerializeField] private float speedMultiplier = 1f;

        private ICharacterInput characterInput;

        private void Awake()
        {
            characterInput = GetComponent<ICharacterInput>();
        }

        private void Update()
        {
            if (animator != null)
            {

                // Get movement input from character input
                Vector2 moveInput = characterInput.GetMoveInput();
                float speed = moveInput.magnitude;

                // Set animator parameters based on input values
                animator.SetFloat("Speed", speed * speedMultiplier);
                animator.SetFloat("Strafe", moveInput.x);
                animator.SetFloat("Forward", moveInput.y);
                //Debug.LogError(moveInput.y);
                //animator.SetBool("IsRunning", characterInput.IsRunning());
                //animator.SetBool("IsJumping", characterInput.IsJumping());
                //animator.SetBool("IsFalling", characterInput.IsFalling());
            }
        }
    }
}