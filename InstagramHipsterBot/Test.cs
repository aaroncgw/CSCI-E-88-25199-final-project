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
        string fisherHipsterDeviceIdentifier = "57ce11c580f8ffdf9a10967fbcc682e63c29d0b5";
        string coffeeHipsterDeviceIdentifier = "ea7384d39eb8d0c8bd587e93d8d99f9ef8fcdfef";

        string fisherHipsterDeviceIP = "192.168.2.11";
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
                DeviceIp(deviceIP).ConnectToApp ();
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

                foreach (var element in all)
                    if (element.Class.Contains("coreSpriteHeartOpen"))
                        app.TapCoordinates(element.Rect.CenterX, element.Rect.CenterY);

                app.ScrollDown();
            }
        }
    }
}
