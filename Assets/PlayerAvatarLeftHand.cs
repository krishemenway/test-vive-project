using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerAvatarLeftHand : NetworkBehaviour
{
	void Awake()
	{
		if (!isLocalPlayer)
		{
			enabled = false;
		}
	}

	void Update()
	{
		if (_leftController.triggerPressed)
		{
			Debug.Log("LEFT TRIGGER PRESSED");

			float minDistance = float.MaxValue;

			float distance;
			foreach (var item in _objectsHoveringOver)
			{
				distance = (item.transform.position - transform.position).sqrMagnitude;

				if (distance < minDistance)
				{
					minDistance = distance;
					_closestItem = item;
				}
			}

			_interactingItem = _closestItem;

			if (_interactingItem)
			{
				if (_interactingItem.IsInteracting())
				{
					_interactingItem.EndInteraction(this);
				}

				_interactingItem.BeginInteraction(this);
			}
		}
		else
		{
			if (_interactingItem != null)
			{
				_interactingItem.EndInteraction(this);
			}
		}
	}

	public override void OnStartLocalPlayer()
	{
		_leftController = GetComponentInParent<PlayerAvatar>().LeftController;
	}

	void OnTriggerEnter(Collider collider)
	{
		Debug.Log("On Enter");

		var collidedItem = collider.GetComponent<InteractableItem>();
		if (collidedItem != null)
		{
			_objectsHoveringOver.Add(collidedItem);
		}
	}

	void OnTriggerExit(Collider collider)
	{
		Debug.Log("On Exit");

		var collidedItem = collider.GetComponent<InteractableItem>();
		if (collidedItem != null)
		{
			_objectsHoveringOver.Remove(collidedItem);
		}
	}

	private SteamVR_TrackedController _leftController;

	private InteractableItem _closestItem;
	private InteractableItem _interactingItem;

	private HashSet<InteractableItem> _objectsHoveringOver = new HashSet<InteractableItem>();
}
