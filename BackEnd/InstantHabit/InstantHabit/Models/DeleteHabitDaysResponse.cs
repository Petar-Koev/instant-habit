namespace InstantHabit.Models
{
    public class DeleteHabitDaysResponse
    {
        public bool Succeeded { get; set; }
        public string Error { get; set; }

        public DeleteHabitDaysResponse(bool succeeded, string error)
        {
            Succeeded = succeeded;
            Error = error;
        }
    }
}
