using Mailjet.Client;
using Mailjet.Client.Resources;
using Microsoft.AspNetCore.Identity.UI.Services;
using Newtonsoft.Json.Linq;

namespace ProSeguridad.Servicios
{
    public class MaiJetEmailSender : IEmailSender
    {
        private readonly IConfiguration _configuration;
        public OptionesMaiJet _OptionesMaiJet;

        public MaiJetEmailSender(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            _OptionesMaiJet = _configuration.GetSection("Mailjet").Get<OptionesMaiJet>();
            MailjetClient client = new MailjetClient(_OptionesMaiJet.Apikey, _OptionesMaiJet.SecretKey)
            {
                Version = ApiVersion.V3_1,
            };
            MailjetRequest request = new MailjetRequest {
     Resource = Send.Resource,
    }
    .Property(Send.Messages, new JArray {
             new JObject {
          {
           "From",
             new JObject {
                {"Email", "hogarhdn@proton.me"}, 
                {"Name", "Hogar del Niño"}
             }
          }, 
                 {
                  "To",
                     new JArray {
                   new JObject {
                   {
                    "Email",
                      email
             },  
                       {
                         "Name",
                        "Hogar"
                        }
                   }
           }
          }, {
           "Subject",
           subject
          },{
           "HTMLPart",
           htmlMessage
          }
         }
            });
         await client.PostAsync(request);
  
   }
  }
 }
