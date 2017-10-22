using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {
	public Transform player;
	public Transform BG;
	private Camera cam;
	
	private void Start() {
		cam = GetComponent<Camera>();
	}

	void LateUpdate () {
		cam.orthographicSize = Mathf.Clamp(Mathf.Lerp(cam.orthographicSize, MasterController.enemies.Count, Time.deltaTime), 20, 40);

		Vector3 pos = player.position;
		float clampx = BG.localScale.x/2 - (cam.orthographicSize + 25) + Mathf.Abs(BG.position.x);
		float clampy = BG.localScale.y/2 - cam.orthographicSize + Mathf.Abs(BG.position.y);

		pos.x = Mathf.Clamp(pos.x, -clampx, clampx);
		pos.y = Mathf.Clamp(pos.y, -clampy, clampy);
		pos.z = transform.position.z;

		transform.position = pos;
	}
}
