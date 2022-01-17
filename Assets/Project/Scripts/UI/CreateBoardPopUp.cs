using Leopotam.EcsLite;
using Project.Infrastructure;

namespace Project.UI
{
    public sealed class CreateBoardPopUp : PopUpView
    {
        private EcsWorld _world;

        private void Awake()
        {
            _world = Worlds.Main;
        }
    }
}