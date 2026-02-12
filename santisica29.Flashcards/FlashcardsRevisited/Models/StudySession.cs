namespace FlashcardsRevisited.Models;
internal class StudySession
{
    public int StudySessionId { get; set; }
    public int Score { get; set; }
    public DateTime DateOfSession { get; set; }
    public StackDeck Stack { get; set; }
}
