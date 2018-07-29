using UnityEngine;
using System.Collections;
 
[System.Serializable] 
public class Character {
 
    public string name;
    public int health;
    public int maxHealth;
    public int totalScrap;
 
    public Character () {
        this.name = "Name";
        this.health = 100;
        this.maxHealth = 100;
        this.totalScrap = 0;
    }
}