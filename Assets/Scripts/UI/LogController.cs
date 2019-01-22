using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LogController : MonoBehaviour {

	// Update is called once per frame
	void Update () {
        int childCount = transform.childCount;
        if (childCount > 10)
        {
            for (int i = 0; i < childCount; i++)
            {
                if(i > 9){
                    Transform child = transform.GetChild(i);
                    Destroy(child.gameObject);
                }
            }

        }

    }
}
