using psychotest.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace psychotest_web.Models
{
    public class QuizViewModel
    {
        public List<Quest> Quests { get; set; }
        public IEnumerable<Quest> TutorialQuests { get; set; } = new List<Quest>();
        //public QuizResult UserAnswers { get; set; }
    }
}
