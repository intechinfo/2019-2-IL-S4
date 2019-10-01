using System;

namespace ITI.PrimarySchool.WebApp.Services
{
    public class WinOrLoseService
    {
        public bool WinOrLose()
        {
            return Environment.TickCount % 2 == 0;
        }
    }
}