using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class SceneFader : MonoBehaviour
{
    public bool fadedIn;
    [SerializeField] Image m_mask;
    [SerializeField] AnimationCurve m_curve;
    [SerializeField] float fadeInDuration = 1.2f;
    [SerializeField] float fadeOutDuration = 0.8f;
    
    void Start()
    {
        StartCoroutine(FadeIn());
        fadedIn = false;
    }
    
    public IEnumerator FadeToScene(int sceneIndex)
    {
        StartCoroutine(FadeOut());
        yield return new WaitForSeconds(fadeOutDuration);
        SceneManager.LoadScene(sceneIndex);
    }

    IEnumerator FadeIn()
    {
        float t = fadeInDuration;
        while (t > 0.0f)
        {
            t -= Time.unscaledDeltaTime;
            float a = m_curve.Evaluate(t / fadeInDuration);
            m_mask.color = new Color(0f, 0f, 0f, a);
            if (a<=0)
            {
                fadedIn = true;
            }
            yield return 0;
        }
    }

    IEnumerator FadeOut()
    {
        fadedIn = false;
        float t = 0f;

        while (t < fadeOutDuration)
        {
            t += Time.unscaledDeltaTime;
            float a = m_curve.Evaluate(t / fadeOutDuration);
            m_mask.color = new Color(0f, 0f, 0f, a);
            yield return 0;
        }
    }

    public IEnumerator FadeOutAndIn()
    {
        StartCoroutine(FadeOut());
        yield return new WaitForSeconds(fadeOutDuration);
        StartCoroutine(FadeIn());
    }
}
