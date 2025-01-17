using OOPTask4.Console;
using OOPTask4.Core.Control;
using Terminal.Gui;

var config = new CarBusinessConfig(2, 2, 2, 5, 5, 5, 5);

using (MainWindowController.CarBusiness = new(config, 16))
{
    Application.Run<MainWindow>();

    MainWindowController.CarBusiness.StopBusiness();

    Application.Shutdown();
}