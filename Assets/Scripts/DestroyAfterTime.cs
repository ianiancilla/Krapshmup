using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyAfterTime : MonoBehaviour
{
    [SerializeField]
    [Tooltip("Time after which object will self-destroy.")]
    float lifeSpan = 300;

    private void Update()
    {
        CountDownAndDestroy();
    }

    private void CountDownAndDestroy()
    {
        lifeSpan -= Time.deltaTime;
        if (lifeSpan <= 0f)
        {
            Destroy(gameObject);
        }
    }
}
