using UnityEngine;

namespace BWV.Interface
{
    public interface ICharacterInput
    {
        Vector2 GetMoveInput();
        float GetRunMultiplier();
        Vector2 GetViewInput();
        float GetFlyDirection();
        bool GetFire();
    }
}