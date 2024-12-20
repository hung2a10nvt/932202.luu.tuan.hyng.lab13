using System.ComponentModel.DataAnnotations;

namespace Lab13.Models
{
    public class QuizQuestion
    {
        public int Num1 { get; set; }
        public int Num2 { get; set; }
        public string Operation { get; set; }

        [Required(ErrorMessage = "You must provide an answer.")]
        public int? UsersAnswer { get; set; }
        public int CorrectAnswer => Operation switch
        {
            "+" => Num1 + Num2,
            "-" => Num1 - Num2,
            "*" => Num1 * Num2,
            "/" => Num2 != 0 ? Num1 / Num2 : 0,
            _ => 0
        };
    }
}
