using UnityEngine;
using UnityEngine.Events;

namespace Util.UI
{
    public class MenuManager : MonoBehaviour
    {
        [SerializeField] private Menu[] menus;
        [SerializeField] private int defaultMenu = 0;
        [SerializeField] private UnityEvent onOpen;
        [SerializeField] private UnityEvent onClose;

        private bool _isOpen;
        private int _currentMenu;

        public void OpenMenu() => OpenMenuTo(defaultMenu);

        public void OpenMenuTo(int menuIdx)
        {
            foreach (var menu in menus)
            {
                menu.gameObject.SetActive(false);
            }

            menus[menuIdx].gameObject.SetActive(true);
            menus[menuIdx].OnMenuEnabled(() => { });

            _currentMenu = menuIdx;
            _isOpen = true;
            onOpen.Invoke();
        }

        public void ToggleMenu()
        {
            if (_isOpen)
            {
                CloseMenu();
            }
            else
            {
                OpenMenu();
            }
        }

        public void SwitchMenu(int nextMenu)
        {
            if (!_isOpen)
            {
                Debug.LogError($"cannot switch to menu {nextMenu} as menu is not open");
                return;
            }

            if (nextMenu >= menus.Length || nextMenu < 0)
            {
                Debug.LogError($"menu {nextMenu} is out of range. only {menus.Length} configured");
                return;

            }

            menus[_currentMenu].OnSwitchedFrom(() =>
            {
                menus[_currentMenu].gameObject.SetActive(false);
                menus[nextMenu].gameObject.SetActive(true);
                menus[nextMenu].OnSwitchedTo(() => { });
                _currentMenu = nextMenu;
            });
        }

        public void CloseMenu()
        {
            menus[_currentMenu].OnMenuDisabled(() =>
            {
                _isOpen = false;
                onClose.Invoke();
            });
        }
    }
}
