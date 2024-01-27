using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseConnection.core
{
    public class Logger
    {

        public static void errorLog(string message, bool show, object element)
        {
            if (show)
            {
                if (element != null)
                {
                    Type Class = element.GetType();
                    DateTime horaAtual = DateTime.Now;
                    string horaFormatada = horaAtual.ToString("HH:mm:ss");
                    string formatMessage = $"{horaFormatada} [ ERROR ] => {message} : {Class.Name}";
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine(formatMessage);
                }
                else
                {
                    errorLog(message, show);
                }
            }
        }
        public static void errorLog(string message, bool show)
        {
            if (show)
            {
                DateTime horaAtual = DateTime.Now;
                string horaFormatada = horaAtual.ToString("HH:mm:ss");
                string formatMessage = $"{horaFormatada} [ ERROR ] => {message}";
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(formatMessage);
            }
        }

        public static void OKLog(string message, bool show, object element)
        {
            if (show)
            {
                if (element != null)
                {
                    Type Class = element.GetType();
                    DateTime horaAtual = DateTime.Now;
                    string horaFormatada = horaAtual.ToString("HH:mm:ss");
                    string formatMessage = $"{horaFormatada} [ OK ] => {message} : {Class.Name}";
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine(formatMessage);
                }
                else
                {
                    OKLog(message, show);
                }
            }
        }

        public static void OKLog(string message, bool show)
        {
            if (show)
            {
                DateTime horaAtual = DateTime.Now;
                string horaFormatada = horaAtual.ToString("HH:mm:ss");
                string formatMessage = $"{horaFormatada} [ OK ] => {message}";
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine(formatMessage);
            }
        }

        public static void InfoLog(string message, bool show, object element)
        {
            if (show)
            {
                if (element != null)
                {
                    Type Class = element.GetType();
                    DateTime horaAtual = DateTime.Now;
                    string horaFormatada = horaAtual.ToString("HH:mm:ss");
                    string formatMessage = $"{horaFormatada} [ INFO ] => {message}";
                    Console.WriteLine(formatMessage);
                }
                else
                {
                    InfoLog(message, show);
                }
            }
        }

        public static void InfoLog(string message, bool show)
        {
            if (show)
            {
                DateTime horaAtual = DateTime.Now;
                string horaFormatada = horaAtual.ToString("HH:mm:ss");
                string formatMessage = $"{horaFormatada} [ INFO ] => {message}";
                Console.WriteLine(formatMessage);
            }
        }
    }
}
