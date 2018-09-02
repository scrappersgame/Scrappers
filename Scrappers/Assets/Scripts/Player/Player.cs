using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

    public GameObject pistolPrefab;
    public StatusIndicator ScrapStatus;

    private PlayerStatus playerStatus;
    private ItemController itemController;
    private Transform gunHand;
    private GameObject currentItem;

    void Awake()
    {
        playerStatus = GameObject.FindGameObjectWithTag("HealthBar").GetComponent<PlayerStatus>();
        itemController = GameObject.FindGameObjectWithTag("HotBar").GetComponent<ItemController>();
    }
    private void Start()
    {
        PlayerMaster.stats.Init();
        if (playerStatus != null)
        {
            playerStatus.SetHealth(PlayerMaster.stats.currentHealth, PlayerMaster.stats.maxHealth);
        }
        if (ScrapStatus != null)
        {
            ScrapStatus.SetHealth(Mathf.Clamp(PlayerMaster.stats.currentScrap, 0, PlayerMaster.stats.maxScrap), PlayerMaster.stats.maxScrap);
        }
    }
    void Update () {
		if (transform.position.y <= -10f){
            DamagePlayer (PlayerMaster.stats.currentHealth);
        }
    }
	public void DamagePlayer (int damage) {
        PlayerMaster.stats.currentHealth -= damage;
        if (PlayerMaster.stats.currentHealth <= 0){
			GameMaster.KillPlayer(this);
		}
        if (playerStatus != null)
        {
            playerStatus.SetHealth(PlayerMaster.stats.currentHealth, PlayerMaster.stats.maxHealth);
        }
	}
    public void SwitchItems (GameObject _item){
        if (gunHand == null)
            gunHand = GameObject.FindGameObjectWithTag("GunHand").transform;
        if (currentItem != null)
            Destroy(currentItem);
        GameObject _newItem = Instantiate(_item, gunHand.position, gunHand.rotation);
        _newItem.transform.SetParent(gunHand.parent);
        _newItem.transform.localScale = _newItem.transform.parent.localScale;
        currentItem = _newItem;
        
    }
    public void RemoveScrap(int value)
    {
        PlayerMaster.stats.currentScrap -= value;
        playerStatus.SetScrap(PlayerMaster.stats.currentScrap, PlayerMaster.stats.maxScrap);
        if(PlayerMaster.stats.currentScrap > 0)
            ScrapStatus.SetHealth(PlayerMaster.stats.currentScrap, PlayerMaster.stats.maxScrap);
    }
    public void AddScrap(int value)
    {
        PlayerMaster.stats.currentScrap += value;
        if (PlayerMaster.stats.currentScrap > PlayerMaster.stats.maxScrap)
        {
            int scrapWasted = PlayerMaster.stats.currentScrap - PlayerMaster.stats.maxScrap;
            playerStatus.LogText("Scrapper full, " + scrapWasted + " scrap lost.");
            PlayerMaster.stats.currentScrap = PlayerMaster.stats.maxScrap;
        }
        if (PlayerMaster.stats.currentScrap > 0)
            ScrapStatus.SetHealth(PlayerMaster.stats.currentScrap, PlayerMaster.stats.maxScrap);
        playerStatus.SetScrap(PlayerMaster.stats.currentScrap, PlayerMaster.stats.maxScrap);
        if (PlayerMaster.stats.currentScrap >= 50 && !StoryMaster.sm.gunGiven){
            GameMaster.gm.SpawnItem(pistolPrefab, 1);
            StoryMaster.sm.GiveGun();
            RemoveScrap(50);
        }
    }
    public void AddItem(GameObject _item, GameObject _pickup){
        playerStatus.LogText(_item.name + " blueprint added");
        itemController.AddItem(_item, _pickup);
    }
}
