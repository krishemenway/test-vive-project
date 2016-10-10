using System.Collections.Generic;
using UnityEngine;

public class PlayerAvatarCollisionDetector : MonoBehaviour
{
	private void Awake()
	{
		CollisionItems = new HashSet<InteractableItem>();
	}

	private void OnTriggerEnter(Collider collider)
	{
		var collidedItem = collider.GetComponent<InteractableItem>();
		if (collidedItem != null)
		{
			CollisionItems.Add(collidedItem);

			Debug.Log("Start collision with " + collidedItem.name + "(" + CollisionItems.Count + " current collisions)");
		}
	}

	private void OnTriggerExit(Collider collider)
	{
		var collidedItem = collider.GetComponent<InteractableItem>();
		if (collidedItem != null)
		{
			CollisionItems.Remove(collidedItem);

			Debug.Log("Stop collision with " + collidedItem.name + "(" + CollisionItems.Count + " current collisions)");
		}
	}

	public HashSet<InteractableItem> CollisionItems { get; set; }
}