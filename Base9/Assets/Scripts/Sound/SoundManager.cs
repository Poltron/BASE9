using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct Cues
{
    public SoundName name;
    public AudioSource cue;

    public Cues(SoundName _name, AudioSource _cue)
    {
        name = _name;
        cue = _cue;
    }
}

public class SoundManager : MonoBehaviour
{
    private List<Cues> cues = new List<Cues>();
    private AudioSource music;
    private AudioSource ambient;

    [SerializeField]
    private SoundList soundList;

    private static SoundManager instance;
    public static SoundManager Instance { get { return instance; } }

    void Start()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }

        if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().name == "Menu")
        {
            PlayMusic(SoundName.Musique_Loop_Menu, Vector3.zero);
        }
    }

    public bool GetSoundFromSoundList(SoundName sound, SoundPrefab[] list, out SoundPrefab prefab)
    {
        foreach (SoundPrefab cue in list)
        {
            if (cue.name == sound)
            {
                prefab = cue;
                return true;
            }
        }

        prefab = new SoundPrefab();
        return false;
    }

    public void RemoveCue(AudioSource source)
    {
        foreach (Cues cue in cues)
        {
            if (cue.cue == source)
            {
                cues.Remove(cue);
                Destroy(cue.cue.gameObject);
                return;
            }
        }
    }

    public void RemoveCue(SoundName name)
    {
        foreach (Cues cue in cues)
        {
            if (cue.name == name)
            {
                cues.Remove(cue);
                Destroy(cue.cue.gameObject);
                return;
            }
        }
    }

    public AudioSource PlaySoundCue(SoundName sound, Vector3 position)
    {
        SoundPrefab soundPrefab;
        // Find the sound cue
        if (!GetSoundFromSoundList(sound, soundList.cues, out soundPrefab))
            return null;

        // Instantiate the cue
        GameObject go = Instantiate(soundPrefab.clip, position, Quaternion.identity);
        OneShotThenDestroy oneShot = go.GetComponent<OneShotThenDestroy>();
        if (oneShot != null)
            oneShot.soundManager = this;

        // Register it
        AudioSource source = go.GetComponent<AudioSource>();
        cues.Add(new Cues(sound, source));

        return source;
    }

    public AudioSource PlayMusic(SoundName sound, Vector3 position)
    {
        if (music != null)
            Destroy(music.gameObject);

        SoundPrefab soundPrefab;
        // Find the sound cue
        if (!GetSoundFromSoundList(sound, soundList.musics, out soundPrefab))
            return null;

        // Instantiate the cue
        GameObject go = Instantiate(soundPrefab.clip, position, Quaternion.identity);

        // Register it
        AudioSource source = go.GetComponent<AudioSource>();
        music = source;

        return source;
    }

    public AudioSource PlayAmbient(SoundName sound, Vector3 position)
    {
        if (ambient != null)
            Destroy(ambient.gameObject);

        SoundPrefab soundPrefab;
        // Find the sound cue
        if (!GetSoundFromSoundList(sound, soundList.ambients, out soundPrefab))
            return null;

        // Instantiate the cue
        GameObject go = Instantiate(soundPrefab.clip, position, Quaternion.identity);
        
        // Register it
        AudioSource source = go.GetComponent<AudioSource>();
        ambient = source;

        return source;
    }
}
