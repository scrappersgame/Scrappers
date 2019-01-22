﻿using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerStatus : MonoBehaviour {
    
    [SerializeField]
    private RectTransform healthbarRect;
    [SerializeField]
    private Text healthbarText;
    [SerializeField]
    private TextMeshProUGUI scrapAmount;
    [SerializeField]
    private Gradient healthGradient;
    [SerializeField]
    private Canvas LogCanvas;
    [SerializeField]
    private TextMeshProUGUI TextPrefab;
    [SerializeField]
    private bool barResizes;

    private bool facingRight = true;

    void Start () {
        if (healthbarRect == null){
            Debug.LogError("no health bar");
        }   
        if (healthbarText == null){
            Debug.LogError("no health text");
        }   
	}
	
    public void SetHealth(int _cur, int _max){
        float _value = (float)_cur / _max;
        healthbarRect.GetComponent<Image>().color = healthGradient.Evaluate(_value);
        if(barResizes)
            healthbarRect.localScale = new Vector3(_value, healthbarRect.localScale.y, healthbarRect.localScale.z);

        healthbarText.text = _cur + "/" + _max + " HP";
    }
    public void SetScrap(int _cur, int _max)
    {
        if(scrapAmount != null)
           scrapAmount.SetText(_cur + "/" + _max);
    }
    public void LogText(string _text)
    {
        if (LogCanvas != null){
            TextMeshProUGUI logLine = Instantiate(TextPrefab) as TextMeshProUGUI;
            logLine.transform.SetParent(LogCanvas.transform, false);
            logLine.text = _text;
        }
    }
    private void Update()
    {
        if (barResizes)
        {
            if (transform.parent.localScale.x > 0 && !facingRight)
            {
                // ... flip the enemy.
                Flip();
            }
            // Otherwise if the input is moving the enemy left and the enemy is facing right...
            else if (transform.parent.localScale.x < 0 && facingRight)
            {
                // ... flip the enemy.
                Flip();
            }
        }
    }
    private void Flip()
    {
        // Switch the way the enemy is labelled as facing.
        facingRight = !facingRight;

        // Multiply the enemy's x local scale by -1.
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }
}