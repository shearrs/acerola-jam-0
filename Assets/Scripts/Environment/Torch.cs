using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Torch : MonoBehaviour
{
    [SerializeField] private Light torchLight;
    [SerializeField] private float flickerTime;
    [SerializeField] private float lowRange = 8;
    [SerializeField] private float highRange = 10;

    private void Awake()
    {
        StartCoroutine(IEFlicker());
    }

    private IEnumerator IEFlicker()
    {
        while(true)
        {
            float elapsedTime = 0;
            float range = Random.Range(lowRange, 10);
            float startingRange = torchLight.range;

            while(elapsedTime < flickerTime)
            {
                float percent = elapsedTime / flickerTime;

                torchLight.range = Mathf.Lerp(startingRange, range, percent);

                elapsedTime += Time.deltaTime;

                yield return null;
            }

            yield return null;
        }
    }
}
