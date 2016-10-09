using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public static class GameObjectExtensions
{
	public static void AttachToParent(this GameObject gameObject, Transform parent)
	{
		gameObject.transform.SetParent(parent);
		gameObject.transform.localRotation = Quaternion.identity;
		gameObject.transform.localPosition = Vector3.zero;
	}

	public static void AttachDebugCube(this GameObject gameObject)
	{
		var newMaterial = new Material(Shader.Find("Unlit/Color"));
		newMaterial.SetColor("_Color", Color.blue);

		var box = GameObject.CreatePrimitive(PrimitiveType.Cube);

		box.transform.parent = gameObject.transform;
		box.transform.localScale = new Vector3(0.25f, 0.25f, 0.25f);
		box.transform.localPosition = Vector3.zero;
		box.transform.localRotation = Quaternion.identity;

		box.GetComponent<MeshRenderer>().material = newMaterial;

		var collider = box.GetComponent<BoxCollider>();
		if (collider)
		{
			UnityEngine.Object.Destroy(collider);
		}
	}
}
