using UnityEngine;
using System.Collections;

public class LightningBolt : MonoBehaviour
{
	
	// Use this for initialization
	void Awake ()
	{		
		Material newMat = GetComponent<LineRenderer>().material;
		newMat.SetFloat("_StartSeed",Random.value*1000);
		GetComponent<LineRenderer>().material = newMat;
		
	}
}

