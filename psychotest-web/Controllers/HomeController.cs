using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using psychotest.Model;
using psychotest_web.Models;

namespace psychotest_web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        public const string SessionKeyProbeResult = "UserProbeResult";
        public const string SessionKeyIdValid = "IdValid";
        public const string SessionKeyQuizStartTime = "QuizStartTime";
        public const string SessionKeyProbeBegun = "ProbeBegun";
        public const string SessionKeyProbeDone = "ProbeDone";
        public const string SessionKeyQuizBegun = "QuizBegun";
        public const string SessionKeyQuizDone = "QuizDone";

        public string UserID { get; set; }
        public SurveyResult UserSurvey { get; set; }

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }


        [Route("/")]
        [Route("/hello")]
        public IActionResult Hello()
        {

            var model = new UserIDModel
            {
                idValidationResult = HttpContext.Session.Get<IdValidationResult>(SessionKeyIdValid)
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> IDform(UserIDModel model)
        {

            string userId = model.UserID;
            // validation
            var IdValidationResult = await ValidateID(userId);
            if (IdValidationResult == IdValidationResult.Valid)
            {
                string group = "";
                try 
                {
                    var probe = await Database.GetProbe(model.UserID);
                    group = probe.Group;
                }
                catch(Exception)
                {
                    group = "";
                }
                var probeResult = new ProbeResult
                {
                    ID = model.UserID,
                    Group = group
                };

                
                HttpContext.Session.Set(SessionKeyProbeResult, probeResult);

                ProbeBegin();
                

                return RedirectToAction("Quiz");
            }
            else
            {
                Console.WriteLine("ID invalid");
                HttpContext.Session.Set(SessionKeyIdValid, IdValidationResult);
                return RedirectToAction("Hello");
            }
        }

        [Route("/quiz")]
        public IActionResult Quiz()
        {
            if (QuizEnabled())
            {
                var model = new QuizViewModel
                {
                    Quests = QuestCreator.CreateDefaultQuests(50),
                    TutorialQuests = QuestCreator.CreateInstructionQuests(),
                };

                QuizBegin();

                return View(model);
            }

            else
                return RedirectToAction("Hello");
            
        }

        [HttpPost]
        public IActionResult QuizForm(QuizViewModel quizViewModel)
        {
            // calculate time
            var endTime = DateTime.Now;
            var beginTime = HttpContext.Session.Get<DateTime>(SessionKeyQuizStartTime);
            var quizTime = (int)endTime.Subtract(beginTime).TotalSeconds;
            Console.WriteLine("Time of quiz: " + quizTime.ToString());

            //calculate score
            var questsFromUser = quizViewModel.Quests;
            var originalQuests = QuestCreator.CreateDefaultQuests(50);
            var score = CalculateScore(questsFromUser, originalQuests);
            Console.WriteLine("Wynik: " + score.ToString());

            // write results to session probe result
            var probeResult = HttpContext.Session.Get<ProbeResult>(SessionKeyProbeResult);
            probeResult.Time = quizTime;
            probeResult.Score = score;
            HttpContext.Session.Set(SessionKeyProbeResult, probeResult);

            QuizDone();

            return RedirectToAction("Survey");
        }



        [Route("/survey")]
        public IActionResult Survey()
        {
            if (SurveyEnabled())
                return View();
            else
                return RedirectToAction("Hello");
        }

        [HttpPost]
        public async Task<IActionResult> SurveyForm([FromForm]SurveyResult survey)
        {
            var probeResult = HttpContext.Session.Get<ProbeResult>(SessionKeyProbeResult);

            probeResult.Q1Answer = survey.Q1Answer;
            probeResult.Q2Answer = survey.Q2Answer;
            probeResult.Q3Answer = survey.Q3Answer;
            probeResult.Q4Answer = survey.Q4Answer;
            probeResult.Q5Answer = survey.Q5Answer;
            probeResult.Q6Answer = survey.Q6Answer;
            probeResult.Q7Answer = survey.Q7Answer;
            probeResult.Q8Answer = survey.Q8Answer;
            probeResult.Q9Answer = survey.Q9Answer;
            probeResult.Email = survey.Email;

            probeResult.Done = true;

            Console.WriteLine("Pyt. 1: " + survey.Q1Answer);
            Console.WriteLine("Pyt. 1: " + survey.Q2Answer);
            Console.WriteLine("Pyt. 1: " + survey.Q3Answer);
            Console.WriteLine("Pyt. 1: " + survey.Q4Answer);
            Console.WriteLine("Pyt. 1: " + survey.Q5Answer);
            Console.WriteLine("Pyt. 1: " + survey.Q6Answer);
            Console.WriteLine("Pyt. 1: " + survey.Q7Answer);
            Console.WriteLine("Pyt. 1: " + survey.Q8Answer);
            Console.WriteLine("Pyt. 1: " + survey.Q9Answer);


            HttpContext.Session.Set(SessionKeyProbeResult, probeResult);
            await Database.PostProbe(probeResult);

            ProbeDone();

            return RedirectToAction("Summary");
        }

        [Route("/summary")]
        public IActionResult Summary()
        {
            if (SummaryEnabled())
            {
                var probeResult = HttpContext.Session.Get<ProbeResult>(SessionKeyProbeResult);
                Console.WriteLine("Wyniki badania.\n" +
                    "ID: {0}, time: {1}, score: {2}, survey: {3}, {4}, {5}, {6}, {7}",
                    probeResult.ID, probeResult.Time.ToString(), probeResult.Score.ToString(),
                    probeResult.Q1Answer, probeResult.Q2Answer, probeResult.Q3Answer, probeResult.Q4Answer, probeResult.Q5Answer);
                return View();
            }
            else
                return RedirectToAction("Hello");
            
        }

        public int CalculateScore(List<Quest> userQuests, List<Quest> correctQuests) 
        {
            int score = 0;
            for (int i = 0; i < userQuests.Count; i++)
            {
                int userAnswer;
                if (int.TryParse(userQuests[i].UserAnswer, out userAnswer))
                {
                    if (userAnswer == correctQuests[i].CorrectAnswer)
                    {
                        score++;
                    }
                }
            }

            return score;
        }

        public async Task<IdValidationResult> ValidateID(string id)
        {
            if (id == null)
                return IdValidationResult.IdIncorrect;

            if (!Regex.IsMatch(id, @"^\d{10}$"))
                return IdValidationResult.IdIncorrect;

            try
            {
                var probe = await Database.GetProbe(id);
                if (probe.Done)
                    return IdValidationResult.IdUsed;
                else
                    return IdValidationResult.Valid;
            }
            catch (ArgumentOutOfRangeException)
            {
                return IdValidationResult.IdIncorrect;
            }
            catch (Exception)
            {
                throw;
            }

        }

        // Condition for accessing pages
        private bool QuizEnabled()
        {
            var probeBegun = HttpContext.Session.Get<bool>(SessionKeyProbeBegun);
            var quizDone = HttpContext.Session.Get<bool>(SessionKeyQuizDone);
            return probeBegun && !quizDone;
        }

        private bool SurveyEnabled()
        {
            var probeBegun = HttpContext.Session.Get<bool>(SessionKeyProbeBegun);
            var quizDone = HttpContext.Session.Get<bool>(SessionKeyQuizDone);

            return probeBegun && quizDone;
        }

        private bool SummaryEnabled()
        {
            var quizDone = HttpContext.Session.Get<bool>(SessionKeyQuizDone);
            var probeDone = HttpContext.Session.Get<bool>(SessionKeyProbeDone);

            return quizDone && probeDone;
        }
        // actions

        private void ProbeBegin() // after IDform
        {
            HttpContext.Session.Set(SessionKeyProbeBegun, true);
            HttpContext.Session.Set(SessionKeyProbeDone, false);
        }

        private void ProbeDone() // after SurveyForm
        {
            HttpContext.Session.Set(SessionKeyProbeBegun, false);
            HttpContext.Session.Set(SessionKeyProbeDone, true);
        }

        private void QuizBegin() // after IDForm
        {
            HttpContext.Session.Set(SessionKeyQuizStartTime, DateTime.Now);
            HttpContext.Session.Set(SessionKeyQuizBegun, true);
        }

        private void QuizDone() // after QuizForm
        {
            HttpContext.Session.Set(SessionKeyQuizBegun, false);
            HttpContext.Session.Set(SessionKeyQuizDone, true);
        }

    }
}
