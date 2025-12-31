using Mercury;
using Mercury.Menu;
using UnityEngine;
using UnityEngine.Video;

public class BadAppleMonkey : MonoBehaviour
{
    private VideoClip videoClip;
    private RenderTexture renderTexture;
    private VideoPlayer videoPlayer;
    private AudioSource audioSource;
    private Material videoMaterial;

    private Material[] originalMaterials;
    private Renderer[] renderers;

    private bool isPlaying;

    void Update()
    {
        if (PluginConfig.badapplemonkey && !isPlaying)
        {
            StartBadApple();
        }
        else if (!PluginConfig.badapplemonkey && isPlaying)
        {
            StopBadApple();
        }
    }

    void StartBadApple()
    {
        if (AssetBundleLoader.bundle == null)
            return;

        videoClip = AssetBundleLoader.bundle.LoadAsset<VideoClip>("badapple");
        if (videoClip == null)
            return;

        renderTexture = new RenderTexture(1920, 1080, 16);
        videoMaterial = new Material(Shader.Find("Unlit/Texture"));
        videoMaterial.mainTexture = renderTexture;

        renderers = FindObjectsOfType<Renderer>();
        originalMaterials = new Material[renderers.Length];

        for (int i = 0; i < renderers.Length; i++)
        {
            originalMaterials[i] = renderers[i].material;
            renderers[i].material = videoMaterial;
        }

        videoPlayer = gameObject.AddComponent<VideoPlayer>();
        audioSource = gameObject.AddComponent<AudioSource>();

        videoPlayer.playOnAwake = false;
        videoPlayer.source = VideoSource.VideoClip;
        videoPlayer.clip = videoClip;
        videoPlayer.renderMode = VideoRenderMode.RenderTexture;
        videoPlayer.targetTexture = renderTexture;
        videoPlayer.audioOutputMode = VideoAudioOutputMode.AudioSource;
        videoPlayer.SetTargetAudioSource(0, audioSource);

        videoPlayer.prepareCompleted += OnPrepared;
        videoPlayer.Prepare();

        isPlaying = true;
    }

    void OnPrepared(VideoPlayer vp)
    {
        vp.Play();
    }

    void StopBadApple()
    {
        isPlaying = false;

        if (videoPlayer != null)
        {
            videoPlayer.prepareCompleted -= OnPrepared;
            videoPlayer.Stop();
            Destroy(videoPlayer);
            videoPlayer = null;
        }

        if (audioSource != null)
        {
            audioSource.Stop();
            Destroy(audioSource);
            audioSource = null;
        }

        if (renderTexture != null)
        {
            renderTexture.Release();
            Destroy(renderTexture);
            renderTexture = null;
        }

        if (videoMaterial != null)
        {
            Destroy(videoMaterial);
            videoMaterial = null;
        }

        if (renderers != null && originalMaterials != null)
        {
            for (int i = 0; i < renderers.Length; i++)
            {
                if (renderers[i] != null)
                    renderers[i].material = originalMaterials[i];
            }
        }
    }

    void OnDestroy()
    {
        if (isPlaying)
            StopBadApple();
    }
}
