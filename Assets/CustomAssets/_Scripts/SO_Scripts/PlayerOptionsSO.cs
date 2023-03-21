using UnityEngine;

[CreateAssetMenu(fileName = "PlayerOptions", menuName = "ScriptableObjects/PlayerOptions", order = 1)]
public class PlayerOptionsSO : ScriptableObject
{
    [Header("Movement")]
    public bool movePlayer = true;
    public bool moveX = true;
    public bool moveY = true;
    public float speed = 2.0f;
    public float speedRunningMultiplier = 2f;

    [Header("Fly")]
    public bool gravityOn = true;
    public bool flyPlayer = true;
    public bool flyForward = true;
    public float flySpeed = 2.0f;

    [Header("Sensitivity")]
    public bool moveViewX = true;
    public bool moveViewY = true;
    public float lookSensitivity = .5f;
    public float smoothLookFactor = 10f;

    [Header("View Rotation Clamp")]
    public float xMax = 80f;
    public float xMin = -70f;

    public bool FlyForward
    {
        get { return flyForward; }
        set
        {
            flyForward = value;
            if (flyPlayer || flyForward)
                GravityOn = false;
            else
                GravityOn = true;
        }
    }

    public bool Levitate
    {
        get { return flyPlayer; }
        set
        {
            flyPlayer = value;
            if (flyPlayer || flyForward)
                GravityOn = false;
            else
                GravityOn = true;
        }
    }

    public bool GravityOn
    {
        get { return gravityOn; }
        set
        {
            gravityOn = value;
            if (gravityOn)
            {
                flyForward = false;
                flyPlayer = false;
            }
        }
    }
}