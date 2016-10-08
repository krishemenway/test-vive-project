using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

[RequireComponent(typeof(NetworkManager))]
public class VirtualPongNetworkManagerHud : MonoBehaviour
{
	public VirtualPongNetworkManager manager;

	[SerializeField]
	public bool showGUI = true;

	[SerializeField]
	public int offsetX;

	[SerializeField]
	public int offsetY;

	// Runtime variable
	bool showServer = false;

	void Awake()
	{
		manager = GetComponent<VirtualPongNetworkManager>();
	}

	void Update()
	{
		if (!showGUI)
			return;

		/*
		if (!NetworkClient.active && !NetworkServer.active && manager.matchMaker == null)
		{
			if (Input.GetKeyDown(KeyCode.S))
			{
				manager.StartServer();
			}
			if (Input.GetKeyDown(KeyCode.H))
			{
				manager.StartHost();
			}
			if (Input.GetKeyDown(KeyCode.C))
			{
				manager.StartClient();
			}
		}
		*/

		if (NetworkServer.active && NetworkClient.active)
		{
			if (Input.GetKeyDown(KeyCode.X))
			{
				manager.StopHost();

				Cursor.lockState = CursorLockMode.None;
				Cursor.visible = true;
			}
		}
		else if (NetworkClient.active)
		{
			if (Input.GetKeyDown(KeyCode.X))
			{
				manager.StopClient();

				Cursor.lockState = CursorLockMode.None;
				Cursor.visible = true;
			}
		}
	}

	void OnGUI()
	{
		if (!showGUI)
			return;

		int xpos = 10 + offsetX;
		int ypos = 40 + offsetY;
		int spacing = 24;

		if (!NetworkClient.active && !NetworkServer.active && manager.matchMaker == null)
		{
			if (GUI.Button(new Rect(xpos, ypos, 200, 20), "LAN Host(H)"))
			{
				manager.StartHost();

				Cursor.lockState = CursorLockMode.Locked;
				Cursor.visible = false;
			}
			ypos += spacing;

			if (GUI.Button(new Rect(xpos, ypos, 105, 20), "LAN Client(C)"))
			{
				manager.StartClient();

				Cursor.lockState = CursorLockMode.Locked;
				Cursor.visible = false;
			}
			manager.networkAddress = GUI.TextField(new Rect(xpos + 150, ypos, 200, 20), manager.networkAddress, 64);
			ypos += spacing;
		}
		else
		{
			if (NetworkServer.active)
			{
				GUI.Label(new Rect(xpos, ypos, 300, 20), "Server: port=" + manager.networkPort);
				ypos += spacing;
			}

			if (NetworkClient.active)
			{
				GUI.Label(new Rect(xpos, ypos, 300, 20), "Client: address=" + manager.networkAddress + " port=" + manager.networkPort);
				ypos += spacing;
			}

			GUI.Label(new Rect(xpos, ypos, 300, 20), "To quit, press (X)");
			ypos += spacing;
		}

		if (NetworkClient.active && !ClientScene.ready)
		{
			if (GUI.Button(new Rect(xpos, ypos, 200, 20), "Client Ready"))
			{
				ClientScene.Ready(manager.client.connection);

				if (ClientScene.localPlayers.Count == 0)
				{
					ClientScene.AddPlayer(0);
				}
			}
			ypos += spacing;
		}
	}
}