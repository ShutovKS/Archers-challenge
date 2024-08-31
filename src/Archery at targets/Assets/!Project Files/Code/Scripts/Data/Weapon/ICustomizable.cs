namespace Data.Weapon
{
    public interface ICustomizable
    {
        string Key { get; }
        void Validate();
        void GenerateKey();
    }
}