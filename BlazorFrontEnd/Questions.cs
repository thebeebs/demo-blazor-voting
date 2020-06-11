namespace BlazorFrontEnd
{
    public class Question
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Answer1 { get; set; }
        public string Answer2 { get; set; }
        public string Answer1Picture { get; set; }
        public string Answer2Picture { get; set; }
        public int Answer1Count { get; set; }
        public int Answer2Count { get; set; }     
    }

    public class VoteResponse
    {
        public int QuestionId { get; set; }
        public int Answer1 { get; set; }
        public int Answer2 { get; set; }

    }
}