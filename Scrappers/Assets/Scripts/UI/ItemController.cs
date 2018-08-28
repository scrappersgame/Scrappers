using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ItemController : MonoBehaviour {
    public GameObject Item1;
    public GameObject Item2;
    public GameObject Item3;
    public Image[] slots;
    private Player player;
	void Update () {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            EquipItem1();
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            EquipItem2();
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            EquipItem3();
        }
	}
    public void AddItem(GameObject _item){
        float _sprWidth = _item.GetComponent<SpriteRenderer>().sprite.bounds.size.x;
        float _sprHeight = _item.GetComponent<SpriteRenderer>().sprite.bounds.size.y;
        float _sprAspectRatio = _sprWidth / _sprHeight;
        if (Item1 == null)
        {
            Item1 = _item;
            slots[0].sprite = _item.GetComponent<SpriteRenderer>().sprite;
            slots[0].gameObject.GetComponent<AspectRatioFitter>().aspectRatio = _sprAspectRatio;
            slots[0].color = new Color(slots[0].color.r, slots[0].color.g, slots[0].color.b, 1);
            EquipItem1();
        } else if (Item2 == null)
        {
            Item2 = _item;
            slots[1].sprite = _item.GetComponent<SpriteRenderer>().sprite;
            slots[1].gameObject.GetComponent<AspectRatioFitter>().aspectRatio = _sprAspectRatio;
            slots[1].color = new Color(slots[1].color.r, slots[1].color.g, slots[1].color.b, 1);
        } else if (Item3 == null)
        {
            Item3 = _item;
            slots[2].sprite = _item.GetComponent<SpriteRenderer>().sprite;
            slots[2].gameObject.GetComponent<AspectRatioFitter>().aspectRatio = _sprAspectRatio;
            slots[2].color = new Color(slots[1].color.r, slots[1].color.g, slots[1].color.b, 1);
        }

    }
    public void RemoveItem(int _slot)
    {
        if (_slot == 1)
        {
            Item1 = null;
            slots[0].color = new Color(slots[0].color.r, slots[0].color.g, slots[0].color.b, 0);
        }
        if (_slot == 2)
        {
            Item2 = null;
            slots[1].color = new Color(slots[1].color.r, slots[1].color.g, slots[1].color.b, 0);
        }
        if (_slot == 3)
        {
            Item3 = null;
            slots[2].color = new Color(slots[2].color.r, slots[2].color.g, slots[2].color.b, 0);
        }
    }
    public void EquipItem1()
    {
        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        }
        if (Item1 != null)
            player.SwitchItems(Item1);
    }
    public void EquipItem2()
    {
        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        }
        if (Item2 != null)
            player.SwitchItems(Item2);
    }
    public void EquipItem3()
    {
        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        }
        if (Item3 != null)
           player.SwitchItems(Item3);
    }
}
