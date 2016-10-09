using System.Collections.Generic;
using UnityEngine;

class PlayerAvatarHandGrab : MonoBehaviour
{
	{
		_interactableItems = new HashSet<InteractableItem>();
		_interactingItem = null;
	}

	void Start()
	{
		_hand = GetComponentInParent<PlayerAvatarHand>();

		if (!_hand.IsLocalPlayer)
		{
			enabled = false;
			return;
		}
	}

	void Update()
	{
		var grippedNow = _hand.Controller.gripped;

		var gripDown = !_isGripped && grippedNow;
		var gripUp = _isGripped && !grippedNow;
		_isGripped = grippedNow;

		if (gripDown)
		{
			Debug.Log("GRIP DOWN");

			float minDistance = float.MaxValue;
			InteractableItem closestItem = null;
			foreach (var item in _interactableItems)
			{
				var distance = (item.transform.position - _hand.Controller.transform.position).sqrMagnitude;

				if (distance < minDistance)
				{
					minDistance = distance;
					closestItem = item;
				}
			}

			if (closestItem != null)
			{
				if (_interactingItem != null)
				{
					_interactingItem.EndInteraction();
				}

				_interactingItem = closestItem;
				_interactingItem.BeginInteraction(_hand.Controller.transform);
			}
		}

		if (gripUp)
		{
			Debug.Log("GRIP UP");

			if (_interactingItem != null)
			{
				_interactingItem.EndInteraction();
			}
		}
	}

	void OnTriggerEnter(Collider collider)
	{
		var collidedItem = collider.GetComponent<InteractableItem>();
		if (collidedItem != null)
		{
			_interactableItems.Add(collidedItem);

			Debug.Log("On Enter " + collidedItem.name);
			Debug.Log("Number of items: " + _interactableItems.Count);
		}
	}

	void OnTriggerExit(Collider collider)
	{
		var collidedItem = collider.GetComponent<InteractableItem>();
		if (collidedItem != null)
		{
			_interactableItems.Remove(collidedItem);

			Debug.Log("On Exit " + collidedItem.name);
			Debug.Log("Number of items: " + _interactableItems.Count);
		}
	}

	private bool _isGripped = false;

	private InteractableItem _interactingItem;
	private HashSet<InteractableItem> _interactableItems;

	private PlayerAvatarHand _hand;
}