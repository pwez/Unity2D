using UnityEngine;

public class ControllerInput : MonoBehaviour {
		
	[Header("Input Axes")] 
	public string horizontalAxis = "Horizontal";
	public string verticalAxis = "Vertical";
		
	[Header("Horizontal Input")]
	[HideInInspector] public int dx;
	public float x;
	public bool right; 
	public bool left;
	
	[Header("Vertical Input")]
	[HideInInspector] public int dy;
	public float y;
	public bool up; 
	public bool down;

	[Header("Jump Command Input")] 
	public bool commandHeld;
	public bool commandPressed;
	public bool commandReleased;

	[Header("Override")] 
	public bool overrideDirectionalInput;
	public Vector2 overrideInput;
	
	void Update() {
		x = overrideDirectionalInput ? overrideInput.x : Input.GetAxisRaw(horizontalAxis);
		y = overrideDirectionalInput ? overrideInput.y : Input.GetAxisRaw(verticalAxis);

        int v = x > 0 ? 1 : x < 0 ? -1 : 0;
        dx = v;
		dy = y > 0 ? 1 : y < 0 ? -1 : 0;
		
		right = x > 0;
		left = x < 0;
		up = y > 0;
		down = y < 0;

		if (Input.GetButtonDown("Jump")) {
			commandPressed = true;
			commandHeld = false;
			commandReleased = false;
		}
		else if (Input.GetButton("Jump")) {
			commandPressed = false;
			commandHeld = true;
			commandReleased = false;
		}
		else if (Input.GetButtonUp("Jump")) {
			commandPressed = false;
			commandHeld = false;
			commandReleased = true;
		}
		else 
			commandHeld = commandPressed = commandReleased = false;
	}
}