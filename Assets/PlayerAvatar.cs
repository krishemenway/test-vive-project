using UnityEngine;
using UnityEngine.Networking;

public class PlayerAvatar : NetworkBehaviour
{
	void Start()
	{
		if (!isLocalPlayer)
		{
			return;
		}

		FindTrackedObjects();

		// Attach the SteamVR camera rig to this local player avatar
		_cameraRig.AttachToParent(transform);

		// Attach the player avatar components to the SteamVR tracked objects
		AvatarHead.AttachToParent(_cameraRigHeadTransform);
		AvatarLeftHand.AttachToParent(LeftController.transform);
		AvatarRightHand.AttachToParent(RightController.transform);
	}

	private void FindTrackedObjects()
	{
		_cameraRig = GameObject.Find("LocalPlayerCameraRig");
		_cameraRigHeadTransform = _cameraRig.transform.Find("Head (eye)");

		LeftController = _cameraRig.transform.Find("LeftController").GetComponent<SteamVR_TrackedController>();
		RightController = _cameraRig.transform.Find("RightController").GetComponent<SteamVR_TrackedController>();
	}

	private GameObject _cameraRig;
	private Transform _cameraRigHeadTransform;

	public SteamVR_TrackedController LeftController;
	public SteamVR_TrackedController RightController;

	public GameObject AvatarHead;
	public GameObject AvatarLeftHand;
	public GameObject AvatarRightHand;
}