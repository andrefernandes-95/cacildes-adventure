using System.Collections;
using UnityEngine;
using UnityEngine.Localization.Settings;
using UnityEngine.Video;

namespace AF
{

    public class EV_PlayVideo : EventBase
    {
        public GameObject videoRoot;

        public VideoPlayer videoPlayerEn;

        [Header("Optional")]
        public VideoPlayer videoPlayerPt;

        public VideoManager videoManager;

        public GenericTrigger genericTrigger;

        public override IEnumerator Dispatch()
        {
            VideoPlayer targetVideo = videoPlayerEn;

            if (videoPlayerPt != null)
            {
                if (LocalizationSettings.SelectedLocale.Equals(LocalizationSettings.AvailableLocales.GetLocale("pt")))
                {
                    videoPlayerEn.gameObject.SetActive(false);
                    targetVideo = videoPlayerPt;
                }
                else
                {
                    videoPlayerPt.gameObject.SetActive(false);
                }
            }

            targetVideo.gameObject.SetActive(true);
            videoManager.PlayVideo(videoRoot, targetVideo, genericTrigger);
            yield return null;
        }
    }

}
