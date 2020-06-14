using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace psychotest.Model
{
    public class Quest
    {
        public Shape shape { get; set; }

        public int CorrectAnswer
        {
            get => shape.ShapeNum;
        }

        public int UserAnswerInt { get; private set; } = 0;

        public string UserAnswer 
        {
            get
            {
                if (UserAnswerInt == 0)
                    return "";

                else 
                    return UserAnswerInt.ToString();
            }
            set
            {
                if (value == "")
                    UserAnswerInt = 0;

                else
                {
                    if (int.TryParse(value, out int tempUserAnswer))
                    {
                        UserAnswerInt = tempUserAnswer;
                    }
                    else
                        UserAnswerInt = 0;
                }

                Console.WriteLine("Udzielenie odpowiedzi: " + UserAnswerInt.ToString());
            } 
        }

        public string ShapeIconPath
        {
            get => shape.IconPath;
        }

        public bool Done
        {
            get
            {
                if (UserAnswer == "")
                    return false;
                else
                    return true;
            }
        }
    }
}
