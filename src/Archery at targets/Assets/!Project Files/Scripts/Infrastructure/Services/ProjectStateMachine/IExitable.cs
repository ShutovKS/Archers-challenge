namespace Infrastructure.Services.ProjectStateMachine
{
    public interface IExitable
    {
        public void OnExit();
    }
}