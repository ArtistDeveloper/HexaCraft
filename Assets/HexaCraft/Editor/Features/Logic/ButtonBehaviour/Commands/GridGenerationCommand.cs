using UnityEngine;

namespace HexaCraft
{
    public class GridGenerationCommand : IBasicCommand
    {
        private GridGeneration _receiver;

        public GridGenerationCommand(GridGeneration receiver)
        {
            _receiver = receiver;
        }

        public void Execute()
        {
            
        }
    }
}
