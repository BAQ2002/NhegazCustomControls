
namespace NhegazCustomControls
{
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            // To customize application configuration such as set high DPI settings or default font,
            // see https://aka.ms/applicationconfiguration.
            ApplicationConfiguration.Initialize();
            Application.Run(new TestForm());
            Application.Run(new SearchTableForm());
            string TestString = "Ass";
            int TestIndex = 2;
            Font testFont = new("Arial Unicode MS", 11.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            MessageBox.Show(
                "Texto = " + TestString //+
                //" largura do " + TestString[TestIndex] + " = "  + NhegazSizeMethods.TextCharRect(TestString, TestIndex, testFont,0,0).Width +
                //" Localização X " + TestString[TestIndex] + " = " + NhegazSizeMethods.TextCharRect(TestString, TestIndex, testFont,0,0).Location.X
                );
        }
    }
}