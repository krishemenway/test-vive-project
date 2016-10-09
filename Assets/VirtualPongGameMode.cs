using UnityEngine;
using UnityEngine.Networking;

public class VirtualPongGameMode : NetworkBehaviour
{
	void OnDisconnectedFromServer(NetworkDisconnection info)
	{
		/*
		Camera.main.transform.parent = GameObject.Find("CameraStartPosition").transform;
		Camera.main.transform.localRotation = Quaternion.identity;
		Camera.main.transform.localPosition = Vector3.zero;
		*/
	}
}