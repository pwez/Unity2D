using UnityEngine;

namespace Interactive {
	public class PendulumMotion : MonoBehaviour {

		public float angle = 40.0f;
		public float speed = 1.5f;
     
		private Quaternion _start, _end;

		private void Start () {
			_start = Quaternion.AngleAxis ( angle, Vector3.forward);
			_end   = Quaternion.AngleAxis (-angle, Vector3.forward);
		}

		private void Update () {
			transform.rotation = Quaternion.Lerp (
				_start, 
				_end, 
				(Mathf.Sin(Time.time * speed) + 1.0f) / 2.0f
			);
		}
	}
}