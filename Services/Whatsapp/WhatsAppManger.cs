using DotNetEnv;
using Newtonsoft.Json;
using RestSharp;
using Method = RestSharp.Method;


namespace FargoApi.Services.Whatsapp
{
    public class WhatsappModel(string data, bool isvalid)
    {
        public string Data { get; set; } = data;
        public bool Isvalid { get; set; } = isvalid;
    }
    
    public class Contact(bool status)
    {
        public bool Status { get; set; } = status;
    }
    
    public class WhatsAppManger
    {
        public async Task<WhatsappModel> SendMessage(string phone, string msg)
        {
            try
            {
                var isright = await IsWhatsappNumber(phone);
                if (isright)
                {
                    var url = Env.GetString("WhatsappUrl") + "messages/chat";
                    var client = new RestClient(url);
                    var request = new RestRequest(url, Method.Post);
                    request.AddHeader("content-type", "application/x-www-form-urlencoded");
                    request.AddParameter("token", Env.GetString("WhatsappToken"));
                    request.AddParameter("to", phone);
                    request.AddParameter("body", "(Verification Code )\n" + msg);

                    RestResponse response = await client.ExecuteAsync(request);
                    return new WhatsappModel("The verification code was sent to WhatsApp", true);
                }
                else
                {
                    return new WhatsappModel("Sorry, this number is not valid", false);
                }
            }
            catch (Exception)
            {
                return new WhatsappModel("There was an error, please contact our support", false);
            }
        }

        public async Task<bool> IsWhatsappNumber(string number)
        {
            try
            {
                var url = Env.GetString("WhatsappUrl") + "contacts/check";
                var client = new RestClient(url);
                var request = new RestRequest(url,Method.Get);

                // Format the number correctly
                string chatId = number.EndsWith("@c.us") ? number : $"{number}@c.us";

                request.AddHeader("Content-Type", "application/x-www-form-urlencoded");
                request.AddParameter("token", Env.GetString("WhatsappToken"));
                request.AddParameter("chatId", chatId);
                request.AddParameter("nocache", "");

                var response = await client.ExecuteAsync(request);

                if (!response.IsSuccessful) return false;

                var contact = JsonConvert.DeserializeObject<Contact>(response.Content);
                return contact != null && contact.Status;
            }
            catch
            {
                return false;
            }
        }

   
    }
}
