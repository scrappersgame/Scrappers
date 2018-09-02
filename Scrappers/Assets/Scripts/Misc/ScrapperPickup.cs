using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrapperPickup : MonoBehaviour {

    public StatusIndicator ScrapStatus;
    public int maxScrap;

    private void Awake()
    {
        ScrapStatus.SetHealth(this.gameObject.GetComponent<Pickup>().ScrapValue, maxScrap);

    }

}
