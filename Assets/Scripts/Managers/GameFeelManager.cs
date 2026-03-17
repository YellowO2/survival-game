using System.Collections;
using UnityEngine;

public class GameFeelManager : MonoBehaviour
{
    public static GameFeelManager Instance { get; private set; }

    public ParticleSystem hitParticlesPrefab;
    public ParticleSystem deathParticlesPrefab;

    private bool isHitStopping = false;
    private Coroutine cameraShakeCoroutine;
    private Vector3 originalCameraPos;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    /// <summary>
    /// Freezes or slows down time for a tiny fraction of a second to make hits feel heavy.
    /// </summary>
    public void HitStop(float duration = 0.15f)
    {
        if (isHitStopping) return;
        StartCoroutine(HitStopRoutine(duration));
    }

    private IEnumerator HitStopRoutine(float duration)
    {
        isHitStopping = true;
        float originalTimeScale = Time.timeScale;
        Time.timeScale = 0.02f; // slow down alot for the short duration specified.
        yield return new WaitForSecondsRealtime(duration);

        // Restore time scale
        Time.timeScale = originalTimeScale;
        isHitStopping = false;
    }

    public void ShakeCamera(float duration = 0.1f, float magnitude = 0.1f)
    {
        if (Camera.main == null) return;

        if (cameraShakeCoroutine != null)
        {
            StopCoroutine(cameraShakeCoroutine);
            Camera.main.transform.localPosition = originalCameraPos;
        }
        else
        {
            originalCameraPos = Camera.main.transform.localPosition;
        }

        cameraShakeCoroutine = StartCoroutine(CameraShakeRoutine(duration, magnitude));
    }

    private IEnumerator CameraShakeRoutine(float duration, float magnitude)
    {
        float elapsed = 0f;

        while (elapsed < duration)
        {
            float x = originalCameraPos.x + Random.Range(-1f, 1f) * magnitude;
            float y = originalCameraPos.y + Random.Range(-1f, 1f) * magnitude;

            Camera.main.transform.localPosition = new Vector3(x, y, originalCameraPos.z);

            elapsed += Time.unscaledDeltaTime;
            yield return null;
        }

        Camera.main.transform.localPosition = originalCameraPos;
        cameraShakeCoroutine = null;
    }

    public void SpawnHitEffect(Vector2 position, Color color)
    {
        if (hitParticlesPrefab == null) return;
        ParticleSystem ps = Instantiate(hitParticlesPrefab, position, Quaternion.identity);

        var main = ps.main;
        main.startColor = color;
        main.startLifetime = 0.12f; // hit spark short
        main.startSize = 0.08f;     // smaller particles
        main.startSpeed = 2.8f;     // quick pop

        Destroy(ps.gameObject, main.startLifetime.constantMax);
    }

    public void SpawnDeathEffect(Vector2 position, Color color)
    {
        if (deathParticlesPrefab == null) return;
        ParticleSystem ps = Instantiate(deathParticlesPrefab, position, Quaternion.identity);

        var main = ps.main;
        var emission = ps.emission;

        main.startColor = color;
        main.startLifetime = 0.22f;
        main.startSize = 0.12f;
        main.startSpeed = 4.0f;

        emission.SetBursts(new ParticleSystem.Burst[] {
        new ParticleSystem.Burst(0f, 18)
        });
        Destroy(ps.gameObject, main.startLifetime.constantMax);
    }
}
