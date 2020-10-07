using System;
using UnityEngine;

namespace Physics.Collisions {
	[Serializable]
	public class CollisionBounds {
		public Bounds bounds;
		public Vector2 offset;
	}
}