using Microsoft.SharePoint.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security;
using System.Xml;

namespace RemoveSealed
{
    class Program
    {
        static void Main(string[] args)
        {

            ConsoleColor defaultForeground = Console.ForegroundColor;

            Console.ForegroundColor = ConsoleColor.Green;

            Console.ForegroundColor = defaultForeground;
            string webUrl = "https://ninetech.sharepoint.com/WeAreNinetech/";

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Enter your username:");
            Console.ForegroundColor = defaultForeground;
            string userName = Console.ReadLine();

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Enter your password.");
            Console.ForegroundColor = defaultForeground;
            SecureString password = GetPasswordFromConsoleInput();

            using (var ctx = new ClientContext(webUrl))
            {
                ctx.Credentials = new SharePointOnlineCredentials(userName, password);
                ctx.Load(ctx.Web, w => w.Title);
                ctx.ExecuteQuery();

                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine("Your site title is: " + ctx.Web.Title);
                Console.ForegroundColor = defaultForeground;

                List li = ctx.Web.Lists.GetByTitle("Semesterlista");
                FieldCollection fldColl = li.Fields;
                Field fld = fldColl.GetByTitle("Location");
                ctx.Load(fldColl);
                
                fld.SchemaXml =
                  "<Field Type = 'Text' " +
                  "DisplayName= 'Location' " +
                  "ID= '{B894C8714-3D41-4E80-ADBF-87648BBD7A7F}' " +
                  "ShowInViewForms = 'FALSE' " +
                  "ShowInNewForms = 'FALSE' " +
                  "Hidden= 'TRUE' /> ";

                fld.Update();
                li.Update();
                ctx.ExecuteQuery();

                //fld.ReadOnlyField = false;
                //fld.SetShowInEditForm(false);
                //fld.SetShowInNewForm(false);
                //fld.Update();
                //ctx.ExecuteQuery();

                Console.WriteLine("Press any key to exit");
                Console.ReadKey();
            }
        }

        private static SecureString GetPasswordFromConsoleInput()
        {
            ConsoleKeyInfo info;

            SecureString securePassword = new SecureString();
            do
            {
                info = Console.ReadKey(true);
                if (info.Key != ConsoleKey.Enter)
                {
                    securePassword.AppendChar(info.KeyChar);
                }
            }
            while (info.Key != ConsoleKey.Enter);
            return securePassword;
        }
       
    }


  


       
}



