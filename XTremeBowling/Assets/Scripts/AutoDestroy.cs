using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoDestroy : MonoBehaviour
{
    private void Awake()
    {
        StartCoroutine(waitToDestroy());
    }

    IEnumerator waitToDestroy()
    {
        yield return new WaitForSeconds(2.0f);
        Destroy(gameObject);
    }
}
