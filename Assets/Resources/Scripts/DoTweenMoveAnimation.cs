using DG.Tweening;
using UnityEngine;

internal class MoveAnimationInfo
{
    internal Resource resource;
    internal Vector3 endLocalPos;
    internal Quaternion endLocalRotation;
    internal float duration;
    internal TweenCallback onCompleted;
}
internal static class DoTweenMoveAnimation
{
    internal static void Play(MoveAnimationInfo animationInfo)
    {
        Transform transform = animationInfo.resource.transform;
        Sequence sequence = DOTween.Sequence().
            Append(transform.DOLocalMove(animationInfo.endLocalPos, animationInfo.duration)).
            Append(transform.DOLocalRotate(Quaternion.identity.eulerAngles, 0)).
            OnComplete(animationInfo.onCompleted);
    }
}