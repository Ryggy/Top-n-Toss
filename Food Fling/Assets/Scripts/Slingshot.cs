using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.EnhancedTouch;
using Touch = UnityEngine.InputSystem.EnhancedTouch.Touch;

public class Slingshot : MonoBehaviour
{
    public float launchForceMultiplier = 10f; // Adjust the force applied when launching
    public Rigidbody2D rb;

    private Vector2 resetPosition;
    
    private Vector2 startPoint;
    private Vector2 dragPoint;
    private bool isDragging = false;

    private void OnEnable()
    {
        EnhancedTouchSupport.Enable();
        Touch.onFingerDown += OnFingerDown;
        Touch.onFingerMove += OnFingerMove;
        Touch.onFingerUp += OnFingerUp;
    }

    private void OnDisable()
    {
        Touch.onFingerDown -= OnFingerDown;
        Touch.onFingerMove -= OnFingerMove;
        Touch.onFingerUp -= OnFingerUp;
        EnhancedTouchSupport.Disable();
    }

    private void Start()
    {
        if (rb == null)
        {
            rb = GetComponent<Rigidbody2D>();
        }

        resetPosition = transform.position;
        rb.isKinematic = true; // Ensure the object doesn't move until launched
    }

    private void Update()
    {
        // Handle mouse input in the Editor
        if (Mouse.current != null)
        {
            if (Mouse.current.leftButton.wasPressedThisFrame)
            {
                Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
                OnPointerDown(mousePosition);
            }

            if (isDragging && Mouse.current.leftButton.isPressed)
            {
                dragPoint = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
                Debug.Log($"Mouse moved to: {dragPoint}");
            }

            if (isDragging && Mouse.current.leftButton.wasReleasedThisFrame)
            {
                OnPointerUp();
            }
        }
    }

    private void OnFingerDown(Finger finger)
    {
        Vector2 touchPosition = Camera.main.ScreenToWorldPoint(finger.screenPosition);
        HandleInputStart(touchPosition);
    }

    private void OnFingerMove(Finger finger)
    {
        if (isDragging)
        {
            dragPoint = Camera.main.ScreenToWorldPoint(finger.screenPosition);
            Debug.Log($"Touch moved to: {dragPoint}");
        }
    }

    private void OnFingerUp(Finger finger)
    {
        HandleInputEnd();
    }

    private void OnPointerDown(Vector2 inputPosition)
    {
        HandleInputStart(inputPosition);
    }

    private void OnPointerUp()
    {
        HandleInputEnd();
    }

    private void HandleInputStart(Vector2 inputPosition)
    {
        Debug.Log($"Input started at: {inputPosition}");
        if (IsTouchingObject(inputPosition))
        {
            isDragging = true;
            startPoint = inputPosition;
            Debug.Log("Dragging started on the object.");
        }
    }

    private void HandleInputEnd()
    {
        if (isDragging)
        {
            isDragging = false;
            Vector2 launchDirection = (startPoint - dragPoint).normalized;
            float launchDistance = Vector2.Distance(startPoint, dragPoint);
            Debug.Log($"Input ended. Launching object with direction: {launchDirection} and distance: {launchDistance}");
            LaunchObject(launchDirection, launchDistance);
        }
        else
        {
            Debug.Log("Input ended but no drag was detected.");
        }
    }

    private bool IsTouchingObject(Vector2 worldPosition)
    {
        Collider2D collider = Physics2D.OverlapPoint(worldPosition);
        bool isTouching = collider != null && collider.gameObject == gameObject;
        Debug.Log(isTouching ? "Input is on the object." : "Input is not on the object.");
        return isTouching;
    }

    private void LaunchObject(Vector2 direction, float distance)
    {
        rb.isKinematic = false; // Allow the object to move
        rb.velocity = direction * distance * launchForceMultiplier;
        Debug.Log($"Object launched with velocity: {rb.velocity}");
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Bouncing logic handled automatically by Rigidbody2D physics

        // Check if the object collides with a "Customer" tagged object
        if (collision.gameObject.CompareTag("Customer"))
        {
            Debug.Log("Hit a customer!");
            Reset();
        }
        else
        {
            Debug.Log($"Collided with: {collision.gameObject.name}");
        }
    }

    private void Reset()
    {
        rb.isKinematic = true; // Stop physics interactions temporarily
        transform.position = resetPosition; // Reset position
        transform.rotation = Quaternion.Euler(0, 0, 0); // Reset rotation
        rb.velocity = Vector2.zero; // Stop linear velocity
        rb.angularVelocity = 0f; // Stop rotational velocity
        rb.rotation = 0f; // Ensure rotation angle is zero
    }
}
