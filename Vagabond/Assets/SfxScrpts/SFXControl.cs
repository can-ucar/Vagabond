using System;
using UnityEngine;
using UnityEngine.Audio;
using Random = UnityEngine.Random;

public class SFXControl : MonoBehaviour
{

    private void Start()
    {
        PlaySound(1);
    }

    void OnEnable()
    {
        SfxEventON();
    }

    void OnDisable()
    {
        SfxEventOFF();
    }
    
    public void SfxEventON()
    {
        BlockSpawner.RunSfx += PlaySound;
        BlockController.RunSFX2 += PlaySound;
    }
    
    public void SfxEventOFF()
    {
        BlockSpawner.RunSfx -= PlaySound;
        BlockController.RunSFX2 -= PlaySound;
    }
    

    private void PlaySound(int val)
    {
        if (DataReceiver.GetSoundStatus())
        {
            if (val == 1)
            {
                PlaySfx(SfxNames.Music);
            }
            else if (val == 2)
            {
                PlaySfx(SfxNames.Hit);
            }
            else if (val == 3)
            {
                PlaySfx(SfxNames.Money);
            }
            else if (val == 4)
            {
                PlaySfx(SfxNames.LevelUp);
            }
            else if (val == 5)
            {
                PlaySfx(SfxNames.Bomb);
            }
        }
    }
    
        public enum SfxNames
        {
            Null,
            Music,
            Hit,
            Money,
            LevelUp,
            Bomb
        }

        //public SfxNames sfxNames;

        [System.Serializable]
        public class Sfxs
        {
            public string name = "Null";

            public enum Type
            {
                Sfx,
                Music,
                SfxOther
            }

            public Type type;
            public AudioClip[] audioClip;
            public bool loop;
            public bool pitch;
            public bool check;
            [Range(1, 2)] public float pitchMin = 1f;
            [Range(1, 2)] public float pitchMax = 2f;
            [Range(0, 1)] public float volume = 1f;
            public AudioSource audioSourceLast;
        }

        public GameObject sfxPrefab;
        public Sfxs[] sfxs;


        public void PlaySfx(SfxNames sfxName)
        {
            CheckSfxName(sfxName,0);
        }

        public void StopSfx(SfxNames sfxName)
        {
            CheckSfxName(sfxName,1);
        }

        private void CheckSfxName(SfxNames sfxName, int stat)
        {
            int selectedSfx = 0;
            foreach (var sfx in sfxs)
            {
                if (sfx.name == sfxName.ToString())
                {
                    break;
                }

                selectedSfx += 1;
            }

            bool dataCheck = true;

            if (stat == 0)
            {
                if (dataCheck)
                {
                    if (sfxs[selectedSfx].check)
                    {
                        if (sfxs[selectedSfx].audioSourceLast == null)
                            AudioSourcePlay(sfxs[selectedSfx]);
                        else
                        {
                            sfxs[selectedSfx].audioSourceLast.Play();
                        }
                    }
                    else
                        AudioSourcePlay(sfxs[selectedSfx]);
                }
            }
            else
            {
                if (sfxs[selectedSfx].audioSourceLast != null)
                {
                    if (stat == 1)
                    {
                        sfxs[selectedSfx].audioSourceLast.Stop();
                        Destroy(sfxs[selectedSfx].audioSourceLast.gameObject);
                    }
                    else if (stat == 2)
                    {
                        sfxs[selectedSfx].audioSourceLast.volume = 0.55f;
                    }
                    else if (stat == 3)
                    {
                        sfxs[selectedSfx].audioSourceLast.volume = 1f;
                    }
                    else
                    {
                        sfxs[selectedSfx].audioSourceLast.mute = true;
                    }
                }
            }
        }

        private void AudioSourcePlay(Sfxs sfx)
        {
            var audio = Instantiate(sfxPrefab, transform.position, Quaternion.identity,transform);
            var audioSource = audio.GetComponent<AudioSource>();
            audioSource.clip = sfx.audioClip[Random.Range(0, sfx.audioClip.Length)];
            audioSource.loop = sfx.loop;
            audioSource.volume = sfx.volume;
            
            string groupName = "Master";
            switch (sfx.type)
            {
                case Sfxs.Type.Music:
                    groupName = "Music";
                    break;
                case Sfxs.Type.Sfx:
                    groupName = "Sfx";
                    break;
                case Sfxs.Type.SfxOther:
                    groupName = "SfxOthers";
                    break;
            }
            
            AudioMixer audioMixer = audioSource.outputAudioMixerGroup.audioMixer;
            AudioMixerGroup[] audioMixGroup = audioMixer.FindMatchingGroups(""+groupName);
            audioSource.outputAudioMixerGroup = audioMixGroup[0];
            float pitch = 1;
            if (sfx.pitch)
            {
                pitch = Random.Range(sfx.pitchMin, sfx.pitchMax);
            }

            audioSource.pitch = pitch;
            sfx.audioSourceLast = audioSource;
            sfx.audioSourceLast.Play();
            if (!sfx.loop)
                audio.AddComponent<SFXDestroyer>();
        }

        public void VolumeDown(SfxNames sfxName)
        {
            CheckSfxName(sfxName,2);
        }
        public void VolumeUp(SfxNames sfxName)
        {
            CheckSfxName(sfxName,3);
        }

        public void MuteSfx(SfxNames sfxName)
        {
            CheckSfxName(sfxName,4);
        }

        public void UnMute(SfxNames sfxName)
        {
            CheckSfxName(sfxName, 5);
        }
    }
