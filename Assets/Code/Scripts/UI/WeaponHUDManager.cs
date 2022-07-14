using Unity.Ricochet.Gameplay;
using UnityEngine;

namespace Unity.Ricochet.UI
{
    public class WeaponHUDManager : MonoBehaviour
    {
        [SerializeField] private GameObject selectorKnife;
        [SerializeField] private GameObject selectorGun;
        [SerializeField] private GameObject aimTip;

        private PlayerController playerBehavior;

        private void Start()
        {
            playerBehavior = FindObjectOfType<PlayerController>();

            playerBehavior.OnWeaponChanged += SelectWeapon;
        }

        public void SelectWeapon(PlayerWeaponType weaponType)
        {
            switch (weaponType)
            {
                case PlayerWeaponType.KNIFE:
                    selectorKnife.SetActive(true);
                    selectorGun.SetActive(false);
                    aimTip.SetActive(false);
                    break;
                case PlayerWeaponType.PISTOL:
                    selectorKnife.SetActive(false);
                    selectorGun.SetActive(true);
                    aimTip.SetActive(true);
                    break;
            }
        }
    }
}
