using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public abstract class Behavior : ScriptableObject {
	/// High level class for all behaviors
	protected int state;
	protected static readonly int NO_ACTION = 0;
	protected GameObject Target;
	protected EnemyController controller;
	public int speed;
	public int turnSpeed;
	public bool firing;
	public bool debug;

	public Behavior(){
		state = NO_ACTION;
	}

	public virtual void init(EnemyController c){		
	}

	public virtual bool check(){
		return false;
	}

	public virtual void run(){
	}
}

[CreateAssetMenu(fileName = "Attack")]
public class Attack : Behavior {
	const int STRAIGHT = 1;
	public Attack(){}

	public override void init(EnemyController c){
		controller = c;
		Target = controller.Target;
		state = STRAIGHT;
		
		speed = 40;
		turnSpeed = 10;
		firing = true;
		debug = false;
	}

	public override bool check(){
		return (Vector2.Distance(controller.transform.position, Target.transform.position) < 20);
	}

	public override void run(){
		controller.transform.Translate(Vector2.up * speed/100f);
		Vector3 diff = Target.transform.position - controller.transform.position;
		diff.Normalize();
		float rot_z = Mathf.Atan2(diff.y, diff.x) * Mathf.Rad2Deg;
		controller.transform.rotation = Quaternion.Lerp(controller.transform.rotation, Quaternion.Euler(0f, 0f, rot_z - 90), Time.deltaTime * turnSpeed);
	}
}

[CreateAssetMenu(fileName = "Retreat")]
public class Retreat : Behavior {
	const int RUN = 1;

	public Retreat(){}

	public override void init(EnemyController c){
		controller = c;
		Target = controller.Target;
		state = RUN;

		speed = 20;
		turnSpeed = 1;
		firing = false;
		debug = false;
	}

	public override bool check(){
		bool run = (controller.DHC.health < 10);
		state = run ? RUN : NO_ACTION;
		return run;
	}

	public override void run(){
		base.run();
		controller.transform.Translate(Vector2.up * (speed/100f));
		Vector3 diff = controller.transform.position - Target.transform.position;
		diff.Normalize();
		float rot_z = Mathf.Atan2(diff.y, diff.x) * Mathf.Rad2Deg;
		controller.transform.rotation = Quaternion.Lerp(controller.transform.rotation, Quaternion.Euler(0f, 0f, rot_z - 90), Time.deltaTime * turnSpeed);
	}

}

[CreateAssetMenu(fileName = "Search")]
public class Search : Behavior {
	const int LEFT = 1;
	const int RIGHT = 2;
	const int STRAIGHT = 3;
	int offset;
	Transform PlayerSprite;

	public Search(){}

	public override void init(EnemyController c){
		controller = c;
		Target = controller.Target;
		state = Random.Range(LEFT, STRAIGHT + 1);
		switch (state){
			case LEFT : offset = 1; break;
			case RIGHT : offset = -1; break;
			default: offset = 0; break;
		}

		speed = 50;
		turnSpeed = 3;
		firing = false;
		debug = false;

		PlayerSprite = Target.GetComponentInChildren<DamageHealthController>().gameObject.transform;
	}

	public override bool check(){
		return controller.DHC.alive;
	}

	public override void run(){
		controller.transform.Translate(Vector2.up * speed/100f);
		Vector3 zdiff = Target.transform.position - controller.transform.position;
		Vector3 diff = (Target.transform.position + PlayerSprite.right * offset * (10 - zdiff.magnitude)) - controller.transform.position;
		if (debug) Debug.DrawLine(controller.transform.position, (Target.transform.position + PlayerSprite.right * offset * (20 - zdiff.magnitude)), Color.red);
		diff.Normalize();
		float rot_z = Mathf.Atan2(diff.y, diff.x) * Mathf.Rad2Deg;
		controller.transform.rotation = Quaternion.Lerp(controller.transform.rotation, Quaternion.Euler(0f, 0f, rot_z - 90), Time.deltaTime * turnSpeed);
	}
}

[CreateAssetMenu(fileName = "Follow")]
public class Follow : Behavior {
	const int FOLLOW = 1;

	public Follow(){}

	public override void init(EnemyController c){
		controller = c;
		state = FOLLOW;

		turnSpeed = 6;
		firing = false;
		debug = false;
	}

	public override bool check(){
		if (Target == null){
			List<GameObject> Leaders = new List<GameObject>();
			foreach (GameObject e in MasterController.enemies){
				if (!e.GetComponent<EnemyController>().follower) Leaders.Add(e);
			}

			if (Leaders.Count > 0) {
				Target = Leaders[Random.Range(0, Leaders.Count)];
				return true;
			}

			return false;
		} else {
			return true;
		}
	}

	public override void run(){
		speed = Target.GetComponent<EnemyController>().speed + 1;
		if (debug) Debug.DrawRay(controller.transform.position, controller.transform.up * 50f, Color.green);
		controller.transform.Translate(Vector2.up * speed/100f);
		Vector3 diff = (Target.transform.position - Target.transform.up*4f) - controller.transform.position;
		diff.Normalize();
		float rot_z = Mathf.Atan2(diff.y, diff.x) * Mathf.Rad2Deg;
		controller.transform.rotation = Quaternion.Lerp(controller.transform.rotation, Quaternion.Euler(0f, 0f, rot_z - 90), Time.deltaTime * turnSpeed);
	}
}
