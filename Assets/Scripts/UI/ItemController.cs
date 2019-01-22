using UnityEngine;
using UnityEngine.UI;
using UnityEditor;

public class ItemController : MonoBehaviour
{
    public static ItemController ic;

    public GameObject[] items;
    public Image[] slots;
    public Object[] pickups;
    private Player player;

    private void Awake()
    {
        if (ic == null)
        {
            ic = GameObject.FindGameObjectWithTag("HotBar").GetComponent<ItemController>();
        }
    }
    private void Start()
    {
        items[0] = null;
        items[1] = null;
        items[2] = null;
    }

    void Update()
    {
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
    public void AddItem(GameObject _item, GameObject _pickup)
    {
        float _sprWidth = _item.GetComponent<SpriteRenderer>().sprite.bounds.size.x;
        float _sprHeight = _item.GetComponent<SpriteRenderer>().sprite.bounds.size.y;
        float _sprAspectRatio = _sprWidth / _sprHeight;
        if (items[0] == null)
        {
            items[0] = _item;
            slots[0].sprite = _item.GetComponent<SpriteRenderer>().sprite;
            slots[0].gameObject.GetComponent<AspectRatioFitter>().aspectRatio = _sprAspectRatio;
            slots[0].color = new Color(slots[0].color.r, slots[0].color.g, slots[0].color.b, 1);
            EquipItem1();
        }
        else if (items[1] == null)
        {
            items[1] = _item;
            slots[1].sprite = _item.GetComponent<SpriteRenderer>().sprite;
            slots[1].gameObject.GetComponent<AspectRatioFitter>().aspectRatio = _sprAspectRatio;
            slots[1].color = new Color(slots[1].color.r, slots[1].color.g, slots[1].color.b, 1);
        }
        else if (items[2] == null)
        {
            items[2] = _item;
            slots[2].sprite = _item.GetComponent<SpriteRenderer>().sprite;
            slots[2].gameObject.GetComponent<AspectRatioFitter>().aspectRatio = _sprAspectRatio;
            slots[2].color = new Color(slots[1].color.r, slots[1].color.g, slots[1].color.b, 1);
        }
    }
    public void RemoveItem(int _slot)
    {
        if (_slot == 1)
        {
            items[0] = null;
            slots[0].color = new Color(slots[0].color.r, slots[0].color.g, slots[0].color.b, 0);
        }
        if (_slot == 2)
        {
            items[1] = null;
            slots[1].color = new Color(slots[1].color.r, slots[1].color.g, slots[1].color.b, 0);
        }
        if (_slot == 3)
        {
            items[2] = null;
            slots[2].color = new Color(slots[2].color.r, slots[2].color.g, slots[2].color.b, 0);
        }
    }
    public void EquipItem1()
    {
        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        }
        else
        {
            if (items[0] != null)
                player.SwitchItems(items[0]);
        }
    }
    public void EquipItem2()
    {
        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        }
        else
        {
            if (items[1] != null)
                player.SwitchItems(items[1]);
        }
    }
    public void EquipItem3()
    {
        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        }
        else
        {
            if (items[2] != null)
                player.SwitchItems(items[2]);
        }
    }
    public void DropAllItems()
    {
        for (int i = 0; i < pickups.Length; i++)
        {
            if (pickups[i] != null)
            {
                Vector3 spawnPosition = new Vector3(player.transform.position.x, player.transform.position.y + 1, player.transform.position.z);
                Instantiate(pickups[i], spawnPosition, player.transform.rotation);
                pickups[i] = null;
            }
        }
        for (int i = 0; i < items.Length; i++)
        {
            RemoveItem(i);
        }
    }
}