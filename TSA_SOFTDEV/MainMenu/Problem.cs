using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Web;

namespace MainMenu
{
    public class Problem
    {
        public string Question { get; private set; }
        public string Answer { get; private set; }
        public bool HasAnswer { get; private set; }
        public Problem(string question)
        {
            HasAnswer = false;
            Question = question;
            Thread thread = new Thread(() => 
            {
                Answer = Core.External.Wolfram.GetSolution(HttpUtility.HtmlEncode(Question)); HasAnswer = true;
            });
            thread.Start();
        }
        public Problem(string question, string answer)
        {
            Question = question;
            Answer = answer;
        }
        public bool CompareAnswer(string answer)
        {
            string s = Core.External.Wolfram.GetSolution(HttpUtility.HtmlEncode($"{answer} == {Answer}"));
            Console.WriteLine();
            return s.ToLower().Contains("yes");
        }
    }
}
