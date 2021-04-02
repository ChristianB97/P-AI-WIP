using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.UI;
using UnityEngine.Rendering.Universal;
using UnityEngine.Rendering;

public class Geheimskript : MonoBehaviour
{
    public float effectTimer;
    public VideoPlayer videoPlayer;
    public VideoClip[] clip;
    public int i;
    float cooldown;
    float maxCooldown = 0.5f;

    public Material material;
    public RawImage rawImage;
    public GameObject sun;
    public GameObject time;
    public VolumeProfile volumeProfile;
    private float vandaniel = 0.217f;
    public float vandanielStarter = 0f;
    public Material defaultMaterial;

    void Start()
    {
        vandanielStarter = 0f;
        i = -1;
        cooldown = maxCooldown;
        PlayNext();

        LensDistortion lens;
        if (!volumeProfile.TryGet(out lens)) throw new System.NullReferenceException(nameof(lens));
        lens.active = true;
        lens.intensity.Override(0);
        lens.active = false;
        Vignette vignette;
        if (!volumeProfile.TryGet(out vignette)) throw new System.NullReferenceException(nameof(vignette));
        vignette.active = false;
        ColorAdjustments ca;
        if (!volumeProfile.TryGet(out ca)) throw new System.NullReferenceException(nameof(ca));
        ca.active = false;
        WhiteBalance balance;
        if (!volumeProfile.TryGet(out balance)) throw new System.NullReferenceException(nameof(balance));
        balance.active = false;
        FilmGrain grain;
        if (!volumeProfile.TryGet(out grain)) throw new System.NullReferenceException(nameof(grain));
        grain.active = false;
    }

    private void Update()
    {
        if (!videoPlayer.isPlaying && i % 2 == 1)
            PlayNext();
        if (Input.GetKeyDown("p"))
        {
            cooldown = maxCooldown;
            PlayNext();
        }
        else if (Input.GetKeyDown("o"))
        {
            cooldown = maxCooldown;
            PlayPrevious();
        }
        if (i == clip.Length - 1 && vandanielStarter < vandaniel)
        {
            vandanielStarter += (0.03f * Time.deltaTime);
            LensDistortion lens;
            if (!volumeProfile.TryGet(out lens)) throw new System.NullReferenceException(nameof(lens));
            lens.intensity.Override(vandanielStarter);
            lens.active = true;
        }
    }

    private void PlayNext()
    {
        if (i < clip.Length-1)
        {
            i++;
            print(i + " / " + clip.Length);
            videoPlayer.clip = clip[i];
            videoPlayer.Prepare();
            videoPlayer.Stop();
            videoPlayer.Play();
            if (i == clip.Length - 1)
            {
                StartCoroutine(StartEffects());
            }
        }
    }

    private void PlayPrevious()
    {
        if (i >= 2)
        {
            i--;
            i--;
            videoPlayer.clip = clip[i];
            videoPlayer.Prepare();
            videoPlayer.Stop();
            videoPlayer.Play();
            Vignette vignette;
            if (!volumeProfile.TryGet(out vignette)) throw new System.NullReferenceException(nameof(vignette));
            vignette.active = false;
            ColorAdjustments ca;
            if (!volumeProfile.TryGet(out ca)) throw new System.NullReferenceException(nameof(ca));
            ca.active = false;
            WhiteBalance balance;
            if (!volumeProfile.TryGet(out balance)) throw new System.NullReferenceException(nameof(balance));
            balance.active = false;
            FilmGrain grain;
            if (!volumeProfile.TryGet(out grain)) throw new System.NullReferenceException(nameof(grain));
            grain.active = false;
            LensDistortion lens;
            if (!volumeProfile.TryGet(out lens)) throw new System.NullReferenceException(nameof(lens));
            lens.intensity.Override(vandanielStarter);
            lens.active = false;
            sun.SetActive(false);
            time.SetActive(false);
            rawImage.material = defaultMaterial;
        }
    }

    private IEnumerator StartEffects()
    {
        yield return new WaitForSeconds(effectTimer);
        rawImage.material = material;
        
        Vignette vignette;
        if (!volumeProfile.TryGet(out vignette)) throw new System.NullReferenceException(nameof(vignette));
        vignette.active = true;
        yield return new WaitForSeconds(0.5f);
        ColorAdjustments ca;
        if (!volumeProfile.TryGet(out ca)) throw new System.NullReferenceException(nameof(ca));
        ca.active = true;
        yield return new WaitForSeconds(0.5f);
        WhiteBalance balance;
        if (!volumeProfile.TryGet(out balance)) throw new System.NullReferenceException(nameof(balance));
        balance.active = true;
        yield return new WaitForSeconds(0.5f);
        FilmGrain grain;
        if (!volumeProfile.TryGet(out grain)) throw new System.NullReferenceException(nameof(grain));
        grain.active = true;
        sun.SetActive(true);
        time.SetActive(true);
    }
}
