using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MasterController : MonoBehaviour {
	public GameObject[] enemyTypes;
	public Text KilledText;
	public Text ButtonText;
	public static List<GameObject> enemies = new List<GameObject>();
	public GameObject Player;
	public Image spaceIsThePlace;
	private Color startCol;
	private DamageHealthController PlayerDHC;
	int deathCount;

	private void Start() {
		startCol = spaceIsThePlace.color;
		PlayerDHC = Player.GetComponentInChildren<DamageHealthController>();
	}

	void Update () {
		if (enemies.Count > 0){
			Color col = spaceIsThePlace.color;
			col = Color.Lerp(col, Color.clear, Time.deltaTime * 3);
			spaceIsThePlace.color = col;

			for (int i = 0; i < enemies.Count; i++){
				DamageHealthController DHC = enemies[i].GetComponent<DamageHealthController>();
				if (!DHC.alive) Death(i, true);
			}
		} else {
			Color col = spaceIsThePlace.color;
			col = Color.Lerp(col, startCol, Time.deltaTime * 3);
			spaceIsThePlace.color = col;
		}

		if (!Player.GetComponentInChildren<DamageHealthController>().alive) {
			ButtonText.fontSize = 11;
			ButtonText.text = "Try Again?";
		}
	}

	public void Spawn(){
		if (!PlayerDHC.alive) {
			Time.timeScale = 1;
			
			for (int i = 0; i < enemies.Count; i++) DestroyImmediate(enemies[i]);
			enemies.Clear();

			PlayerDHC.health = 100;
			PlayerDHC.alive = true;

			ButtonText.fontSize = 15;
			ButtonText.text = "Spawn";
		} else {
			int numSpawn = Random.Range(30, 40);
			for (int i = 0; i < numSpawn; i++){
				Vector3 randLocation = new Vector3(Random.Range(-100f, 100f), Random.Range(-60f, 60f), 1);
				GameObject e = (GameObject) Instantiate(enemyTypes[Random.Range(0, enemyTypes.Length)], randLocation, transform.rotation);
				
				e.transform.SetParent(transform.FindChild("Enemies").transform);
				e.name = enemies.Count.ToString();
				e.GetComponent<EnemyController>().SetTarget(Player);
				enemies.Add(e);
			}
		}
	}

	void Death(int e, bool countDeath){
		GameObject en = enemies[e];
		enemies.RemoveAt(e);
		DestroyImmediate(en);

		if (IsEmpty()) Player.GetComponent<PlayerController>().ToggleFiring();

		if (countDeath) deathCount++;
		KilledText.text = string.Format("Enemies Killed: {0}", deathCount);
	}

	public static bool IsEmpty(){
		return enemies.Count == 0;
	}
}
