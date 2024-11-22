using OOPTask4.Console;
using Terminal.Gui;

Application.Run<MainWindow>();

Application.Shutdown();

Console.WriteLine($"Username: {MainWindow.UserName}");

Console.ReadLine();