﻿using System;
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
        /// Ведение журнала событий
        /// </summary>
        private NLog.Logger  log = NLog.LogManager.GetCurrentClassLogger();

        /// <summary>
        /// Конструктор без параметров
        /// </summary>
        public Bot()
        {
            // Создание клиента для Telegram
            client = new TelegramBotClient("1282004742:AAHYvVD7B_NixeIwGWPRoIZ1n9yJ5l5cNFY");
            //string token = Properties.Settings.Default.Token;
           // client = new TelegramBotClient(token);
            var user = client.GetMeAsync();
            Console.WriteLine(user.Result.Username);
            //string name = user.Result.Username;
            client.OnMessage += MessageProcessor;
        }

        /// <summary>
        /// Обработка входящего сообщения
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MessageProcessor(object sender, Telegram.Bot.Args.MessageEventArgs e)
        {
            try
            {
                log.Trace("|<- MassageProcessor");
                switch (e.Message.Type)
                {
                    case Telegram.Bot.Types.Enums.MessageType.Contact: // телефон
                        if (e.Message.Chat.Id == e.Message.Contact.UserId)
                        {
                            string phone = e.Message.Contact.PhoneNumber;
                            client.SendTextMessageAsync(e.Message.Chat.Id, $"Твой телефон: {phone}");
                            log.Trace(phone);
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
                            log.Trace(e.Message.Text);
                        }
                        break;
                    case Telegram.Bot.Types.Enums.MessageType.Photo:// фотография 
                        client.SendTextMessageAsync(e.Message.Chat.Id, $"Вы отправили мне кртинку");
                        log.Trace("Отправлено фото");

                        
                        client.SendTextMessageAsync(e.Message.Chat.Id, $"Я её сохранил!)");
                        break;
                    default:
                        client.SendTextMessageAsync(e.Message.Chat.Id, $"Ты прислал мне {e.Message.Type}, но я это пока не понимаю");
                        log.Info(e.Message.Type);
                        break;
                }
            }
            catch (Exception ex)
            {
                log.Warn(ex);
            }
            finally
            {
                log.Trace("|-> MessageProcessor");
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
