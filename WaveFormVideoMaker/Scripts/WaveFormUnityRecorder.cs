using UnityEngine;
using UnityEditor;
using UnityEditor.Recorder;
using UnityEditor.Recorder.Input;
using System.Diagnostics;
using Debug = UnityEngine.Debug;
using System;

public class WaveFormUnityRecorder : MonoBehaviour
{
    private RecorderController recorderController;
    private string outputPath;

    public int width = 1920;
    public int height = 1080;

    public bool isRecording = false;


    public void StartCapture()
    {
        isRecording = true;
        // Set up the Recorder Controller settings
        var recorderControllerSettings = ScriptableObject.CreateInstance<RecorderControllerSettings>();
        recorderController = new RecorderController(recorderControllerSettings);

        var videoRecorderSettings = ScriptableObject.CreateInstance<MovieRecorderSettings>();
        videoRecorderSettings.name = "Video Recorder";
        videoRecorderSettings.Enabled = true;
        videoRecorderSettings.OutputFormat = MovieRecorderSettings.VideoRecorderOutputFormat.MP4;
        videoRecorderSettings.VideoBitRateMode = VideoBitrateMode.High;

        // Set resolution and output path
        videoRecorderSettings.ImageInputSettings = new GameViewInputSettings
        {
            OutputWidth = width,
            OutputHeight = height
        };
        string projectPath = Application.dataPath;
        projectPath = projectPath.Substring(0, projectPath.LastIndexOf('/')); // Get parent directory
        string timestamp = DateTime.Now.ToString("yyyyMMdd_HHmmss");
        outputPath = $"{projectPath}/RecordedVideo_{timestamp}";
        videoRecorderSettings.OutputFile = outputPath;

        recorderControllerSettings.AddRecorderSettings(videoRecorderSettings);
        recorderControllerSettings.SetRecordModeToManual();
        recorderController.PrepareRecording();
        recorderController.StartRecording();

        Debug.Log("Recording started at resolution: " + width + "x" + height);

        // Set the game view resolution to match the capture resolution if in the editor
#if UNITY_EDITOR
        SetGameViewResolution(width, height);
#endif
    }

    public void StartVideoRecording()
    {

    }

    // Stop Capture and open the folder
    public void StopCapture()
    {
        isRecording = false;
        if (recorderController != null && recorderController.IsRecording())
        {
            recorderController.StopRecording();
            Debug.Log("Recording stopped. Video saved at: " + outputPath);

            // Open the folder where the video is stored
            OpenRecordingFolder();
        }
    }

    // Open the folder where the video was stored
    [NaughtyAttributes.Button]
    private void OpenRecordingFolder()
    {
        string projectPath = Application.dataPath;
        projectPath = projectPath.Substring(0, projectPath.LastIndexOf('/')); // Get parent directory
        string outputPath2 = projectPath + "/RecordedVideo";
        string folderPath = System.IO.Path.GetDirectoryName(outputPath2);

        // Open the folder in the file explorer
        Application.OpenURL("file://" + folderPath);
    }

#if UNITY_EDITOR
    // Set the GameView resolution in the Editor to match the capture resolution
    private void SetGameViewResolution(int width, int height)
    {
        var gameViewType = typeof(Editor).Assembly.GetType("UnityEditor.GameView");
        var gameViewWindow = EditorWindow.GetWindow(gameViewType);
        gameViewWindow.titleContent.text = "Game"; // Just in case it has been renamed

        // Set the resolution
        if (gameViewWindow != null)
        {
            gameViewWindow.maxSize = new Vector2(width, height);
            gameViewWindow.minSize = new Vector2(width, height);
        }
    }
#endif
}
