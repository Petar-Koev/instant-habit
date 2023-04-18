namespace InstantHabit.Models
{
    public class DeleteDayResponse : GlobalResponse
    {
        public DeleteDayResponse(bool succeeded, string error) : base(succeeded, error)
        {
        }
    }
}
