namespace InstantHabit.Models
{
    public class DeleteDayResponse
    {
        public bool Succeeded { get; set; }
        public string Error { get; set; }

        public DeleteDayResponse(bool succeeded, string error)
        {
            Succeeded = succeeded;
            Error = error;
        }
    }
}
