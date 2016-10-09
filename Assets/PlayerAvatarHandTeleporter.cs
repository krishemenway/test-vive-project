using UnityEngine;

class PlayerAvatarHandTeleporter : MonoBehaviour
{
	void Start()
	{
		_hand = GetComponentInParent<PlayerAvatarHand>();

		if (!_hand.IsLocalPlayer)
		{
			enabled = false;
			return;
		}

		_hand.Controller.TriggerClicked += _leftController_TriggerClicked;
	}

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
		Ray ray = new Ray(_hand.Controller.transform.position, _hand.Controller.transform.forward);

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
			_hand.PlayerAvatar.transform.position = ray.origin + ray.direction * dist - headPosOnGround;
		}
	}

	private PlayerAvatarHand _hand;
}
