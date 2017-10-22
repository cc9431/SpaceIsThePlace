using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleTrigger : MonoBehaviour {

	void OnParticleCollision(GameObject other) {
		int damageGiven = transform.parent.GetComponent<DamageHealthController>().damage;
		other.GetComponent<DamageHealthController>().Damage(damageGiven);
	}
}
