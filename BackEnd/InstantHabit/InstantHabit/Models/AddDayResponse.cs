namespace InstantHabit.Models
{
    public class AddDayResponse
    {
        public bool Succeeded { get; set; }
        public string Error { get; set; }

        public AddDayResponse(bool succeeded, string error)
        {
            Succeeded = succeeded;
            Error = error;
        }
    }
}
