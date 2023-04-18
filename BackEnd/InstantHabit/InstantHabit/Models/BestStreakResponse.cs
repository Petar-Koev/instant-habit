namespace InstantHabit.Models
{
    public class BestStreakResponse : GlobalResponse
    {
        public int BestStreak { get; set; }
        public string MotivationalMessage { get; set; }

        public BestStreakResponse(int bestStreak, string motivationalMessage, bool succeeded, string error) : base(succeeded, error)
        {
            BestStreak = bestStreak;
            MotivationalMessage = motivationalMessage;  

        }

        public BestStreakResponse(bool succeeded, string error) : base(succeeded, error)
        {
        }
    }

   
}
