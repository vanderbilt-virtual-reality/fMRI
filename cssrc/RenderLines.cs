using UnityEngine;
using System.Collections;

public class GenerateLine : MonoBehavior {

	private LineRenderer lineRenderer;
	private float cov;

	public Transform origin;
	public Transform destination;

	void Start() {
		lineRenderer = GetComponent<lineRenderer>();
		lineRenderer.SetPosition(0, origin.position);
		lineRenderer.SetPosition(1, origin.position);
		int covMax = 19126;
		int covMin = -3135;
		double gVal = 255 * Math.floor((cov - covMin) / (covMax - covMin))
		lineRenderer.material.color = new Color32(114, 0, gVal, 255)
	}
}