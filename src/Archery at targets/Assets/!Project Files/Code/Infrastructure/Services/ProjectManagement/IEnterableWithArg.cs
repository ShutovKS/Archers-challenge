namespace Infrastructure.Services.ProjectManagement
{
    public interface IEnterableWithArg<in T0>
    {
        public void OnEnter(T0 arg);
    }
}