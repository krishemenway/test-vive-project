using System.Collections.Generic;
using UnityEngine;

public class PlayerAvatarLeftHand : MonoBehaviour
{
	void Start()
	{
		_playerAvatar = GetComponentInParent<PlayerAvatar>();

		if (!_playerAvatar.isLocalPlayer)
		{
			enabled = false;
			return;
		}

		_leftController = _playerAvatar.LeftController;

		_leftController.TriggerClicked += _leftController_TriggerClicked;

		StartPickingStuffUp();
		StartLaserPointer();
	}

	void Update()
	{
		UpdatePickingStuffUp();
		UpdateLaserPointer();
	}

	#region Laser Pointer
	public Color color;
	public float thickness = 0.002f;
	private GameObject holder;
	private GameObject pointer;
	private GameObject box;
	private bool isActive = false;

	private void StartLaserPointer()
	{
		var newMaterial = new Material(Shader.Find("Unlit/Color"));
		newMaterial.SetColor("_Color", color);

		holder = new GameObject();
		holder.transform.parent = _leftController.transform;
		holder.transform.localPosition = Vector3.zero;
		holder.transform.localRotation = Quaternion.identity;

		pointer = GameObject.CreatePrimitive(PrimitiveType.Cube);
		pointer.transform.parent = holder.transform;
		pointer.transform.localScale = new Vector3(thickness, thickness, 100f);
		pointer.transform.localPosition = new Vector3(0f, 0f, 50f);
		pointer.transform.localRotation = Quaternion.identity;
		pointer.GetComponent<MeshRenderer>().material = newMaterial;

		var collider = pointer.GetComponent<BoxCollider>();
		if (collider)
		{
			Object.Destroy(collider);
		}
	}

	private void UpdateLaserPointer()
	{
		if (!isActive)
		{
			isActive = true;
			_leftController.transform.GetChild(0).gameObject.SetActive(true);
		}

		Ray raycast = new Ray(_leftController.transform.position, _leftController.transform.forward);
		RaycastHit hit;
		bool bHit = Physics.Raycast(raycast, out hit);

		float dist = 100f;
		if (bHit && hit.distance < 100f)
		{
			dist = hit.distance;
		}

		if (_leftController.triggerPressed)
		{
			pointer.transform.localScale = new Vector3(thickness * 5f, thickness * 5f, dist);
		}
		else
		{
			pointer.transform.localScale = new Vector3(thickness, thickness, dist);
		}
		pointer.transform.localPosition = new Vector3(0f, 0f, dist / 2f);
	}
	#endregion

	#region Teleportation
	private Transform reference
	{
		get
		{
			var top = SteamVR_Render.Top();
			return (top != null) ? top.origin : null;
		}
	}

	private void _leftController_TriggerClicked(object sender, ClickedEventArgs e)
	{
		var t = reference;
		if (t == null)
			return;

		float refY = t.position.y;

		Plane plane = new Plane(Vector3.up, -refY);
		Ray ray = new Ray(_leftController.transform.position, _leftController.transform.forward);

		bool hasGroundTarget = false;
		float dist = 0f;
		hasGroundTarget = plane.Raycast(ray, out dist);

		if (hasGroundTarget)
		{
			Vector3 headPosOnGround = new Vector3(SteamVR_Render.Top().head.localPosition.x, 0.0f, SteamVR_Render.Top().head.localPosition.z);

			// Standard transport behavior -- moves the camera only ???
			//t.position = ray.origin + ray.direction * dist - new Vector3(t.GetChild(0).localPosition.x, 0f, t.GetChild(0).localPosition.z) - headPosOnGround;

			// My transport behavior -- moves the player avatar position ???
			//_playerAvatar.transform.position = ray.origin + ray.direction * dist - new Vector3(t.GetChild(0).localPosition.x, 0f, t.GetChild(0).localPosition.z) - headPosOnGround;

			// Fixed???
			_playerAvatar.transform.position = ray.origin + ray.direction * dist - headPosOnGround;
		}
	}
	#endregion

	#region Picking stuff up
	private void StartPickingStuffUp()
	{
	}

	private bool isGripped = false;

	private void UpdatePickingStuffUp()
	{
		var grippedNow = _leftController != null && _leftController.gripped;

		var gripDown = !isGripped && grippedNow;
		var gripUp = isGripped && !grippedNow;
		isGripped = grippedNow;

		if (gripDown)
		{
			Debug.Log("GRIP DOWN");

			float minDistance = float.MaxValue;

			float distance;
			foreach (var item in _objectsHoveringOver)
			{
				distance = (item.transform.position - _leftController.transform.position).sqrMagnitude;

				if (distance < minDistance)
				{
					minDistance = distance;
					_closestItem = item;
				}
			}

			_interactingItem = _closestItem;

			if (_interactingItem)
			{
				_interactingItem.BeginInteraction(_leftController.transform);
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
			Debug.Log("On Enter " + collidedItem.name);

			_objectsHoveringOver.Add(collidedItem);

			Debug.Log("Number of items: " + _objectsHoveringOver.Count);
		}
	}

	void OnTriggerExit(Collider collider)
	{
		var collidedItem = collider.GetComponent<InteractableItem>();
		if (collidedItem != null)
		{
			Debug.Log("On Exit " + collidedItem.name);

			_objectsHoveringOver.Remove(collidedItem);

			Debug.Log("Number of items: " + _objectsHoveringOver.Count);
		}
	}

	private InteractableItem _closestItem;
	private InteractableItem _interactingItem;

	private HashSet<InteractableItem> _objectsHoveringOver = new HashSet<InteractableItem>();
	#endregion

	private PlayerAvatar _playerAvatar;
	private SteamVR_TrackedController _leftController;
}
