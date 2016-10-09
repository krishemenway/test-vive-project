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
	}

	void Update()
	{
		AvatarHead.transform.position = _cameraRigHeadTransform.transform.position;
		AvatarHead.transform.rotation = _cameraRigHeadTransform.transform.rotation;

		AvatarLeftHand.transform.position = LeftController.transform.position;
		AvatarLeftHand.transform.rotation = LeftController.transform.rotation;

		AvatarRightHand.transform.position = RightController.transform.position;
		AvatarRightHand.transform.rotation = RightController.transform.rotation;
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