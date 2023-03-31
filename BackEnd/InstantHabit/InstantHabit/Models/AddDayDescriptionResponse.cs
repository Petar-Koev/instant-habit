namespace InstantHabit.Models
{
    public class AddDayDescriptionResponse
    {
        public bool Succeeded { get; set; }
        public string Error { get; set; }

        public AddDayDescriptionResponse(bool succeeded, string error)
        {
            Succeeded = succeeded;
            Error = error;
        }
    }
}
