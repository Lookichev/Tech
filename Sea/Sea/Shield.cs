using System;
using System.Collections.Generic;
using System.Windows;

namespace Sea
{
	class Shield
	{
		/// <summary>
		/// Множитель рейтинга дальности пути
		/// </summary>
		private const int _BalanceCurse = 10;

		/// <summary>
		/// Множитель рейтинга количества мин
		/// </summary>
		private const int _BalanceMines = 300;

		/// <summary>
		/// Множитель рейтинга вариативности
		/// </summary>
		private const int _BalanceVariables = 300;

		/// <summary>
		/// Позиция левой верхней точки на карте
		/// </summary>
		private readonly Point _StartPosition;

		/// <summary>
		/// Шаг между точками по вертикале и горизонтале
		/// </summary>
		private readonly Point _Step;


		/// <summary>
		/// Доска позиций
		/// </summary>
		private Position[][] _Plane = new Position[22][];

		/// <summary>
		/// Местонахождение пиратов
		/// </summary>
		private int[] _PirateLocation = new int[2];



		/// <summary>
		/// Оповещение о победе
		/// </summary>
		public event EventHandler WinEvent;

		/// <summary>
		/// Оповещение о поражении
		/// </summary>
		public event EventHandler LoseEvent;



		/// <summary>
		/// Конструктор
		/// </summary>
		/// <param name="start">Координата первой точки</param>
		/// <param name="step">Шаг между точками</param>
		public Shield(Point start, Point step)
		{
			//Сохранение исходных данных позиционирования
			_StartPosition = start; _Step = step;

			var pos = _StartPosition;

			//Заполнения поля точками
			for (var i = 0; i < 22; i++)
			{
				_Plane[i] = new Position[16];

				for (var j = 0; j < 16; j++)
				{
					//Если : точка на карте доступна - она пустая, иначе - закрытая
					var startStatus = (i % 2 == 0 && j % 2 == 0) || (i % 2 != 0 && j % 2 != 0) 
						? StatusPoint.Empty 
						: StatusPoint.Close;

					_Plane[i][j] = new Position(new Point(pos.X + 15.5, pos.Y + 15.5), startStatus);

					pos.Y += step.Y;
				}

				pos.Y = _StartPosition.Y;
				pos.X += _Step.X;
			}
		}

		/// <summary>
		/// Обработка нажатия на поле игроком
		/// </summary>
		/// <param name="x">Координата горизонтали</param>
		/// <param name="y">Координата вертикали</param>
		/// <returns>null - если касание не защитано, иначе - точка с новой миной</returns>
		public Position PlayerClick(Point point)
		{
			//Поиск ближайшей точки к месту касания
			Position dot = null;
			var i = (int)Math.Floor((point.X - _StartPosition.X) / _Step.X);
			var j = (int)Math.Floor((point.Y - _StartPosition.Y) / _Step.Y);

			try
			{
				dot = _Plane[i][j];
			}
			catch { return null; }

			//Если : точка занята миной или кораблем - ничего не делаем
			if (dot.Status != StatusPoint.Empty) return null;

			//Определение, попало-ли касание в радиус срабатывания 
			if (point.X < dot.ClickRectangle.X || point.X > dot.ClickRectangle.X + dot.ClickRectangle.Width ||
				point.Y < dot.ClickRectangle.Y || point.Y > dot.ClickRectangle.Y + dot.ClickRectangle.Height) return null;

			dot.Status = StatusPoint.Mine;

			return dot;
		}

