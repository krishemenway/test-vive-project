using UnityEngine;
using UnityEngine.Networking;

class InteractableItem : NetworkBehaviour
{
	void Awake()
	{
		_rigidBody = GetComponent<Rigidbody>();

		velocityFactor /= _rigidBody.mass;
		rotationFactor /= _rigidBody.mass;

		_initialGrabLocation = new GameObject();
		_initialGrabLocation.AttachDebugCube();
	}

	void Update()
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

	public void BeginInteraction(PlayerAvatarHand hand)
	{
		CmdBeginInteraction(hand.PlayerAvatar.netId, hand.Hand);
	}

	public void EndInteraction(PlayerAvatarHand hand)
	{
		CmdEndInteraction(hand.PlayerAvatar.netId);
	}

	[Command]
	public void CmdBeginInteraction(NetworkInstanceId playerId, HandType hand)
	{
		var playerAvatar = NetworkServer.FindLocalObject(playerId).GetComponent<PlayerAvatar>();
		var playerAvatarHand = hand == HandType.Left ? playerAvatar.AvatarLeftHand : playerAvatar.AvatarRightHand;

		_currentPlayerAvatar = playerAvatar;
		_currentGrabLocation = playerAvatarHand.transform;

		// Remember the initial grab location relative to the object that is being grabbed
		_initialGrabLocation.transform.position = _currentGrabLocation.position;
		_initialGrabLocation.transform.rotation = _currentGrabLocation.rotation;
		_initialGrabLocation.transform.SetParent(transform, true);
	}

	[Command]
	public void CmdEndInteraction(NetworkInstanceId playerId)
	{
		var playerAvatar = NetworkServer.FindLocalObject(playerId).GetComponent<PlayerAvatar>();

		if (_currentPlayerAvatar == playerAvatar)
		{
			_currentPlayerAvatar = null;
			_currentGrabLocation = null;
		}
	}

	public bool IsInteracting
	{
		get { return _currentPlayerAvatar != null; }
	}

	public Rigidbody _rigidBody;

	private PlayerAvatar _currentPlayerAvatar;
	private Transform _currentGrabLocation;

	private GameObject _initialGrabLocation;

	private float velocityFactor = 20000f;
	private float rotationFactor = 400f;
}
