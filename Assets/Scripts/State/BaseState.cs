using System.Collections;
using JetBrains.Annotations;
using UnityEngine;

namespace State {
	public abstract class BaseState : MonoBehaviour, State {

		[Header("Audio & Animation")]
		public AudioClip audioClip;
		public AnimationClip animationClip;

		[HideInInspector] public SpriteRenderer spriteRenderer;
		[HideInInspector] [CanBeNull] public Animator animator;
		[HideInInspector] [CanBeNull] public AudioSource audioSource;

		public virtual void Awake() {
			var spriteContainer = transform.parent.parent.GetChild(0);
			spriteRenderer = spriteContainer.GetComponent<SpriteRenderer>();
			animator = spriteContainer.GetComponent<Animator>();
			audioSource = spriteContainer.GetComponent<AudioSource>();
		}
		
		public virtual void Enter(Player player) {
			PlayAnimation();
		}

		public void PlayAnimation() {
			if (audioSource && audioClip) {
				audioSource.clip = audioClip;
				audioSource.Play();
			}

			if (animator && animationClip) {
				animator.enabled = true;
				animator.Play(animationClip.name);
			}
		}

		public virtual void Resume(Player player) {
			CheckToFlipLocalScale(player);
		}

		public virtual void Exit(Player player) {}

		protected void ContinueAnimation(bool state = true) {
			if (animator)
				animator.enabled = state;
		}

		protected static void CheckToFlipLocalScale(Player player) {
			var directionFacing = player.physics.facingDirection;
			var directionMoving = player.physics.movingDirection;
			if (Mathf.Abs(player.input.x) > 0 && directionFacing == -directionMoving) {
				player.physics.facingDirection = -directionFacing;
				var element = player.transform;
				var localScale = element.localScale;
				localScale = new Vector3(-localScale.x, localScale.y, 0f);
				element.localScale = localScale;
			}
		}

		protected static void FlipScale(Player player) {
			var directionFacing = player.physics.facingDirection;
			player.physics.facingDirection = -directionFacing;
			var element = player.transform;
			var localScale = element.localScale;
			localScale = new Vector3(-localScale.x, localScale.y, 0f);
			element.localScale = localScale;
		}

		protected IEnumerator Squeeze(Vector2 amount, float duration) {
			var originalSize = Vector2.one;
			var squeezeSize = new Vector2(amount.x, amount.y);
			transform.parent.parent.GetChild(0).transform.localScale = squeezeSize;
			
			var time = 0f;
			while (time <= 1.0f) {
				time += Time.deltaTime / duration;
				transform.parent.parent.GetChild(0).transform.localScale = Vector2.Lerp(squeezeSize, originalSize, time);
				yield return null;
			}
		}
		
	}
}