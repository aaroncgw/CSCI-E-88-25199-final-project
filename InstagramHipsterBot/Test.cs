using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

using Newtonsoft.Json;
using NUnit.Framework;

using Xamarin.UITest;
using Xamarin.UITest.iOS;

namespace InstagramHipsterBot
{
    enum CurrentBot {
        FisherHipster,
        CoffeeHipster
    }

    [TestFixture]
    public class Test
    {
        const string CustomVisionUrl = "https://southcentralus.api.cognitive.microsoft.com/customvision/v1.1/Prediction/e74dd823-4617-4ff2-94dd-6360ce255414/image?iterationId=f8ef8c82-6560-4df9-a6b2-b5c02b951d50";

        string fisherHipsterDeviceIdentifier = "57ce11c580f8ffdf9a10967fbcc682e63c29d0b5";
        string coffeeHipsterDeviceIdentifier = "ea7384d39eb8d0c8bd587e93d8d99f9ef8fcdfef";

        // Subject for a change
        string fisherHipsterDeviceIP = "10.0.0.215";
        string coffeeHipsterDeviceIP = "192.168.2.2";

        iOSApp app;
        CurrentBot bot = CurrentBot.FisherHipster;

        [SetUp]
        public void Setup ()
        {
            string deviceUDID = string.Empty;
            string deviceIP = string.Empty;

            if (bot == CurrentBot.FisherHipster) {
                deviceUDID = fisherHipsterDeviceIdentifier;
                deviceIP = fisherHipsterDeviceIP;
            } else {
                deviceUDID = coffeeHipsterDeviceIdentifier;
                deviceIP = coffeeHipsterDeviceIP;
            }

            app = ConfigureApp.iOS.
                DeviceIdentifier(deviceUDID).
                DeviceIp(deviceIP).
                EnableLocalScreenshots ().
                ConnectToApp ();
                //InstalledApp ("com.olegoid.InstagramWrapper.InstagramWrapper").
                //StartApp ();
        }

        [TearDown]
        public void TearDown ()
        {
            app = null;
        }

        [Test]
        public void LikeLoop ()
        {
            while (true) {
                var all = app.Query(q => q.Class("WKWebView").Css("*"));

                var hashtags = new List<string> ();

                foreach (var element in all) {
                    if (!string.IsNullOrEmpty(element.TextContent) && element.Class == "_ezgzd") {
                        var regex = new Regex(@"(?<=#)\w+");
                        var matches = regex.Matches(element.TextContent);

                        foreach (Match m in matches)
                            hashtags.Add(m.Value);
                    }
                }

                foreach (var element in all) {
                    if (element.Class.Contains("coreSpriteHeartOpen")) {
                        app.TapCoordinates(element.Rect.CenterX, element.Rect.CenterY);

                        var imageFileInfo = app.Screenshot(DateTime.Now.ToString());

                        try {
                            MakePredictionRequest(imageFileInfo.FullName).Wait();
                        } catch(Exception ex) {
                            Console.WriteLine(ex.Message);
                        } finally {
                            File.Delete(imageFileInfo.FullName);
                        }

                        break;
                    }
                }

                app.ScrollDown();
            }
        }

        async Task<CustomVisionResponse> MakePredictionRequest(string imageFilePath)
        {
            var client = new HttpClient();
            client.DefaultRequestHeaders.Add("Prediction-Key", "a209a4fe330e453d81d8dc3ba24c5163");

            byte[] byteData = GetImageAsByteArray(imageFilePath);

            using (var content = new ByteArrayContent(byteData)) {
                content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
                var response = await client.PostAsync(CustomVisionUrl, content);
                var responseString = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<CustomVisionResponse>(responseString);
            }
        }

        byte[] GetImageAsByteArray(string imageFilePath)
        {
            FileStream fileStream = new FileStream(imageFilePath, FileMode.Open, FileAccess.Read);
            BinaryReader binaryReader = new BinaryReader(fileStream);
            return binaryReader.ReadBytes((int)fileStream.Length);
        }
    }
}
