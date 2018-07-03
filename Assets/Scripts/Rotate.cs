using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotate : MonoBehaviour
{
	[SerializeField]
	Vector3 rotatePerSecond;

	private void Update()
	{
		transform.Rotate(rotatePerSecond * Time.deltaTime);
	}
}
