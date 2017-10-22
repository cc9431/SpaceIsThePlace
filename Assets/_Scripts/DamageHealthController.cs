using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageHealthController : MonoBehaviour {
	public int health;
	public int damage;
	public bool alive;
	public bool outOfBounds;
	private Transform bounds;
	private int startHealth;

	private void Awake() {
		alive = true;
		outOfBounds = false;
		bounds = Camera.main.GetComponent<CameraController>().BG;
		startHealth = health;
	}

	private void Update() {
		if (alive) {
			health = outOfBounds ? startHealth : health;
			alive = health > 0;
			outOfBounds = Mathf.Abs(transform.localPosition.y) > bounds.localScale.y/1.75 || Mathf.Abs(transform.localPosition.x) > bounds.localScale.x/1.75;
		}
	}

	public void Damage(int damage){
		health -= damage;
	}
}
