using Mailjet.Client;
using Mailjet.Client.Resources;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ASP.netCore5NTier.Utility
{
    public class EmailSender : IEmailSender
    {
        private readonly IConfiguration _configuration;

        public MailJetSettings _mailJetSettings { get; set; }

        public Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            return Execute(email, subject, htmlMessage);
        }

        public async Task Execute(string email, string subject, string htmlMessage)
        {
            

            MailjetClient client = new MailjetClient("084dbbc337f733fbe72f78136bbb22c1", "4c52e1c774f516ed228c83fa63a45c81");
            MailjetRequest request = new MailjetRequest
                {
                    Resource = Send.Resource,
                }
                .Property(Send.Messages, new JArray {
                    new JObject {
                        {
                            "From",
                            new JObject {
                                {"Email", "geopik@gmail.com"},
                                {"Name", "George"}
                            }
                        }, {
                            "To",
                            new JArray {
                                new JObject {
                                    {
                                        "Email",
                                        email
                                    }, {
                                        "Name",
                                        "DotNetMastery"
                                    }
                                }
                            }
                        }, {
                            "Subject",
                            subject
                        }, {
                            "HTMLPart",
                            htmlMessage
                        }
                    }
                });
            await client.PostAsync(request);
        }
    }
}
    

