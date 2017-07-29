using UnityEngine;

namespace Lunaria
{
    public class AppInitialization
    {
        private static bool _initialized = false;

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void Initialize()
        {
            if (_initialized)
            {
                return;
            }

            _initialized = true;
            ServiceHolder.Instance.Hold<IMessageService>(new MessageService());
        }
    }
}
