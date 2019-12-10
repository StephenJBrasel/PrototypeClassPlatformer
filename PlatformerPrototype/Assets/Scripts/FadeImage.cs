using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeImage : MonoBehaviour
{
	[Range(0, 1)] [SerializeField] private float startAlpha = 0.673f;
	[SerializeField] private Image image;
	[SerializeField] private float fadeSpeed = 0.9f;

    void Awake()
    {
		if(image == null)
		{
			image = GetComponent<Image>();
		}

		Color tempColor = image.color;
		tempColor.a = 0f;
		image.color = tempColor;
    }

    // Update is called once per frame
    void Update()
    {
		Color tempColor = image.color;
		if(tempColor.a > 0f)
		{
			float tempA = tempColor.a - (fadeSpeed * Time.unscaledDeltaTime);
			tempColor.a = Mathf.Clamp(tempA, 0f, 1f);
			image.color = tempColor;
		}
	}

	public void ResetAlpha()
	{
		Color tempColor = image.color;
		tempColor.a = startAlpha;
		image.color = tempColor;
	}
}
