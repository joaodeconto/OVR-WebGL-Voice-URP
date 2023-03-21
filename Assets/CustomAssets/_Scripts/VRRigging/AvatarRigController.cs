using UnityEngine;

[System.Serializable]
public class MapTransforms
{
    public Transform vrTarget;
    public Transform ikTarget;

    public Vector3 trackingPositionOffset;
    public Vector3 trackingRotationOffset;

    public void VRMaping()
    {
        ikTarget.position = vrTarget.TransformPoint(trackingPositionOffset);
        ikTarget.rotation = vrTarget.rotation * Quaternion.Euler(trackingRotationOffset);
    }
}
public class AvatarRigController : MonoBehaviour
{
    [SerializeField] private MapTransforms head;
    [SerializeField] private MapTransforms rightHand;
    [SerializeField] private MapTransforms leftHand;

    [SerializeField, Range(0.0f, 1.0f)] private float turnSmoothness;
    [SerializeField] private Transform ikHead;
    [SerializeField] private Vector3 headBodyOffset;


    private void LateUpdate()
    {
        transform.position = ikHead.position + headBodyOffset;
        transform.forward = Vector3.Lerp(transform.forward, Vector3.ProjectOnPlane(ikHead.forward, Vector3.up).normalized, Time.deltaTime * turnSmoothness);

        head.VRMaping();
        leftHand.VRMaping();
        rightHand.VRMaping();
    }


}