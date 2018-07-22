using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ItemController : MonoBehaviour
                      {
    public GameObject[] slots;
	void Update () {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            slots[0].GetComponent<Button>().onClick.Invoke();
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            slots[1].GetComponent<Button>().onClick.Invoke();
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            slots[2].GetComponent<Button>().onClick.Invoke();
        }
	}
}
