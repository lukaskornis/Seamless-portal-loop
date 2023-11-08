using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class VehicleDebug : MonoBehaviour
{
    public bool log;

    [Header("UI")] public float updateInterval = 0.1f;
    [SerializeField]TMP_Text fpsText;
    [SerializeField]TMP_Text resolutionText;
    [SerializeField]TMP_Text positionText;


    void Start()
    {
        Vehicle.Player.onBump.AddListener( (obj) => Log( $"Collided with {obj}") );
        Vehicle.Player.onFly.AddListener( (isFlying) => Log( $"Flying {isFlying}"));
        Vehicle.Player.onLand.AddListener( () => Log( $"Landed"));
        Vehicle.Player.onJump.AddListener( () => Log( $"Jumped"));
        Vehicle.Player.onTurn.AddListener( (side) => Log( $"Turned {side}"));

        //InvokeRepeating(nameof(UpdateUI), 0, updateInterval);
    }

    void Log(string message)
    {
        if (log)
        {
            print($"[{GetType().Name}] {message}");
        }
    }


    void Update()
    {
        UpdateUI();
        if(Input.GetKeyDown(KeyCode.R))Restart();
        if(Vehicle.Player.transform.position.y < -10)Restart();
    }

    void UpdateUI()
    {
        fpsText.text = $"fps: {1 / Time.deltaTime:F0}";
        resolutionText.text = $"Resolution: {Screen.width}x{Screen.height}";
        positionText.text = $"Position: {Vehicle.Player.transform.position}";
    }

    void Restart()
    {
         var scene = SceneManager.GetActiveScene();
         SceneManager.LoadScene(scene.name);
    }
}