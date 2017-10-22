using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour {
	public GameObject Sprite;
	public int turnSpeed;
	public Image healthBar;
	private Text healthAmount;
	private bool firing = true;
	private float speedMod = 0;
	private ParticleSystem[] PS;
	private DamageHealthController DHC;
	private Vector3 Target;
	private Color startColor;

	void Start(){
		PS = GetComponentsInChildren<ParticleSystem>();
		DHC = GetComponentInChildren<DamageHealthController>();
		healthAmount = healthBar.GetComponentInChildren<Text>();
		startColor = healthBar.color;

		ToggleFiring();
	}

	void Update () {
		float leftRight = Input.GetAxis("Horizontal");
		float upDown = Input.GetAxis("Vertical");

		if (DHC.alive){
			if (!MasterController.IsEmpty()) {
				MovePlayer(leftRight, upDown);
				if (Input.GetKeyDown(KeyCode.Space)) ToggleFiring();
			}

			healthBar.fillAmount = DHC.health/100f;
			healthBar.color = Color.Lerp(startColor, Color.red, (100 - DHC.health)/90f);
			healthAmount.text = DHC.health.ToString() + "%";
		} else Time.timeScale = 0;
	}

	void MovePlayer(float leftRight, float upDown){
		if (firing) speedMod = 2f;
		else speedMod = 1.5f;
		transform.Translate(new Vector2(leftRight, upDown) / speedMod);

		Vector3 pos = Camera.main.WorldToViewportPoint(transform.position);
		pos.x = Mathf.Clamp01(pos.x);
		pos.y = Mathf.Clamp01(pos.y);
		transform.position = Camera.main.ViewportToWorldPoint(pos);

		GetClosestBoy();
		Vector3 diff = Target - transform.position;
        diff.Normalize();
		float rot_z = Mathf.Atan2(diff.y, diff.x) * Mathf.Rad2Deg;
		Sprite.transform.rotation = Quaternion.Lerp(Sprite.transform.rotation, Quaternion.Euler(0f, 0f, rot_z - 270), Time.deltaTime * turnSpeed);
	}

	void GetClosestBoy(){
		if (MasterController.enemies.Count > 0){
			Vector3 closestBoy = MasterController.enemies[0].transform.position;
			for (int boy = 0; boy < MasterController.enemies.Count; boy++){
				float closestBoysDistance = Vector3.Distance(closestBoy, transform.position);
				float thisBoysDistance = Vector3.Distance(MasterController.enemies[boy].transform.position, transform.position);
				if (thisBoysDistance < closestBoysDistance) closestBoy = MasterController.enemies[boy].transform.position;
			}
			Target = closestBoy;
		}
		else {
			Target = transform.position + Vector3.up;
		}
	}

	public void ToggleFiring(){
		firing = !firing;
		foreach (ParticleSystem ps in PS) {
			var em = ps.emission;
			em.enabled = firing && !MasterController.IsEmpty();
		}
	}
}
