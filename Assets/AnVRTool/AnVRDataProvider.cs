

namespace AnVRTool
{
    public interface IAnVRDataProvider<T>
    {
        bool isActive { get; }
        T GetData();
    }
}
