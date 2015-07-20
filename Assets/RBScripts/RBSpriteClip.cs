using UnityEngine;
using System.Collections;

namespace RedBlueTools
{
	[RequireComponent (typeof(SpriteRenderer))]
	public class RBSpriteClip : MonoBehaviour
	{
		public string ClipName;
		public float SpeedMultiplier = 1.0f;
		public bool Looping = true;
		public bool PlayOnStart;
		public RBSpriteFrame[] SpriteFrames;

		int SampleRate = 30;
		[SerializeField]
		[ReadOnly]
		RBSpriteFrame curSpriteFrame;

		int curSpriteIndex;
		SpriteRenderer spriteRenderer;
		bool isPlaying = false;
		float timeSinceSpriteStart = 0;

		public void Start ()
		{
			spriteRenderer = GetComponent<SpriteRenderer>();
			InitializeClip();

			if(PlayOnStart) {
				Play ();
			}
		}

		public void Update ()
		{
			if (!isPlaying) {
				return;
			}
			timeSinceSpriteStart += Time.deltaTime;
			float spriteDurationInSeconds = ((float) curSpriteFrame.durationInFrames / SampleRate) / SpeedMultiplier;
			if (timeSinceSpriteStart >= spriteDurationInSeconds) {
				NextSprite();
				timeSinceSpriteStart = timeSinceSpriteStart - spriteDurationInSeconds;
			}
		}

		public void Play ()
		{
			isPlaying = true;
		}

		public void Play (int frameDelay)
		{
			StartCoroutine (PlayDelayed(frameDelay));
		}

		public void Pause ()
		{
			isPlaying = false;
			StopAllCoroutines();
		}

		public void Stop ()
		{
			Pause ();
			gameObject.SetActive (false);
		}

		void NextSprite ()
		{
			curSpriteIndex = (curSpriteIndex + 1) % SpriteFrames.Length;
			if (!Looping && curSpriteIndex == 0) {
				Stop ();
			}
			RBSpriteFrame nextSpriteFrame = SpriteFrames[curSpriteIndex];
			if (nextSpriteFrame.durationInFrames > 0) {
				curSpriteFrame = nextSpriteFrame;
				spriteRenderer.sprite = curSpriteFrame.sprite;
			}
		}

		void InitializeClip ()
		{
			curSpriteIndex = 0;
			curSpriteFrame = SpriteFrames[curSpriteIndex];
			spriteRenderer.sprite = curSpriteFrame.sprite;
		}

		IEnumerator PlayDelayed (int framesToDelay)
		{
			for (int i = 0; i < framesToDelay; ++i) {
				yield return null;
			}
			Play();
		}
	}

	[System.Serializable]
	public class RBSpriteFrame
	{
		[SerializeField]
		public int durationInFrames = 30;

		[SerializeField]
		public Sprite sprite;
	}
}