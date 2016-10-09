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
		AttachObjectToParentTransform(_cameraRig, transform);

		// Attach the player avatar components to the SteamVR tracked objects
		AttachObjectToParentTransform(AvatarHead, _cameraRigHeadTransform);
		AttachObjectToParentTransform(AvatarLeftHand, LeftController.transform);
		AttachObjectToParentTransform(AvatarRightHand, RightController.transform);
	}

	private void FindTrackedObjects()
	{
		_cameraRig = GameObject.Find("LocalPlayerCameraRig");
		_cameraRigHeadTransform = _cameraRig.transform.Find("Head (eye)");

		LeftController = _cameraRig.transform.Find("LeftController").GetComponent<SteamVR_TrackedController>();
		RightController = _cameraRig.transform.Find("RightController").GetComponent<SteamVR_TrackedController>();
	}

	private void AttachObjectToParentTransform(GameObject gameObject, Transform t)
	{
		gameObject.transform.parent = t;
		gameObject.transform.localRotation = Quaternion.identity;
		gameObject.transform.localPosition = Vector3.zero;
	}

	private GameObject _cameraRig;
	private Transform _cameraRigHeadTransform;

	public SteamVR_TrackedController LeftController;
	public SteamVR_TrackedController RightController;

	public GameObject AvatarHead;
	public GameObject AvatarLeftHand;
	public GameObject AvatarRightHand;
}