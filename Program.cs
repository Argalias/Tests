using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;

[assembly: CLSCompliant(true)]
namespace EDMSConsole
{
    public class CollectionsDemo
    {
        enum ProcessingDocumentOption
        {
            DirectTo = 1,
            ReturnBack,
            ShowFullPath,
        }
        enum DocumentOptions
        {
            CheckArrived = 1,
            ShowWatchers,
            ProcessDocument,
        }

        class Document
        {
            private LinkedList<string> Path { get; set; }
            public string Title { get; set; }
            public DateTime CreationDate { get; set; }
            public Document()
            {
                Title = String.Empty;
                CreationDate = new DateTime();
                Path = new LinkedList<string>();
            }
            public void DirectTo(string email)
            {
                Path.AddLast(email);
            }

            public void ReturnBack()
            {

                Path.RemoveLast();
            }
            public void ShowPath()
            {
                foreach (var email in Path)
                {
                    Console.Write($"{email} -> ");
                }
            }
            public override string ToString()
            {
                return $"Повідомлення: {Title}\t\tвід {CreationDate.ToString("dd.MM.yyyy")}";
            }
        }
        private Dictionary<Document, string> DocumentsWatchers { get; set; }
        private Queue<Document> ArrivedDocuments { get; set; }
        public CollectionsDemo()
        {
            DocumentsWatchers = new Dictionary<Document, string>();
            ArrivedDocuments = new Queue<Document>();
        }
        private string SetDocumentWatcher()
        {
            Console.WriteLine($"Привіт, як справи?");
            return Console.ReadLine();
        }

        private void OutputAllWatchers()
        {
            int index = 1;
            foreach (var watcher in DocumentsWatchers)
            {
                Console.WriteLine($"{index}.)\tE-mail:\t{watcher.Value}");
                Console.WriteLine($"{watcher.Key}");
                index++;
            }
        }
        private Document SelectDocument()
        {
            Console.WriteLine($"Відкрийте повідомлення:");
            OutputAllWatchers();
            int selectedDocIndex;
            while (!int.TryParse(Console.ReadLine(), out selectedDocIndex))
            {
                Console.WriteLine($"Введено неправильні дані, перевірте та спробуйте знову");
            }

            if (selectedDocIndex > DocumentsWatchers.Keys.Count)
                return null;
            return DocumentsWatchers.Keys.ElementAt(selectedDocIndex - 1);
        }
        private void ProcessDocument()
        {
            var document = SelectDocument();
            Console.WriteLine($"Переглянути повідомлення:");
            Console.WriteLine($"1.) Надіслати повідомлення;");
            Console.WriteLine($"2.) Повернути відправнику;");
            Console.WriteLine($"3.) Переглянути повний шлях повідомлення;");
            Byte docOption;
            while (!Byte.TryParse(Console.ReadLine(), out docOption))
            {
                Console.WriteLine($"Введено неправильні дані, перевірте та спробуйте знову");
            }
            ProcessingDocumentOption selectedOption = (ProcessingDocumentOption)docOption;
            Console.WriteLine(Environment.NewLine);
            switch (selectedOption)
            {
                case ProcessingDocumentOption.DirectTo:
                    Console.WriteLine($"Введіть повідомлення");
                    document?.DirectTo(Console.ReadLine());
                    break;
                case ProcessingDocumentOption.ReturnBack:
                    document?.ReturnBack();
                    Console.WriteLine($"Повідомлення надіслано");
                    Console.WriteLine(Environment.NewLine);
                    document?.ShowPath();
                    break;
                case ProcessingDocumentOption.ShowFullPath:
                    Console.WriteLine($"Повний шлях повідомлення");
                    document?.ShowPath();
                    break;
                default:
                    break;
            }
        }
        private void CheckArrivedDocuments()
        {
            var rand = new Random();
            var titles = new StringCollection() { $"Zhenya Sanevych №",};
            int randomTitleIndex = rand.Next(titles.Count);
            var newDocument = new Document()
            {
                Title = $"{titles[randomTitleIndex] + rand.Next(51)}",
                CreationDate = DateTime.Now,
            };
            Console.WriteLine($"Надійшло нове повідомлення:");
            Console.WriteLine($"{newDocument}");
            var watcher = SetDocumentWatcher();
            newDocument.DirectTo(watcher);
            ArrivedDocuments.Enqueue(newDocument);
            DocumentsWatchers.TryAdd(newDocument, watcher);
        }
        public void Execute()
        {
            Console.WriteLine($"Оберіть дію з повідомленням:");
            Console.WriteLine($"1.) Перевірити надходження;");
            Console.WriteLine($"2.) Переглянути повідомлення;");
            Console.WriteLine($"3.) Відповісти;");
            Byte docOption;
            while (!Byte.TryParse(Console.ReadLine(), out docOption))
            {
                Console.WriteLine($"Введено неправильні дані, перевірте та спробуйте знову");
            }
            DocumentOptions selectedOption = (DocumentOptions)docOption;
            Console.WriteLine(Environment.NewLine);
            switch (selectedOption)
            {
                case DocumentOptions.CheckArrived:
                    CheckArrivedDocuments();
                    break;
                case DocumentOptions.ShowWatchers:
                    OutputAllWatchers();
                    break;
                case DocumentOptions.ProcessDocument:
                    ProcessDocument();
                    break;
                default:
                    break;
            }
        }
    }
    class Program
    {
        enum MenuOptions : byte
        {
            Execute = 1,
            Exit,
        }
        static CollectionsDemo dl = new CollectionsDemo();
        static void Menu()
        {
            Console.WriteLine($"Оберіть пункт меню:");
            Console.WriteLine($"1.) Виконати програму;");
            Console.WriteLine($"2.) Завершити роботу;");
            Byte menuOption;
            while (!Byte.TryParse(Console.ReadLine(), out menuOption))
            {
                Console.WriteLine($"Введено неправильні дані, перевірте та спробуйте знову");
            }
            MenuOptions selectedOption = (MenuOptions)menuOption;
            switch (selectedOption)
            {
                case MenuOptions.Execute:
                    dl.Execute();
                    Console.ReadLine();
                    break;
                case MenuOptions.Exit:
                    Environment.Exit(0);
                    break;
                default:
                    break;
            }
        }
        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            while (true)
            {
                Menu();
            }
        }
    }
}