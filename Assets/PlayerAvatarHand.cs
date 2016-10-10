using UnityEngine;
using UnityEngine.Networking;

public abstract class PlayerAvatarHand : NetworkBehaviour
{
	private void Start()
	{
		PlayerAvatar = GetComponent<PlayerAvatar>();
		HandGameObject = Hand == HandType.Left ? PlayerAvatar.AvatarLeftHand : PlayerAvatar.AvatarRightHand;
		Controller = Hand == HandType.Left ? PlayerAvatar.LeftController : PlayerAvatar.RightController;

		StartHand();
	}

	protected abstract void StartHand();

	public HandType Hand;

	public PlayerAvatar PlayerAvatar { get; private set; }
	public GameObject HandGameObject { get; private set; }
	public SteamVR_TrackedController Controller { get; private set; }
}

public enum HandType
{
	Left,
	Right
}