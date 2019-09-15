using System;
using System.Collections.Generic;
using System.Net;
using System.Text.RegularExpressions;
using System.Windows.Media.Imaging;
using System.Xml.Linq;

namespace Money
{
	public class Client
	{
		/// <summary>
		/// Базовый адрес сайта
		/// </summary>
		private string _BaseAddress;

		/// <summary>
		/// Разбитый адрес загрузки
		/// </summary>
		private string[] _Uri = new string[3];

		/// <summary>
		/// Включения в путь
		/// </summary>
		private string[] _Passes = new string[3] { "1010", "00.00.2000", "00.00.2001" };

		/// <summary>
		/// Искомая строка в html сайта
		/// </summary>
		private string _Pattern;

		/// <summary>
		/// Средство загрузки
		/// </summary>
		private WebClient _WebClient = new WebClient();



		/// <summary>
		/// Вовзращает собранную строку ссылки на график курсов
		/// </summary>
		public string Uri => string.Concat(_Uri[0], _Passes[0], _Uri[1], _Passes[1], _Uri[2], _Passes[2]);

		public Client()
		{
			//Получаем первый узел связанного списка элементов из корневого узла разметки
			LinkedListNode<XElement> elementsXML = new LinkedList<XElement>(XDocument.Load(Environment.CurrentDirectory + "\\MainParams.xml").Root.Elements()).First;

			_BaseAddress = elementsXML.Value.Attribute("Uri").Value;
			elementsXML = elementsXML.Next;

			//Второй узел дает две части ссылки скачивания картинки, со вставками амперсандов
			_Uri[0] = elementsXML.Value.Attribute("BasePart").Value.Replace("p#p", "&");
			_Uri[1] = elementsXML.Value.Attribute("CurrencyPart").Value.Replace("p#p", "&");
			_Uri[2] = elementsXML.Value.Attribute("ExtraPart").Value.Replace("p#p", "&");
			elementsXML = elementsXML.Next;

			//Третий узел дает паттерн для поиска ссылки на картинку
			_Pattern = elementsXML.Value.Attribute("String").Value;
		}

		/// <summary>
		/// Устанавливает период просмотра графика
		/// </summary>
		/// <param name="fromDate">С какой даты начало просмотра</param>
		/// <param name="toDate">До какой даты конец просмотра</param>
		public void SetPeriod(string fromDateDay, string fromDateMonth, string fromDateYear, string toDateDay, string toDateMonth, string toDateYear)
		{
			_Passes[1] = string.Concat(fromDateDay, '.', fromDateMonth, '.', fromDateYear);
			_Passes[2] = string.Concat(toDateDay, '.', toDateMonth, '.', toDateYear);
		}

		/// <summary>
		/// Устанавливает валюту просмотра графика
		/// </summary>
		/// <param name="currency">Тип валюты</param>
		public void SetCurrency(string currency)
		{
			_Passes[0] = currency;
		}

		/// <summary>
		/// Возвращает загруженную картинку для отображения
		/// </summary>
		public BitmapImage GetImage()
		{
			var str = Regex.Match(_WebClient.DownloadString(Uri), _Pattern).Value;

			str = string.Concat(_BaseAddress, str.Replace("&amp;", "&"));

			var imageUri = Environment.CurrentDirectory + "\\image.png";

			try
			{
				_WebClient.DownloadFile(str, imageUri);
			}
			catch(WebException)
			{
				return null;
			}

			return new BitmapImage(new Uri(imageUri));
		}

