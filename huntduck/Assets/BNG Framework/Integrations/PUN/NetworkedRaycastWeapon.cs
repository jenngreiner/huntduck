#if PUN_2_OR_NEWER
using Photon.Pun;
#endif
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if PUN_2_OR_NEWER
namespace BNG {
    public class NetworkedRaycastWeapon : RaycastWeapon, IPunObservable {

        PhotonView photonView;

        void Start() {
            photonView = GetComponent<PhotonView>();
        }

        public override void Shoot() {
            if (photonView) {
                var playEmptySound = !BulletInChamber && MustChamberRounds && !playedEmptySound || ws != null && ws.LockedBack;
                photonView.RPC("ShootRPC", RpcTarget.Others, playEmptySound);
            }

            base.Shoot();
        }

        [PunRPC]
        private void ShootRPC(bool playEmptySound) {
            if (playEmptySound) {
                VRUtils.Instance.PlaySpatialClipAt(EmptySound, transform.position, EmptySoundVolume, 0.5f);
                return;
            }

            VRUtils.Instance.PlaySpatialClipAt(GunShotSound, transform.position, GunShotVolume);

             DoMuzzleFlash();
             if (AutoChamberRounds) {
                 // Animate Slide
                 if (ws) {
                     ws.BlowbackSlide(0.1f);
                 }

                 // Eject Shell Slightly after slide is back
                 Invoke("EjectShell", 0.05f);
             }
        }

        public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info) {
            // This is our object, send our positions to the other players
            if (stream.IsWriting && photonView.IsMine) {
                // Send Weapon state such as firing, ammo, etc.
                //stream.SendNext(transform.position);
               
            }
            // Receive updates
            else {
               
            }
        }

    }
}
#endif