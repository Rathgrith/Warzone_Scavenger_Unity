using System.Collections;
using UnityEngine;

public class ExplosionHandler : MonoBehaviour
{
    [SerializeField]
    private float maxScale = 10f; // The maximum scale of the explosion
    [SerializeField]
    private float growDuration = 0.2f; // The duration of the explosion growth
    [SerializeField]
    private float shrinkDuration = 1f; // The duration of the explosion shrink
    private float shakeDuration = 0.5f; // How long the shake should last
    [SerializeField]
    private float shakeMagnitude = 0.1f; // How much the camera should shake
    private Vector3 initialScale;

    private Transform cameraTransform; // The camera's transform
    private Vector3 cameraInitialPosition; // The camera's initial position
    private void Start()
    {
        initialScale = transform.localScale;
        cameraTransform = Camera.main.transform;
        cameraInitialPosition = cameraTransform.position;
        StartCoroutine(Explode());
    }
    private void Update() {
        cameraTransform = Camera.main.transform;
        cameraInitialPosition = cameraTransform.position;
    }
    IEnumerator ShakeCamera(float duration, float magnitude)
    {
        float elapsed = 0.0f;

        while (elapsed < duration)
        {
            float x = Random.Range(-1f, 1f) * magnitude;
            float y = Random.Range(-1f, 1f) * magnitude;

            cameraTransform.position = new Vector3(cameraInitialPosition.x + x, cameraInitialPosition.y + y, cameraInitialPosition.z);

            elapsed += Time.deltaTime;

            yield return null;
        }

        cameraTransform.position = cameraInitialPosition;
    }
    IEnumerator Explode()
    {
        // Grow phase
        float timer = 0;
        while (timer <= growDuration)
        {
            float progress = timer / growDuration;
            transform.localScale = Vector3.Lerp(initialScale, initialScale * maxScale, progress);
            timer += Time.deltaTime;
            yield return null;
        }
        StartCoroutine(ShakeCamera(shakeDuration, shakeMagnitude));
        // Wait phase
        yield return new WaitForSeconds(0.5f);

        // Shrink phase
        timer = 0;
        while (timer <= shrinkDuration)
        {
            float progress = timer / shrinkDuration;
            transform.localScale = Vector3.Lerp(initialScale * maxScale, initialScale, progress);
            timer += Time.deltaTime;
            yield return null;
        }

        // Reset scale to initial scale
        transform.localScale = initialScale;
        Destroy(this.gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            // Debug.Log("Hit");
            GameStat.Instance.onPause(2);
        }

    }
}
