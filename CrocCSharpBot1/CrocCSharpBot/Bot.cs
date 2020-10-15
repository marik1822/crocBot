using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Telegram.Bot;
using Telegram.Bot.Requests;
using Telegram.Bot.Types.ReplyMarkups;
namespace CrocCSharpBot
{
    /// <summary>
    /// Основной модуль бота
    /// </summary>
    public class Bot
    {
        /// <summary>
        /// Клиент Telegram
        /// </summary>
        private TelegramBotClient client;

        /// <summary>
        /// Конструктор без параметров
        /// </summary>
        public Bot()
        {
            // Создание клиента для Telegram
            client = new TelegramBotClient("1282004742:AAHYvVD7B_NixeIwGWPRoIZ1n9yJ5l5cNFY");
            client.OnMessage += MessageProcessor;
        }

        /// <summary>
        /// Обработка входящего сообщения
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MessageProcessor(object sender, Telegram.Bot.Args.MessageEventArgs e)
        {
            switch (e.Message.Type)
            {
                case Telegram.Bot.Types.Enums.MessageType.Contact: // телефон
                    if (e.Message.Chat.Id == e.Message.Contact.UserId)
                    {
                        string phone = e.Message.Contact.PhoneNumber;
                        client.SendTextMessageAsync(e.Message.Chat.Id, $"Твой телефон: {phone}");
                        Console.WriteLine(phone);
                    }
                    else
                    {
                        client.SendTextMessageAsync(e.Message.Chat.Id, $"Это не ваш номер телефона");
                    }

                    break;

                case Telegram.Bot.Types.Enums.MessageType.Text: // текстовое сообщение
                    if (e.Message.Text.Substring(0, 1) == "/")
                    {
                        CommandProcessor(e.Message);
                    }
                    else
                    {
                        client.SendTextMessageAsync(e.Message.Chat.Id, $"Ты сказал мне: {e.Message.Text}");
                        Console.WriteLine(e.Message.Text);
                    }
                    break;
                case Telegram.Bot.Types.Enums.MessageType.Photo:// фотография 
                    client.SendTextMessageAsync(e.Message.Chat.Id, $"Вы отправили мне кртинку");
                    Console.WriteLine("Отправлена фотография");
                    
                    /*client.GetFileAsync();
                    TelegramBotClient.GetFileAsync(e.Message.Photo,) ;
                    public static async void downloadFile(Message message)
                    {
                        try
                        {
                            var file = await TelegramBotClient.GetFileAsync(message.Photo.LastOrDefault().FileId);
                            var filename = file.FileId + "." + file.FilePath.Split('.').Last();

                            using (var saveImageStream = new FileStream(path + "/" + filename, FileMode.Create))
                            {
                                await file.FileStream.CopyToAsync(saveImageStream);
                            }
                        }
                        catch (Exception ex)
                        {
                            Console.Writeline(ex.Message);
                            }
                    }*/
                    //e.Message.Photo;
                    //SaveFileDialog savedialog = new SaveFileDialog(); 
                    //save(e.Message.Photo.Clone);
                    //File.Create(e.Message.Photo);
                    //client.UploadStickerFileAsync();
                    //Создадим обьект класса XmlSerializer, для выполнения сериализации обьекта(в скобачках тип сеарилизованых данных)
                    /* DirectoryInfo di = new DirectoryInfo("DataTime");
                     try
                     {
                         // Проверяем существует ли, указанная папка
                         if (di.Exists)
                         {
                             // Если - да, сообщаем об этом
                             Console.WriteLine("\nПапка DataTime уже существует");
                             goto Label1;
                         }
                         // Если - нет, создаём её
                         else
                         {
                             di.Create();
                             Console.WriteLine("\nСоздание папки прошло успешно");
                             goto Label1;
                         }
                     }
                     finally { }

                 Label1:
                      Объявляем строковую переменную "path", 
                      * которая описывает путь к файлу 
                     string path = @"DataTime\\OutputTime.txt";
                     if (File.Exists(path))
                     {
                         // Если - да, то сообщаем об этом
                         Console.WriteLine("\nФайл OutputTime.txt существует\n");
                         //Console.ReadKey(e.Message.Photo);
                         goto Label2;
                     }
                     else
                     {
                         // Если - нет, тоже сообщаем 
                         Console.WriteLine("\nФайл Output.txt отсутствует!\nПриложение автоматически создаст его!\n");
                         goto Label2;
                     }

                 Label2:
                     /* В аргументах инициализатора нового экземпляра класса, наряду с
                      * переменной "path", нужно указать свойство "true" - разрешена
                        дозапись в существующий файл или "false" - переписать файл
                     StreamWriter sw = new StreamWriter(path, true);
                     sw.WriteLine(e.Message.Photo);
                     // Записываем текущие дату и время в файл
                     //sw.WriteLine(e.Message.Photo);
                     Console.WriteLine("Запись прошла успешно!");
                     /* Перед выходом из приложения не забываем закрывать файл 
                     sw.Close();*/
                    client.SendTextMessageAsync(e.Message.Chat.Id, $"Я её сохранил!)");
                    break;
                default:
                    client.SendTextMessageAsync(e.Message.Chat.Id, $"Ты прислал мне {e.Message.Type}, но я это пока не понимаю");
                    Console.WriteLine(e.Message.Type);
                    break;
            }
        }
        
        /// <summary>
        /// Обработка команды
        /// </summary>
        /// <param name="message"></param>
        private void CommandProcessor(Telegram.Bot.Types.Message message)
        {
            // Отрезаем первый символ (который должен быть '/')
            string command = message.Text.Substring(1).ToLower();

            switch (command)
            {
                case "start":
                    var button = new KeyboardButton("Поделись телефоном");
                    button.RequestContact = true;
                    var array = new KeyboardButton[] { button };
                    var reply = new ReplyKeyboardMarkup(array, true, true);
                        client.SendTextMessageAsync(message.Chat.Id, $"Привет, {message.Chat.FirstName}, скажи мне свой телефон", replyMarkup: reply); 
                    break;
                case "help":
                    client.SendTextMessageAsync(message.Chat.Id, $"Вот мой список команд: /help - список команд /start - авторизация по номеру ");
                    //client.SendTextMessageAsync(message.Chat.Id, $"help - список команд");
                    //client.SendTextMessageAsync(message.Chat.Id, $"start - авторизация по номеру");
                    break;
                default:
                    client.SendTextMessageAsync(message.Chat.Id, $"Я пока не понимаю команду {command}");
                    break;
            }
        }

        /// <summary>
        /// Запуск бота
        /// </summary>
        public void Run()
        {
            // Запуск приема сообщений
            client.StartReceiving();
        }
    }
}
