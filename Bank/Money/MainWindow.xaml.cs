using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace Money
{
	/// <summary>
	/// Логика взаимодействия для MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		/// <summary>
		/// Клиент логики взаимодействия с интернетом
		/// </summary>
		private Client _Client = new Client();



		public MainWindow()
		{
			InitializeComponent();

			_CurrencySelector.ItemsSource = Bank.Currency.Select(t => t.Value);
			_CurrencySelector.SelectedIndex = 0;

			_ErrorLabel.Content = "Центробанк не располагает сведениями\nо динамике данного курса в указанный период";
		}

		/// <summary>
		/// Обработка события отображения картинки
		/// </summary>
		/// <param name="sender">источник</param>
		/// <param name="e">аргументы</param>
		private void _ShowButton_Click(object sender, RoutedEventArgs e)
		{
			//Установка даты
			_Client.SetPeriod(_FromDay.Text, _FromMonth.Text, _FromYear.Text, _ToDay.Text, _ToMonth.Text, _ToYear.Text);

			//Установка валюты
			_Client.SetCurrency(Bank.CurrencyCode[_CurrencySelector.SelectedIndex].ToString());

			//Получаем картинку
			_ImageBox.Source = _Client.GetImage();

			_ErrorLabel.Visibility = _ImageBox.Source == null ? Visibility.Visible : Visibility.Collapsed;
		}
	}
}
