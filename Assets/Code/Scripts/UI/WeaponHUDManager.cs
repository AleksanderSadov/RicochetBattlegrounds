using Unity.Ricochet.Gameplay;
using UnityEngine;

namespace Unity.Ricochet.UI
{
    public class WeaponHUDManager : MonoBehaviour
    {
        [SerializeField] GameObject selectorKnife;
        [SerializeField] GameObject selectorGun;

        private PlayerBehavior playerBehavior;

        private void Start()
        {
            playerBehavior = FindObjectOfType<PlayerBehavior>();

            playerBehavior.OnWeaponChanged += SelectWeapon;
        }

        public void SelectWeapon(PlayerWeaponType weaponType)
        {
            switch (weaponType)
            {
                case PlayerWeaponType.KNIFE:
                    selectorKnife.SetActive(true);
                    selectorGun.SetActive(false);
                    break;
                case PlayerWeaponType.PISTOL:
                    selectorKnife.SetActive(false);
                    selectorGun.SetActive(true);
                    break;
            }
        }
    }
}
