using UnityEngine;
using UnityEngine.Networking;

public class PlayerAvatarHandGrab : PlayerAvatarHand
{
	protected override void StartHand()
	{
		_collisionDetector = HandGameObject.GetComponent<PlayerAvatarCollisionDetector>();
		_interactingItem = null;
	}

	private void Update()
	{
		if (!isLocalPlayer)
		{
			return;
		}

		var grippedNow = Controller.gripped;

		var gripDown = !_isGripped && grippedNow;
		var gripUp = _isGripped && !grippedNow;
		_isGripped = grippedNow;

		if (gripDown)
		{
			float minDistance = float.MaxValue;
			InteractableItem closestItem = null;
			foreach (var item in _collisionDetector.CollisionItems)
			{
				var distance = (item.transform.position - Controller.transform.position).sqrMagnitude;

				if (distance < minDistance)
				{
					minDistance = distance;
					closestItem = item;
				}
			}

			if (closestItem != null)
			{
				Debug.Log("Send command to server: PickUpObject, ObjectId=" + closestItem.netId + ", PlayerId=" + PlayerAvatar.netId);
				CmdPickUpObject(closestItem.netId);
			}
		}

		if (gripUp)
		{
			Debug.Log("Send command to server: DropObject, PlayerId=" + PlayerAvatar.netId);
			CmdDropObject();
		}
	}

	[Command]
	private void CmdPickUpObject(NetworkInstanceId objectToPickUpId)
	{
		Debug.Log("Got command on server: PickUpObject, ObjectId=" + objectToPickUpId + ", PlayerId=" + PlayerAvatar.netId);

		var gameObject = NetworkServer.FindLocalObject(objectToPickUpId);

		Debug.Log("Found game object from ID: " + gameObject.name);

		var item = gameObject.GetComponent<InteractableItem>();

		if (item != null)
		{
			if (_interactingItem != null)
			{
				_interactingItem.EndInteraction(PlayerAvatar);
			}

			_interactingItem = item;
			_interactingItem.BeginInteraction(PlayerAvatar, HandGameObject.transform);
		}
	}

	[Command]
	private void CmdDropObject()
	{
		Debug.Log("Got command on server: DropObject, PlayerId=" + PlayerAvatar.netId);

		if (_interactingItem != null)
		{
			_interactingItem.EndInteraction(PlayerAvatar);
			_interactingItem = null;
		}
	}

	// Server variables
	private InteractableItem _interactingItem;

	// Client variables
	private bool _isGripped = false;
	private PlayerAvatarCollisionDetector _collisionDetector;
}