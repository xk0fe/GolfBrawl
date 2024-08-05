using Fusion;

namespace Source.GameInput
{
    public struct NetworkInput : INetworkInput
    {
        public float Horizontal { get; set; }
        public float Vertical { get; set; }
        public bool Jump { get; set; }
    }
}