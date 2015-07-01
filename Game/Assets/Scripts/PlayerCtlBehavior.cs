using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody))]
public class PlayerCtlBehavior : MonoBehaviour {

	private Vector3 targetGrav;
	private bool killed;

	public float boostStrength = 5.0f;

	void Start() {
		targetGrav = new Vector3(0.0f,-1.0f,0.0f);
	}

	void FixedUpdate() {
		if (gravityUpdate ())
		{
			float playerHeight = GetComponent<CapsuleCollider>().height;
			transform.up = -Physics.gravity;
			Rigidbody body = GetComponent<Rigidbody>();
			body.velocity = new Vector3(0.0f,0.0f,0.0f);
			//body.velocity = Physics.gravity.normalized * boostStrength;
			transform.position += Physics.gravity.normalized * boostStrength;
		}
		if(killed) {
			killed = false;
			Application.LoadLevel(Application.loadedLevelName);
		}
	}

	void OnTriggerEnter(Collider other) {
		if(other.gameObject.tag == "gravArrow") {
			other.gameObject.SetActive(false);
			GravitationOrientation orientation = other.gameObject.GetComponent<GravitationOrientation>();
			targetGrav = new Vector3(orientation.x_Grav_Factor,orientation.y_Grav_Factor,orientation.z_Grav_Factor);
		}
		else if(other.gameObject.tag == "killZone")
			killed = true;
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