		/*
		/// <summary>
		/// Типы отображаемых валют
		/// </summary>
		public enum Currency : int
		{
			/// <summary>
			/// Ирландский фунт
			/// </summary>
			R01305 = 0,
			/// <summary>
			/// Исландская крона
			/// </summary>
			R01310 = 1,
			/// <summary>
			/// Испанская песета
			/// </summary>
			R01315 = 2,
			/// <summary>
			/// Итальянская лира
			/// </summary>
			R01325 = 3,
			/// <summary>
			/// Йеменский риал
			/// </summary>
			R01330 = 4,
			/// <summary>
			/// Казахстанский тенге
			/// </summary>
			R01335 = 5,
			/// <summary>
			/// Канадский доллар
			/// </summary>
			R01350 = 6,
			/// <summary>
			/// Катарский риал
			/// </summary>
			R01355 = 7,
			/// <summary>
			/// Кенийский шиллинг
			/// </summary>
			R01360 = 8,
			/// <summary>
			/// Кипрский фунт
			/// </summary>
			R01365 = 9,
			/// <summary>
			/// Киргизский сом
			/// </summary>
			R01370 = 10,
			/// <summary>
			/// Китайский юань
			/// </summary>
			R01375 = 11,
			/// <summary>
			/// Колумбийский песо
			/// </summary>
			R01380 = 12,
			/// <summary>
			/// Конголезский франк
			/// </summary>
			R01383 = 13,
			/// <summary>
			/// Костариканский колон
			/// </summary>
			R01385 = 14,
			/// <summary>
			/// Кубинское песо
			/// </summary>
			R01395 = 15,
			/// <summary>
			/// Кувейтский динар
			/// </summary>
			R01390 = 16,
			/// <summary>
			/// Лаосский кип
			/// </summary>
			R01400 = 17,
			/// <summary>
			/// Латвийский лат
			/// </summary>
			R01405 = 18,
			/// <summary>
			/// Леоне Сьерра-Леоне
			/// </summary>
			R01410 = 19,
			/// <summary>
			/// Ливанский фунт
			/// </summary>
			R01420 = 20,
			/// <summary>
			/// Ливийский динар
			/// </summary>
			R01425 = 21,
			/// <summary>
			/// Лилангени Свазиленда
			/// </summary>
			R01430 = 22,
			/// <summary>
			/// Литовский лит
			/// </summary>
			R01435 = 23,
			/// <summary>
			/// Маврикийская рупия
			/// </summary>
			R01445 = 24,
			/// <summary>
			/// Мавританская угия
			/// </summary>
			R01450 = 25,
			/// <summary>
			/// Македонский дина
			/// </summary>
			R01460 = 26,
			/// <summary>
			/// Малавийская квач
			/// </summary>
			R01465 = 27,
			/// <summary>
			/// Малагасийский ариари
			/// </summary>
			R01470 = 28,
			/// <summary>
			/// Малайзийский ринггит
			/// </summary>
			R01475 = 29,
			/// <summary>
			/// Мальтийская лира
			/// </summary>
			R01480 = 30,
			/// <summary>
			/// Марокканский дирхам
			/// </summary>
			R01485 = 31,
			/// <summary>
			/// Мексиканский песо
			/// </summary>
			R01495 = 32,
			/// <summary>
			/// Мозамбикский метикал
			/// </summary>
			R01498 = 33,
			/// <summary>
			/// Молдавский лей
			/// </summary>
			R01500 = 34,
			/// <summary>
			/// Монгольский тугрик
			/// </summary>
			R01503 = 35,
			/// <summary>
			/// Немецкая марка
			/// </summary>
			R01510 = 36,
			/// <summary>
			/// Непальская рупия
			/// </summary>
			R01515 = 37,
			/// <summary>
			/// Нигерийский найр
			/// </summary>
			R01520 = 38,
			/// <summary>
			/// Нидерландский гульден
			/// </summary>
			R01523 = 39,
			/// <summary>
			/// Никарагуанская золотая к
			/// </summary>
			R01525 = 40,
			/// <summary>
			/// Новозеландский доллар
			/// </summary>
			R01530 = 41,
			/// <summary>
			/// Новый туркменский манат
			/// </summary>
			R01710 = 42,
			/// <summary>
			/// Норвежская крона
			/// </summary>
			R01535 = 43,
			/// <summary>
			/// Оманский риал
			/// </summary>
			R01540 = 44,
			/// <summary>
			/// Пакистанская рупия
			/// </summary>
			R01545 = 45,
			/// <summary>
			/// Парагвайская гуарани
			/// </summary>
			R01555 = 46,
			/// <summary>
			/// Перуанский новый соль
			/// </summary>
			R01560 = 47,
			/// <summary>
			/// Польский злотый
			/// </summary>
			R01565 = 48,
			/// <summary>
			/// Португальский эскудо
			/// </summary>
			R01570 = 49,
			/// <summary>
			/// Риель Камбоджи
			/// </summary>
			R01575 = 50,
			/// <summary>
			/// Риял Саудовской Аравии
			/// </summary>
			R01580 = 51,
			/// <summary>
			/// Румынский лей
			/// </summary>
			R01585 = 52,
			/// <summary>
			/// СДР
			/// </summary>
			R01589 = 53,
			/// <summary>
			/// Сейшельская рупия
			/// </summary>
			R01595 = 54,
			/// <summary>
			/// Сингапурский доллар
			/// </summary>
			R01625 = 55,
			/// <summary>
			/// Сирийский фунт
			/// </summary>
			R01630 = 56,
			/// <summary>
			/// Словацкая крона
			/// </summary>
			R01635 = 57,
			/// <summary>
			/// Словенский толар
			/// </summary>
			R01640 = 58,
			/// <summary>
			/// Сомалийский шиллинг
			/// </summary>
			R01650 = 59,
			/// <summary>
			/// Суданский фунт
			/// </summary>
			R01660 = 60,
			/// <summary>
			/// Суринамский доллар
			/// </summary>
			R01665 = 61,
			/// <summary>
			/// Таджикский рубл
			/// </summary>
			R01670 = 62,
			/// <summary>
			/// Таиландский бат
			/// </summary>
			R01675 = 63,
			/// <summary>
			/// Тайваньский новый доллар
			/// </summary>
			R01680 = 64,
			/// <summary>
			/// Так Бангладеш
			/// </summary>
			R01685 = 65,
			/// <summary>
			/// Танзанийский шиллинг
			/// </summary>
			R01690 = 66,
			/// <summary>
			/// Тунисский динар
			/// </summary>
			R01695 = 67,
			/// <summary>
			/// Турецкая лира
			/// </summary>
			R01700 = 68,
			/// <summary>
			/// Угандийский шиллинг
			/// </summary>
			R01714 = 69,
			/// <summary>
			/// Узбекский сум
			/// </summary>
			R01717 = 70,
			/// <summary>
			/// Украинская гривна
			/// </summary>
			R01720 = 71,
			/// <summary>
			/// Уругвайское песо
			/// </summary>
			R01725 = 72,
			/// <summary>	
			/// Филиппинское песо	
			/// </summary>
			R01743 = 73,
			/// <summary>
			/// Финляндская марка
			/// </summary>
			R01740 = 74,
			/// <summary>
			/// Франк Джибути
			/// </summary>
			R01746 = 75,
			/// <summary>
			/// Франк КФА ВЕАС
			/// </summary>
			R01748 = 76,
			/// <summary>
			/// Франк КФА ВСЕАО
			/// </summary>
			R01749 = 77,
			/// <summary>
			/// Французский франк
			/// </summary>
			R01750 = 78,
			/// <summary>
			/// Фунт стерлингов Соединен
			/// </summary>
			R01035 = 79,
			/// <summary>
			/// Хорватская куна
			/// </summary>
			R01755 = 80,
			/// <summary>
			/// Чехословацкая крона
			/// </summary>
			R01761 = 81,
			/// <summary>
			/// Чешская крона
			/// </summary>
			R01760 = 82,
			/// <summary>
			/// Чилийское песо
			/// </summary>
			R01765 = 83,
			/// <summary>
			/// Шведская крона
			/// </summary>
			R01770 = 84,
			/// <summary>
			/// Швейцарский франк
			/// </summary>
			R01775 = 85,
			/// <summary>
			/// Шри - Ланкийская рупия
			/// </summary>
			R01780 = 86,
			/// <summary>
			/// Эквадорский сукре
			/// </summary>
			R01785 = 87,
			/// <summary>
			/// ЭКЮ
			/// </summary>
			R01790 = 88,
			/// <summary>
			/// Эстонская крона
			/// </summary>
			R01795 = 89,
			/// <summary>
			/// Эфиопский быр
			/// </summary>
			R01800 = 90,
			/// <summary>
			/// Югославский новый динар
			/// </summary>
			R01804 = 91,
			/// <summary>
			/// Южноафриканский рэнд
			/// </summary>
			R01810 = 92,
			/// <summary>
			/// Японская иена
			/// </summary>
			R01820 = 93
		}*/
	}
}