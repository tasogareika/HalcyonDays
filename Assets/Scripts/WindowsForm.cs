using System.Collections;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Drawing;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WindowsForm : MonoBehaviour
{
    public static WindowsForm singleton;
    private Form form1;

    //code refs
    //from https://docs.microsoft.com/en-us/dotnet/api/system.windows.forms.form?view=netframework-4.7.2
    //https://docs.microsoft.com/en-us/dotnet/framework/winforms/how-to-create-event-handlers-at-run-time-for-windows-forms
    //https://docs.microsoft.com/en-us/dotnet/api/system.windows.forms.messagebox?view=netframework-4.7.2

    private void Awake()
    {
        singleton = this;
    }

    public void CreateMyForm()
    {
        // Initializes the variables to pass to the MessageBox.Show method.
        //string message = "\n\n\n ş̶̡̞̤̞͚̲̗̘͐̈́̐̽̊̾ẗ̵͔̪̦̈́̎̂̇̈̉̃͂̚͝o̷̭̞̜͒͒̈́̉̾̂̔͜͜p̶̢͍͉̠̟̱͙̤͖͕̰͖̓ ̷̡̝̳̘̒̔̕b̸̨̟̈́̓̀̊̈̔̇́̐̔͝͝͠ẹ̶͕͙̩̍̔̓̈́͒̋̚͝f̷̡̧̱͔͎̮̮̠͆̒̾̽̀̈́͐̓̽͠͝o̸̯͓̙̾̑̾͗̈́̊̆̄̈́̇̿͝ř̴͔̬̹̹̤̻̇̉̐͆́ę̸̝̼̱̠͈̠̃̉͊̓͜ ̶͓͍̣͈̰͉͍̼́͐͂̊̏̿̽̓̏͝͝į̶͇̙̣̺̻̱̪͎̟̱̬̓͆́̓͗̃̉̅̀̐͋t̵͓̂̔̈́s̵̟̠͉͙̞̼̳̦̹͆͆ ̸̨̢͇̞̠̙̙̦̫̮̙̱̤̅̍̄̆̕t̵̢̰̬͓͓̰̠͉͖̻͍͎̮̎̄ơ̴̡̢̡̠͎̝̪͓̮̩̭̹͑̿̈́͆͋́̿̄͗̿̒͊͘o̶̧̱̩͇̟̯̠̣̰̝̟̭̓̈̀̅̽̉̐̎͝͝͝ ̶̜̦̝͎̑̀͂̒͝l̴͙̻͚̩̮̦̘̼̈́̈́͑̾̀̋̊̽̃̚͘ą̵̱̪͇͎̘̬̹̯͐̓̄̂̾̇t̵̢̡̛̫̫̰̫̯̥̓͗̇͋̉̐̽̌͋̇́͛̔͝ͅḛ̴̤͉̯̱̋͘ \n\n\n\n\n";
        //string caption = "-.. --- -. .----. -  -.-. --- -. - .. -. ..- .";
        string message = "\n\n\n You promised. \n\n\n\n\n";
        string caption = "Remember,";
        MessageBoxButtons buttons = MessageBoxButtons.OK;
        DialogResult result;

        // Displays the MessageBox.
        result = MessageBox.Show(message, caption, buttons);
        if (result == DialogResult.OK || result == DialogResult.Cancel)
        {
            GameFlags.singleton.loadDemoCredits();
        } 
    }
}
