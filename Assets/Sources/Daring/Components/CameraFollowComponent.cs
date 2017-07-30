using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollowComponent : MonoBehaviour
{
    public GameObject Reference;

	private void Update()
    {
		transform.position = new Vector3(Reference.transform.position.x,
                                         Reference.transform.position.y,
                                         transform.position.z);
	}
}
