using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpGrade : MonoBehaviour {

    public Sprite[] upgradeSprs;
    public string upgradeName = "";

    private void Awake()
    {
        Sprite icon = upgradeSprs[Random.Range(0, upgradeSprs.Length)];
        GetComponent<SpriteRenderer>().sprite = icon;
        upgradeName = icon.name.ToString();
    }
	
	// Update is called once per frame
	void Update () {
        Destroy(gameObject,1.0f);
	}
}
