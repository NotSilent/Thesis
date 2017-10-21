using UnityEngine;
using UnityEngine.Networking;

public class CameraFollow : NetworkBehaviour
{
	[SerializeField] Vector3 relativePosition;

	Camera mainCamera;

	private void Awake()
	{
		mainCamera = Camera.main;
	}

	private void LateUpdate()
	{
		if (!isLocalPlayer)
		{
			return;
		}
		mainCamera.transform.SetParent(this.transform);
		mainCamera.transform.localPosition = relativePosition;
		mainCamera.transform.LookAt(this.transform);
	}
}
