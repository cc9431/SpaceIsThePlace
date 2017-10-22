using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGController : MonoBehaviour {
	MeshRenderer MR;
	Material mat;
	public float speedDelay;
	public float trackSpeed;
	public bool delay;

	void Start() {
		MR = GetComponent<MeshRenderer>();
		mat = MR.material;
	}
	
	void LateUpdate () {
		if (delay){
			Vector3 offset = transform.position;
			offset.y = (Camera.main.transform.position.y) / speedDelay;
			offset.x = (Camera.main.transform.position.x) / speedDelay;
			offset.z = transform.position.z;

			transform.position = offset;
		}

		if (MasterController.IsEmpty()) {
			Vector2 setOff = mat.mainTextureOffset;
			setOff.y += (trackSpeed/1000);
			
			mat.mainTextureOffset = setOff;
		}
	}
}
