using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections.Generic;

public class Destructible : MonoBehaviour {
    public AudioClip hitSound;
    public AudioClip destroySound;
    public Transform destroyParticals;
    public int minimumDrops = 0;
    public Transform[] itemDrops;
    Dictionary<Vector3Int, int> tileDict = new Dictionary<Vector3Int, int>();
    private int maxHealth = 50;
    private Tilemap tileMap;
    public void Awake()
    {
        tileMap = this.gameObject.GetComponent<Tilemap>();
    }

    public void DamageTile(int damage, Vector2 hitPoint)
    {
        Vector3 hitPosition = new Vector3(hitPoint.x, hitPoint.y, 0);
        Vector3Int cellPosition = tileMap.WorldToCell(hitPosition);
        int tileHealth = 0;
        if (tileDict.TryGetValue(cellPosition, out tileHealth))
        {
            tileHealth -= damage;
            tileDict[cellPosition] = tileHealth;
        }
        else
        {
            tileHealth = maxHealth - damage;
            tileDict.Add(cellPosition, tileHealth);
        }
        if (tileHealth <= 0)
        {
            DestroyTile(cellPosition, hitPosition);
        }
    }
    private void DestroyTile(Vector3Int cellPosition, Vector3 hitPosition)
    {
        tileMap.SetTile(cellPosition, null);
        Vector3 tilePosition = tileMap.CellToWorld(cellPosition);
        if (itemDrops.Length > 0)
        {
            int _maxDrops = itemDrops.Length;
            int _numberDrops = Random.Range(minimumDrops, _maxDrops);
            for (int i = 0; i < _numberDrops; i++)
            {
                Transform _droppedItem = Instantiate(itemDrops[Random.Range(0, _maxDrops)], tilePosition, transform.rotation);
                Vector3 _rotDirection = Vector3.left;
                if (Random.Range(0, 2) == 1)
                    _rotDirection = Vector3.right;
                _droppedItem.Rotate(_rotDirection * Random.Range(1, 10));
                Vector3 _moveDirection = (tilePosition - hitPosition).normalized * Random.Range(.1f, .7f);
                _droppedItem.gameObject.GetComponent<Rigidbody2D>().AddForce(_moveDirection, ForceMode2D.Impulse);
                _droppedItem.gameObject.GetComponent<Rigidbody2D>().AddTorque(.7f);
            }
        }
        if (destroyParticals != null)
        {
            Transform particles = Instantiate(destroyParticals, tilePosition, transform.rotation);
            Destroy(particles.gameObject, 2f);
        }
        if (destroySound != null)
        {
            float masterVolume = GameMaster.gm.masterVolume;
            AudioSource.PlayClipAtPoint(destroySound, tilePosition, masterVolume);
        }
    }
}
