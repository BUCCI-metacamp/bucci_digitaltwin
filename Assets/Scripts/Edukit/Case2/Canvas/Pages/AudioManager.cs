
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : BaseMono<AudioManager>
{
    public enum KindOfAudio
    {   None = 0,
        BTN_DESC,
        BTN_START,
        BTN_CLICK,
        BTN_CLICK_X,
        POPUP,
        MALE_CHILD_CHANGE,
        MALE_CHILD_GOOD,
        MALE_CHILD_NO,
        FEMAIL_CHILD_CHANGE,
        FEMAIL_CHILD_GOOD,
        FEMAIL_CHILD_NO,
        NEXTSTEP,
        WIN,
        LOSE,
        TAKECAMERA,
        FOODCARD_FLIP,
        Bgm = 100,
        BGM_CLASSIC,
        BGM_FOREST,
        BGM_SEA,
        BGM_NOTHING,
    };

    [System.Serializable]
    public class AudioElements
    {   public KindOfAudio kind;
        public float volume;
        public List<AudioClip> listClips;
    }

    [SerializeField]
    Dictionary<KindOfAudio, AudioElements> listAudios = new Dictionary<KindOfAudio, AudioElements>();

    [SerializeField]
    public AudioSource sourceBgm;

    [SerializeField]
    public List<AudioElements> listserialAudios;

    [SerializeField]
    public AudioSource sourceSfx;
       

    public float VolumeSfx
    {   get
        {
            return PlayerPrefs.GetFloat("volumeSfx", 0.8f);
        }
        set { PlayerPrefs.SetFloat("volumeSfx", value ); }
    }
    public float VolumeBgm
    {   get
        {
            return PlayerPrefs.GetFloat("volumeBgm", 0.3f );
        }
        set
        {
            PlayerPrefs.SetFloat("volumeBgm", value);            
            sourceBgm.volume = value;
        }
    }
    void Awake()
    {
        listAudios = new Dictionary<KindOfAudio, AudioElements>();
        foreach (var listOne in listserialAudios)
            listAudios.Add( listOne.kind, listOne );

        VolumeSfx = 0.9f;
        VolumeBgm = 0.6f;
    }
    public void SetDefaultVolume()
    {
        VolumeBgm = VolumeBgm;
    }
    public void PlayBgm(AudioClip clip)
    {
        sourceBgm.clip = clip;
        sourceBgm.loop = true;
        sourceBgm.volume = VolumeBgm;
        sourceBgm.Play();
    }
    public void PlayBgmFade(AudioClip clip)
    {
        var curveOff = AnimationCurve.EaseInOut(0.0f, sourceBgm.volume, 0.4f, 0.0f);
        var curveOn = AnimationCurve.EaseInOut(0.0f, 0.0f, 0.4f, sourceBgm.volume);

        Timer.Instance.CurveSingle(curveOff,
            delegate (Timer.Element time, float value) 
            {   sourceBgm.volume = value;            }
            ,
             delegate (Timer.Element time, int count)
             {
                 sourceBgm.loop = true;
                 sourceBgm.clip = clip;
                 sourceBgm.Play();

                 Timer.Instance.CurveSingle(curveOn,
                     delegate (Timer.Element timeOn, float valueOn) { sourceBgm.volume = valueOn; }
                     ,delegate (Timer.Element timeOn, int countOn) {} );
             });
    }
    public void PlaySfx(AudioClip clip)
    {   
        sourceSfx.PlayOneShot(clip, VolumeSfx);
    }
    public void StopSfx ()
    {
        sourceSfx.Stop();
    }
    public void StopBgm()
    {
        sourceBgm.Stop();
    }
    public void PlayBgm(KindOfAudio kindAudio)
    {
        if (listAudios.ContainsKey(kindAudio) == false) return;

        var randomValue = Random.Range(0, listAudios[kindAudio].listClips.Count);
        PlayBgm(listAudios[kindAudio].listClips[randomValue]);
    }
    public void PlayBgmFade(KindOfAudio kindAudio)
    {
        if (listAudios.ContainsKey(kindAudio) == false) return;

        var randomValue = Random.Range(0, listAudios[kindAudio].listClips.Count);

        PlayBgmFade(listAudios[kindAudio].listClips[randomValue]);
    }
    public void PlaySfx(KindOfAudio kindAudio)
    {
        if (listAudios.ContainsKey(kindAudio) == false) return;       

        var randomValue = Random.Range(0, listAudios[kindAudio].listClips.Count);
        PlaySfx(listAudios[kindAudio].listClips[randomValue]);
    }
    public void PlaySfxs(KindOfAudio kindAudio)
    {
        if (listAudios.ContainsKey(kindAudio) == false) return;
                
        for( int i=0;i< listAudios[kindAudio].listClips.Count;++i)
            PlaySfx(listAudios[kindAudio].listClips[i]);
    }
    public bool isMuteAndroidState()
    {
#if UNITY_EDITOR

        //return keyState;
#endif
        const string AUDIO_SERVICE = "audio";
        AndroidJavaClass unityClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        AndroidJavaObject unityActivity = unityClass.GetStatic<AndroidJavaObject>("currentActivity");
        AndroidJavaObject unityContext = unityActivity.Call<AndroidJavaObject>("getApplicationContext");

        //bool mIsPlaying;
        int isRingerMode = 0;
        string strRingerMod = string.Empty;
        using (AndroidJavaObject audioManager = unityContext.Call<AndroidJavaObject>("getSystemService", AUDIO_SERVICE))
        {
            isRingerMode = audioManager.Call<int>("getRingerMode");            
        }
        
        return isRingerMode == 2;
    }
}
