using CMS.ApplicationCore;
using CMS.ApplicationCore.DTO;

namespace CMS.Infrastructure
{
    public class Testing
    {
        public void SendTestingEmail()
        {
            EmailConfig email = new EmailConfig();
            email.To = new string[] { "thanhnguyenbk@gmail.com"};
            email.Subject = "Active account";
            email.Body = "test";

            IEmailSender sender = new GoogleEmailSender(email, "1035158221116-qv9p42ldlbcljjsc95a1058mp4tuv2vt.apps.googleusercontent.com", "D95ItqksMp9-vauoLQqvhAag");
            sender.Send();
		}
    }
}
