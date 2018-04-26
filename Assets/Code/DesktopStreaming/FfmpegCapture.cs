using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Threading;
using System.Diagnostics;

public class FfmpegCapture : MonoBehaviour {

    public string destinationPort = "9998";
    public string destinationFilename = "feed1.ffm";

    public string serverAdress = "http://35.196.115.121";

    private string destinationAdress;

    private Thread ffmpegCaptureThread;
    private Process ffmpegCaptureProcess;

    private string capturingProcessPath;

    private bool captureProcessRunning;

    void Start()
    {
        destinationAdress = serverAdress + ":" + destinationPort + "/" + destinationFilename;
        StartCapturing();
    }

    private void StartCapturing()
    {
        captureProcessRunning = true;
        capturingProcessPath = Application.dataPath + "/ffmpeg.exe";
        ffmpegCaptureThread = new Thread(RunFfmpegCaptureProcess);
        ffmpegCaptureThread.Start();
    }

    void RunFfmpegCaptureProcess()
    {

        var processInfo = new ProcessStartInfo(capturingProcessPath, "-f gdigrab  -framerate 30 -resize 1280x960 -i desktop -tune zerolatency " + destinationAdress);
        processInfo.CreateNoWindow = true;
        processInfo.UseShellExecute = false;
        ffmpegCaptureProcess = Process.Start(processInfo);

        ffmpegCaptureProcess.WaitForExit();
        ffmpegCaptureProcess.Close();
        captureProcessRunning = false;
    }

    private void Update()
    {
        if (!captureProcessRunning)
        {
            StartCapturing();
        }
    }

    private void StopFfmpegCaptureProcess()
    {
        try
        {
            ffmpegCaptureProcess.Kill();
        }
        catch (Exception) { };
        ffmpegCaptureThread.Abort();

    }

    private void OnApplicationQuit()
    {
        StopFfmpegCaptureProcess();
    }
}
