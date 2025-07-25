# Мастерская № 3 «Web и сервисы — открываем мир!»
Приложение клиент-серверной архитектуры, которое объединяет множество баз данных и выводит на одной странице. Подробно [тут](https://github.com/Meekl-e/ssyp-2025/blob/master/%D0%9E%D0%BF%D0%B8%D1%81%D0%B0%D0%BD%D0%B8%D0%B5_%D0%BC%D0%B0%D1%81%D1%82%D0%B5%D1%80%D1%81%D0%BA%D0%BE%D0%B9.md).

## Структура репозитория
1. [Базы данных: `datasets/`](https://github.com/Meekl-e/ssyp-2025/tree/master/datasets)
2. [Ресурсы: `recources/`](https://github.com/Meekl-e/ssyp-2025/tree/master/resources)
3. [Информация о проекте](https://github.com/Meekl-e/ssyp-2025/blob/master/%D0%9E%D0%BF%D0%B8%D1%81%D0%B0%D0%BD%D0%B8%D0%B5_%D0%BC%D0%B0%D1%81%D1%82%D0%B5%D1%80%D1%81%D0%BA%D0%BE%D0%B9.md)
4. [Учебные материалы](https://github.com/Meekl-e/ssyp-2025/tree/master/masters)

## Сборка и запуск
1. Скачать .NET версии 9 (желательно 9.0.302, на которой разрабатывался проект). Ссылки для скачивания x64 ([остальные на сайте](https://dotnet.microsoft.com/en-us/download/dotnet/9.0)): [Windows](https://dotnet.microsoft.com/en-us/download/dotnet/thank-you/sdk-9.0.302-windows-x64-installer), [MacOS](https://dotnet.microsoft.com/en-us/download/dotnet/thank-you/sdk-9.0.302-macos-x64-installer), [Linux](https://learn.microsoft.com/dotnet/core/install/linux?WT.mc_id=dotnet-35129-website), [All](https://dotnet.microsoft.com/en-us/download/dotnet/scripts).
2. Клонировать проект `git clone https://github.com/Meekl-e/ssyp-2025.git`
3. Перейти в папку project
4. Создать файл `api_key.env` в папке `project` и установить туда свой ключ от GoogleAPI. Как его получить, смотреть [тут](https://github.com/Meekl-e/ssyp-2025/blob/master/masters/lesson_6_google_api.md). Конечный файл должен содержать только API ключ, поскольку код берет ключ через File.ReadAllText("api_key.env")
5. Установить зависимости командами: `dotnet add package Nestor -v 0.5.2` и `dotnet add package Newtonsoft.Json -v 13.0.3`
6. Запустить проект командой `dotnet run`
7. Перейти по ссылке из консоли на localhost


## Состав мастерской
### Ученики:
* Пирогов Антон
* Новиков Никита
* Глушков Дмитрий
### Руководитель
Александр Гурьевич Марчук
### Подмастерье
Михаил Ноговицин
