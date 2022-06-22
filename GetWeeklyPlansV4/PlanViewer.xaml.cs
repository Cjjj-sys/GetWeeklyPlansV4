using System;
using System.Text;
using System.Windows;

namespace GetWeeklyPlansV4;

public partial class PlanViewer : Window
{
    public PlanViewer(string html)
    {
        InitializeComponent();
        ViewFrame.NavigateToString(Unicode2HTML(html));
    }

    public static string Unicode2HTML(string HTML)
    {
        StringBuilder str = new StringBuilder();
        char c;
        for (int i = 0; i < HTML.Length; i++)
        {
            c = HTML[i];
            if (Convert.ToInt32(c) > 127)
            {
                str.Append("&#" + Convert.ToInt32(c) + ";");
            }
            else
            {
                str.Append(c);
            }
        }

        return str.ToString();
    }
}