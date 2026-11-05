using UnityEngine;
using FMODUnity;
using FMOD.Studio;

public class MusicManager : MonoBehaviour
{
    [Header("FMOD Event")]
    [SerializeField] private EventReference musicEvent;

    [Header("Music Settings")]
    [SerializeField, Range(0f, 2f)]
    private float startingRoomType = 1.0f;

    [Header("Transition Settings")]
    [SerializeField] private float transitionSpeed = 0.75f;

    private float targetRoomType;

    private EventInstance musicInstance;

    private float roomType;

    public static MusicManager Instance;

    private void Start()
    {

        // If an instance already exists and it's not this one, destroy this duplicate
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        // Set the global instance to this script
        Instance = this;

        // Optional: Keep this object alive when switching scenes
        DontDestroyOnLoad(gameObject);


        // Create the FMOD event instance
        musicInstance = RuntimeManager.CreateInstance(musicEvent);

        // Set the initial intensity before playback
        targetRoomType = startingRoomType;
        roomType = startingRoomType;
        musicInstance.setParameterByName("RoomType", roomType);

        // Start the music
        musicInstance.start();
    }

    public void SetRoomType(float newRoomType)
    {
        // Keep the value within the parameter range
        targetRoomType = Mathf.Clamp(newRoomType, 0f, 2f);
    }

    //    // Avoid unnecessary parameter updates
    //    if (Mathf.Approximately(newRoomType, roomType))
    //        return;

    //    roomType = newRoomType;

    //    // Update the FMOD parameter
    //    musicInstance.setParameterByName("RoomType", roomType);
    //}

    public void StopMusic()
    {
        if (musicInstance.isValid())
        {
            musicInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
            musicInstance.release();
        }
    }

    //debugging
    private void Update()
    {
        if (Application.isPlaying)
        {
            SetRoomType(startingRoomType);
        }

        roomType = Mathf.MoveTowards(
        roomType,
        targetRoomType,
        transitionSpeed * Time.deltaTime
        );

        musicInstance.setParameterByName("RoomType", roomType);
    }

    private void OnDestroy()
    {
        StopMusic();
    }
}