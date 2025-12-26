using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using Newtonsoft.Json;
using Mercury.Menu;
using Mercury.Patches;
using System;
using System.Diagnostics;
using MercuryV2.Mods;
using UnityEngine.XR;
using UnityEngine.SpatialTracking;
using OVR.OpenVR;
using Mercury.Mods;
using Photon.Pun;
using UnityEngine.InputSystem;
using Mercury;

public class MacroRecorder : MonoBehaviour
{
    public class FrameData
    {
        public float timestamp;
        public Vector3 bodyPosition;
        public Quaternion bodyRotation;
        public Vector3 leftHandPosition;
        public Quaternion leftHandRotation;
        public Vector3 rightHandPosition;
        public Quaternion rightHandRotation;
        public Vector3 bodyVelocity;
        public Vector3 cameraPosition;
        public Quaternion cameraRotation;
    }

    private static List<FrameData> recordedFrames = new List<FrameData>();
    private static bool isRecording = false;
    private static bool isPlaying = false;
    private static bool isLerping = false;
    private static float recordInterval = 0.011f; // ~90 FPS
    private static string baseFileName = "Macro";
    private static readonly string fileExtension = ".json";
    private static float recordTimer = 0f;
    private static float playbackTimer = 0f;
    private static int currentFrameIndex = 0;
    private static FrameData targetStartFrame;
    private static GameObject indicator;
    private static float lastBindPressTime = -1f;
    private static readonly float bindCooldown = 0.5f;
    private static float maxLerpTime;
    private static Coroutine activeCoroutine;
    private static int selectedMacroIndex = 0;
    private static int retryCount = 0;
    private static readonly int maxRetries = 5;
    private static int lastSelectedMacroIndex = -1;
    private static string lastLoadedFilePath;
    private static FrameData currentPlaybackFrame; // Stores current frame for potential use
    private static Vector3 originalTrackingRotationOffset; // Store original offset

    public static event Action OnMacroListChanged;

    private static readonly float[] proximityDistances = { 0.5f, 1f, 1.5f, 2f, 3f, 4f, 5f };

    private static GameObject proxyCam;

    private static MacroRecorder instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        float[] lerpspeed = { 0.1f, 0.2f, 0.3f, 0.4f, 0.5f, 0.6f };
        maxLerpTime = (float)lerpspeed[Math.Min(PluginConfig.macrolerpspeed, lerpspeed.Length)];

        if (PluginConfig.recordmacro)
        {
            string bind = CustomBinding.GetBinds("recordmacro");
            if (!string.IsNullOrEmpty(bind) && bind != "UNBOUND")
            {
                if (ControlsV2.GetControl(bind) && Time.time - lastBindPressTime >= bindCooldown)
                {
                    lastBindPressTime = Time.time;
                    if (!isRecording && !isPlaying && !isLerping)
                    {
                        StartRecording();
                    }
                    else if (isRecording)
                    {
                        StopRecording();
                    }
                }
            }
        }

        if (isRecording)
            RecordFrame();

