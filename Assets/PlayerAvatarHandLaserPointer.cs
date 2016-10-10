using UnityEngine;
using UnityEngine.Networking;

public class PlayerAvatarHandLaserPointer : NetworkBehaviour
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

		CreateLaserPointerObjects();
	}

	private void CreateLaserPointerObjects()
	{
		var newMaterial = new Material(Shader.Find("Unlit/Color"));
		newMaterial.SetColor("_Color", color);


		_laserPointerStartPosition = new GameObject();
		_laserPointerStartPosition.name = "Laser Pointer Start Position";

		_laserPointerStartPosition.transform.parent = _controller.transform;
		_laserPointerStartPosition.transform.localPosition = Vector3.zero;
		_laserPointerStartPosition.transform.localRotation = Quaternion.identity;


		_laserPointerCube = GameObject.CreatePrimitive(PrimitiveType.Cube);
		_laserPointerCube.name = "Laser Pointer Cube";

		_laserPointerCube.transform.parent = _laserPointerStartPosition.transform;
		_laserPointerCube.transform.localScale = new Vector3(thickness, thickness, 100f);
		_laserPointerCube.transform.localPosition = new Vector3(0f, 0f, 50f);
		_laserPointerCube.transform.localRotation = Quaternion.identity;

		_laserPointerCube.GetComponent<MeshRenderer>().material = newMaterial;

		var collider = _laserPointerCube.GetComponent<BoxCollider>();
		if (collider)
		{
			Destroy(collider);
		}
	}

	private void Update()
	{
		if (!isActive)
		{
			isActive = true;

			// All this code is suspect...
			var childGameObject = _controller.transform.GetChild(0).gameObject;  // This line of code seems extremely suspect...  Why are we getting child 0?
			Debug.Log("ACTIVATING CHILD GAME OBJECT: " + childGameObject.name);
			childGameObject.SetActive(true);
		}

		Ray raycast = new Ray(_controller.transform.position, _controller.transform.forward);
		RaycastHit hit;
		bool bHit = Physics.Raycast(raycast, out hit);

		float dist = 100f;
		if (bHit && hit.distance < 100f)
		{
			dist = hit.distance;
		}

		var currentThickness = _controller.triggerPressed ? thickness * 5f : thickness;

		_laserPointerCube.transform.localScale = new Vector3(currentThickness, currentThickness, dist);
		_laserPointerCube.transform.localPosition = new Vector3(0f, 0f, dist / 2f);
	}

	public Color color;
	public float thickness = 0.002f;

	private GameObject _laserPointerStartPosition;
	private GameObject _laserPointerCube;

	private SteamVR_TrackedController _controller;
	private PlayerAvatar _playerAvatar;

	private bool isActive = false;
}