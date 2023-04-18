namespace InstantHabit.Models
{
    public class AddDayResponse : GlobalResponse
    {
        public AddDayResponse(bool succeeded, string error) : base(succeeded, error) { }
    }
}
