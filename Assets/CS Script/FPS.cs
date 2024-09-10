using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class FPS : MonoBehaviour
{
    public TextMeshProUGUI text;
    [Range(.1f,3f)]
    public float updateEveryXSeconds = .5f;

    private int count = 0;
    private static FPS _instence;
    
    private void Awake()
    {
        Application.targetFrameRate = 60;
        System.Array.Fill(fps, 60.0f);
        
        if (_instence)
        {
            Destroy(gameObject);
            return;
        }
        _instence = this;
    }

    // Update is called once per frame
    private float[] fps = new float[60];
    private int fpsIdx;
    private float timeSincelastUpdate = 10f;

    private FrameTiming[] _frameTimings = new FrameTiming[60];
    
    void Update()
    {
        for (int i = 0; i < 10; i++)
        {
            if (Input.GetKeyDown(KeyCode.Keypad1+i)|| Input.GetKeyDown(KeyCode.Alpha1 + i))
            {
                SceneManager.LoadScene(i);
            }
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene(gameObject.scene.name);
        }
        if (Input.GetKeyDown(KeyCode.V))
        {
            if(QualitySettings.vSyncCount==0)
                QualitySettings.vSyncCount = 1;
            else
                QualitySettings.vSyncCount=0;
        }

        fps[fpsIdx] = 1.0f / Time.deltaTime;
        fpsIdx = (fpsIdx + 1) % fps.Length;
        
        timeSincelastUpdate += Time.deltaTime;
        if (timeSincelastUpdate>=updateEveryXSeconds)
        {
            timeSincelastUpdate = 0;
        }
        else {
            return; 
        }
        
        float fpsMean = 0.0f;
        foreach (float f in fps) {
            fpsMean += f;
        }
        fpsMean /= fps.Length;

        uint timingsNum = FrameTimingManager.GetLatestTimings((uint)_frameTimings.Length, _frameTimings);
        if (timingsNum > 0) {
            double cpuMean = 0.0f;
            double renderMean = 0.0f;
            double gpuMean = 0.0f;
            for (int timingIdx = 0; timingIdx < timingsNum; ++timingIdx) {
                FrameTiming timing = _frameTimings[timingIdx];
                cpuMean += timing.cpuFrameTime;
                renderMean += timing.cpuRenderThreadFrameTime;
                gpuMean += timing.gpuFrameTime;
            }
            cpuMean /= timingsNum;
            renderMean /= timingsNum;
            gpuMean /= timingsNum;
            text.SetText($"num: {count}\nfps: {fpsMean:F2}\ncpu: {cpuMean:F2}ms\nrt: {renderMean:F2}ms\ngpu: {gpuMean:F2}ms");
        } else {
            text.SetText($"num: {count}\nfps: {fpsMean:F2}");
        }
    }

    public static void SetCount(int count)
    {
        _instence.count = count;
    }
}
