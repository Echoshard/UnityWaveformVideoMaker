
# Unity Waveform Video Maker

Create waveform videos directly in Unity. 

## Setup

This tool requires the following Unity/packages assets to function correctly:

1. **Naughty Attributes**
   [Asset Store Link](https://assetstore.unity.com/packages/tools/utilities/naughtyattributes-129996)

2. **SimpleSpectrum**
   [Website](https://samboyer.uk/) | [Asset Store Link](https://assetstore.unity.com/packages/tools/audio/simplespectrum-free-audio-spectrum-generator-webgl-85294)
3. **Unity Recorder** install via Unity package manager
```
com.unity.recorder
```

## Features

- Optional title and end cards
- Option to loop a video or use a background image

## Running

1. Open the One of the provided recording scenes.
2. Select the **WaveFormVideoMaker** GameObject.
3. Apply your audio and additional videos
4. Click the **Start Recording** on  button.

Unity will enter play mode and start recording. The recording duration will match the length of the audio. The remaining recording time is displayed on the **WaveFormVideoMaker** component.

5. The video will be saved in your project folder you can access this quickly clicking ***Open Recording Folder**


### Notes

- If you cancel while recording, Unity may become unresponsive to audio playback. To resolve this, restarting Unity.

- This Unity Project will not build it's an editor recording project.

- Larger videos over 1080 can cause unity to lag and the recording will look choppy

- This was tested in Unity 2022.3.42
  

