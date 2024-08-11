using UnityEditor;

namespace Zenject
{
    [CanEditMultipleObjects]
    [CustomEditor(typeof(CompositeMonoInstaller))]
    [NoReflectionBaking]
    public class CompositeMonoInstallerEditor : BaseCompositetInstallerEditor<CompositeMonoInstaller, MonoInstallerBase>
    {
    }
}