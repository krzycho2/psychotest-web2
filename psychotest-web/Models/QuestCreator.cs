using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace psychotest.Model
{
    public static class QuestCreator
    {
        public static List<Quest> CreateDefaultQuests(int numOfQuests)
        {
            var quests = new List<Quest>();
            for (int questNum = 0; questNum < numOfQuests; questNum++)
            {
                var shapeNum = DEFAULT_QUEST_CHOICE[questNum];
                quests.Add(new Quest 
                {
                    shape = new Shape(shapeNum),
                    UserAnswer = "0"
                    
                });
            }

            return quests;
        }

        public static List<Quest> CreateInstructionQuests()
        {
            var quests = new List<Quest>();
            for (int shapeNum = 1; shapeNum <= 10; shapeNum++)
            {
                var quest = new Quest
                {
                    shape = new Shape(shapeNum)
                };
                quest.UserAnswer = quest.CorrectAnswer.ToString();
                quests.Add(quest);
            }

            return quests;
        }

        public static List<Quest> CreateExampleInstructionQuests()
        {
            var quests = new List<Quest>();
            for (int shapeNum = 1; shapeNum <= 5; shapeNum++)
            {
                var quest = new Quest
                {
                    shape = new Shape(shapeNum)
                };
                quest.UserAnswer = quest.CorrectAnswer.ToString();
                quests.Add(quest);
            }

            return quests;
        }

        public static List<Quest> CreateExampleQuests()
        {
            var quests = new List<Quest>();
            int[] shapeNums = { 2, 1, 1, 3, 4 };
            foreach(var shapeNum in shapeNums)
            {
                var quest = new Quest
                {
                    shape = new Shape(shapeNum)
                };
                quest.UserAnswer = quest.CorrectAnswer.ToString();
                quests.Add(quest);
            }

            return quests;
        }

        public static List<int> DEFAULT_QUEST_CHOICE
        {
            get
            {
                return new List<int> { 2, 7, 3, 5, 4, 5, 5, 1, 6, 4, 2, 1, 10, 3, 4, 4, 2, 7, 6, 4, 6, 10, 3, 7, 5, 7, 9, 2, 7, 10, 7, 3, 1, 10, 3, 10, 7, 6, 6, 7, 9, 8, 4, 7, 9, 4, 1, 5, 8, 8, 3, 5, 1, 4, 2, 2, 2, 10, 7, 6, 4, 2, 7, 2, 3, 8, 7, 7, 5, 8, 4, 1, 4, 3, 3, 5, 8, 10, 6, 3, 2, 2, 8, 7, 7, 7, 2, 4, 3, 9, 10, 6, 8, 9, 7, 8, 10, 6, 10, 5, 8, 10, 4, 4, 4, 8, 7, 5, 9, 1, 3, 2, 4, 1, 5, 10, 3, 5, 5, 3, 8, 3, 1, 6, 2, 7, 6, 9, 6, 9, 2, 3, 10, 1, 5, 8, 7, 1, 1, 10, 6, 4, 5, 6, 9, 2, 5, 3, 5, 1, 1, 5, 1, 7, 2, 7, 9, 5, 5, 1, 4, 8, 6, 9, 2, 1, 7, 7, 10, 6, 6, 10, 3, 3, 6, 3, 4, 3, 7, 10, 5, 1, 2, 10, 8, 4, 4, 5, 9, 5, 9, 5, 7, 2, 9, 6, 4, 2, 9, 3, 5, 4, 5, 10, 7, 9, 3, 8, 9, 8, 8, 9, 7, 8, 9, 5, 8, 10, 2, 7, 7, 3, 10, 3, 2, 3, 10, 3, 1, 10, 6, 7, 1, 3, 4, 5, 3, 1, 6, 2, 10, 3, 6, 6, 10, 3, 9, 6, 4, 8 };
            }
        }
    }
}
