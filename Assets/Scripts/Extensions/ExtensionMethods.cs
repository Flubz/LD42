using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// I am reusing most of this script from GHAP 2017-2018.
public static class ExtensionMethods
{
	public static void RotateTowardsVector (this Transform trans_, Vector3 vel_, float rotSpeed_ = 4.0f, float angleOffset_ = 0.0f)
	{
		float angle = Mathf.Atan2 (vel_.y, vel_.x) * Mathf.Rad2Deg;
		trans_.rotation = Quaternion.Lerp (trans_.rotation, Quaternion.AngleAxis (angle + angleOffset_, Vector3.up), Time.deltaTime * rotSpeed_);
	}

	public static Vector3 ToInt (this Vector3 vec_)
	{
		vec_.x.ToInt ();
		vec_.y.ToInt ();
		vec_.z.ToInt ();
		return vec_;
	}

	public static int ToInt (this float float_)
	{
		int i = Mathf.RoundToInt (float_);
		return i;
	}

	public static bool IsEven (this int int_)
	{
		if (int_ % 2 != 0) return false;
		return true;
	}

	public static T RandomFromList<T> (this List<T> list_)
	{
		return list_[UnityEngine.Random.Range (0, list_.Count)];
	}

	public static float Remap (this float value_, float min_, float max_, float newMin_, float newMax_)
	{
		return (value_ - min_) / (max_ - min_) * (newMax_ - newMin_) + newMin_;
	}

	public static Vector3 Remap (this Vector3 value_, float min_, float max_, float newMin_, float newMax_)
	{
		return value_ = new Vector3 (Remap (value_.x, min_, max_, newMin_, newMax_),
			Remap (value_.y, min_, max_, newMin_, newMax_),
			Remap (value_.z, min_, max_, newMin_, newMax_));
	}

	private static System.Random rng = new System.Random ();

	public static void Shuffle<T> (this IList<T> list)
	{
		int n = list.Count;
		while (n > 1)
		{
			n--;
			int k = rng.Next (n + 1);
			T value = list[k];
			list[k] = list[n];
			list[n] = value;
		}
	}

	public static bool NearlyEqual (float valA_, float valB_, float acceptableDifference_)
	{
		return Mathf.Abs (valA_ - valB_) <= acceptableDifference_;
	}
}