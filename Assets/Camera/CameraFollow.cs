using UnityEngine;
using UnityEngine.Networking;

public class CameraFollow : NetworkBehaviour
{
	[SerializeField] Vector3 relativePosition;
        
	Camera cam;

	private void Awake()
	{
		cam = Camera.main;
	}

	private void LateUpdate()
	{
		if (!isLocalPlayer)
		{
			return;
		}
		cam.transform.SetParent(this.transform);
		cam.transform.localPosition = relativePosition;
		cam.transform.LookAt(this.transform);
	}
}
