using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyByTime : MonoBehaviour
{ 
    public float timeToDestroy;

	void Start()
    {
        Destroy(this.gameObject, timeToDestroy);	
	} 
}