        if (isPlaying)
            PlayFrame();
    }

    private void LateUpdate()
    {
        // Removed camera override, as trackingRotationOffset handles rotation
        // Position is set in PlayFrame/Lerp methods
    }

    private static bool IsGameStateValid()
    {
        if (GorillaTagger.Instance == null || GorillaLocomotion.GTPlayer.Instance == null || GorillaTagger.Instance.mainCamera == null)
        {
            return false;
        }
        return true;
    }

    public static void StartRecording()
    {
        recordedFrames.Clear();
        isRecording = true;
        recordTimer = 0f;

        if (IsGameStateValid() && GorillaTagger.Instance.bodyCollider != null)
        {
            if (indicator != null)
            {
                TargetIndicator.Destroy3D(indicator);
                indicator = null;
            }
            indicator = TargetIndicator.Create3D(GorillaTagger.Instance.bodyCollider.transform, false);
        }
    }

    public static void StopRecording()
    {
        isRecording = false;
        SaveToJson();

        if (indicator != null)
        {
            TargetIndicator.Destroy3D(indicator);
            indicator = null;
        }
    }

    private static void RecordFrame()
    {
        recordTimer += Time.deltaTime;
        if (recordTimer < recordInterval) return;

        if (!IsGameStateValid())
        {
            return;
        }

        FrameData frame = new FrameData
        {
            timestamp = Time.time,
            bodyPosition = GorillaTagger.Instance.bodyCollider.transform.position,
            bodyRotation = GorillaTagger.Instance.bodyCollider.transform.rotation,
            leftHandPosition = GorillaTagger.Instance.leftHandTransform.position,
            leftHandRotation = GorillaTagger.Instance.leftHandTransform.rotation,
            rightHandPosition = GorillaTagger.Instance.rightHandTransform.position,
            rightHandRotation = GorillaTagger.Instance.rightHandTransform.rotation,
            bodyVelocity = GorillaLocomotion.GTPlayer.Instance.bodyCollider.attachedRigidbody.linearVelocity,
            cameraPosition = GorillaTagger.Instance.mainCamera.transform.position,
            cameraRotation = GorillaTagger.Instance.mainCamera.transform.rotation
        };

        recordedFrames.Add(frame);

        if (indicator != null && recordedFrames.Count == 1)
        {
            indicator.transform.position = frame.bodyPosition;
        }

        recordTimer = 0f;
    }

    public static void SetSelectedMacro(int index)
    {
        string[] macroFiles = GetMacroFileNames();
        if (index >= 0 && index < macroFiles.Length && macroFiles[0] != "No Macros")
        {
            selectedMacroIndex = index;
            Menu.Macro[0].stringsliderind = index;
        }
        else
        {
            selectedMacroIndex = -1;
            Menu.Macro[0].stringsliderind = -1;
        }
    }

    public static void StartPlayback(int index)
    {
        string[] macroFiles = GetMacroFileNames();
        if (index < 0 || index >= macroFiles.Length || macroFiles[0] == "No Macros")
        {
            return;
        }
        string filePath = Path.Combine(Configs.macroPath, macroFiles[index] + fileExtension);
        StartPlayback(filePath);
    }

    private static void StartPlayback(string filePath)
    {
        StopPlayback();

        if (!IsGameStateValid())
        {
            if (retryCount < maxRetries && instance != null)
            {
                retryCount++;
                instance.StartCoroutine(RetryPlayback(filePath));
            }
            else
            {
                retryCount = 0;
            }
            return;
        }

        // Capture original trackingRotationOffset
        if (VRRig.LocalRig != null)
        {
            originalTrackingRotationOffset = VRRig.LocalRig.head.trackingRotationOffset;
        }
        else
        {
            originalTrackingRotationOffset = Vector3.zero; // Fallback if rig is null
        }

        retryCount = 0;
        if (string.IsNullOrEmpty(filePath))
        {
            return;
        }

        LoadFromJson(filePath);
        if (recordedFrames.Count == 0)
        {
            return;
        }

        if (instance == null)
        {
            return;
        }

        currentFrameIndex = 0;
        playbackTimer = 0f;
        isLerping = true;
        targetStartFrame = recordedFrames[0];

        if (activeCoroutine != null)
        {
            instance.StopCoroutine(activeCoroutine);
            activeCoroutine = null;
        }
        activeCoroutine = instance.StartCoroutine(LerpToStart());
    }

    private static System.Collections.IEnumerator RetryPlayback(string filePath)
    {
        yield return new WaitForSeconds(0.1f);
        StartPlayback(filePath);
    }

    public static void StopPlayback()
    {
        if (isPlaying || isLerping)
        {
            if (proxyCam != null)
                Destroy(proxyCam);
            if (GorillaTagger.Instance.mainCamera.GetComponent<Camera>() != null && !GorillaTagger.Instance.mainCamera.GetComponent<Camera>().enabled)
                GorillaTagger.Instance.mainCamera.GetComponent<Camera>().enabled = true;

            isPlaying = false;
            isLerping = false;
            currentFrameIndex = 0;
            playbackTimer = 0f;
            currentPlaybackFrame = null; // Clear current frame to stop any overrides
            if (instance != null && activeCoroutine != null)
            {
                instance.StopCoroutine(activeCoroutine);
                activeCoroutine = null;
            }

            // Restore original trackingRotationOffset
            if (VRRig.LocalRig != null)
            {
                VRRig.LocalRig.head.trackingRotationOffset = originalTrackingRotationOffset;
            }
        }
    }

    private static void PlayFrame()
    {
        if (!isPlaying || currentFrameIndex >= recordedFrames.Count)
        {
            StartLerpingOut();
            return;
        }

        if (!IsGameStateValid())
        {
            return;
        }

        FrameData currentFrame = recordedFrames[currentFrameIndex];
        currentPlaybackFrame = currentFrame; // Store for potential use

        GorillaTagger.Instance.bodyCollider.transform.position = currentFrame.bodyPosition;
        GorillaTagger.Instance.bodyCollider.transform.rotation = currentFrame.bodyRotation;
        GorillaTagger.Instance.leftHandTransform.position = currentFrame.leftHandPosition;
        GorillaTagger.Instance.leftHandTransform.rotation = currentFrame.leftHandRotation;
        GorillaTagger.Instance.rightHandTransform.position = currentFrame.rightHandPosition;
        GorillaTagger.Instance.rightHandTransform.rotation = currentFrame.rightHandRotation;
        GorillaTagger.Instance.mainCamera.transform.position = currentFrame.cameraPosition;
        // Apply recorded rotation to trackingRotationOffset as Vector3
        if (VRRig.LocalRig != null)
        {
            VRRig.LocalRig.head.trackingRotationOffset = currentFrame.cameraRotation.eulerAngles;
        }
        GorillaLocomotion.GTPlayer.Instance.bodyCollider.attachedRigidbody.linearVelocity = currentFrame.bodyVelocity;

        playbackTimer += Time.deltaTime;
        if (playbackTimer >= recordInterval)
        {
            currentFrameIndex++;
            playbackTimer = 0f;
        }
    }

    private static void CheckProximity()
    {
        if (!PluginConfig.autoplayproximity)
        {
            return;
        }

        string[] macroFiles = GetMacroFileNames();
        if (macroFiles.Length == 0 || macroFiles[0] == "No Macros")
        {
            return;
        }

        int menuIndex = Menu.Macro[0].stringsliderind;
        if (menuIndex >= 0 && menuIndex < macroFiles.Length)
        {
            selectedMacroIndex = menuIndex;
        }
        else
        {
            selectedMacroIndex = 0;
            Menu.Macro[0].stringsliderind = 0;
        }

        string filePath = Path.Combine(Configs.macroPath, macroFiles[selectedMacroIndex] + fileExtension);

        if (filePath != lastLoadedFilePath || recordedFrames.Count == 0)
        {
            LoadFromJson(filePath);
            lastLoadedFilePath = filePath;
            if (recordedFrames.Count == 0)
            {
                return;
            }
        }

        FrameData firstFrame = recordedFrames[0];

        if (indicator == null || selectedMacroIndex != lastSelectedMacroIndex)
        {
            if (indicator != null)
            {
                TargetIndicator.Destroy3D(indicator);
                indicator = null;
            }
            indicator = TargetIndicator.Create3D(firstFrame.bodyPosition, false);
            lastSelectedMacroIndex = selectedMacroIndex;
        }

        int distanceIndex = PluginConfig.autoplaydistance;
        if (distanceIndex < 0 || distanceIndex >= proximityDistances.Length)
        {
            distanceIndex = 2;
            PluginConfig.autoplaydistance = distanceIndex; // Ensure valid index
        }
        float distanceThreshold = proximityDistances[distanceIndex];

        if (indicator != null)
        {
            indicator.transform.position = firstFrame.bodyPosition;
            indicator.transform.localScale = Vector3.one * distanceThreshold;
        }

        if (IsGameStateValid() && GorillaTagger.Instance.bodyCollider != null)
        {
            float distance = Vector3.Distance(GorillaTagger.Instance.bodyCollider.transform.position, firstFrame.bodyPosition);
            if (distance <= distanceThreshold)
            {
                StartLerpingIn(firstFrame);
            }
        }
    }

    private static void StartLerpingIn(FrameData startFrame)
    {
        isLerping = true;
        targetStartFrame = startFrame;
        if (instance != null)
        {
            if (activeCoroutine != null)
            {
                instance.StopCoroutine(activeCoroutine);
                activeCoroutine = null;
            }
            activeCoroutine = instance.StartCoroutine(LerpToStart());
        }
    }

    private static void StartLerpingOut()
    {
        isPlaying = false;
        isLerping = true;
        if (instance != null)
        {
            if (activeCoroutine != null)
            {
                instance.StopCoroutine(activeCoroutine);
                activeCoroutine = null;
            }
            activeCoroutine = instance.StartCoroutine(LerpToOriginal());
        }

        if (indicator != null)
        {
            TargetIndicator.Destroy3D(indicator);
            indicator = null;
        }
    }

    private static System.Collections.IEnumerator LerpToStart()
    {
        if (!IsGameStateValid())
        {
            isLerping = false;
            yield break;
        }

        if (targetStartFrame == null)
        {
            isLerping = false;
            yield break;
        }

        if(proxyCam != null)
            Destroy(proxyCam);
        if(proxyCam == null)
        {
            proxyCam = new GameObject("proxyCam");
            proxyCam.AddComponent<Camera>();

            proxyCam.GetComponent<Camera>().transform.position = GorillaTagger.Instance.bodyCollider.transform.position + Vector3.up * 0.5f;
            proxyCam.transform.position = GorillaTagger.Instance.bodyCollider.transform.position + Vector3.up * 0.5f;
            proxyCam.transform.rotation = GorillaTagger.Instance.bodyCollider.transform.rotation;
            proxyCam.transform.SetParent(GorillaTagger.Instance.bodyCollider.transform, true);

            proxyCam.GetComponent<Camera>().depth = 0; // Higher depth renders later (on top)

            if (!GorillaTagger.Instance.thirdPersonCamera.activeSelf)
                GorillaTagger.Instance.thirdPersonCamera.SetActive(true);
            if (GorillaTagger.Instance.thirdPersonCamera.GetComponentInChildren<Camera>().enabled)
                GorillaTagger.Instance.thirdPersonCamera.GetComponentInChildren<Camera>().enabled = false;

            if (GorillaTagger.Instance.mainCamera.GetComponent<Camera>() != null && GorillaTagger.Instance.mainCamera.GetComponent<Camera>().enabled)
                GorillaTagger.Instance.mainCamera.GetComponent<Camera>().enabled = false;
        }

        float elapsedTime = 0f;
        Vector3 startBodyPos = GorillaTagger.Instance.bodyCollider.transform.position;
        Quaternion startBodyRot = GorillaTagger.Instance.bodyCollider.transform.rotation;
        Vector3 startLeftPos = GorillaTagger.Instance.leftHandTransform.position;
        Quaternion startLeftRot = GorillaTagger.Instance.leftHandTransform.rotation;
        Vector3 startRightPos = GorillaTagger.Instance.rightHandTransform.position;
        Quaternion startRightRot = GorillaTagger.Instance.rightHandTransform.rotation;
        Vector3 startCameraPos = GorillaTagger.Instance.mainCamera.transform.position;
        Quaternion startCameraRot = GorillaTagger.Instance.mainCamera.transform.rotation;
        Vector3 startVelocity = GorillaLocomotion.GTPlayer.Instance.bodyCollider.attachedRigidbody.linearVelocity;
        Vector3 startTrackingRotOffset = originalTrackingRotationOffset;

        while (elapsedTime < maxLerpTime)
        {
            elapsedTime += Time.deltaTime;
            float t = Mathf.Clamp01(elapsedTime / maxLerpTime);
            Vector3 lerpedCameraPos = Vector3.Lerp(startCameraPos, targetStartFrame.cameraPosition, t);
            Quaternion lerpedCameraRot = Quaternion.Slerp(startCameraRot, targetStartFrame.cameraRotation, t);

            currentPlaybackFrame = new FrameData
            {
                bodyPosition = Vector3.Lerp(startBodyPos, targetStartFrame.bodyPosition, t),
                bodyRotation = Quaternion.Slerp(startBodyRot, targetStartFrame.bodyRotation, t),
                leftHandPosition = Vector3.Lerp(startLeftPos, targetStartFrame.leftHandPosition, t),
                leftHandRotation = Quaternion.Slerp(startLeftRot, targetStartFrame.leftHandRotation, t),
                rightHandPosition = Vector3.Lerp(startRightPos, targetStartFrame.rightHandPosition, t),
                rightHandRotation = Quaternion.Slerp(startRightRot, targetStartFrame.rightHandRotation, t),
                cameraPosition = lerpedCameraPos,
                cameraRotation = lerpedCameraRot,
                bodyVelocity = Vector3.Lerp(startVelocity, targetStartFrame.bodyVelocity, t)
            };

            GorillaTagger.Instance.bodyCollider.transform.position = currentPlaybackFrame.bodyPosition;
            GorillaTagger.Instance.bodyCollider.transform.rotation = currentPlaybackFrame.bodyRotation;
            GorillaTagger.Instance.leftHandTransform.position = currentPlaybackFrame.leftHandPosition;
            GorillaTagger.Instance.leftHandTransform.rotation = currentPlaybackFrame.leftHandRotation;
            GorillaTagger.Instance.rightHandTransform.position = currentPlaybackFrame.rightHandPosition;
            GorillaTagger.Instance.rightHandTransform.rotation = currentPlaybackFrame.rightHandRotation;
            GorillaTagger.Instance.mainCamera.transform.position = currentPlaybackFrame.cameraPosition;
            if (VRRig.LocalRig != null)
            {
                Vector3 targetEuler = targetStartFrame.cameraRotation.eulerAngles;
                VRRig.LocalRig.head.trackingRotationOffset = Vector3.Lerp(startTrackingRotOffset, targetEuler, t);
            }
            GorillaLocomotion.GTPlayer.Instance.bodyCollider.attachedRigidbody.linearVelocity = currentPlaybackFrame.bodyVelocity;

            yield return null;
        }

        currentPlaybackFrame = targetStartFrame;
        GorillaTagger.Instance.bodyCollider.transform.position = targetStartFrame.bodyPosition;
        GorillaTagger.Instance.bodyCollider.transform.rotation = targetStartFrame.bodyRotation;
        GorillaTagger.Instance.leftHandTransform.position = targetStartFrame.leftHandPosition;
        GorillaTagger.Instance.leftHandTransform.rotation = targetStartFrame.leftHandRotation;
        GorillaTagger.Instance.rightHandTransform.position = targetStartFrame.rightHandPosition;
        GorillaTagger.Instance.rightHandTransform.rotation = targetStartFrame.rightHandRotation;
        GorillaTagger.Instance.mainCamera.transform.position = targetStartFrame.cameraPosition;
        if (VRRig.LocalRig != null)
        {
            VRRig.LocalRig.head.trackingRotationOffset = targetStartFrame.cameraRotation.eulerAngles;
        }
        GorillaLocomotion.GTPlayer.Instance.bodyCollider.attachedRigidbody.linearVelocity = targetStartFrame.bodyVelocity;

        isLerping = false;
        currentFrameIndex = 0;
        isPlaying = true;
    }

    private static System.Collections.IEnumerator LerpToOriginal()
    {
        if (!IsGameStateValid())
        {
            isLerping = false;
            yield break;
        }

        float elapsedTime = 0f;
        FrameData endFrame = recordedFrames[recordedFrames.Count - 1];
        Vector3 originalBodyPos = GorillaTagger.Instance.bodyCollider.transform.position;
        Quaternion originalBodyRot = GorillaTagger.Instance.bodyCollider.transform.rotation;
        Vector3 originalLeftPos = GorillaTagger.Instance.leftHandTransform.position;
        Quaternion originalLeftRot = GorillaTagger.Instance.leftHandTransform.rotation;
        Vector3 originalRightPos = GorillaTagger.Instance.rightHandTransform.position;
        Quaternion originalRightRot = GorillaTagger.Instance.rightHandTransform.rotation;
        Vector3 originalCameraPos = GorillaTagger.Instance.mainCamera.transform.position;
        Quaternion originalCameraRot = GorillaTagger.Instance.mainCamera.transform.rotation;

        while (elapsedTime < maxLerpTime)
        {
            elapsedTime += Time.deltaTime;
            float t = Mathf.Clamp01(elapsedTime / maxLerpTime);
            Vector3 lerpedCameraPos = Vector3.Lerp(endFrame.cameraPosition, originalCameraPos, t);
            Quaternion lerpedCameraRot = Quaternion.Slerp(endFrame.cameraRotation, originalCameraRot, t);

            currentPlaybackFrame = new FrameData
            {
                bodyPosition = Vector3.Lerp(endFrame.bodyPosition, originalBodyPos, t),
                bodyRotation = Quaternion.Slerp(endFrame.bodyRotation, originalBodyRot, t),
                leftHandPosition = Vector3.Lerp(endFrame.leftHandPosition, originalLeftPos, t),
                leftHandRotation = Quaternion.Slerp(endFrame.leftHandRotation, originalLeftRot, t),
                rightHandPosition = Vector3.Lerp(endFrame.rightHandPosition, originalRightPos, t),
                rightHandRotation = Quaternion.Slerp(endFrame.rightHandRotation, originalRightRot, t),
                cameraPosition = lerpedCameraPos,
                cameraRotation = lerpedCameraRot
            };

            GorillaTagger.Instance.bodyCollider.transform.position = currentPlaybackFrame.bodyPosition;
            GorillaTagger.Instance.bodyCollider.transform.rotation = currentPlaybackFrame.bodyRotation;
            GorillaTagger.Instance.leftHandTransform.position = currentPlaybackFrame.leftHandPosition;
            GorillaTagger.Instance.leftHandTransform.rotation = currentPlaybackFrame.leftHandRotation;
            GorillaTagger.Instance.rightHandTransform.position = currentPlaybackFrame.rightHandPosition;
            GorillaTagger.Instance.rightHandTransform.rotation = currentPlaybackFrame.rightHandRotation;
            GorillaTagger.Instance.mainCamera.transform.position = currentPlaybackFrame.cameraPosition;
            if (VRRig.LocalRig != null)
            {
                Vector3 startEuler = endFrame.cameraRotation.eulerAngles;
                VRRig.LocalRig.head.trackingRotationOffset = Vector3.Lerp(startEuler, originalTrackingRotationOffset, t);
            }

            if (proxyCam != null)
                Destroy(proxyCam);
            if (GorillaTagger.Instance.mainCamera.GetComponent<Camera>() != null && !GorillaTagger.Instance.mainCamera.GetComponent<Camera>().enabled)
                GorillaTagger.Instance.mainCamera.GetComponent<Camera>().enabled = true;

            yield return null;
        }

        currentPlaybackFrame = null; // Stop any potential overrides
        isLerping = false;
        currentFrameIndex = 0;

        // Restore original trackingRotationOffset
        if (VRRig.LocalRig != null)
        {
            VRRig.LocalRig.head.trackingRotationOffset = originalTrackingRotationOffset;
        }
    }

    private static void SaveToJson()
    {
        try
        {
            if (string.IsNullOrEmpty(Configs.macroPath))
            {
                return;
            }

            string[] existingFiles = Directory.GetFiles(Configs.macroPath, "*" + fileExtension);
            int nextFileNumber = 1;
            while (existingFiles.Any(file => Path.GetFileNameWithoutExtension(file).EndsWith($"_{nextFileNumber}")))
            {
                nextFileNumber++;
            }

            string newFileName = $"{baseFileName}_{nextFileNumber}{fileExtension}";
            string filePath = Path.Combine(Configs.macroPath, newFileName);

            string jsonContent = JsonConvert.SerializeObject(recordedFrames, Formatting.Indented);
            File.WriteAllText(filePath, jsonContent);

            if (OnMacroListChanged != null)
                OnMacroListChanged.Invoke();
        }
        catch (System.Exception e)
        {
            UnityEngine.Debug.LogError("[COLOSSAL] MACRO : " + e);
        }
    }

    private static void LoadFromJson(string filePath)
    {
        try
        {
            if (!File.Exists(filePath))
            {
                recordedFrames = new List<FrameData>();
                return;
            }

            string json = File.ReadAllText(filePath);
            if (string.IsNullOrEmpty(json))
            {
                recordedFrames = new List<FrameData>();
                return;
            }

            recordedFrames = JsonConvert.DeserializeObject<List<FrameData>>(json) ?? new List<FrameData>();
            if (recordedFrames.Any(frame => frame == null))
            {
                recordedFrames = recordedFrames.Where(frame => frame != null).ToList();
            }
        }
        catch (System.Exception)
        {
            recordedFrames = new List<FrameData>();
        }
    }

    public static string[] GetMacroFileNames()
    {
        try
        {
            if (string.IsNullOrEmpty(Configs.macroPath) || !Directory.Exists(Configs.macroPath))
            {
                return new string[] { "No Macros" };
            }

            string[] files = Directory.GetFiles(Configs.macroPath, "*" + fileExtension);
            if (files.Length == 0)
            {
                return new string[] { "No Macros" };
            }

            string[] names = new string[files.Length];
            for (int i = 0; i < files.Length; i++)
            {
                names[i] = Path.GetFileNameWithoutExtension(files[i]);
                if (string.IsNullOrEmpty(names[i]))
                {
                    names[i] = "Invalid Macro";
                }
            }
            return names;
        }
        catch (System.Exception)
        {
            return new string[] { "Error" };
        }
    }

    public static void DeleteMacro()
    {
        string[] macroFiles = GetMacroFileNames();
        if (macroFiles == null || macroFiles.Length == 0 || macroFiles[0] == "No Macros")
        {
            return;
        }

        if (string.IsNullOrEmpty(Configs.macroPath))
        {
            return;
        }

        int index = Menu.Macro[0].stringsliderind;
        if (index < 0 || index >= macroFiles.Length)
        {
            return;
        }

        string fileName = macroFiles[index];
        if (string.IsNullOrEmpty(fileName))
        {
            return;
        }

        string filePath = Path.Combine(Configs.macroPath, fileName + fileExtension);
        try
        {
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
                string[] updatedMacroFiles = GetMacroFileNames();
                if (updatedMacroFiles.Length == 0 || updatedMacroFiles[0] == "No Macros")
                {
                    selectedMacroIndex = -1;
                    Menu.Macro[0].stringsliderind = -1;
                    lastSelectedMacroIndex = -1;
                }
                else
                {
                    selectedMacroIndex = Mathf.Min(index, updatedMacroFiles.Length - 1);
                    Menu.Macro[0].stringsliderind = selectedMacroIndex;
                    lastSelectedMacroIndex = selectedMacroIndex;
                }

                if (OnMacroListChanged != null)
                    OnMacroListChanged.Invoke();
            }
        }
        catch (System.Exception e)
        {
            UnityEngine.Debug.LogError("[COLOSSAL] MACRO : " + e);
        }
    }
}