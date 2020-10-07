using JetBrains.Annotations;
using UnityEngine;

public abstract class Character : MonoBehaviour {
	[HideInInspector] [CanBeNull] public AudioSource audioSource;
	[HideInInspector] [CanBeNull] public Animator animator;
}