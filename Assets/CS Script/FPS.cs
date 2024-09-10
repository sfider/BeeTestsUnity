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
        text.SetText(count+"\n"+fpsMean.ToString("0.0"));
    }

    public static void SetCount(int count)
    {
        _instence.count = count;
    }
}
