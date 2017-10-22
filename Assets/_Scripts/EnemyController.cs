using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour {
	public GameObject Target;
	public bool follower;
	public int speed;
	public string StateName;
	private int currBehavior;
	private int prevBehavior = -1;
	[HideInInspector] public DamageHealthController DHC;
	public IList<Behavior> behaviors = new List<Behavior>();

	void Start(){
		behaviors.Add((Behavior) ScriptableObject.CreateInstance(typeof(Retreat)));
		behaviors.Add((Behavior) ScriptableObject.CreateInstance(typeof(Attack)));
		if (follower) behaviors.Add((Behavior) ScriptableObject.CreateInstance(typeof(Follow)));
		behaviors.Add((Behavior) ScriptableObject.CreateInstance(typeof(Search)));
		for (int behavior = 0; behavior < behaviors.Count; behavior++){
			behaviors[behavior].init(this);
		}
		DHC = GetComponent<DamageHealthController>();
	}

	void Update () {
		if (DHC.alive && Time.timeScale != 0) {
			foreach (Behavior behavior in behaviors){
				if (behavior.check()) {
					currBehavior = behaviors.IndexOf(behavior);
					StateName = behavior.GetType().Name;
					behavior.run();
					//Target = behavior.Target;
					speed = behavior.speed;
					break;
				}
			}
			if (currBehavior != prevBehavior){
				ParticleSystem[] PS = GetComponentsInChildren<ParticleSystem>();
				foreach (ParticleSystem ps in PS) {
					var em = ps.emission;
					em.enabled = behaviors[currBehavior].firing;
				}
			}
		}
		else return;
		prevBehavior = currBehavior;
	}

	public void SetTarget(GameObject target){
		Target = target;
	}
}
