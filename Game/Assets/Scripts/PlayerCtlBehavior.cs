using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody))]
public class PlayerCtlBehavior : MonoBehaviour {
	
	private Vector3 targetGrav;



	void Start() {
		targetGrav = new Vector3(0.0f,-1.0f,0.0f);
	}

	void FixedUpdate() {
		if (gravityUpdate ()) 
		{
			transform.up = -Physics.gravity;
			Rigidbody body = GetComponent<Rigidbody>();
			body.velocity = new Vector3(0.0f,0.0f,0.0f);
		}
	}

	void OnTriggerEnter(Collider other) {
		if(other.gameObject.tag == "gravArrow") {
			other.gameObject.SetActive(false);
			GravitationOrientation orientation = other.gameObject.GetComponent<GravitationOrientation>();
			targetGrav = new Vector3(orientation.x_Grav_Factor,orientation.y_Grav_Factor,orientation.z_Grav_Factor);
		}
	}

	bool gravityUpdate() {
		if (Physics.gravity.normalized != targetGrav) {
			Physics.gravity = new Vector3 (targetGrav.x * 9.81f, targetGrav.y * 9.81f, targetGrav.z * 9.81f);
			return true;
		}
		return false;
		/*
		Quaternion prevWorldOrient = worldTransform.localRotation;
		worldTransform.localRotation = Quaternion.RotateTowards(worldTransform.localRotation,
				targetWorldOrient, gravityRotateSpeed * Time.fixedDeltaTime);
		Quaternion deltaOrient = Quaternion.Inverse(prevWorldOrient) * worldTransform.localRotation;
		transform.localRotation = deltaOrient * transform.localRotation;
		*/
	}

}
