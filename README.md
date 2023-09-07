# Тестовое задание для CrossInform

## Задача
Необходимо написать консольное приложение на C#, выполняющее частотный анализ текста.
Входные параметры: путь к текстовому файлу.
Выходные результаты: вывести на экран 10 самых часто встречающихся в тексте триплетов (3 идущих подряд буквы слова) и число их повторений, и на последней строке время работы программы в миллисекундах.
Требования: программа должна обрабатывать текст в многопоточном режиме.

## Решение
Для проверки возможности нагрузки и быстродействия программы, было разработано четыре класса для нахождения триплетов с разными подходами:
- Последовательная обработка;
- Последовательная обработка с использованием `Span`;
- Параллельная обработка;
- Параллельная обработка с использованием  `Span`.

Оценка работы представлена в таблице ниже.

## Результат 

| Количество символов в файл  | Размер файла | Последовательная обработка | Последовательная обработка с использованием `Span` | Параллельная обработка | Параллельная обработка с использованием  `Span` |
| ------------- | ------------- | ------------- | ------------- | ------------- | ------------- |
| 100  | 112 Б  | 52 мс | 9 мс | 46 мс | 5 мс |
| 1 144 300  | 2.02 МБ | 272 мс | 111 мс | 198 мс | 77 мс |
| 68 658 000  | 121 МБ | 12 910 мс | 6 286 мс | 3 592 мс | 2 426 мс |
| 1 073 742 895  | 1.02 ГБ | 92 685 мс | 65 312 мс | 32 956 мс | 27 259 мс |

## Вывод
Наилучшие результаты во всех тестах показала параллельная обработка с исиользованием `Span`.
