namespace Model
{
    public class Queztion
    {
        public virtual int Id { get; set; }
        public virtual string Question { get; set; }
        public virtual string Answers { get; set; }
        public virtual string CorrectAnswer { get; set; }
    }
}
