using UnityEngine;

class PlayerAvatarHand : MonoBehaviour
{
	void Start()
	{
		PlayerAvatar = GetComponentInParent<PlayerAvatar>();

		if (!IsLocalPlayer)
		{
			enabled = false;
			return;
		}

		Controller = Hand == HandType.Left ? PlayerAvatar.LeftController : PlayerAvatar.RightController;
	}

	public HandType Hand;

	public PlayerAvatar PlayerAvatar { get; private set; }
	public SteamVR_TrackedController Controller { get; private set; }
	public bool IsLocalPlayer
	{
		get { return PlayerAvatar.isLocalPlayer; }
	}
}

public enum HandType
{
	Left,
	Right
}