using OOPTask4.Core;
using OOPTask4.Core.Products;
using OOPTask4.Core.Supplier;
using Terminal.Gui;

namespace OOPTask4.Console;

public sealed class MainWindow : Window
{
    public static string UserName;

    private TextField userNameText;
    private TextField passwordText;

    public MainWindow()
    {
        Title = $"OOPTask4 Console ({Application.QuitKey} to quit)";

        var usernameLabel = new Label
        {
            Text = "Username:"
        };

        userNameText = new()
        {
            X = Pos.Right(usernameLabel) + 1,
            Width = Dim.Fill()
        };

        var passwordLabel = new Label
        {
            Text = "Password:", X = Pos.Left(usernameLabel), Y = Pos.Bottom(usernameLabel) + 1
        };
        
        passwordText = new()
        {
            Secret = true,
            X = Pos.Left(userNameText),
            Y = Pos.Top(passwordLabel),
            Width = Dim.Fill()
        };

        var btnLogin = new Button
        {
            Text = "Login",
            Y = Pos.Bottom(passwordLabel) + 1,
            X = Pos.Center(),
            IsDefault = true
        };
        
        btnLogin.Clicked += BtnLoginOnClicked;

        TickableGroup<Supplier<Carcase>> pool = new();
        pool.Add(new());
        pool.Add(new());
        pool.Add(new());

        var s = new ListView(pool.GetTickables().ToList())
        {
            Text = "Login",
            //Y = Pos.Bottom(btnLogin) + 1,
            //X = Pos.Center(),
            Width = Dim.Fill(),
            Height = Dim.Fill()
        };

        //Add(s);

        Add(usernameLabel, userNameText, passwordLabel, passwordText, btnLogin);
    }

    private void BtnLoginOnClicked()
    {
        if (userNameText.Text == "admin" && passwordText.Text == "password")
        {
            MessageBox.Query ("Logging In", "Login Successful", "Ok");
            UserName = (string)userNameText.Text;
            Application.RequestStop();
        }
        else
        {
            MessageBox.ErrorQuery("Logging In", "Incorrect username or password", "Ok");
        }
    }
}