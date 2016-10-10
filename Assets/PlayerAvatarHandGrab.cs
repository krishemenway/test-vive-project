using UnityEngine;
using UnityEngine.Networking;

public class PlayerAvatarHandGrab : NetworkBehaviour
{
	private void Start()
	{
		_playerAvatar = GetComponent<PlayerAvatar>();

		_leftHandGrabContext = new HandGrabContext
		{
			HandType = HandType.Left,
			HandGameObject = _playerAvatar.AvatarLeftHand,
			CollisionDetector = _playerAvatar.AvatarLeftHand.GetComponent<PlayerAvatarCollisionDetector>()
		};

		_rightHandGrabContext = new HandGrabContext
		{
			HandType = HandType.Right,
			HandGameObject = _playerAvatar.AvatarRightHand,
			CollisionDetector = _playerAvatar.AvatarRightHand.GetComponent<PlayerAvatarCollisionDetector>()
		};
	}

	private void Update()
	{
		if (!isLocalPlayer)
		{
			return;
		}

		CheckGripOnController(_leftHandGrabContext, _playerAvatar.LeftController);
		CheckGripOnController(_rightHandGrabContext, _playerAvatar.RightController);
	}

	private void CheckGripOnController(HandGrabContext context, SteamVR_TrackedController controller)
	{
		var grippedNow = controller.gripped;

		var gripDown = !context.IsGripped && grippedNow;
		var gripUp = context.IsGripped && !grippedNow;

		context.IsGripped = grippedNow;

		if (gripDown)
		{
			float minDistance = float.MaxValue;
			InteractableItem closestItem = null;
			foreach (var item in context.CollisionDetector.CollisionItems)
			{
				var distance = (item.transform.position - controller.transform.position).sqrMagnitude;

				if (distance < minDistance)
				{
					minDistance = distance;
					closestItem = item;
				}
			}

			if (closestItem != null)
			{
				Debug.Log("Send command to server: PickUpObject, ObjectId=" + closestItem.netId + ", PlayerId=" + _playerAvatar.netId + ", Hand=" + context.HandType);
				CmdPickUpObject(context.HandType, closestItem.netId);
			}
		}

		if (gripUp)
		{
			Debug.Log("Send command to server: DropObject, PlayerId=" + _playerAvatar.netId + ", Hand=" + context.HandType);
			CmdDropObject(context.HandType);
		}
	}

	[Command]
	private void CmdPickUpObject(HandType handType, NetworkInstanceId objectToPickUpId)
	{
		var handGrabContext = handType == HandType.Left ? _leftHandGrabContext : _rightHandGrabContext;

		Debug.Log("Got command on server: PickUpObject, ObjectId=" + objectToPickUpId + ", PlayerId=" + _playerAvatar.netId + ", Hand=" + handType);

		var gameObject = NetworkServer.FindLocalObject(objectToPickUpId);

		Debug.Log("Found game object from ID: " + gameObject.name);

		var item = gameObject.GetComponent<InteractableItem>();

		if (item != null)
		{
			if (handGrabContext.InteractingItem != null)
			{
				handGrabContext.InteractingItem.EndInteraction(_playerAvatar);
			}

			handGrabContext.InteractingItem = item;
			handGrabContext.InteractingItem.BeginInteraction(_playerAvatar, handGrabContext.HandGameObject.transform);
		}
	}

	[Command]
	private void CmdDropObject(HandType handType)
	{
		var handGrabContext = handType == HandType.Left ? _leftHandGrabContext : _rightHandGrabContext;

		Debug.Log("Got command on server: DropObject, PlayerId=" + _playerAvatar.netId + ", Hand=" + handType);

		if (handGrabContext.InteractingItem != null)
		{
			handGrabContext.InteractingItem.EndInteraction(_playerAvatar);
			handGrabContext.InteractingItem = null;
		}
	}

	private PlayerAvatar _playerAvatar;

	private HandGrabContext _leftHandGrabContext;
	private HandGrabContext _rightHandGrabContext;

	private class HandGrabContext
	{
		public HandType HandType;
		public GameObject HandGameObject;
		public PlayerAvatarCollisionDetector CollisionDetector;

		public bool IsGripped;
		public InteractableItem InteractingItem;
	}
}