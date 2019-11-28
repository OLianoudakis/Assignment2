using UnityEngine;
using System.Collections;

public class LightningBolt : MonoBehaviour
{
    public Transform hook1 = null;
    public Transform hook2 = null;
    public NeedToSet choice = NeedToSet.no;
	
	// Use this for initialization
	void Awake ()
	{		
		Material newMat = GetComponent<LineRenderer>().material;
		newMat.SetFloat("_StartSeed",Random.value*1000);
		GetComponent<LineRenderer>().material = newMat;

        if (choice == NeedToSet.yes)
        {
            GetComponent<LineRenderer>().SetPosition(0, hook1.position);
            GetComponent<LineRenderer>().SetPosition(1, hook2.position);
        }
		
	}

    public enum NeedToSet
    {
        yes,
        no
    }
}

