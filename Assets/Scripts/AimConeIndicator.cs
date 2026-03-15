using UnityEngine;

public class AimConeIndicator : MonoBehaviour
{
    public Transform origin;
    public LineRenderer leftLine;

    public float aimLength = 8f;
    public LayerMask collisionMask = ~0;

    private bool isCharging;

    public void showAim(bool charging)
    {
        print("showAim: " + charging);
        isCharging = charging;
        if (leftLine != null)
        {
            leftLine.enabled = charging;
        }
    }

    void Update()
    {
        if (!isCharging) return;

        Vector2 aim = InputManager.Instance.aimDirection;
        if (aim.sqrMagnitude < 0.0001f) aim = origin.up;
        aim.Normalize();

        Vector3 start = origin.position;
        Vector3 end = start + (Vector3)(aim * aimLength);

        // Stop the line at first collision so aiming feels more like a pool guideline.
        RaycastHit2D hit = Physics2D.Raycast(start, aim, aimLength, collisionMask);
        if (hit.collider != null)
        {
            end = hit.point;
        }

        leftLine.positionCount = 2;
        leftLine.SetPosition(0, start);
        leftLine.SetPosition(1, end);
        //now lets draw the reflected line if we hit something
        if (hit.collider != null)
        {
            Vector2 incomingVec = end - start;
            Vector2 normalVec = hit.normal;
            Vector2 reflectedVec = Vector2.Reflect(incomingVec, normalVec).normalized;

            Vector2 reflectedEnd = hit.point + (Vector2)(reflectedVec * aimLength);

            // Check for collisions along the reflected path
            RaycastHit2D reflectedHit = Physics2D.Raycast(hit.point, reflectedVec, aimLength, collisionMask);
            if (reflectedHit.collider != null)
            {
                reflectedEnd = reflectedHit.point;
            }

            // Draw the reflected line
            leftLine.positionCount = 3; // We need 3 points to draw the original and reflected lines
            leftLine.SetPosition(2, reflectedEnd);
        }
    }
}