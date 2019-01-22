using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InsideOutside : MonoBehaviour {
    
    public Sprite insideSprite;
    public Sprite outsideSprite;
    private SpriteRenderer _spriteRenderer;

    private void Awake()
    {
        _spriteRenderer = transform.gameObject.GetComponent<SpriteRenderer>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Player _player = collision.GetComponent<Player>();
        if (_player != null)
        {
            _spriteRenderer.sprite = insideSprite;
            _spriteRenderer.sortingLayerName = "Background";
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        Player _player = collision.GetComponent<Player>();
        if (_player != null)
        {
            _spriteRenderer.sprite = outsideSprite;
            _spriteRenderer.sortingLayerName = "Foreground";
        }
    }
}
