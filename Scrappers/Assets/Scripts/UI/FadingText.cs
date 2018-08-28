using UnityEngine;
using TMPro;

public class FadingText : MonoBehaviour {

    private TextMeshProUGUI thisText;
    private float startTime;

    private void Awake()
    {
        thisText = this.gameObject.GetComponent<TextMeshProUGUI>();
        startTime = Time.time;
    }

    // Update is called once per frame
    void Update () {
        if (Time.time > startTime + 3f)
        {
            if (thisText.color.a > 0)
            {
                thisText.color = new Color(thisText.color.r, thisText.color.g, thisText.color.b, thisText.color.a - .01f);
            }
            else
            {
                Destroy(this.gameObject);
            }
        }
	}
}
