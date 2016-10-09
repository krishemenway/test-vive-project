using UnityEngine;

class InteractableItem : MonoBehaviour
{
	public Rigidbody found_rigidbody;

	private bool currentlyInteracting;

	private Component attachedWand;

	private Transform interactionPoint;

	private float velocityFactor = 20000f;
	private Vector3 posDelta;

	private float rotationFactor = 400f;
	private Quaternion rotationDelta;
	private float angle;
	private Vector3 axis;

	private GameObject pointer;
	private GameObject box;

	void Start()
	{
		found_rigidbody = GetComponent<Rigidbody>();
		interactionPoint = new GameObject().transform;
		velocityFactor /= found_rigidbody.mass;
		rotationFactor /= found_rigidbody.mass;



		var newMaterial = new Material(Shader.Find("Unlit/Color"));
		newMaterial.SetColor("_Color", Color.blue);

		box = new GameObject();
		box.transform.parent = interactionPoint.transform;
		box.transform.localPosition = Vector3.zero;
		box.transform.localRotation = Quaternion.identity;

		pointer = GameObject.CreatePrimitive(PrimitiveType.Cube);
		pointer.transform.parent = box.transform;
		pointer.transform.localScale = new Vector3(0.25f, 0.25f, 0.25f);
		pointer.transform.localPosition = Vector3.zero;
		pointer.transform.localRotation = Quaternion.identity;
		pointer.GetComponent<MeshRenderer>().material = newMaterial;

		var collider = pointer.GetComponent<BoxCollider>();
		if (collider)
		{
			Object.Destroy(collider);
		}
	}

	void Update()
	{
		if (attachedWand != null && currentlyInteracting)
		{
			posDelta = attachedWand.transform.position - interactionPoint.position;
			found_rigidbody.velocity = posDelta * velocityFactor * Time.fixedDeltaTime;

			rotationDelta = attachedWand.transform.rotation * Quaternion.Inverse(interactionPoint.rotation);
			rotationDelta.ToAngleAxis(out angle, out axis);

			if (angle > 180)
			{
				angle -= 360;
			}

			found_rigidbody.angularVelocity = (Time.fixedDeltaTime * angle * axis) * rotationFactor;
		}
	}

	public void BeginInteraction(Component wand)
	{
		attachedWand = wand;

		interactionPoint.position = attachedWand.transform.position;
		interactionPoint.rotation = attachedWand.transform.rotation;
		interactionPoint.SetParent(this.transform, true);

		currentlyInteracting = true;
	}

	public void EndInteraction(Component wand)
	{
		if (attachedWand == wand)
		{
			attachedWand = null;
			currentlyInteracting = false;
		}
	}

	public bool IsInteracting()
	{
		return currentlyInteracting;
	}
}
