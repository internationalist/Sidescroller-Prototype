using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetComponent : MonoBehaviour {

	void OnTriggerEnter(Collider other)
	{
		Debug.Log("entered");
	}


}
