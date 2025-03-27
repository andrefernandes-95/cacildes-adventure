using System.Collections;
using UnityEngine;
using UnityEngine.Video;

namespace AF
{
    public class VideoManager : MonoBehaviour
    {
        Coroutine PlayVideoCoroutine;

        GameObject currentVideoRoot;
        GenericTrigger currentGenericTrigger;

        bool canSkipVideo = false;

        public void PlayVideo(GameObject videoRoot, VideoPlayer videoPlayer, GenericTrigger genericTrigger)
        {
            canSkipVideo = false;

            this.currentVideoRoot = videoRoot;
            this.currentGenericTrigger = genericTrigger;


            if (PlayVideoCoroutine != null)
            {
                StopCoroutine(PlayVideoCoroutine);
            }

            PlayVideoCoroutine = StartCoroutine(PlayVideo_Coroutine(videoPlayer));
        }
        public IEnumerator PlayVideo_Coroutine(VideoPlayer videoPlayer)
        {
            currentVideoRoot.gameObject.SetActive(true);

            if (currentGenericTrigger != null)
            {
                currentGenericTrigger.DisableCapturable();
            }

            yield return new WaitForSeconds(0.1f);
            canSkipVideo = true;
            yield return new WaitForSeconds((float)videoPlayer.clip.length);
            currentVideoRoot.gameObject.SetActive(false);

            if (currentGenericTrigger != null)
            {
                currentGenericTrigger.TurnCapturable();
            }

            currentVideoRoot = null;
            currentGenericTrigger = null;
            canSkipVideo = false;
        }

        public void CancelVideo()
        {
            if (!canSkipVideo || currentVideoRoot == null)
            {
                return;
            }

            currentVideoRoot.gameObject.SetActive(false);

            if (currentGenericTrigger != null)
            {
                currentGenericTrigger.TurnCapturable();
            }

            StopCoroutine(PlayVideoCoroutine);
            currentVideoRoot = null;
            canSkipVideo = false;
        }
    }
}
