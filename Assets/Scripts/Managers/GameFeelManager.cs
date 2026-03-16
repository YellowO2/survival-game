using System.Collections;
using UnityEngine;

public class GameFeelManager : MonoBehaviour
{
    public static GameFeelManager Instance { get; private set; }

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
}
