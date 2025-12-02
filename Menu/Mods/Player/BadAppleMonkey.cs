
﻿using Colossal;
using UnityEngine;
using UnityEngine.Video;

public class BadAppleMonkey : MonoBehaviour
{
    private VideoClip videoClip;
    private static RenderTexture renderTexture;
    private Material videoMaterial;
    private static VideoPlayer videoPlayer;
    private static AudioSource audioSource;
    private static Material[] originalMaterials;
    private static Renderer[] renderers;

    void Start()
    {
        if (AssetBundleLoader.bundle == null)
        {
            return;
        }

        // Load the video clip from the AssetBundle
        videoClip = AssetBundleLoader.bundle.LoadAsset<VideoClip>("badapple");
        if (videoClip == null)
        {
            return;
        }

        // Create RenderTexture to display the video
        renderTexture = new RenderTexture(1920, 1080, 16); // Adjust size as needed

        // Create a material with a shader that can display the video
        videoMaterial = new Material(Shader.Find("Unlit/Texture"));
        videoMaterial.mainTexture = renderTexture;

        // Find all objects with a Renderer component in the scene
        renderers = FindObjectsOfType<Renderer>();
        originalMaterials = new Material[renderers.Length];

        // Save the original materials and apply the video material to each renderer
        for (int i = 0; i < renderers.Length; i++)
        {
            // Save the original material of the renderer
            originalMaterials[i] = renderers[i].material;
            // Apply the video material to the object
            renderers[i].material = videoMaterial;
        }

        // Set up the VideoPlayer
        videoPlayer = gameObject.AddComponent<VideoPlayer>();
        audioSource = gameObject.AddComponent<AudioSource>();

        videoPlayer.playOnAwake = false; // Prevent automatic playback
        videoPlayer.source = VideoSource.VideoClip;
        videoPlayer.clip = videoClip;
        videoPlayer.renderMode = VideoRenderMode.RenderTexture;
        videoPlayer.targetTexture = renderTexture; // Set the RenderTexture as the target
        videoPlayer.audioOutputMode = VideoAudioOutputMode.AudioSource;
        videoPlayer.SetTargetAudioSource(0, audioSource); // Set the AudioSource for the video

        // Prepare and play the video
        videoPlayer.Prepare();
        videoPlayer.prepareCompleted += (vp) => vp.Play(); // Start playing once prepared
    }

    public static void stop()
    {
        // Restore the original materials of the renderers
        for (int i = 0; i < renderers.Length; i++)
        {
            renderers[i].material = originalMaterials[i];
        }

        // Clean up the RenderTexture and VideoPlayer when the object is destroyed
        if (renderTexture != null)
        {
            Destroy(renderTexture);
        }

        if (videoPlayer != null)
        {
            videoPlayer.Stop(); // Ensure the video stops
            Destroy(videoPlayer);
        }

        if (audioSource != null)
        {
            audioSource.Stop(); // Ensure the audio stops
            Destroy(audioSource);
        }

        // Optionally log that everything has been cleaned up
    }
}