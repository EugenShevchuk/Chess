using Leopotam.EcsLite;
using Project.Infrastructure;

namespace Project.UI
{
    public sealed class CreateBoardDisplayableUI : DisplayableUIView
    {
        private EcsWorld _world;

        private void Awake()
        {
            _world = Worlds.Main;
        }
    }
}