namespace InstantHabit.Models
{
    public class AddHabitResponse
    {
        public bool Succeeded { get; set; }
        public string Error { get; set; }

        public AddHabitResponse(bool succeeded, string error)
        {
            Succeeded = succeeded;
            Error = error;
        }
    }
}