		/// <summary>
		/// Ход пиратов
		/// </summary>
		/// <returns>Новое положение пиратов</returns>
		public Position PirateGo()
		{
			//Если : пираты уже на границе поля - поражение
			if (_PirateLocation[0] == 0 || _PirateLocation[0] == 21 || _PirateLocation[1] == 0 || _PirateLocation[1] == 15)
			{
				LoseEvent.Invoke(this, new EventArgs());
				return null;
			}

			//Рейтинг выбора движения для пиратов
			//[0] - северо-запад
			//[1] - северо-восток
			//[2] - юго-восток
			//[3] - юго-запад
			var rating = new int[] { 10000, 10000, 10000, 10000 };

			//Определение разрешенных направлений
			for (var i = 0; i < 4; i++)
			{
				var address = GetNewPosition(_PirateLocation, (Direction)i);
				//Если : путь кораблю в соответствующем направлении закрыт - туда не ходим
				if (_Plane[address[0]][address[1]].Status == StatusPoint.Mine) rating[i] = -10000;
			}

			//Проверка зажат-ли кораблик минами
			if (rating[0] == -10000 && rating[1] == -10000 && rating[2] == -10000 && rating[3] == -10000)
			{
				WinEvent.Invoke(this, new EventArgs());
				return null;
			}


			#region Определение рейтинга ближайшего пути:

			//Каждая сторона света убавляет рейтинга по дальности до границы напрямую
			rating[0] -= _BalanceCurse * (_PirateLocation[1]); rating[1] -= _BalanceCurse * (_PirateLocation[1]);
			rating[1] -= _BalanceCurse * (21 - _PirateLocation[0]); rating[2] -= _BalanceCurse * (21 - _PirateLocation[0]);
			rating[2] -= _BalanceCurse * (15 - _PirateLocation[1]); rating[3] -= _BalanceCurse * (15 - _PirateLocation[1]);
			rating[3] -= _BalanceCurse * (_PirateLocation[0]); rating[0] -= _BalanceCurse * (_PirateLocation[0]);

			#endregion

			#region  Определение рейтинга опасности:

			//Каждая мина на прямой и по сторонам от главного курса уменьшает рейтинг пути
			for (var i = 0; i < 4; i++)
			{
				var tick = 0;
				int[] address = _PirateLocation;
				int[] temp = null;

				//Ведем подсчет, пока не достигнем конца курса
				while(true)
				{
					//Получаем первое смещение по курсу
					address = GetNewPosition(address, (Direction)i);

					//Если : уперлись в границу карты - выход
					if (address == null) break;

					//Каждая мина изменяет рейтинг пути
					if (_Plane[address[0]][address[1]].Status == StatusPoint.Mine) tick++;

					//Учитываем мину при повороте курса против часовой стрелки
					temp = GetNewPosition(address, RollCourse(i - 1));
					if (temp != null && _Plane[temp[0]][temp[1]].Status == StatusPoint.Mine) tick++;
					//Учитываем мину при повороте курса по часовой стрелке
					temp = GetNewPosition(address, RollCourse(i + 1));
					if (temp != null && _Plane[temp[0]][temp[1]].Status == StatusPoint.Mine) tick++;
				}

				rating[i] -= _BalanceMines * tick;
			}

			#endregion

			#region Определение рейтинга вариативности:

			//Кол-во вариантов выбора пути после совершения хода
			for(var i = 0; i < 4; i++)
			{
				var tick = 0;
				var address = GetNewPosition(_PirateLocation, (Direction)i);
				int[] temp = null;

				for(var j = 0; j < 4; j++)
				{
					temp = GetNewPosition(address, (Direction)j);
					if (temp != null && _Plane[temp[0]][temp[1]].Status == StatusPoint.Mine) tick++;
				}

				rating[i] -= _BalanceVariables * tick;
			}

			#endregion

			var dic = new Dictionary<int, int>(4);
			for (var i = 0; i < 4; i++) dic.Add(i, rating[i]);

			//Смена локации корабля
			_Plane[_PirateLocation[0]][_PirateLocation[1]].Status = StatusPoint.Empty;

			_PirateLocation = GetNewPosition(_PirateLocation, (Direction)GetTopCurse(dic, GetTopCurse(dic, 0, 1), GetTopCurse(dic, 2, 3)));

			_Plane[_PirateLocation[0]][_PirateLocation[1]].Status = StatusPoint.Ship;
			return _Plane[_PirateLocation[0]][_PirateLocation[1]];
		}

		/// <summary>
		/// Перезапуск игры
		/// </summary>
		public void Restart()
		{
			foreach (var pos in _Plane) foreach (var dot in pos) if (dot.Status != StatusPoint.Close) dot.Status = StatusPoint.Empty;

			_PirateLocation[0] = 10; _PirateLocation[1] = 8;
			_Plane[10][8].Status = StatusPoint.Ship;
		}

		/// <summary>
		/// Возвращает самый высокорейтинговый курс
		/// </summary>
		private int GetTopCurse(Dictionary<int, int> dic, int keyOne, int keyTwo)
		{
			var random = new Random();
			if (dic[keyOne] == dic[keyTwo]) return random.Next(0, 2) == 0 ? keyOne : keyTwo;
			else return Math.Max(dic[keyOne], dic[keyTwo]) == dic[keyOne] ? keyOne : keyTwo;
		}

		/// <summary>
		/// Безопасно поворачивает курс
		/// </summary>
		private Direction RollCourse(int dir)
		{
			if (dir < 0) return Direction.Southwest;
			if (dir > 3) return Direction.Northwest;
			return (Direction)dir;
		}

		/// <summary>
		/// Возвращает адрес новой точки по указанному направлению
		/// </summary>
		/// <param name="dir">Направление движения</param>
		private int[] GetNewPosition(int[] position, Direction dir)
		{
			var i = position[0];
			var j = position[1];

			switch(dir)
			{
				case Direction.Northwest:
					i -= 1; j -= 1;
					break;
				case Direction.Northeast:
					i += 1; j -= 1;
					break;
				case Direction.Southeast:
					i += 1; j += 1;
					break;
				case Direction.Southwest:
					i -= 1; j += 1;
					break;
			}

			//Выход за границы поля
			if (i < 0 || i > 21) return null;
			if (j < 0 || j > 15) return null;

			return new int[] { i, j };
		}

		/// <summary>
		/// Описывает точку на карте
		/// </summary>
		internal class Position
		{
			private StatusPoint _Status;

			/// <summary>
			/// Центр точки на карте
			/// </summary>
			public Point Center { get; }

			/// <summary>
			/// Область реагирования точки
			/// </summary>
			public Rect ClickRectangle { get; }

			/// <summary>
			/// Состояние точки
			/// </summary>
			internal StatusPoint Status
			{
				get { return _Status; }
				set { if (_Status != StatusPoint.Close) _Status = value; }
			}



			/// <summary>
			/// Конструирует точку карты
			/// </summary>
			internal Position(Point center, StatusPoint status)
			{
				Center = center;
				Status = status;
				ClickRectangle = new Rect(new Point(Center.X - 15.5, Center.Y - 15.5), new Size(31, 31));
			}
		}

		/// <summary>
		/// Состояние точки на карте
		/// </summary>
		public enum StatusPoint : int
		{
			Close = -1,
			Empty = 0,
			Ship = 1,
			Mine = 2
		}

		/// <summary>
		/// Направление движения пиратов
		/// </summary>
		public enum Direction : int
		{
			/// <summary>
			/// Северо-запад
			/// </summary>
			Northwest = 0,

			/// <summary>
			/// Северо-восток
			/// </summary>
			Northeast = 1,

			/// <summary>
			/// Юго-восток
			/// </summary>
			Southeast = 2,

			/// <summary>
			/// Юго-запад
			/// </summary>
			Southwest = 3
		}
	}
}
