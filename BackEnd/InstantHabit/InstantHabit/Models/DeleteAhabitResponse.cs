namespace InstantHabit.Models
{
    public class DeleteAhabitResponse
    {
        public bool Succeeded { get; set; }
        public string Error { get; set; }

        public DeleteAhabitResponse(bool succeeded, string error)
        {
            Succeeded = succeeded;
            Error = error;
        }
    }
}
