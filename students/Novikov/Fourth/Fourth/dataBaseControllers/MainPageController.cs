

public class MainPageContoller
{
    VkController vkController = new();
    private readonly Random rand = new();
    public IResult GetResult(HttpRequest request)
    {
        bool redirect = false;
        if (!int.TryParse(request.Query["vkN"], out int vkN))
        {
            vkN = rand.Next();
            redirect = true;
        }
        if (!int.TryParse(request.Query["tgN"], out int tgN))
        {
            tgN = rand.Next();
            redirect = true;
        }
        if (!int.TryParse(request.Query["oBN"], out int oBN))
        {
            oBN = rand.Next();
            redirect = true;
        }
        if (!int.TryParse(request.Query["cNewsN"], out int cNewsN))
        {
            cNewsN = rand.Next();
            redirect = true;
        }
        if (!int.TryParse(request.Query["academCN"], out int academCN))
        {
            academCN = rand.Next();
            redirect = true;
        }
        if (!int.TryParse(request.Query["elementyN"], out int elementyN))
        {
            elementyN = rand.Next();
            redirect = true;
        }
        if (redirect)
        {
            return Results.Redirect($"/?vkN={vkN}&tgN={tgN}&oBN={oBN}&cNewsN={cNewsN}&academCN={academCN}&elementyN={elementyN}");
        }
        return Results.Content(CreateHtml(vkN, tgN, oBN, cNewsN, academCN, elementyN), "text/html");
    }
    public MainPageContoller()
    {

    }

    public string CreateHtml(int vkN, int tgN, int oBN, int cNewsN, int academCN, int elementyN)
    {
        //return HtmlPage.GetHtml('', vkController.CreateField(vkN));
        return GetHtml(vkController.CreateField(vkN), GoogleSheetsReader.CreateField(tgN), "", "", "", "");
    }

    private string GetHtml(string vk, string tg, string oB, string cNews, string academC, string elementy)
    {
        return @$"<!DOCTYPE html>
<html>
    <head>
        <meta charset='utf-8'>
        <link rel='stylesheet' href='style.css'>
        <style>
            .article {{
                margin: 20px 12px 20px 12px;
            }}
            .art-heading {{
                margin-bottom: 14px;
                margin-top: 14px;
            }}
            .container_w {{
                display: flex;
                flex-direction: row;
                flex-wrap: wrap;
            }}
            .box {{
                width: 500px;
                min-height:500px;
            }}
            .rubrika {{
                width: 100%;
                background-color: lime;
                font-size: large;
                color: white;
            }}
        </style>
    </head>
    <body>

        <div class='container' style='width:100%;height: 60px; background-color:lime;justify-content: space-between;'>
            <a href='#'> <img src='logossyp.png' style='height:50px;margin-left: 16px;'> </a>

            <div class='container'>
                <a href='' style='margin: 0px 6px 0px 6px;'>Форматы обучения</a> |
                <a href='' style='margin: 0px 6px 0px 6px;'>Программа</a> |
                <a href='' style='margin: 0px 6px 0px 6px;'>Мастерские</a> |
                <a href='' style='margin: 0px 6px 0px 6px;'>Как попасть</a> |
                <a href='' style='margin: 0px 6px 0px 6px;'>Контакты</a>
                <a href='/' style='margin: 0px 6px 0px 6px;'>Обновить</a>
            </div>
            <div class='container' style='font-size:small;margin-right: 14px;'>
                <span> +7 (383) 330-80-51 +7 (913) 713-27-64</span>
                <a href='#'> <img src='logotelegram.png' height='30'> </a>
                <a href='#'> <img src='logoVK.png' height='30'> </a>
            </div>
        </div>

        <div style='display:flex;flex-direction:column;margin-left: 40px;'>
            <h1 style='margin-top: 40px;'>Окна в мир ЛШЮП, сезон 2025 года</h1>
            <div style='font-size: large; margin-top: 20px;'>Окна созданы мастерской №3 'Веб и сервисы'</div>
        </div>

        <div style='' class='container_w'>
            <div class='article' style=''>
                <div class='box' style=''>
                    <div class='rubrika' style=''>Рубрика информация об источнике, идентификатор статьи</div>
                    <h3 class='art-heading' style=''>Это заголовок статьи</h3>{vk}
                    <img src='NSU-building.jpg' style='width:500px;' />
                </div>
            </div>
            <div class='article'>
                <div class='box'>
                    <div class='rubrika'>Рубрика информация об источнике, идентификатор статьи</div>
                    <h3 class='art-heading'>Обучение</h3>
                    {tg}
                </div>
            </div>
            <div class='article' style=''>
                <div class='box' style=''>
                    <div class='rubrika' style='width:100%; background-color:lime; font-size:large; color:white; '>Рубрика информация об источнике, идентификатор статьи</div>
                    <h3 class='art-heading'>Отборочный этап</h3>
                    Для участия в ЛШЮП необходимо будет заполнить форму для регистрации и пройти очное собеседование.
                    Для иногородних участников возможно прохождение собеседования онлайн.
                    <img src='pexels.jpg' style='width:500px;' />
                </div>
            </div>
            <div class='article' style=''>
                <div class='box' style=''>
                    <div class='rubrika' style='width:100%; background-color:lime; font-size:large; color:white; '>Рубрика информация об источнике, идентификатор статьи</div>
                    <h3 class='art-heading'>Обучение</h3>
                    Во время летней школы учащихся ждут: работа в мастерских, мастер-классы от практикующих специалистов,
                    а также лекции, квизы и соревнования.
                    Также каждая мастерская в конце летней школы презентует написанный проект.
                    <img src='diggity.jpg' style='width:500px;' />
                </div>
            </div>
        </div>

        

    </body>
</html>
               ";
    }
}