using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Lab13.Models;

namespace Lab13.Utils
{
    public class QuizSessionManager
    {
        private readonly ISession _session;
        private const string QuestionsKey = "Questions";

        public QuizSessionManager(ISession session)
        {
            _session = session;
        }

        public void AddQuestion(QuizQuestion question)
        {
            var questions = GetAllQuestions() ?? new List<QuizQuestion>();

            // Tạo một bản sao mới của câu hỏi
            var newQuestion = new QuizQuestion
            {
                Num1 = question.Num1,
                Num2 = question.Num2,
                Operation = question.Operation,
                UsersAnswer = question.UsersAnswer
            };

            questions.Add(newQuestion);
            Console.WriteLine($"Added question: {newQuestion.Num1} {newQuestion.Operation} {newQuestion.Num2}");
            _session.Set(QuestionsKey, questions);
        }

        public List<QuizQuestion> GetAllQuestions()
        {
            return _session.Get<List<QuizQuestion>>(QuestionsKey) ?? new List<QuizQuestion>();
        }

        public bool HasReachedMaxQuestions(int maxQuestions)
        {
            var questions = GetAllQuestions();
            return questions.Count >= maxQuestions;
        }

        public void Clear()
        {
            _session.Remove(QuestionsKey); // Xóa danh sách khỏi Session
        }
    }
}
