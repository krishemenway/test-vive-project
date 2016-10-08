using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class VirtualPongNetworkManager : NetworkManager {

	void Start()
	{
		spawnPoints = FindObjectsOfType<NetworkStartPosition>();
	}

	public override void OnServerAddPlayer(NetworkConnection conn, short playerControllerId)
	{
		Transform playerSpawnPos = null;

		// If there is a spawn point array and the array is not empty, pick a spawn point at random
		if (spawnPoints != null && spawnPoints.Length > 0)
		{
			playerSpawnPos = spawnPoints[Random.Range(0, spawnPoints.Length)].transform;
		}

		var player = (GameObject)Instantiate(playerPrefab, playerSpawnPos != null ? playerSpawnPos.transform.position : Vector3.zero, playerSpawnPos != null ? playerSpawnPos.transform.rotation : Quaternion.identity);

		NetworkServer.AddPlayerForConnection(conn, player, playerControllerId);
	}

	public VirtualPongGameMode GameMode;

	private NetworkStartPosition[] spawnPoints;
}
