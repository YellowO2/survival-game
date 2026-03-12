using UnityEngine;

public class AimConeIndicator : MonoBehaviour
{
    public Transform origin;
    public LineRenderer leftLine;
    public LineRenderer rightLine;

    public float chargeDuration = 1.2f;
    public float startHalfAngle = 35f;
    public float coneLength = 6f;

    private bool isCharging;
    private float chargeTimer;

    public void SetCharging(bool charging)
    {
        isCharging = charging;
        leftLine.enabled = charging;
        rightLine.enabled = charging;

        if (!charging)
        {
            chargeTimer = 0f;
        }
    }

    void Update()
    {
        if (!isCharging || origin == null) return;

        chargeTimer += Time.deltaTime;
        float t = Mathf.Clamp01(chargeTimer / chargeDuration);
        float halfAngle = Mathf.Lerp(startHalfAngle, 0f, t);

        Vector2 aim = InputManager.Instance.aimDirection;
        if (aim.sqrMagnitude < 0.0001f) aim = origin.up;
        aim.Normalize();

        Vector2 leftDir = Quaternion.Euler(0f, 0f, halfAngle) * aim;
        Vector2 rightDir = Quaternion.Euler(0f, 0f, -halfAngle) * aim;

        Vector3 p0 = origin.position;
        leftLine.positionCount = 2;
        rightLine.positionCount = 2;
        leftLine.SetPosition(0, p0);
        leftLine.SetPosition(1, p0 + (Vector3)(leftDir * coneLength));
        rightLine.SetPosition(0, p0);
        rightLine.SetPosition(1, p0 + (Vector3)(rightDir * coneLength));
    }
}