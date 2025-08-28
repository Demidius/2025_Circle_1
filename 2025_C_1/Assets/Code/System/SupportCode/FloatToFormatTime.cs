
    using System;

    namespace CodeBase.System.Services.Utilities
    {
        /// <summary>
        /// Утилитный класс для форматирования времени из секунд в строку "HH:mm:ss".
        /// </summary>
        public static class FloatToFormatTime
        {
            /// <summary>
            /// Преобразует общее количество секунд (float) в формат "HH:mm:ss".
            /// </summary>
            /// <param name="totalSeconds">Общее количество секунд (может быть дробным).</param>
            /// <returns>Строка вида "HH:mm:ss".</returns>
            public static string FormatTime(float totalSeconds)
            {
                // TimeSpan умеет работать с дробными секундами (от double)
                TimeSpan ts = TimeSpan.FromSeconds(totalSeconds);

                // если есть дни, переводим их в часы
                int hours = ts.Days * 24 + ts.Hours;

                // формируем строку с ведущими нулями
                return $"{hours:D2}:{ts.Minutes:D2}:{ts.Seconds:D2}";
            }
        }
    }

