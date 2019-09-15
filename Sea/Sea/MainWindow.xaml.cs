using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Sea
{
	/// <summary>
	/// Логика взаимодействия для MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		/// <summary>
		/// Текст при старте игры
		/// </summary>
		private readonly string _StartString = "Эй, эй, эй! Тысяча чертей!\nСтарпом свалил на нашем корабле с награбленным, пока мы пили ром на острове!\nХватай наши запасы мин и топи эту сухопутную крысу!\nЙииииии - хаааа!";

		/// <summary>
		/// Текст при победе в игре
		/// </summary>
		private readonly string _WinString = "А мне понравилось! Аха-ха-ха!\nКто следующий в очереди? Поехааааали!";

		/// <summary>
		/// Текст при поражении в игре
		/// </summary>
		private readonly string _LoseString = "Тысяча чертей и старпом!\nЭтот недобитый матрос нас уделал:-(";

		/// <summary>
		/// Фон стола
		/// </summary>
		private readonly BitmapImage _DeskBitmapImage = new BitmapImage(new Uri(Environment.CurrentDirectory + "\\back.jpg"));

		/// <summary>
		/// Корабль пиратов
		/// </summary>
		private readonly BitmapImage _ShipBitmapImage = new BitmapImage(new Uri(Environment.CurrentDirectory + "\\ship.png"));

		/// <summary>
		/// Мина
		/// </summary>
		private readonly BitmapImage _MineBitmapImage = new BitmapImage(new Uri(Environment.CurrentDirectory + "\\mine.png"));

		/// <summary>
		/// Коллекция мин
		/// </summary>
		private readonly List<Image> _MinesList = new List<Image>();

		/// <summary>
		/// Игровое поле
		/// </summary>
		private Shield _Shield;



		public MainWindow()
		{
			InitializeComponent();

			//Инициализация стартовых элементов
			_Shield = new Shield(new Point(181, 146), new Point(31, 31));
			_DeskImage.Source = _DeskBitmapImage;
			_PirateShip = new Image() { Width = 82, Height = 82, Source = _ShipBitmapImage };
			_UpShield.Children.Add(_PirateShip);
			_StartText.Text = _StartString;

			//Подписка на оповещение о поражении
			_Shield.LoseEvent += (q, qq) =>
			{
				_StartText.Text = _LoseString;
				_StartGamePanel.Visibility = Visibility.Visible;
			};
			//Подписка на оповещение о победе
			_Shield.WinEvent += (q, qq) =>
			{
				_StartText.Text = _WinString;
				_StartGamePanel.Visibility = Visibility.Visible;
			};
		}

		/// <summary>
		/// Инициализация мины
		/// </summary>
		/// <returns>Картинка мины</returns>
		private void CreateMineImage(Point center)
		{
			var position = new Thickness
			{
				Left = center.X - 24,
				Top = center.Y - 27
			};

			var image = new Image() { Width = 48, Height = 54, Source = _MineBitmapImage, Margin = position };

			_UpShield.Children.Add(image);
			_MinesList.Add(image);
		}

		/// <summary>
		/// Нажатие по панели
		/// </summary>
		private void Shield_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
		{
			//Поиск точки на карте под установку мины
			var position = _Shield.PlayerClick(e.GetPosition((IInputElement)sender));

			//Если : свободная точка найдена
			if (position != null)
			{
				//Установка мины
				CreateMineImage(position.Center);

				//Перемещение пиратов
				var newPos = _Shield.PirateGo();

				//Если : наступил конец игры - ничего не делаем
				if (newPos == null) return;

				Canvas.SetLeft(_PirateShip, newPos.Center.X - 41);
				Canvas.SetTop(_PirateShip, newPos.Center.Y - 41);

				InvalidateVisual();
			}
		}

		/// <summary>
		/// Обработка события рестарта
		/// </summary>
		private void _RestartButton_Click(object sender, RoutedEventArgs e)
		{
			foreach (var el in _MinesList)
			{
				el.Visibility = Visibility.Collapsed;
				_UpShield.Children.Remove(el);
			}
			_MinesList.Clear();

			Canvas.SetLeft(_PirateShip, 460);
			Canvas.SetTop(_PirateShip, 363);

			_Shield.Restart();

			_StartGamePanel.Visibility = Visibility.Collapsed;
		}
	}
}
