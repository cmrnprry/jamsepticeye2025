using AYellowpaper.SerializedCollections;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Util : MonoBehaviour
{
	public void PlayMenuClick()
	{
		AudioManager.instance.PlaySFX("stab");
		AudioManager.instance.PlaySFX("click");
	}

	public void PlayFire()
	{
		AudioManager.instance.PlaySFX("fire");
	}
}
