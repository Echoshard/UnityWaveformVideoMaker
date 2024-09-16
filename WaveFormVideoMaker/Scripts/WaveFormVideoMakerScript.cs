using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;
using System.Collections;
using NaughtyAttributes;
using UnityEditor;

public class WaveFormVideoMakerScript : MonoBehaviour
{
    [Header("Audio For Waveform")]
    [Required]
    public AudioClip audioClip;
    [Header("Optional Hookups")]
    public bool hasTitleCards;
    [ShowIf("hasTitleCards")]
    public Sprite titleCardImage;
    [ShowIf("hasTitleCards")]
    public Sprite endCardImage;
    [ShowIf("hasTitleCards")]
    public float fadeTime = 1;
    [ShowIf("hasTitleCards")]
    [Header("Background")]
    public float stayDuration = 1.5f;
    public Sprite imageBackground;
    public VideoClip loopingVideo;
    public Color fallBackgroundColor = Color.black;


    [Foldout("Already Set")]
    public Image titleCard;
    [Foldout("Already Set")]
    public Image endCard;
    [Foldout("Already Set")]
    public Image imageBackdrop;
    [Foldout("Already Set")]
    public AudioSource audioSource;
    [Foldout("Already Set")]
    public RawImage rawVideoImage;
    [Foldout("Already Set")]
    public VideoPlayer videoPlayer;
    [Foldout("Already Set")]
    public WaveFormUnityRecorder myRecorder;
    CanvasGroup titleCardFader;
    CanvasGroup endCardFader;

    [Header("Debug Area")]
    [ReadOnly]
    public bool isRecording = false;

    [Label("Time Left in Audio")]
    [ReadOnly]
    public float timeLeft;

    public void Start()
    {
        var sessionBool = EditorPrefs.GetBool("EditorBool", false);
        if (sessionBool)
        {
           isRecording = true;
           EditorPrefs.SetBool("EditorBool", false);
        }
        //Setting the framerate down in order to stop stutters
        Application.targetFrameRate = 60;
        SetupTitleCards();
        SetupBackground();
        RunVideo();



    }

    [Button("Start Recording (No Audio Will play When recording)")]
    public void StartRecording()
    {
        EditorPrefs.SetBool("EditorBool", true);
        if (!EditorApplication.isPlaying)
        {
            EditorApplication.isPlaying = true;
        }
    }

    public void OnDisable()
    {

    }

    public void SetupTitleCards()
    {
        if (hasTitleCards)
        {
            titleCardFader = titleCard.GetComponent<CanvasGroup>();
            endCardFader = endCard.GetComponent<CanvasGroup>();

            titleCard.sprite = titleCardImage;
            endCard.sprite = endCardImage;
            titleCardFader.alpha = 1;
            endCardFader.alpha = 0;
        }
        else
        {
            titleCard.gameObject.SetActive(false);
            endCard.gameObject.SetActive(false);
        }
    }

    public void SetupBackground()
    {
        if(imageBackground)
        {
            imageBackdrop.sprite = imageBackground;
            return;
        }
        if(loopingVideo)
        {
            imageBackdrop.gameObject.SetActive(false);
            videoPlayer.clip = loopingVideo;
            videoPlayer.Play();
            return;
        }

        imageBackdrop.color = fallBackgroundColor;
    }

    public void RunVideo()
    {
        StartCoroutine(VideoLooper());
    }

    public IEnumerator VideoLooper()
    {
        if (myRecorder)
        {
            if(isRecording)
            {
                myRecorder.StartCapture();
                Debug.LogError("Capture has Started");
                isRecording = false;
            }
        }
        // Fade in title card if enabled
        if (hasTitleCards)
        {
            // Fade in the title card, wait for the specified duration (stayDuration), then fade out
            StartCoroutine(FadeCanvas(titleCardFader, 1f, 0f, fadeTime, stayDuration));
            yield return new WaitForSeconds(stayDuration - .1f);
            audioSource.clip = audioClip;
            audioSource.loop = false;
            audioSource.Play();
        }
        else
        {
            audioSource.clip = audioClip;
            audioSource.loop = false;
            audioSource.Play();
        }

        // Wait for the audio source to finish
        while (audioSource.isPlaying)
        {
            // Calculate time left
            timeLeft = audioSource.clip.length - audioSource.time;
            yield return null;
        }
        // Fade in the end card if enabled
        if (hasTitleCards && endCardFader != null)
        {
            yield return StartCoroutine(FadeCanvas(endCardFader, 0f, 1f, fadeTime));
        }
        yield return new WaitForSeconds(stayDuration);

        if (myRecorder)
        {
            if (myRecorder.isRecording)
            {
                myRecorder.StopCapture();
            }
        }
        Debug.LogError("WaveFormVideoMakerDone");
    }

    public IEnumerator FadeCanvas(CanvasGroup canvasGroup, float startAlpha, float endAlpha, float duration,float startDelay = 0)
    {
        yield return new WaitForSeconds(startDelay);
        // Set the initial alpha value
        canvasGroup.alpha = startAlpha;

        // Calculate the difference in alpha values
        float alphaDifference = endAlpha - startAlpha;

        // Track the time passed
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            // Increase elapsed time
            elapsedTime += Time.deltaTime;

            // Calculate the new alpha value
            float newAlpha = startAlpha + (alphaDifference * (elapsedTime / duration));
            canvasGroup.alpha = newAlpha;

            // Wait for the next frame
            yield return null;
        }

        // Ensure the final alpha is set
        canvasGroup.alpha = endAlpha;
    }
}