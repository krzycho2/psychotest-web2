using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace psychotest.Model
{
    public class GroupOf5Quests
    {
        public GroupOf5Quests(List<Quest> quests)
        {
            if (quests.Count % 5 != 0)
                throw new ArgumentException();

            First = quests[0];
            Second = quests[1];
            Third = quests[2];
            Fourth = quests[3];
            Fifth = quests[4];
            Quests = quests;
        }

        public List<Quest> Quests { get; }

        public Quest First { get; }
        public Quest Second { get; }
        public Quest Third { get; }
        public Quest Fourth { get; }
        public Quest Fifth { get; }
    }

    public static class GroupOf5QuestsConverter
    {
        public static List<GroupOf5Quests> ConvertToGroupOf5Quests(List<Quest> quests)
        {
            var outputList = new List<GroupOf5Quests>();
            int counter = 0;
            var tempList = new List<Quest>(); 
            for (int questNum=0; questNum<quests.Count; questNum++)
            {
                var quest = quests[questNum];
                if (counter < 5)
                {
                    tempList.Add(quest);
                    if(questNum == quests.Count -1)
                    {
                        outputList.Add(new GroupOf5Quests(tempList));
                        break;
                    }

                    counter++;
                }

                else
                {
                    
                    outputList.Add(new GroupOf5Quests(tempList));

                    tempList = new List<Quest>
                    {
                        quest
                    };

                    counter = 1;
                }
            }

            return outputList;

        }

        public static List<Quest> ConvertToQuests(List<GroupOf5Quests> groupedQuests)
        {
            var quests = new List<Quest>();
            foreach (var group in groupedQuests)
                foreach (var quest in group.Quests)
                    quests.Add(quest);

            return quests;
        }
    }
}
