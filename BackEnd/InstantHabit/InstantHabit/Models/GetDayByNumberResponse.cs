namespace InstantHabit.Models
{
    public class GetDayByNumberResponse : GlobalResponse
    {
        public Day Day { get; set; }

        public GetDayByNumberResponse(Day day, bool succeeded, string error) : base(succeeded, error)
        {
            Day = day;
        }
        public GetDayByNumberResponse(bool succeeded, string error) : base(succeeded, error)
        {

        }
    }
}
