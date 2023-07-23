using System;
using Newtonsoft.Json.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web.Http;
using System.Threading.Tasks;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web;

namespace WebApplication2.Controllers
{
    

    public class User
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Avatar { get; set; }
        public string Subtitle // Add the Subtitle property
        {
            get { return LastName; }
        }
    }
    public class ValuesController : ApiController
    {
        
        public async Task<string> GetDataFromExternalApiAsync(string endpoint)
        {
            HttpClient _httpClient = new HttpClient();

            HttpResponseMessage response = await _httpClient.GetAsync(endpoint);

            Console.WriteLine(response.StatusCode);

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadAsStringAsync();
            }
            else
            {
                return null;
            }
        }


        // GET api/values
        [HttpGet]
        //HttpResponseMessage
        public async Task<HttpResponseMessage> Get()
        {
            

            string externalData = await GetDataFromExternalApiAsync("https://reqres.in/api/users");



            String currurl = HttpContext.Current.Request.RawUrl;
            

            
            int iqs = currurl.IndexOf('?');
            if (iqs != -1)
            {
                int pageIndex = currurl.IndexOf("page=");
                int per_pageIndex = currurl.IndexOf("per_page=");
                if (pageIndex != -1 && per_pageIndex != -1)
                {
                        
                        string[] x = currurl.Split('?'); // page = 5 & per_page= 11
                        string[] values = x[1].Split('&');
                        
                        if(values[0].StartsWith("page"))
                        externalData = await GetDataFromExternalApiAsync("https://reqres.in/api/users" + "?" + values[0] + "&" + values[1]);
                           
                                   
                       
                        else if(values[0].StartsWith("per_page"))
                        externalData = await GetDataFromExternalApiAsync("https://reqres.in/api/users" + "?" + values[1] + "&" + values[0]);

                }
                else if(pageIndex != -1)
                {   
                    
                    string[] x = currurl.Split('?');
                    externalData = await GetDataFromExternalApiAsync("https://reqres.in/api/users" +  x[1]) ;
                    //return "https://reqres.in/api/users?" + x[1];
                } 
                else if (per_pageIndex != -1)
                {
                    Console.WriteLine(currurl);
                    string[] x = currurl.Split('?');
                    externalData = await GetDataFromExternalApiAsync("https://reqres.in/api/users" +  x[1]);
                }
            }
            
            
            JObject jsonResponse = JObject.Parse(externalData);
            JArray dataArray = (JArray)jsonResponse["data"];

            // Convert "last_name" to "subtitle" for each user in the array
            foreach (JObject item in dataArray)
            {
                item["title"] = item["first_name"];
                item["image_url"] = item["avatar"];
                item["subtitle"] = item["last_name"];
                JObject defaultAction = new JObject();
                defaultAction.Add("type","web_url");
                defaultAction.Add("url", $"https://mail.google.com/mail/u/0/?fs=1&tf=cm&to=" + item["email"] + $"&su=Hello&body=Send%20Email");
                defaultAction.Add("webview_height_ratio", "tall");
                item["default_action"] = defaultAction;

                defaultAction.Remove("webview_height_ratio");
                defaultAction.Add("title", "Send Email");

                JArray buttons = new JArray();
                buttons.Add(defaultAction);
                item["buttons"] = buttons; 

                item.Remove("first_name");
                item.Remove("last_name");
                item.Remove("id");
                item.Remove("avatar");
                item.Remove("email");
            }


            var response = Request.CreateResponse(HttpStatusCode.OK);
            response.Content = new StringContent(dataArray.ToString(), Encoding.UTF8, "application/json");
            
            return response;
        }

        // GET api/values/5
        public HttpResponseMessage Get(int id)
        {
            var response = Request.CreateResponse(HttpStatusCode.OK);
            response.Content = new StringContent("{'test' : 'test'}".ToString(), Encoding.UTF8, "application/json");
            return response;
        }

        
    }
}
