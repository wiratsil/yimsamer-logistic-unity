using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;

public class Bezier_Curve : MonoBehaviour
{
	void Update()
	{
		//var m_Angle = AngleBetweenTwoPoints(m_MyFirstVector.transform.position,m_MySecondVector.transform.position) ;

		//m_MyFirstVector.transform.rotation = Quaternion.Euler(new Vector3(0f, 0f, m_Angle + 90));
	//	Debug.LogError(m_Angle);
	}
	public static float AngleBetweenTwoPoints(Vector3 a, Vector3 b)
	{
		return Mathf.Atan2(a.y - b.y, a.x - b.x) * Mathf.Rad2Deg;
	}

	public static Vector3[] GetCurve(Vector3 start, Vector3 middle, Vector3 end)
	{
		int numberOfPoints = 10;
		Vector3[] curvePath = new Vector3[0];
		if (start == null || middle == null || end == null)
		{
			return curvePath; // no points specified
		}

		// set points of quadratic Bezier curve
		Vector3 p0 = start;
		Vector3 p1 = middle;
		Vector3 p2 = end;
		float t;
		Vector3 position;
		curvePath = new Vector3[numberOfPoints];
		for (int i = 0; i < numberOfPoints; i++)
		{
			t = i / (numberOfPoints - 1.0f);
			position = (1.0f - t) * (1.0f - t) * p0
			+ 2.0f * (1.0f - t) * t * p1 + t * t * p2;
			curvePath[i] =(position);
		}
		return curvePath;
	}

}