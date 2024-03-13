using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SheepProp : MonoBehaviour
{
    [SerializeField] private bool stuckInPlace;
    private Animator animator;

    public void Start()
    {
        animator = GetComponent<Animator>();

        if (!stuckInPlace)
        {
            float time = Random.Range(0, 5f);
            Invoke(nameof(WalkAround), time);
        }
        else
        {
            StartCoroutine(IELookAround());
        }
    }

    private IEnumerator IELookAround()
    {
        while (true)
        {
            float time = Random.Range(0, 10f);

            yield return new WaitForSeconds(time);

            animator.Play("Look Around");

            while (animator.IsPlaying("Look Around"))
                yield return null;
        }
    }

    private void WalkAround()
    {
        animator.Play("Walk Around");
    }
}
