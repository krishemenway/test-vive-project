using UnityEngine;
using UnityEngine.Networking;

class PlayerAvatarHandTeleporter : NetworkBehaviour
{
	private void Start()
	{
		if (!isLocalPlayer)
		{
			enabled = false;
			return;
		}

		_playerAvatar = GetComponent<PlayerAvatar>();
		_controller = _playerAvatar.LeftController;

		_controller.TriggerClicked += _leftController_TriggerClicked;
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
		Ray ray = new Ray(_controller.transform.position, _controller.transform.forward);

		bool hasGroundTarget = false;
		float dist = 0f;
		hasGroundTarget = plane.Raycast(ray, out dist);

		if (hasGroundTarget)
		{
			Vector3 headPosOnGround = new Vector3(SteamVR_Render.Top().head.localPosition.x, 0.0f, SteamVR_Render.Top().head.localPosition.z);

			// Standard transport behavior -- moves the camera only... this is bad, the player avatar position doesn't change, so the camera is way far away from the base position of the player
			//t.position = ray.origin + ray.direction * dist - new Vector3(t.GetChild(0).localPosition.x, 0f, t.GetChild(0).localPosition.z) - headPosOnGround;

			// Better transport behavior -- moves the player avatar position, but seems to not move where you point exactly
			//_playerAvatar.transform.position = ray.origin + ray.direction * dist - new Vector3(t.GetChild(0).localPosition.x, 0f, t.GetChild(0).localPosition.z) - headPosOnGround;

			// Fixed???  Ignores the Child(0) position (Whatever that is supposed to be?  maybe it is the location of your head within the play area???)
			_playerAvatar.transform.position = ray.origin + ray.direction * dist - headPosOnGround;
		}
	}

	private SteamVR_TrackedController _controller;
	private PlayerAvatar _playerAvatar;
}
