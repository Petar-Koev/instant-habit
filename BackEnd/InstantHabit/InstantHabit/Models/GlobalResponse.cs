namespace InstantHabit.Models
{
    public class GlobalResponse
    {
        public bool Succeeded { get; set; }
        public string Error { get; set; }

        public GlobalResponse(bool succeeded, string error)
        {
            Succeeded = succeeded;
            Error = error;
        }
    }
}
