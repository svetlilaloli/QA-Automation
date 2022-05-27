namespace Summator.Tests.Windows
{
    public class SummatorWindow
    {
        private readonly WindowsDriver<WindowsElement> driver;
        public SummatorWindow(WindowsDriver<WindowsElement> driver)
        {
            this.driver = driver;
        }
        public WindowsElement FirstNumberElement => driver.FindElementByAccessibilityId("textBoxFirstNum");
        public WindowsElement SecondNumberElement => driver.FindElementByAccessibilityId("textBoxSecondNum");
        public WindowsElement CalculateButtonElement => driver.FindElementByAccessibilityId("buttonCalc");
        public WindowsElement ResultElemet => driver.FindElementByAccessibilityId("textBoxSum");

        public void Sum(string num1, string num2)
        {
            FirstNumberElement.SendKeys(num1);
            SecondNumberElement.SendKeys(num2);
            CalculateButtonElement.Click();
        }
        public string GetResult()
        {
            return ResultElemet.Text;
        }
        public void ClearForm()
        {
            FirstNumberElement.Clear();
            SecondNumberElement.Clear();
        }
    }
}
