using UnityEngine;

public class AimConeIndicator : MonoBehaviour
{
    public Transform origin;
    public LineRenderer leftLine;

    public float maxAimLength = 8f;
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
        Vector3 end = start + (Vector3)(aim * maxAimLength);

        RaycastHit2D hit = Physics2D.CircleCast(start, 0.5f, aim, maxAimLength, collisionMask); //perhaps should have a constant term for radius instead of 1

        if (hit.collider != null) //if hit something
        {
            end = hit.centroid;
        }

        leftLine.positionCount = 2;
        leftLine.SetPosition(0, start);
        leftLine.SetPosition(1, end);

        //now lets draw the reflected line
        //the reflection is different for collision with ball vs collision with static wall
        if (hit.collider != null && !hit.collider.CompareTag("Enemy")) //if hit wall
        {
            Vector2 reflectedDir = Vector2.Reflect(aim, hit.normal).normalized;
            Vector2 reflectedEnd = hit.centroid + (Vector2)(reflectedDir * maxAimLength);

            // // Check for collisions along the reflected path
            RaycastHit2D reflectedHit = Physics2D.CircleCast(hit.point, 0.5f, reflectedDir, maxAimLength, collisionMask);
            if (reflectedHit.collider != null)
            {
                reflectedEnd = reflectedHit.centroid;
            }

            // Draw the reflected line
            leftLine.positionCount = 3; // We need 3 points to draw the original and reflected lines
            leftLine.SetPosition(2, reflectedEnd);
        }else if (hit.collider != null && hit.collider.CompareTag("Enemy")) //if hit enemy, which is a ball
        {
            // enemy reflects 90 degrees to the line connecting centers of the enemy.
            Vector2 reflectedDir = Vector2.Perpendicular(hit.centroid - (Vector2)origin.position).normalized;
            Vector2 reflectedEnd = hit.centroid + (Vector2)(reflectedDir * maxAimLength);
            leftLine.positionCount = 3;
            leftLine.SetPosition(2, reflectedEnd);
        }
    }
}