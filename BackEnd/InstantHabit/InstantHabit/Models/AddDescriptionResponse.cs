namespace InstantHabit.Models
{
    public class AddDescriptionResponse
    {
        public bool Succeeded { get; set; }
        public string Error { get; set; }

        public AddDescriptionResponse(bool succeeded, string error)
        {
            Succeeded = succeeded;
            Error = error;
        }
    }
}
