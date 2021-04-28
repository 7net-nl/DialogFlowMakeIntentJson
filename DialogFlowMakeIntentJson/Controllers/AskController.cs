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
    public class AskController : Controller
    {
        private readonly IWebHostEnvironment webHost;

        public AskController(IWebHostEnvironment webHost)
        {
            this.webHost = webHost;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Index(string InputText = "", string InputLang = "")
        {

            var SpliteText = InputText.Replace('"', '#').Replace("#","").Replace(",","").Split("\r\n").ToList();

            var model = new IntentFile();
            var UserSays = new IntentUserSays();
            var GetJson = "";
            var FileNameSpeech = "";
            var FileNameUserSays = "";
            var FilePathSpeech = "";
            var FilePathBase = "";
            var FilePathAskMeQuestion = webHost.WebRootPath + "\\" + "Ask Me question.txt";
            double Count = SpliteText.Count / 200.0;
            var OkCount = Math.Ceiling(Count);
            var CountFile = 1; 

            for (int i = 0; i < OkCount; i++)
            {
               
                Repeat:

                FileNameSpeech = $@"Req {InputLang} Ask Me Question 0{CountFile}.json";
                FilePathSpeech = webHost.WebRootPath + "\\question\\" + FileNameSpeech;
                FilePathBase = webHost.WebRootPath + "\\question\\Base.json";

                if (!System.IO.File.Exists(FilePathSpeech))
                {

                  

                        var RdModel = JsonConvert.DeserializeObject<IntentFile>(System.IO.File.ReadAllText(FilePathBase));
                        
                        model = new IntentFile
                        {

                            name = FileNameSpeech.Replace(".json", ""),
                            auto = true,
                            userSays = RdModel.userSays,
                            responses = new List<IntentRespons> { new IntentRespons { messages = new List<IntentMessage> { new IntentMessage { type = "0".ToString(), condition = "", lang = InputLang.ToString(), speech = SpliteText.OrderBy(c=>c).Skip(i * 200).Take(200).ToList() } } } }
                        };


                        GetJson = JsonConvert.SerializeObject(model);
                        System.IO.File.WriteAllText(FilePathSpeech, GetJson);

                    

                }

                else
                {
                    CountFile++;
                    goto Repeat;
                }






            }

            return View();
        }


    }
}
