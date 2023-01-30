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

    private void HitBall(Transform ballTrans)
    {
        // print("Player Anim | Ball Hit Trans: " + ballTrans.position + " local: " + ballTrans.localPosition);

        AdjustPosHitCurve(ballTrans.localPosition.y);

        PlayHitClip();
    }

    private void AdjustPosHitCurve(float height)
    {
        // Get the Time Position of the Max Height Key
        var time = posHitCurve.keys[1].time;

        // Replace Max Height Key with actual position 
        posHitCurve.RemoveKey(1);
        posHitCurve.AddKey(time, height);

        posHitCurve.SmoothTangents(1, posHitCurveTan);
    }

    private void PlayHitClip()
    {
        var clip = new AnimationClip
        {
            legacy = true,
            name = "vehicleHitClip"
        };

        clip.SetCurve("", typeof(Transform), "localPosition.y", posHitCurve);

        vehicleAnim.AddClip(clip, clip.name);
        vehicleAnim.Play(clip.name);
    }


    private void OnEnable()
    {
        GameEvtMan.Instance.ActVehicleHitBall += HitBall;
    }

    private void OnDisable()
    {
        GameEvtMan.Instance.ActVehicleHitBall -= HitBall;
    }
}