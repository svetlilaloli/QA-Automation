using OpenQA.Selenium;
using OpenQA.Selenium.Appium;
using OpenQA.Selenium.Appium.Windows;

namespace SevenZip.Tests
{
    public class SevenZipTests
    {
        private const string AppiumServerUrl = @"http://127.0.0.1:4723/wd/hub";
        private WindowsDriver<WindowsElement> driver;
        private WindowsDriver<WindowsElement> rootDriver;
        private string workDir;
        private const string App = @"C:\Program Files\7-Zip\7zFM.exe";
        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            var desktopOptions = new AppiumOptions()
            {
                PlatformName = "Windows"
            };
            desktopOptions.AddAdditionalCapability("app", "Root");
            rootDriver = new WindowsDriver<WindowsElement>(new Uri(AppiumServerUrl), desktopOptions);

            var options = new AppiumOptions()
            {
                PlatformName = "Windows"
            };
            options.AddAdditionalCapability("app", App);
            driver = new WindowsDriver<WindowsElement>(new Uri(AppiumServerUrl), options);

            workDir = Directory.GetCurrentDirectory() + @"\workdir";
            if (Directory.Exists(workDir))
            {
                Directory.Delete(workDir, true);
            }
            Directory.CreateDirectory(workDir);
        }

        [Test]
        public void Test_MaxCompression()
        {
            var locationTextBox = driver.FindElementByClassName("Edit");
            locationTextBox.SendKeys(@"C:\Program Files\7-Zip\" + Keys.Enter);
            var filesList = driver.FindElementByClassName("SysListView32");
            filesList.SendKeys(Keys.Control + 'a');
            driver.FindElementByName("Add").Click();

            string archiveFileName = workDir + "\\" + DateTime.Now.Ticks + ".7z";

            Thread.Sleep(500);
            var archiveView = rootDriver.FindElementByName("Add to Archive");
            var archiveNameTextBox = archiveView.FindElementByXPath("/Window/ComboBox/Edit[@Name='Archive:']");
            archiveNameTextBox.SendKeys(archiveFileName);

            var archiveFormat = archiveView.FindElementByXPath("/Window/ComboBox[@Name='Archive format:']");
            archiveFormat.SendKeys(Keys.Home);

            var compressionLevel = archiveView.FindElementByXPath("/Window/ComboBox[@Name='Compression level:']");
            compressionLevel.SendKeys(Keys.End);

            var compressionMethod = archiveView.FindElementByXPath("/Window/ComboBox[@Name='Compression method:']");
            compressionMethod.SendKeys(Keys.Home);

            var dictionarySize = archiveView.FindElementByXPath("/Window/ComboBox[@Name='Dictionary size:']");
            dictionarySize.SendKeys(Keys.End);

            var wordSize = archiveView.FindElementByXPath("/Window/ComboBox[@Name='Word size:']");
            wordSize.SendKeys(Keys.End);

            var okButton = archiveView.FindElementByXPath("/Window/Button[@Name='OK']");
            okButton.Click();
            Thread.Sleep(1000);

            // extract
            locationTextBox.SendKeys(archiveFileName + Keys.Enter);
            var extractButton = driver.FindElementByName("Extract");
            extractButton.Click();

            var okButtonExtract = driver.FindElementByName("OK");
            okButtonExtract.Click();
            Thread.Sleep(1000);

            string extracted = workDir + @"\7zFM.exe";
            FileAssert.AreEqual(App, extracted);
        }
        [OneTimeTearDown]
        public void OneTimeTearDown()
        {
            driver.Quit();
            rootDriver.Quit();
        }
    }
}