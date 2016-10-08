using System.Collections.Generic;
using UnityEngine;

class WandController : MonoBehaviour
{
	private SteamVR_TrackedObject TrackedObj { get; set; }
	private SteamVR_Controller.Device Controller
	{
		get
		{
			return SteamVR_Controller.Input((int)TrackedObj.index);
		}
	}

	private HashSet<InteractableItem> objectsHoveringOver = new HashSet<InteractableItem>();

	private InteractableItem closestItem;
	private InteractableItem interactingItem;

	public bool GripButtonUp;
	public bool GripButtonDown;
	public bool GripButtonPressed;
	public bool TriggerButtonPressed;

	void Start()
	{
		TrackedObj = GetComponent<SteamVR_TrackedObject>();
	}

	void Update()
	{
		var c = Controller;

		var grip = Valve.VR.EVRButtonId.k_EButton_Grip;
		var trigger = Valve.VR.EVRButtonId.k_EButton_SteamVR_Trigger;

		GripButtonPressed = c.GetPress(grip);
		GripButtonUp = c.GetPressUp(grip);
		GripButtonDown = c.GetPressDown(grip);

		TriggerButtonPressed = c.GetPress(trigger);

		if (GripButtonDown)
		{
			float minDistance = float.MaxValue;

			float distance;
			foreach (var item in objectsHoveringOver)
			{
				distance = (item.transform.position - transform.position).sqrMagnitude;

				if (distance < minDistance)
				{
					minDistance = distance;
					closestItem = item;
				}
			}

			interactingItem = closestItem;

			if (interactingItem)
			{
				if (interactingItem.IsInteracting())
				{
					interactingItem.EndInteraction(this);
				}

				interactingItem.BeginInteraction(this);
			}
		}

		if (GripButtonUp && interactingItem != null)
		{
			interactingItem.EndInteraction(this);
		}
	}

	void OnTriggerEnter(Collider collider)
	{
		Debug.Log("On Enter");

		var collidedItem = collider.GetComponent<InteractableItem>();
		if (collidedItem != null)
		{
			objectsHoveringOver.Add(collidedItem);
		}
	}

	void OnTriggerExit(Collider collider)
	{
		Debug.Log("On Exit");

		var collidedItem = collider.GetComponent<InteractableItem>();
		if (collidedItem != null)
		{
			objectsHoveringOver.Remove(collidedItem);
		}
	}
}
