

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
        <link rel='stylesheet' href='css/index.css'>
        <script src='script.js'></script>

    </head>
    <body>

        <div class='container' style='width:100%;height: 60px; background-color: #ffebb6;justify-content: space-between;'>
            <a href='#'> <img src='datasets/image/logossyp.png' style='height:50px;margin-left: 16px;margin-top: 5px;'> </a>
            <input type='checkbox' id='theme' name='theme' checked style='margin-left: 1700px;'/>
            <label for='theme'>Темная тема</label>

            <div class='container'>
                <a href='' style='margin: 0px 6px 0px 6px;'>Форматы обучения</a> |
                <a href='' style='margin: 0px 6px 0px 6px;'>Программа</a> |
                <a href='' style='margin: 0px 6px 0px 6px;'>Мастерские</a> |
                <a href='' style='margin: 0px 6px 0px 6px;'>Как попасть</a> |
                <a href='' style='margin: 0px 6px 0px 6px;'>Контакты</a>
            </div>
            <div class='container' style='font-size:small;margin-right: 14px;'>
                <span> +7 (383) 330-80-51 +7 (913) 713-27-64</span>
                <a href='#'> <img src='/datasets/image/logotelegram.png' height='30'></a>
                <a href='#'> <img src='datasets/image/logoVK.png' height='30'> </a>
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
                    <h3 class='art-heading' style=''>Как будет проходить работа?</h3>
                    Работа в мастерских будет проходить на базе Новосибирского государственного университета в очном режиме.
                    Участники будут около 10 утра приезжать по адресу Пирогова, 1.
                    Занятия идут приблизительно до 18:00 с перерывами на обед и полдник в столовой НГУ. Далее идет рисунок, рекомендуемая ширина 500 px.
                    <img src='datasets/image/ngy.png' style='width:500px;' />
                </div>
            </div>
            <div class='article'>
                <div class='box'>
                    <div class='rubrika'>Рубрика информация об источнике, идентификатор статьи</div>
                    <h3 class='art-heading'>Используемое ПО</h3>
                    Обучение проходит за компьютерами оснащенными
                    : VisualStudioCode2019, WebStorm, VisualStudioCode. Используемые языки(кликабельно):
                    <a href='https://learn.microsoft.com/ru-ru/dotnet/csharp/'><img src='datasets/image/logocsharp.png' height='40'/></a>
                    <a href='https://developer.mozilla.org/ru/docs/Web/HTML'><img src='datasets/image/logohtml.png' height='40'/></a>
                    <a href='https://developer.mozilla.org/ru/docs/Web/CSS'><img src='datasets/image/logocss.png' height='40'/></a>
                    <a href='https://developer.mozilla.org/ru/docs/Web/JavaScript'><img src='datasets/image/logojs.png' height='40'/></a>
                    <img src='datasets/image/po.png' style='width:500px;' />

                </div>
            </div>
            <div class='article' style=''>
                <div class='box' style=''>
                    <div class='rubrika' style='width:100%; background-color:#ffebb6; font-size:large; color:black; '>Рубрика информация об источнике, идентификатор статьи</div>
                    <h3 class='art-heading'>Отборочный этап</h3>
                    {vk}
                    <img src='datasets/image/classroom.png' style='width:500px;' />
                </div>
            </div>
            <div class='article' style=''>
                <div class='box' style=''>
                    <div class='rubrika' style='width:100%; background-color:#ffebb6; font-size:large; color:white; color: black; '>Рубрика информация об источнике, идентификатор статьи</div>
                    <h3 class='art-heading'>Обучение</h3>
                    Во время летней школы учащихся ждут: работа в мастерских, мастер-классы от практикующих специалистов,
                    а также лекции, квизы и соревнования.
                    Также каждая мастерская в конце летней школы презентует написанный проект.
                    <img src='datasets/image/eductaion.png' style='width:500px;' />
                </div>
            </div>
        </div>

        

    </body>
</html>

               ";
    }
}