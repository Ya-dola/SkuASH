using System.Threading.Tasks;
using UnityEngine;

public class PlayerAnimations : MonoBehaviour
{
    [Header("Animation Curves")]
    [Header("Position Curves")]
    [SerializeField]
    private AnimationCurve posHitCurve;

    [SerializeField]
    private float posHitCurveTan;

    [Header("Rotation Curves")]
    [SerializeField]
    private AnimationCurve rotHitCurve;

    [SerializeField]
    private Animation vehicleAnim;

    private float _hitTime;

    private void HitBallAnimation(Transform ballTrans, Vector3 collisionPos, Vector3 collisionNormal)
    {
        // print("Player Anim | Ball Hit Trans: " + ballTrans.position + " local: " + ballTrans.localPosition);

        AdjustPosHitCurve(ballTrans.localPosition.y);

        PlayHitAnimClip();

        HitBall(collisionPos, collisionNormal);
    }

    private void AdjustPosHitCurve(float height)
    {
        // Get the Time Pos of the Max Height Key - AKA Time Vehicle Hits Ball
        _hitTime = posHitCurve.keys[1].time;

        // Replace Max Height Key with actual position 
        posHitCurve.RemoveKey(1);
        posHitCurve.AddKey(_hitTime, height);

        // Smoothening the Newly Added Key
        posHitCurve.SmoothTangents(1, posHitCurveTan);
    }

    private void PlayHitAnimClip()
    {
        var clip = new AnimationClip
        {
            legacy = true,
            name = "vehicleHitClip"
        };

        clip.SetCurve("", typeof(Transform), "localPosition.y", posHitCurve);
        // clip.SetCurve("", typeof(Transform), "localRotation.z", rotHitCurve);

        vehicleAnim.AddClip(clip, clip.name);
        vehicleAnim.Play(clip.name);
    }

    private async void HitBall(Vector3 collisionPos, Vector3 collisionNormal)
    {
        // Delays HitBall Event by the Hit time in Milliseconds
        await Task.Delay((int)_hitTime * 1000);

        GameEvtMan.Instance.EvtVehicleHitBall(collisionPos, collisionNormal);
    }

    private void OnEnable()
    {
        GameEvtMan.Instance.ActVehicleHitBallAnim += HitBallAnimation;
    }

    private void OnDisable()
    {
        GameEvtMan.Instance.ActVehicleHitBallAnim -= HitBallAnimation;
    }
}