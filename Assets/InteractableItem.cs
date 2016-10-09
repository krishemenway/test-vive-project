using UnityEngine;

class InteractableItem : MonoBehaviour
{
	void Start()
	{
		_rigidBody = GetComponent<Rigidbody>();

		velocityFactor /= _rigidBody.mass;
		rotationFactor /= _rigidBody.mass;

		_initialGrabLocation = new GameObject();
		_initialGrabLocation.AttachDebugCube();
	}

	void Update()
	{
		if (IsInteracting)
		{
			var posDelta = _currentGrabLocation.position - _initialGrabLocation.transform.position;
			_rigidBody.velocity = posDelta * velocityFactor * Time.fixedDeltaTime;


			float angle;
			Vector3 axis;

			var rotationDelta = _currentGrabLocation.rotation * Quaternion.Inverse(_initialGrabLocation.transform.rotation);
			rotationDelta.ToAngleAxis(out angle, out axis);
			if (angle > 180)
			{
				angle -= 360;
			}
			_rigidBody.angularVelocity = (Time.fixedDeltaTime * angle * axis) * rotationFactor;
		}
	}

	public void BeginInteraction(Transform grabLocation)
	{
		_currentGrabLocation = grabLocation;

		// Remember the initial grab location relative to the object that is being grabbed
		_initialGrabLocation.transform.position = _currentGrabLocation.position;
		_initialGrabLocation.transform.rotation = _currentGrabLocation.rotation;
		_initialGrabLocation.transform.SetParent(this.transform, true);
	}

	public void EndInteraction()
	{
		_currentGrabLocation = null;
	}

	public bool IsInteracting
	{
		get { return _currentGrabLocation != null; }
	}

	public Rigidbody _rigidBody;

	private Transform _currentGrabLocation;
	private GameObject _initialGrabLocation;

	private float velocityFactor = 20000f;
	private float rotationFactor = 400f;
}
