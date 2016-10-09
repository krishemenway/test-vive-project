using System.Linq;
using UnityEngine;
using UnityEngine.Networking;

public class VRPawn : NetworkBehaviour
{
	public GameObject Head;

	void Start()
	{
		/*
		GetComponentInChildren<SteamVR_ControllerManager>(true).enabled = isLocalPlayer;
		GetComponentInChildren<SteamVR_PlayArea>(true).enabled = isLocalPlayer;
		GetComponentsInChildren<SteamVR_TrackedObject>(true).ToList().ForEach(x => x.enabled = isLocalPlayer);
		GetComponentsInChildren<SteamVR_Teleporter>(true).ToList().ForEach(x => x.enabled = isLocalPlayer);
		GetComponentsInChildren<SteamVR_LaserPointer>(true).ToList().ForEach(x => x.enabled = isLocalPlayer);
		GetComponentsInChildren<WandController>(true).ToList().ForEach(x => x.enabled = isLocalPlayer);
		GetComponentsInChildren<Camera>(true).ToList().ForEach(x => x.enabled = isLocalPlayer);
		Head.SetActive(isLocalPlayer);
		*/
	}

	void OnDestroy()
	{
	}
}
