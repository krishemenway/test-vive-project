using UnityEngine;

class MirrorTransform : MonoBehaviour
{
	public GameObject SourceObject;

	void Start()
	{
		this.transform.SetParent(SourceObject.transform);
	}
}
