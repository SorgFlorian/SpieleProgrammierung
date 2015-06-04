using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(BoxCollider))]
[RequireComponent(typeof(Rigidbody))]
public class PlayerCtlBehavior : MonoBehaviour {

	public float animSpeed = 1.5f;
	public float lookSmoother = 3.0f;
	public bool useCurves = true;
	public float useCurvesHeight = 0.5f;
	public float forwardSpeed = 7.0f;
	public float backwardSpeed = 7.0f;
	public float rotateSpeed = 2.0f;
	public float jumpPower = 3.0f;
	public float gravityRotateSpeed = 10.0f;

	public GameObject world;

	private BoxCollider col;
	private Rigidbody rb;
	private Vector3 velocity;
	private float orgColHeight;
	private Vector3 orgVectColCenter;
	private Animator anim;
	private AnimatorStateInfo currentBaseState;
	private Transform worldTransform;
	private Quaternion targetWorldOrient = Quaternion.identity;

	static int idleState = Animator.StringToHash("Base Layer.Idle");
	static int locoState = Animator.StringToHash("Base Layer.Locomotion");
	static int jumpState = Animator.StringToHash("Base Layer.Jump");
	static int restState = Animator.StringToHash("Base Layer.Rest");

	void Start() {
		anim = GetComponent<Animator>();
		col = GetComponent<BoxCollider>();
		rb = GetComponent<Rigidbody>();
		orgColHeight = col.size.y;
		orgVectColCenter = col.center;
		worldTransform = world.GetComponent<Transform>();
	}

	void FixedUpdate() {
		gravityUpdate();
		controlUpdate();
	}

	void controlUpdate() {
		float h = Input.GetAxis("Horizontal");
		float v = Input.GetAxis("Vertical");
		anim.SetFloat("Speed", v);
		anim.SetFloat("Direction", h);
		anim.speed = animSpeed;
		currentBaseState = anim.GetCurrentAnimatorStateInfo(0);
		rb.useGravity = true;
		velocity = new Vector3(0, 0, v);
		velocity = transform.TransformDirection(velocity);
		if(v > 0.1)
			velocity *= forwardSpeed;
		else if(v < -0.1)
			velocity *= backwardSpeed;
		if(Input.GetButtonDown("Jump")) {
			if(currentBaseState.nameHash == locoState) {
				if(!anim.IsInTransition(0)) {
					rb.AddForce(Vector3.up * jumpPower, ForceMode.VelocityChange);
					anim.SetBool("Jump", true);
				}
			}
		}
		transform.localPosition += velocity * Time.fixedDeltaTime;
		transform.Rotate(0, h * rotateSpeed, 0);
		if(currentBaseState.nameHash == locoState) {
			if(useCurves)
				resetCollider();
		}
		else if(currentBaseState.nameHash == jumpState) {
			if(!anim.IsInTransition(0)) {
				if(useCurves) {
					float jumpHeight = anim.GetFloat("JumpHeight");
					float gravityControl = anim.GetFloat("GravityControl");
					if(gravityControl > 0)
						rb.useGravity = false;
					Ray ray = new Ray(transform.position + Vector3.up, -Vector3.up);
					RaycastHit hitInfo = new RaycastHit();
					if(Physics.Raycast(ray, out hitInfo)) {
						if(hitInfo.distance > useCurvesHeight) {
							Vector3 newColSize = col.size;
							newColSize.y = orgColHeight - jumpHeight;
							col.size = newColSize;
							float adjCenterY = orgVectColCenter.y + jumpHeight;
							col.center = new Vector3(0, adjCenterY, 0);
						}
						else
							resetCollider();
					}
					anim.SetBool("Jump", false);
				}
			}
		}
		else if(currentBaseState.nameHash == idleState) {
			if(useCurves)
				resetCollider();
			if(Input.GetButtonDown("Jump"))
				anim.SetBool("Rest", true);
		}
		else if(currentBaseState.nameHash == restState) {
			if(!anim.IsInTransition(0))
				anim.SetBool("Rest", false);
		}
	}

	void resetCollider() {
		Vector3 newColSize = col.size;
		newColSize.y = orgColHeight;
		col.size = newColSize;
		col.center = orgVectColCenter;
	}

	void OnTriggerEnter(Collider other) {
		if(other.gameObject.tag == "gravArrow") {
			other.gameObject.SetActive(false);
			Transform xform = other.gameObject.GetComponent<Transform>();
			Vector3 grav = targetWorldOrient * Vector3.down;
			Vector3 arrow = xform.localRotation * targetWorldOrient * Vector3.up;
			float diff = Vector3.Angle(grav, arrow);
			Vector3 targetGrav;
			if(diff < 10.0f)
				targetGrav = arrow;
			else {
				double halfRad = (double)diff * System.Math.PI / 360.0;
				targetGrav = Vector3.RotateTowards(grav, arrow, (float)halfRad, 0.0f);
			}
			targetWorldOrient = Quaternion.FromToRotation(Vector3.down, targetGrav);
		}
	}

	void gravityUpdate() {
		Quaternion prevWorldOrient = worldTransform.localRotation;
		worldTransform.localRotation = Quaternion.RotateTowards(worldTransform.localRotation,
				targetWorldOrient, gravityRotateSpeed * Time.fixedDeltaTime);
		Quaternion deltaOrient = Quaternion.Inverse(prevWorldOrient) * worldTransform.localRotation;
		transform.localRotation = deltaOrient * transform.localRotation;
	}

}
