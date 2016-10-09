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

	void Start()
	{
		found_rigidbody = GetComponent<Rigidbody>();
		interactionPoint = new GameObject().transform;
		velocityFactor /= found_rigidbody.mass;
		rotationFactor /= found_rigidbody.mass;
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
		interactionPoint.position = wand.transform.position;
		interactionPoint.rotation = wand.transform.rotation;
		interactionPoint.SetParent(transform, true);

		currentlyInteracting = true;
	}

	public void EndInteraction(Component wand)
	{
		if (wand == attachedWand)
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
