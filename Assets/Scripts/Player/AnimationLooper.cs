using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Animation))]
public class AnimationLooper : MonoBehaviour
{
    private Animation _animation;
    private AnimationClip clip;

    private void Start()
    {
        _animation = GetComponent<Animation>();
        clip  = _animation.clip;

        StartCoroutine(PlayAllThenLoopLast());
    }

    private IEnumerator PlayAllThenLoopLast()
    {
            _animation.Play("p1");

            // 대기: 현재 클립 길이만큼 재생
            yield return new WaitForSeconds(clip.length);

            _animation.Play("p2");
    }
}
