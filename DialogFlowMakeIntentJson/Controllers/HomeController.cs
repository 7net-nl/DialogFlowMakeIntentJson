using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using DialogFlowMakeIntentJson.Models;
using Microsoft.AspNetCore.Hosting;
using Newtonsoft.Json;
using System.Text.RegularExpressions;

namespace DialogFlowMakeIntentJson.Controllers
{
    public class HomeController : Controller
    {
        private readonly IWebHostEnvironment webHost;

        public HomeController(IWebHostEnvironment webHost)
        {
            this.webHost = webHost;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Index(string InputText="",string InputLang="")
        {
            
            var SpliteText = InputText.Split("\r\n").ToList();

            var model = new IntentFile();
            var UserSays = new IntentUserSays();
            var GetJson = "";
            var FileNameSpeech = "";
            var FileNameUserSays = "";
            var FilePathSpeech = "";
            var FilePathUserSays = "";
            var FilePathAskMeQuestion = webHost.WebRootPath + "\\" + "Ask Me question.txt";
           
            

            for (int i = 0; i < SpliteText.Count; i++)
            {
                FileNameSpeech = SpliteText[i].Split(";").First().Replace(@"?", "").Replace("*", "").Replace(@"\","").Replace("/","").Replace(":","");

                FileNameSpeech = FileNameSpeech.Length > 50 ? FileNameSpeech.Remove(50) + "_" : FileNameSpeech + "_";


                FileNameUserSays = FileNameSpeech + "_" + "usersays_"+InputLang+".json";
                FileNameSpeech = FileNameSpeech + ".json";

                FilePathSpeech = webHost.WebRootPath + "\\Story\\" +$"Req {InputLang} "+FileNameSpeech;
                FilePathUserSays = webHost.WebRootPath + "\\Story\\" + $"Req {InputLang} " + FileNameUserSays;

                

                    if (!System.IO.File.Exists(FilePathSpeech))
                   {

                    

                    model = new IntentFile
                    {
                         
                        name = $"Req {InputLang} " + FileNameSpeech.Replace(".json",""),
                        auto = true,
                        userSays = new List<IntentUserSays> { new IntentUserSays { data = new List<Intentdata> { new Intentdata { text = SpliteText[i].Split(";").First() } } } },
                        responses = new List<IntentRespons> { new IntentRespons { messages = new List<IntentMessage> { new IntentMessage { type = "0".ToString(), condition = "", lang =InputLang.ToString(), speech = new List<string> { SpliteText[i].Split(";").Last() } } } } }
                    };
                    GetJson = JsonConvert.SerializeObject(model);
                    System.IO.File.WriteAllText(FilePathSpeech, GetJson);
                    System.IO.File.AppendAllLines(FilePathAskMeQuestion,new List<string> {'"'+SpliteText[i].Split(";").First()+'"'+"," });

                }
                else
                {

                    

                    model = JsonConvert.DeserializeObject<IntentFile>(System.IO.File.ReadAllText(FilePathSpeech));

                    model.responses[0].messages[0].speech.Add(SpliteText[i].Split(";").Last());
                    model.responses[0].messages[0].lang = InputLang;
                    model.userSays[0].data.Add(new Intentdata { text = SpliteText[i].Split(";").First() });
                    GetJson = JsonConvert.SerializeObject(model);
                    System.IO.File.WriteAllText(FilePathSpeech, GetJson);
                    System.IO.File.AppendAllLines(FilePathAskMeQuestion, new List<string> { '"' + SpliteText[i].Split(";").First() + '"' + "," });
                }


                if (!System.IO.File.Exists(FilePathUserSays))
                {
                    UserSays = new IntentUserSays
                    {
                        data = new List<Intentdata>
                        {
                            new Intentdata{ text =SpliteText[i].Split(";").First()}
                        }
                    };

                    GetJson = JsonConvert.SerializeObject(UserSays);
                    System.IO.File.WriteAllText(FilePathUserSays, GetJson);

                }
                else
                {
                   

                    UserSays = JsonConvert.DeserializeObject<IntentUserSays>(System.IO.File.ReadAllText(FilePathUserSays));

                    UserSays.data.Add(new Intentdata { text = SpliteText[i].Split(";").Last() });
                    GetJson = JsonConvert.SerializeObject(model);
                    System.IO.File.WriteAllText(FilePathSpeech, GetJson);
                }


            }

            return View();
        }
    }
}
