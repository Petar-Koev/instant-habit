namespace InstantHabit.Models
{
    public class ExtendHabitResponse
    {
            public bool Succeeded { get; set; }
            public string Error { get; set; }

            public ExtendHabitResponse(bool succeeded, string error)
            {
                Succeeded = succeeded;
                Error = error;
            }
        
    }
}

