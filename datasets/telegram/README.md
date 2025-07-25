# Информация из телеграма
Информация из канала [@LSHUP](https://t.me/LSHUP) с 2022 года по май 2025

## Структуры файлов
#### Структура LSHUP.csv 

ID| date | text | views | Media
-----------|-------|-------|-------|-------|
Индетификатор (с 1) | Дата (в формате секунд с начала эпохи) | Текст поста (в кодировке `base64`) | просмотры поста | Наличие медиа файлов (NoneType или расширение файла) 

#### Структура файлов медиа (LSHUP_media)
Каждый файл совпадает с ID сообщения, к которому привязан.
Например, сообщению с ID 131 соотвествует файл 131.jpg

#### Некоторых файлов может не быть, оригинал базы данных: 
[https://disk.yandex.ru/d/T_1_FJ_kYd0QTg](https://disk.yandex.ru/d/T_1_FJ_kYd0QTg )

## API Google таблиц
Не рекомендуется использовать данную базу данных, вместо этого воспользуйтесь API Google таблиц

[https://docs.google.com/spreadsheets/d/12OhmW7UWUHXsOi1mrSyczMs2UBHHcoPKnJ1pt3sFGAI](https://docs.google.com/spreadsheets/d/12OhmW7UWUHXsOi1mrSyczMs2UBHHcoPKnJ1pt3sFGAI) - аналогичная таблица с автоматическим обновлением.

Отличие в том, что есть столбец `url_file`, который соотвествует ID файлу на Google диске в папке: [https://drive.google.com/drive/folders/1gJq3hOfp0oilUjqENgJtjrdg6GRtHWF9?usp=drive_link](https://drive.google.com/drive/folders/1gJq3hOfp0oilUjqENgJtjrdg6GRtHWF9?usp=drive_link)

### Подключение к API 
TODO: дописать

## Ссылки
Данная база данных была создана с помощью [Телеграм архиватора](https://gitverse.ru/meekle/ssyp-2025)

