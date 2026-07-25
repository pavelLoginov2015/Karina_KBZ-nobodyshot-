using UnityEngine;

public class ByClickAnimator : MonoBehaviour
{
    [SerializeField] Animation anim;
    [SerializeField] string[] animations;

    public void CamPosOnToggle(bool state)
    {
        anim.Rewind();
        if (state)
            anim.Play(animations[0]);
        else
            anim.Play(animations[1]);
    }
}