using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scaler : MonoBehaviour
{
    private bool reverseScale = false;

    // Update is called once per frame
    void Update()
    {
        if ((transform.localScale.x < 1.0 || transform.localScale.y < 1.0 || transform.localScale.z < 1.0) && !reverseScale)
        {
            transform.localScale += new Vector3(0.1f, 0.1f, 0.1f);
        }

        if (reverseScale)
        {
            transform.localScale -= new Vector3(0.1f, 0.1f, 0.1f);
            if ((transform.localScale.x <= 0.0 && transform.localScale.y <= 0.0 && transform.localScale.z <= 0.0))
            {
                StopAllCoroutines();
                reverseScale = false;
                gameObject.SetActive(false);
            }
        }
    }

    public void ActivateNitroDeactivation()
    {
        StartCoroutine(deactivation());
    }

    IEnumerator deactivation()
    {
        yield return new WaitForSeconds(2.0f);
        reverseScale = true;
    }
}
