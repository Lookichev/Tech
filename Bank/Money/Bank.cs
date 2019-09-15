using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Money
{
	public static class Bank
	{
		/// <summary>
		/// Таблица кодировок и строковых значений валют
		/// </summary>
		public static Dictionary<int, string> Currency { get; }

		/// <summary>
		/// Нумерованный список кодов
		/// </summary>
		public static List<int> CurrencyCode { get; }

		/// <summary>
		/// Код валюты начинается на "R0" + Key
		/// </summary>
		static Bank()
		{
			Currency = new Dictionary<int, string>();

			Currency.Add(1010, "Австралийский доллар");
			Currency.Add(1015, "Австрийский шиллинг");
			Currency.Add(1020, "Азербайджанский манат");
			Currency.Add(1025, "Албанский лек");
			Currency.Add(1030, "Алжирский динар");
			Currency.Add(1040, "Ангольская новая кванза");
			Currency.Add(1055, "Аргентинское песо");
			Currency.Add(1060, "Армянский драм");
			Currency.Add(1065, "Афганский афгани");
			Currency.Add(1080, "Бахрейнский динар");
			Currency.Add(1090, "Белорусский рубль");
			Currency.Add(1095, "Бельгийский франк");
			Currency.Add(1100, "Болгарский лев ");
			Currency.Add(1105, "Боливийский боливиано");
			Currency.Add(1110, "Ботсванская пула");
			Currency.Add(1115, "Бразильский реал");
			Currency.Add(1111, "Брунейский доллар");
			Currency.Add(1120, "Бурундийский франк");
			Currency.Add(1135, "Венгерский форинт");
			Currency.Add(1140, "Венесуэльский боливар фу");
			Currency.Add(1815, "Вон Республики Корея");
			Currency.Add(1145, "Вона КНДР");
			Currency.Add(1150, "Вьетнамский донг");
			Currency.Add(1160, "Гамбийский даласи");
			Currency.Add(1165, "Ганский седи");
			Currency.Add(1175, "Гвинейский франк");
			Currency.Add(1200, "Гонконгский доллар");
			Currency.Add(1205, "Греческая драхма");
			Currency.Add(1210, "Грузинский лари");
			Currency.Add(1215, "Датская крона");
			Currency.Add(1230, "Дирхам ОАЭ");
			Currency.Add(1233, "Доллар Зимбабве");
			Currency.Add(2004, "Доллар Намибии");
			Currency.Add(1235, "Доллар США");
			Currency.Add(1239, "Евро");
			Currency.Add(1240, "Египетский фунт");
			Currency.Add(1245, "Заир ДРК");
			Currency.Add(1250, "Замбийская квача");
			Currency.Add(1265, "Израильский новый шекель");
			Currency.Add(1270, "Индийская рупия");
			Currency.Add(1280, "Индонезийская рупия");
			Currency.Add(1285, "Иорданский динар");
			Currency.Add(1290, "Иракский динар");
			Currency.Add(1300, "Иранский риал");
			Currency.Add(1305, "Ирландский фунт");
			Currency.Add(1310, "Исландская крона");
			Currency.Add(1315, "Испанская песета");
			Currency.Add(1325, "Итальянская лира");
			Currency.Add(1330, "Йеменский риал");
			Currency.Add(1335, "Казахстанский тенге");
			Currency.Add(1350, "Канадский доллар");
			Currency.Add(1355, "Катарский риал");
			Currency.Add(1360, "Кенийский шиллинг");
			Currency.Add(1365, "Кипрский фунт");
			Currency.Add(1370, "Киргизский сом");
			Currency.Add(1375, "Китайский юань");
			Currency.Add(1380, "Колумбийский песо");
			Currency.Add(1383, "Конголезский франк");
			Currency.Add(1385, "Костариканский колон");
			Currency.Add(1395, "Кубинское песо");
			Currency.Add(1390, "Кувейтский динар");
			Currency.Add(1400, "Лаосский кип");
			Currency.Add(1405, "Латвийский лат");
			Currency.Add(1410, "Леоне Сьерра-Леоне");
			Currency.Add(1420, "Ливанский фунт");
			Currency.Add(1425, "Ливийский динар");
			Currency.Add(1430, "Лилангени Свазиленда");
			Currency.Add(1435, "Литовский лит");
			Currency.Add(1445, "Маврикийская рупия");
			Currency.Add(1450, "Мавританская угия");
			Currency.Add(1460, "Македонский дина");
			Currency.Add(1465, "Малавийская квач");
			Currency.Add(1470, "Малагасийский ариари");
			Currency.Add(1475, "Малайзийский ринггит");
			Currency.Add(1480, "Мальтийская лира");
			Currency.Add(1485, "Марокканский дирхам");
			Currency.Add(1495, "Мексиканский песо");
			Currency.Add(1498, "Мозамбикский метикал");
			Currency.Add(1500, "Молдавский лей");
			Currency.Add(1503, "Монгольский тугрик");
			Currency.Add(1510, "Немецкая марка");
			Currency.Add(1515, "Непальская рупия");
			Currency.Add(1520, "Нигерийский найр");
			Currency.Add(1523, "Нидерландский гульден");
			Currency.Add(1525, "Никарагуанская золотая к");
			Currency.Add(1530, "Новозеландский доллар");
			Currency.Add(1710, "Новый туркменский манат");
			Currency.Add(1535, "Норвежская крона");
			Currency.Add(1540, "Оманский риал");
			Currency.Add(1545, "Пакистанская рупия");
			Currency.Add(1555, "Парагвайская гуарани");
			Currency.Add(1560, "Перуанский новый соль");
			Currency.Add(1565, "Польский злотый");
			Currency.Add(1570, "Португальский эскудо");
			Currency.Add(1575, "Риель Камбоджи");
			Currency.Add(1580, "Риял Саудовской Аравии");
			Currency.Add(1585, "Румынский лей");
			Currency.Add(1589, "СДР");
			Currency.Add(1595, "Сейшельская рупия");
			Currency.Add(1625, "Сингапурский доллар");
			Currency.Add(1630, "Сирийский фунт");
			Currency.Add(1635, "Словацкая крона");
			Currency.Add(1640, "Словенский толар");
			Currency.Add(1650, "Сомалийский шиллинг");
			Currency.Add(1660, "Суданский фунт");
			Currency.Add(1665, "Суринамский доллар");
			Currency.Add(1670, "Таджикский рубл");
			Currency.Add(1675, "Таиландский бат");
			Currency.Add(1680, "Тайваньский новый доллар");
			Currency.Add(1685, "Так Бангладеш");
			Currency.Add(1690, "Танзанийский шиллинг");
			Currency.Add(1695, "Тунисский динар");
			Currency.Add(1700, "Турецкая лира");
			Currency.Add(1714, "Угандийский шиллинг");
			Currency.Add(1717, "Узбекский сум");
			Currency.Add(1720, "Украинская гривна");
			Currency.Add(1725, "Уругвайское песо");
			Currency.Add(1743, "Филиппинское песо");
			Currency.Add(1740, "Финляндская марка");
			Currency.Add(1746, "Франк Джибути");
			Currency.Add(1748, "Франк КФА ВЕАС");
			Currency.Add(1749, "Франк КФА ВСЕАО");
			Currency.Add(1750, "Французский франк");
			Currency.Add(1035, "Фунт стерлингов Соединен");
			Currency.Add(1755, "Хорватская куна");
			Currency.Add(1761, "Чехословацкая крона");
			Currency.Add(1760, "Чешская крона");
			Currency.Add(1765, "Чилийское песо");
			Currency.Add(1770, "Шведская крона");
			Currency.Add(1775, "Швейцарский франк");
			Currency.Add(1780, "Шри - Ланкийская рупия");
			Currency.Add(1785, "Эквадорский сукре");
			Currency.Add(1790, "ЭКЮ");
			Currency.Add(1795, "Эстонская крона");
			Currency.Add(1800, "Эфиопский быр");
			Currency.Add(1804, "Югославский новый динар");
			Currency.Add(1810, "Южноафриканский рэнд");
			Currency.Add(1820, "Японская иена");

			CurrencyCode = new List<int>(Currency.Select(t => t.Key));
		}
	}
}
