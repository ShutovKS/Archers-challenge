namespace Infrastructure.Services.ProjectStateMachine
{
    public interface IEnterableWithOneArg<in T0>
    {
        public void OnEnter(T0 arg);
    }
}