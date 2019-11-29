using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*=====================================================
 =====THIS CLASS IS FOR THE PLAYER EFFECTS OBJECT======
 =====IT FOLLOWS THE PLAYER AND ADJUST ITS ROTATION====
 =====BASED ON PLAYER MOVEMENT INTENT. ALSO IT SPANWS==
 =====PLAYER EFFECTS LIKE BOOSTERS AND PARTICLES=======
 ======================================================*/

public class AdjustPosition : MonoBehaviour
{
    public GameObject nitroEngine;
    public GameObject jumpExplosion;

    public Transform targetObject;
    public GameObject leftEnginePos;
    public GameObject rightEnginePos;
    public Transform jumpEffectPos;

    private Vector3 newIntent = Vector3.zero;
    private Vector3 oldIntent = Vector3.zero;

    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector3(targetObject.position.x, targetObject.position.y, targetObject.position.z);
        //transform.rotation = new Quaternion(0.0f, 0.0f, 0.0f, targetObject.transform.rotation.w);
        Vector3 intent = Vector3.zero;
        if (newIntent != oldIntent)
        {
            intent = Vector3.Lerp(oldIntent, newIntent, 40.0f * Time.deltaTime);
        }
        else
        {
            intent = oldIntent;
        }
        transform.rotation = Quaternion.LookRotation(intent);
        oldIntent = newIntent;        
    }

    public void SetRotation (Vector3 intent)
    {
        newIntent = intent;
    }

    public void PlayNitroEffect()
    {
        leftEnginePos.SetActive(true);
        leftEnginePos.GetComponent<Scaler>().ActivateNitroDeactivation();
        rightEnginePos.SetActive(true);
        rightEnginePos.GetComponent<Scaler>().ActivateNitroDeactivation();
    }

    public void PlayJumpEffect()
    {
        Instantiate(jumpExplosion, jumpEffectPos.position, Quaternion.identity);
    }
}
