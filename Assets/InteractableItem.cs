using UnityEngine;
using UnityEngine.Networking;

public class InteractableItem : NetworkBehaviour
{
	private void Awake()
	{
		_rigidBody = GetComponent<Rigidbody>();

		velocityFactor /= _rigidBody.mass;
		rotationFactor /= _rigidBody.mass;

		_initialGrabLocation = new GameObject();
		_initialGrabLocation.AttachDebugCube();
	}

	private void Update()
	{
		if (!isServer)
		{
			return;
		}

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

	public void BeginInteraction(PlayerAvatar playerAvatar, Transform handTransform)
	{
		Debug.Log("Begin interaction on " + name + " for player " + playerAvatar.name + " with hand " + handTransform.name);

		_currentPlayerAvatar = playerAvatar;
		_currentGrabLocation = handTransform;

		// Remember the initial grab location relative to the object that is being grabbed
		_initialGrabLocation.transform.position = _currentGrabLocation.position;
		_initialGrabLocation.transform.rotation = _currentGrabLocation.rotation;
		_initialGrabLocation.transform.SetParent(transform, true);
	}

	public void EndInteraction(PlayerAvatar playerAvatar)
	{
		Debug.Log("End interaction on " + name + "?");

		if (_currentPlayerAvatar == playerAvatar)
		{
			_currentPlayerAvatar = null;
			_currentGrabLocation = null;
		}
	}

	private bool IsInteracting
	{
		get { return _currentPlayerAvatar != null; }
	}

	private Rigidbody _rigidBody;

	private PlayerAvatar _currentPlayerAvatar;
	private Transform _currentGrabLocation;

	private GameObject _initialGrabLocation;

	private float velocityFactor = 20000f;
	private float rotationFactor = 400f;
}
