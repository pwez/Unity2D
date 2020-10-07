namespace Physics {
	public interface IPhysics {
		void ApplyGravity();
		void ApplyGravity(float gravity);
		void AccelerateX(float acceleration, float boundingValue);
	}
}