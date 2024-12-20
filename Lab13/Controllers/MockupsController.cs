using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Lab13.Models;
using Lab13.Utils;
using System;

namespace Lab13.Controllers
{
    public class MockupsController : Controller
    {
        private const int TotalQuestions = 4;
        private readonly ILogger<MockupsController> _logger;

        public MockupsController(ILogger<MockupsController> logger)
        {
            _logger = logger;
        }

        private QuizQuestion GenerateQuestion()
        {
            var random = new Random();
            var operationIndex = random.Next(0, 4);
            string operation = operationIndex switch
            {
                0 => "+",
                1 => "-",
                2 => "*",
                3 => "/",
                _ => "+"
            };

            int num1 = random.Next(0, 10);
            int num2 = operation == "/" ? random.Next(1, 10) : random.Next(0, 10);

            var question = new QuizQuestion
            {
                Num1 = num1,
                Num2 = num2,
                Operation = operation
            };

            _logger.LogInformation($"Generated question: {num1} {operation} {num2}");
            return question;
        }

        [HttpGet]
        public IActionResult Quiz()
        {
            var quizManager = new QuizSessionManager(HttpContext.Session);

            if (quizManager.HasReachedMaxQuestions(TotalQuestions))
            {
                _logger.LogInformation("Maximum questions reached. Redirecting to QuizResult.");
                return RedirectToAction("QuizResult");
            }

            var question = GenerateQuestion();
            return View(question);
        }

        [HttpPost]
        public IActionResult Quiz(QuizQuestion question)
        {
            var quizManager = new QuizSessionManager(HttpContext.Session);

            if (!ModelState.IsValid)
            {
                return View(question);
            }

            quizManager.AddQuestion(question);

            if (quizManager.HasReachedMaxQuestions(TotalQuestions))
            {
                return RedirectToAction("QuizResult");
            }

            ModelState.Clear();

            var newQuestion = GenerateQuestion();
            return View(newQuestion);
        }

        public IActionResult QuizResult()
        {
            var quizManager = new QuizSessionManager(HttpContext.Session);
            var questions = quizManager.GetAllQuestions();

            foreach (var q in questions)
            {
                _logger.LogInformation("Stored question: {Num1} {Operation} {Num2} = {UsersAnswer}, Correct: {CorrectAnswer}",
                    q.Num1, q.Operation, q.Num2, q.UsersAnswer, q.CorrectAnswer);
            }

            var correctAnswers = questions.Count(q => q.UsersAnswer == q.CorrectAnswer);

            ViewBag.CorrectAnswers = correctAnswers;
            ViewBag.TotalQuestions = questions.Count;

            quizManager.Clear(); 

            return View(questions);
        }

    }
}
