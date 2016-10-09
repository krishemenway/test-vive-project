using UnityEngine;
using UnityEngine.Networking;

public class PlayerAvatar : NetworkBehaviour
{
	public void Start()
	{
		if (!isLocalPlayer)
		{
			return;
		}
	}

	public void Update()
	{
		if (!isLocalPlayer)
		{
			return;
		}

		var trigger = Valve.VR.EVRButtonId.k_EButton_SteamVR_Trigger;

		TriggerButtonPressed = _leftController.GetPress(trigger);
	}

	public override void OnStartLocalPlayer()
	{
		CameraRig = GameObject.Find("LocalPlayerCameraRig");

		FindTrackedObjects();

		AttachGameObjectToTransform(CameraRig, transform);
		AttachGameObjectToTransform(AvatarHead, _cameraRigHeadTransform);
		AttachGameObjectToTransform(AvatarLeftHand, _cameraRigLeftControllerObject.transform);
		AttachGameObjectToTransform(AvatarRightHand, _cameraRigRightControllerObject.transform);
	}

	private void FindTrackedObjects()
	{
		_cameraRigHeadTransform = CameraRig.transform.Find("Head (eye)");
		_cameraRigLeftControllerObject = CameraRig.transform.Find("LeftController").GetComponent<SteamVR_TrackedObject>();
		_cameraRigRightControllerObject = CameraRig.transform.Find("RightController").GetComponent<SteamVR_TrackedObject>();

		_leftController = SteamVR_Controller.Input((int)_cameraRigLeftControllerObject.index);
		_rightController = SteamVR_Controller.Input((int)_cameraRigRightControllerObject.index);
	}

	private void AttachGameObjectToTransform(GameObject gameObject, Transform t)
	{
		gameObject.transform.parent = t;
		gameObject.transform.localRotation = Quaternion.identity;
		gameObject.transform.localPosition = Vector3.zero;
	}

	private SteamVR_TrackedObject _cameraRigLeftControllerObject;
	private SteamVR_TrackedObject _cameraRigRightControllerObject;
	private Transform _cameraRigHeadTransform;

	private SteamVR_Controller.Device _leftController;
	private SteamVR_Controller.Device _rightController;

	public GameObject CameraRig;

	public GameObject AvatarHead;
	public GameObject AvatarLeftHand;
	public GameObject AvatarRightHand;

	public bool TriggerButtonPressed;
}
