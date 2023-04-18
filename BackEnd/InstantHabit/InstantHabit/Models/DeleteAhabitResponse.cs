namespace InstantHabit.Models
{
    public class DeleteAhabitResponse : GlobalResponse
    {
        public DeleteAhabitResponse(bool succeeded, string error) : base(succeeded, error)
        {
        }
    }
}
