namespace Data.Configurations.Weapon
{
    public interface ICustomizable
    {
        string Key { get; }
        void Validate();
        void GenerateKey();
    }
}