#if PUN_2_OR_NEWER
using Photon.Pun;
#endif

namespace DoubTech.SyntyIntegrations.FinalIK
{
    public class PhotonBodyFitter : BodyFitter
#if PUN_2_OR_NEWER
        , IPunObservable
#endif
    {
#if PUN_2_OR_NEWER
        public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
        {
            if (stream.IsReading)
            {
                bodyFit = (BodyFit) stream.ReceiveNext();
                UpdateScales();
            }
            else
            {
                stream.SendNext(bodyFit);
            }
        }
#endif
    }
}
