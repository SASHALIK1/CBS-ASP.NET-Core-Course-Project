using CBS_ASP.NET_Core_Course_Project.Models;
using Microsoft.AspNetCore.Mvc;

namespace CBS_ASP.NET_Core_Course_Project
{
    public class EmailSendSettingViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            var model = new EmailSendSettingModel
            {
                sendEmails = GetSendEmailPreference()
            };
            return View(model);
        }

        private bool GetSendEmailPreference()
        {
            return true;
        }
    }
}
